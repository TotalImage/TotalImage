using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using TotalImage.Containers;

namespace TotalImage.Partitions
{
    /// <summary>
    /// A partition table class that represents a GUID Partition Table
    /// </summary>
    public class GptPartitionTable : PartitionTable
    {
        private GptHeader? _header = null;
        private readonly uint _sectorSize;

        /// <summary>
        /// The header for the GUID Partition Table
        /// </summary>
        public GptHeader? Header => _header;

        /// <inheritdoc />
        public override string DisplayName => "GUID Partition Table";

        /// <inheritdoc />
        public GptPartitionTable(Container container, uint sectorSize = 512) : base(container)
        {
            _sectorSize = sectorSize;
        }

        /// <inheritdoc />
        protected override IEnumerable<PartitionEntry> LoadPartitions()
        {
            _container.Content.Seek(_sectorSize, SeekOrigin.Begin);

            byte[] buffer = new byte[92];
            _container.Content.Read(buffer);
            _header = new GptHeader(buffer);

            if (_header.Signature != "EFI PART")
            {
                throw new InvalidDataException("Not a GPT Protective Partition");
            }

            _container.Content.Seek((long)(_sectorSize * _header.TableLBA), SeekOrigin.Begin);

            var entries = ImmutableList.CreateBuilder<PartitionEntry>();
            buffer = new byte[_header.SizeOfPartitionEntry];

            for (int i = 0; i < _header.PartitionEntries; i++)
            {
                _container.Content.Read(buffer);
                Guid typeId = new Guid(buffer[0..16]);
                Guid entryId = new Guid(buffer[16..32]);
                ulong firstLba = BinaryPrimitives.ReadUInt64LittleEndian(buffer[32..40]);
                ulong lastLba = BinaryPrimitives.ReadUInt64LittleEndian(buffer[40..48]);
                GptPartitionFlags flags = (GptPartitionFlags)BinaryPrimitives.ReadUInt64LittleEndian(buffer[48..56]);
                string name = Encoding.Unicode.GetString(buffer[56..128]);

                long offset = (long)(firstLba * _sectorSize);
                long length = (long)((lastLba - firstLba + 1) * _sectorSize);

                if (typeId != Guid.Empty)
                {
                    entries.Add(new GptPartitionEntry(typeId, entryId, flags, name, offset, length, new PartialStream(_container.Content, offset, length)));
                }
            }

            return entries.ToImmutableList();
        }

        /// <summary>
        /// Represents the header of a GUID Partition Table
        /// </summary>
        public class GptHeader
        {
            /// <summary>
            /// The signature of a GUID Partition Table header - should be "EFI PART"
            /// </summary>
            public string Signature { get; }

            /// <summary>
            /// The major version number of the GUID Partition Table
            /// </summary>
            public ushort VersionMajor { get; }

            /// <summary>
            /// The minor version of the GUID Partition Table
            /// </summary>
            public ushort VersionMinor { get; }

            /// <summary>
            /// The size of the GUID Partition Table header
            /// </summary>
            public uint HeaderSize { get; }

            /// <summary>
            /// The CRC32 of the GUID Partition Table header
            /// </summary>
            public uint HeaderHash { get; }

            /// <summary>
            /// The LBA location of the current header
            /// </summary>
            public ulong CurrentLBA { get; }

            /// <summary>
            /// The LBA location of the alternative copy of the header
            /// </summary>
            public ulong BackupLBA { get; }

            /// <summary>
            /// The first LBA available for use in GPT partitions
            /// </summary>
            public ulong FirstPartitionLBA { get; }

            /// <summary>
            /// The last LBA available for use in GPT partitions
            /// </summary>
            public ulong LastPartitionLBA { get; }

            /// <summary>
            /// The GUID of this disk
            /// </summary>
            public Guid DiskGuid { get; }
            /// <summary>
            /// The starting LBA of the partition table
            /// </summary>
            public ulong TableLBA { get; }

