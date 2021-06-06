using System;
using System.IO;

namespace TotalImage.FileSystems.ISO
{
    /// <summary>
    /// Represents a file on an ISO 9660 file system
    /// </summary>
    public class IsoFile : File
    {
        /// <summary>
        /// The internal file system record for the file
        /// </summary>
        protected  IsoFileSystemObject Record { get; }

        /// <inheritdoc />
        public override string Name
        {
            get => Record.FileIdentifier;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override FileAttributes Attributes
        {
            get => 0;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? LastAccessTime
        {
            get => null;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? LastWriteTime
        {
            get => Record.RecordingDate?.LocalDateTime.ToLocalTime();
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? CreationTime
        {
            get => null;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ulong Length
        {
            get => Record.DataLength;
            set => throw new NotImplementedException();
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
            long offset = ((Iso9660FileSystem)FileSystem).PrimaryVolumeDescriptor.LogicalBlockSize * Record.ExtentOffset;
            var fileStream = new PartialStream(FileSystem.GetStream(), offset, Record.DataLength);
            return fileStream;
        }

        /// <summary>
        /// Create a file on an ISO 9660 file system from a file record
        /// </summary>
        /// <param name="fso">The file system record</param>
        /// <param name="fileSystem">The file system containing the file</param>
        /// <param name="parent">The parent directory, if any</param>
        public IsoFile(IsoFileSystemObject fso, FileSystem fileSystem, Directory? parent = null) : base(fileSystem, parent)
        {
            Record = fso;
        }
    }
}