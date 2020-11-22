using Path = System.IO.Path;

namespace TotalImage.FileSystems
{
    /*
     * Base file class to be implemented by the file system handlers.
     */
    public abstract class File : FileSystemObject
    {
        private readonly Directory _directory;

        protected File(FileSystem fileSystem, Directory directory) : base(fileSystem)
        {
            this._directory = directory;
        }

        public virtual string Extension => Path.GetExtension(Name);

        public Directory Directory => _directory;

        public string DirectoryName => Directory.FullName;

        public override string FullName => Path.Combine(DirectoryName, Name);
    }
}