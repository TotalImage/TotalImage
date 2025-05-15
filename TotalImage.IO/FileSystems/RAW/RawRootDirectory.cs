using System;
using System.Collections.Generic;
using System.IO;

namespace TotalImage.FileSystems.RAW
{
    /// <summary>
    /// Reprents the root directory of a RAW file system
    /// </summary>
    public class RawRootDirectory : Directory
    {
        /// <inheritdoc />
        public override string Name
        {
            get => "";
            set { return; }
        }

        /// <inheritdoc />
        public override FileAttributes Attributes
        {
            get => FileAttributes.Directory;
            set { return; }
        }

        /// <inheritdoc />
        public override DateTime? LastAccessTime
        {
            get => null;
            set { return; }
        }

        /// <inheritdoc />
        public override DateTime? LastWriteTime
        {
            get => null;
            set { return; }
        }

        /// <inheritdoc />
        public override DateTime? CreationTime
        {
            get => null;
            set { return; }
        }

        /// <inheritdoc />
        public override ulong Length
        {
            get => 0;
            set { return; }
        }

        /// <summary>
        /// Create a placeholder root directory for a RAW file system
        /// </summary>
        /// <param name="fileSystem">The RAW file system</param>
        public RawRootDirectory(RawFileSystem fileSystem) : base(fileSystem, null) { }    

        /// <inheritdoc />
        public override Directory CreateSubdirectory(string path)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override void Delete()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden)
            => Array.Empty<FileSystemObject>();

        /// <inheritdoc />
        public override void MoveTo(string path)
        {
            throw new NotSupportedException();
        }
    }
}
