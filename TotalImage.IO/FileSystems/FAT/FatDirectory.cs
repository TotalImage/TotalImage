using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TotalImage.Changes;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// A directory containing other directories and files in a FAT file system.
    /// </summary>
    public class FatDirectory : Directory, IFatFileSystemObject
    {
        internal DirectoryEntry? entry = null;
        private LongDirectoryEntry[]? lfnEntries = null;

        /// <inheritdoc />
        public string ShortName
        {
            get => entry?.FileName ?? string.Empty;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string? LongName
        {
            get => lfnEntries?.Length > 0 ? LongDirectoryEntry.CombineEntries(lfnEntries) : null;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override string Name
        {
            get => LongName ?? ShortName;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override FileAttributes Attributes
        {
            get => (FileAttributes?)entry?.Attributes ?? FileAttributes.Directory;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? LastAccessTime
        {
            get => entry?.LastAccessTime ?? null;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? LastWriteTime
        {
            get => entry?.LastWriteTime ?? null;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? CreationTime
        {
            get => entry?.CreationTime ?? null;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ulong Length
        {
            get
            {
                var fat = (FatFileSystem)FileSystem;

                if (entry is not null)
                {
                    var clusters = fat.MainFat.GetClusterChain(FirstCluster).Length;
                    return (uint)clusters * fat.BytesPerCluster;
                }
                else
                {
                    return fat.RootDirectorySectors * fat.BiosParameterBlock.BytesPerLogicalSector;
                }
            }
            set => throw new NotSupportedException();
        }

        /// <inheritdoc />
        public uint FirstCluster
        {
            get => entry?.FirstClusterOfFile ??
                (((FatFileSystem)FileSystem).BiosParameterBlock as Fat32BiosParameterBlock)?.RootDirectoryCluster ??
                throw new InvalidOperationException();
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the root directory for a FAT file system.
        /// </summary>
        /// <param name="fat">The file system that owns the directory.</param>
        public FatDirectory(FatFileSystem fat) : base(fat, null) { }

        /// <summary>
        /// Creates a FAT directory from a directory entry.
        /// </summary>
        /// <param name="fat">The file system that owns the directory.</param>
        /// <param name="entry">The directory entry describing the directory.</param>
        /// <param name="lfnEntries">The long file name entries associated with <paramref name="entry"/>.</param>
        /// <param name="parent">The parent directory.</param>
        public FatDirectory(FatFileSystem fat, DirectoryEntry entry, LongDirectoryEntry[]? lfnEntries, Directory parent) : base(fat, parent)
        {
            this.entry = entry;
            this.lfnEntries = lfnEntries;
        }

        /// <inheritdoc />
        public override Directory CreateSubdirectory(string path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Delete()
        {
            throw new NotImplementedException();
            /* When deleting a directory, the first character of the name needs to be changed to 0xE5.
            * The directory's directory entry can then be reused, and its clusters are marked as free until they're
            * overwritten. The same must then be done for all files and subdirectories inside.
            * This code is untested until this class is hooked up to the UI... */

            //entry.name[0] = 0xE5;

            //And then mark all clusters in the chain as free, and do the same for all files and subdirectories inside.
        }

        /// <inheritdoc />
        public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden, bool showDeleted = false)
        {
            var fat = (FatFileSystem)FileSystem;
            var entries = entry switch
            {
                null => DirectoryEntry.EnumerateRootDirectory(fat, showDeleted),
                _ => DirectoryEntry.EnumerateSubdirectory(fat, entry.Value, showDeleted)
            };

            foreach (var (entry, lfnEntries) in entries)
            {
                if (entry.Attributes.HasFlag(FatAttributes.VolumeId))
                {
                    // Skip volume label entries
                    continue;
                }
                else if (entry.Attributes.HasFlag(FatAttributes.Hidden) && !showHidden)
                {
                    // Skip hidden files unless showHidden is true
                    continue;
                }
                else if (entry.Attributes.HasFlag(FatAttributes.Subdirectory))
                {
                    // Folder entry
                    yield return new FatDirectory(fat, entry, lfnEntries, this);
                }
                else
                {
                    // File entry
                    yield return new FatFile(fat, entry, lfnEntries, this);
                }
            }
        }

        /// <inheritdoc />
        public override void MoveTo(string path)
        {
            throw new NotImplementedException();
        }

        // -----------------------------------------------------------------------
        // Public mutation entry points — validate and enqueue changes
        // -----------------------------------------------------------------------

        /// <summary>
        /// Builds the path component array for a <see cref="FileSystemObject"/> by walking
        /// up its parent chain. The returned array includes all ancestor directory names
        /// followed by the object's own name (if <paramref name="includeSelf"/> is true).
        /// </summary>
        internal static string[] GetPathComponents(FileSystems.FileSystemObject obj, bool includeSelf = true)
        {
            var parts = new Stack<string>();
            if (includeSelf)
                parts.Push(obj.Name);

            FileSystems.Directory? dir = obj is FileSystems.Directory d ? d.Parent : ((FileSystems.File)obj).Directory;
            while (dir is not null && dir.Parent is not null) // stop at root (Parent == null)
            {
                parts.Push(dir.Name);
                dir = dir.Parent;
            }
            return parts.ToArray();
        }

        /// <summary>
        /// Stages a new file addition into this directory.
        /// </summary>
        /// <param name="name">The 8.3 filename for the new entry.</param>
        /// <param name="source">The data source for the file content.</param>
        /// <param name="attributes">FAT attributes to assign.</param>
        /// <param name="creationTime">File creation timestamp.</param>
        /// <param name="lastWriteTime">File last write timestamp.</param>
        /// <param name="lastAccessTime">File last access timestamp.</param>
        public void EnqueueAddFile(string name, FileDataSource source, FatAttributes attributes,
            DateTime creationTime, DateTime lastWriteTime, DateTime lastAccessTime)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must not be empty.", nameof(name));

            var container = ((FatFileSystem)FileSystem).OwningContainer
                ?? throw new InvalidOperationException("This file system is not associated with a container.");

            var destPath = GetPathComponents(this, includeSelf: false)
                .Concat(new[] { name })
                .ToArray();

            container.PendingChanges.Add(new AddFileChange(destPath, source, attributes, creationTime, lastWriteTime, lastAccessTime));
        }

        /// <summary>
        /// Stages a new subdirectory creation inside this directory.
        /// </summary>
        /// <param name="name">The name of the new subdirectory.</param>
        public void EnqueueCreateSubdirectory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must not be empty.", nameof(name));

            var container = ((FatFileSystem)FileSystem).OwningContainer
                ?? throw new InvalidOperationException("This file system is not associated with a container.");

            var path = GetPathComponents(this, includeSelf: false)
                .Concat(new[] { name })
                .ToArray();

            container.PendingChanges.Add(new CreateDirectoryChange(path));
        }

        /// <summary>
        /// Stages the deletion of this directory (and all its contents).
        /// </summary>
        public void EnqueueDelete()
        {
            if (entry is null)
                throw new InvalidOperationException("Cannot delete the root directory.");

            var container = ((FatFileSystem)FileSystem).OwningContainer
                ?? throw new InvalidOperationException("This file system is not associated with a container.");

            container.PendingChanges.Add(new DeleteEntryChange(GetPathComponents(this)));
        }

        /// <summary>
        /// Stages a rename of this directory.
        /// </summary>
        /// <param name="newName">The new 8.3 directory name.</param>
        public void EnqueueRename(string newName)
        {
            if (entry is null)
                throw new InvalidOperationException("Cannot rename the root directory.");
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("New name must not be empty.", nameof(newName));

            var container = ((FatFileSystem)FileSystem).OwningContainer
                ?? throw new InvalidOperationException("This file system is not associated with a container.");

            container.PendingChanges.Add(new RenameChange(GetPathComponents(this), newName));
        }

        // -----------------------------------------------------------------------
        // Internal write helpers (used by ChangeApplicator during commit)
        // -----------------------------------------------------------------------

        /// <summary>
        /// Returns true when <paramref name="name"/> already fits within 8.3 constraints and
        /// contains only characters legal in FAT short names (no lowercase, no spaces, etc.).
        /// </summary>
        private static bool NeedsLfn(string name)
        {
            var dot = name.LastIndexOf('.');
            string basePart = dot < 0 ? name : name[..dot];
            string extPart  = dot < 0 ? string.Empty : name[(dot + 1)..];

            // More than one dot → needs LFN
            if (name.Count(c => c == '.') > 1) return true;

            // Exceeds 8.3
            if (basePart.Length > 8 || extPart.Length > 3) return true;

            // Contains lowercase or characters illegal in short names
            foreach (char c in name)
            {
                if (char.IsLower(c)) return true;
                if (" +,;=[]".IndexOf(c) >= 0) return true;
            }

            return false;
        }

        /// <summary>
        /// Collects all existing 11-byte short names in this directory (space-padded, uppercase).
        /// Used for collision detection during short-name generation.
        /// </summary>
        private HashSet<string> CollectExistingShortNames(FatFileSystem fat)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var stream = fat.GetStream();

            void ScanRange(long start, long end)
            {
                for (long pos = start; pos < end; pos += 32)
                {
                    stream.Position = pos;
                    var buf = new byte[32];
                    if (stream.Read(buf, 0, 32) < 32) break;
                    byte first = buf[0];
                    if (first == 0x00) break;          // end of directory
                    if (first == 0xE5) continue;       // deleted
                    if ((buf[11] & 0x0F) == 0x0F) continue; // LFN slot
                    result.Add(Encoding.ASCII.GetString(buf, 0, 11));
                }
            }

            if (entry is null && fat.BiosParameterBlock is not Fat32BiosParameterBlock)
            {
                long rootStart = (fat.ReservedSectors + fat.ClusterMapsSectors) * fat.BiosParameterBlock.BytesPerLogicalSector;
                long rootEnd   = rootStart + (long)fat.BiosParameterBlock.RootDirectoryEntries * 32;
                ScanRange(rootStart, rootEnd);
            }
            else
            {
                uint firstCluster = entry.HasValue
                    ? entry.Value.FirstClusterOfFile
                    : ((Fat32BiosParameterBlock)fat.BiosParameterBlock).RootDirectoryCluster;
                uint dataAreaByteOffset = fat.DataAreaFirstSector * fat.BiosParameterBlock.BytesPerLogicalSector;

                foreach (var cluster in fat.MainFat.GetClusterChain(firstCluster))
                {
                    long clusterStart = dataAreaByteOffset + (long)(cluster - 2) * fat.BytesPerCluster;
                    ScanRange(clusterStart, clusterStart + fat.BytesPerCluster);
                }
            }

            return result;
        }

        // Characters illegal in FAT 8.3 short names (beyond control chars).
        private static readonly char[] ShortNameIllegal = { ' ', '+', ',', ';', '=', '[', ']', '.', '"', '/', '\\', ':', '*', '?', '<', '>', '|' };

        /// <summary>
        /// Generates a unique, collision-free 11-byte FAT short name for <paramref name="longName"/>.
        /// If the name fits in 8.3 and contains no illegal/lowercase chars, it is used directly.
        /// Otherwise a tilde-numbered basis is produced and the first free slot is returned.
        /// </summary>
        private byte[] GenerateShortName(string longName, HashSet<string> existingShortNames)
        {
            // --- Split on last dot ---
            int dotIdx  = longName.LastIndexOf('.');
            string baseLong = dotIdx < 0 ? longName : longName[..dotIdx];
            string extLong  = dotIdx < 0 ? string.Empty : longName[(dotIdx + 1)..];

            // --- Strip illegal / strip leading dots / uppercase ---
            string baseClean = StripIllegal(baseLong).TrimStart('.').ToUpperInvariant();
            string extClean  = StripIllegal(extLong).ToUpperInvariant();

            if (baseClean.Length == 0) baseClean = "_";

            // Truncate extension to 3
            if (extClean.Length > 3) extClean = extClean[..3];

            // If the name fits neatly in 8.3 and needs no tilde, use it directly.
            if (baseClean.Length <= 8 && !NeedsLfn(longName))
            {
                return BuildShortNameBytes(baseClean, extClean);
            }

            // Truncate base to 6 chars for the tilde section
            string baseTrunc = baseClean.Length > 6 ? baseClean[..6] : baseClean;

            for (int n = 1; n <= 999999; n++)
            {
                string tilde = $"~{n}";
                // baseTrunc + tilde must fit in 8 chars
                string candidate = baseTrunc.Length + tilde.Length > 8
                    ? baseClean[..(8 - tilde.Length)] + tilde
                    : baseTrunc + tilde;

                var candidateBytes = BuildShortNameBytes(candidate, extClean);
                string key = Encoding.ASCII.GetString(candidateBytes);
                if (!existingShortNames.Contains(key))
                    return candidateBytes;
            }

            throw new IOException($"Could not generate a unique short name for '{longName}'.");
        }

        private static string StripIllegal(string s)
        {
            var sb = new StringBuilder(s.Length);
            foreach (char c in s)
            {
                if (c < 0x20) continue;
                if (Array.IndexOf(ShortNameIllegal, c) >= 0) continue;
                sb.Append(c);
            }
            return sb.ToString();
        }

        private static byte[] BuildShortNameBytes(string basePart, string extPart)
        {
            var result = new byte[11];
            for (int i = 0; i < 11; i++) result[i] = 0x20;
            var baseBytes = Encoding.ASCII.GetBytes(basePart);
            var extBytes  = Encoding.ASCII.GetBytes(extPart);
            Array.Copy(baseBytes, result, Math.Min(8, baseBytes.Length));
            if (extBytes.Length > 0)
                Array.Copy(extBytes, 0, result, 8, Math.Min(3, extBytes.Length));
            return result;
        }

        /// <summary>
        /// Finds the stream position of the first run of <paramref name="count"/> consecutive free
        /// (0x00 or 0xE5) 32-byte directory entry slots in this directory.
        /// Allocates a new cluster if needed (cluster-chain directories only).
        /// Returns the offset of the first slot in the run.
        /// </summary>
        private long FindOrAllocateFreeDirEntryOffset(FatFileSystem fat, int count = 1)
        {
            var stream = fat.GetStream();

            // --- FAT12/16 fixed root directory ---
            if (entry is null && fat.BiosParameterBlock is not Fat32BiosParameterBlock)
            {
                long rootStart = (fat.ReservedSectors + fat.ClusterMapsSectors) * fat.BiosParameterBlock.BytesPerLogicalSector;
                long rootEnd   = rootStart + (long)fat.BiosParameterBlock.RootDirectoryEntries * 32;

                int run = 0;
                long runStart = -1;

                for (long pos = rootStart; pos < rootEnd; pos += 32)
                {
                    stream.Position = pos;
                    int firstByte = stream.ReadByte();
                    bool isFree = firstByte == 0x00 || firstByte == 0xE5;
                    if (isFree)
                    {
                        if (run == 0) runStart = pos;
                        run++;
                        if (run >= count) return runStart;
                        // 0x00 means every slot from here to rootEnd is implicitly free —
                        // we can extend the run without reading further bytes.
                        if (firstByte == 0x00)
                        {
                            // Check that the remaining entries can satisfy the count.
                            long slotsRemaining = (rootEnd - pos) / 32;
                            if (run + slotsRemaining - 1 >= count)
                                return runStart;
                            break; // truly full
                        }
                    }
                    else
                    {
                        run = 0; runStart = -1;
                    }
                }

                throw new IOException(
                    $"The root directory is full and cannot be expanded (FAT12/FAT16 root directories have a " +
                    $"fixed maximum of {fat.BiosParameterBlock.RootDirectoryEntries} entries). " +
                    $"Delete some existing entries first.");
            }

            // --- Cluster-chain directory (subdirs or FAT32 root) ---
            uint firstCluster = entry.HasValue
                ? entry.Value.FirstClusterOfFile
                : ((Fat32BiosParameterBlock)fat.BiosParameterBlock).RootDirectoryCluster;

            uint dataAreaByteOffset = fat.DataAreaFirstSector * fat.BiosParameterBlock.BytesPerLogicalSector;

            // We may need to grow the chain, so iterate with index to get the last cluster.
            var clusters = fat.MainFat.GetClusterChain(firstCluster);

            int runC = 0;
            long runStartC = -1;

            foreach (var cluster in clusters)
            {
                long clusterStart = dataAreaByteOffset + (long)(cluster - 2) * fat.BytesPerCluster;
                for (long pos = clusterStart; pos < clusterStart + fat.BytesPerCluster; pos += 32)
                {
                    stream.Position = pos;
                    int firstByte = stream.ReadByte();
                    bool isFree = firstByte == 0x00 || firstByte == 0xE5;
                    if (isFree)
                    {
                        if (runC == 0) runStartC = pos;
                        runC++;
                        if (runC >= count) return runStartC;
                    }
                    else
                    {
                        runC = 0; runStartC = -1;
                    }
                    if (firstByte == 0x00) goto doneScanning;
                }
            }
            doneScanning:

            // Not enough contiguous free slots — allocate a new cluster.
            // (The partial run at the end of the existing chain will be extended by the new cluster.)
            uint lastCluster = clusters[^1];
            uint newCluster  = fat.AllocateCluster(lastCluster);
            long newStart    = dataAreaByteOffset + (long)(newCluster - 2) * fat.BytesPerCluster;

            stream.Position = newStart;
            stream.Write(new byte[fat.BytesPerCluster]);

            // If we had a partial run ending exactly at the boundary, the new cluster extends it.
            if (runC > 0 && count - runC <= (int)(fat.BytesPerCluster / 32))
                return runStartC;

            return newStart;
        }

        /// <summary>
        /// Builds a padded 13-character chunk array for LFN entries from a long name.
        /// Each chunk is exactly 13 UTF-16 chars; after the NUL terminator positions are filled with U+FFFF.
        /// </summary>
        private static char[][] BuildLfnChunks(string longName)
        {
            // LFN always has a NUL terminator, then pads with 0xFFFF
            int totalChars = longName.Length + 1; // +1 for NUL
            int chunkCount = (totalChars + 12) / 13;
            var chunks = new char[chunkCount][];

            for (int i = 0; i < chunkCount; i++)
            {
                var chunk = new char[13];
                for (int j = 0; j < 13; j++)
                {
                    int nameIdx = i * 13 + j;
                    if (nameIdx < longName.Length)
                        chunk[j] = longName[nameIdx];
                    else if (nameIdx == longName.Length)
                        chunk[j] = '\0'; // NUL terminator
                    else
                        chunk[j] = '\uFFFF'; // padding
                }
                chunks[i] = chunk;
            }

            return chunks;
        }

        /// <summary>
        /// Writes LFN directory entries preceding the short-name entry, then writes the short-name entry.
        /// All entries are written contiguously starting at <paramref name="slotOffset"/>.
        /// </summary>
        private static void WriteLfnAndShortEntry(Stream stream, long slotOffset,
            string longName, byte[] shortNameBytes,
            FatAttributes attributes, uint firstCluster, uint fileSize,
            DateTime? creationTime, DateTime? lastWriteTime, DateTime? lastAccessTime)
        {
            byte checksum = LongDirectoryEntry.GetShortNameChecksum(shortNameBytes);
            var chunks    = BuildLfnChunks(longName);
            int n         = chunks.Length;

            stream.Position = slotOffset;

            // Write LFN entries in reverse order (last chunk first, highest ordinal with 0x40 flag)
            for (int i = n - 1; i >= 0; i--)
            {
                byte ordinalByte = (byte)((i + 1) | (i == n - 1 ? 0x40 : 0x00));
                var lfn = new LongDirectoryEntry(ordinalByte, chunks[i], checksum);
                lfn.WriteTo(stream);
            }

            // Write the short-name entry last
            var shortEntry = new DirectoryEntry(
                shortNameBytes, attributes, firstCluster, fileSize,
                creationTime, lastWriteTime, lastAccessTime);
            shortEntry.WriteTo(stream);
        }

        /// <summary>
        /// Writes a new file into this directory during commit.
        /// Allocates clusters, copies file data, and writes LFN + short-name directory entries.
        /// </summary>
        internal void WriteAddFile(string name, Stream sourceStream, FatAttributes attributes,
            DateTime? creationTime, DateTime? lastWriteTime, DateTime? lastAccessTime)
        {
            var fat = (FatFileSystem)FileSystem;
            var stream = fat.GetStream();

            uint fileSize = (uint)sourceStream.Length;
            uint firstCluster = 0;

            if (fileSize > 0)
            {
                // Allocate cluster chain and copy data
                uint? prevCluster = null;
                long remaining = fileSize;

                while (remaining > 0)
                {
                    uint cluster = fat.AllocateCluster(prevCluster);
                    if (prevCluster is null)
                        firstCluster = cluster;
                    prevCluster = cluster;

                    long clusterOffset = fat.DataAreaFirstSector * fat.BiosParameterBlock.BytesPerLogicalSector
                                        + (long)(cluster - 2) * fat.BytesPerCluster;
                    stream.Position = clusterOffset;

                    int toWrite = (int)Math.Min(remaining, fat.BytesPerCluster);
                    var buf = new byte[toWrite];
                    sourceStream.Read(buf, 0, toWrite);
                    stream.Write(buf, 0, toWrite);
                    remaining -= toWrite;
                }
            }

            var existingShortNames = CollectExistingShortNames(fat);
            var shortNameBytes = GenerateShortName(name, existingShortNames);
            bool needsLfn = NeedsLfn(name);

            int lfnCount = needsLfn ? (name.Length + 13) / 13 : 0;
            int totalSlots = lfnCount + 1;

            long slotOffset = FindOrAllocateFreeDirEntryOffset(fat, totalSlots);

            if (needsLfn)
            {
                WriteLfnAndShortEntry(stream, slotOffset, name, shortNameBytes,
                    attributes | FatAttributes.Archive, firstCluster, fileSize,
                    creationTime, lastWriteTime, lastAccessTime);
            }
            else
            {
                var newEntry = new DirectoryEntry(
                    shortNameBytes, attributes | FatAttributes.Archive,
                    firstCluster, fileSize, creationTime, lastWriteTime, lastAccessTime);
                stream.Position = slotOffset;
                newEntry.WriteTo(stream);
            }
        }

        /// <summary>
        /// Creates a new subdirectory entry in this directory during commit.
        /// </summary>
        internal void WriteCreateSubdirectory(string name)
        {
            var fat = (FatFileSystem)FileSystem;
            var stream = fat.GetStream();
            var now = DateTime.Now;

            // Allocate one cluster for the new directory
            uint newCluster = fat.AllocateCluster();

            // Zero out the cluster
            long clusterOffset = fat.DataAreaFirstSector * fat.BiosParameterBlock.BytesPerLogicalSector
                                 + (long)(newCluster - 2) * fat.BytesPerCluster;
            stream.Position = clusterOffset;
            stream.Write(new byte[fat.BytesPerCluster]);

            // Write '.' entry
            stream.Position = clusterOffset;
            var dotEntry = new DirectoryEntry(
                Encoding.ASCII.GetBytes(".          "),
                FatAttributes.Subdirectory,
                newCluster, 0, now, now, now);
            dotEntry.WriteTo(stream);

            // Write '..' entry — points to this directory's first cluster (0 for root)
            uint parentCluster = entry.HasValue ? entry.Value.FirstClusterOfFile : 0;
            var dotDotEntry = new DirectoryEntry(
                Encoding.ASCII.GetBytes("..         "),
                FatAttributes.Subdirectory,
                parentCluster, 0, now, now, now);
            dotDotEntry.WriteTo(stream);

            // Write the directory entry (with optional LFN) in the parent
            var existingShortNames = CollectExistingShortNames(fat);
            var shortNameBytes = GenerateShortName(name, existingShortNames);
            bool needsLfn = NeedsLfn(name);

            int lfnCount = needsLfn ? (name.Length + 13) / 13 : 0;
            int totalSlots = lfnCount + 1;

            long slotOffset = FindOrAllocateFreeDirEntryOffset(fat, totalSlots);

            if (needsLfn)
            {
                WriteLfnAndShortEntry(stream, slotOffset, name, shortNameBytes,
                    FatAttributes.Subdirectory, newCluster, 0, now, now, now);
            }
            else
            {
                var newEntry = new DirectoryEntry(
                    shortNameBytes, FatAttributes.Subdirectory, newCluster, 0, now, now, now);
                stream.Position = slotOffset;
                newEntry.WriteTo(stream);
            }
        }

        /// <summary>
        /// Marks the directory entry for this directory as deleted (0xE5) during commit.
        /// </summary>
        internal void WriteDelete()
        {
            if (entry is null)
                throw new InvalidOperationException("Cannot delete the root directory.");

            var fat = (FatFileSystem)FileSystem;
            var stream = fat.GetStream();

            long entryOffset = FindEntryOffset(fat, entry.Value);
            stream.Position = entryOffset;
            stream.WriteByte(0xE5);

            fat.FreeClusterChain(entry.Value.FirstClusterOfFile);
        }

        /// <summary>
        /// Renames this directory entry during commit (8.3 only).
        /// </summary>
        internal void WriteRename(string newName)
        {
            if (entry is null)
                throw new InvalidOperationException("Cannot rename the root directory.");

            var fat = (FatFileSystem)FileSystem;
            var stream = fat.GetStream();

            long entryOffset = FindEntryOffset(fat, entry.Value);
            var existingShortNames = CollectExistingShortNames(fat);
            var shortNameBytes = GenerateShortName(newName, existingShortNames);

            stream.Position = entryOffset;
            stream.Write(shortNameBytes, 0, 11);
        }

        /// <summary>
        /// Searches the parent directory's region for the stream offset of a specific directory entry
        /// (matched by first cluster + file size).
        /// </summary>
        private long FindEntryOffset(FatFileSystem fat, DirectoryEntry target)
        {
            var stream = fat.GetStream();
            var parentDir = (FatDirectory?)Parent;

            long start, length;
            if (parentDir?.entry is null && fat.BiosParameterBlock is not Fat32BiosParameterBlock)
            {
                // FAT12/16 root dir
                start = (fat.ReservedSectors + fat.ClusterMapsSectors) * fat.BiosParameterBlock.BytesPerLogicalSector;
                length = (long)fat.BiosParameterBlock.RootDirectoryEntries * 32;
            }
            else
            {
                uint firstCluster = parentDir?.entry?.FirstClusterOfFile
                    ?? ((Fat32BiosParameterBlock)fat.BiosParameterBlock).RootDirectoryCluster;
                var clusters = fat.MainFat.GetClusterChain(firstCluster);
                long dataBase = fat.DataAreaFirstSector * fat.BiosParameterBlock.BytesPerLogicalSector;
                foreach (var cl in clusters)
                {
                    long clStart = dataBase + (long)(cl - 2) * fat.BytesPerCluster;
                    for (long pos = clStart; pos < clStart + fat.BytesPerCluster; pos += 32)
                    {
                        stream.Position = pos;
                        var buf = new byte[32];
                        stream.Read(buf, 0, 32);
                        if (buf[0] == 0x00) break;
                        if (buf[0] == 0xE5) continue;

                        var e = new DirectoryEntry(fat, buf);
                        if (e.FirstClusterOfFile == target.FirstClusterOfFile &&
                            e.FileSize == target.FileSize &&
                            e.FileName == target.FileName)
                            return pos;
                    }
                }
                throw new FileNotFoundException("Directory entry not found.");
            }

            for (long pos = start; pos < start + length; pos += 32)
            {
                stream.Position = pos;
                var buf = new byte[32];
                stream.Read(buf, 0, 32);
                if (buf[0] == 0x00) break;
                if (buf[0] == 0xE5) continue;

                var e = new DirectoryEntry(fat, buf);
                if (e.FirstClusterOfFile == target.FirstClusterOfFile &&
                    e.FileSize == target.FileSize &&
                    e.FileName == target.FileName)
                    return pos;
            }

            throw new FileNotFoundException("Directory entry not found.");
        }
    }
}
