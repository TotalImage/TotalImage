using System;
using System.Collections.Immutable;
using System.Linq;

namespace TotalImage.FileSystems.HSF
{
    /// <summary>
    /// Represents an ISO 9660 Volume Descriptor structure
    /// </summary>
    public abstract class HsfVolumeDescriptor
    {
        /// <summary>
        /// The standard ISO 9660 identifier for a volume (CD001)
        /// </summary>
        public static ImmutableArray<byte> StandardIdentifier { get; } = (new byte[] { 0x43, 0x44, 0x52, 0x4f, 0x4d }).ToImmutableArray();

        /// <summary>
        /// This indicates the type of volume descriptor
        /// </summary>
        public HsfVolumeDescriptorType Type { get; }

        /// <summary>
        /// The record identifier - should always be CD001
        /// </summary>
        public ImmutableArray<byte> Identifier { get; }

        /// <summary>
        /// The version of the volume descriptor
        /// </summary>
        public byte Version { get; }

        /// <summary>
        /// Check whether the specified record is valid
        /// </summary>
        /// <returns>Whether the record is valid</returns>
        public virtual bool IsValid()
        {
            return (Type <= HsfVolumeDescriptorType.VolumePartitionDescriptor || Type == HsfVolumeDescriptorType.VolumeDescriptorSetTerminator)
                && Identifier.SequenceEqual(StandardIdentifier);
        }

        /// <summary>
        /// Create a volume descriptor record - this should be used by inheriting types to construct the base object.
        /// </summary>
        /// <param name="type">The type of the volume descriptor</param>
        /// <param name="identifier">The volume descriptor identifier</param>
        /// <param name="version">The version of the volume descriptor</param>
        protected HsfVolumeDescriptor(in HsfVolumeDescriptorType type, in ImmutableArray<byte> identifier, in byte version)
        {
            Type = type;
            Identifier = identifier;
            Version = version;
        }

        /// <summary>
        /// Read a volume descriptor
        /// </summary>
        /// <param name="record">A span containing the volume descriptor record</param>
        /// <param name="fileSystem">The file system containing the volume descriptor</param>
        /// <returns>The volume descriptor record</returns>
        public static HsfVolumeDescriptor? ReadVolumeDescriptor(in ReadOnlySpan<byte> record, HighSierraFileSystem fileSystem)
        {
            HsfVolumeDescriptorType type = (HsfVolumeDescriptorType)record[0];
            ImmutableArray<byte> identifier = record[1..6].ToArray().ToImmutableArray();

            if (!identifier.SequenceEqual(StandardIdentifier))
            {
                return null;
            }

            byte version = record[6];

            return type switch
            {
                HsfVolumeDescriptorType.PrimaryVolumeDescriptor => new HsfPrimaryVolumeDescriptor(record, type, identifier, version),
                HsfVolumeDescriptorType.BootRecord => new HsfBootVolumeDescriptor(record, type, identifier, version),
                HsfVolumeDescriptorType.SupplementaryVolumeDescriptor => new HsfSecondaryVolumeDescriptor(record, type, identifier, version),
                HsfVolumeDescriptorType.VolumePartitionDescriptor => new HsfPartitionVolumeDescriptor(record, type, identifier, version),
                HsfVolumeDescriptorType.VolumeDescriptorSetTerminator => new HsfSetTerminatorVolumeDescriptor(type, identifier, version),
                _ => new HsfUnknownVolumeDescriptor(record, type, identifier, version),
            };
        }
    }
}
