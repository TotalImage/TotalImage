using System;
using TotalImage.Partitions;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for deleting a partition in the image.
    /// </summary>
    /// <param name="targetObject">The partition to be deleted.</param>
    /// <param name="timestamp">The date and time when the partition was deleted.</param>
    public class PartitionDeletedOperation(PartitionEntry targetObject, DateTime timestamp) : Operation(targetObject, timestamp) 
    {

    }
}
