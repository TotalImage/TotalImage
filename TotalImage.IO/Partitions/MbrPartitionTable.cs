using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.IO;
using TotalImage.Containers;

namespace TotalImage.Partitions
{
    /// <summary>
    /// A partition table class that represents a basic MBR partition table
    /// </summary>
    public class MbrPartitionTable : PartitionTable
    {
        private readonly uint _sectorSize;

        /// <inheritdoc />
        public override string DisplayName => "Master Boot Record";

        /// <inheritdoc />
        public MbrPartitionTable(Container container, uint sectorSize = 512) : base(container)
        {
            _sectorSize = sectorSize;
        }

        /// <inheritdoc />
        protected override IEnumerable<PartitionEntry> LoadPartitions()
        {
            _container.Content.Seek(0x1FE, SeekOrigin.Begin);

            Span<byte> buffer = new byte[2];
            _container.Content.Read(buffer);
            ushort signature = BinaryPrimitives.ReadUInt16LittleEndian(buffer);
            if (signature != 0xAA55)
            {
                throw new InvalidDataException();
            }

            _container.Content.Seek(0x1BE, SeekOrigin.Begin);

            buffer = new byte[64];
            _container.Content.Read(buffer);

            var entries = ImmutableList.CreateBuilder<PartitionEntry>();
            for (int i = 0; i < 4; i++)
            {
                var record = buffer[(i * 16)..((i + 1) * 16)];

                MbrPartitionType type = (MbrPartitionType)record[4];
                if (type == MbrPartitionType.Empty)
                {
                    continue;
                }

                byte status = record[0];
                CHSAddress chsStart = new CHSAddress(record[1..4]);
                CHSAddress chsEnd = new CHSAddress(record[5..8]);
                uint lbaStart = BinaryPrimitives.ReadUInt32LittleEndian(record[8..12]);
                uint lbaLength = BinaryPrimitives.ReadUInt32LittleEndian(record[12..16]);



                uint offset = lbaStart * _sectorSize;
                uint length = lbaLength * _sectorSize;
                MbrPartitionEntry entry = new MbrPartitionEntry((status & 0x80) != 0, type, chsStart, chsEnd, lbaStart, lbaLength, offset, length, new PartialStream(_container.Content, offset, length));
                entries.Add(entry);
            }

            return entries.ToImmutableList();
        }

        /// <summary>
        /// A partition entry within an MBR Partition Table
        /// </summary>
        public class MbrPartitionEntry : PartitionEntry
        {
            private bool _active;
            private MbrPartitionType _type;
            private CHSAddress _chsStart;
            private CHSAddress _chsEnd;
            private uint _lbaStart;
            private uint _lbaLength;

            /// <summary>
            /// Indicates whether the partition is marked as active
            /// </summary>
            public bool Active
            {
                get => _active;
                set => _active = value;
            }

            /// <summary>
            /// Indicates the type of partition
            /// </summary>
            public MbrPartitionType Type
            {
                get => _type;
                set => _type = value;
            }

            /// <summary>
            /// CHS address of first absolute sector in the partition. Maximum supported values are 1023,255,63.
            /// </summary>
            /// <remarks>
            /// For very large disks, these values are bogus and LBA-aware software will use the LBA fields instead.
            /// </remarks>
            public CHSAddress ChsStart
            {
                get => _chsStart;
                set => _chsStart = value;
            }

            /// <summary>
            /// CHS address of last absolute sector in the partition. Maximum supported values are 1023,255,63.
            /// </summary>
            /// <remarks>
            /// For very large disks, these values are bogus and LBA-aware software will use the LBA fields instead.
            /// </remarks>
            public CHSAddress ChsEnd
            {
                get => _chsEnd;
                set => _chsEnd = value;
            }

            /// <summary>
            /// LBA of first absolute sector in the partition. 
            /// </summary>
            /// <remarks>
            /// This value is only used by LBA-aware software.
            /// </remarks>
            public uint LbaStart
            {
                get => _lbaStart;
                set => _lbaStart = value;
            }

            /// <summary>
            /// Number of sectors in the partition. Must be non-zero for valid entries.
            /// </summary>
            /// <remarks>
            /// This value is only used by LBA-aware software.
            /// </remarks>
            public uint LbaLength
            {
                get => _lbaLength;
                set => _lbaLength = value;
            }

