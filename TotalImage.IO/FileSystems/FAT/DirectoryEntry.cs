using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /*
     * This class represents the traditional 32-byte FAT directory entry, used in FAT12, FAT16 and FAT32
     */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DirectoryEntry
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string name; // “Short” file name limited to 11 characters.
        public FatAttributes attr; // File attributes.
        public byte ntRes; // Reserved. Must be set to 0. 
        public byte crtTimeTenth; // Component of the file creation time. Count of tenths of a second. 
        public ushort crtTime; // Creation time. Granularity is 2 seconds. 
        public ushort crtDate; // Creation date.
        public ushort lstAccDate; // Last access date. Last access is defined as a read or write operation performed on the file/directory described by this entry
        public ushort fstClusHI; //High word of first data cluster number for file/directory described by this entry. Only valid for volumes formatted FAT32. Must be set to 0 on volumes formatted FAT12/FAT16.
        public ushort wrtTime; // Last modification (write) time.
        public ushort wrtDate; // Last modification (write) date.
        public ushort fstClusLO; // Low word of first data cluster number for file/directory described by this entry. 
        public uint fileSize; // 32-bit quantity containing size in bytes of file/directory described by this entry. 

        public static DirectoryEntry Parse(byte[] bytes)
        {
            DirectoryEntry entry;

            using (var stream = new MemoryStream(bytes))
            using (var reader = new BinaryReader(stream, Encoding.ASCII))
            {
                entry.name = new string(reader.ReadChars(11));
                entry.attr = (FatAttributes)reader.ReadByte();
                entry.ntRes = reader.ReadByte();
                entry.crtTimeTenth = reader.ReadByte();
                entry.crtTime = reader.ReadUInt16();
                entry.crtDate = reader.ReadUInt16();
                entry.lstAccDate = reader.ReadUInt16();
                entry.fstClusHI = reader.ReadUInt16();
                entry.wrtTime = reader.ReadUInt16();
                entry.wrtDate = reader.ReadUInt16();
                entry.fstClusLO = reader.ReadUInt16();
                entry.fileSize = reader.ReadUInt32();
            }

            return entry;
        }

        public static IEnumerable<DirectoryEntry> ReadRootDirectory(Fat12 fat, bool includeDeleted = false)
        {
            var bpb = fat.BiosParameterBlock;
            var sector = bpb.ReservedLogicalSectors + (uint)(bpb.LogicalSectorsPerFAT * bpb.NumberOfFATs);

            return ReadDirectory(fat, sector, bpb.RootDirectoryEntries, includeDeleted);
        }

        public static IEnumerable<DirectoryEntry> ReadSubdirectory(Fat12 fat, DirectoryEntry entry, bool includeDeleted = false)
        {
            var cluster = (uint)(entry.fstClusHI << 16) | entry.fstClusLO;

            do
            {
                foreach (var subentry in ReadSubdirectory(fat, cluster, includeDeleted))
                {
                    yield return subentry;
                }

                cluster = fat.GetNextCluster(cluster);
            }
            while (cluster <= 0xFEF);
        }

        public static IEnumerable<DirectoryEntry> ReadSubdirectory(Fat12 fat, uint cluster, bool includeDeleted = false)
        {
            var bpb = fat.BiosParameterBlock;
            var sector = (cluster - 2) * bpb.LogicalSectorsPerCluster;
            var entries = bpb.LogicalSectorsPerCluster * bpb.BytesPerLogicalSector / 32;

            return ReadDirectory(fat, fat.DataAreaFirstSector + sector, entries, includeDeleted);
        }

        private static IEnumerable<DirectoryEntry> ReadDirectory(Fat12 fat, uint sector, int entries, bool includeDeleted)
        {
            var stream = fat.GetStream();
            using var reader = new BinaryReader(stream, Encoding.ASCII, true);

            for(var i = 0; i < entries; i++)
            {
                stream.Seek(sector * fat.BiosParameterBlock.BytesPerLogicalSector + i * 32, SeekOrigin.Begin);

                var firstByte = reader.ReadByte();

                /* 0x00/0xF6 = no more entries after this one, stop
                 * 0xE5/0x05 = deleted entry, skip for now
                 * 0x2E      = virtual . and .. folders, skip*/
                if (firstByte == 0x00 || firstByte == 0xF6) break;
                if (firstByte == 0x2E) continue;
                if ((firstByte == 0xE5 || firstByte == 0x05) && !includeDeleted) continue;
                if (firstByte == 0xE5 && includeDeleted)
                {
                    //This check is needed for old DOS 1.x disks that don't mark unused entries with 0x00 and instead use the deleted
                    //marker (0xE5), which can trip the code
                    if (reader.ReadUInt32() == 0xF6F6F6F6) break;
                    else stream.Seek(-4, SeekOrigin.Current);
                }

                stream.Seek(-1, SeekOrigin.Current);

                yield return DirectoryEntry.Parse(reader.ReadBytes(32));
            }
        }
    }
}
