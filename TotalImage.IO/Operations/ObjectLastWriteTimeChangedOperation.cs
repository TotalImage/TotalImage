using System;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for changing the last write time of a file system object.
    /// </summary>
    /// <param name="targetObject">The file system object whose last write time is to be changed.</param>
    /// <param name="oldTime">Old last write time of the file system object.</param>
    /// <param name="newTime">New last write time of the file system object.</param>
    /// <param name="timestamp">The date and time when the file system object last write time was modified.</param>
    public class ObjectLastWriteTimeChangedOperation(TiFileSystemObject targetObject, DateTime oldTime, DateTime newTime, DateTime timestamp) : Operation(targetObject, timestamp)
    {
        /// <summary>
        /// The last write time of the target object before this operation.
        /// </summary>
        public DateTime OldLastWriteTime { get; } = oldTime;

        /// <summary>
        /// The last write time of the target object after this operation.
        /// </summary>
        public DateTime NewLastWriteTime { get; } = newTime;
    }
}
