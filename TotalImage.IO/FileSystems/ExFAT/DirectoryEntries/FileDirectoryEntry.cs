using System;
using System.Buffers.Binary;
using TotalImage.FileSystems.FAT;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Represents an exFAT file directory entry.
/// </summary>
public class FileDirectoryEntry : PrimaryDirectoryEntry
{
    /// <summary>
    /// Gets the file attribute flags.
    /// </summary>
    public ushort FileAttributes { get; }
    /// <summary>
    /// Gets the raw creation timestamp value.
    /// </summary>
    public uint CreateTimestamp { get; }
    /// <summary>
    /// Gets the raw last modified timestamp value.
    /// </summary>
    public uint LastModifiedTimestamp { get; }
    /// <summary>
    /// Gets the raw last accessed timestamp value.
    /// </summary>
    public uint LastAccessedTimestamp { get; }
    /// <summary>
    /// Gets the 10-millisecond creation time increment.
    /// </summary>
    public byte Create10msIncrement { get; }
    /// <summary>
    /// Gets the 10-millisecond last modified time increment.
    /// </summary>
    public byte LastModified10msIncrement { get; }
    /// <summary>
    /// Gets the UTC offset for the creation time.
    /// </summary>
    public byte CreateUtcOffset { get; }
    /// <summary>
    /// Gets the UTC offset for the last modified time.
    /// </summary>
    public byte LastModifiedUtcOffset { get; }
    /// <summary>
    /// Gets the UTC offset for the last accessed time.
    /// </summary>
    public byte LastAccessedUtcOffset { get; }

    /// <summary>
    /// Creates a file directory entry from a raw directory entry buffer.
    /// </summary>
    /// <param name="entry">The 32-byte exFAT directory entry buffer.</param>
    public FileDirectoryEntry(in ReadOnlySpan<byte> entry) : base(entry)
    {
        FileAttributes = BinaryPrimitives.ReadUInt16LittleEndian(entry[4..6]);
        CreateTimestamp = BinaryPrimitives.ReadUInt32LittleEndian(entry[8..12]);
        LastModifiedTimestamp = BinaryPrimitives.ReadUInt32LittleEndian(entry[12..16]);
        LastAccessedTimestamp = BinaryPrimitives.ReadUInt32LittleEndian(entry[16..20]);
        Create10msIncrement = entry[20];
        LastModified10msIncrement = entry[21];
        CreateUtcOffset = entry[22];
        LastModifiedUtcOffset = entry[23];
        LastAccessedUtcOffset = entry[24];
    }

    /// <summary>
    /// Gets the creation time as a <see cref="DateTime"/> value when it can be decoded.
    /// </summary>
    public DateTime? CreateTime =>
        FatDateTime.ToDateTime(
            (ushort)((CreateTimestamp & 0xFFFF_0000) >> 16),
            (ushort)CreateTimestamp, Create10msIncrement);

    /// <summary>
    /// Gets the last modified time as a <see cref="DateTime"/> value when it can be decoded.
    /// </summary>
    public DateTime? LastModifiedTime =>
        FatDateTime.ToDateTime(
            (ushort)((LastModifiedTimestamp & 0xFFFF_0000) >> 16),
            (ushort)LastModifiedTimestamp, LastModified10msIncrement);


    /// <summary>
    /// Gets the last accessed time as a <see cref="DateTime"/> value when it can be decoded.
    /// </summary>
    public DateTime? LastAccessedTime =>
        FatDateTime.ToDateTime(
            (ushort)((LastAccessedTimestamp & 0xFFFF_0000) >> 16),
            (ushort)LastAccessedTimestamp);

    /// <summary>
    /// Gets the general primary flags. Not applicable to file directory entries.
    /// </summary>
    public override ushort GeneralPrimaryFlags => throw new InvalidOperationException();
    /// <summary>
    /// Gets the first cluster. Not applicable to file directory entries.
    /// </summary>
    public override uint FirstCluster => throw new InvalidOperationException();
    /// <summary>
    /// Gets the data length. Not applicable to file directory entries.
    /// </summary>
    public override ulong DataLength => throw new InvalidOperationException();
}
