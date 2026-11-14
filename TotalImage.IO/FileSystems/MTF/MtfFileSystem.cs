using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TotalImage.FileSystems.MTF;

/// <summary>
/// A read-only view of uncompressed, single-medium Microsoft Tape Format backups.
/// Data sets and source volumes are kept in separate synthetic directories.
/// </summary>
public sealed class MtfFileSystem : FileSystem
{
    /// <summary>The synthetic root containing separate backup-set directories.</summary>
    private readonly MtfDirectory _root;
    /// <summary>The tape name, or a fallback label when the tape name is empty.</summary>
    private string _label = "NT Backup";
    /// <summary>The format logical block size recorded in the TAPE descriptor.</summary>
    private int _blockSize;

    /// <inheritdoc />
    public override string DisplayName => "NT Backup (Microsoft Tape Format)";
    /// <inheritdoc />
    /// <exception cref="NotSupportedException">The volume label cannot be changed in a read-only archive.</exception>
    public override string VolumeLabel
    {
        get
        {
            return _label;
        }
        set
        {
            throw new NotSupportedException("NT Backup archives are read-only.");
        }
    }

    /// <inheritdoc />
    public override Directory RootDirectory => _root;
    /// <inheritdoc />
    public override long TotalFreeSpace => 0;
    /// <inheritdoc />
    public override long TotalSize => _stream.Length;
    /// <inheritdoc />
    public override long AllocationUnitSize => _blockSize;
    /// <inheritdoc />
    public override bool SupportsSubdirectories => true;
    /// <inheritdoc />
    public override bool IsReadOnly => true;

    /// <summary>
    /// Reads the backup index, retaining only metadata and data-stream offsets.
    /// </summary>
    /// <param name="stream">The readable, seekable logical MTF stream, without tape-record framing.</param>
    /// <remarks>
    /// This filesystem does not own the stream. Its owner must keep the stream open while file views are in use.
    /// File payloads are read on demand rather than copied into the index.
    /// </remarks>
    /// <exception cref="ArgumentException">The stream is not readable and seekable.</exception>
    /// <exception cref="InvalidDataException">The archive is malformed, corrupt, incomplete, or contains unsafe paths.</exception>
    /// <exception cref="NotSupportedException">The archive uses an unsupported format feature.</exception>
    public MtfFileSystem(Stream stream) : base(stream)
    {
        if (!stream.CanRead || !stream.CanSeek)
        {
            throw new ArgumentException("A readable, seekable MTF stream is required.", nameof(stream));
        }
        _root = new MtfDirectory(this, null, "");
        ReadIndex();
    }

