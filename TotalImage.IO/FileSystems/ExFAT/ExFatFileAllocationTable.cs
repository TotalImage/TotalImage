using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Base class for accessing a FAT-style cluster map
/// </summary>
public class ExFatFileAllocationTable : IEnumerable<uint>
{
    private ExFatFileSystem _fileSystem;
    private int _fatIndex;

    /// <summary>
    /// Access the value stored at a given index.
    /// </summary>
    public uint this[uint index]
    {
        get
        {
            if (index > _fileSystem.BootSector.FatLength * _fileSystem.BytesPerSector / 4)
            {
                throw new IndexOutOfRangeException();
            }

            var offset = _fileSystem.BootSector.FatOffset * _fileSystem.BytesPerSector;
            offset += _fatIndex * _fileSystem.BootSector.FatLength * _fileSystem.BytesPerSector;

            var stream = _fileSystem.GetStream();
            stream.Position = offset + index * 4;

            var fatEntry = new byte[4];
            stream.Read(fatEntry);

            return BinaryPrimitives.ReadUInt32LittleEndian(fatEntry);
        }
    }

    /// <summary>
    /// Access the value stored at a given index.
    /// </summary>
    public uint this[int index]
    {
        get => this[(uint)index];
    }

    /// <summary>
    /// Retrieves the number of entries in the allocation table.
    /// </summary>
    public uint Length { get; }

    public ExFatFileAllocationTable(ExFatFileSystem fileSystem, int fatIndex)
    {
        _fileSystem = fileSystem;
        _fatIndex = fatIndex;
    }

    /// <inheritdoc/>
    public IEnumerator<uint> GetEnumerator() => new Enumerator(this);

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Retrieves the number of the next cluster in a cluster chain.
    /// </summary>
    /// <param name="index">Current cluster</param>
    /// <returns>The next cluster number if any, otherwise <see langword="null"/>.</returns>
    public uint? GetNextCluster(uint index) => this[index] switch
    {
        0 => null,
        1 => null,
        0xFFFF_FFF7 => null,
        0xFFFF_FFFF => null,
        uint x => x
    };

    /// <summary>
    /// Retrieves the full chain of clusters starting with a given cluster.
    /// </summary>
    /// <param name="firstCluster">First cluster in the chain.</param>
    public uint[] GetClusterChain(uint firstCluster)
    {
        var clusters = new List<uint>();
        uint? cluster = firstCluster;

        while (cluster.HasValue)
        {
            clusters.Add(cluster.Value);
            cluster = GetNextCluster(cluster.Value);
        }

        return clusters.ToArray();
    }

    public class Enumerator : IEnumerator<uint>
    {
        uint _currentIndex = 0;
        ExFatFileAllocationTable _fat;

        /// <inheritdoc/>
        public uint Current => _fat[_currentIndex];

        /// <inheritdoc/>
        object IEnumerator.Current => Current;

        internal Enumerator(ExFatFileAllocationTable fat)
        {
            _fat = fat;
        }

        /// <inheritdoc/>
        public void Dispose() { }

        /// <inheritdoc/>
        public bool MoveNext() => ++_currentIndex < _fat.Length ? false : true;

        /// <inheritdoc/>
        public void Reset() => _currentIndex = 0;
    }
}
