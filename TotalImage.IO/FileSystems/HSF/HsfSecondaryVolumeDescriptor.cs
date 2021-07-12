using System;
using System.Collections.Immutable;

namespace TotalImage.FileSystems.HSF
{
    /// <summary>
    /// Represents a High Sierra secondary volume descriptor
    /// </summary>
    public class HsfSecondaryVolumeDescriptor : HsfPrimaryVolumeDescriptor
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
        /// Create a High Sierra secondary volume descriptor
        /// </summary>
        /// <param name="record">A span containing the volume descriptor record</param>
        /// <param name="type">The type of the volume descriptor</param>
        /// <param name="identifier">The volume descriptor identifier</param>
        /// <param name="version">The version of the volume descriptor</param>
        public HsfSecondaryVolumeDescriptor(in ReadOnlySpan<byte> record, in HsfVolumeDescriptorType type, in ImmutableArray<byte> identifier, in byte version)
            : base(record, type, identifier, version)
        {
            VolumeFlags = record[7];
            EscapeSequences = record[88..120].ToArray().ToImmutableArray();
        }
    }
}