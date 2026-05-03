using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace TotalImage.FileSystems.NTFS;

/// <summary>
/// Represents an NTFS volume.
/// </summary>
public class NtfsFileSystem : FileSystem
{
    private const uint StandardInformationAttributeType = 0x10;
    private const uint FileNameAttributeType = 0x30;
    private const uint VolumeNameAttributeType = 0x60;
    private const uint DataAttributeType = 0x80;
    private const uint IndexRootAttributeType = 0x90;
    private const uint IndexAllocationAttributeType = 0xA0;
    private const ulong RootDirectoryRecordNumber = 5;
    private const ulong BitmapRecordNumber = 6;
    private const ulong VolumeRecordNumber = 3;
    private const ulong FirstMetadataFileRecordNumber = 16;

    private readonly Dictionary<ulong, NtfsFileRecord> _recordCache = new();
    private readonly NtfsBootSector _bootSector;
    private readonly NtfsFileRecord _mftRecord;
    private readonly NtfsAttributeRecord _mftDataAttribute;
    private long? _freeSpace;
    private string? _volumeLabel;

    /// <summary>
    /// Creates an NTFS file system from a stream.
    /// </summary>
    /// <param name="stream">The stream containing the NTFS volume.</param>
    public NtfsFileSystem(Stream stream) : base(stream)
    {
        byte[] bootSectorBytes = new byte[512];
        stream.Position = 0;
        stream.ReadExactly(bootSectorBytes);

        _bootSector = NtfsBootSector.Parse(bootSectorBytes);
        _mftRecord = ParseFileRecord(ReadFileRecordBytesFromVolume(_bootSector.MftLogicalClusterNumber, 0), 0);
        _recordCache[0] = _mftRecord;

        _mftDataAttribute = _mftRecord.Attributes
            .Single(attribute => attribute.Type == DataAttributeType && attribute.Name == null);
    }

    /// <inheritdoc />
    public override string DisplayName => "NTFS";

    /// <inheritdoc />
    public override string VolumeLabel
    {
        get
        {
            if (_volumeLabel != null)
            {
                return _volumeLabel;
            }

            NtfsFileRecord volumeRecord = LoadFileRecord(VolumeRecordNumber);
            NtfsResidentAttributeRecord? attribute = volumeRecord.Attributes
                .OfType<NtfsResidentAttributeRecord>()
                .SingleOrDefault(candidate => candidate.Type == VolumeNameAttributeType);

            _volumeLabel = attribute == null
                ? string.Empty
                : Encoding.Unicode.GetString(attribute.Value).TrimEnd('\0');

            return _volumeLabel;
        }
        set => throw new NotSupportedException("NTFS is currently exposed as read-only.");
    }

    /// <inheritdoc />
    public override Directory RootDirectory => new NtfsDirectory(this, LoadFileRecord(RootDirectoryRecordNumber), null, null);

    /// <inheritdoc />
    public override long TotalFreeSpace
    {
        get
        {
            if (_freeSpace.HasValue)
            {
                return _freeSpace.Value;
            }

            NtfsFileRecord bitmapRecord = LoadFileRecord(BitmapRecordNumber);
            using Stream bitmapStream = OpenDataStream(bitmapRecord);
            byte[] bitmap = new byte[bitmapStream.Length];
            bitmapStream.ReadExactly(bitmap);

            long freeClusters = 0;
            long clusterCount = _bootSector.TotalSectors / _bootSector.SectorsPerCluster;

            for (long cluster = 0; cluster < clusterCount; cluster++)
            {
                int byteIndex = checked((int)(cluster / 8));
                int bitIndex = checked((int)(cluster % 8));
                if (byteIndex >= bitmap.Length || (bitmap[byteIndex] & (1 << bitIndex)) == 0)
                {
                    freeClusters++;
                }
            }

            _freeSpace = freeClusters * _bootSector.BytesPerCluster;
            return _freeSpace.Value;
        }
    }

    /// <inheritdoc />
    public override long TotalSize => _bootSector.TotalSectors * _bootSector.BytesPerSector;

