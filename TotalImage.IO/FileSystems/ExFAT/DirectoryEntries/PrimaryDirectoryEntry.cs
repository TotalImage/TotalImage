using System;
using System.Buffers.Binary;

namespace TotalImage.FileSystems.ExFAT;

public abstract class PrimaryDirectoryEntry : DirectoryEntry
{
    public virtual byte SecondaryCount { get; }
    public virtual ushort SetChecksum { get; }
    public virtual ushort GeneralPrimaryFlags { get; }

    public PrimaryDirectoryEntry(in ReadOnlySpan<byte> entry) : base(entry)
    {
        SecondaryCount = entry[1];
        SetChecksum = BinaryPrimitives.ReadUInt16LittleEndian(entry[2..4]);
        GeneralPrimaryFlags = BinaryPrimitives.ReadUInt16LittleEndian(entry[4..6]);
    }
}
