using System.Collections.Generic;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// Provides read-only stream access to data stored in a FAT cluster chain.
    /// </summary>
    internal sealed class FatDataStream : ReadOnlyExtentStream
    {
        /// <summary>
        /// Creates a stream over a whole FAT cluster chain.
        /// </summary>
        /// <param name="fat">The file system that owns the cluster chain.</param>
        /// <param name="firstCluster">The first cluster in the chain.</param>
        public FatDataStream(FatFileSystem fat, uint firstCluster) : base(fat.GetStream(), BuildExtents(fat, firstCluster, null)) { }

        /// <summary>
        /// Creates a stream over the data referenced by a FAT directory entry, limited to its file size.
        /// </summary>
        /// <param name="fat">The file system that owns the entry.</param>
        /// <param name="entry">The directory entry that identifies the data stream.</param>
        public FatDataStream(FatFileSystem fat, DirectoryEntry entry) : base(fat.GetStream(), BuildExtents(fat, entry.FirstClusterOfFile, entry.FileSize)) { }

        private static List<StreamExtent> BuildExtents(FatFileSystem fat, uint firstCluster, long? length)
        {
            long dataStart = (long)fat.DataAreaFirstSector * fat.BiosParameterBlock.BytesPerLogicalSector;
            return fat.MainFat.GetChainExtents(firstCluster, dataStart, fat.BytesPerCluster, length);
        }
    }
}