    /// <inheritdoc />
    public override long AllocationUnitSize => _bootSector.BytesPerCluster;

    /// <inheritdoc />
    public override bool SupportsSubdirectories => true;

    /// <inheritdoc />
    public override bool IsReadOnly => true;

    internal IEnumerable<(NtfsFileRecord Record, NtfsFileNameRecord FileName)> EnumerateDirectoryEntries(NtfsFileRecord directoryRecord)
    {
        NtfsResidentAttributeRecord? indexRoot = directoryRecord.Attributes
            .OfType<NtfsResidentAttributeRecord>()
            .SingleOrDefault(attribute => attribute.Type == IndexRootAttributeType);

        if (indexRoot == null)
        {
            yield break;
        }

        byte[] value = indexRoot.Value;
        if (value.Length < 0x20)
        {
            yield break;
        }

        int bytesPerIndexRecord = checked((int)BinaryPrimitives.ReadUInt32LittleEndian(value.AsSpan(8, 4)));
        NtfsNonResidentAttributeRecord? indexAllocation = directoryRecord.Attributes
            .OfType<NtfsNonResidentAttributeRecord>()
            .SingleOrDefault(attribute =>
                attribute.Type == IndexAllocationAttributeType &&
                attribute.Name == indexRoot.Name);

        if (indexAllocation == null)
        {
            indexAllocation = directoryRecord.Attributes
                .OfType<NtfsNonResidentAttributeRecord>()
                .SingleOrDefault(attribute => attribute.Type == IndexAllocationAttributeType);
        }

        HashSet<(ulong FileReference, string Name)> seenEntries = new();
        foreach ((ulong fileReference, NtfsFileNameRecord fileName) in EnumerateIndexEntries(value, 0x10, bytesPerIndexRecord, indexAllocation, []))
        {
            if ((fileName.Name == ".") || (fileName.Name == "..") || (fileName.Namespace == NtfsFileNameNamespace.Dos))
            {
                continue;
            }

            if (seenEntries.Add((fileReference, fileName.Name)))
            {
                yield return (LoadFileRecord(fileReference), fileName);
            }
        }

        if (indexAllocation != null)
        {
            foreach (byte[] indexBuffer in EnumerateIndexBuffers(indexAllocation, bytesPerIndexRecord))
            {
                foreach ((ulong fileReference, NtfsFileNameRecord fileName) in EnumerateIndexEntries(indexBuffer, 0x18, bytesPerIndexRecord, null, []))
                {
                    if ((fileName.Name == ".") || (fileName.Name == "..") || (fileName.Namespace == NtfsFileNameNamespace.Dos))
                    {
                        continue;
                    }

                    if (seenEntries.Add((fileReference, fileName.Name)))
                    {
                        yield return (LoadFileRecord(fileReference), fileName);
                    }
                }
            }
        }
    }

    private IEnumerable<(ulong FileReference, NtfsFileNameRecord FileName)> EnumerateIndexEntries(
        byte[] buffer,
        int indexHeaderOffset,
        int bytesPerIndexRecord,
        NtfsNonResidentAttributeRecord? indexAllocation,
        HashSet<long> visitedVcns)
    {
        int entryOffset = indexHeaderOffset + checked((int)BinaryPrimitives.ReadUInt32LittleEndian(buffer.AsSpan(indexHeaderOffset, 4)));
        int totalLength = checked((int)BinaryPrimitives.ReadUInt32LittleEndian(buffer.AsSpan(indexHeaderOffset + 4, 4)));
        int entriesEnd = Math.Min(buffer.Length, indexHeaderOffset + totalLength);

        while ((entryOffset + 0x10) <= buffer.Length && entryOffset < entriesEnd)
        {
            ushort length = BinaryPrimitives.ReadUInt16LittleEndian(buffer.AsSpan(entryOffset + 8, 2));
            ushort keyLength = BinaryPrimitives.ReadUInt16LittleEndian(buffer.AsSpan(entryOffset + 10, 2));
            ushort flags = BinaryPrimitives.ReadUInt16LittleEndian(buffer.AsSpan(entryOffset + 12, 2));

            if (length < 0x10 || entryOffset + length > buffer.Length)
            {
                yield break;
            }

            if ((flags & 0x01) != 0 && indexAllocation != null)
            {
                long childVcn = BinaryPrimitives.ReadInt64LittleEndian(buffer.AsSpan(entryOffset + length - 8, 8));
                if (visitedVcns.Add(childVcn))
                {
                    byte[] childBuffer = ReadIndexBuffer(indexAllocation, childVcn, bytesPerIndexRecord);
                    foreach ((ulong fileReference, NtfsFileNameRecord fileName) in EnumerateIndexEntries(childBuffer, 0x18, bytesPerIndexRecord, indexAllocation, visitedVcns))
                    {
                        yield return (fileReference, fileName);
                    }
                }
            }

            if (keyLength != 0)
            {
                ulong fileReference = BinaryPrimitives.ReadUInt64LittleEndian(buffer.AsSpan(entryOffset, 8)) & 0x0000FFFFFFFFFFFFUL;
                NtfsFileNameRecord fileName = NtfsFileNameRecord.Parse(buffer.AsSpan(entryOffset + 0x10, keyLength));
                yield return (fileReference, fileName);
            }

            if ((flags & 0x02) != 0)
            {
                yield break;
            }

            entryOffset += length;
        }
    }

