using System;
using TiDirectory = TotalImage.FileSystems.Directory;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for moving a file system object within the image.
    /// </summary>
    public class ObjectMovedOperation : Operation
    {
        /// <summary>
        /// The target object's parent directory before this operation.
        /// </summary>
        public TiDirectory OldParentDirectory { get; }

        /// <summary>
        /// The target object's parent directory after this operation.
        /// </summary>
        public TiDirectory NewParentDirectory { get; }

        /// <summary>
        /// An operation for moving a file system object within the image.
        /// </summary>
        /// <param name="targetObject">The file system object to be moved.</param>
        /// <param name="oldParentDir">Old parent directory of the object to be moved.</param>
        /// <param name="newParentDir">New parent directory of the object to be moved.</param>
        /// <param name="timestamp">The date and time when the file system object was moved.</param>
        public ObjectMovedOperation(FileSystems.FileSystemObject targetObject, TiDirectory oldParentDir, TiDirectory newParentDir, DateTime timestamp) : base(targetObject, timestamp)
        {
            ArgumentNullException.ThrowIfNull(oldParentDir);
            ArgumentNullException.ThrowIfNull(newParentDir);
            
            //Cannot move to the same directory it's already in...
            if(oldParentDir.Equals(newParentDir))
            {
                throw new ArgumentException("newParentDir cannot be the same as oldParentDir!");
            }

            OldParentDirectory = oldParentDir;
            NewParentDirectory = newParentDir;
        }
    }
}
