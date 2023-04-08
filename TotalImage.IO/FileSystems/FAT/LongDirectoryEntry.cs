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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public char[] name2;

        /// <summary>
        /// Must be zero.
        /// </summary>
        public ushort fstClusLO;

        /// <summary>
        /// Characters 12-13 of the long name sub-component.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public char[] name3;

        public LongDirectoryEntry(ReadOnlySpan<byte> entry) : this()
        {
            ord = entry[0];

            name1 = new char[5];
            Encoding.Unicode.GetChars(entry[1..11], name1);

            attr = (FatAttributes)entry[11];
            type = entry[12];
            chksum = entry[13];

            name2 = new char[6];
            Encoding.Unicode.GetChars(entry[14..26], name2);

            fstClusLO = BinaryPrimitives.ReadUInt16LittleEndian(entry[26..28]);

            name3 = new char[2];
            Encoding.Unicode.GetChars(entry[28..32], name3);
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