    private byte[] ReadIndexBuffer(NtfsNonResidentAttributeRecord indexAllocation, long vcn, int bytesPerIndexRecord)
    {
        byte[] buffer = new byte[bytesPerIndexRecord];
        ReadFromAttribute(indexAllocation, checked(vcn * bytesPerIndexRecord), buffer, 0, buffer.Length);
        return PrepareIndexBuffer(buffer, vcn);
    }

    private IEnumerable<byte[]> EnumerateIndexBuffers(NtfsNonResidentAttributeRecord indexAllocation, int bytesPerIndexRecord)
    {
        long indexBufferCount = checked((long)((indexAllocation.DataSize + (ulong)bytesPerIndexRecord - 1) / (ulong)bytesPerIndexRecord));
        for (long vcn = 0; vcn < indexBufferCount; vcn++)
        {
            byte[] buffer = new byte[bytesPerIndexRecord];
            ReadFromAttribute(indexAllocation, checked(vcn * bytesPerIndexRecord), buffer, 0, buffer.Length);

            if (!buffer.AsSpan(0, 4).SequenceEqual("INDX"u8))
            {
                continue;
            }

            yield return PrepareIndexBuffer(buffer, vcn);
        }
    }

    private byte[] PrepareIndexBuffer(byte[] buffer, long vcn)
    {
        ApplyUpdateSequenceFixup(buffer, _bootSector.BytesPerSector);

        if (!buffer.AsSpan(0, 4).SequenceEqual("INDX"u8))
        {
            throw new InvalidDataException($"Invalid NTFS index record signature for VCN {vcn}.");
        }

        return buffer;
    }

    internal Stream OpenDataStream(NtfsFileRecord record)
    {
        NtfsAttributeRecord dataAttribute = record.Attributes
            .Where(attribute => attribute.Type == DataAttributeType && attribute.Name == null)
            .DefaultIfEmpty()
            .SingleOrDefault()
            ?? throw new NotSupportedException("The NTFS file does not contain an unnamed data stream.");

        if (dataAttribute is NtfsResidentAttributeRecord resident)
        {
            return new MemoryStream(resident.Value.ToArray(), writable: false);
        }

        if (dataAttribute is NtfsNonResidentAttributeRecord nonResident)
        {
            return new NtfsDataStream(_stream, _bootSector.BytesPerCluster, nonResident.Runs, nonResident.DataSize);
        }

        throw new NotSupportedException("Unsupported NTFS data attribute format.");
    }

    internal FileAttributes GetEffectiveAttributes(NtfsFileRecord record, NtfsFileNameRecord? fileName)
    {
        FileAttributes attributes = fileName?.Attributes ?? record.StandardInformation?.Attributes ?? 0;
        if (record.IsDirectory)
        {
            attributes |= FileAttributes.Directory;
        }

        // NTFS metadata files live in reserved MFT slots and should always present as hidden/system.
        if (record.RecordNumber < FirstMetadataFileRecordNumber)
        {
            attributes |= FileAttributes.System | FileAttributes.Hidden;
        }

        return attributes;
    }

