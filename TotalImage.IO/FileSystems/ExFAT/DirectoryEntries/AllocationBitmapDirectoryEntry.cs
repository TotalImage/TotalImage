using System;
using System.IO;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Represents an exFAT allocation bitmap directory entry.
/// </summary>
public class AllocationBitmapDirectoryEntry : PrimaryDirectoryEntry, IStreamable
{
    /// <summary>
    /// Bitmap flags associated with the allocation bitmap.
    /// </summary>
    public byte BitmapFlags { get; }

    /// <summary>
    /// Creates an allocation bitmap directory entry from a raw directory entry buffer.
    /// </summary>
    /// <param name="entry">The 32-byte exFAT directory entry buffer.</param>
    public AllocationBitmapDirectoryEntry(in ReadOnlySpan<byte> entry) : base(entry)
    {
        BitmapFlags = entry[1];
    }

    /// <inheritdoc />
    public Stream GetStream(ExFatFileSystem fileSystem) =>
        GetStreamInternal(fileSystem, true);

    /// <summary>
    /// Gets the general primary flags. Not applicable to allocation bitmap entries.
    /// </summary>
    public override ushort GeneralPrimaryFlags => throw new InvalidOperationException();
    /// <summary>
    /// Gets the number of secondary entries. Not applicable to allocation bitmap entries.
    /// </summary>
    public override byte SecondaryCount => throw new InvalidOperationException();
    /// <summary>
    /// Gets the set checksum. Not applicable to allocation bitmap entries.
    /// </summary>
    public override ushort SetChecksum => throw new InvalidOperationException();
}
