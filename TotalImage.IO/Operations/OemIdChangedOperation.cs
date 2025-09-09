using System;
using TotalImage.Partitions;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for changing the OEM ID of a boot sector.
    /// </summary>
    public class OemIdChangedOperation : Operation
    {
        /// <summary>
        /// The OEM ID before this operation.
        /// </summary>
        public string OldOemId { get; }

        /// <summary>
        /// The OEM ID after this operation.
        /// </summary>
        public string NewOemId { get; }

        /// <summary>
        /// An operation for changing the OEM ID of a boot sector.
        /// </summary>
        /// <param name="targetObject">The partition entry of the partition whose OEM ID is to be changed.</param>
        /// <param name="oldOemId">Old OEM ID.</param>
        /// <param name="newOemId">New OEM ID.</param>
        /// <param name="timestamp">The date and time when the OEM ID was modified.</param>
        public OemIdChangedOperation(PartitionEntry targetObject, string oldOemId, string newOemId, DateTime timestamp) : base(targetObject, timestamp)
        {
            if (string.IsNullOrWhiteSpace(oldOemId))
            {
                throw new ArgumentException("Old OEM ID cannot be null, empty or whitespace!", nameof(oldOemId));
            }

            if (string.IsNullOrWhiteSpace(newOemId))
            {
                throw new ArgumentException("New OEM ID cannot be null, empty or whitespace!", nameof(newOemId));
            }

            OldOemId = oldOemId;
            NewOemId = newOemId;
        }
    }
}