    internal NtfsFileRecord LoadFileRecord(ulong recordNumber)
    {
        if (_recordCache.TryGetValue(recordNumber, out NtfsFileRecord? cached))
        {
            return cached;
        }

        ulong maxRecordCount = (ulong)(_mftDataAttribute is NtfsNonResidentAttributeRecord mftData
            ? mftData.DataSize / (ulong)_bootSector.BytesPerFileRecord
            : 0UL);
        if ((maxRecordCount != 0) && (recordNumber >= maxRecordCount))
        {
            throw new InvalidDataException($"NTFS file reference {recordNumber} is outside the loaded $MFT range ({maxRecordCount} records).");
        }

        byte[] buffer = new byte[_bootSector.BytesPerFileRecord];
        ReadFromAttribute(_mftDataAttribute, checked((long)(recordNumber * (ulong)_bootSector.BytesPerFileRecord)), buffer, 0, buffer.Length);

        NtfsFileRecord record = ParseFileRecord(buffer, recordNumber);
        _recordCache[recordNumber] = record;
        return record;
    }

    private NtfsFileRecord ParseFileRecord(byte[] bytes, ulong recordNumber)
    {
        ApplyUpdateSequenceFixup(bytes, _bootSector.BytesPerSector);

        if (!bytes.AsSpan(0, 4).SequenceEqual("FILE"u8))
        {
            throw new InvalidDataException($"Invalid NTFS file record signature for record {recordNumber}.");
        }

        ushort firstAttributeOffset = BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(0x14, 2));
        ushort flags = BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(0x16, 2));
        uint bytesInUse = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(0x18, 4));
        List<NtfsAttributeRecord> attributes = new();

        int offset = firstAttributeOffset;
        while (offset + 4 <= bytesInUse && offset + 4 <= bytes.Length)
        {
            uint type = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(offset, 4));
            if (type == 0xFFFFFFFF)
            {
                break;
            }

            uint length = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(offset + 4, 4));
            if (length < 0x18 || offset + length > bytes.Length)
            {
                throw new InvalidDataException($"Invalid NTFS attribute length in record {recordNumber}.");
            }

            ReadOnlySpan<byte> attributeBytes = bytes.AsSpan(offset, checked((int)length));
            attributes.Add(ParseAttribute(attributeBytes));
            offset += checked((int)length);
        }

        NtfsStandardInformation? standardInformation = attributes
            .OfType<NtfsResidentAttributeRecord>()
            .Where(attribute => attribute.Type == StandardInformationAttributeType)
            .Select(attribute => NtfsStandardInformation.Parse(attribute.Value))
            .SingleOrDefault();

        return new NtfsFileRecord(recordNumber, (flags & 0x01) != 0, (flags & 0x02) != 0, standardInformation, attributes.ToImmutableArray());
    }

    private NtfsAttributeRecord ParseAttribute(ReadOnlySpan<byte> bytes)
    {
        uint type = BinaryPrimitives.ReadUInt32LittleEndian(bytes.Slice(0, 4));
        bool nonResident = bytes[8] != 0;
        byte nameLength = bytes[9];
        ushort nameOffset = BinaryPrimitives.ReadUInt16LittleEndian(bytes.Slice(10, 2));
        string? name = null;

        if (nameLength != 0)
        {
            name = Encoding.Unicode.GetString(bytes.Slice(nameOffset, nameLength * 2)).TrimEnd('\0');
        }

        if (!nonResident)
        {
            uint valueLength = BinaryPrimitives.ReadUInt32LittleEndian(bytes.Slice(16, 4));
            ushort valueOffset = BinaryPrimitives.ReadUInt16LittleEndian(bytes.Slice(20, 2));
            return new NtfsResidentAttributeRecord(type, name, bytes.Slice(valueOffset, checked((int)valueLength)).ToArray());
        }

        ushort mappingPairsOffset = BinaryPrimitives.ReadUInt16LittleEndian(bytes.Slice(32, 2));
        ulong dataSize = BinaryPrimitives.ReadUInt64LittleEndian(bytes.Slice(48, 8));
        return new NtfsNonResidentAttributeRecord(type, name, dataSize, ParseDataRuns(bytes.Slice(mappingPairsOffset)));
    }

    private ImmutableArray<NtfsDataRun> ParseDataRuns(ReadOnlySpan<byte> bytes)
    {
        List<NtfsDataRun> runs = new();
        int offset = 0;
        long currentLcn = 0;

        while (offset < bytes.Length && bytes[offset] != 0)
        {
            int lengthSize = bytes[offset] & 0x0F;
            int offsetSize = bytes[offset] >> 4;
            offset++;

            long clusterCount = ReadSignedInteger(bytes.Slice(offset, lengthSize));
            offset += lengthSize;

            long lcnDelta = offsetSize == 0 ? 0 : ReadSignedInteger(bytes.Slice(offset, offsetSize));
            offset += offsetSize;

            long? logicalClusterNumber = null;
            if (offsetSize != 0)
            {
                currentLcn += lcnDelta;
                logicalClusterNumber = currentLcn;
            }

            runs.Add(new NtfsDataRun(logicalClusterNumber, clusterCount));
        }

        return runs.ToImmutableArray();
    }

    private byte[] ReadFileRecordBytesFromVolume(long mftLogicalClusterNumber, ulong recordNumber)
    {
        byte[] buffer = new byte[_bootSector.BytesPerFileRecord];
        long volumeOffset = checked((mftLogicalClusterNumber * _bootSector.BytesPerCluster) + ((long)recordNumber * _bootSector.BytesPerFileRecord));
        _stream.Position = volumeOffset;
        _stream.ReadExactly(buffer);
        return buffer;
    }

    private void ReadFromAttribute(NtfsAttributeRecord attribute, long position, byte[] buffer, int offset, int count)
    {
        switch (attribute)
        {
            case NtfsResidentAttributeRecord resident:
                resident.Value.AsSpan().Slice(checked((int)position), count).CopyTo(buffer.AsSpan(offset, count));
                return;
            case NtfsNonResidentAttributeRecord nonResident:
                ReadFromRuns(nonResident.Runs, position, buffer, offset, count);
                return;
            default:
                throw new NotSupportedException("Unsupported NTFS attribute format.");
        }
    }

    private void ReadFromRuns(ImmutableArray<NtfsDataRun> runs, long position, byte[] buffer, int offset, int count)
    {
        long remaining = count;
        long logicalOffset = 0;

        foreach (NtfsDataRun run in runs)
        {
            long runLengthBytes = checked(run.ClusterCount * _bootSector.BytesPerCluster);
            if (position >= logicalOffset + runLengthBytes)
            {
                logicalOffset += runLengthBytes;
                continue;
            }

            long runOffset = Math.Max(0, position - logicalOffset);
            long available = runLengthBytes - runOffset;
            int bytesToRead = checked((int)Math.Min(remaining, available));
            if (bytesToRead <= 0)
            {
                break;
            }

            if (run.LogicalClusterNumber.HasValue)
            {
                long absoluteOffset = checked((run.LogicalClusterNumber.Value * _bootSector.BytesPerCluster) + runOffset);
                _stream.Position = absoluteOffset;
                _stream.ReadExactly(buffer.AsSpan(offset, bytesToRead));
            }
            else
            {
                buffer.AsSpan(offset, bytesToRead).Clear();
            }

            offset += bytesToRead;
            remaining -= bytesToRead;
            position += bytesToRead;
            logicalOffset += runLengthBytes;

            if (remaining == 0)
            {
                return;
            }
        }

        if (remaining != 0)
        {
            throw new EndOfStreamException();
        }
    }

    private static void ApplyUpdateSequenceFixup(Span<byte> record, int bytesPerSector)
    {
        ushort usaOffset = BinaryPrimitives.ReadUInt16LittleEndian(record.Slice(4, 2));
        ushort usaCount = BinaryPrimitives.ReadUInt16LittleEndian(record.Slice(6, 2));
        if (usaCount <= 1)
        {
            return;
        }

        ushort sequenceNumber = BinaryPrimitives.ReadUInt16LittleEndian(record.Slice(usaOffset, 2));

        for (int index = 1; index < usaCount; index++)
        {
            int sectorEnd = (index * bytesPerSector) - 2;
            if (sectorEnd + 2 > record.Length)
            {
                throw new InvalidDataException("Invalid NTFS update sequence array.");
            }

            ushort current = BinaryPrimitives.ReadUInt16LittleEndian(record.Slice(sectorEnd, 2));
            if (current != sequenceNumber)
            {
                throw new InvalidDataException("NTFS update sequence number mismatch.");
            }

            record.Slice(usaOffset + (index * 2), 2).CopyTo(record.Slice(sectorEnd, 2));
        }
    }

    private static long ReadSignedInteger(ReadOnlySpan<byte> bytes)
    {
        long value = 0;
        for (int i = 0; i < bytes.Length; i++)
        {
            value |= (long)bytes[i] << (8 * i);
        }

        if (bytes.Length != 0 && (bytes[^1] & 0x80) != 0)
        {
            value |= -1L << (bytes.Length * 8);
        }

        return value;
    }
}

