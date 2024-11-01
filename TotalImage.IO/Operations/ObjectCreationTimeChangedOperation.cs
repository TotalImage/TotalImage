using System;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for changing the creation time of a file system object.
    /// </summary>
    /// <param name="targetObject">The file system object whose creation time is to be changed.</param>
    /// <param name="oldTime">Old creation time of the file system object.</param>
    /// <param name="newTime">New creation time of the file system object.</param>
    public class ObjectCreationTimeChangedOperation(TiFileSystemObject targetObject, DateTime oldTime, DateTime newTime) : Operation(targetObject)
    {
        /// <summary>
        /// The creation time of the target object before this operation.
        /// </summary>
        public DateTime OldCreationTime { get; } = oldTime;

        /// <summary>
        /// The creation time of the target object after this operation.
        /// </summary>
        public DateTime NewCreationTime { get; } = newTime;
    }
}
