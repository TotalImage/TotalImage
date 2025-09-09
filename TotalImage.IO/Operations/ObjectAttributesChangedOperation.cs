using System;
using System.IO;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for changing the attributes of a file system object.
    /// </summary>

    public class ObjectAttributesChangedOperation : Operation
    {
        /// <summary>
        /// The attributes of the target object before this operation.
        /// </summary>
        public FileAttributes OldAttributes { get; }
        /// <summary>
        /// The attributes of the target object after this operation.
        /// </summary>
        public FileAttributes NewAttributes { get; }

        /// <summary>
        /// Creates a new ObjectAttributesChangedOperation, if oldAttributes doesn't include read-only or if newAttributes clears it.
        /// </summary>
        /// <param name="targetObject">The file system object whose attributes are to be changed.</param>
        /// <param name="oldAttributes">Old attributes of the file system object.</param>
        /// <param name="newAttributes">New attributes of the file system object.</param>
        /// <param name="timestamp">The date and time when the file system object attributes were changed.</param>
        public ObjectAttributesChangedOperation(TiFileSystemObject targetObject, FileAttributes oldAttributes, FileAttributes newAttributes, DateTime timestamp) : base(targetObject, timestamp)
        {
            //If the old attributes have the read-only flag set and the new attributes don't clear it, the operation should fail since the only thing
            //that can be changed on a read-only object is the read-only flag...
            if(oldAttributes.HasFlag(FileAttributes.ReadOnly) && NewAttributes.HasFlag(FileAttributes.ReadOnly))
            {
                throw new InvalidOperationException("Cannot change attributes of a read-only object unless the read-only attribute is cleared first!");
            }

            OldAttributes = oldAttributes;
            NewAttributes = newAttributes;
        }
    }
}
