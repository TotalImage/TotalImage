using System;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for erasing a file system object.
    /// </summary>
    /// <param name="targetObject">The file system object to be erased.</param>
    /// <param name="timestamp">The date and time when this object was erased.</param>
    public class ObjectErasedOperation(TiFileSystemObject targetObject, DateTime timestamp) : Operation(targetObject, timestamp)
    {

    }
}
