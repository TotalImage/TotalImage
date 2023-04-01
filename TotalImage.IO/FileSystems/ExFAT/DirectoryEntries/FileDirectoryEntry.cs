using System;
using System.Buffers.Binary;
using TotalImage.FileSystems.FAT;

namespace TotalImage.FileSystems.ExFAT;

public class FileDirectoryEntry : PrimaryDirectoryEntry
{
    public ushort FileAttributes { get; }
    public uint CreateTimestamp { get; }
    public uint LastModifiedTimestamp { get; }
    public uint LastAccessedTimestamp { get; }
    public byte Create10msIncrement { get; }
    public byte LastModified10msIncrement { get; }
    public byte CreateUtcOffset { get; }
    public byte LastModifiedUtcOffset { get; }
    public byte LastAccessedUtcOffset { get; }

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

    public DateTime? CreateTime =>
        FatDateTime.ToDateTime(
            (ushort)((CreateTimestamp & 0xFFFF_0000) >> 16),
            (ushort)CreateTimestamp, Create10msIncrement);

    public DateTime? LastModifiedTime =>
        FatDateTime.ToDateTime(
            (ushort)((LastModifiedTimestamp & 0xFFFF_0000) >> 16),
            (ushort)LastModifiedTimestamp, LastModified10msIncrement);


    public DateTime? LastAccessedTime =>
        FatDateTime.ToDateTime(
            (ushort)((LastAccessedTimestamp & 0xFFFF_0000) >> 16),
            (ushort)LastAccessedTimestamp);

    public override ushort GeneralPrimaryFlags => throw new InvalidOperationException();
    public override uint FirstCluster => throw new InvalidOperationException();
    public override ulong DataLength => throw new InvalidOperationException();
}
