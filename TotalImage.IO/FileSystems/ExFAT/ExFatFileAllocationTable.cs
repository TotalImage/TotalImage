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

    /// <inheritdoc/>
    public override uint Length => (uint)Math.Min(
        (ulong)_fileSystem.BootSector.FatLength * (ulong)_fileSystem.BytesPerSector / 4,
        (ulong)_fileSystem.BootSector.ClusterCount + 2);

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
    /// Classifies an exFAT cluster map value. 0xFFFFFFF7 is bad, 0xFFFFFFFF is end-of-chain and
    /// 2 to <see cref="FileAllocationTableBase.Length"/> - 1 is a data cluster pointer; every other value
    /// (0, 1, pointers past the last cluster, 0xFFFFFFF8–0xFFFFFFFE) is invalid.
    /// </summary>
    public override ClusterType GetClusterType(uint value) => value switch
    {
        0xFFFF_FFF7 => ClusterType.Bad,
        0xFFFF_FFFF => ClusterType.EndOfChain,
        _ when value >= 2 && value < Length => ClusterType.Data,
        _ => ClusterType.Invalid
    };
}
