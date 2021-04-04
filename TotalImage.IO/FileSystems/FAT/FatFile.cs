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
        private DirectoryEntry[]? lfnEntries;

        public FatFile(Fat12 fat, DirectoryEntry entry, DirectoryEntry[]? lfnEntries, Directory dir) : base(fat, dir)
        {
            this.entry = entry;
            this.lfnEntries = lfnEntries;
        }

        /// <inheritdoc />
        public string ShortName
        {
            get => Helper.TrimFileName(Encoding.ASCII.GetString(entry.name));
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string? LongName
        {
            get => lfnEntries != null ? Helper.RetrieveLongName(lfnEntries) : null;
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
            get => (FileAttributes)entry.attr;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? LastAccessTime
        {
            get => Helper.FatToDateTime(entry.lstAccDate);
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? LastWriteTime
        {
            get => Helper.FatToDateTime(entry.wrtDate, entry.wrtTime);
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? CreationTime
        {
            get => Helper.FatToDateTime(entry.crtDate, entry.crtTime, entry.crtTimeTenth);
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ulong Length
        {
            get => entry.fileSize;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override string Extension => Encoding.ASCII.GetString(entry.name).Substring(8).Trim();

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
            return new FatDataStream((Fat12)FileSystem, entry);
        }
    }
}