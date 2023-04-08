using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        byte ordinal;

        /// <summary>
        /// Characters 1-5 of the long name sub-component.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        string name1;

        /// <summary>
        /// Attributes - must be <c>FatAttributes.LongName</c>
        /// </summary>
        FatAttributes attributes;

        /// <summary>
        /// If zero, indicates a directory entry that is a sub-component of a
        /// long name. Other values are reserved for future extensions.
        /// </summary>
        byte type;

        /// <summary>
        /// Checksum of the short directory entry at the end of the set.
        /// </summary>
        byte checksum;

        /// <summary>
        /// Characters 6-11 of the long name sub-component.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        string name2;

        /// <summary>
        /// Must be zero.
        /// </summary>
        ushort mustBeZero;

        /// <summary>
        /// Characters 12-13 of the long name sub-component.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        string name3;

        public LongDirectoryEntry(ReadOnlySpan<byte> entry) : this()
        {
            ordinal = entry[0];
            name1 = Encoding.Unicode.GetString(entry[1..11]);
            attributes = (FatAttributes)entry[11];
            type = entry[12];
            checksum = entry[13];
            name2 = Encoding.Unicode.GetString(entry[14..26]);
            mustBeZero = BinaryPrimitives.ReadUInt16LittleEndian(entry[26..28]);
            name3 = Encoding.Unicode.GetString(entry[28..32]);
        }

        public int Ordinal => ordinal;
        public byte Type => type;
        public byte Checksum => checksum;

        public static string CombineEntries(ICollection<LongDirectoryEntry> entries)
        {
            var sb = new StringBuilder(entries.Count * 13);
            foreach(var entry in entries)
            {
                sb.Append(entry.name1);
                sb.Append(entry.name2);
                sb.Append(entry.name3);
            }

            return sb.ToString().Split('\0')[0];
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
