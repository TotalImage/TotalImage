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
    /// Retrieves the number of entries in the allocation table that describe clusters of the volume.
    /// This is at most the cluster count plus 2 (entries 0 and 1 are reserved); any slack past the
    /// end of the volume's clusters is excluded. Cluster pointers must be in the range 2..Length-1.
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

    /// <summary>
    /// Builds the byte ranges of the volume that hold the first <paramref name="length"/> bytes of a cluster chain.
    /// </summary>
    /// <param name="firstCluster">The first cluster in the chain. Not read when <paramref name="length"/> is 0.</param>
    /// <param name="dataStart">The byte offset of cluster 2 in the volume.</param>
    /// <param name="bytesPerCluster">The cluster size in bytes.</param>
    /// <param name="clusterCount">The number of data clusters in the volume.</param>
    /// <param name="length">The number of bytes to expose, or <see langword="null"/> for the whole chain.</param>
    /// <exception cref="InvalidDataException">The chain is cyclic, a cluster is out of range, or <paramref name="length"/> exceeds the chain.</exception>
    internal List<StreamExtent> GetChainExtents(uint firstCluster, long dataStart, long bytesPerCluster, uint clusterCount, long? length)
    {
        if (length == 0)
            return [];
        var chain = GetClusterChain(firstCluster);
        return BuildExtents(chain, dataStart, bytesPerCluster, clusterCount, length ?? chain.LongLength * bytesPerCluster);
    }

    /// <summary>
    /// Builds the source ranges covering the first <paramref name="length"/> bytes of a cluster chain.
    /// </summary>
    /// <param name="chain">The clusters in logical order.</param>
    /// <param name="dataStart">The byte offset of cluster 2 in the volume.</param>
    /// <param name="bytesPerCluster">The cluster size in bytes.</param>
    /// <param name="clusterCount">The number of data clusters in the volume.</param>
    /// <param name="length">The number of bytes to expose.</param>
    /// <returns>Ranges with consecutive clusters merged and the last range clipped to <paramref name="length"/>.</returns>
    /// <exception cref="InvalidDataException">A cluster is out of range, or <paramref name="length"/> exceeds the chain.</exception>
    internal static List<StreamExtent> BuildExtents(uint[] chain, long dataStart, long bytesPerCluster, uint clusterCount, long length)
    {
        var extents = new List<StreamExtent>();
        if (length <= 0)
            return extents;
        if (length > chain.LongLength * bytesPerCluster)
            throw new InvalidDataException("The data length exceeds its cluster chain.");

        long remaining = length;
        uint previous = 0;
        for (int i = 0; i < chain.Length && remaining > 0; i++)
        {
            uint cluster = chain[i];
            if (cluster < 2 || cluster - 2 >= clusterCount)
                throw new InvalidDataException($"Cluster {cluster} is outside the data area.");
            long take = remaining < bytesPerCluster ? remaining : bytesPerCluster;
            if (extents.Count > 0 && cluster == previous + 1)
            {
                var last = extents[^1];
                extents[^1] = last with { Length = last.Length + take };
            }
            else
            {
                extents.Add(new StreamExtent(dataStart + ((long)cluster - 2) * bytesPerCluster, take));
            }
            previous = cluster;
            remaining -= take;
        }
        return extents;
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
