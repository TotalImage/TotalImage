using System.IO;
using TiDirectory = TotalImage.FileSystems.Directory;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for injecting a file system object into the image.
    /// </summary>
    /// <param name="targetObject">The directory the object will be injected into.</param>
    /// <param name="injectedObject">FileSystemInfo for the object to be injected.</param>
    public class ObjectInjectedOperation(TiDirectory targetObject, FileSystemInfo injectedObject) : Operation(targetObject)
    {
        /// <summary>
        /// Basic information about the object that was injected.
        /// </summary>
        public FileSystemInfo InjectedObject { get; } = injectedObject;
    }
}
