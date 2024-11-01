using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for erasing a file system object.
    /// </summary>
    /// <param name="targetObject">The file system object to be erased.</param>
    public class ObjectErasedOperation(TiFileSystemObject targetObject) : Operation(targetObject) { }
}
