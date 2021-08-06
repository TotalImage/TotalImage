using System.IO;

namespace TotalImage.FileSystems
{
    /// <summary>
    /// An interface to support creating file systems from saved images
    /// </summary>
    public interface IFileSystemFactory
    {
        /// <summary>
        /// Attempts to load a file system from a given steam
        /// </summary>
        /// <param name="stream">The stream containing a file system</param>
        /// <returns>The file system if load was successful, null if not.</returns>
        public FileSystem? TryLoadFileSystem(Stream stream);
    }
}
