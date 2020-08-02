using System.Collections.Generic;
using System.Linq;
using static System.IO.Path;

namespace TotalImage.FileSystems
{
    /*
     * Base directory class to be implemented by the file system handlers.
     */
    public abstract class Directory : FileSystemObject
    {
        Directory parent;

        protected Directory(FileSystem fileSystem, Directory parent) : base(fileSystem)
        {
            this.parent = parent;
        }

        public Directory Parent => parent;
        public Directory Root => FileSystem.RootDirectory;

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
            => Parent?.FullName + (Parent?.FullName.Last() == DirectorySeparatorChar ? "" : DirectorySeparatorChar.ToString()) + Name;

        public abstract Directory CreateSubdirectory(string path);
    }
}