using System.Collections;
using System.Collections.Generic;
using System.IO;
using TotalImage.FileSystems.FAT;

namespace TotalImage.FileSystems;

/// <summary>
/// Read-only base class for FAT-style cluster maps (FAT12/16/32 and exFAT).
/// </summary>
public abstract class FileAllocationTableBase : IEnumerable<uint>
{
    /// <summary>
    /// Reads the value stored at a given index.
    /// </summary>
    protected abstract uint GetEntry(uint index);

    /// <summary>
    /// Retrieves the number of entries in the allocation table.
    /// </summary>
    public abstract uint Length { get; }

    /// <summary>
    /// Determines the type of a cluster map value.
    /// </summary>
    public abstract ClusterType GetClusterType(uint value);

    /// <summary>
    /// Access the value stored at a given index.
    /// </summary>
    public uint this[uint index] => GetEntry(index);

    /// <summary>
    /// Access the value stored at a given index.
    /// </summary>
    public uint this[int index] => GetEntry((uint)index);

    /// <summary>
    /// Determines whether the cluster map value is an end-of-chain marker or a valid pointer to another data cluster.
    /// </summary>
    public bool IsEndOfChainMarker(uint cluster)
        => GetClusterType(cluster) != ClusterType.Data;

    /// <summary>
    /// Retrieves the number of the next cluster in a cluster chain.
    /// </summary>
    /// <param name="index">Current cluster</param>
    /// <returns>The next cluster number if any, otherwise <see langword="null"/>.</returns>
    public uint? GetNextCluster(uint index)
    {
        var value = GetEntry(index);
        return IsEndOfChainMarker(value) ? null : value;
    }

    /// <summary>
    /// Retrieves the full chain of clusters starting with a given cluster.
    /// </summary>
    /// <param name="firstCluster">First cluster in the chain.</param>
    /// <exception cref="InvalidDataException">The chain is cyclic.</exception>
    public uint[] GetClusterChain(uint firstCluster)
    {
        var clusters = new List<uint>();
        var visited = new HashSet<uint>();
        var cluster = (uint?)firstCluster;

        while (cluster.HasValue)
        {
            if (!visited.Add(cluster.Value))
                throw new InvalidDataException("Cyclic FAT cluster chain.");
            clusters.Add(cluster.Value);
            cluster = GetNextCluster(cluster.Value);
        }

        return clusters.ToArray();
    }

    /// <inheritdoc/>
    public IEnumerator<uint> GetEnumerator() => new Enumerator(this);

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Enumerates allocation table values.
    /// </summary>
    public class Enumerator : IEnumerator<uint>
    {
        uint _currentIndex = 0;
        readonly FileAllocationTableBase _fat;

        /// <inheritdoc/>
        public uint Current => _fat.GetEntry(_currentIndex);

        /// <inheritdoc/>
        object IEnumerator.Current => Current;

        internal Enumerator(FileAllocationTableBase fat)
        {
            _fat = fat;
        }

        /// <inheritdoc/>
        public void Dispose() { }

        /// <inheritdoc/>
        public bool MoveNext() => ++_currentIndex < _fat.Length;

        /// <inheritdoc/>
        public void Reset() => _currentIndex = 0;
    }
}
