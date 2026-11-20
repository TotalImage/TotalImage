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
    /// Classifies a cluster map value. <see cref="ClusterType.Data"/> is returned only for pointers to
    /// clusters described by this table (2 to <see cref="Length"/> - 1), so chain walking follows exactly
    /// the <see cref="ClusterType.Data"/> values and stops at <see cref="ClusterType.EndOfChain"/>.
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
    /// Determines whether the cluster map value is an end-of-chain marker.
    /// </summary>
    public bool IsEndOfChainMarker(uint value)
        => GetClusterType(value) == ClusterType.EndOfChain;

    /// <summary>
    /// Determines whether a cluster number refers to a cluster described by this table.
    /// </summary>
    public bool IsValidCluster(uint cluster)
        => cluster >= 2 && cluster < Length;

    /// <summary>
    /// Retrieves the number of the next cluster in a cluster chain.
    /// </summary>
    /// <param name="index">Current cluster</param>
    /// <returns>The next cluster number, or <see langword="null"/> if the entry is an end-of-chain marker.</returns>
    /// <exception cref="InvalidDataException">
    /// The entry is neither an end-of-chain marker nor a pointer to a cluster within the table
    /// (for example free, bad, reserved or out of range).
    /// </exception>
    public uint? GetNextCluster(uint index)
    {
        var value = GetEntry(index);
        return GetClusterType(value) switch
        {
            ClusterType.EndOfChain => null,
            ClusterType.Data => value,
            var type => throw new InvalidDataException($"Cluster {index} has invalid cluster map value 0x{value:X} ({type}).")
        };
    }

    /// <summary>
    /// Retrieves the full chain of clusters starting with a given cluster.
    /// </summary>
    /// <param name="firstCluster">First cluster in the chain.</param>
    /// <exception cref="InvalidDataException">
    /// The first cluster is out of range, the chain is cyclic, or it contains a value that is neither
    /// an end-of-chain marker nor a valid cluster pointer.
    /// </exception>
    public uint[] GetClusterChain(uint firstCluster)
    {
        if (!IsValidCluster(firstCluster))
            throw new InvalidDataException($"First cluster {firstCluster} is outside the allocation table.");

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
    /// <param name="length">The number of bytes to expose, or <see langword="null"/> for the whole chain.</param>
    /// <exception cref="InvalidDataException">The chain is cyclic, a cluster is out of range, or <paramref name="length"/> exceeds the chain.</exception>
    internal List<StreamExtent> GetChainExtents(uint firstCluster, long dataStart, long bytesPerCluster, long? length)
    {
        if (length == 0)
            return [];
        var chain = GetClusterChain(firstCluster);
        return BuildExtents(chain, dataStart, bytesPerCluster, length ?? chain.LongLength * bytesPerCluster);
    }

    /// <summary>
    /// Builds the source ranges covering the first <paramref name="length"/> bytes of a cluster chain.
    /// </summary>
    /// <param name="chain">The clusters in logical order.</param>
    /// <param name="dataStart">The byte offset of cluster 2 in the volume.</param>
    /// <param name="bytesPerCluster">The cluster size in bytes.</param>
    /// <param name="length">The number of bytes to expose.</param>
    /// <returns>Ranges with consecutive clusters merged and the last range clipped to <paramref name="length"/>.</returns>
    /// <exception cref="InvalidDataException">A cluster is out of range, or <paramref name="length"/> exceeds the chain.</exception>
    internal List<StreamExtent> BuildExtents(uint[] chain, long dataStart, long bytesPerCluster, long length)
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
            if (!IsValidCluster(cluster))
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
