using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// This class represents the traditional 32-byte FAT directory entry, used in FAT12, FAT16 and FAT32
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DirectoryEntry
    {
        /// <summary>
        /// “Short” file name limited to 11 characters.
        /// </summary>
        public byte[] name;

        /// <summary>
        /// File attributes.
        /// </summary>
        public FatAttributes attr;

        /// <summary>
        /// Reserved. Must be set to 0.
        /// </summary>
        public byte ntRes;

        /// <summary>
        /// Component of the file creation time. Count of tenths of a second.
        /// </summary>
        public byte crtTimeTenth;

        /// <summary>
        /// Creation time. Granularity is 2 seconds.
        /// </summary>
        public ushort crtTime;

        /// <summary>
        /// Creation date.
        /// </summary>
        public ushort crtDate;

        /// <summary>
        /// Last access date. Last access is defined as a read or write operation performed on the file/directory described by this entry
        /// </summary>
        public ushort lstAccDate;

        /// <summary>
        /// High word of first data cluster number for file/directory described by this entry. Only valid for volumes formatted FAT32. Must be set to 0 on volumes formatted FAT12/FAT16.
        /// </summary>
        public ushort fstClusHI;

        /// <summary>
        /// Last modification (write) time.
        /// </summary>
        public ushort wrtTime;

        /// <summary>
        /// Last modification (write) date.
        /// </summary>
        public ushort wrtDate;

        /// <summary>
        /// Low word of first data cluster number for file/directory described by this entry.
        /// </summary>
        public ushort fstClusLO;

        /// <summary>
        /// 32-bit quantity containing size in bytes of file/directory described by this entry.
        /// </summary>
        public uint fileSize;

        public static DirectoryEntry Parse(BinaryReader reader)
            => new DirectoryEntry
            {
                name = reader.ReadBytes(11),
                attr = (FatAttributes)reader.ReadByte(),
                ntRes = reader.ReadByte(),
                crtTimeTenth = reader.ReadByte(),
                crtTime = reader.ReadUInt16(),
                crtDate = reader.ReadUInt16(),
                lstAccDate = reader.ReadUInt16(),
                fstClusHI = reader.ReadUInt16(),
                wrtTime = reader.ReadUInt16(),
                wrtDate = reader.ReadUInt16(),
                fstClusLO = reader.ReadUInt16(),
                fileSize = reader.ReadUInt32()
            };

        public static IEnumerable<DirectoryEntry> ReadRootDirectory(FatFileSystem fat, bool includeDeleted = false)
        {
            var bpb = fat.BiosParameterBlock;
            var sector = bpb.ReservedLogicalSectors + (uint)(bpb.LogicalSectorsPerFAT * bpb.NumberOfFATs);

            return ReadDirectory(fat, sector, bpb.RootDirectoryEntries, includeDeleted);
        }

        public static IEnumerable<DirectoryEntry> ReadSubdirectory(FatFileSystem fat, DirectoryEntry entry, bool includeDeleted = false)
        {
            var cluster = (uint?)(entry.fstClusHI << 16) | entry.fstClusLO;

            do
            {
                foreach (var subentry in ReadSubdirectory(fat, cluster.Value, includeDeleted))
                {
                    yield return subentry;
                }

                cluster = fat.GetNextCluster(cluster.Value);
            }
            while (cluster.HasValue);
        }

        public static IEnumerable<DirectoryEntry> ReadSubdirectory(FatFileSystem fat, uint cluster, bool includeDeleted = false)
        {
            var bpb = fat.BiosParameterBlock;
            var sector = (cluster - 2) * bpb.LogicalSectorsPerCluster;
            var entries = bpb.LogicalSectorsPerCluster * bpb.BytesPerLogicalSector / 32;

            return ReadDirectory(fat, fat.DataAreaFirstSector + sector, entries, includeDeleted);
        }

        private static IEnumerable<DirectoryEntry> ReadDirectory(FatFileSystem fat, uint sector, int entries, bool includeDeleted)
        {
            var stream = fat.GetStream();
            using var reader = new BinaryReader(stream, Encoding.ASCII, true);

            for(var i = 0; i < entries; i++)
            {
                stream.Seek(sector * fat.BiosParameterBlock.BytesPerLogicalSector + i * 32, SeekOrigin.Begin);

                var entry = DirectoryEntry.Parse(reader);

                /* 0x00/0xF6 = no more entries after this one, stop
                 * 0xE5/0x05 = deleted entry, skip for now
                 * 0x2E      = virtual . and .. folders, skip*/
                if (entry.name[0] == 0x00 || entry.name[0] == 0xF6) break;
                if (entry.name[0] == 0x2E) continue;
                if (entry.name[0] == 0xE5 || entry.name[0] == 0x05)
                {
                    //This check is needed for old DOS 1.x disks that don't mark unused entries with 0x00 and instead use the deleted
                    //marker (0xE5), which can trip the code
                    if (BitConverter.ToUInt32(entry.name, 1) == 0xF6F6F6F6) break;
                    if (!includeDeleted) continue;
                }

                yield return entry;
            }
        }
    }
}