    /// <summary>
    /// Walks descriptor blocks and their streams to build the directory tree and standard-data index.
    /// </summary>
    /// <exception cref="InvalidDataException">The backup structure or its metadata is invalid or incomplete.</exception>
    /// <exception cref="NotSupportedException">The backup requires an unsupported feature.</exception>
    private void ReadIndex()
    {
        long position = 0;
        MtfDirectory? set = null, volume = null;
        var directories = new Dictionary<uint, MtfDirectory>();
        var sourceDirectoryNames = new Dictionary<MtfDirectory, string>();
        bool setOpen = false;
        int setCount = 0, volumeCount = 0;
        while (position < _stream.Length)
        {
            byte[] common = ReadAt(position, 52);
            VerifyChecksum(common, "descriptor");
            string type = Encoding.ASCII.GetString(common, 0, 4);
            if ((U32(common, 4) & 1) != 0)
            {
                throw new NotSupportedException("Multi-tape NT Backup continuations are not supported.");
            }
            int streamOffset = U16(common, 8);
            if (streamOffset < 52)
            {
                throw new InvalidDataException("Invalid MTF stream offset.");
            }
            byte[] block = ReadAt(position, streamOffset);
            MtfEntry? file = null;
            switch (type)
            {
                case "TAPE":
                    Require(block, 94);
                    if (position != 0)
                    {
                        throw new InvalidDataException("Unexpected MTF tape header.");
                    }
                    _blockSize = U16(block, 84);
                    if (_blockSize < 512 || _blockSize % 512 != 0 || block[93] != 1)
                    {
                        throw new NotSupportedException("Unsupported MTF version or logical block size.");
                    }
                    if (U16(block, 60) != 1)
                    {
                        throw new NotSupportedException("This backup starts on another tape; multi-tape backups are not supported.");
                    }
                    _label = ReadString(block, 68);
                    if (_label.Length == 0)
                    {
                        _label = "NT Backup";
                    }
                    break;
                case "SSET":
                    RequireTape();
                    Require(block, 98);
                    if (setOpen)
                    {
                        throw new InvalidDataException("An MTF data set ended without an ESET descriptor.");
                    }
                    if (U16(block, 56) != 0 || U16(block, 58) != 0)
                    {
                        throw new NotSupportedException("Encrypted or compressed NT Backup data sets are not supported.");
                    }
                    set = AddDirectory(_root, $"Backup set {++setCount}");
                    set.Entry.CreationTime = ReadDate(block.AsSpan(88, 5));
                    setOpen = true;
                    volume = null;
                    volumeCount = 0;
                    directories.Clear();
                    break;
                case "VOLB":
                    Require(block, 73);
                    if (!setOpen || set == null)
                    {
                        throw new InvalidDataException("An MTF volume is outside a data set.");
                    }
                    // The device is a source label, never a host path (e.g. C:).
                    string device = ReadString(block, 56).TrimEnd(':', '\\', '/');
                    string volumeName = $"Volume {++volumeCount}";
                    if (device.Length != 0 && IsSafeComponent(device))
                    {
                        volumeName += $" ({device})";
                    }
                    volume = AddDirectory(set, volumeName);
                    directories.Clear();
                    break;
                case "DIRB":
                    Require(block, 84);
                    if (!setOpen || volume == null)
                    {
                        throw new InvalidDataException("An MTF directory has no source volume.");
                    }
                    CheckObjectAttributes(block);
                    var directory = volume;
                    foreach (string part in ReadString(block, 80).Split(['\0', '\\', '/'], StringSplitOptions.RemoveEmptyEntries))
                    {
                        // System State paths can embed source-drive labels, e.g. Boot Files\0C:\0WINNT.
                        // Expose them as relative directory names, never host drive-qualified paths.
                        string component = part;
                        if (part.Length == 2 && char.IsAsciiLetter(part[0]) && part[1] == ':')
                        {
                            component = $"Drive {char.ToUpperInvariant(part[0])}";
                        }
                        ValidateComponent(component);
                        var existing = directory.Children.FirstOrDefault(x =>
                        {
                            return x.Name.Equals(component, StringComparison.OrdinalIgnoreCase);
                        });
                        if (existing == null)
                        {
                            directory = AddDirectory(directory, component);
                            sourceDirectoryNames.Add(directory, part);
                        }
                        else if (existing is MtfDirectory child)
                        {
                            if (!sourceDirectoryNames[child].Equals(part, StringComparison.OrdinalIgnoreCase))
                            {
                                throw new InvalidDataException("Conflicting MTF source directory names.");
                            }
                            directory = child;
                        }
                        else
                        {
                            throw new InvalidDataException("An MTF path conflicts with a file.");
                        }
                    }
                    ReadMetadata(block, directory.Entry);
                    uint directoryId = U32(block, 76);
                    if (directories.TryGetValue(directoryId, out var previous) && previous != directory)
                    {
                        throw new InvalidDataException("Conflicting MTF directory identifiers.");
                    }
                    directories[directoryId] = directory;
                    break;
                case "FILE":
                    Require(block, 88);
                    if (!setOpen || !directories.TryGetValue(U32(block, 76), out var parent))
                    {
                        throw new InvalidDataException("An MTF file references an unknown directory.");
                    }
                    CheckObjectAttributes(block);
                    string name = ReadString(block, 84);
                    ValidateComponent(name);
                    string displayName = name;
                    int copy = 1;
                    // System State can record the same path more than once; retain every copy without overwriting.
                    while (parent.Children.Any(x =>
                    {
                        return x.Name.Equals(displayName, StringComparison.OrdinalIgnoreCase);
                    }))
                    {
                        displayName = $"{Path.GetFileNameWithoutExtension(name)} ({++copy}){Path.GetExtension(name)}";
                    }
                    file = new MtfEntry(displayName);
                    ReadMetadata(block, file, isFile: true);
                    parent.Children.Add(new MtfFile(this, parent, file));
                    break;
                case "ESET":
                    RequireTape();
                    setOpen = false; // A second ESET may terminate the media-based catalog.
                    break;
                case "EOTM":
                    RequireTape();
                    if (setOpen)
                    {
                        throw new NotSupportedException("The NT Backup data set continues on another tape.");
                    }
                    break;
                case "SFMB":
                case "ESPB":
                    RequireTape();
                    break;
                case "CFIL":
                    throw new InvalidDataException("The NT Backup archive contains a corrupt-file descriptor.");
                default:
                    throw new NotSupportedException($"Unsupported MTF descriptor '{type}'.");
            }
            RequireTape();
            long next = ReadStreams(position + streamOffset, file);
            if (file != null && (ulong)file.Length != U64(block, 12))
            {
                throw new InvalidDataException($"The data for '{file.Name}' is missing or has an inconsistent length.");
            }
            if (next <= position || next % _blockSize != 0)
            {
                throw new InvalidDataException("Misaligned MTF descriptor boundary.");
            }
            position = next;
        }
        if (_blockSize == 0 || setCount == 0 || setOpen)
        {
            throw new InvalidDataException("The NT Backup archive is incomplete (missing tape, data-set, or end-of-set descriptor).");
        }
    }

