using System;
using System.Collections.Immutable;
using System.IO;

namespace TotalImage.FileSystems.ISOEx
{
    /// <summary>
    /// Represents an ISO 13490 entity identifier
    /// </summary>
    public class IsoExIdentifier
    {
        /// <summary>
        /// Flags for the ISO 13490 entity identifier
        /// </summary>
        public byte Flags { get; }

        /// <summary>
        /// The ISO 13490 entity identifier
        /// </summary>
        public ImmutableArray<byte> Identifier { get; }

        /// <summary>
        /// A suffix for the ISO 13490 entity identifier
        /// </summary>
        public ImmutableArray<byte> IdentifierSuffix { get; }

        /// <summary>
        /// Create an ISO 13490 entity identifier
        /// </summary>
        /// <param name="record">A span containing the identifier</param>
        /// <exception cref="InvalidDataException"></exception>
        public IsoExIdentifier(in ReadOnlySpan<byte> record)
        {
            if (record.Length < 32)
            {
                throw new InvalidDataException("Not enough data for an ISO 13490 identifier");
            }

            Flags = record[0];
            Identifier = record[1..24].ToArray().ToImmutableArray();
            IdentifierSuffix = record[24..32].ToArray().ToImmutableArray();
        }
    }
}
