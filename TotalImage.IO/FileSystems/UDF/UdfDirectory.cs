using System;
using System.Collections.Generic;
using System.IO;

namespace TotalImage.FileSystems.UDF
{
    /// <summary>
    /// Represents a directory on a UDF file system.
    /// </summary>
    public class UdfDirectory : Directory
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
            get => FileAttributes.Directory | FileAttributes.ReadOnly | (_isHidden ? FileAttributes.Hidden : 0);
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
        /// Create a directory on a UDF file system.
        /// </summary>
        /// <param name="name">Directory name</param>
        /// <param name="entry">Directory entry metadata</param>
        /// <param name="fileSystem">The containing file system</param>
        /// <param name="parent">The parent directory, if any</param>
        /// <param name="isHidden">Whether the directory is hidden</param>
        internal UdfDirectory(string name, UdfFileEntryInfo entry, FileSystem fileSystem, Directory? parent, bool isHidden)
            : base(fileSystem, parent)
        {
            _name = name;
            _entry = entry;
            _isHidden = isHidden;
        }

        /// <inheritdoc />
        public override Directory CreateSubdirectory(string path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Delete()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden)
        {
            var fileSystem = (UdfFileSystem)FileSystem;
            foreach (UdfFileIdentifierDescriptorInfo child in fileSystem.EnumerateDirectoryEntries(_entry))
            {
                if (child.IsParent)
                {
                    continue;
                }

                if (!showHidden && child.IsHidden)
                {
                    continue;
                }

                if (child.IsDeleted)
                {
                    continue;
                }

                UdfFileEntryInfo childEntry = fileSystem.ReadFileEntry(child.Icb);
                if (child.IsDirectory || childEntry.FileType == UdfUtilities.IcbFileTypeDirectory)
                {
                    yield return new UdfDirectory(child.Name, childEntry, fileSystem, this, child.IsHidden);
                }
                else
                {
                    yield return new UdfFile(child.Name, childEntry, fileSystem, this, child.IsHidden);
                }
            }
        }

        /// <inheritdoc />
        public override void MoveTo(string path)
        {
            throw new NotImplementedException();
        }
    }
}
