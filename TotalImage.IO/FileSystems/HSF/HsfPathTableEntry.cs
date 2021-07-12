using System;
using System.Buffers.Binary;

namespace TotalImage.FileSystems.HSF
{
    /// <summary>
    /// Represents an entry in the path table of an ISO 9660 file system
    /// </summary>
    public class HsfPathTableEntry
    {
        /// <summary>
        /// The length of the directory identifier
        /// </summary>
        public byte DirectoryIdentifierLength { get; }

        /// <summary>
        /// The length of the extended attribute record
        /// </summary>
        public byte ExtendedAttributeRecordLength { get; }

        /// <summary>
        /// The offset of the extent
        /// </summary>
        public uint ExtentOffset { get; }

        /// <summary>
        /// The index number of the parent directory in the path table
        /// </summary>
        public ushort ParentDirectoryIndex { get; }

        /// <summary>
        /// Create a path table entry from a span of bytes
        /// </summary>
        /// <param name="entry">The bytes for the entry</param>
        /// <param name="useMsb">If true, treat multi-byte values as big-endian</param>
        public HsfPathTableEntry(in ReadOnlySpan<byte> entry, bool useMsb = false)
        {
            DirectoryIdentifierLength = entry[0];
            ExtendedAttributeRecordLength = entry[1];

            ExtentOffset = useMsb
                ? BinaryPrimitives.ReadUInt32BigEndian(entry[2..5])
                : BinaryPrimitives.ReadUInt32LittleEndian(entry[2..5]);

            ParentDirectoryIndex = useMsb
                ? BinaryPrimitives.ReadUInt16BigEndian(entry[6..8])
                : BinaryPrimitives.ReadUInt16LittleEndian(entry[6..8]);
        }
    }
}