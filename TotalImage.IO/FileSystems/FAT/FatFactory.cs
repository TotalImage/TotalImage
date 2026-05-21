using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TotalImage.DiskGeometries;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// A factory class that can create a FAT file system
    /// </summary>
    public class FatFactory : IFileSystemFactory
    {
        private static FileSystem? GetFatFromBiosParameterBlock(Stream stream, BiosParameterBlock bpb)
        {
            uint rootDirSectors = (((uint)bpb.RootDirectoryEntries * 32) + ((uint)bpb.BytesPerLogicalSector - 1)) / (uint)bpb.BytesPerLogicalSector;
            uint fatSectors = bpb.LogicalSectorsPerFAT;
            uint totalSectors = bpb.TotalLogicalSectors;
            uint dataSectors = totalSectors - (bpb.ReservedLogicalSectors + (bpb.NumberOfFATs * fatSectors) + rootDirSectors);
            uint clusterCount = dataSectors / bpb.LogicalSectorsPerCluster;

            stream.Position = 0;
            if (clusterCount < 4085)
            {
                // return FAT12
                return new Fat12FileSystem(stream, bpb);
            }
            else if (clusterCount < 65525)
            {
                // return FAT16
                return new Fat16FileSystem(stream, bpb);
            }
            else
            {
                // return FAT32
                return new Fat32FileSystem(stream, bpb);
            }
        }

        /// <inheritdoc />
        public FileSystem? TryLoadFileSystem(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.ASCII, true);

            BiosParameterBlock? _bpb;
            byte bpbOffset = 0x0B;

            // Check for early 86-DOS (pre-0.42) before attempting BPB parsing.
            // These disks have no BPB and do not follow the DOS media descriptor convention
            // in the FAT header, so they cannot be detected via CheckForNoBpb.
            var early86Dos = CheckForEarly86DosGeometry(reader);
            if (early86Dos is not null)
            {
                return new EarlyFat12FileSystem(stream, early86Dos.Value.Item2);
            }

            try
            {
                _bpb = BiosParameterBlock.Parse(reader, bpbOffset); //Try to parse the BPB at the standard offset
            }
            catch (InvalidDataException)
            {
                //BPB likely invalid, check if this is an Acorn 800k disk without one
                if (CheckForAcorn800k(reader))
                {
                    return new Fat12FileSystem(stream, BiosParameterBlock.DefaultAcornParameters);
                }
                //Try Victor 9000 instead, which also has no BPB
                else if (CheckForVictor9k(reader) == 1 && stream.Length == 626688)
                {
                    return new Fat12FileSystem(stream, BiosParameterBlock.DefaultVictorSSParameters); //Single-sided
                }
                else if (CheckForVictor9k(reader) == 2 && stream.Length == 1224192)
                {
                    return new Fat12FileSystem(stream, BiosParameterBlock.DefaultVictorDSParameters); //Double-sided
                }
                else
                {
                    //Try parsing it at 0x04 in case it's a Zenith Z-100 disk
                    try
                    {
                        bpbOffset = 0x04;
                        _bpb = BiosParameterBlock.Parse(reader, bpbOffset);
                    }
                    catch (InvalidDataException)
                    {
                        //Try parsing it at 0x50 in case it's an Apricot disk
                        try
                        {
                            bpbOffset = 0x50;
                            _bpb = BiosParameterBlock.Parse(reader, bpbOffset);
                        }
                        catch (InvalidDataException)
                        {
                            //BPB still invalid, it may not even be there, try to figure out if it's a DOS 1.x disk by looking at file length
                            //(we can do this for raw sector images) and the media descriptor byte
                            if (CheckForNoBpb(reader) is FloppyGeometry geometry)
                            {
                                _bpb = BiosParameterBlock.FromGeometry(geometry, BiosParameterBlockVersion.Dos20, "");
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }

            return GetFatFromBiosParameterBlock(stream, _bpb);
        }

        /// <summary>
        /// Known 86-DOS (pre-0.42) disk geometries. These disks have no BPB and write FF FF FF
        /// at the start of both FATs regardless of media type, rather than MediaDescriptor FF FF.
        /// </summary>
        private static readonly (FloppyGeometry.FriendlyName Name, FloppyGeometry Geometry)[] Early86DosGeometries =
        [
            (FloppyGeometry.FriendlyName.SingleDensity87k,     FloppyGeometry.KnownGeometries[FloppyGeometry.FriendlyName.SingleDensity87k]),
            (FloppyGeometry.FriendlyName.SingleDensity90k,     FloppyGeometry.KnownGeometries[FloppyGeometry.FriendlyName.SingleDensity90k]),
            (FloppyGeometry.FriendlyName.SingleDensity250kSCP, FloppyGeometry.KnownGeometries[FloppyGeometry.FriendlyName.SingleDensity250kSCP]),
            (FloppyGeometry.FriendlyName.HighDensity1232kSCP,  FloppyGeometry.KnownGeometries[FloppyGeometry.FriendlyName.HighDensity1232kSCP]),
        ];

        /// <summary>
        /// Checks whether the image is an early 86-DOS (pre-0.42) disk with 16-byte directory entries.
        /// Matches by image size against known 86-DOS geometries, then verifies FAT header is FF FF FF
        /// and that the root directory decodes correctly as 16-byte entries.
        /// </summary>
        /// <returns>The matching geometry and synthesised BPB, or null if not detected.</returns>
        private static (FloppyGeometry, BiosParameterBlock)? CheckForEarly86DosGeometry(BinaryReader reader)
        {
            long imageSize = reader.BaseStream.Length;

            foreach (var (_, geometry) in Early86DosGeometries)
            {
                uint bytesPerSector = (uint)(128 << geometry.BPS);
                long expectedSize = (long)geometry.SPT * geometry.Tracks * geometry.Sides * bytesPerSector;

                if (imageSize != expectedSize)
                    continue;

                long fat1Start = geometry.ReservedSectors * bytesPerSector;
                long fat2Start = fat1Start + geometry.SPF * bytesPerSector;
                long rootStart = fat2Start + geometry.SPF * bytesPerSector;

                // Early 86-DOS always writes FF FF FF at the start of both FATs
                reader.BaseStream.Seek(fat1Start, SeekOrigin.Begin);
                if (reader.ReadByte() != 0xFF || reader.ReadByte() != 0xFF || reader.ReadByte() != 0xFF)
                    continue;

                reader.BaseStream.Seek(fat2Start, SeekOrigin.Begin);
                if (reader.ReadByte() != 0xFF || reader.ReadByte() != 0xFF || reader.ReadByte() != 0xFF)
                    continue;

                // Verify at least one root directory entry decodes plausibly as a 16-byte entry
                int readSize = (int)Math.Min(geometry.RootDirectoryEntries * 16L, 512);
                reader.BaseStream.Seek(rootStart, SeekOrigin.Begin);
                byte[] buf = reader.ReadBytes(readSize);

                bool hasValidEntry = false;
                for (int i = 0; i + 16 <= buf.Length; i += 16)
                {
                    byte first = buf[i];
                    if (first is 0x00 or 0xF6)
                        break;
                    if (first is 0xE5)
                        continue;

                    ushort cluster = (ushort)(buf[i + 11] | (buf[i + 12] << 8));
                    uint size = (uint)(buf[i + 13] | (buf[i + 14] << 8) | (buf[i + 15] << 16));

                    if (cluster > 0 && size > 0 && size <= imageSize)
                    {
                        hasValidEntry = true;
                        break;
                    }
                }

                if (!hasValidEntry)
                    continue;

                var bpb = BiosParameterBlock.FromGeometry(geometry, BiosParameterBlockVersion.Dos20, "");
                return (geometry, bpb);
            }

            return null;
        }

        /// <summary>
        /// Checks whether the disk uses the early 86-DOS (pre-0.42) 16-byte directory entry format.
        /// This is detected by reading the root directory and verifying that entries are consistent with
        /// the 16-byte layout (11-byte filename, 2-byte cluster, 3-byte size) rather than the standard 32-byte layout.
        /// </summary>
        /// <param name="reader">BinaryReader used to read the disk image.</param>
        /// <param name="geometry">The floppy geometry for this disk.</param>
        /// <param name="bpb">The synthesised BPB for this disk.</param>
        /// <returns>True if the root directory appears to use 16-byte entries.</returns>
        private static bool CheckForEarly86Dos(BinaryReader reader, FloppyGeometry geometry, BiosParameterBlock bpb)
        {
            uint bytesPerSector = (ushort)(128 << geometry.BPS);
            long rootDirStart = (geometry.ReservedSectors + geometry.NoOfFATs * geometry.SPF) * bytesPerSector;

            // Read enough bytes for a few 32-byte candidate entries to compare interpretations
            int readSize = Math.Min(bpb.RootDirectoryEntries * 32, 512);
            reader.BaseStream.Seek(rootDirStart, SeekOrigin.Begin);
            byte[] buf = reader.ReadBytes(readSize);

            int validAs16 = 0;

            for (int i = 0; i + 16 <= buf.Length; i += 16)
            {
                byte first = buf[i];

                // Skip unused/deleted entries
                if (first is 0x00 or 0xF6 or 0xE5)
                    break;

                // In the 16-byte layout, bytes 11–12 are the first cluster (should be small and non-zero)
                // and bytes 13–15 are the 24-bit file size (non-zero for any real file).
                // In a standard DOS 1.x 32-byte entry, bytes 13–15 are creation time fields which are
                // always zeroed on early DOS, so size16 == 0 for those — this is the key discriminator.
                ushort cluster16 = (ushort)(buf[i + 11] | (buf[i + 12] << 8));
                uint size16 = (uint)(buf[i + 13] | (buf[i + 14] << 8) | (buf[i + 15] << 16));

                if (cluster16 > 0 && size16 > 0 && size16 <= reader.BaseStream.Length)
                    validAs16++;
            }

            return validAs16 > 0;
        }

        /// <summary>
        /// Checks whether the image contains Acorn 800k format, which starts with the first (and only) FAT.
        /// </summary>
        /// <returns>True if the image is in Acorn 800k format, otherwise false.</returns>
        private static bool CheckForAcorn800k(BinaryReader reader)
        {
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            return (reader.ReadUInt32() & 0xFFFFFF) == 0xFFFFFD;
        }

        /// <summary>
        /// Checks whether the image contains Victor 9000 single-sided format, which does not have a BPB and needs special treatment.
        /// </summary>
        /// <returns>0 if the disk is not a Victor disk, 1 if it's a single-sided Victor disk, 2 if it's a double-sided Victor disk</returns>
        private static byte CheckForVictor9k(BinaryReader reader)
        {
            //First look for the first FAT with the 0xF8 media descriptor byte
            reader.BaseStream.Seek(0x200, SeekOrigin.Begin);
            if ((reader.ReadUInt32() & 0xFFFFFF) == 0xFFFFF8)
            {
                //First FAT matches, now look for the second FAT to determine if this is single-sided or double-sided
                reader.BaseStream.Seek(0x400, SeekOrigin.Begin);
                uint bytes2 = reader.ReadUInt32() & 0xFFFFFF;
                if (bytes2 == 0xFFFFF8)
                    return 1; //This could be a single-sided Victor 9000 disk
                else
                {
                    reader.BaseStream.Seek(0x600, SeekOrigin.Begin);
                    bytes2 = reader.ReadUInt32() & 0xFFFFFF;
                    if (bytes2 == 0xFFFFF8)
                        return 2; //This could be a double-sided Victor 9000 disk
                }
            }
            return 0; //FATs not found or do not match, so probably not a Victor 9000 disk
        }

        /// <summary>
        /// Checks whether the disk is DOS 1.x-formatted (that is, without a BPB) by doing several rounds of tests to determine the best matching geometry.
        /// </summary>
        /// <param name="reader">BinaryReader used to read the disk image.</param>
        /// <returns>The best matching geometry if found, otherwise null.</returns>
        private static FloppyGeometry? CheckForNoBpb(BinaryReader reader)
        {
            //Round 1: filter by total image size and the 3-byte FAT header that includes the media descriptor byte at the start of both FATs
            var candidates = new List<(FloppyGeometry.FriendlyName Name, FloppyGeometry Geometry)>();

            foreach (var (name, geometry) in FloppyGeometry.KnownGeometries)
            {
                //Formats with variable SPT (e.g. Victor 9000) have SPT set to 0 and are handled separately
                if (geometry.SPT == 0)
                    continue;

                uint bytesPerSector = (ushort)(128 << geometry.BPS);
                long expectedSize = (long)geometry.SPT * geometry.Tracks * geometry.Sides * bytesPerSector;

                if (reader.BaseStream.Length != expectedSize)
                    continue;

                long fat1Start = geometry.ReservedSectors * bytesPerSector;
                long fat2Start = fat1Start + geometry.SPF * bytesPerSector;

                //Validate the first FAT: first 3 bytes must be MediaDescriptor, 0xFF, 0xFF
                if (fat1Start + 3 > reader.BaseStream.Length || fat2Start + 3 > reader.BaseStream.Length)
                    continue;

                reader.BaseStream.Seek(fat1Start, SeekOrigin.Begin);
                byte fat1b0 = reader.ReadByte();
                byte fat1b1 = reader.ReadByte();
                byte fat1b2 = reader.ReadByte();

                if (fat1b0 != geometry.MediaDescriptor || fat1b1 != 0xFF || fat1b2 != 0xFF)
                    continue;

                //Validate the second FAT: first 3 bytes must match the first FAT
                reader.BaseStream.Seek(fat2Start, SeekOrigin.Begin);
                byte fat2b0 = reader.ReadByte();
                byte fat2b1 = reader.ReadByte();
                byte fat2b2 = reader.ReadByte();

                if (fat2b0 != fat1b0 || fat2b1 != fat1b1 || fat2b2 != fat1b2)
                    continue;

                candidates.Add((name, geometry));
            }

            if (candidates.Count == 0)
                return null;
            else if (candidates.Count == 1)
                return candidates[0].Geometry;

            //Round 2: compare the entire FAT contents, correct geometry should have identical FATs
            var round2 = new List<(FloppyGeometry.FriendlyName Name, FloppyGeometry Geometry)>();

            foreach (var (name, geometry) in candidates)
            {
                uint bytesPerSector = (ushort)(128 << geometry.BPS);
                long fat1Start = geometry.ReservedSectors * bytesPerSector;
                long fat2Start = fat1Start + geometry.SPF * bytesPerSector;
                long fatSize = geometry.SPF * bytesPerSector;

                reader.BaseStream.Seek(fat1Start, SeekOrigin.Begin);
                byte[] fat1 = reader.ReadBytes((int)fatSize);

                reader.BaseStream.Seek(fat2Start, SeekOrigin.Begin);
                byte[] fat2 = reader.ReadBytes((int)fatSize);

                if (fat1.AsSpan().SequenceEqual(fat2))
                    round2.Add((name, geometry));
            }

            if (round2.Count == 1)
                return round2[0].Geometry;

            /* Round 3: for geometries with identical FATs but different root dir entries count, check the root directory area.
             * Read the sector(s) between the smaller and larger root directory and check whether
             * they look like valid directory entries (all zero or 0xE5-deleted first byte, valid attributes). */
            var workingCandidates = round2.Count > 0 ? round2 : candidates;

            if (workingCandidates.Count > 1)
            {
                //Sort by RootDirectoryEntries ascending so we test smaller first
                workingCandidates.Sort((a, b) => a.Geometry.RootDirectoryEntries.CompareTo(b.Geometry.RootDirectoryEntries));
                var smallest = workingCandidates[0].Geometry;

                foreach (var (name, geometry) in workingCandidates)
                {
                    if (geometry.RootDirectoryEntries == smallest.RootDirectoryEntries)
                        continue;

                    uint bytesPerSector = (ushort)(128 << geometry.BPS);
                    long rootStart = (geometry.ReservedSectors + geometry.NoOfFATs * geometry.SPF) * bytesPerSector;

                    //Sectors that exist in this geometry's root dir but not in the smaller one
                    long extraStart = rootStart + smallest.RootDirectoryEntries * 32L;
                    long extraEnd = rootStart + geometry.RootDirectoryEntries * 32L;

                    if (extraEnd > reader.BaseStream.Length)
                        continue;

                    reader.BaseStream.Seek(extraStart, SeekOrigin.Begin);
                    byte[] extraRegion = reader.ReadBytes((int)(extraEnd - extraStart));

                    if (IsValidRootDirectoryRegion(extraRegion))
                        return geometry;
                }

                //Round 4: last resort, fall back to the order in KnownGeometries, first matching geometry wins
                foreach (var (name, geometry) in FloppyGeometry.KnownGeometries)
                {
                    foreach (var (cname, cgeometry) in workingCandidates)
                    {
                        if (name == cname)
                            return cgeometry;
                    }
                }
            }

            return workingCandidates.Count > 0 ? workingCandidates[0].Geometry : null;
        }

        /// <summary>
        /// Checks if the given byte region looks like valid (empty or deleted) FAT directory entries.
        /// </summary>
        /// <remarks>Each entry is 32 bytes; the first byte is 0x00 (never used) or 0xE5 (deleted), and byte 11 (attributes) must be valid.</remarks>
        /// <returns>True if the region looks like valid directory entries, otherwise false.</returns>
        private static bool IsValidRootDirectoryRegion(byte[] region)
        {
            if (region.Length % 32 != 0)
                return false;

            for (int i = 0; i < region.Length; i += 32)
            {
                byte firstByte = region[i];
                if (firstByte != 0x00 && firstByte != 0xE5)
                    return false;

                //Attribute byte must only use known bits (0x3F mask); upper 2 bits unused
                byte attributes = region[i + 11];
                if ((attributes & 0xC0) != 0)
                    return false;
            }

            return true;
        }
    }
}
