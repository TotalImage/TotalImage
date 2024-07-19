using System;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// A file in a FAT file system.
    /// </summary>
    public class FatFile : File, IFatFileSystemObject
    {
        private DirectoryEntry entry;
        private LongDirectoryEntry[] lfnEntries;
        private FatFileSystem fat;

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

        public FatFile(FatFileSystem fat, DirectoryEntry entry, LongDirectoryEntry[] lfnEntries, Directory dir) : base(fat, dir)
        {
            this.entry = entry;
            this.lfnEntries = lfnEntries;
            this.fat = fat;
        }

        /// <inheritdoc />
        public override void Delete()
        {
            //TODO: Currently this only marks the file's clusters in all FATs as free. However, we still need to mark the directory entry as deleted as well.
            uint[] clusters = fat.MainFat.GetClusterChain(FirstCluster); //Should we first check FAT integrity here just to be sure?

            //Perform the change in all the present FATs
            foreach (FileAllocationTable table in fat.Fats)
            {
                foreach (uint cluster in clusters)
                {
                    table[cluster] = 0;
                }
            }
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
    }
}