internal sealed record NtfsBootSector(
    ushort BytesPerSector,
    byte SectorsPerCluster,
    long TotalSectors,
    long MftLogicalClusterNumber,
    int BytesPerFileRecord)
{
    public long BytesPerCluster => BytesPerSector * SectorsPerCluster;

    public static NtfsBootSector Parse(ReadOnlySpan<byte> bytes)
    {
        ushort bytesPerSector = BinaryPrimitives.ReadUInt16LittleEndian(bytes.Slice(0x0B, 2));
        byte sectorsPerCluster = bytes[0x0D];
        long totalSectors = BinaryPrimitives.ReadInt64LittleEndian(bytes.Slice(0x28, 8));
        long mftLogicalClusterNumber = BinaryPrimitives.ReadInt64LittleEndian(bytes.Slice(0x30, 8));
        sbyte clustersPerFileRecord = unchecked((sbyte)bytes[0x40]);

        int bytesPerFileRecord = clustersPerFileRecord < 0
            ? 1 << -clustersPerFileRecord
            : clustersPerFileRecord * bytesPerSector * sectorsPerCluster;

        return new NtfsBootSector(bytesPerSector, sectorsPerCluster, totalSectors, mftLogicalClusterNumber, bytesPerFileRecord);
    }
}

internal sealed record NtfsFileRecord(
    ulong RecordNumber,
    bool IsInUse,
    bool IsDirectory,
    NtfsStandardInformation? StandardInformation,
    ImmutableArray<NtfsAttributeRecord> Attributes);

