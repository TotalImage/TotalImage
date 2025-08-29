using System;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for deleting a file system object.
    /// </summary>
    /// <param name="targetObject">The file system object to be deleted.</param>
    /// <param name="timestamp">The date and time when this object was deleted.</param>
    public class ObjectDeletedOperation(TiFileSystemObject targetObject, DateTime timestamp) : Operation(targetObject, timestamp) { }
}
