using System.Collections.Generic;
using Path = System.IO.Path;

namespace TotalImage.FileSystems
{
    /// <summary>
    /// Represents a directory within a file system.
    /// </summary>
    public abstract class Directory : FileSystemObject
    {
        private readonly Directory? _parent;

        /// <summary>
        /// Create a directory on a filesystem
        /// </summary>
        /// <param name="fileSystem">The file system containing the directory</param>
        /// <param name="parent">The parent directory, if it exists</param>
        protected Directory(FileSystem fileSystem, Directory? parent) : base(fileSystem)
        {
            _parent = parent;
        }

        /// <summary>
        /// Get the parent directory, if it exists
        /// </summary>
        public Directory? Parent => _parent;

        /// <summary>
        /// Get the root directory of the file system
        /// </summary>
        public Directory Root => FileSystem.RootDirectory;

        /// <summary>
        /// Get the contents of a directory
        /// </summary>
        /// <param name="showHidden">Whether to show hidden objects</param>
        /// <param name="showDeleted">Whether to show deleted objects</param>
        /// <returns>An enumerable of file system objects contained within the directory</returns>
        public abstract IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden, bool showDeleted);

        /// <summary>
        /// Get directories contained within a directory
        /// </summary>
        /// <param name="showHidden">Whether to show hidden directories</param>
        /// <param name="showDeleted">Whether to show deleted directories</param>
        /// <returns>An enumerable of directories contained within the directory</returns>
        public IEnumerable<Directory> EnumerateDirectories(bool showHidden, bool showDeleted)
        {
            foreach (var obj in EnumerateFileSystemObjects(showHidden, showDeleted))
            {
                if (obj is Directory dObj)
                {
                    yield return dObj;
                }
            }
        }

        /// <summary>
        /// Get files contained within a directory
        /// </summary>
        /// <param name="showHidden">Whether to show hidden files</param>
        /// <param name="showDeleted">Whether to show deleted files</param>
        /// <returns>An enumerable of files contained within the directory</returns>
        public IEnumerable<File> EnumerateFiles(bool showHidden, bool showDeleted)
        {
            foreach (var obj in EnumerateFileSystemObjects(showHidden, showDeleted))
            {
                if (obj is File fObj)
                {
                    yield return fObj;
                }
            }
        }

        /// <summary>
        /// Get full path of directory on the file system
        /// </summary>
        public override string FullName
            => _parent == null
                ? Path.DirectorySeparatorChar.ToString()
                : Path.Combine(_parent.FullName, Name);

        /// <summary>
        /// Create a subdirectory within the directory
        /// </summary>
        /// <param name="path">Name of the subdirectory to create</param>
        /// <returns>The created directory</returns>
        public abstract Directory CreateSubdirectory(string path);

        /// <summary>
        /// Get the file count in a directory.
        /// </summary>
        /// <param name="recursive">Whether to enumerate subdirectories as well.</param>
        /// <returns>File count in a directory excluding subdirectories if recursive is false, otherwise file count in a directory including subdirectories.</returns>
        public abstract ulong FileCount(bool recursive);

        /// <summary>
        /// Get the subdirectory count in a directory.
        /// </summary>
        /// <param name="recursive">Whether to enumerate subdirectories as well.</param>
        /// <returns>Subdirectory count in a directory excluding subdirectory contents if recursive is false, otherwise subdirectory count in a directory including subdirectory contents.</returns>
        public abstract ulong SubdirectoryCount(bool recursive);

        /// <summary>
        /// Get the combined size of files in a directory.
        /// </summary>
        /// <param name="recursive">Whether to enumerate subdirectories as well.</param>
        /// <param name="sizeOnDisk">Whether to report actual file size or size on disk.</param>
        /// <returns>Combined size of files in a directory excluding subdirectories if recursive is false, otherwise combined size of files in a directory including subdirectories.</returns>
        public abstract ulong Size(bool recursive, bool sizeOnDisk);
    }
}
