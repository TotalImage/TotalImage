using System.Linq;
using static System.IO.Path;

namespace TotalImage.FileSystems
{
    public abstract class File : FileSystemObject
    {
        Directory directory;

        protected File(Directory directory)
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