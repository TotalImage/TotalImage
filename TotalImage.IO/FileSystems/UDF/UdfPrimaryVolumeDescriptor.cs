using System;
using System.Buffers.Binary;
using System.Collections.Immutable;

namespace TotalImage.FileSystems.UDF
{
    /// <summary>
    /// Represents an unknown type of UDF volume descriptor
    /// </summary>
    public class UdfPrimaryVolumeDescriptor : UdfVolumeDescriptor
    {
        public uint VolumeDescriptorSequenceNumber { get; }

        public uint PrimaryVolumeDescriptorNumber { get; }



        /// <summary>
        /// Create a UDF volume descriptor of an unknown type
        /// </summary>
        /// <param name="record">A span containing the volume descriptor record</param>
        /// <param name="tag">The volume descriptor tag</param>
        public UdfPrimaryVolumeDescriptor(in ReadOnlySpan<byte> record, UdfDescriptorTag tag) : base(tag)
        {
            VolumeDescriptorSequenceNumber = BinaryPrimitives.ReadUInt32LittleEndian(record[16..20]);
            PrimaryVolumeDescriptorNumber = BinaryPrimitives.ReadUInt32LittleEndian(record[20..24]);
        }
    }
}
