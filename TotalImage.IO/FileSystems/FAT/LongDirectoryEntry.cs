using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// This struct represents the special directory entry used to store long file names on FAT file systems.
    /// </summary>
    public struct LongDirectoryEntry
    {
        private byte ordinal;
        private string name1; //Characters 1-5 of the long name sub-component.
        private FatAttributes attributes;
        private byte type;
        private byte checksum;
        private string name2; //Characters 6-11 of the long name sub-component.
        private ushort mustBeZero; //Must be zero.
        private string name3; //Characters 12-13 of the long name sub-component.

        /// <summary>
        /// The order of this entry in the sequence of long directory entries. If masked with 0x40, this indicates the entry is the last entry in
        /// the set (physically first).
        /// </summary>
        public int Ordinal => ordinal;

        /// <summary>
        /// If zero, indicates a directory entry that is a sub-component of a
        /// long name. Other values are reserved for future extensions.
        /// </summary>
        public byte Type => type;

        /// <summary>
        /// Checksum of the short directory entry at the end of the set.
        /// </summary>
        public byte Checksum => checksum;

        /// <summary>
        /// Attributes - must be <c>FatAttributes.LongName</c>
        /// </summary>
        public FatAttributes Attributes => attributes;

        /// <summary>
        /// Creates a new LongDirectoryEntry from the provided byte span.
        /// </summary>
        /// <param name="entry">The byte span to parse</param>
        public LongDirectoryEntry(ReadOnlySpan<byte> entry)
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

        /// <summary>
        /// Creates a new LongDirectoryEntry for writing, given a 13-character chunk of the long name,
        /// the ordinal sequence number, and the checksum of the associated short-name entry.
        /// </summary>
        /// <param name="ordinalValue">
        /// The 1-based sequence number of this entry within the LFN set.
        /// Set bit 6 (OR with 0x40) to mark this as the last entry in the sequence (physically written first).
        /// </param>
        /// <param name="nameChunk">
        /// Exactly 13 Unicode characters for this LFN slot. Pad with U+FFFF up to 13 if the name ends mid-chunk,
        /// and with U+0000 for any remaining positions after the NUL terminator.
        /// </param>
        /// <param name="checksumValue">The checksum of the 11-byte short-name entry.</param>
        public LongDirectoryEntry(byte ordinalValue, ReadOnlySpan<char> nameChunk, byte checksumValue)
        {
            if (nameChunk.Length != 13)
                throw new ArgumentException("nameChunk must be exactly 13 characters.", nameof(nameChunk));

            ordinal = ordinalValue;
            attributes = FatAttributes.LongName;
            type = 0;
            checksum = checksumValue;
            mustBeZero = 0;

            // Split the 13 chars into name1 (5), name2 (6), name3 (2)
            name1 = new string(nameChunk[..5]);
            name2 = new string(nameChunk[5..11]);
            name3 = new string(nameChunk[11..13]);
        }

        /// <summary>
        /// Serializes this LFN entry as 32 bytes and writes it to <paramref name="stream"/>.
        /// </summary>
        public readonly void WriteTo(Stream stream)
        {
            var buf = new byte[32];
            buf[0] = ordinal;
            Encoding.Unicode.GetBytes(name1, buf.AsSpan(1, 10));
            buf[11] = (byte)attributes;
            buf[12] = type;
            buf[13] = checksum;
            Encoding.Unicode.GetBytes(name2, buf.AsSpan(14, 12));
            BinaryPrimitives.WriteUInt16LittleEndian(buf.AsSpan(26, 2), mustBeZero);
            Encoding.Unicode.GetBytes(name3, buf.AsSpan(28, 4));
            stream.Write(buf);
        }

        /// <summary>
        /// Combines all three special entries into a string representing a single long file name.
        /// </summary>
        /// <param name="entries">The entries to combine</param>
        /// <returns>A string representing a single long file name</returns>
        public static string CombineEntries(ICollection<LongDirectoryEntry> entries)
        {
            var sb = new StringBuilder(entries.Count * 13);
            foreach(var entry in entries)
            {
                sb.Append(entry.name1);
                sb.Append(entry.name2);
                sb.Append(entry.name3);
            }

            var result = sb.ToString();
            var nullIndex = result.IndexOf('\0');
            return nullIndex >= 0 ? result[..nullIndex] : result;
        }

        /// <summary>
        /// Returns the checksum byte of the short directory entry.
        /// </summary>
        /// <param name="filename">The filename to get the checksum for</param>
        /// <returns>Checksum byte of the shorty directory entry</returns>
        /// <exception cref="ArgumentException"></exception>
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
