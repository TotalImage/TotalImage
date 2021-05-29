using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.ISO
{
    /// <summary>
    /// Represents a directory record within an ISO 9660 file system
    /// </summary>
    public class IsoDirectoryRecord : Directory
    {
        /// <summary>
        /// Used to separate a file name and extension in a file identifier
        /// </summary>
        public const char IDENTIFIER_SEPARATOR_1 = '.';

        /// <summary>
        /// Used to separate a file extension and file version in a file identifier
        /// </summary>
        public const char IDENTIFIER_SEPARATOR_2 = ';';

        #region Implemented Properties / Methods
        /// <inheritdoc />
        public override string Name { get => FileIdentifier; set => throw new NotImplementedException(); }

        /// <inheritdoc />
        public override FileAttributes Attributes { get => 0; set => throw new NotImplementedException(); }

        /// <inheritdoc />
        public override DateTime? LastAccessTime { get => null; set => throw new NotImplementedException(); }

        /// <inheritdoc />
        public override DateTime? LastWriteTime { get => RecordingDate?.LocalDateTime.ToLocalTime(); set => throw new NotImplementedException(); }

        /// <inheritdoc />
        public override DateTime? CreationTime { get => null; set => throw new NotImplementedException(); }

        /// <inheritdoc />
        public override ulong Length { get => DataLength; set => throw new NotImplementedException(); }

        public override Directory CreateSubdirectory(string path)
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden, bool showDeleted)
        {
            return Array.Empty<FileSystemObject>();
        }

        public override void MoveTo(string path)
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Length of directory record
        /// </summary>
        public byte RecordLength { get; }

        /// <summary>
        /// Length of extended attribute record
        /// </summary>
        public byte ExtendedAttributeLength { get; }

        /// <summary>
        /// Offset of extent
        /// </summary>
        public uint ExtentOffset { get; }

        /// <summary>
        /// Length of data
        /// </summary>
        public uint DataLength { get; }

        /// <summary>
        /// Time directory was recorded to disc
        /// </summary>
        public DateTimeOffset? RecordingDate { get; }

        /// <summary>
        /// Flags indicating the characteristics of the directory
        /// </summary>
        public IsoFileFlags FileFlags { get; }

        /// <summary>
        /// File unit size if data is recorded in interleave mode
        /// </summary>
        public byte FileUnitSize { get; }

        /// <summary>
        /// The interleave gap size if data is recorded in interleave mode
        /// </summary>
        public byte InterleaveGapSize { get; }

        /// <summary>
        /// The ordinal number of the volume that this extent appears on within the volume set
        /// </summary>
        public ushort VolumeSequenceNumber { get; }

        /// <summary>
        /// Length of the file identifier
        /// </summary>
        public byte FileIdentifierLength { get; }

        /// <summary>
        /// The identifier of the file
        /// </summary>
        public string FileIdentifier { get; }

        /// <summary>
        /// Raw binary data reserved for system use
        /// </summary>
        public ImmutableArray<byte> SystemUseContent { get; }

        /// <summary>
        /// Create an ISO 9660 directory record
        /// </summary>
        /// <param name="record">A span containing the directory record</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the span provided does not cover the entire record</exception>
        public IsoDirectoryRecord(in ReadOnlySpan<byte> record) : base(null, null)
        {
            if (record[0] > record.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(record));
            }

            RecordLength = record[0];
            ExtendedAttributeLength = record[1];
            ExtentOffset = IsoUtilities.ReadUInt32MultiEndian(record[2..10]);
            DataLength = IsoUtilities.ReadUInt32MultiEndian(record[10..18]);
            RecordingDate = IsoUtilities.FromIsoRecordingDateTime(record[18..25]);
            FileFlags = (IsoFileFlags)record[25];
            FileUnitSize = record[26];
            InterleaveGapSize = record[27];
            VolumeSequenceNumber = IsoUtilities.ReadUInt16MultiEndian(record[28..32]);
            FileIdentifierLength = record[32];

            char[] textBuffer = new char[FileIdentifierLength];
            Encoding.ASCII.GetChars(record[33..(33 + FileIdentifierLength)], textBuffer);
            FileIdentifier = textBuffer.AsSpan().Trim('\0').ToString();

            int systemUseStart = 33 + FileIdentifierLength;
            if (systemUseStart % 2 == 1) systemUseStart++; // account for padding field

            if (systemUseStart == RecordLength)
            {
                SystemUseContent = Array.Empty<byte>().ToImmutableArray();
            }
            else
            {
                record[systemUseStart..RecordLength].ToArray().ToImmutableArray();
            }
        }
    }
}
