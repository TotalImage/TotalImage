using System;
using TotalImage.Partitions;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for resizing a partition in the image.
    /// </summary>
    public class PartitionResizedOperation : Operation
    {
        /// <summary>
        /// The old size of this partition before the operation.
        /// </summary>
        public long OldSize { get; }

        /// <summary>
        /// The new size of this partition after the operation.
        /// </summary>
        public long NewSize { get; }

        /// <summary>
        /// An operation for resizing a partition in the image.
        /// </summary>
        /// <param name="targetObject">The partition to be resized.</param>
        /// <param name="oldSize">Old size of the partition.</param>
        /// <param name="newSize">New size of the partition.</param>
        /// <param name="timestamp">The date and time when the partition was resized.</param>
        public PartitionResizedOperation(PartitionEntry targetObject, long oldSize, long newSize, DateTime timestamp) : base(targetObject, timestamp)
        {
            //Either size being 0 is indicative of a problem and not something we can work with in this operation...
            if (oldSize <= 0)
            {
                throw new ArgumentException("Old size cannot be 0 or less!", nameof(oldSize));
            }

            if (newSize <= 0)
            {
                throw new ArgumentException("New size cannot be 0 or less!", nameof(newSize));
            }

            OldSize = oldSize;
            NewSize = newSize;
        }
    }
}
