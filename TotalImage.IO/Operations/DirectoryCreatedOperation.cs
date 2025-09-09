using System;
using TiDirectory = TotalImage.FileSystems.Directory;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for creating a new directory, if the file system supports subdirectories.
    /// </summary>

    public class DirectoryCreatedOperation : Operation
    {
        /// <summary>
        /// The name of the directory that was created.
        /// </summary>
        public string DirectoryName { get; }

        /// <summary>
        /// Creates a new DirectoryCreatedOperation with the provided arguments, if the current file system supports subdirectories.
        /// </summary>
        /// <param name="targetObject">The parent directory of the directory to be created.</param>
        /// <param name="dirName">The name of the directory to be created.</param>
        /// <param name="timestamp">The date and time when the directory was created.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public DirectoryCreatedOperation(TiDirectory targetObject, string dirName, DateTime timestamp) : base(targetObject, timestamp)
        {
            if (!targetObject.FileSystem.SupportsSubdirectories)
            {
                throw new InvalidOperationException("This file system does not support subdirectories!");
            }

            DirectoryName = dirName;
        }
    }
}
