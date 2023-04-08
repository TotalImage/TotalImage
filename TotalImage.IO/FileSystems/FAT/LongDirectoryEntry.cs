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
        ImmutableArray<char> name1;

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
        ImmutableArray<char> name2;

        /// <summary>
        /// Must be zero.
        /// </summary>
        ushort mustBeZero;

        /// <summary>
        /// Characters 12-13 of the long name sub-component.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        ImmutableArray<char> name3;

        public LongDirectoryEntry(ReadOnlySpan<byte> entry) : this()
        {
            ordinal = entry[0];

            var name1 = new char[5];
            Encoding.Unicode.GetChars(entry[1..11], name1);
            this.name1 = name1.ToImmutableArray();

            attributes = (FatAttributes)entry[11];
            type = entry[12];
            checksum = entry[13];

            var name2 = new char[6];
            Encoding.Unicode.GetChars(entry[14..26], name2);
            this.name2 = name2.ToImmutableArray();

            mustBeZero = BinaryPrimitives.ReadUInt16LittleEndian(entry[26..28]);

            var name3 = new char[2];
            Encoding.Unicode.GetChars(entry[28..32], name3);
            this.name3 = name3.ToImmutableArray();
        }

        public int Ordinal => ordinal;
        public byte Type => type;
        public byte Checksum => checksum;

        public static string CombineEntries(LongDirectoryEntry[] entries)
        {
            var sb = new StringBuilder(entries.Length * 13);
            foreach(var entry in entries)
            {
                sb.Append(entry.name1.AsSpan());
                sb.Append(entry.name2.AsSpan());
                sb.Append(entry.name3.AsSpan());
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