internal abstract record NtfsAttributeRecord(uint Type, string? Name);

internal sealed record NtfsResidentAttributeRecord(uint Type, string? Name, byte[] Value)
    : NtfsAttributeRecord(Type, Name);

internal sealed record NtfsNonResidentAttributeRecord(uint Type, string? Name, ulong DataSize, ImmutableArray<NtfsDataRun> Runs)
    : NtfsAttributeRecord(Type, Name);

internal sealed record NtfsDataRun(long? LogicalClusterNumber, long ClusterCount);

internal sealed record NtfsStandardInformation(
    DateTime? CreationTime,
    DateTime? LastWriteTime,
    DateTime? LastAccessTime,
    FileAttributes Attributes)
{
    public static NtfsStandardInformation Parse(ReadOnlySpan<byte> bytes)
    {
        return new NtfsStandardInformation(
            ReadFileTime(bytes.Slice(0, 8)),
            ReadFileTime(bytes.Slice(8, 8)),
            ReadFileTime(bytes.Slice(24, 8)),
            (FileAttributes)BinaryPrimitives.ReadUInt32LittleEndian(bytes.Slice(32, 4)));
    }

    private static DateTime? ReadFileTime(ReadOnlySpan<byte> bytes)
    {
        long raw = BinaryPrimitives.ReadInt64LittleEndian(bytes);
        if (raw <= 0)
        {
            return null;
        }

        try
        {
            return DateTime.FromFileTimeUtc(raw);
        }
        catch (ArgumentOutOfRangeException)
        {
            return null;
        }
    }
}

