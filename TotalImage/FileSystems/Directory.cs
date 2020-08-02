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

        public abstract IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden, bool showDeleted);

        public IEnumerable<Directory> EnumerateDirectories(bool showHidden, bool showDeleted)
            => from x in EnumerateFileSystemObjects(showHidden, showDeleted)
                where x is Directory
                select x as Directory;

        public IEnumerable<File> EnumerateFiles(bool showHidden, bool showDeleted)
            => from x in EnumerateFileSystemObjects(showHidden, showDeleted)
                where x is File
                select x as File;

        public FileSystemObject[] GetFileSystemObjects(bool showHidden, bool showDeleted)
            => EnumerateFileSystemObjects(showHidden, showDeleted).ToArray();

        public Directory[] GetDirectories(bool showHidden, bool showDeleted)
            => EnumerateDirectories(showHidden, showDeleted).ToArray();

        public File[] GetFiles(bool showHidden, bool showDeleted)
            => EnumerateFiles(showHidden, showDeleted).ToArray();

        public override string FullName
            => Parent?.FullName + (Parent?.FullName.Last() == DirectorySeparatorChar ? "" : DirectorySeparatorChar.ToString()) + Name;

        public abstract Directory CreateSubdirectory(string path);
    }
}