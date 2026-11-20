using System.Collections.Generic;

namespace TotalImage.FileSystems.ExFAT
{
    /// <summary>
    /// Provides read-only stream access to data stored in an exFAT cluster chain.
    /// </summary>
    internal sealed class ExFatDataStream : ReadOnlyExtentStream
    {
        /// <summary>
        /// Creates a stream over a whole exFAT cluster chain.
        /// </summary>
        /// <param name="fileSystem">The file system that owns the cluster chain.</param>
        /// <param name="firstCluster">The first cluster in the chain.</param>
        public ExFatDataStream(ExFatFileSystem fileSystem, uint firstCluster) : base(fileSystem.GetStream(), BuildExtents(fileSystem, firstCluster, null)) { }

        /// <summary>
        /// Creates a stream over the first <paramref name="length"/> bytes of an exFAT cluster chain.
        /// </summary>
        /// <param name="fileSystem">The file system that owns the cluster chain.</param>
        /// <param name="firstCluster">The first cluster in the chain.</param>
        /// <param name="length">The number of bytes to expose.</param>
        /// <exception cref="System.IO.InvalidDataException"><paramref name="length"/> exceeds the cluster chain.</exception>
        public ExFatDataStream(ExFatFileSystem fileSystem, uint firstCluster, long length) : base(fileSystem.GetStream(), BuildExtents(fileSystem, firstCluster, length)) { }

        private static List<StreamExtent> BuildExtents(ExFatFileSystem fs, uint firstCluster, long? length)
        {
            long dataStart = (long)fs.BootSector.ClusterHeapOffset * fs.BytesPerSector;
            return fs.ActiveFat.GetChainExtents(firstCluster, dataStart, fs.BytesPerCluster, length);
        }
    }
}
