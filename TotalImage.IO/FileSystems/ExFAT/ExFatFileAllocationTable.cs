using System;
using System.Buffers.Binary;
using TotalImage.FileSystems.FAT;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Read-only accessor for an exFAT cluster map
/// </summary>
public class ExFatFileAllocationTable : FileAllocationTableBase
{
    private ExFatFileSystem _fileSystem;
    private int _fatIndex;

    /// <inheritdoc/>
    protected override uint GetEntry(uint index)
    {
        if (index >= Length)
        {
            throw new IndexOutOfRangeException();
        }

        long offset = ((long)_fileSystem.BootSector.FatOffset + (long)_fatIndex * _fileSystem.BootSector.FatLength) * _fileSystem.BytesPerSector;

        var stream = _fileSystem.GetStream();
        stream.Position = offset + (long)index * 4;

        var fatEntry = new byte[4];
        stream.ReadExactly(fatEntry);

        return BinaryPrimitives.ReadUInt32LittleEndian(fatEntry);
    }

    /// <summary>
    /// Retrieves the number of entries in the allocation table.
    /// </summary>
    public override uint Length => checked((uint)((long)_fileSystem.BootSector.FatLength * _fileSystem.BytesPerSector / 4));

    /// <summary>
    /// Creates an exFAT allocation table reader.
    /// </summary>
    /// <param name="fileSystem">The file system that owns the allocation table.</param>
    /// <param name="fatIndex">The zero-based FAT index.</param>
    public ExFatFileAllocationTable(ExFatFileSystem fileSystem, int fatIndex)
    {
        _fileSystem = fileSystem;
        _fatIndex = fatIndex;
    }

    /// <summary>
    /// Determines the type of an exFAT cluster map value. 0 is free, 1 is reserved, 0xFFFFFFF7 is bad,
    /// 0xFFFFFFF8 and above are end-of-chain; everything else points to a data cluster.
    /// </summary>
    public override ClusterType GetClusterType(uint value) => value switch
    {
        0 => ClusterType.Free,
        1 => ClusterType.NonFree,
        0xFFFF_FFF7 => ClusterType.Bad,
        >= 0xFFFF_FFF8 => ClusterType.EndOfChain,
        _ => ClusterType.Data
    };
}
