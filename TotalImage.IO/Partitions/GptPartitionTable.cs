using System.Collections.Generic;
using TotalImage.Containers;

namespace TotalImage.Partitions
{
    /// <summary>
    /// A partition table class that represents a GPT partition table
    /// </summary>
    public class GptPartitionTable : PartitionTable
    {
        private readonly uint _sectorSize;

        /// <inheritdoc />
        public GptPartitionTable(Container container, uint sectorSize = 512) : base(container)
        {
            _sectorSize = sectorSize;
        }

        /// <inheritdoc />
        protected override IEnumerable<PartitionEntry> LoadPartitions()
        {
            throw new System.NotImplementedException();
        }
    }
}
