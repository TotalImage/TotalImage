using System;
using System.Collections.Generic;
using System.IO;
using TotalImage.FileSystems;

namespace TotalImage
{
    /// <summary>
    /// A read-only stand-in for a file that has been injected into the pending change set
    /// but does not yet exist on disk. Used as the <c>Tag</c> on synthetic list-view items
    /// so that all existing UI code paths can safely cast <c>Tag</c> to
    /// <see cref="FileSystemObject"/> without null checks.
    /// </summary>
    internal sealed class PendingFile : FileSystemObject
    {
        private readonly string _name;
        private readonly ulong _length;
        private readonly DateTime _lastWriteTime;

        public PendingFile(string name, ulong length, DateTime lastWriteTime)
            : base(null!)   // no real FileSystem backing
        {
            _name = name;
            _length = length;
            _lastWriteTime = lastWriteTime;
        }

        public override string Name
        {
            get => _name;
            set => throw new NotSupportedException("Pending items are read-only.");
        }

        public override string FullName => _name;

        public override FileAttributes Attributes
        {
            get => FileAttributes.Normal;
            set => throw new NotSupportedException("Pending items are read-only.");
        }

        public override DateTime? LastAccessTime
        {
            get => _lastWriteTime;
            set => throw new NotSupportedException("Pending items are read-only.");
        }

        public override DateTime? LastWriteTime
        {
            get => _lastWriteTime;
            set => throw new NotSupportedException("Pending items are read-only.");
        }

        public override DateTime? CreationTime
        {
            get => _lastWriteTime;
            set => throw new NotSupportedException("Pending items are read-only.");
        }

        public override ulong Length
        {
            get => _length;
            set => throw new NotSupportedException("Pending items are read-only.");
        }

        public override ulong LengthOnDisk => _length;

        public override void Delete() => throw new NotSupportedException("Pending items are read-only.");
        public override void MoveTo(string path) => throw new NotSupportedException("Pending items are read-only.");
    }

    /// <summary>
    /// A read-only stand-in for a directory that has been created in the pending change set
    /// but does not yet exist on disk. Used as the <c>Tag</c> on synthetic list-view items.
    /// </summary>
    internal sealed class PendingDirectory : FileSystems.Directory
    {
        private readonly string _name;
        private readonly string _fullName;

        /// <param name="name">The directory's short name (e.g. "NEWFOLDER").</param>
        /// <param name="parentFullName">FullName of the parent directory (e.g. "\" or "\SUB").
        /// Used to construct a correct FullName so that FindNode can match this node.</param>
        public PendingDirectory(string name, string parentFullName)
            : base(null!, null)   // no real FileSystem or parent
        {
            _name = name;
            _fullName = System.IO.Path.Combine(parentFullName, name);
        }

        public override string Name
        {
            get => _name;
            set => throw new NotSupportedException("Pending items are read-only.");
        }

        // Override FullName: base implementation walks Parent (null here) which would throw.
        public override string FullName => _fullName;

        public override FileAttributes Attributes
        {
            get => FileAttributes.Directory;
            set => throw new NotSupportedException("Pending items are read-only.");
        }

        public override DateTime? LastAccessTime
        {
            get => null;
            set => throw new NotSupportedException("Pending items are read-only.");
        }

        public override DateTime? LastWriteTime
        {
            get => null;
            set => throw new NotSupportedException("Pending items are read-only.");
        }

        public override DateTime? CreationTime
        {
            get => null;
            set => throw new NotSupportedException("Pending items are read-only.");
        }

        public override ulong Length
        {
            get => 0;
            set => throw new NotSupportedException("Pending items are read-only.");
        }

        public override ulong LengthOnDisk => 0;

        public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden)
            => Array.Empty<FileSystemObject>();

        public override FileSystems.Directory CreateSubdirectory(string path)
            => throw new NotSupportedException("Pending items are read-only.");

        public override void Delete() => throw new NotSupportedException("Pending items are read-only.");
        public override void MoveTo(string path) => throw new NotSupportedException("Pending items are read-only.");
    }
}
