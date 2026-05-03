using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Represents an exFAT file system.
/// </summary>
public class ExFatFileSystem : FileSystem
{
    /// <summary>
    /// Gets the parsed exFAT boot sector.
    /// </summary>
    public ExFatBootSector BootSector { get; }

    /// <summary>
    /// Gets the allocation tables present in the volume.
    /// </summary>
    public ImmutableArray<ExFatFileAllocationTable> Fats { get; }

    /// <summary>
    /// Gets the active allocation table selected by the volume flags.
    /// </summary>
    public ExFatFileAllocationTable ActiveFat =>
        Fats[BootSector.VolumeFlags & 0x01];

    /// <inheritdoc />
    public override bool SupportsSubdirectories => true;

    /// <inheritdoc />
    public override bool IsReadOnly => false;

    /// <summary>
    /// Creates an exFAT file system from a stream.
    /// </summary>
    /// <param name="stream">The stream containing the exFAT volume.</param>
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

    /// <summary>
    /// Gets the number of bytes per sector.
    /// </summary>
    public int BytesPerSector => 1 << BootSector.BytesPerSectorShift;
    /// <summary>
    /// Gets the number of sectors per cluster.
    /// </summary>
    public int SectorsPerCluster => 1 << BootSector.SectorsPerClusterShift;
    /// <summary>
    /// Gets the number of bytes per cluster.
    /// </summary>
    public long BytesPerCluster => SectorsPerCluster * BytesPerSector;

    /// <inheritdoc />
    public override string DisplayName => "exFAT";

    /// <inheritdoc />
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

    /// <inheritdoc />
    public override Directory RootDirectory => new ExFatDirectory(this);

    /// <inheritdoc />
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

    /// <inheritdoc />
    public override long TotalSize => (long)BootSector.VolumeLength;

    /// <inheritdoc />
    public override long AllocationUnitSize => BytesPerCluster;
}
