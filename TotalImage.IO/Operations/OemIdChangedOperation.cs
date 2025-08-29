using System;
using TotalImage.Partitions;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for changing the OEM ID of a boot sector.
    /// </summary>
    /// <param name="targetObject">The partition entry of the partition whose OEM ID is to be changed.</param>
    /// <param name="oldOemId">Old OEM ID.</param>
    /// <param name="newOemId">New OEM ID.</param>
    /// <param name="timestamp">The date and time when the OEM ID was modified.</param>
    public class OemIdChangedOperation(PartitionEntry targetObject, string oldOemId, string newOemId, DateTime timestamp) : Operation(targetObject, timestamp)
    {
        /// <summary>
        /// The OEM ID before this operation.
        /// </summary>
        public string OldOemId { get; } = oldOemId;

        /// <summary>
        /// The OEM ID after this operation.
        /// </summary>
        public string NewOemId { get; } = newOemId;
    }
}
