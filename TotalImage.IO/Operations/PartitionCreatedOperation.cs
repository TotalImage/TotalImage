using System;
using TotalImage.Partitions;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for creating a partition in the image.
    /// </summary>
    /// <param name="targetObject">The partition to be created.</param>
    /// <param name="timestamp">The date and time when the partition was created.</param>
    public class PartitionCreatedOperation(PartitionEntry targetObject, DateTime timestamp) : Operation(targetObject, timestamp) 
    {

    }
}
