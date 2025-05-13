using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace TotalImage.FileSystems.ExFAT;

public class ExFatFileSystem : FileSystem
{
    public ExFatBootSector BootSector { get; }

    public ImmutableArray<ExFatFileAllocationTable> Fats { get; }

    public ExFatFileAllocationTable ActiveFat =>
        Fats[BootSector.VolumeFlags & 0x01];

    public ExFatFileSystem(Stream stream) : base(stream)
    {
        var sector = new byte[512];

        stream.Seek(0, SeekOrigin.Begin);
        stream.Read(sector);

        BootSector = new(sector);

        var builder = ImmutableArray.CreateBuilder<ExFatFileAllocationTable>();

        for (var i = 0; i < BootSector.NumberOfFats; i++)
        {
            builder.Add(new ExFatFileAllocationTable(this, i));
        }

        Fats = builder.ToImmutable();
    }

    public int BytesPerSector => 1 << BootSector.BytesPerSectorShift;
    public int SectorsPerCluster => 1 << BootSector.SectorsPerClusterShift;
    public long BytesPerCluster => SectorsPerCluster * BytesPerSector;

    public override string DisplayName => "exFAT";

    public override string VolumeLabel
    {
        get
        {
            var rootDirectoryStream = new ExFatDataStream(this, BootSector.FirstClusterOfRootDirectory);
            var volumeLabel = (VolumeLabelDirectoryEntry?)DirectoryEntry
                .EnumerateDirectory(rootDirectoryStream)
                .SingleOrDefault(x => x is VolumeLabelDirectoryEntry);

            return volumeLabel?.VolumeLabel[0..volumeLabel.CharacterCount] ?? string.Empty;
        }
        set => throw new NotImplementedException();
    }

    public override Directory RootDirectory => new ExFatDirectory(this);

    public override long TotalFreeSpace
    {
        get
        {
            var rootDirectoryStream = new ExFatDataStream(this, BootSector.FirstClusterOfRootDirectory);
            var allocationBitmap = (AllocationBitmapDirectoryEntry)DirectoryEntry
                .EnumerateDirectory(rootDirectoryStream)
                .Single(x => x is AllocationBitmapDirectoryEntry entry &&
                    (entry.BitmapFlags & 0x01) == (BootSector.VolumeFlags & 0x01));

            var buffer = new byte[BootSector.ClusterCount / 8 + 1];
            var stream = allocationBitmap.GetStream(this);
            stream.Read(buffer);

            var freeClusters = 0L;

            for (var i = 0; i < buffer.Length - 1; i++)
            {
                for (var shift = 0; shift < 8; shift++)
                {
                    if ((buffer[i] & (1 << shift)) == 0)
                    {
                        freeClusters++;
                    }
                }
            }

            for (var shift = 0; shift < buffer.Length % 8; shift++)
            {
                if ((buffer[^1] & (1 << shift)) == 0)
                {
                    freeClusters++;
                }
            }

            return freeClusters * BytesPerCluster;
        }
    }

    public override long TotalSize => (long)BootSector.VolumeLength;

    public override long AllocationUnitSize => BytesPerCluster;
}
