using System;
using TotalImage.FileSystems;
using TotalImage.Partitions;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for formatting a partition in the image.
    /// </summary>
    /// <param name="targetObject">The partition to be formatted.</param>
    /// <param name="oldFileSystem">Old file system of the partition.</param>
    /// <param name="newFileSystem">New file system of the partition.</param>
    /// <param name="timestamp">The date and time when the partition was formatted.</param>
    public class PartitionFormattedOperation(PartitionEntry targetObject, FileSystem oldFileSystem, FileSystem newFileSystem, DateTime timestamp) : Operation(targetObject, timestamp) 
    {
        /// <summary>
        /// The file system of this partition before the operation.
        /// </summary>
        public FileSystem OldFileSystem { get;  } = oldFileSystem;

        /// <summary>
        /// The file system of this partition after the operation.
        /// </summary>
        public FileSystem NewFileSystem { get; } = newFileSystem;
    }
}
