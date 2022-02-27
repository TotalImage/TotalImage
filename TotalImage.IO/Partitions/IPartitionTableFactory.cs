using TotalImage.Containers;

namespace TotalImage.Partitions
{
    /// <summary>
    /// An interface to support creating partition tables from saved images
    /// </summary>
    public interface IPartitionTableFactory
    {
        /// <summary>
        /// Attempts to load a partition table from a given container
        /// </summary>
        /// <param name="container">The container containing a partition table</param>
        /// <returns>The partition table if load was successful, null if not.</returns>
        public PartitionTable? TryLoadPartitionTable(Container container);
    }
}
