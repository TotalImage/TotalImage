using System;
using System.IO;

namespace TotalImage.FileSystems.HSF
{
    /// <summary>
    /// Represents a file on a High Sierra file system
    /// </summary>
    public class HsfFile : File
    {
        /// <summary>
        /// The internal file system record for the file
        /// </summary>
        protected  HsfFileSystemObject Record { get; }

        /// <inheritdoc />
        public override string Name
        {
            get => string.IsNullOrEmpty(Record.FileIdentifierExtension)
                ? Record.FileIdentifierName
                : $"{Record.FileIdentifierName}.{Record.FileIdentifierExtension}";
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
            long offset = ((HighSierraFileSystem)FileSystem).PrimaryVolumeDescriptor.LogicalBlockSize * Record.ExtentOffset;
            var fileStream = new PartialStream(FileSystem.GetStream(), offset, Record.DataLength);
            return fileStream;
        }

        /// <summary>
        /// Create a file on a High Sierra file system from a file record
        /// </summary>
        /// <param name="fso">The file system record</param>
        /// <param name="fileSystem">The file system containing the file</param>
        /// <param name="parent">The parent directory</param>
        public HsfFile(HsfFileSystemObject fso, FileSystem fileSystem, Directory parent) : base(fileSystem, parent)
        {
            Record = fso;
        }
    }
}