using System;
using System.Collections.Immutable;
using System.Text;

namespace TotalImage.FileSystems.HSF
{
    /// <summary>
    /// Represents a file system object within an ISO 9660 file system
    /// </summary>
    public class HsfFileSystemObject
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
        public HsfFileFlags FileFlags { get; }

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
        public string FileIdentifierName { get; }

        /// <summary>
        /// The identifier of the file
        /// </summary>
        public string FileIdentifierExtension { get; }

        /// <summary>
        /// The identifier of the file
        /// </summary>
        public ushort FileIdentifierVersion { get; }

        /// <summary>
        /// Raw binary data reserved for system use
        /// </summary>
        public ImmutableArray<byte> SystemUseContent { get; }

        /// <summary>
        /// Create an ISO 9660 directory record
        /// </summary>
        /// <param name="record">A span containing the directory record</param>
        /// <param name="isRoot">Indicating whether the record should be treated as a root directory element</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the span provided does not cover the entire record</exception>
        public HsfFileSystemObject(in ReadOnlySpan<byte> record, bool isRoot = false)
        {
            /*
             * Root directories may specify the length of the in-disc record
             * and not the volume descriptor record.
             */
            if (record[0] < 34 || (!isRoot && record[0] != record.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(record));
            }

            RecordLength = record[0];
            ExtendedAttributeLength = record[1];
            ExtentOffset = HsfUtilities.ReadUInt32MultiEndian(record[2..10]);
            DataLength = HsfUtilities.ReadUInt32MultiEndian(record[10..18]);
            RecordingDate = HsfUtilities.FromHsfRecordingDateTime(record[18..25]);
            FileFlags = (HsfFileFlags)record[25];
            FileUnitSize = record[26];
            InterleaveGapSize = record[27];
            VolumeSequenceNumber = HsfUtilities.ReadUInt16MultiEndian(record[28..32]);
            FileIdentifierLength = record[32];

            var nameBytes = record[33..(33 + FileIdentifierLength)];
            if (nameBytes.Length == 1 && nameBytes[0] == 0)
            {
                FileIdentifierName = "\u0000";
                FileIdentifierExtension = "";
                FileIdentifierVersion = 0;
            }
            else if (nameBytes.Length == 1 && nameBytes[0] == 1)
            {
                FileIdentifierName = "\u0001";
                FileIdentifierExtension = "";
                FileIdentifierVersion = 0;
            }
            else
            {
                Encoding encoding = Encoding.ASCII;
                Span<char> textBuffer = new char[encoding.GetCharCount(nameBytes)].AsSpan();
                encoding.GetChars(nameBytes, textBuffer);

                int endOfName = textBuffer.IndexOfAny('.', ';');
                if (endOfName == -1)
                {
                    endOfName = textBuffer.Length;
                }
                FileIdentifierName = new string(textBuffer[..endOfName]);

                if (endOfName != textBuffer.Length)
                {
                    int endOfExt = textBuffer[(endOfName + 1)..].IndexOf(';');
                    endOfExt = endOfExt == -1
                        ? textBuffer.Length
                        : endOfExt + endOfName + 1;

                    FileIdentifierExtension = new string(textBuffer[(endOfName + 1)..endOfExt]);

                    if (endOfExt != textBuffer.Length && textBuffer[(endOfExt + 1)..].Length > 0)
                    {
                        if (ushort.TryParse(textBuffer[(endOfExt + 1)..], out ushort version))
                        {
                            FileIdentifierVersion = version;
                        }
                        else
                        {
                            FileIdentifierVersion = 1;
                        }
                    }
                    else
                    {
                        FileIdentifierVersion = 1;
                    }
                }
                else
                {
                    FileIdentifierExtension = "";
                    FileIdentifierVersion = 1;
                }

            }

            int systemUseStart = 33 + FileIdentifierLength;
            if (systemUseStart % 2 == 1) systemUseStart++; // account for padding field

            if (systemUseStart == RecordLength || isRoot)
            {
                SystemUseContent = Array.Empty<byte>().ToImmutableArray();
            }
            else
            {
                SystemUseContent = record[systemUseStart..RecordLength].ToArray().ToImmutableArray();
            }
        }
    }
}
