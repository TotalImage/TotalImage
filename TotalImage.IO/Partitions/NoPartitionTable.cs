using System.Collections.Generic;
using System.Collections.Immutable;
using TotalImage.Containers;

namespace TotalImage.Partitions
{
    /// <summary>
    /// A partition table class that represents the absence of a partition table
    /// and exposes a single file system containing the content of the image.
    /// </summary>
    public class NoPartitionTable : PartitionTable
    {
        /// <inheritdoc />
        public NoPartitionTable(Container container) : base(container)
        {
        }

        /// <inheritdoc />
        public override string DisplayName => "Unpartitioned";

        /// <inheritdoc />
        protected override IEnumerable<PartitionEntry> LoadPartitions()
        {
            return ImmutableList.Create(new PartitionEntry(0, _container.Length, _container.Content));
        }
    }
}
