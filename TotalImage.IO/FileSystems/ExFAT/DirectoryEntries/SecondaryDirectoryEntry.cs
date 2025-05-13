using System;

namespace TotalImage.FileSystems.ExFAT;

public abstract class SecondaryDirectoryEntry : DirectoryEntry
{
    public byte GeneralSecondaryFlags { get; }

    public SecondaryDirectoryEntry(in ReadOnlySpan<byte> entry) : base(entry)
    {
        GeneralSecondaryFlags = entry[1];
    }
}