    /// <summary>
    /// Validates and skips an object's streams, recording its standard file-data range when present.
    /// </summary>
    /// <param name="position">The logical byte offset immediately after the object's metadata, or of the next descriptor.</param>
    /// <param name="file">The file metadata to populate, or null for a descriptor without file content.</param>
    /// <returns>The logical offset following the streams, at the next descriptor boundary or end of archive.</returns>
    /// <exception cref="InvalidDataException">A stream is corrupt, truncated, or incorrectly aligned.</exception>
    /// <exception cref="NotSupportedException">A stream uses an unsupported encoding or storage feature.</exception>
    private long ReadStreams(long position, MtfEntry? file)
    {
        // Windows 2000 can record an unaligned metadata length (e.g. an 85-byte ESET).
        // The first stream header, like subsequent stream headers, starts on a four-byte boundary.
        position = checked((position + 3) & ~3L);
        if (position > _stream.Length)
        {
            throw new InvalidDataException("Truncated MTF stream alignment padding.");
        }
        bool hasData = false;
        while (position < _stream.Length)
        {
            // Some descriptors have no streams; the next descriptor begins at the FLB boundary.
            if (position % _blockSize == 0 && IsDescriptor(position))
            {
                return position;
            }
            byte[] header = ReadAt(position, 22);
            VerifyChecksum(header, "stream");
            string type = Encoding.ASCII.GetString(header, 0, 4);
            ushort mediaFlags = U16(header, 6);
            if ((mediaFlags & 0x5f) != 0 || U16(header, 16) != 0 || U16(header, 18) != 0)
            {
                throw new NotSupportedException("Compressed, encrypted, variable-length or continued MTF streams are not supported.");
            }
            if ((U16(header, 4) & 8) != 0 || type is "SPAR" or "LINK" or "NTRP")
            {
                throw new NotSupportedException("Sparse files and link/reparse streams in NT Backup archives are not supported.");
            }
            if (type == "CRPT")
            {
                throw new InvalidDataException("The NT Backup archive contains corrupt file data.");
            }
            ulong length = U64(header, 8);
            long data = checked(position + 22);
            if (length > (ulong)(_stream.Length - data))
            {
                throw new InvalidDataException("Truncated MTF stream.");
            }
            long end = data + (long)length;
            if (type == "STAN" && file != null)
            {
                if (hasData)
                {
                    throw new NotSupportedException("Multiple standard data streams for one MTF file are not supported.");
                }
                hasData = true;
                file.Extents.Add(new StreamExtent(data, (long)length));
                file.Length = (long)length;
            }
            if (type is "FNAM" or "PNAM")
            {
                throw new NotSupportedException("MTF names stored in streams are not supported.");
            }
            // Security, alternate data streams and tape catalogs are skipped by length, not scanned for tags.
            position = checked((end + 3) & ~3L);
            if (position > _stream.Length)
            {
                throw new InvalidDataException("Truncated MTF stream alignment padding.");
            }
            if (type == "SPAD")
            {
                return position;
            }
        }
        return position;
    }

