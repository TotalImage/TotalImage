using System;
using System.Buffers.Binary;

namespace TotalImage.FileSystems.ExFAT;

public abstract class DirectoryEntry
{
    public EntryType EntryType { get; }
    public virtual uint FirstCluster { get; }
    public virtual ulong DataLength { get; }

    public DirectoryEntry(in ReadOnlySpan<byte> entry)
    {
        EntryType = (EntryType)entry[0];
        FirstCluster = BinaryPrimitives.ReadUInt32LittleEndian(entry[20..24]);
        DataLength = BinaryPrimitives.ReadUInt64LittleEndian(entry[24..32]);
    }
}
