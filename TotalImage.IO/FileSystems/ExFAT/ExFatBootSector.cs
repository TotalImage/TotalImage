using System;
using System.Buffers.Binary;
using System.Collections.Immutable;
using System.Text;

namespace TotalImage.FileSystems.ExFAT;

public class ExFatBootSector
{
    public ImmutableArray<byte> JumpBoot { get; }
    public string FileSystemName { get; }
    public ImmutableArray<byte> MustBeZero { get; }
    public ulong PartitionOffset { get; }
    public ulong VolumeLength { get; }
    public uint FatOffset { get; }
    public uint FatLength { get; }
    public uint ClusterHeapOffset { get; }
    public uint ClusterCount { get; }
    public uint FirstClusterOfRootDirectory { get; }
    public uint VolumeSerialNumber { get; }
    public ushort FileSystemRevision { get; }
    public ushort VolumeFlags { get; }
    public byte BytesPerSectorShift { get; }
    public byte SectorsPerClusterShift { get; }
    public byte NumberOfFats { get; }
    public byte DriveSelect { get; }
    public byte PercentInUse { get; }
    public ImmutableArray<byte> Reserved { get; }
    public ImmutableArray<byte> BootCode { get; }
    public ushort BootSignature { get; }

    public ExFatBootSector(in ReadOnlySpan<byte> bootSector)
    {
        JumpBoot = bootSector[0..3].ToImmutableArray();
        FileSystemName = Encoding.ASCII.GetString(bootSector[3..11]);
        MustBeZero = bootSector[11..64].ToImmutableArray();

        PartitionOffset = BinaryPrimitives.ReadUInt64LittleEndian(bootSector[64..72]);
        VolumeLength = BinaryPrimitives.ReadUInt64LittleEndian(bootSector[72..80]);
        FatOffset = BinaryPrimitives.ReadUInt32LittleEndian(bootSector[80..84]);
        FatLength = BinaryPrimitives.ReadUInt32LittleEndian(bootSector[84..88]);
        ClusterHeapOffset = BinaryPrimitives.ReadUInt32LittleEndian(bootSector[88..92]);
        ClusterCount = BinaryPrimitives.ReadUInt32LittleEndian(bootSector[92..96]);
        FirstClusterOfRootDirectory = BinaryPrimitives.ReadUInt32LittleEndian(bootSector[96..100]);
        VolumeSerialNumber = BinaryPrimitives.ReadUInt32LittleEndian(bootSector[100..104]);
        FileSystemRevision = BinaryPrimitives.ReadUInt16LittleEndian(bootSector[104..106]);
        VolumeFlags = BinaryPrimitives.ReadUInt16LittleEndian(bootSector[106..108]);
        BytesPerSectorShift = bootSector[108];
        SectorsPerClusterShift = bootSector[109];
        NumberOfFats = bootSector[110];
        DriveSelect = bootSector[111];
        PercentInUse = bootSector[112];
        Reserved = bootSector[113..120].ToImmutableArray();

        BootCode = bootSector[120..510].ToImmutableArray();
        BootSignature = BinaryPrimitives.ReadUInt16LittleEndian(bootSector[510..512]);
    }
}
