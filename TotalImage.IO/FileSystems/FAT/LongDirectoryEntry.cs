using System;
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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
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
        public char[] name1;

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
        public char[] name2;

        /// <summary>
        /// Must be zero.
        /// </summary>
        public ushort fstClusLO;

        /// <summary>
        /// Characters 12-13 of the long name sub-component.
        /// </summary>
        public char[] name3;

        public LongDirectoryEntry(DirectoryEntry entry)
        {
            var name1 = new byte[10];
            var name2 = new byte[12];
            var name3 = new byte[4];

            ord = entry.name[0];

            Buffer.BlockCopy(entry.name, 1, name1, 0, 10);
            this.name1 = Encoding.Unicode.GetChars(name1);

            attr = entry.attr;
            type = entry.ntRes;
            chksum = entry.crtTimeTenth;

            BitConverter.GetBytes(entry.crtTime).CopyTo(name2, 0);
            BitConverter.GetBytes(entry.crtDate).CopyTo(name2, 2);
            BitConverter.GetBytes(entry.lstAccDate).CopyTo(name2, 4);
            BitConverter.GetBytes(entry.fstClusHI).CopyTo(name2, 6);
            BitConverter.GetBytes(entry.wrtTime).CopyTo(name2, 8);
            BitConverter.GetBytes(entry.wrtDate).CopyTo(name2, 10);
            this.name2 = Encoding.Unicode.GetChars(name2);

            fstClusLO = entry.fstClusLO;

            BitConverter.GetBytes(entry.fileSize).CopyTo(name3, 0);
            this.name3 = Encoding.Unicode.GetChars(name3);
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

        public static byte GetShortNameChecksum(byte[] filename)
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