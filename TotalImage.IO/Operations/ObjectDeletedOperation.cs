using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for deleting a file system object.
    /// </summary>
    /// <param name="targetObject">The file system object to be deleted.</param>
    public class ObjectDeletedOperation(TiFileSystemObject targetObject) : Operation(targetObject) { }
}
