using System;
using System.Buffers.Binary;
using System.IO;

namespace TotalImage.FileSystems.ExFAT;

public class ExFatStreamExtensionDirectoryEntry
{
    public byte EntryType { get; }
    public byte GeneralSecondaryFlags { get; }
    public byte NameLength { get; }
    public ushort NameHash { get; }
    public ulong ValidDataLength { get; }
    public uint FirstCluster { get; }
    public ulong DataLength { get; }

    public ExFatStreamExtensionDirectoryEntry(in ReadOnlySpan<byte> entry)
    {
        EntryType = entry[0];
        GeneralSecondaryFlags = entry[1];
        NameLength = entry[3];
        NameHash = BinaryPrimitives.ReadUInt16LittleEndian(entry[4..6]);
        ValidDataLength = BinaryPrimitives.ReadUInt64LittleEndian(entry[8..16]);
        FirstCluster = BinaryPrimitives.ReadUInt32LittleEndian(entry[20..24]);
        DataLength = BinaryPrimitives.ReadUInt64LittleEndian(entry[24..32]);
    }
}
