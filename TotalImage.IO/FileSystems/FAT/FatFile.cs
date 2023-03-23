using System;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /*
     * FAT12/FAT16/FAT32 file entry class.
     *
     * Just for looks atm
     */
    public class FatFile : File, IFatFileSystemObject
    {
        private DirectoryEntry entry;
        private LongDirectoryEntry[]? lfnEntries;

        public FatFile(FatFileSystem fat, DirectoryEntry entry, LongDirectoryEntry[]? lfnEntries, Directory dir) : base(fat, dir)
        {
            this.entry = entry;
            this.lfnEntries = lfnEntries;
        }

        /// <inheritdoc />
        public string ShortName
        {
            get => entry.Name;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string? LongName
        {
            get => lfnEntries != null ? LongDirectoryEntry.CombineEntries(lfnEntries) : null;
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
            get => entry.Attributes;
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
        public override long Length
        {
            get => entry.fileSize;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override string Extension => entry.Extension;

        /// <inheritdoc />
        public uint FirstCluster
        {
            get => (uint)((entry.fstClusHI << 16) | entry.fstClusLO);
            set => throw new NotImplementedException();
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
    }
}
