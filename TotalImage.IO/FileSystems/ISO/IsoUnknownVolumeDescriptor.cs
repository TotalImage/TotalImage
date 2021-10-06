using System;
using System.Collections.Immutable;
using System.Linq;

namespace TotalImage.FileSystems.ISO
{
    /// <summary>
    /// Represents an unknown type of ISO 9660 volume descriptor
    /// </summary>
    public class IsoUnknownVolumeDescriptor : IsoVolumeDescriptor
    {
        /// <summary>
        /// The raw binary data contained within the record
        /// </summary>
        public ImmutableArray<byte> Content { get; }

        /// <summary>
        /// Create an ISO 9660 volume descriptor of an unknown type
        /// </summary>
        /// <param name="record">A span containing the volume descriptor record</param>
        /// <param name="type">The type of the volume descriptor</param>
        /// <param name="identifier">The volume descriptor identifier</param>
        /// <param name="version">The version of the volume descriptor</param>
        public IsoUnknownVolumeDescriptor(in ReadOnlySpan<byte> record, in IsoVolumeDescriptorType type, in ImmutableArray<byte> identifier, in byte version)
            : base((byte)type, identifier, version)
        {
            if (identifier.SequenceEqual(HsfStandardIdentifier))
                Content = record[15..].ToArray().ToImmutableArray();
            else if (identifier.SequenceEqual(IsoStandardIdentifier))
                Content = record[7..].ToArray().ToImmutableArray();
        }
    }
}
