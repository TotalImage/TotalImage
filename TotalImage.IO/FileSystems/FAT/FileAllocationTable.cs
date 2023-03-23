using System.Collections;
using System.Collections.Generic;

namespace TotalImage.FileSystems.FAT;

/// <summary>
/// Base class for accessing a FAT-style cluster map
/// </summary>
public abstract class FileAllocationTable : IEnumerable<uint>
{
    /// <summary>
    /// Access the value stored at a given index.
    /// </summary>
    public abstract uint this[uint index] { get; set; }

    /// <summary>
    /// Access the value stored at a given index.
    /// </summary>
    public uint this[int index]
    {
        get => this[(uint)index];
        set => this[(uint)index] = value;
    }

    /// <summary>
    /// Retrieves the number of entries in the allocation table.
    /// </summary>
    public abstract uint Length { get; }

    /// <summary>
    /// Cluster number bit mask. This is also the maximum cluster map value.
    /// </summary>
    protected abstract uint Mask { get; }

    /// <summary>
    /// Determines the type of a cluster map value.
    /// </summary>
    public virtual ClusterType GetClusterType(uint cluster)
    {
        if (cluster > Mask - 0x10)
        {
            if ((cluster & 0xF) == 7)
                return ClusterType.Bad;
            if ((cluster & 0xF) > 7)
                return ClusterType.EndOfChain;

            return ClusterType.Reserved;
        }
        else if (cluster < 2)
        {
            if (cluster == 0)
                return ClusterType.Free;
            if (cluster == 1)
                return ClusterType.NonFree;
        }

        return ClusterType.Data;
    }

    /// <inheritdoc/>
    public IEnumerator<uint> GetEnumerator() => new Enumerator(this);

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
        => IsEndOfChainMarker(this[index]) ? null : (uint?)this[index];

    /// <summary>
    /// Retrieves the full chain of clusters starting with a given cluster.
    /// </summary>
    /// <param name="firstCluster">First cluster in the chain.</param>
    public uint[] GetClusterChain(uint firstCluster)
    {
        var clusters = new List<uint>();
        var cluster = (uint?)firstCluster;

        while (cluster.HasValue)
        {
            clusters.Add(cluster.Value);
            cluster = GetNextCluster(cluster.Value);
        }

        return clusters.ToArray();
    }

    class Enumerator : IEnumerator<uint>
    {
        uint _currentIndex = 0;
        FileAllocationTable _fat;

        /// <inheritdoc/>
        public uint Current => _fat[_currentIndex];

        /// <inheritdoc/>
        object IEnumerator.Current => Current;

        internal Enumerator(FileAllocationTable fat)
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
