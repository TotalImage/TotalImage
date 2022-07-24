using System;
using System.Buffers.Binary;

namespace TotalImage.Containers.VHD;

/// <summary>
/// A table of absolute sector offsets into the file backing the hard disk, only present for dynamic and differencing disks
/// </summary>
public class VhdBlockAllocationTable
{
    readonly uint[] _table;
    readonly int _length;
    readonly uint _blockSize;

    internal VhdBlockAllocationTable(ReadOnlySpan<byte> bytes, uint blockSize)
    {
        _length = bytes.Length / 4;
        _table = new uint[_length];
        _blockSize = blockSize;

        for (var i = 0; i < _length; i++)
        {
            _table[i] = BinaryPrimitives.ReadUInt32BigEndian(bytes[(i*4)..]);
        }
    }

    /// <summary>
    /// Returns the value of the specified index in the block allocation table.
    /// </summary>
    public uint this[int i] => _table[i];

    /// <summary>
    /// Returns the value of the specified index in the block allocation table.
    /// </summary>
    public uint this[long i] => _table[i];

    /// <summary>
    /// Size of the sector bitmap stored in the beginning of each block
    /// </summary>
    public int BitmapSizeInBytes
        => (int)(_blockSize / VhdContainer.SectorLength / 8);

    /// <summary>
    /// Size of the sector bitmap stored in the beginning of each block, rounded
    /// up to sector length
    /// </summary>
    public int SectorPaddedBitmapSizeInBytes
        => (int)Math.Ceiling(BitmapSizeInBytes / (double)VhdContainer.SectorLength) * VhdContainer.SectorLength;

    /// <summary>
    /// Returns the file-based offset of the sector bitmap for the specified
    /// block
    /// </summary>
    public long GetBitmapAddress(uint blockIndex)
        => this[blockIndex] * VhdContainer.SectorLength;

    /// <summary>
    /// Returns the file-based offset of block data for the specified block
    /// </summary>
    public long GetBlockDataAddress(uint blockIndex)
        => GetBitmapAddress(blockIndex) + SectorPaddedBitmapSizeInBytes;

    /// <summary>
    /// Returns true if the specified block is allocated
    /// </summary>
    public bool HasData(long blockIndex) => this[blockIndex] != uint.MaxValue;
}
