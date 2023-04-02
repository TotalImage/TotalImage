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
}
