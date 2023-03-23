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
        public override long Length { get => Record.DataLength; set => throw new NotImplementedException(); }

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

        /// <inheritdoc/>
        public override long GetFileCount(bool recursive)
        {
            long count = 0;
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

                var record = fileSystem.PrimaryVolumeDescriptor.Identifier.SequenceEqual(IsoVolumeDescriptor.HsfStandardIdentifier)
                    ? new IsoFileSystemObject(records[nextRecord..(nextRecord + recordLength)], fileSystem.PrimaryVolumeDescriptor.IsJolietVolumeDescriptor, true)
                    : new IsoFileSystemObject(records[nextRecord..(nextRecord + recordLength)], fileSystem.PrimaryVolumeDescriptor.IsJolietVolumeDescriptor, false);

                // A record whose identifier is a single zero byte is the current directory
                // A record whose identifier is a single one byte is either the parent directory or the root directory if it is the root directory
                if (record.FileIdentifierName != "\u0000" && record.FileIdentifierName != "\u0001")
                {
                    if (record.FileFlags.HasFlag(IsoFileFlags.Directory))
                    {
                        if (recursive)
                        {
                            count += new IsoDirectory(record, fileSystem, this).GetFileCount(recursive);
                        }
                        else
                            continue;
                    }
                    else
                    {
                        count++;
                    }
                }

                nextRecord += recordLength;
            }

            return count;
        }

        /// <inheritdoc/>
        public override long GetSize(bool recursive, bool sizeOnDisk)
        {
            long size = 0;
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

                //var record = new IsoFileSystemObject(records[nextRecord..(nextRecord + recordLength)], fileSystem.PrimaryVolumeDescriptor.IsJolietVolumeDescriptor);
                var record = fileSystem.PrimaryVolumeDescriptor.Identifier.SequenceEqual(IsoVolumeDescriptor.HsfStandardIdentifier) ? new IsoFileSystemObject(records[nextRecord..(nextRecord + recordLength)], fileSystem.PrimaryVolumeDescriptor.IsJolietVolumeDescriptor, true) : new IsoFileSystemObject(records[nextRecord..(nextRecord + recordLength)], fileSystem.PrimaryVolumeDescriptor.IsJolietVolumeDescriptor, false);

                // A record whose identifier is a single zero byte is the current directory
                // A record whose identifier is a single one byte is either the parent directory or the root directory if it is the root directory
                if (record.FileIdentifierName != "\u0000" && record.FileIdentifierName != "\u0001")
                {
                    if (record.FileFlags.HasFlag(IsoFileFlags.Directory))
                    {
                        if (recursive)
                        {
                            size += new IsoDirectory(record, fileSystem, this).GetSize(recursive, sizeOnDisk);
                        }
                        else
                            continue;
                    }
                    else
                    {
                        if (sizeOnDisk)
                        {
                            size += new IsoFile(record, fileSystem, this).LengthOnDisk;
                        }
                        else
                        {
                            size += new IsoFile(record, fileSystem, this).Length;
                        }
                    }
                }

                nextRecord += recordLength;
            }

            return size;
        }

        /// <inheritdoc/>
        public override long GetSubdirectoryCount(bool recursive)
        {
            long count = 0;
            Iso9660FileSystem fileSystem = (Iso9660FileSystem)FileSystem;
            var stream = FileSystem.GetStream();

            stream.Seek(fileSystem.PrimaryVolumeDescriptor.LogicalBlockSize * Record.ExtentOffset, SeekOrigin.Begin);

            Span<byte> records = new byte[Record.DataLength];
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

                //var record = new IsoFileSystemObject(records[nextRecord..(nextRecord + recordLength)], fileSystem.PrimaryVolumeDescriptor.IsJolietVolumeDescriptor);
                var record = fileSystem.PrimaryVolumeDescriptor.Identifier.SequenceEqual(IsoVolumeDescriptor.HsfStandardIdentifier)
                    ? new IsoFileSystemObject(records[nextRecord..(nextRecord + recordLength)], fileSystem.PrimaryVolumeDescriptor.IsJolietVolumeDescriptor, true)
                    : new IsoFileSystemObject(records[nextRecord..(nextRecord + recordLength)], fileSystem.PrimaryVolumeDescriptor.IsJolietVolumeDescriptor, false);

                // A record whose identifier is a single zero byte is the current directory
                // A record whose identifier is a single one byte is either the parent directory or the root directory if it is the root directory
                if (record.FileIdentifierName != "\u0000" && record.FileIdentifierName != "\u0001")
                {
                    if (record.FileFlags.HasFlag(IsoFileFlags.Directory))
                    {
                        count++;
                        if (recursive)
                        {
                            count += new IsoDirectory(record, fileSystem, this).GetSubdirectoryCount(recursive);
                        }
                    }
                }

                nextRecord += recordLength;
            }

            return count;
        }
    }
}
