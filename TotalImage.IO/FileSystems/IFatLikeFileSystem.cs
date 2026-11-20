using System.Collections.Generic;
using System.IO;

namespace TotalImage.FileSystems;

/// <summary>
/// A FAT or exFAT file system, which stores data in cluster chains linked through a file allocation table.
/// </summary>
internal interface IFatLikeFileSystem
{
    /// <summary>
    /// Gets the stream containing the volume.
    /// </summary>
    Stream GetStream();

    /// <summary>
    /// Builds the byte ranges of the volume that hold the first <paramref name="length"/> bytes of a cluster chain.
    /// </summary>
    /// <param name="firstCluster">The first cluster in the chain. Not read when <paramref name="length"/> is 0.</param>
    /// <param name="length">The number of bytes to expose, or <see langword="null"/> for the whole chain.</param>
    /// <exception cref="InvalidDataException">The chain is cyclic, a cluster is out of range, or <paramref name="length"/> exceeds the chain.</exception>
    List<StreamExtent> GetChainExtents(uint firstCluster, long? length);
}
