using System;
using System.Collections.Immutable;
using System.Linq;

namespace TotalImage.FileSystems.ISO
{
    /// <summary>
    /// Represents an ISO 9660 Volume Descriptor structure
    /// </summary>
    public abstract class IsoVolumeDescriptor
    {
        /// <summary>
        /// The standard ISO 9660 identifier for a volume (CD001)
        /// </summary>
        public static ImmutableArray<byte> IsoStandardIdentifier { get; } = (new byte[] { 0x43, 0x44, 0x30, 0x30, 0x31 }).ToImmutableArray();

        /// <summary>
        /// The standard High Sierra identifier for a volume (CDROM)
        /// </summary>
        public static ImmutableArray<byte> HsfStandardIdentifier { get; } = (new byte[] { 0x43, 0x44, 0x52, 0x4F, 0x4D }).ToImmutableArray();

        /// <summary>
        /// This indicates the type of volume descriptor
        /// </summary>
        public IsoVolumeDescriptorType Type { get; }

        /// <summary>
        /// The record identifier - should always be CD001
        /// </summary>
        public ImmutableArray<byte> Identifier { get; }

        /// <summary>
        /// The version of the volume descriptor
        /// </summary>
        public byte Version { get; }

        /// <summary>
        /// Create a volume descriptor record - this should be used by inheriting types to construct the base object.
        /// </summary>
        /// <param name="type">The type of the volume descriptor</param>
        /// <param name="identifier">The volume descriptor identifier</param>
        /// <param name="version">The version of the volume descriptor</param>
        protected IsoVolumeDescriptor(in IsoVolumeDescriptorType type, in ImmutableArray<byte> identifier, in byte version)
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
        public static IsoVolumeDescriptor? ReadVolumeDescriptor(in ReadOnlySpan<byte> record, Iso9660FileSystem fileSystem)
        {
            IsoVolumeDescriptorType type = (IsoVolumeDescriptorType)record[0];
            ImmutableArray<byte> identifier = record[1..6].ToArray().ToImmutableArray();
            byte version = 0;

            if (identifier.SequenceEqual(IsoStandardIdentifier))
            {
                version = record[6];
            }
            else
            {
                type = (IsoVolumeDescriptorType)record[8];
                identifier = record[9..14].ToArray().ToImmutableArray();
                if (!identifier.SequenceEqual(HsfStandardIdentifier))
                    return null;
                version = record[14];
            }

            return type switch
            {
                IsoVolumeDescriptorType.PrimaryVolumeDescriptor => new IsoPrimaryVolumeDescriptor(record, type, identifier, version),
                IsoVolumeDescriptorType.BootRecord => new IsoBootVolumeDescriptor(record, type, identifier, version),
                IsoVolumeDescriptorType.SupplementaryVolumeDescriptor => new IsoSecondaryVolumeDescriptor(record, type, identifier, version),
                IsoVolumeDescriptorType.VolumePartitionDescriptor => new IsoPartitionVolumeDescriptor(record, type, identifier, version),
                IsoVolumeDescriptorType.VolumeDescriptorSetTerminator => new IsoSetTerminatorVolumeDescriptor(type, identifier, version),
                _ => new IsoUnknownVolumeDescriptor(record, type, identifier, version),
            };
        }

        /// <summary>
        /// Check whether the specified record is valid
        /// </summary>
        /// <returns>Whether the record is valid</returns>
        public virtual bool IsValid()
        {
            return (Type <= IsoVolumeDescriptorType.VolumePartitionDescriptor || Type == IsoVolumeDescriptorType.VolumeDescriptorSetTerminator)
                && (Identifier.SequenceEqual(IsoStandardIdentifier) || Identifier.SequenceEqual(HsfStandardIdentifier));
        }
    }
}
