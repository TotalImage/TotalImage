using System.Linq;
using static System.IO.Path;

namespace TotalImage.FileSystems
{
    /*
     * Base file class to be implemented by the file system handlers.
     */
    public abstract class File : FileSystemObject
    {
        Directory directory;

        protected File(FileSystem fileSystem, Directory directory) : base(fileSystem)
        {
            this.directory = directory;
        }

        public virtual string Extension
        {
            get => Name.Split('.').Last();
        }

        public Directory Directory => directory;

        public string DirectoryName
        {
            get
            {
                return Directory.FullName;
            }
        }

        public override string FullName
        {
            get
            {
                return DirectoryName + DirectorySeparatorChar + Name;
            }
        }
    }
}