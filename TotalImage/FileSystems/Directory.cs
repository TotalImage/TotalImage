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

        public abstract IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool deleted);

        public IEnumerable<Directory> EnumerateDirectories(bool deleted)
            => from x in EnumerateFileSystemObjects(deleted)
                where x is Directory
                select x as Directory;

        public IEnumerable<File> EnumerateFiles(bool deleted)
            => from x in EnumerateFileSystemObjects(deleted)
                where x is File
                select x as File;

        public IEnumerable<FileSystemObject> EnumerateFileSystemObjects() => EnumerateFileSystemObjects(false);
        public IEnumerable<Directory> EnumerateDirectories() => EnumerateDirectories(false);
        public IEnumerable<File> EnumerateFiles() => EnumerateFiles(false);

        public FileSystemObject[] GetFileSystemObjects()
            => EnumerateFileSystemObjects().ToArray();

        public FileSystemObject[] GetFileSystemObjects(bool deleted)
            => EnumerateFileSystemObjects(deleted).ToArray();

        public Directory[] GetDirectories()
            => EnumerateDirectories().ToArray();

        public Directory[] GetDirectories(bool deleted)
            => EnumerateDirectories(deleted).ToArray();

        public File[] GetFiles()
            => EnumerateFiles().ToArray();

        public File[] GetFiles(bool deleted)
            => EnumerateFiles(deleted).ToArray();

        public override string FullName
            => Parent?.FullName + (Parent?.FullName.Last() == DirectorySeparatorChar ? "" : DirectorySeparatorChar.ToString()) + Name;

        public abstract Directory CreateSubdirectory(string path);
    }
}