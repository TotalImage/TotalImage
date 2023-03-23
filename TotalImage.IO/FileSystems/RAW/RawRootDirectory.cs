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
        /// <summary>
        /// Create a placeholder root directory for a RAW file system
        /// </summary>
        /// <param name="fileSystem">The RAW file system</param>
        public RawRootDirectory(RawFileSystem fileSystem) : base(fileSystem, null)
        {
        }

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
        public override long Length
        {
            get => 0;
            set { return; }
        }

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
        public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden, bool showDeleted)
            => Array.Empty<FileSystemObject>();

        /// <inheritdoc />
        public override long GetFileCount(bool recursive)
            => 0;

        /// <inheritdoc />
        public override void MoveTo(string path)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override long GetSize(bool recursive, bool sizeOnDisk)
            => 0;

        /// <inheritdoc />
        public override long GetSubdirectoryCount(bool recursive)
            => 0;
    }
}
