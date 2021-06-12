using System;
using System.Collections.Immutable;
using System.Text;

namespace TotalImage.FileSystems.ISO
{
    /// <summary>
    /// Represents a file system object within an ISO 9660 file system
    /// </summary>
    public class IsoFileSystemObject
    {
        /// <summary>
        /// Used to separate a file name and extension in a file identifier
        /// </summary>
        public const char IDENTIFIER_SEPARATOR_1 = '.';

        /// <summary>
        /// Used to separate a file extension and file version in a file identifier
        /// </summary>
        public const char IDENTIFIER_SEPARATOR_2 = ';';

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
        /// <param name="isUnicode">Indicating whether the record should be treated as unicode</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the span provided does not cover the entire record</exception>
        public IsoFileSystemObject(in ReadOnlySpan<byte> record, bool isUnicode)
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

            var nameBytes = record[33..(33 + FileIdentifierLength)];
            if (nameBytes.Length == 1 && nameBytes[0] == 0)
            {
                FileIdentifier = "\u0000";
            }
            else if (nameBytes.Length == 1 && nameBytes[0] == 1)
            {
                FileIdentifier = "\u0001";
            }
            else
            {
                Encoding encoding = isUnicode ? Encoding.BigEndianUnicode : Encoding.ASCII;
                char[] textBuffer = new char[encoding.GetCharCount(nameBytes)];
                encoding.GetChars(nameBytes, textBuffer);
                FileIdentifier = new string(textBuffer);
            }

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
