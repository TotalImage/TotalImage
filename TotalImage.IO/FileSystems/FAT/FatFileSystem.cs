using System.IO;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
    public abstract class FatFileSystem : FileSystem
    {
        protected FatFileSystem(Stream stream) : base(stream) { }

        public abstract BiosParameterBlock BiosParameterBlock { get; }
        public abstract uint DataAreaFirstSector { get; }
        public abstract uint BytesPerCluster { get; }
        public abstract uint ClusterCount { get; }

        /// <summary>
        /// Retrieves the number of the next cluster in a cluster chain.
        /// </summary>
        /// <param name="index">Current cluster</param>
        /// <param name="fat">Specifies which copy of the FAT should be used</param>
        /// <returns>The next cluster number if any, otherwise <c>null</c>.</returns>
        public abstract uint? GetNextCluster(uint index, int fat = 0);

        public abstract ClusterMap[] ClusterMaps { get; }
        public ClusterMap MainClusterMap => ClusterMaps[0];

        public abstract class ClusterMap
        {
            public abstract uint this[uint index] { get; set; }
        }
    }
}