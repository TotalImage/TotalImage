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
    public class PartitionFormattedOperation(PartitionEntry targetObject, FileSystem oldFileSystem, FileSystem newFileSystem) : Operation(targetObject) 
    {
        /// <summary>
        /// The file system of this partition before this operation.
        /// </summary>
        FileSystem OldFileSystem { get;  } = oldFileSystem;

        /// <summary>
        /// The file system of this partition after this operation.
        /// </summary>
        FileSystem NewFileSystem { get; } = newFileSystem;
    }
}
