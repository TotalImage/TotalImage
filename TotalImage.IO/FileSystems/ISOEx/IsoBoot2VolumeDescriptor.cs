using System;
using System.Buffers.Binary;
using System.Collections.Immutable;
using System.Linq;
using TotalImage.FileSystems.ISO;

namespace TotalImage.FileSystems.ISOEx
{
    /// <summary>
    /// Represents an ISO 13490 Boot Record
    /// </summary>
    public class IsoBoot2VolumeDescriptor : IsoVolumeDescriptor
    {
        /// <summary>
        /// The standard ISO 13490 Boot Record identifier (BOOT2)
        /// </summary>
        public static ImmutableArray<byte> StandardIdentifier { get; } = (new byte[] { 0x42, 0x4f, 0x4f, 0x54, 0x32 }).ToImmutableArray();

        /// <summary>
        /// Identifies the type of system that can use the boot record
        /// </summary>
        public IsoExIdentifier ArchitectureType { get; }

        /// <summary>
        /// Identifies the type of implementation that can use the boot record
        /// </summary>
        public IsoExIdentifier BootIdentifier { get; }

        /// <summary>
        /// The address of an extent containing boot information
        /// </summary>
        public uint ExtentLocation { get; }

        /// <summary>
        /// The length of an extent containing boot information
        /// </summary>
        public uint ExtentLength { get; }

        /// <summary>
        /// The memory address at which the extent should be loaded
        /// </summary>
        public ulong LoadAddress { get; }

        /// <summary>
        /// The memory address to which control should be transferred after loading the extent
        /// </summary>
        public ulong StartAddress { get; }

        /// <summary>
        /// The date and time the descriptor was created
        /// </summary>
        public DateTimeOffset? CreationDateTime { get; }

        /// <summary>
        /// Specifies characteristics of the boot descriptor
        /// </summary>
        public ushort Flags { get; }

        /// <summary>
        /// Content reserved for implementation use
        /// </summary>
        public ImmutableArray<byte> Content { get; }


        /// <inheritdoc />
        public override bool IsValid()
        {
            return Type == 0 && Identifier.SequenceEqual(StandardIdentifier);
        }

        /// <summary>
        /// Create an ISO 13490 Boot Record
        /// </summary>
        /// <param name="record">A span containing the volume descriptor record</param>
        /// <param name="type">The type of the volume descriptor</param>
        /// <param name="identifier">The volume descriptor identifier</param>
        /// <param name="version">The version of the volume descriptor</param>
        public IsoBoot2VolumeDescriptor(in ReadOnlySpan<byte> record, in IsoVolumeDescriptorType type, in ImmutableArray<byte> identifier, in byte version)
            : base((byte)type, identifier, version)
        {
            ArchitectureType = new IsoExIdentifier(record[8..40]);
            BootIdentifier = new IsoExIdentifier(record[40..72]);

            ExtentLocation = BinaryPrimitives.ReadUInt32LittleEndian(record[72..76]);
            ExtentLength = BinaryPrimitives.ReadUInt32LittleEndian(record[76..80]);

            LoadAddress = BinaryPrimitives.ReadUInt64LittleEndian(record[80..88]);
            StartAddress = BinaryPrimitives.ReadUInt64LittleEndian(record[88..96]);

            CreationDateTime = IsoUtilities.FromIsoExTimestamp(record[96..108]);

            Flags = BinaryPrimitives.ReadUInt16LittleEndian(record[108..110]);

            Content = record[142..2048].ToArray().ToImmutableArray();
        }
    }
}
