using System;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for renaming a file system object.
    /// </summary>
    public class ObjectRenamedOperation : Operation
    {
        /// <summary>
        /// The name of the target object before this operation.
        /// </summary>
        public string OldName { get; }

        /// <summary>
        /// The name of the target object after this operation.
        /// </summary>
        public string NewName { get; }

        /// <summary>
        /// An operation for renaming a file system object.
        /// </summary>
        /// <param name="targetObject">The file system object to be renamed.</param>
        /// <param name="oldName">Old name of the file system object.</param>
        /// <param name="newName">New name of the file system object.</param>
        /// <param name="timestamp">The date and time when the file system object was renamed.</param>
        public ObjectRenamedOperation(TiFileSystemObject targetObject, string oldName, string newName, DateTime timestamp) : base(targetObject, timestamp)
        {
            if (string.IsNullOrWhiteSpace(oldName))
            {
                throw new ArgumentException("Old name cannot be null, empty or whitespace!", nameof(oldName));
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("New name cannot be null, empty or whitespace!", nameof(newName));
            }

            OldName = oldName;
            NewName = newName;
        }
    }
}
