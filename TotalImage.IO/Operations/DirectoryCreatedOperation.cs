using TiDirectory = TotalImage.FileSystems.Directory;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for creating a new directory.
    /// </summary>
    /// <param name="targetObject">The parent directory of the directory to be created.</param>
    /// <param name="dirName">The name of the directory to be created.</param>
    public class DirectoryCreatedOperation(TiDirectory targetObject, string dirName) : Operation(targetObject)
    {
        /// <summary>
        /// The name of the directory that was created.
        /// </summary>
        public string DirectoryName { get; } = dirName;
    }
}
