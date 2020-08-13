using System.Collections.Generic;
using System.Linq;
using Path = System.IO.Path;

namespace TotalImage.FileSystems
{
    /*
     * Base directory class to be implemented by the file system handlers.
     */
    public abstract class Directory : FileSystemObject
    {
        private readonly Directory _parent;

        protected Directory(FileSystem fileSystem, Directory parent) : base(fileSystem)
        {
            this._parent = parent;
        }

        public Directory Parent => _parent;
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

        public override string FullName
            => _parent == null
                ? Name
                : Path.Combine(Parent.FullName, Name);

        public abstract Directory CreateSubdirectory(string path);
    }
}