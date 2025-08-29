using System;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for changing the last access time of a file system object.
    /// </summary>
    /// <param name="targetObject">The file system object whose last access time is to be changed.</param>
    /// <param name="oldTime">Old last access time of the file system object.</param>
    /// <param name="newTime">New last access time of the file system object.</param>
    /// <param name="timestamp">The date and time when the file system object last access time was modified.</param>
    public class ObjectLastAccessTimeChangedOperation(TiFileSystemObject targetObject, DateTime oldTime, DateTime newTime, DateTime timestamp) : Operation(targetObject, timestamp)
    {
        /// <summary>
        /// The last access time of the target object before this operation.
        /// </summary>
        public DateTime OldLastAccessTime { get; } = oldTime;

        /// <summary>
        /// The last access time of the target object after this operation.
        /// </summary>
        public DateTime NewLastAccessTime { get; } = newTime;
    }
}
