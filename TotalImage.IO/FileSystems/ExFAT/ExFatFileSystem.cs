using System;
using System.Collections.Immutable;
using System.IO;

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

        for(var i = 0; i < BootSector.NumberOfFats; i++)
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
        get => "";
        set => throw new NotImplementedException();
    }

    public override Directory RootDirectory => new ExFatDirectory(this);

    public override long TotalFreeSpace => 0;

    public override long TotalSize => (long)BootSector.VolumeLength;

    public override long AllocationUnitSize => BytesPerCluster;
}
