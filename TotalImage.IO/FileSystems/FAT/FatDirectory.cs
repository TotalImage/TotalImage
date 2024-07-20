using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private FatFileSystem fat;

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
            this.fat = fat;
        }

        /// <inheritdoc />
        public override Directory CreateSubdirectory(string path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Delete()
        {
            //TODO: Currently this only marks the clusters in all FATs as free. However, we still need to mark the directory entries as deleted as well.

            //First enumerate all the extant objects in this directory
            IEnumerable<FileSystemObject> objects = EnumerateFileSystemObjects(false, false);

            //If this directory is not empty, recursively call Delete() on everything inside
            if (objects.Any())
            {
                foreach (FileSystemObject obj in objects)
                {
                    obj.Delete();
                } 
            }

            uint[] clusters = fat.MainFat.GetClusterChain(FirstCluster); //Should we first check FAT integrity here just to be sure?

            //Finally, delete the directory itself as well
            foreach (FileAllocationTable table in fat.Fats)
            {
                foreach (uint cluster in clusters)
                {
                    table[cluster] = 0;
                }
            }
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
