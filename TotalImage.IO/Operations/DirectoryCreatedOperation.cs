using System;
using TiDirectory = TotalImage.FileSystems.Directory;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for creating a new directory.
    /// </summary>
    /// <param name="targetObject">The parent directory of the directory to be created.</param>
    /// <param name="dirName">The name of the directory to be created.</param>
    /// <param name="timestamp">The date and time when the directory was created.</param>
    public class DirectoryCreatedOperation(TiDirectory targetObject, string dirName, DateTime timestamp) : Operation(targetObject, timestamp)
    {
        /// <summary>
        /// The name of the directory that was created.
        /// </summary>
        public string DirectoryName { get; } = dirName;
    }
}
