using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        ImmutableArray<byte> fileName;

        /// <summary>
        /// File attributes.
        /// </summary>
        FatAttributes attributes;

        /// <summary>
        /// Reserved. Must be set to 0.
        /// </summary>
        byte ntByte;

        /// <summary>
        /// Component of the file creation time. Count of tenths of a second.
        /// </summary>
        byte creationMSec;

        /// <summary>
        /// Creation time. Granularity is 2 seconds.
        /// </summary>
        ushort creationTime;

        /// <summary>
        /// Creation date.
        /// </summary>
        ushort creationDate;

        /// <summary>
        /// Last access date. Last access is defined as a read or write operation performed on the file/directory described by this entry
        /// </summary>
        ushort lastAccessDate;

        /// <summary>
        /// High word of first data cluster number for file/directory described by this entry. Only valid for volumes formatted FAT32. Must be set to 0 on volumes formatted FAT12/FAT16.
        /// </summary>
        ushort firstClusterOfFileHi;

        /// <summary>
        /// Last modification (write) time.
        /// </summary>
        ushort lastWriteTime;

        /// <summary>
        /// Last modification (write) date.
        /// </summary>
        ushort lastWriteDate;

        /// <summary>
        /// Low word of first data cluster number for file/directory described by this entry.
        /// </summary>
        ushort firstClusterOfFile;

        /// <summary>
        /// 32-bit quantity containing size in bytes of file/directory described by this entry.
        /// </summary>
        uint fileSize;

        public DirectoryEntry(ReadOnlySpan<byte> entry)
        {
            fileName = entry[0..11].ToImmutableArray();
            attributes = (FatAttributes)entry[11];
            ntByte = entry[12];
            creationMSec = entry[13];
            creationTime = BinaryPrimitives.ReadUInt16LittleEndian(entry[14..16]);
            creationDate = BinaryPrimitives.ReadUInt16LittleEndian(entry[16..18]);
            lastAccessDate = BinaryPrimitives.ReadUInt16LittleEndian(entry[18..20]);
            firstClusterOfFileHi = BinaryPrimitives.ReadUInt16LittleEndian(entry[20..22]);
            lastWriteTime = BinaryPrimitives.ReadUInt16LittleEndian(entry[22..24]);
            lastWriteDate = BinaryPrimitives.ReadUInt16LittleEndian(entry[24..26]);
            firstClusterOfFile = BinaryPrimitives.ReadUInt16LittleEndian(entry[26..28]);
            fileSize = BinaryPrimitives.ReadUInt32LittleEndian(entry[28..32]);
        }

        public string BaseName => Encoding.ASCII.GetString(fileName.AsSpan()[0..8]).Trim();
        public string Extension => Encoding.ASCII.GetString(fileName.AsSpan()[8..11]).Trim();

        public string FileName => $"{BaseName}{(!string.IsNullOrWhiteSpace(Extension) ? "." : "")}{Extension}";
        public ReadOnlySpan<byte> FileNameBytes => fileName.AsSpan();

        public DateTime? CreationTime => FatDateTime.ToDateTime(creationDate, creationTime, creationMSec);
        public DateTime? LastAccessTime => FatDateTime.ToDateTime(lastAccessDate);
        public DateTime? LastWriteTime => FatDateTime.ToDateTime(lastWriteDate, lastWriteTime);

        public FatAttributes Attributes => attributes;

        public uint FirstClusterOfFile => (uint)(firstClusterOfFileHi << 16) | firstClusterOfFile;
        public uint FileSize => fileSize;

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
            EnumerateSubdirectory(fat, entry.FirstClusterOfFile, includeDeleted);

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

                    if (lfnEntry.Type != 0)
                    {
                        // Type is supposed to be zero
                        lfnStack.Clear();
                    }
                    else if (lfnEntry.Ordinal == 0xE5)
                    {
                        // This is a deleted LFN entry
                        lfnStack.Clear();
                    }
                    else if (Convert.ToBoolean(lfnEntry.Ordinal & 0x40))
                    {
                        // This is the first LFN entry
                        lfnStack.Clear();
                        lfnStack.Push(lfnEntry);
                    }
                    else if (lfnStack.TryPeek(out var previousLfnEntry))
                    {
                        if ((previousLfnEntry.Ordinal & 0x1F) != (lfnEntry.Ordinal & 0x1F) + 1)
                        {
                            // The LFN entry is out of order
                            lfnStack.Clear();
                        }
                        else if (previousLfnEntry.Checksum != lfnEntry.Checksum)
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

                if (lfnStack.TryPeek(out var topLfnEntry) && topLfnEntry.Checksum != LongDirectoryEntry.GetShortNameChecksum(entry[0..11]))
                {
                    lfnStack.Clear();
                }

                yield return (new DirectoryEntry(entry), lfnStack.ToArray());

                lfnStack.Clear();
            }
        }
    }
}