            /// <summary>
            /// The number of entries in the partition table
            /// </summary>
            public uint PartitionEntries { get; }

            /// <summary>
            /// The size in bytes of each partition entry
            /// </summary>
            public uint SizeOfPartitionEntry { get; }

            /// <summary>
            /// The CRC32 of the partition entries together
            /// </summary>
            public uint TableHash { get; }

            /// <summary>
            /// Read a GUID Partition Table header from a stream
            /// </summary>
            /// <param name="bytes">The bytes containing the header</param>
            public GptHeader(in Span<byte> bytes)
            {
                Signature = Encoding.ASCII.GetString(bytes[0..8]);
                uint version = BinaryPrimitives.ReadUInt32LittleEndian(bytes[8..12]);
                VersionMajor = (ushort)(version >> 16);
                VersionMinor = (ushort)(version & 0xFFFF);
                HeaderSize = BinaryPrimitives.ReadUInt32LittleEndian(bytes[12..16]);
                HeaderHash = BinaryPrimitives.ReadUInt32LittleEndian(bytes[16..20]);
                CurrentLBA = BinaryPrimitives.ReadUInt64LittleEndian(bytes[24..32]);
                BackupLBA = BinaryPrimitives.ReadUInt64LittleEndian(bytes[32..40]);
                FirstPartitionLBA = BinaryPrimitives.ReadUInt64LittleEndian(bytes[40..48]);
                LastPartitionLBA = BinaryPrimitives.ReadUInt64LittleEndian(bytes[48..56]);
                DiskGuid = new Guid(bytes[56..72]);
                TableLBA = BinaryPrimitives.ReadUInt64LittleEndian(bytes[72..80]);
                PartitionEntries = BinaryPrimitives.ReadUInt32LittleEndian(bytes[80..84]);
                SizeOfPartitionEntry = BinaryPrimitives.ReadUInt32LittleEndian(bytes[84..88]);
                TableHash = BinaryPrimitives.ReadUInt32LittleEndian(bytes[88..92]);
            }
        }

        /// <summary>
        /// Represents an entry in a GUID Partition Table
        /// </summary>
        public class GptPartitionEntry : PartitionEntry
        {
            /// <summary>
            /// A unique identifier indicating the type of partition
            /// </summary>
            public Guid TypeId { get; }

            /// <summary>
            /// A unique identifier for the partition
            /// </summary>
            public Guid EntryId { get; }

            /// <summary>
            /// The flags set on the partition
            /// </summary>
            public GptPartitionFlags Flags { get; }

            /// <summary>
            /// The name of the partition
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Create a GUID Partition Table entry
            /// </summary>
            /// <param name="typeId">A unique identifier indicating the type of the partition</param>
            /// <param name="entryId">A unique identifier for the partition</param>
            /// <param name="flags">The flags to set on the partition</param>
            /// <param name="name">The name of the partition</param>
            /// <param name="offset">The offset of the partition in it's container file</param>
            /// <param name="length">The length of the partition</param>
            /// <param name="stream">The stream containing the partition data</param>
            public GptPartitionEntry(Guid typeId, Guid entryId, GptPartitionFlags flags, string name, long offset, long length, Stream stream) : base(offset, length, stream)
            {
                TypeId = typeId;
                EntryId = entryId;
                Flags = flags;
                Name = name;
            }
        }

        /// <summary>
        /// Represents any flags set on the partition entry
        /// </summary>
        [Flags]
        public enum GptPartitionFlags : ulong
        {
            /// <summary>
            /// Indicates no flags are set
            /// </summary>
            [Display(Name = "No flags")]
            None = 0x0,

            /// <summary>
            /// Indicates EFI firmware should ignore the partition
            /// </summary>
            [Display(Name = "Should be ignored by EFI")]
            EfiIgnore = 0x1,

            /// <summary>
            /// Legacy BIOSes can boot from this partition - equivalent to "active" flag in MBR
            /// </summary>
            [Display(Name = "Bootable by Legacy BIOS")]
            BiosBootable = 0x2
        }
    }
}
