using System.Buffers.Binary;
using System.IO;
using TotalImage.Containers;

namespace TotalImage.Partitions
{
    /// <summary>
    /// A factory class that can create an MBR or GPT partition table
    /// </summary>
    public class MbrGptFactory : IPartitionTableFactory
    {
        /// <inheritdoc />
        public PartitionTable? TryLoadPartitionTable(Container container)
        {
            Stream Content = container.Content;
            Content.Seek(0x1FE, SeekOrigin.Begin);

            byte[] signatureBytes = new byte[2];
            Content.Read(signatureBytes);
            ushort signature = BinaryPrimitives.ReadUInt16LittleEndian(signatureBytes);

            if (signature != 0xAA55)
            {
                return null;
            }

            MbrPartitionTable mbrPartition = new MbrPartitionTable(container);
            if (mbrPartition.Partitions.Count >= 1)
            {
                //GPT, return and let the GPT factory handle this
                if (mbrPartition.Partitions[0] is MbrPartitionTable.MbrPartitionEntry entry
                    && (entry.Offset + entry.Length) > uint.MaxValue
                    && entry.Type == MbrPartitionTable.MbrPartitionType.GptProtectivePartition)
                {
                    return new GptPartitionTable(container);
                }

                // check partitions seem fine (ie, no overlapping)
                bool sanity = true;
                long lastOffset = 512;
                foreach (var partition in mbrPartition.Partitions)
                {
                    sanity &= (partition.Offset >= lastOffset);
                    sanity &= (partition.Length > 0);
                    lastOffset = partition.Offset + partition.Length;
                    sanity &= (lastOffset <= Content.Length);
                }

                if (!sanity && CheckIfUnpartitioned(Content))
                    return null;
            }
            else
            {
                if (CheckIfUnpartitioned(Content))
                    return null;
            }

            return mbrPartition;
        }

        /// <summary>
        /// Checks if the boot sector/system area is immediately followed by a FAT12 file allocation table, which would make this a floppy image.
        /// </summary>
        /// <param name="Content">Content stream to check</param>
        /// <returns></returns>
        private bool CheckIfUnpartitioned(Stream Content)
        {
            // Check for 86-DOS 0.x and 1.x floppy formats
            // They contain reserved system areas, so our FAT-after-boot-sect
            // algo won't work on them
            if (Content.Length == 256256)
            {
                // 8" SSSD floppy
                return true;
            }
            else if (Content.Length == 92160)
            {
                // Cromemco 5.25" SSSD
                return true;
            }
            else if (Content.Length == 89600)
            {
                // NorthStar 5.25" SSSD
                return true;
            }
            else if (Content.Length == 163840)
            {
                // IBM PC 5.25" SSDD
                return true;
            }
            else if (Content.Length == 630784)
            {
                // 8" SSDD
                return true;
            }
            else if (Content.Length == 1261568)
            {
                // 8" DSDD
                return true;
            }
            Content.Seek(0x201, SeekOrigin.Begin); //TODO: DON'T ASSUME 512 BYTE SECT!
            byte[] fatBytes = new byte[2];
            Content.Read(fatBytes);
            ushort fatSignature = BinaryPrimitives.ReadUInt16LittleEndian(fatBytes);

            return fatSignature == 0xFFFF;
        }
    }
}
