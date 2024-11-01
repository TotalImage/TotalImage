using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for renaming a file system object.
    /// </summary>
    /// <param name="targetObject">The file system object to be renamed.</param>
    /// <param name="oldName">Old name of the file system object.</param>
    /// <param name="newName">New name of the file system object.</param>
    public class ObjectRenamedOperation(TiFileSystemObject targetObject, string oldName, string newName) : Operation(targetObject)
    {
        /// <summary>
        /// The name of the target object before this operation.
        /// </summary>
        public string OldName { get; } = oldName;

        /// <summary>
        /// The name of the target object after this operation.
        /// </summary>
        public string NewName { get; } = newName;
    }
}
