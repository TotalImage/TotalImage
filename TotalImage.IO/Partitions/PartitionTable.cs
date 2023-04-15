using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using TotalImage.Containers;

namespace TotalImage.Partitions
{
    /// <summary>
    /// An abstract representation of a partition table
    /// </summary>
    public abstract class PartitionTable
    {
        private static readonly ImmutableArray<IPartitionTableFactory> _knownFactories = ImmutableArray.Create<IPartitionTableFactory>(
            new MbrGptFactory()
        );

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

        /// <summary>
        /// Attempt to detect the partition table of a container using known partition table types
        /// </summary>
        /// <param name="container">The container containing a partition table</param>
        /// <returns>A partition table if detection was successful, null if not.</returns>
        public static PartitionTable AttemptDetection(Container container)
        {
            foreach (var factory in _knownFactories)
            {
                var result = factory.TryLoadPartitionTable(container);
                if (result != null)
                {
                    return result;
                }
            }

            return new NoPartitionTable(container);
        }
    }
}
