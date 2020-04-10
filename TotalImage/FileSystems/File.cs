using System;
using System.Linq;
using static System.IO.Path;

namespace TotalImage.FileSystems
{
    public abstract class File : FileSystemObject
    {
        public virtual string Extension
        {
            get => Name.Split('.').Last();
        }

        public abstract Directory Directory { get; }

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