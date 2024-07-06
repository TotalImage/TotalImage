using Path = System.IO.Path;

namespace TotalImage.FileSystems
{
    /// <summary>
    /// Represents a file within a file system
    /// </summary>
    public abstract class File : FileSystemObject
    {
        private readonly Directory _directory;

        /// <summary>
        /// The full path to the file
        /// </summary>
        public override string FullName => Path.Combine(DirectoryName, Name);

        /// <summary>
        /// The extension of the file
        /// </summary>
        public virtual string Extension => Path.GetExtension(Name);

        /// <summary>
        /// The directory containing the file
        /// </summary>
        public Directory Directory => _directory;

        /// <summary>
        /// The full path of the directory that contains the file
        /// </summary>
        public string DirectoryName => Directory.FullName;

        /// <summary>
        /// Create a file within a file system
        /// </summary>
        /// <param name="fileSystem">The file system containing the file</param>
        /// <param name="directory">The directory that contains the file</param>
        protected File(FileSystem fileSystem, Directory directory) : base(fileSystem)
        {
            _directory = directory;
        }

        /// <summary>
        /// Retrieves a stream of the file contents.
        /// </summary>
        public abstract System.IO.Stream GetStream();
    }
}
