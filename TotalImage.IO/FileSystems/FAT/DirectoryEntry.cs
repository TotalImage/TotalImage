using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using TotalImage.FileSystems.BPB;

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

        public static IEnumerable<(DirectoryEntry, LongDirectoryEntry[])> EnumerateRootDirectory(FatFileSystem fat, bool includeDeleted = false)
        {
            if (fat.BiosParameterBlock is Fat32BiosParameterBlock { RootDirectoryEntries: 0, RootDirectoryCluster: var firstCluster })
            {
                return EnumerateSubdirectory(fat, firstCluster, includeDeleted);
            }

            var stream = fat.GetStream();
            var buffer = new byte[fat.BiosParameterBlock.RootDirectoryEntries * 32];

            stream.Position = (fat.ReservedSectors + fat.ClusterMapsSectors) * fat.BiosParameterBlock.BytesPerLogicalSector;
            stream.Read(buffer);

            return EnumerateDirectory(buffer, includeDeleted);
        }

        public static IEnumerable<(DirectoryEntry, LongDirectoryEntry[])> EnumerateSubdirectory(FatFileSystem fat, DirectoryEntry entry, bool includeDeleted = false) =>
            EnumerateSubdirectory(fat, (uint)(entry.fstClusHI << 16) | entry.fstClusLO, includeDeleted);

        public static IEnumerable<(DirectoryEntry, LongDirectoryEntry[])> EnumerateSubdirectory(FatFileSystem fat, uint firstCluster, bool includeDeleted = false)
        {
            var stream = new FatDataStream(fat, firstCluster);
            var buffer = new byte[stream.Length];

            stream.Read(buffer);

            return EnumerateDirectory(buffer, includeDeleted);
        }

        private static IEnumerable<(DirectoryEntry, LongDirectoryEntry[])> EnumerateDirectory(ReadOnlyMemory<byte> buffer, bool includeDeleted)
        {
            var lfnStack = new Stack<LongDirectoryEntry>();

            for (var i = 0; i < buffer.Length; i += 32)
            {
                var entry = buffer.Slice(i, 32).Span;

                if (entry[0] is 0x00 or 0xF6)
                {
                    // No more entries after this one, stop
                    break;
                }

                if (entry[11] == (byte)FatAttributes.LongName)
                {
                    var lfnEntry = new LongDirectoryEntry(entry);

                    if (lfnEntry.type != 0)
                    {
                        // Type is supposed to be zero
                        lfnStack.Clear();
                    }
                    else if (lfnEntry.ord == 0xE5)
                    {
                        // This is a deleted LFN entry
                        lfnStack.Clear();
                    }
                    else if (Convert.ToBoolean(lfnEntry.ord & 0x40))
                    {
                        // This is the first LFN entry
                        lfnStack.Clear();
                        lfnStack.Push(lfnEntry);
                    }
                    else if (lfnStack.TryPeek(out var previousLfnEntry))
                    {
                        if ((previousLfnEntry.ord & 0x1F) != (lfnEntry.ord & 0x1F) + 1)
                        {
                            // The LFN entry is out of order
                            lfnStack.Clear();
                        }
                        else if (previousLfnEntry.chksum != lfnEntry.chksum)
                        {
                            // Short name checksum is different from the last entry
                            lfnStack.Clear();
                        }
                        else
                        {
                            lfnStack.Push(lfnEntry);
                        }
                    }

                    continue;
                }

                if (entry[0] == 0x2E)
                {
                    // Virtual . and .. folders, skip
                    continue;
                }

                if (entry[0] is 0xE5 or 0x05)
                {
                    // Deleted entries
                    if (!includeDeleted)
                    {
                        continue;
                    }

                    // Some old DOS 1.x disks don't mark unused entries with 0x00
                    // and instead use the deleted marker (0xE5)
                    if (BinaryPrimitives.ReadUInt32LittleEndian(entry[1..5]) == 0xF6F6F6F6)
                    {
                        break;
                    }
                }

                if (lfnStack.TryPeek(out var topLfnEntry) && topLfnEntry.chksum != LongDirectoryEntry.GetShortNameChecksum(entry[0..11]))
                {
                    lfnStack.Clear();
                }

                yield return (new DirectoryEntry(entry), lfnStack.ToArray());

                lfnStack.Clear();
            }
        }
    }
}
