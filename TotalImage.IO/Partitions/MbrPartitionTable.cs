using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using TotalImage.Containers;

namespace TotalImage.Partitions
{
    /// <summary>
    /// A partition table class that represents a basic MBR partition table
    /// </summary>
    public class MbrPartitionTable : PartitionTable
    {
        private readonly uint _sectorSize;

        public override string DisplayName => "Master Boot Record";

        /// <inheritdoc />
        public MbrPartitionTable(Container container, uint sectorSize = 512) : base(container)
        {
            _sectorSize = sectorSize;
        }

        /// <inheritdoc />
        protected override IEnumerable<PartitionEntry> LoadPartitions()
        {
            using BinaryReader br = new BinaryReader(_container.Content, Encoding.ASCII, true);
            br.BaseStream.Seek(0x1FE, SeekOrigin.Begin);

            var signature = br.ReadUInt16();
            if (signature != 0xaa55)
            {
                throw new InvalidDataException();
            }

            br.BaseStream.Seek(0x1BE, SeekOrigin.Begin);

            var entries = ImmutableList.CreateBuilder<PartitionEntry>();
            for (int i = 0; i < 4; i++)
            {
                byte status = br.ReadByte();
                CHSAddress chsStart = new CHSAddress(br.ReadBytes(3));
                MbrPartitionType type = (MbrPartitionType)br.ReadByte();
                CHSAddress chsEnd = new CHSAddress(br.ReadBytes(3));
                uint lbaStart = br.ReadUInt32();
                uint lbaLength = br.ReadUInt32();

                if (type == MbrPartitionType.Empty)
                {
                    continue;
                }

                uint offset = lbaStart * _sectorSize;
                uint length = lbaLength * _sectorSize;
                var entry = new MbrPartitionEntry((status & 0x80) != 0, type, offset, length, new PartialStream(_container.Content, offset, length));
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

            /// <summary>
            /// Indicates whether this partition is marked as active
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
            /// Initialises an MBR partition entry
            /// </summary>
            /// <param name="active">Whether the partition is marked as active</param>
            /// <param name="type">The type of the partition</param>
            /// <param name="offset">The offset of the partition in it's container file</param>
            /// <param name="length">The length of the partition</param>
            /// <param name="stream">The stream containing the partition data</param>
            public MbrPartitionEntry(bool active, MbrPartitionType type, uint offset, uint length, Stream stream)
                : base(offset, length, stream)
            {
                _active = active;
                _type = type;
            }
        }

        /// <summary>
        /// The type of partition
        /// </summary>
        public enum MbrPartitionType : byte
        {
            /// <summary>
            /// An empty partition entry
            /// </summary>
            [Display(Name = "Empty")]
            Empty = 0,

            /// <summary>
            /// A FAT-12 partition for use with DOS
            /// </summary>
            [Display(Name = "DOS (FAT-12)")]
            DosFat12 = 0x01,

            /// <summary>
            /// A protective MBR partition for GPT drives
            /// </summary>
            [Display(Name = "GPT Protective Partition")]
            GptProtectivePartition = 0xEE,
        }
    }
}
