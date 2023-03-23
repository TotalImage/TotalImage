using System;
using System.IO;

namespace TotalImage.FileSystems
{
    /// <summary>
    /// Represents an object stored within a file system
    /// </summary>
    public abstract class FileSystemObject
    {
        private readonly FileSystem _fileSystem;

        /// <summary>
        /// Create an object on a file system
        /// </summary>
        /// <param name="fileSystem">The file system that contains the object</param>
        protected FileSystemObject(FileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// The name of the object on the file system
        /// </summary>
        public abstract string Name { get; set; }

        /// <summary>
        /// The fully-qualified name of the object on the file system
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// The attributes of the file system object
        /// </summary>
        public abstract FileAttributes Attributes { get; set; }

        /// <summary>
        /// The last access time of the file system object
        /// </summary>
        public abstract DateTime? LastAccessTime { get; set; }

        /// <summary>
        /// The last write time of the file system object
        /// </summary>
        public abstract DateTime? LastWriteTime { get; set; }

        /// <summary>
        /// The creation time of the file system object
        /// </summary>
        public abstract DateTime? CreationTime { get; set; }

        /// <summary>
        /// The file system that contains the file system object
        /// </summary>
        public FileSystem FileSystem => _fileSystem;

        /// <summary>
        /// The length of the file system object
        /// </summary>
        public abstract long Length { get; set; }

        /// <summary>
        /// The length of the file system object as represented on the disk
        /// </summary>
        public virtual long LengthOnDisk => ((Length / FileSystem.AllocationUnitSize) + 1) * FileSystem.AllocationUnitSize;

        /// <summary>
        /// Delete a file system object
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// Move the location of a file system object
        /// </summary>
        /// <param name="path">The new location of the file system object</param>
        public abstract void MoveTo(string path);

        /// <summary>
        /// Rename a file system object
        /// </summary>
        /// <param name="name">The new name of the file system object</param>
        public void Rename(string name) => Name = name;

        /// <summary>
        /// Indicates if the file system object is read only
        /// </summary>
        public bool IsReadOnly
        {
            get => (Attributes & FileAttributes.ReadOnly) > 0;
            set => Attributes |= value ? FileAttributes.ReadOnly : 0;
        }

        /// <inheritdoc />
        public override string ToString() => FullName;
    }
}
