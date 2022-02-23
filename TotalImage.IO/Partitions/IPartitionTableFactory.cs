using System.IO;

namespace TotalImage.Partitions
{
    /// <summary>
    /// An interface to support creating partition tables from saved images
    /// </summary>
    public interface IPartitionTableFactory
    {
        /// <summary>
        /// Attempts to load a partition table from a given steam
        /// </summary>
        /// <param name="stream">The stream containing a partition table</param>
        /// <returns>The partition table if load was successful, null if not.</returns>
        public PartitionTable? TryLoadPartitionTable(Stream stream);
    }
}
