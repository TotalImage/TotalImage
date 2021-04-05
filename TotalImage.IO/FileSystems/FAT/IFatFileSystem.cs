using System.IO;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
    public interface IFatFileSystem
    {
        BiosParameterBlock BiosParameterBlock { get; }
        uint DataAreaFirstSector { get; }
        uint BytesPerCluster { get; }
        uint ClusterCount { get; }

        /// <summary>
        /// Retrieves the number of the next cluster in a cluster chain.
        /// </summary>
        /// <param name="index">Current cluster</param>
        /// <param name="fat">Specifies which copy of the FAT should be used</param>
        /// <returns>The next cluster number if any, otherwise <c>null</c>.</returns>
        uint? GetNextCluster(uint index, int fat = 0);

        Stream GetStream();
    }
}