    /// <summary>
    /// Probes for a known descriptor tag; the main index reader validates its header afterward.
    /// </summary>
    /// <param name="position">The logical byte offset to probe.</param>
    /// <returns>True when a complete common header with a recognized tag is available.</returns>
    private bool IsDescriptor(long position)
    {
        if (_stream.Length - position < 52)
        {
            return false;
        }
        var header = ReadAt(position, 52);
        string type = Encoding.ASCII.GetString(header, 0, 4);
        return type is "TAPE" or "SSET" or "VOLB" or "DIRB" or "FILE" or "ESET" or "EOTM" or "SFMB" or "ESPB" or "CFIL";
    }

    /// <summary>Ensures the archive's leading tape descriptor has established the logical block size.</summary>
    /// <exception cref="InvalidDataException">No valid leading TAPE descriptor has been read.</exception>
    private void RequireTape()
    {
        if (_blockSize == 0)
        {
            throw new InvalidDataException("The MTF stream does not begin with a TAPE descriptor.");
        }
    }

    /// <summary>Creates a directory and attaches it to its parent in the indexed tree.</summary>
    /// <param name="parent">The directory that will contain the new entry.</param>
    /// <param name="name">The validated entry name or generated grouping name.</param>
    /// <returns>The directory added to the parent.</returns>
    private MtfDirectory AddDirectory(MtfDirectory parent, string name)
    {
        var directory = new MtfDirectory(this, parent, name);
        parent.Children.Add(directory);
        return directory;
    }

    /// <summary>Rejects corrupt objects and object names stored in unsupported name streams.</summary>
    /// <param name="block">The directory or file descriptor containing object attributes.</param>
    /// <exception cref="InvalidDataException">The object is marked corrupt.</exception>
    /// <exception cref="NotSupportedException">The object's name is stored in a stream.</exception>
    private static void CheckObjectAttributes(byte[] block)
    {
        uint attributes = U32(block, 52);
        if ((attributes & 0x40000) != 0)
        {
            throw new InvalidDataException("The NT Backup archive contains a corrupt object.");
        }
        if ((attributes & 0x20000) != 0)
        {
            throw new NotSupportedException("MTF names stored in streams are not supported.");
        }
    }

    /// <summary>Reads recorded timestamps and attributes, preferring NT-specific attributes when available.</summary>
    /// <param name="block">The validated file or directory descriptor and its metadata area.</param>
    /// <param name="entry">The indexed entry to populate.</param>
    /// <param name="isFile">Whether to interpret NT file-specific flags in addition to shared attributes.</param>
    /// <exception cref="InvalidDataException">The OS-specific metadata range is outside the descriptor.</exception>
    /// <exception cref="NotSupportedException">The object is an unsupported hard link or reparse point.</exception>
    private static void ReadMetadata(byte[] block, MtfEntry entry, bool isFile = false)
    {
        uint attributes = U32(block, 52);
        entry.Attributes = (FileAttributes)((attributes >> 8) & 7);
        if ((attributes & 0x800) != 0)
        {
            entry.Attributes |= FileAttributes.Archive;
        }
        int osSize = U16(block, 44), osOffset = U16(block, 46);
        if (osSize != 0)
        {
            if (osOffset < 52 || osOffset > block.Length - osSize)
            {
                throw new InvalidDataException("Invalid MTF operating-system metadata offset.");
            }
            if (block[10] == 14 && osSize >= 4)
            {
                entry.Attributes = (FileAttributes)U32(block, osOffset);
                if (isFile && block[11] == 1 && osSize >= 12 && (U32(block, osOffset + 8) & 1) != 0)
                {
                    throw new NotSupportedException("NT Backup hard links are not supported.");
                }
            }
        }
        if ((entry.Attributes & FileAttributes.ReparsePoint) != 0)
        {
            throw new NotSupportedException("NT Backup reparse points are not supported.");
        }
        entry.LastWriteTime = ReadDate(block.AsSpan(56, 5));
        entry.CreationTime = ReadDate(block.AsSpan(61, 5));
        entry.LastAccessTime = ReadDate(block.AsSpan(71, 5));
    }

