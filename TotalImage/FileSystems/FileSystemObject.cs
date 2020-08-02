using System;
using System.IO;

namespace TotalImage.FileSystems
{
    /*
     * The most basic building block for a file system browser.
     */
    public abstract class FileSystemObject
    {
        FileSystem fileSystem;

        protected FileSystemObject(FileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public abstract string Name { get; set; }
        public abstract string FullName { get; }

        public abstract FileAttributes Attributes { get; set; }

        public abstract DateTime? LastAccessTime { get; set; }
        public abstract DateTime? LastWriteTime { get; set; }
        public abstract DateTime? CreationTime { get; set; }

        public FileSystem FileSystem => fileSystem;

        public abstract ulong Length { get; set; }

        public abstract void Delete();
        public abstract void MoveTo(string path);
        public void Rename(string name) => Name = name;

        public bool IsReadOnly
        {
            get => (Attributes & FileAttributes.ReadOnly) > 0;
            set => Attributes = Attributes | (value ? FileAttributes.ReadOnly : 0);
        }

        public override string ToString() => FullName;
    }
}
