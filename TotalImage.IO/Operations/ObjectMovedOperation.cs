using TiDirectory = TotalImage.FileSystems.Directory;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for moving a file system object within the image.
    /// </summary>
    /// <param name="targetObject">The file system object to be moved.</param>
    /// <param name="oldParentDir">Old parent directory of the object to be moved.</param>
    /// <param name="newParentDir">New parent directory of the object to be moved.</param>
    public class ObjectMovedOperation(FileSystems.FileSystemObject targetObject, TiDirectory oldParentDir, TiDirectory newParentDir) : Operation(targetObject)
    {
        /// <summary>
        /// The target object's parent directory before this operation.
        /// </summary>
        public TiDirectory OldParentDirectory { get; } = oldParentDir;

        /// <summary>
        /// The target object's parent directory after this operation.
        /// </summary>
        public TiDirectory NewParentDirectory { get; } = newParentDir;
    }
}
