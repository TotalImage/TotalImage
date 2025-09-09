using System;
using System.IO;
using TiDirectory = TotalImage.FileSystems.Directory;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for injecting file system objects into the image.
    /// </summary>
    /// <param name="targetObject">The directory the objects will be injected into.</param>
    /// <param name="injectedObjects">FileSystemInfo for the objects to be injected.</param>
    /// <param name="timestamp">The date and time when the objects were injected.</param>
    public class ObjectsInjectedOperation(TiDirectory targetObject, FileSystemInfo[] injectedObjects, DateTime timestamp) : Operation(targetObject, timestamp)
    {
        /// <summary>
        /// Basic information about the objects that were injected.
        /// </summary>
        public FileSystemInfo[] InjectedObjects { get; } = injectedObjects ?? throw new ArgumentNullException("injectedObjects cannot be null!", nameof(injectedObjects));
    }
}
