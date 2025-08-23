using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;
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

            uint sectorSize = 512;

            // Discover sector size on MS Flash images
            uint attemptedFlashSectorSize = 512;
            Span<byte> flashSignatureBuffer = new byte[8];
            while (attemptedFlashSectorSize <= 65536 && (attemptedFlashSectorSize + 8) <= Content.Length)
            {
                Content.Seek(attemptedFlashSectorSize, SeekOrigin.Begin);
                Content.Read(flashSignatureBuffer);
                if (Encoding.ASCII.GetString(flashSignatureBuffer) == "MSFLSH50")
                {
                    sectorSize = attemptedFlashSectorSize;
                    break;
                }

                attemptedFlashSectorSize *= 2;
            }

            MbrPartitionTable mbrPartition = new MbrPartitionTable(container, sectorSize);
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
        /// Checks if the boot sector is immediately followed by a FAT12 file allocation table, which would make this an unpartitioned disk.
        /// </summary>
        /// <param name="Content">Content stream to check</param>
        /// <returns></returns>
        private bool CheckIfUnpartitioned(Stream Content)
        {
            Content.Seek(0x201, SeekOrigin.Begin);
            byte[] fatBytes = new byte[2];
            Content.Read(fatBytes);
            ushort fatSignature = BinaryPrimitives.ReadUInt16LittleEndian(fatBytes);

            return fatSignature == 0xFFFF;
        }
    }
}
