using System;
using System.Buffers.Binary;
using System.Collections.Immutable;
using System.Text;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Represents the on-disk exFAT boot sector.
/// </summary>
public class ExFatBootSector
{
    /// <summary>
    /// Gets the jump boot instruction bytes.
    /// </summary>
    public ImmutableArray<byte> JumpBoot { get; }
    /// <summary>
    /// Gets the file system name signature.
    /// </summary>
    public string FileSystemName { get; }
    /// <summary>
    /// Gets the reserved bytes that must be zero.
    /// </summary>
    public ImmutableArray<byte> MustBeZero { get; }
    /// <summary>
    /// Gets the partition offset in sectors.
    /// </summary>
    public ulong PartitionOffset { get; }
    /// <summary>
    /// Gets the volume length in sectors.
    /// </summary>
    public ulong VolumeLength { get; }
    /// <summary>
    /// Gets the FAT offset in sectors.
    /// </summary>
    public uint FatOffset { get; }
    /// <summary>
    /// Gets the FAT length in sectors.
    /// </summary>
    public uint FatLength { get; }
    /// <summary>
    /// Gets the cluster heap offset in sectors.
    /// </summary>
    public uint ClusterHeapOffset { get; }
    /// <summary>
    /// Gets the number of clusters in the cluster heap.
    /// </summary>
    public uint ClusterCount { get; }
    /// <summary>
    /// Gets the first cluster of the root directory.
    /// </summary>
    public uint FirstClusterOfRootDirectory { get; }
    /// <summary>
    /// Gets the volume serial number.
    /// </summary>
    public uint VolumeSerialNumber { get; }
    /// <summary>
    /// Gets the file system revision.
    /// </summary>
    public ushort FileSystemRevision { get; }
    /// <summary>
    /// Gets the volume flags.
    /// </summary>
    public ushort VolumeFlags { get; }
    /// <summary>
    /// Gets the base-2 shift for bytes per sector.
    /// </summary>
    public byte BytesPerSectorShift { get; }
    /// <summary>
    /// Gets the base-2 shift for sectors per cluster.
    /// </summary>
    public byte SectorsPerClusterShift { get; }
    /// <summary>
    /// Gets the number of FATs.
    /// </summary>
    public byte NumberOfFats { get; }
    /// <summary>
    /// Gets the drive select value.
    /// </summary>
    public byte DriveSelect { get; }
    /// <summary>
    /// Gets the percentage of clusters in use.
    /// </summary>
    public byte PercentInUse { get; }
    /// <summary>
    /// Gets reserved boot sector bytes.
    /// </summary>
    public ImmutableArray<byte> Reserved { get; }
    /// <summary>
    /// Gets the boot code region.
    /// </summary>
    public ImmutableArray<byte> BootCode { get; }
    /// <summary>
    /// Gets the boot signature.
    /// </summary>
    public ushort BootSignature { get; }

    /// <summary>
    /// Creates an exFAT boot sector from a 512-byte sector buffer.
    /// </summary>
    /// <param name="bootSector">The raw boot sector bytes.</param>
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
