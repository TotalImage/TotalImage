using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TotalImage.FileSystems.ISO
{
    /// <summary>
    /// Represents a directory on an ISO 9660 file system
    /// </summary>
    public class IsoDirectory : Directory
    {
        /// <summary>
        /// The internal file system record for the directory
        /// </summary>
        protected IsoFileSystemObject Record { get; }

        /// <inheritdoc />
        public override string Name
        {
            get => string.IsNullOrEmpty(Record.FileIdentifierExtension)
                ? Record.FileIdentifierName
                : $"{Record.FileIdentifierName}.{Record.FileIdentifierExtension}";
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override FileAttributes Attributes { get => FileAttributes.Directory; set => throw new NotImplementedException(); }

        /// <inheritdoc />
        public override DateTime? LastAccessTime { get => null; set => throw new NotImplementedException(); }

        /// <inheritdoc />
        public override DateTime? LastWriteTime { get => Record.RecordingDate?.LocalDateTime.ToLocalTime(); set => throw new NotImplementedException(); }

        /// <inheritdoc />
        public override DateTime? CreationTime { get => null; set => throw new NotImplementedException(); }

        /// <inheritdoc />
        public override ulong Length { get => Record.DataLength; set => throw new NotImplementedException(); }

        /// <summary>
        /// Create a directory on an ISO 9660 file system from a file record
        /// </summary>
        /// <param name="fso">The file system record</param>
        /// <param name="fileSystem">The file system containing the directory</param>
        /// <param name="parent">The parent directory, if any</param>
        public IsoDirectory(IsoFileSystemObject fso, FileSystem fileSystem, Directory? parent = null) : base(fileSystem, parent)
        {
            Record = fso;
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
        public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden, bool showDeleted)
        {
            Iso9660FileSystem fileSystem = (Iso9660FileSystem)FileSystem;
            var stream = FileSystem.GetStream();

            stream.Seek(fileSystem.PrimaryVolumeDescriptor.LogicalBlockSize * Record.ExtentOffset, SeekOrigin.Begin);

            byte[] records = new byte[Record.DataLength];
            stream.Read(records);

            int nextRecord = 0;
            while (nextRecord < Record.DataLength)
            {
                byte recordLength = records[nextRecord];
                if (recordLength == 0)
                {
                    // records can not cross sector boundaries, we probably just need to jump up to the next sector
                    nextRecord = ((nextRecord / 2048) + 1) * 2048;
                    continue;
                }

                IsoFileSystemObject record = new IsoFileSystemObject(records[nextRecord..(nextRecord + recordLength)], fileSystem.PrimaryVolumeDescriptor.IsJolietVolumeDescriptor, fileSystem.PrimaryVolumeDescriptor.Identifier.SequenceEqual(IsoVolumeDescriptor.HsfStandardIdentifier));

                // A record whose identifier is a single zero byte is the current directory
                // A record whose identifier is a single one byte is either the parent directory or the root directory if it is the root directory
                if (record.FileIdentifierName != "\u0000" && record.FileIdentifierName != "\u0001")
                {
                    if (record.FileFlags.HasFlag(IsoFileFlags.Directory))
                    {
                        yield return new IsoDirectory(record, fileSystem, this);
                    }
                    else
                    {
                        yield return new IsoFile(record, fileSystem, this);
                    }
                }

                nextRecord += recordLength;
            }
        }

        /// <inheritdoc />
        public override void MoveTo(string path)
        {
            throw new NotImplementedException();
        }
    }
}
