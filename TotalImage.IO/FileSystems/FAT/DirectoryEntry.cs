using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// This class represents the traditional 32-byte FAT directory entry, used in FAT12, FAT16 and FAT32
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DirectoryEntry
    {
        /// <summary>
        /// “Short” file name limited to 11 characters.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
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

        public DirectoryEntry(ReadOnlySpan<byte> entry)
        {
            name = entry[0..11].ToArray();
            attr = (FatAttributes)entry[11];
            ntRes = entry[12];
            crtTimeTenth = entry[13];
            crtTime = BinaryPrimitives.ReadUInt16LittleEndian(entry[14..16]);
            crtDate = BinaryPrimitives.ReadUInt16LittleEndian(entry[16..18]);
            lstAccDate = BinaryPrimitives.ReadUInt16LittleEndian(entry[18..20]);
            fstClusHI = BinaryPrimitives.ReadUInt16LittleEndian(entry[20..22]);
            wrtTime = BinaryPrimitives.ReadUInt16LittleEndian(entry[22..24]);
            wrtDate = BinaryPrimitives.ReadUInt16LittleEndian(entry[24..26]);
            fstClusLO = BinaryPrimitives.ReadUInt16LittleEndian(entry[26..28]);
            fileSize = BinaryPrimitives.ReadUInt32LittleEndian(entry[28..32]);
        }

        public string BaseName => Encoding.ASCII.GetString(name[0..8]).Trim();
        public string Extension => Encoding.ASCII.GetString(name[8..11]).Trim();

        public string Name => $"{BaseName}{(!string.IsNullOrWhiteSpace(Extension) ? "." : "")}{Extension}";

        public DateTime? CreationTime => FatDateTime.ToDateTime(crtDate, crtTime, crtTimeTenth);
        public DateTime? LastAccessTime => FatDateTime.ToDateTime(lstAccDate);
        public DateTime? LastWriteTime => FatDateTime.ToDateTime(wrtDate, wrtTime);

        public FileAttributes Attributes => (FileAttributes)attr;

        public static IEnumerable<DirectoryEntry> ReadRootDirectory(FatFileSystem fat, bool includeDeleted = false)
        {
            var stream = fat.GetStream();
            stream.Position = (fat.ReservedSectors + fat.ClusterMapsSectors) * fat.BiosParameterBlock.BytesPerLogicalSector;

            return ReadDirectory(stream, fat.BiosParameterBlock.RootDirectoryEntries, includeDeleted);
        }

        public static IEnumerable<DirectoryEntry> ReadSubdirectory(FatFileSystem fat, DirectoryEntry entry, bool includeDeleted = false)
            => ReadDirectory(new FatDataStream(fat, entry, true), int.MaxValue, includeDeleted);

        public static IEnumerable<DirectoryEntry> ReadSubdirectory(FatFileSystem fat, uint cluster, bool includeDeleted = false)
            => ReadDirectory(new FatDataStream(fat, cluster), int.MaxValue, includeDeleted);

        private static IEnumerable<DirectoryEntry> ReadDirectory(Stream stream, int entries, bool includeDeleted)
        {
            using var reader = new BinaryReader(stream, Encoding.ASCII, true);
            var position = stream.Position;

            var buffer = new byte[32];

            if (entries == int.MaxValue) entries = (int)(stream.Length / 32);

            for(var i = 0; i < entries; i++)
            {
                if (position != stream.Position) stream.Position = position;

                reader.Read(buffer);
                var entry = new DirectoryEntry(buffer);

                position = stream.Position;

                /* 0x00/0xF6 = no more entries after this one, stop
                 * 0xE5/0x05 = deleted entry, skip for now
                 * 0x2E      = virtual . and .. folders, skip*/
                if (entry.name[0] == 0x00 || entry.name[0] == 0xF6) break;
                if (entry.name[0] == 0x2E) continue;
                if (entry.name[0] == 0xE5 || entry.name[0] == 0x05)
                {
                    //This check is needed for old DOS 1.x disks that don't mark unused entries with 0x00 and instead use the deleted
                    //marker (0xE5), which can trip the code
                    if (BinaryPrimitives.ReadUInt32LittleEndian(entry.name) == 0xF6F6F6F6) break;
                    if (!includeDeleted) continue;
                }

                yield return entry;
            }
        }
    }
}
