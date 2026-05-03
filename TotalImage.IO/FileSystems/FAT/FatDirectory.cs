using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            set => Rename(value); //Is this the right way to do it?
        }

        /// <inheritdoc />
        public string? LongName
        {
            get => lfnEntries?.Length > 0 ? LongDirectoryEntry.CombineEntries(lfnEntries) : null;
            set => Rename(value); //Is this the right way to do it?
        }

        /// <inheritdoc />
        public override string Name
        {
            get => LongName ?? ShortName;
            set => Rename(value); //Is this the right way to do it?
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
        public override void Erase()
        { 
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Delete()
        {
            //First, check if the directory is already deleted, just in case
            if (entry.Value.FileNameBytes[0] == 0xE5)
            {
                throw new InvalidOperationException("Directory is already deleted.");
            }

            var stream = fat.GetStream();

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

            //TODO: Here, we need to write the FAT cluster changes to the stream as well, not just in memory

            var entryOffset = 0xB40; //Get the actual dir entry offset here!!!

            //Change the first character of the file name in the directory entry to 0xE5 to mark it as deleted
            var newEntry = entry.Value.GetBytes();
            newEntry[0] = 0xE5;
            entry = new DirectoryEntry(fat, newEntry.AsSpan());

            //Write the directory entry change to the stream
            stream.Seek(entryOffset, SeekOrigin.Begin);
            stream.Write(newEntry, 0, newEntry.Length);
        }

        /// <inheritdoc />
        public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden, bool showDeleted)
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

        private void Rename(string name)
        {
            //First, check if the directory is already deleted, just in case
            if (entry.Value.FileNameBytes[0] == 0xE5)
            {
                throw new InvalidOperationException("Directory is deleted and cannot be renamed.");
            }

            //Get the stream, make a new directory entry for the directory with new name, replace the old entry and write it to the stream
            var stream = fat.GetStream();

            //Not sure if this is the way to do it - what about ShortName and LongName?
            Name = name;

            var entryOffset = 0xB40; //TODO: Get the actual dir entry offset here!!!
            var newEntry = entry.Value.GetBytes();
            Encoding.ASCII.GetBytes(name, 0, 11, newEntry, 0);
            entry = new DirectoryEntry(fat, newEntry.AsSpan());

            stream.Seek(entryOffset, SeekOrigin.Begin);
            stream.Write(newEntry, 0, newEntry.Length);
        }
    }
}
