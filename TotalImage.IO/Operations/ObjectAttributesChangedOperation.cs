using System.IO;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for changing the attributes of a file system object.
    /// </summary>
    /// <param name="targetObject">The file system object whose attributes are to be changed.</param>
    /// <param name="oldAttributes">Old attributes of the file system object.</param>
    /// <param name="newAttributes">New attributes of the file system object.</param>
    public class ObjectAttributesChangedOperation(TiFileSystemObject targetObject, FileAttributes oldAttributes, FileAttributes newAttributes) : Operation(targetObject)
    {
        /// <summary>
        /// The attributes of the target object before this operation.
        /// </summary>
        public FileAttributes OldAttributes { get; } = oldAttributes;
        /// <summary>
        /// The attributes of the target object after this operation.
        /// </summary>
        public FileAttributes NewAttributes { get; } = newAttributes;
    }
}
