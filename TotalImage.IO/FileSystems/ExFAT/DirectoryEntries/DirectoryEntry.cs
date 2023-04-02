using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using DirectoryEntryType = TotalImage.FileSystems.ExFAT.EntryType;

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

    protected Stream GetStreamInternal(ExFatFileSystem fileSystem, bool noFatChain) =>
        GetStreamInternal(fileSystem, noFatChain, DataLength);

    public static DirectoryEntry? Parse(in ReadOnlySpan<byte> entry) => (EntryType)entry[0] switch
    {
        DirectoryEntryType.AllocationBitmap => new AllocationBitmapDirectoryEntry(entry),
        DirectoryEntryType.VolumeLabel => new VolumeLabelDirectoryEntry(entry),
        DirectoryEntryType.File => new FileDirectoryEntry(entry),
        DirectoryEntryType.StreamExtension => new StreamExtensionDirectoryEntry(entry),
        DirectoryEntryType.FileName => new FileNameDirectoryEntry(entry),
        _ => null
    };

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
