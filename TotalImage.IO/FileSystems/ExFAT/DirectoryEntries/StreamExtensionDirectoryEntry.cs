using System;
using System.Buffers.Binary;
using System.IO;

namespace TotalImage.FileSystems.ExFAT;

public class StreamExtensionDirectoryEntry : SecondaryDirectoryEntry
{
    public byte NameLength { get; }
    public ushort NameHash { get; }
    public ulong ValidDataLength { get; }

    public StreamExtensionDirectoryEntry(in ReadOnlySpan<byte> entry) : base(entry)
    {
        NameLength = entry[3];
        NameHash = BinaryPrimitives.ReadUInt16LittleEndian(entry[4..6]);
        ValidDataLength = BinaryPrimitives.ReadUInt64LittleEndian(entry[8..16]);
    }

    public Stream GetStream(ExFatFileSystem fileSystem)
    {
        if ((GeneralSecondaryFlags & 0x02) != 0) // NoFatChain
        {
            var clusterHeapOffset = fileSystem.BootSector.ClusterHeapOffset * fileSystem.BytesPerSector;
            var clusterOffset = clusterHeapOffset + (FirstCluster - 2) * fileSystem.BytesPerCluster;
            return new PartialStream(fileSystem.GetStream(), clusterOffset, (long)ValidDataLength)
            {
                Position = 0
            };
        }
        else
        {
            return new ExFatDataStream(fileSystem, FirstCluster);
        }
    }
}
