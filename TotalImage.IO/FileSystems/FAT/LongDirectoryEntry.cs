using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// This struct represents the special directory entry used to store long
    /// file names.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public struct LongDirectoryEntry
    {
        /// <summary>
        /// The order of this entry in the sequence of long directory entries.
        /// If masked with 0x40, this indicates the entry is the last entry in
        /// the set (physically first).
        /// </summary>
        public byte ord;

        /// <summary>
        /// Characters 1-5 of the long name sub-component.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public char[] name1 = new char[5];

        /// <summary>
        /// Attributes - must be <c>FatAttributes.LongName</c>
        /// </summary>
        public FatAttributes attr;

        /// <summary>
        /// If zero, indicates a directory entry that is a sub-component of a
        /// long name. Other values are reserved for future extensions.
        /// </summary>
        public byte type;

        /// <summary>
        /// Checksum of the short directory entry at the end of the set.
        /// </summary>
        public byte chksum;

        /// <summary>
        /// Characters 6-11 of the long name sub-component.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public char[] name2 = new char[6];

        /// <summary>
        /// Must be zero.
        /// </summary>
        public ushort fstClusLO;

        /// <summary>
        /// Characters 12-13 of the long name sub-component.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public char[] name3 = new char[2];

        public static explicit operator LongDirectoryEntry(DirectoryEntry entry)
        {
            var lfnEntry = new LongDirectoryEntry();

            var name2 = new byte[12];
            var name3 = new byte[4];

            lfnEntry.ord = entry.name[0];

            Encoding.Unicode.GetChars(entry.name[1..11], lfnEntry.name1);

            lfnEntry.attr = entry.attr;
            lfnEntry.type = entry.ntRes;
            lfnEntry.chksum = entry.crtTimeTenth;

            BinaryPrimitives.WriteUInt16LittleEndian(name2.AsSpan()[0..2], entry.crtTime);
            BinaryPrimitives.WriteUInt16LittleEndian(name2.AsSpan()[2..4], entry.crtDate);
            BinaryPrimitives.WriteUInt16LittleEndian(name2.AsSpan()[4..6], entry.lstAccDate);
            BinaryPrimitives.WriteUInt16LittleEndian(name2.AsSpan()[6..8], entry.fstClusHI);
            BinaryPrimitives.WriteUInt16LittleEndian(name2.AsSpan()[8..10], entry.wrtTime);
            BinaryPrimitives.WriteUInt16LittleEndian(name2.AsSpan()[10..12], entry.wrtDate);
            Encoding.Unicode.GetChars(name2, lfnEntry.name2);

            lfnEntry.fstClusLO = entry.fstClusLO;

            BinaryPrimitives.WriteUInt32LittleEndian(name3, entry.fileSize);
            Encoding.Unicode.GetChars(name3, lfnEntry.name3);

            return lfnEntry;
        }

        public static string CombineEntries(LongDirectoryEntry[] entries)
        {
            var sb = new StringBuilder(entries.Length * 13);
            foreach(var entry in entries)
            {
                sb.Append(entry.name1);
                sb.Append(entry.name2);
                sb.Append(entry.name3);
            }

            var name = sb.ToString();
            var nullIndex = name.IndexOf('\0');
            return nullIndex > 0 ? name.Substring(0, nullIndex) : name;
        }

        public static byte GetShortNameChecksum(ReadOnlySpan<byte> filename)
        {
            if (filename.Length != 11) throw new ArgumentException();

            byte sum = 0;

            for (var i = 0; i < 11; i++)
            {
                sum = (byte)(((sum & 1) << 7) + (sum >> 1) + filename[i]);
            }

            return sum;
        }
    }
}
