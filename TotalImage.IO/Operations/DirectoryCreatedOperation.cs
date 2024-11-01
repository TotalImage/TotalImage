using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for creating a new directory.
    /// </summary>
    /// <param name="targetObject">The parent directory of the directory to be created.</param>
    /// <param name="dirName">The name of the directory to be created.</param>
    public class DirectoryCreatedOperation(TiFileSystemObject targetObject, string dirName) : Operation(targetObject)
    {
        /// <summary>
        /// The name of the directory that was created.
        /// </summary>
        public string DirectoryName = dirName;
    }
}
