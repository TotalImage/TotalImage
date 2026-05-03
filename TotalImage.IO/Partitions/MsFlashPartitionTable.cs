using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using TotalImage.Containers;

namespace TotalImage.Partitions
{
    /// <summary>
    /// Represents a Microsoft Flash partition table.
    /// </summary>
    public class MsFlashPartitionTable : PartitionTable
    {
        /// <summary>
        /// Creates a Microsoft Flash partition table from a container.
        /// </summary>
        /// <param name="container">The container that holds the partition table.</param>
        public MsFlashPartitionTable(Container container)
            : base(container)
        {
        }

        /// <inheritdoc />
        public override string DisplayName => "Microsoft Flash";

        /// <inheritdoc />
        protected override IEnumerable<PartitionEntry> LoadPartitions()
        {
            _container.Content.Seek(0x808, SeekOrigin.Begin);

            Span<byte> buffer = new byte[8];
            _container.Content.Read(buffer);

            int reservedCount = BinaryPrimitives.ReadInt32LittleEndian(buffer[0..4]) / 16;
            int flashRegionCount = BinaryPrimitives.ReadInt32LittleEndian(buffer[4..8]) / 28;

            var entries = ImmutableList.CreateBuilder<PartitionEntry>();
            for (int i = 0; i < reservedCount; i++)
            {
                buffer = new byte[16];
                _container.Content.Read(buffer);
            }
            for (int i = 0; i < flashRegionCount; i++)
            {
                buffer = new byte[28];
                _container.Content.Read(buffer);
                uint type = BinaryPrimitives.ReadUInt32LittleEndian(buffer[0..4]);
                uint pstart = BinaryPrimitives.ReadUInt32LittleEndian(buffer[4..8]);
                uint psize = BinaryPrimitives.ReadUInt32LittleEndian(buffer[8..12]);
                uint lsize = BinaryPrimitives.ReadUInt32LittleEndian(buffer[12..16]);
                uint secblk = BinaryPrimitives.ReadUInt32LittleEndian(buffer[16..20]);
                uint bytblk = BinaryPrimitives.ReadUInt32LittleEndian(buffer[20..24]);
                uint compact = BinaryPrimitives.ReadUInt32LittleEndian(buffer[24..28]);

                entries.Add(new MsFlashRegionPartitionEntry(pstart, lsize == uint.MaxValue ? 0 : lsize * bytblk, _container.Content));
            }

            MbrPartitionTable mbr = new MbrPartitionTable(_container, 2048);
            entries.AddRange(mbr.Partitions);
            return entries.ToImmutableList();
        }

        /// <summary>
        /// Represents a Microsoft Flash region partition entry.
        /// </summary>
        public class MsFlashRegionPartitionEntry : PartitionEntry
        {
            /// <summary>
            /// Creates a Microsoft Flash region partition entry.
            /// </summary>
            /// <param name="offset">The byte offset of the partition.</param>
            /// <param name="length">The length of the partition in bytes.</param>
            /// <param name="stream">The stream containing the partition data.</param>
            public MsFlashRegionPartitionEntry(long offset, long length, Stream stream)
                : base(offset, length, stream)
            {
            }
        }
    }
}
