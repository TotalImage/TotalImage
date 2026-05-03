using System;
using System.IO;

namespace TotalImage.FileSystems.UDF
{
    /// <summary>
    /// Represents a file on a UDF file system.
    /// </summary>
    public class UdfFile : File
    {
        private readonly string _name;
        private readonly UdfFileEntryInfo _entry;
        private readonly bool _isHidden;

        /// <inheritdoc />
        public override string Name
        {
            get => _name;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override FileAttributes Attributes
        {
            get => FileAttributes.ReadOnly | (_isHidden ? FileAttributes.Hidden : 0);
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? LastAccessTime
        {
            get => _entry.LastAccessTime;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? LastWriteTime
        {
            get => _entry.LastWriteTime;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? CreationTime
        {
            get => _entry.CreationTime;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ulong Length
        {
            get => _entry.InformationLength;
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Create a file on a UDF file system.
        /// </summary>
        /// <param name="name">File name</param>
        /// <param name="entry">File entry metadata</param>
        /// <param name="fileSystem">The containing file system</param>
        /// <param name="parent">The parent directory</param>
        /// <param name="isHidden">Whether the file is hidden</param>
        internal UdfFile(string name, UdfFileEntryInfo entry, FileSystem fileSystem, Directory parent, bool isHidden)
            : base(fileSystem, parent)
        {
            _name = name;
            _entry = entry;
            _isHidden = isHidden;
        }

        /// <inheritdoc />
        public override void Delete()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void MoveTo(string path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Stream GetStream()
        {
            return ((UdfFileSystem)FileSystem).OpenContentStream(_entry);
        }
    }
}
