using System.Collections.Generic;
using System.Collections.Immutable;
using TotalImage.Containers;

namespace TotalImage.Partitions
{
    /// <summary>
    /// An abstract representation of a partition table
    /// </summary>
    public abstract class PartitionTable
    {
        /// <summary>
        /// The container that contains the partition table
        /// </summary>
        protected readonly Container _container;

        /// <summary>
        /// Name of the partition table for displaying in the UI
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// A list of partitions contained within the partition table, to be validated and exposed by <see cref="Partitions" />
        ///
        /// </summary>
        protected IReadOnlyList<PartitionEntry>? _partitions = null;

        /// <summary>
        /// A list of partitions contained within the partition table
        /// </summary>
        public IReadOnlyList<PartitionEntry> Partitions
        {
            get
            {
                _partitions ??= LoadPartitions().ToImmutableArray();
                return _partitions;
            }
        }

        /// <summary>
        /// Create a partition table from a parent container
        /// </summary>
        /// <param name="container">The container that contains the partition table</param>
        protected PartitionTable(Container container)
        {
            _container = container;
        }

        /// <summary>
        /// Load partition entries for the table
        /// </summary>
        /// <returns>A list of partition entries</returns>
        protected abstract IEnumerable<PartitionEntry> LoadPartitions();
    }
}
