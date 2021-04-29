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
        public override ulong Length
        {
            get => entry.fileSize;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override string Extension => entry.Extension;

        /// <inheritdoc />
        public override void Delete()
        {
            /* When deleting a file, only the first character of the name needs to be changed to 0xE5.
             * The file's directory entry can then be reused, and its clusters are marked as free until they're
             * overwritten.
             * This code is untested until this class is hooked up to the UI... */
            entry.name[0] = 0xE5;

            //And then mark all clusters in the chain as free...
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