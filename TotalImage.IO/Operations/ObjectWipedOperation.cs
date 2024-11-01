using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for wiping a file system object.
    /// </summary>
    /// <param name="targetObject">The file system object to be wiped.</param>
    public class ObjectWipedOperation(TiFileSystemObject targetObject) : Operation(targetObject) { }
}
