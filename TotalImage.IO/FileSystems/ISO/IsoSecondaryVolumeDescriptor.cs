using System;
using System.Collections.Immutable;

namespace TotalImage.FileSystems.ISO
{
    /// <summary>
    /// Represents an ISO 9660 secondary volume descriptor
    /// </summary>
    public class IsoSecondaryVolumeDescriptor : IsoPrimaryVolumeDescriptor
    {
        /// <summary>
        /// This field specifies certain volume characteristics. Currently no standardised flags.
        /// </summary>
        public byte VolumeFlags { get; }

        /// <summary>
        /// One or more escape sequences encoded according to ISO-2022
        /// </summary>
        public ImmutableArray<byte> EscapeSequences { get; }

        /// <summary>
        /// Create an ISO 9660 secondary volume descriptor
        /// </summary>
        /// <param name="record">A span containing the volume descriptor record</param>
        /// <param name="type">The type of the volume descriptor</param>
        /// <param name="identifier">The volume descriptor identifier</param>
        /// <param name="version">The version of the volume descriptor</param>
        public IsoSecondaryVolumeDescriptor(in ReadOnlySpan<byte> record, in IsoVolumeDescriptorType type, in ImmutableArray<byte> identifier, in byte version)
            : base(record, type, identifier, version)
        {
            VolumeFlags = record[7];
            EscapeSequences = record[88..120].ToArray().ToImmutableArray();
        }
    }
}