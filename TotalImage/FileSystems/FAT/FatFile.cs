using System;
using System.IO;

namespace TotalImage.FileSystems.FAT
{
    /*
     * FAT12/FAT16/FAT32 file entry class.
     *
     * Just for looks atm
     */
    public class FatFile : File
    {
        DirectoryEntry entry;

        public FatFile(Fat12 fat, DirectoryEntry entry, Directory dir) : base(fat, dir)
        {
            this.entry = entry;
        }

        public override string Name
        {
            get => Helper.TrimFileName(entry.name);
            set => throw new NotImplementedException();
        }

        public override FileAttributes Attributes
        {
            get => (FileAttributes)entry.attr;
            set => throw new NotImplementedException();
        }

        public override DateTime LastAccessTime
        {
            get => Helper.FatToDateTime(entry.lstAccDate);
            set => throw new NotImplementedException();
        }

        public override DateTime LastWriteTime
        {
            get => Helper.FatToDateTime(entry.wrtDate, entry.wrtTime);
            set => throw new NotImplementedException();
        }

        public override DateTime CreationTime
        {
            get => Helper.FatToDateTime(entry.crtDate, entry.crtTime, entry.crtTimeTenth);
            set => throw new NotImplementedException();
        }

        public override long Length
        {
            get => entry.fileSize;
            set => throw new NotImplementedException();
        }

        public override string Extension => entry.name.Substring(8).Trim();

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override void MoveTo(string path)
        {
            throw new NotImplementedException();
        }
    }
}