            /// <summary>
            /// Initialises an MBR partition entry
            /// </summary>
            /// <param name="active">Whether the partition is marked as active</param>
            /// <param name="type">The type of the partition</param>
            /// <param name="chsStart">CHS address of first absolute sector in the partition.</param>
            /// <param name="chsEnd">CHS address of last absolute sector in the partition.</param>
            /// <param name="lbaStart">LBA of first absolute sector in the partition.</param>
            /// <param name="lbaLength">Number of sectors in the partition.</param>
            /// <param name="offset">The offset of the partition in it's container file</param>
            /// <param name="length">The length of the partition</param>
            /// <param name="stream">The stream containing the partition data</param>
            public MbrPartitionEntry(bool active, MbrPartitionType type, CHSAddress chsStart, CHSAddress chsEnd, uint lbaStart, uint lbaLength, uint offset, uint length, Stream stream)
                : base(offset, length, stream)
            {
                _active = active;
                _type = type;
                _chsStart = chsStart;
                _chsEnd = chsEnd;
                _lbaStart = lbaStart;
                _lbaLength = lbaLength;
            }
        }

        /// <summary>
        /// The type of partition
        /// </summary>
        public enum MbrPartitionType : byte
        {
            /// <summary>
            /// An empty (unused) partition entry.
            /// </summary>
            [Display(Name = "Empty")]
            Empty = 0,

            /// <summary>
            /// A FAT12 primary partition in first physical 32 MB of the disk, or in a logical drive anywhere on the disk.
            /// </summary>
            [Display(Name = "FAT12")]
            Fat12 = 0x01,

            /// <summary>
            /// A FAT16 primary partition with fewer than 65,536 sectors (32 MB) in the first physical 32 MB of the disk, or in a logical drive anywhere on the disk.
            /// </summary>
            [Display(Name = "FAT16")]
            Fat16 = 0x04,

            /// <summary>
            /// Extended partition that can contain logical partitions. Must reside within the first physical 8 GB of the disk.
            /// </summary>
            [Display(Name = "Extended Partition")]
            Extended = 0x05,

            /// <summary>
            /// A FAT16B primary partition with 65,536 (32 MB) or more sectors in the first 8 GB of the disk, or in a logical drive anywhere on the disk.
            /// Also used for FAT12 and FAT16 volumes in primary partitions if they are not residing in first physical 32 MB of the disk.
            /// </summary>
            [Display(Name = "FAT16B")]
            Fat16B = 0x06,

            /// <summary>
            /// A primary partition containing HPFS, NTFS or exFAT file systems.
            /// </summary>
            [Display(Name = "HPFS/NTFS/exFAT")]
            HpfsNtfsExFat = 0x07,

            /// <summary>
            /// A FAT32 primary partition.
            /// </summary>
            [Display(Name = "FAT32")]
            Fat32 = 0x0B,

            /// <summary>
            /// A FAT32 primary partition that requires Logical Block Addressing for access.
            /// </summary>
            [Display(Name = "FAT32 (LBA)")]
            Fat32Lba = 0x0C,

            /// <summary>
            /// A FAT16B primary partition that requires Logical Block Addressing for access.
            /// </summary>
            [Display(Name = "FAT16B (LBA)")]
            Fat16BLba = 0x0E,

            /// <summary>
            /// Extended partition that requires Logical Block Addressing for access.
            /// </summary>
            [Display(Name = "Extended Partition (LBA)")]
            ExtendedLba = 0x0F,

            /// <summary>
            /// Linux swap space.
            /// </summary>
            [Display(Name = "Linux swap")]
            LinuxSwap = 0x82,

            /// <summary>
            /// Partition containing a native Linux file system such as the ext family etc.
            /// </summary>
            [Display(Name = "Linux native")]
            LinuxNative = 0x83,

            /// <summary>
            /// A protective MBR partition for GPT drives that prevents GPT-unaware software from overwriting the GPT partitions.
            /// </summary>
            [Display(Name = "GPT Protective Partition")]
            GptProtectivePartition = 0xEE,

            /// <summary>
            /// An EFI System Partition (ESP).
            /// </summary>
            [Display(Name = "EFI System Partition")]
            EfiSystemPartition = 0xEF,
        }
    }
}
