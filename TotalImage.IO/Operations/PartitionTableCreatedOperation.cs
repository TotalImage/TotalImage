using System;
using TotalImage.Partitions;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for creating a partition table in the image.
    /// </summary>
    /// <param name="targetObject">The partition table to be created.</param>
    /// <param name="timestamp">The date and time when the partition table was created.</param>
    public class PartitionTableCreatedOperation(PartitionTable targetObject, DateTime timestamp) : Operation(targetObject, timestamp) 
    {

    }
}
