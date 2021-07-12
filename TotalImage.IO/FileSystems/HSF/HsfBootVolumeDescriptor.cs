using System;
using System.Collections.Immutable;
using System.Text;

namespace TotalImage.FileSystems.HSF
{
    /// <summary>
    /// Represents an ISO 9660 Boot Record
    /// </summary>
    public class HsfBootVolumeDescriptor : HsfVolumeDescriptor
    {
        /// <summary>
        /// An identifier of which system can read the boot record
        /// </summary>
        public string BootSystemIdentifier { get; }

        /// <summary>
        /// An identifier for the boot record
        /// </summary>
        public string BootIdentifier { get; }

        /// <summary>
        /// The raw binary to be used by the boot system
        /// </summary>
        public ImmutableArray<byte> BootSystemContent { get; }

        /// <summary>
        /// Create an ISO 9660 Boot Record
        /// </summary>
        /// <param name="record">A span containing the volume descriptor record</param>
        /// <param name="type">The type of the volume descriptor</param>
        /// <param name="identifier">The volume descriptor identifier</param>
        /// <param name="version">The version of the volume descriptor</param>
        public HsfBootVolumeDescriptor(in ReadOnlySpan<byte> record, in HsfVolumeDescriptorType type, in ImmutableArray<byte> identifier, in byte version)
            : base(type, identifier, version)
        {
            Span<char> tmp = new char[32];

            Encoding.ASCII.GetChars(record[7..39], tmp);
            BootSystemIdentifier = tmp.TrimEnd('\0').ToString();

            Encoding.ASCII.GetChars(record[39..71], tmp);
            BootIdentifier = tmp.TrimEnd('\0').ToString();

            BootSystemContent = record[71..].ToArray().ToImmutableArray();
        }
    }
}
