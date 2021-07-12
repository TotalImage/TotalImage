using System;
using System.Collections.Immutable;
using System.Text;

namespace TotalImage.FileSystems.HSF
{
    /// <summary>
    /// Represents a High Sierra Partition Volume Descriptor
    /// </summary>
    public class HsfPartitionVolumeDescriptor : HsfVolumeDescriptor
    {
        /// <summary>
        /// The system that can read the system use content
        /// </summary>
        public string SystemIdentifier { get; }

        /// <summary>
        /// An identifier for the volume partition
        /// </summary>
        public string PartitionIdentifier { get; }

        /// <summary>
        /// The offset of the volume partition
        /// </summary>
        public uint PartitionOffset { get; }

        /// <summary>
        /// The length of the volume partition
        /// </summary>
        public uint PartitionLength { get; }

        /// <summary>
        /// Raw binary data reserved for system use
        /// </summary>
        public ImmutableArray<byte> SystemUseContent { get; set; }

        /// <summary>
        /// Create a High Sierra Partition Volume Descriptor
        /// </summary>
        /// <param name="record">A span containing the volume descriptor record</param>
        /// <param name="type">The type of the volume descriptor</param>
        /// <param name="identifier">The volume descriptor identifier</param>
        /// <param name="version">The version of the volume descriptor</param>
        public HsfPartitionVolumeDescriptor(in ReadOnlySpan<byte> record, in HsfVolumeDescriptorType type, in ImmutableArray<byte> identifier, in byte version)
            : base(type, identifier, version)
        {
            char[] textBuffer = new char[32];

            Encoding.ASCII.GetChars(record[8..40], textBuffer);
            SystemIdentifier = textBuffer.AsSpan().Trim('\0').ToString();

            Encoding.ASCII.GetChars(record[40..72], textBuffer);
            PartitionIdentifier = textBuffer.AsSpan().Trim('\0').ToString();

            PartitionOffset = HsfUtilities.ReadUInt32MultiEndian(record[72..80]);
            PartitionLength = HsfUtilities.ReadUInt32MultiEndian(record[80..88]);

            SystemUseContent = record[88..].ToArray().ToImmutableArray();
        }
    }
}
