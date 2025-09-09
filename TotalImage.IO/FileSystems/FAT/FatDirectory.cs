using System;
using System.Collections.Generic;
using System.IO;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// A directory containing other directories and files in a FAT file system.
    /// </summary>
    public class FatDirectory : Directory, IFatFileSystemObject
    {
        private DirectoryEntry? entry = null;
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

        public FatDirectory(FatFileSystem fat) : base(fat, null) { }

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
    }
}
