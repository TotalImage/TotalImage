using System;
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
        public GptPartitionTable(Container container, uint sectorSize = 512) : base(container)
        {
            _sectorSize = sectorSize;
        }

        /// <inheritdoc />
        protected override IEnumerable<PartitionEntry> LoadPartitions()
        {
            using BinaryReader br = new BinaryReader(_container.Content, Encoding.ASCII, true);
            br.BaseStream.Seek(_sectorSize, SeekOrigin.Begin);

            _header = new GptHeader(br);

            if (_header.Signature != "EFI PART")
            {
                throw new InvalidDataException("Not a GPT Protective Partition");
            }

            br.BaseStream.Seek((long)(_sectorSize * _header.TableLBA), SeekOrigin.Begin);

            var entries = ImmutableList.CreateBuilder<PartitionEntry>();
            for (int i = 0; i < _header.PartitionEntries; i++)
            {
                Guid typeId = new Guid(br.ReadBytes(16));
                Guid entryId = new Guid(br.ReadBytes(16));
                ulong firstLba = br.ReadUInt64();
                ulong lastLba = br.ReadUInt64();
                GptPartitionFlags flags = (GptPartitionFlags)br.ReadUInt64();
                string name = Encoding.Unicode.GetString(br.ReadBytes(72));

                long offset = (long)(firstLba * _sectorSize);
                long length = (long)((lastLba - firstLba + 1) * _sectorSize);

                long additionalBytes = _header.SizeOfPartitionEntry - 0x80;
                if (additionalBytes > 0)
                {
                    br.BaseStream.Seek(additionalBytes, SeekOrigin.Current);
                }

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
            /// <param name="reader">The binary reader for the stream</param>
            public GptHeader(BinaryReader reader)
            {
                Signature = new string(reader.ReadChars(8));
                uint version = reader.ReadUInt32();
                VersionMajor = (ushort)(version >> 16);
                VersionMinor = (ushort)(version & 0xFFFF);
                HeaderSize = reader.ReadUInt32();
                HeaderHash = reader.ReadUInt32();
                reader.BaseStream.Seek(4, SeekOrigin.Current); // skip past reserved field
                CurrentLBA = reader.ReadUInt64();
                BackupLBA = reader.ReadUInt64();
                FirstPartitionLBA = reader.ReadUInt64();
                LastPartitionLBA = reader.ReadUInt64();
                DiskGuid = new Guid(reader.ReadBytes(16));
                TableLBA = reader.ReadUInt64();
                PartitionEntries = reader.ReadUInt32();
                SizeOfPartitionEntry = reader.ReadUInt32();
                TableHash = reader.ReadUInt32();
            }
        }

        /// <summary>
        /// Represents an entry in a GUID Partition Table
        /// </summary>
        public class GptPartitionEntry : PartitionEntry
        {
            public Guid TypeId { get; }

            public Guid EntryId { get; }

            public GptPartitionFlags Flags { get; }

            public string Name { get; }

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
