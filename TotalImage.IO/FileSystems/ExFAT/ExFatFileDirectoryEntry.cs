using System;
using System.Buffers.Binary;

namespace TotalImage.FileSystems.ExFAT;

public class ExFatFileDirectoryEntry
{
    public byte EntryType { get; }
    public byte SecondaryCount { get; }
    public ushort SetChecksum { get; }
    public ushort FileAttributes { get; }
    public uint CreateTimestamp { get; }
    public uint LastModifiedTimestamp { get; }
    public uint LastAccessedTimestamp { get; }
    public byte Create10msIncrement { get; }
    public byte LastModified10msIncrement { get; }
    public byte CreateUtcOffset { get; }
    public byte LastModifiedUtcOffset { get; }
    public byte LastAccessedUtcOffset { get; }

    public ExFatFileDirectoryEntry(in ReadOnlySpan<byte> entry)
    {
        EntryType = entry[0];
        SecondaryCount = entry[1];
        SetChecksum = BinaryPrimitives.ReadUInt16LittleEndian(entry[2..4]);
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
}
