using System;
using System.Collections.Immutable;
using System.Linq;
using TotalImage.FileSystems.ISO;

namespace TotalImage.FileSystems.ISOEx
{
    /// <summary>
    /// Represents an ISO 13490 Begin Extended Area descriptor
    /// </summary>
    public class IsoTerminateExtendedAreaDescriptor : IsoVolumeDescriptor
    {
        /// <summary>
        /// The standard ISO 13490 Begin Extended Area identifier (BEA01)
        /// </summary>
        public static ImmutableArray<byte> StandardIdentifier { get; } = (new byte[] { 0x54, 0x45, 0x41, 0x30, 0x31 }).ToImmutableArray();

        /// <inheritdoc />
        public override bool IsValid()
        {
            return Type == 0 && Identifier.SequenceEqual(StandardIdentifier);
        }

        /// <summary>
        /// Create an ISO 13490 begin extended area descriptor
        /// </summary>
        /// <param name="type">The type of the volume descriptor</param>
        /// <param name="identifier">The volume descriptor identifier</param>
        /// <param name="version">The version of the volume descriptor</param>
        public IsoTerminateExtendedAreaDescriptor(in byte type, in ImmutableArray<byte> identifier, in byte version)
            : base(type, identifier, version)
        {
        }
    }
}
