using System.Collections.Generic;
using System.Linq;
using static System.IO.Path;

namespace TotalImage.FileSystems
{
    public abstract class Directory : FileSystemObject
    {
        protected Directory() { }

        public abstract Directory Parent { get; }
        public abstract Directory Root { get; }

        public abstract IEnumerable<FileSystemObject> EnumerateFileSystemObjects();

        public IEnumerable<Directory> EnumerateDirectories()
            => from x in EnumerateFileSystemObjects()
                where x is Directory
                select x as Directory;

        public IEnumerable<File> EnumerateFiles()
            => from x in EnumerateFileSystemObjects()
                where x is File
                select x as File;

        public FileSystemObject[] GetFileSystemObjects()
            => EnumerateFileSystemObjects().ToArray();

        public Directory[] GetDirectories()
            => EnumerateDirectories().ToArray();

        public File[] GetFiles()
            => EnumerateFiles().ToArray();

        public override string FullName
        {
            get
            {
                var path = new List<string>();
                var currentDir = this;

                while((currentDir = currentDir.Parent) != null)
                    path.Add(currentDir.Name);

                path.Reverse();

                return DirectorySeparatorChar + string.Join(DirectorySeparatorChar.ToString(), path);
            }
        }

        public abstract Directory CreateSubdirectory(string path);
    }
}