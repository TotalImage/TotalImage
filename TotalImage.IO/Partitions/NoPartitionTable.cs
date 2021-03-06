﻿using System.Collections.Generic;
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
        protected override IEnumerable<PartitionEntry> LoadPartitions()
        {
            return new[]
            {
                new PartitionEntry(0, _container.Length, _container.Content),
            };
        }
    }
}