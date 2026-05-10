using System.Collections.Generic;
using System.Collections.Immutable;
using TotalImage.Containers;
using TotalImage.FileSystems;

namespace TotalImage.Partitions
{
    /// <summary>
    /// A partition table class that represents the absence of a partition table
    /// and exposes a single file system containing the content of the image.
    /// </summary>
    public class NoPartitionTable : PartitionTable
    {
        /// <inheritdoc />
        public override string DisplayName => "Unpartitioned";

        /// <inheritdoc />
        public NoPartitionTable(Container container) : base(container) { }

        /// <inheritdoc />
        protected override IEnumerable<PartitionEntry> LoadPartitions()
        {
            /* We attempts to detect a file system within the "only partition" represented by the entire content of the image
             * and if we fail to detect one, we return a single partition entry with no file system. 
             * This ensures we can immediately get some file system metadata (such as TotalSize) without instantiating it twice. */
            var fileSystem = FileSystem.AttemptDetection(_container.Content);
            if (fileSystem != null)
            {
                return ImmutableList.Create(new PartitionEntry(0, fileSystem.TotalSize, _container.Content, fileSystem));
            }

            return ImmutableList.Create(new PartitionEntry(0, _container.Length, _container.Content));
        }
    }
}
