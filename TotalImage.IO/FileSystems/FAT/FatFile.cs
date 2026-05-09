using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TotalImage.Changes;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// A file in a FAT file system.
    /// </summary>
    public class FatFile : File, IFatFileSystemObject
    {
        private DirectoryEntry entry;
        private LongDirectoryEntry[] lfnEntries;

        /// <inheritdoc />
        public string ShortName
        {
            get => entry.FileName;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string? LongName
        {
            get => lfnEntries.Length > 0 ? LongDirectoryEntry.CombineEntries(lfnEntries) : null;
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
            get => (FileAttributes)entry.Attributes;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? LastAccessTime
        {
            get => entry.LastAccessTime;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? LastWriteTime
        {
            get => entry.LastWriteTime;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? CreationTime
        {
            get => entry.CreationTime;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ulong Length
        {
            get => entry.FileSize;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override string Extension => entry.Extension;

        /// <inheritdoc />
        public uint FirstCluster
        {
            get => entry.FirstClusterOfFile;
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a FAT file from a directory entry.
        /// </summary>
        /// <param name="fat">The file system that owns the file.</param>
        /// <param name="entry">The directory entry describing the file.</param>
        /// <param name="lfnEntries">The long file name entries associated with <paramref name="entry"/>.</param>
        /// <param name="dir">The parent directory.</param>
        public FatFile(FatFileSystem fat, DirectoryEntry entry, LongDirectoryEntry[] lfnEntries, Directory dir) : base(fat, dir)
        {
            this.entry = entry;
            this.lfnEntries = lfnEntries;
        }

        /// <inheritdoc />
        public override void Delete()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void MoveTo(string path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Stream GetStream()
        {
            return new FatDataStream((FatFileSystem)FileSystem, entry);
        }

        // -----------------------------------------------------------------------
        // Public mutation entry points — validate and enqueue changes
        // -----------------------------------------------------------------------

        /// <summary>
        /// Stages the deletion of this file.
        /// </summary>
        public void EnqueueDelete()
        {
            var container = ((FatFileSystem)FileSystem).OwningContainer
                ?? throw new InvalidOperationException("This file system is not associated with a container.");

            container.PendingChanges.Add(new DeleteEntryChange(FatDirectory.GetPathComponents(this)));
        }

        /// <summary>
        /// Stages a rename of this file.
        /// </summary>
        /// <param name="newName">The new 8.3 filename.</param>
        public void EnqueueRename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("New name must not be empty.", nameof(newName));

            var container = ((FatFileSystem)FileSystem).OwningContainer
                ?? throw new InvalidOperationException("This file system is not associated with a container.");

            container.PendingChanges.Add(new RenameChange(FatDirectory.GetPathComponents(this), newName));
        }

        // -----------------------------------------------------------------------
        // Internal write helpers (used by ChangeApplicator during commit)
        // -----------------------------------------------------------------------

        /// <summary>
        /// Marks this file as deleted (first byte of short name → 0xE5) and frees its clusters.
        /// </summary>
        internal void WriteDelete()
        {
            var fat = (FatFileSystem)FileSystem;
            var stream = fat.GetStream();
            long entryOffset = FindEntryOffset(fat);
            stream.Position = entryOffset;
            stream.WriteByte(0xE5);

            if (entry.FirstClusterOfFile >= 2)
                fat.FreeClusterChain(entry.FirstClusterOfFile);
        }

        /// <summary>
        /// Renames this file entry (8.3 only) by rewriting the first 11 bytes of its directory entry.
        /// </summary>
        internal void WriteRename(string newName)
        {
            var fat = (FatFileSystem)FileSystem;
            var stream = fat.GetStream();
            long entryOffset = FindEntryOffset(fat);

            var shortNameBytes = ToShortNameBytes(newName);
            stream.Position = entryOffset;
            stream.Write(shortNameBytes, 0, 11);
        }

        private static byte[] ToShortNameBytes(string name)
        {
            var result = new byte[11];
            for (int i = 0; i < 11; i++) result[i] = 0x20;
            var dot = name.IndexOf('.');
            string basePart = dot < 0 ? name.ToUpperInvariant() : name[..dot].ToUpperInvariant();
            string extPart  = dot < 0 ? string.Empty : name[(dot + 1)..].ToUpperInvariant();
            var baseBytes = Encoding.ASCII.GetBytes(basePart);
            var extBytes  = Encoding.ASCII.GetBytes(extPart);
            Array.Copy(baseBytes, result, Math.Min(8, baseBytes.Length));
            if (extBytes.Length > 0)
                Array.Copy(extBytes, 0, result, 8, Math.Min(3, extBytes.Length));
            return result;
        }

        /// <summary>
        /// Searches the parent directory for the stream offset of this file's directory entry.
        /// </summary>
        private long FindEntryOffset(FatFileSystem fat)
        {
            var stream = fat.GetStream();
            var parentDir = (FatDirectory?)Directory;

            IEnumerable<long> CandidateOffsets()
            {
                if (parentDir?.entry is null && fat.BiosParameterBlock is not Fat32BiosParameterBlock)
                {
                    long rootStart = (fat.ReservedSectors + fat.ClusterMapsSectors) * fat.BiosParameterBlock.BytesPerLogicalSector;
                    long rootEnd = rootStart + (long)fat.BiosParameterBlock.RootDirectoryEntries * 32;
                    for (long pos = rootStart; pos < rootEnd; pos += 32)
                        yield return pos;
                }
                else
                {
                    uint firstCluster = parentDir?.entry?.FirstClusterOfFile
                        ?? ((Fat32BiosParameterBlock)fat.BiosParameterBlock).RootDirectoryCluster;
                    long dataBase = fat.DataAreaFirstSector * fat.BiosParameterBlock.BytesPerLogicalSector;
                    foreach (var cl in fat.MainFat.GetClusterChain(firstCluster))
                    {
                        long clStart = dataBase + (long)(cl - 2) * fat.BytesPerCluster;
                        for (long pos = clStart; pos < clStart + fat.BytesPerCluster; pos += 32)
                            yield return pos;
                    }
                }
            }

            foreach (var pos in CandidateOffsets())
            {
                stream.Position = pos;
                var buf = new byte[32];
                stream.Read(buf, 0, 32);
                if (buf[0] == 0x00) break;
                if (buf[0] == 0xE5) continue;
                var e = new DirectoryEntry(fat, buf);
                if (e.FirstClusterOfFile == entry.FirstClusterOfFile &&
                    e.FileSize == entry.FileSize &&
                    e.FileName == entry.FileName)
                    return pos;
            }

            throw new FileNotFoundException($"Directory entry for '{entry.FileName}' not found.");
        }
    }
}
