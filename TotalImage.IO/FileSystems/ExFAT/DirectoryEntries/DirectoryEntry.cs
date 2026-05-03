using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using DirectoryEntryType = TotalImage.FileSystems.ExFAT.EntryType;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Represents a parsed exFAT directory entry.
/// </summary>
public abstract class DirectoryEntry
{
    /// <summary>
    /// Gets the raw exFAT directory entry type.
    /// </summary>
    public EntryType EntryType { get; }
    /// <summary>
    /// Gets the first cluster referenced by the entry.
    /// </summary>
    public virtual uint FirstCluster { get; }
    /// <summary>
    /// Gets the data length referenced by the entry.
    /// </summary>
    public virtual ulong DataLength { get; }

    /// <summary>
    /// Creates a directory entry from a raw directory entry buffer.
    /// </summary>
    /// <param name="entry">The 32-byte exFAT directory entry buffer.</param>
    public DirectoryEntry(in ReadOnlySpan<byte> entry)
    {
        EntryType = (EntryType)entry[0];
        FirstCluster = BinaryPrimitives.ReadUInt32LittleEndian(entry[20..24]);
        DataLength = BinaryPrimitives.ReadUInt64LittleEndian(entry[24..32]);
    }

    /// <summary>
    /// Creates a stream for the entry payload.
    /// </summary>
    /// <param name="fileSystem">The file system that owns the entry.</param>
    /// <param name="noFatChain"><see langword="true"/> to read directly from the cluster heap; otherwise follow the FAT chain.</param>
    /// <param name="length">The length of the stream to expose.</param>
    /// <returns>A readable stream for the entry payload.</returns>
    protected Stream GetStreamInternal(ExFatFileSystem fileSystem, bool noFatChain, ulong length)
    {
        if (noFatChain)
        {
            var clusterHeapOffset = fileSystem.BootSector.ClusterHeapOffset * fileSystem.BytesPerSector;
            var clusterOffset = clusterHeapOffset + (FirstCluster - 2) * fileSystem.BytesPerCluster;
            return new PartialStream(fileSystem.GetStream(), clusterOffset, (long)length)
            {
                Position = 0
            };
        }
        else
        {
            return new ExFatDataStream(fileSystem, FirstCluster);
        }
    }

    /// <summary>
    /// Creates a stream for the entry payload using <see cref="DataLength"/>.
    /// </summary>
    /// <param name="fileSystem">The file system that owns the entry.</param>
    /// <param name="noFatChain"><see langword="true"/> to read directly from the cluster heap; otherwise follow the FAT chain.</param>
    /// <returns>A readable stream for the entry payload.</returns>
    protected Stream GetStreamInternal(ExFatFileSystem fileSystem, bool noFatChain) =>
        GetStreamInternal(fileSystem, noFatChain, DataLength);

    /// <summary>
    /// Parses a raw exFAT directory entry.
    /// </summary>
    /// <param name="entry">The 32-byte exFAT directory entry buffer.</param>
    /// <returns>The parsed directory entry, or <see langword="null"/> for unsupported entry types.</returns>
    public static DirectoryEntry? Parse(in ReadOnlySpan<byte> entry) => (EntryType)entry[0] switch
    {
        DirectoryEntryType.AllocationBitmap => new AllocationBitmapDirectoryEntry(entry),
        DirectoryEntryType.VolumeLabel => new VolumeLabelDirectoryEntry(entry),
        DirectoryEntryType.File => new FileDirectoryEntry(entry),
        DirectoryEntryType.StreamExtension => new StreamExtensionDirectoryEntry(entry),
        DirectoryEntryType.FileName => new FileNameDirectoryEntry(entry),
        _ => null
    };

    /// <summary>
    /// Enumerates parsed exFAT directory entries from a directory stream.
    /// </summary>
    /// <param name="stream">The stream containing directory entry records.</param>
    /// <returns>A sequence of parsed directory entries.</returns>
    public static IEnumerable<DirectoryEntry> EnumerateDirectory(Stream stream)
    {
        var buffer = new byte[32];

        DirectoryEntry? entry = null;

        do
        {
            stream.Read(buffer);

            var position = stream.Position;

            if ((entry = Parse(buffer)) is not null)
            {
                yield return entry;
            }

            // PartialStream is stupid and we have no guarantee the stream
            // will remain at the same position we left it on before
            // the `yield return`
            stream.Position = position;
        }
        while (buffer[0] != (byte)DirectoryEntryType.EndOfDirectory);
    }
}
