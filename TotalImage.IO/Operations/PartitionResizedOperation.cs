using System;
using TotalImage.Partitions;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for resizing a partition in the image.
    /// </summary>
    /// <param name="targetObject">The partition to be resized.</param>
    /// <param name="oldSize">Old size of the partition.</param>
    /// <param name="newSize">New size of the partition.</param>
    /// <param name="timestamp">The date and time when the partition was resized.</param>
    public class PartitionResizedOperation(PartitionEntry targetObject, long oldSize, long newSize, DateTime timestamp) : Operation(targetObject, timestamp) 
    {
        /// <summary>
        /// The old size of this partition before the operation.
        /// </summary>
        public long OldSize { get;  } = oldSize;

        /// <summary>
        /// The new size of this partition after the operation.
        /// </summary>
        public long NewSize { get; } = newSize;
    }
}