internal sealed record NtfsFileNameRecord(string Name, FileAttributes Attributes, ulong RealSize, NtfsFileNameNamespace Namespace)
{
    public static NtfsFileNameRecord Parse(ReadOnlySpan<byte> bytes)
    {
        ulong realSize = BinaryPrimitives.ReadUInt64LittleEndian(bytes.Slice(0x30, 8));
        FileAttributes attributes = (FileAttributes)BinaryPrimitives.ReadUInt32LittleEndian(bytes.Slice(0x38, 4));
        byte nameLength = bytes[0x40];
        NtfsFileNameNamespace nameNamespace = (NtfsFileNameNamespace)bytes[0x41];
        string name = Encoding.Unicode.GetString(bytes.Slice(0x42, nameLength * 2));
        return new NtfsFileNameRecord(name, attributes, realSize, nameNamespace);
    }
}

internal enum NtfsFileNameNamespace : byte
{
    Posix = 0,
    Win32 = 1,
    Dos = 2,
    Win32AndDos = 3
}

internal sealed class NtfsDataStream : Stream
{
    private readonly Stream _baseStream;
    private readonly long _bytesPerCluster;
    private readonly ImmutableArray<NtfsDataRun> _runs;
    private readonly long _length;
    private long _position;

    public NtfsDataStream(Stream baseStream, long bytesPerCluster, ImmutableArray<NtfsDataRun> runs, ulong length)
    {
        _baseStream = baseStream;
        _bytesPerCluster = bytesPerCluster;
        _runs = runs;
        _length = checked((long)length);
    }

    /// <inheritdoc />
    public override bool CanRead => true;

    /// <inheritdoc />
    public override bool CanSeek => true;

    /// <inheritdoc />
    public override bool CanWrite => false;

    /// <inheritdoc />
    public override long Length => _length;

    /// <inheritdoc />
    public override long Position
    {
        get => _position;
        set
        {
            if (value < 0 || value > _length)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            _position = value;
        }
    }

    /// <inheritdoc />
    public override void Flush()
    {
    }

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count)
    {
        if (_position >= _length)
        {
            return 0;
        }

        int bytesToRead = checked((int)Math.Min(count, _length - _position));
        long remaining = bytesToRead;
        long logicalOffset = 0;
        int destinationOffset = offset;
        long position = _position;

        foreach (NtfsDataRun run in _runs)
        {
            long runLengthBytes = checked(run.ClusterCount * _bytesPerCluster);
            if (position >= logicalOffset + runLengthBytes)
            {
                logicalOffset += runLengthBytes;
                continue;
            }

            long runOffset = Math.Max(0, position - logicalOffset);
            int chunkLength = checked((int)Math.Min(remaining, runLengthBytes - runOffset));
            if (chunkLength <= 0)
            {
                break;
            }

            if (run.LogicalClusterNumber.HasValue)
            {
                long absoluteOffset = checked((run.LogicalClusterNumber.Value * _bytesPerCluster) + runOffset);
                _baseStream.Position = absoluteOffset;
                _baseStream.ReadExactly(buffer.AsSpan(destinationOffset, chunkLength));
            }
            else
            {
                buffer.AsSpan(destinationOffset, chunkLength).Clear();
            }

            destinationOffset += chunkLength;
            remaining -= chunkLength;
            position += chunkLength;
            logicalOffset += runLengthBytes;

            if (remaining == 0)
            {
                break;
            }
        }

        _position += bytesToRead - checked((int)remaining);
        return bytesToRead - checked((int)remaining);
    }

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin) => Position = origin switch
    {
        SeekOrigin.Begin => offset,
        SeekOrigin.Current => _position + offset,
        SeekOrigin.End => _length + offset,
        _ => throw new ArgumentOutOfRangeException(nameof(origin))
    };

    /// <inheritdoc />
    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }
}
