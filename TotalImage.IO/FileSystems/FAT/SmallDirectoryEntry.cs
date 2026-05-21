using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// Represents the 16-byte FAT directory entry used by 86-DOS versions prior to 0.42.
    /// Layout: 11-byte 8.3 filename, 2-byte first cluster (little-endian), 3-byte file size (24-bit little-endian).
    /// There are no attributes, timestamps, or subdirectory support.
    /// </summary>
    public struct SmallDirectoryEntry
    {
        private ImmutableArray<byte> fileName;
        private ushort firstCluster;
        private uint fileSize; // only low 24 bits are used

        /// <summary>
        /// File name without extension (8 characters max).
        /// </summary>
        public string BaseName => Encoding.ASCII.GetString(fileName.AsSpan()[0..8]).Trim();

        /// <summary>
        /// File extension (3 characters max).
        /// </summary>
        public string Extension => Encoding.ASCII.GetString(fileName.AsSpan()[8..11]).Trim();

        /// <summary>
        /// File name with extension.
        /// </summary>
        public string FileName => $"{BaseName}{(!string.IsNullOrWhiteSpace(Extension) ? "." : "")}{Extension}";

        /// <summary>
        /// Returns the filename with extension as a byte span.
        /// </summary>
        public ReadOnlySpan<byte> FileNameBytes => fileName.AsSpan();

        /// <summary>
        /// First data cluster number.
        /// </summary>
        public uint FirstClusterOfFile => firstCluster;

        /// <summary>
        /// File size in bytes (24-bit value, max ~16 MB).
        /// </summary>
        public uint FileSize => fileSize & 0xFFFFFF;

        /// <summary>
        /// Creates a small (16-byte) FAT directory entry from a raw 16-byte buffer.
        /// </summary>
        /// <param name="entry">The raw 16-byte directory entry buffer.</param>
        public SmallDirectoryEntry(ReadOnlySpan<byte> entry)
        {
            fileName = entry[0..11].ToImmutableArray();
            firstCluster = (ushort)(entry[11] | (entry[12] << 8));
            fileSize = (uint)(entry[13] | (entry[14] << 8) | (entry[15] << 16));
        }

        /// <summary>
        /// Enumerates the 16-byte directory entries from the root directory of an early FAT12 volume.
        /// </summary>
        /// <param name="fat">The file system that owns the root directory.</param>
        /// <returns>The parsed small directory entries.</returns>
        public static IEnumerable<SmallDirectoryEntry> EnumerateRootDirectory(FatFileSystem fat)
        {
            var stream = fat.GetStream();
            var buffer = new byte[fat.BiosParameterBlock.RootDirectoryEntries * 16];

            stream.Position = (fat.ReservedSectors + fat.ClusterMapsSectors) * fat.BiosParameterBlock.BytesPerLogicalSector;
            stream.Read(buffer);

            return EnumerateDirectory(buffer);
        }

        private static IEnumerable<SmallDirectoryEntry> EnumerateDirectory(ReadOnlyMemory<byte> buffer)
        {
            for (var i = 0; i < buffer.Length; i += 16)
            {
                var entry = buffer.Slice(i, 16).Span;

                if (entry[0] is 0x00 or 0xF6)
                {
                    // No more entries
                    break;
                }

                if (entry[0] is 0xE5)
                {
                    // Deleted entry — skip
                    continue;
                }

                yield return new SmallDirectoryEntry(entry);
            }
        }
    }
}
