using System.IO;

namespace TotalImage.FileSystems;

/// <summary>
/// Provides read-only stream access to data stored in a FAT or exFAT cluster chain.
/// </summary>
internal sealed class ClusterChainStream : ReadOnlyExtentStream
{
    /// <summary>
    /// Creates a stream over the first <paramref name="length"/> bytes of a cluster chain.
    /// </summary>
    /// <param name="fileSystem">The file system that owns the cluster chain.</param>
    /// <param name="firstCluster">The first cluster in the chain.</param>
    /// <param name="length">The number of bytes to expose, or <see langword="null"/> for the whole chain.</param>
    /// <exception cref="InvalidDataException">The chain is cyclic, a cluster is out of range, or <paramref name="length"/> exceeds the chain.</exception>
    public ClusterChainStream(IFatLikeFileSystem fileSystem, uint firstCluster, long? length = null)
        : base(fileSystem.GetStream(), fileSystem.GetChainExtents(firstCluster, length)) { }
}