    /// <summary>Decodes a five-byte, bit-packed MTF civil timestamp without assigning a time zone.</summary>
    /// <param name="b">The five packed date/time bytes.</param>
    /// <returns>The recorded timestamp with unspecified kind, or null for an absent or invalid date.</returns>
    /// <remarks>Validates components before constructing a date so missing or invalid values do not throw exceptions.</remarks>
    private static DateTime? ReadDate(ReadOnlySpan<byte> b)
    {
        int year = (b[0] << 6) | (b[1] >> 2);
        int month = ((b[1] & 3) << 2) | (b[2] >> 6);
        int day = (b[2] & 62) >> 1;
        int hour = ((b[2] & 1) << 4) | (b[3] >> 4);
        int minute = ((b[3] & 15) << 2) | (b[4] >> 6);
        int second = b[4] & 63;
        if (year is < 1 or > 9999 || month is < 1 or > 12 || day < 1
            || day > DateTime.DaysInMonth(year, month) || hour > 23 || minute > 59 || second > 59)
        {
            return null;
        }
        return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Unspecified);
    }

    /// <summary>Decodes an MTF size/offset string field and removes trailing null terminators.</summary>
    /// <param name="block">The descriptor containing the field and its string storage.</param>
    /// <param name="field">The offset of the four-byte size/offset field within the descriptor.</param>
    /// <returns>The decoded string, preserving embedded null separators in directory paths.</returns>
    /// <exception cref="InvalidDataException">The string range is outside the descriptor metadata.</exception>
    /// <exception cref="NotSupportedException">The string type or encoded length is unsupported.</exception>
    /// <exception cref="DecoderFallbackException">A Unicode string contains an invalid UTF-16 sequence.</exception>
    private static string ReadString(byte[] block, int field)
    {
        int length = U16(block, field), offset = U16(block, field + 2);
        if (length == 0)
        {
            return "";
        }
        if (offset < 52 || offset > block.Length - length)
        {
            throw new InvalidDataException("Invalid MTF string offset.");
        }
        string text = block[48] switch
        {
            1 => Encoding.Latin1.GetString(block, offset, length),
            2 when length % 2 == 0 => new UnicodeEncoding(false, false, true).GetString(block, offset, length),
            _ => throw new NotSupportedException("Unsupported MTF string encoding.")
        };
        return text.TrimEnd('\0');
    }

    /// <summary>Checks that an archive name is a single Windows-compatible, non-device path component.</summary>
    /// <param name="name">The name to validate.</param>
    /// <returns>True when the name passes the component safety checks; otherwise, false.</returns>
    private static bool IsSafeComponent(string name)
    {
        return name.Length > 0 && name is not "." and not ".." && name.TrimEnd(' ', '.') == name
            && !name.Any(c => c < 32 || "<>:\"/\\|?*".Contains(c))
            && !IsReservedName(name);
    }

    /// <summary>Recognizes DOS device names, including names with filename extensions.</summary>
    /// <param name="name">The entry name to check.</param>
    /// <returns>True for a reserved device name recognized by this reader; otherwise, false.</returns>
    private static bool IsReservedName(string name)
    {
        string stem = name.Split('.')[0];
        return stem.Equals("CON", StringComparison.OrdinalIgnoreCase) || stem.Equals("PRN", StringComparison.OrdinalIgnoreCase)
            || stem.Equals("AUX", StringComparison.OrdinalIgnoreCase) || stem.Equals("NUL", StringComparison.OrdinalIgnoreCase)
            || (stem.Length == 4 && (stem.StartsWith("COM", StringComparison.OrdinalIgnoreCase)
                || stem.StartsWith("LPT", StringComparison.OrdinalIgnoreCase)) && stem[3] is >= '1' and <= '9');
    }

    /// <summary>Rejects names that cannot safely be used as individual extraction path components.</summary>
    /// <param name="name">The archive entry name to validate.</param>
    /// <exception cref="InvalidDataException">The name is unsafe or unsupported on the host filesystem.</exception>
    private static void ValidateComponent(string name)
    {
        if (!IsSafeComponent(name))
        {
            throw new InvalidDataException("An NT Backup entry contains an unsafe or unsupported file name.");
        }
    }

    /// <summary>Reads an exact, bounded region of the logical archive into a metadata buffer.</summary>
    /// <param name="offset">The logical byte offset of the region.</param>
    /// <param name="count">The number of metadata bytes to read.</param>
    /// <returns>The requested metadata bytes.</returns>
    /// <exception cref="InvalidDataException">The requested region is outside the archive.</exception>
    private byte[] ReadAt(long offset, int count)
    {
        if (offset < 0 || offset > _stream.Length - count)
        {
            throw new InvalidDataException("Truncated MTF descriptor or stream header.");
        }
        byte[] buffer = new byte[count];
        _stream.Position = offset;
        _stream.ReadExactly(buffer);
        return buffer;
    }

    /// <summary>Validates an even-sized MTF header using its word-wise XOR checksum.</summary>
    /// <param name="bytes">The complete header, including its final two-byte checksum field.</param>
    /// <param name="kind">The header description used in validation errors.</param>
    /// <exception cref="InvalidDataException">The header checksum does not match.</exception>
    private static void VerifyChecksum(ReadOnlySpan<byte> bytes, string kind)
    {
        ushort checksum = 0;
        for (int i = 0; i < bytes.Length; i += 2)
        {
            checksum ^= BinaryPrimitives.ReadUInt16LittleEndian(bytes[i..]);
        }
        if (checksum != 0)
        {
            throw new InvalidDataException($"Invalid MTF {kind} header checksum.");
        }
    }

    /// <summary>Ensures a descriptor contains the fixed fields needed by its parser.</summary>
    /// <param name="block">The descriptor metadata buffer.</param>
    /// <param name="length">The minimum required length in bytes.</param>
    /// <exception cref="InvalidDataException">The descriptor is too short.</exception>
    private static void Require(byte[] block, int length)
    {
        if (block.Length < length)
        {
            throw new InvalidDataException("Truncated MTF descriptor fields.");
        }
    }

    /// <summary>Reads a little-endian 16-bit unsigned descriptor field.</summary>
    /// <param name="b">The metadata buffer containing the field.</param>
    /// <param name="offset">The byte offset of the field.</param>
    /// <returns>The decoded field value.</returns>
    private static ushort U16(byte[] b, int offset)
    {
        return BinaryPrimitives.ReadUInt16LittleEndian(b.AsSpan(offset));
    }

    /// <summary>Reads a little-endian 32-bit unsigned descriptor field.</summary>
    /// <param name="b">The metadata buffer containing the field.</param>
    /// <param name="offset">The byte offset of the field.</param>
    /// <returns>The decoded field value.</returns>
    private static uint U32(byte[] b, int offset)
    {
        return BinaryPrimitives.ReadUInt32LittleEndian(b.AsSpan(offset));
    }

    /// <summary>Reads a little-endian 64-bit unsigned descriptor field.</summary>
    /// <param name="b">The metadata buffer containing the field.</param>
    /// <param name="offset">The byte offset of the field.</param>
    /// <returns>The decoded field value.</returns>
    private static ulong U64(byte[] b, int offset)
    {
        return BinaryPrimitives.ReadUInt64LittleEndian(b.AsSpan(offset));
    }
}
