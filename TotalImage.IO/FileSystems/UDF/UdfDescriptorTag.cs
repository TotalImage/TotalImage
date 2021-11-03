using System;
using System.Buffers.Binary;
using System.IO;

namespace TotalImage.FileSystems.UDF
{
    /// <summary>
    /// A tag which describes a UDF descriptor
    /// </summary>
    public class UdfDescriptorTag
    {
        /// <summary>
        /// The type of descriptor
        /// </summary>
        public UdfDescriptorTagIdentifier Identifier { get; }

        /// <summary>
        /// The version of the descriptor
        /// </summary>
        public ushort DescriptorVersion { get; }

        /// <summary>
        /// The checksum of the tag
        /// </summary>
        public byte Checksum { get; }

        /// <summary>
        /// An identification for the current set of descriptors - used to group multiple descriptors on a single medium together
        /// </summary>
        public ushort SerialNumber { get; }

        /// <summary>
        /// The CRC-16 of the descriptor starting after the tag
        /// </summary>
        public ushort DescriptorCRC { get; }

        /// <summary>
        /// The length of the data used to calculate the descriptor checksum
        /// </summary>
        public ushort DescriptorLength { get; }

        /// <summary>
        /// The location of of the tag
        /// </summary>
        public uint Location { get; }

        /// <summary>
        /// Checks to see if the descriptor tag is valid
        /// </summary>
        /// <returns>Whether the tag is valid</returns>
        public bool IsValid()
        {
            return Identifier <= UdfDescriptorTagIdentifier.LogicalVolumeIntegrityDescriptor
                && (DescriptorVersion == 2 || DescriptorVersion == 3);
        }

        /// <summary>
        /// Create a UDF descriptor tag
        /// </summary>
        /// <param name="record">the data containing the tag</param>
        public UdfDescriptorTag(in ReadOnlySpan<byte> record)
        {
            if (record.Length < 16)
            {
                throw new InvalidDataException("Not enough data for a UDF tag");
            }

            Identifier = (UdfDescriptorTagIdentifier)BinaryPrimitives.ReadUInt16LittleEndian(record[0..2]);
            DescriptorVersion = BinaryPrimitives.ReadUInt16LittleEndian(record[2..4]);
            Checksum = record[4];
            SerialNumber = BinaryPrimitives.ReadUInt16LittleEndian(record[6..8]);
            DescriptorCRC = BinaryPrimitives.ReadUInt16LittleEndian(record[8..10]);
            DescriptorLength = BinaryPrimitives.ReadUInt16LittleEndian(record[10..12]);
            Location = BinaryPrimitives.ReadUInt32LittleEndian(record[12..16]);
        }
    }

    /// <summary>
    /// The type of descriptor record
    /// </summary>
    public enum UdfDescriptorTagIdentifier : ushort
    {
        /// <summary>
        /// The primary volume descriptor
        /// </summary>
        PrimaryVolumeDescriptor = 1,

        /// <summary>
        /// An anchor volume descriptor pointer
        /// </summary>
        AnchorVolumeDescriptorPointer = 2,

        /// <summary>
        /// A volume descriptor pointer
        /// </summary>
        VolumeDescriptorPointer = 3,

        /// <summary>
        /// An implementation-use volume descriptor
        /// </summary>
        ImplementationUseVolumeDescriptor = 4,

        /// <summary>
        /// A partition descriptor
        /// </summary>
        PartitionDescriptor = 5,

        /// <summary>
        /// A logical volume descriptor
        /// </summary>
        LogicalVolumeDescriptor = 6,

        /// <summary>
        /// An unallocated space descriptor
        /// </summary>
        UnallocatedSpaceDescriptor = 7,

        /// <summary>
        /// A terminating descriptor
        /// </summary>
        TerminatingDescriptor = 8,

        /// <summary>
        /// A logical volume integrity descriptor
        /// </summary>
        LogicalVolumeIntegrityDescriptor = 9
    }
}
