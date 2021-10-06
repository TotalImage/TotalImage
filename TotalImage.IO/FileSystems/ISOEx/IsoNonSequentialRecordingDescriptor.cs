using System;
using System.Collections.Immutable;
using System.Linq;
using TotalImage.FileSystems.ISO;

namespace TotalImage.FileSystems.ISOEx
{
    /// <summary>
    /// Represents an ISO 13490 Non Sequential Recording descriptor
    /// </summary>
    public class IsoNonSequentialRecordingDescriptor : IsoVolumeDescriptor
    {
        /// <summary>
        /// The standard ISO 13490 Non Sequential Recording identifier (NSR02)
        /// </summary>
        public static new ImmutableArray<byte> IsoStandardIdentifier { get; } = (new byte[] { 0x4e, 0x53, 0x52, 0x30, 0x32 }).ToImmutableArray();

        /// <summary>
        /// The standard UDF (v1.5+) Non Sequential Recording identifier (NSR03)
        /// </summary>
        public static ImmutableArray<byte> UdfStandardIdentifier { get; } = (new byte[] { 0x4e, 0x53, 0x52, 0x30, 0x33 }).ToImmutableArray();

        /// <inheritdoc />
        public override bool IsValid()
        {
            return Type == 0 && (Identifier.SequenceEqual(IsoStandardIdentifier) || Identifier.SequenceEqual(UdfStandardIdentifier));
        }

        /// <summary>
        /// Create an ISO 13490 begin extended area descriptor
        /// </summary>
        /// <param name="type">The type of the volume descriptor</param>
        /// <param name="identifier">The volume descriptor identifier</param>
        /// <param name="version">The version of the volume descriptor</param>
        public IsoNonSequentialRecordingDescriptor(in byte type, in ImmutableArray<byte> identifier, in byte version)
            : base(type, identifier, version)
        {
        }
    }
}
