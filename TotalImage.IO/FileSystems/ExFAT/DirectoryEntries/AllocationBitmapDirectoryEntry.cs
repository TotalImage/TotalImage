using System;
using System.IO;

namespace TotalImage.FileSystems.ExFAT;

public class AllocationBitmapDirectoryEntry : PrimaryDirectoryEntry, IStreamable
{
    public byte BitmapFlags { get; }

    public AllocationBitmapDirectoryEntry(in ReadOnlySpan<byte> entry) : base(entry)
    {
        BitmapFlags = entry[1];
    }

    public Stream GetStream(ExFatFileSystem fileSystem) =>
        GetStreamInternal(fileSystem, true);

    public override ushort GeneralPrimaryFlags => throw new InvalidOperationException();
    public override byte SecondaryCount => throw new InvalidOperationException();
    public override ushort SetChecksum => throw new InvalidOperationException();
}
