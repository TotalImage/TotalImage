using System;
using TotalImage.Partitions;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for replacing the bootsector of a partition.
    /// </summary>
    /// <param name="targetObject">The partition whose bootsector will be replaced.</param>
    /// <param name="bootsector">The bootsector to be added to the partition.</param>
    /// <param name="timestamp">The date and time when the bootsector was replaced.</param>
    public class BootsectorReplacedOperation(PartitionEntry targetObject, byte[] bootsector, DateTime timestamp) : Operation(targetObject, timestamp)
    {
        /// <summary>
        /// The bootsector to be added to the partition.
        /// </summary>
        public byte[] Bootsector { get; } = bootsector;
    }
}
