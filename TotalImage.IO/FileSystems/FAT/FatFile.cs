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
    public class FatFile : File
    {
        private DirectoryEntry entry;

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

        public override DateTime? LastAccessTime
        {
            get => Helper.FatToDateTime(entry.lstAccDate);
            set => throw new NotImplementedException();
        }

        public override DateTime? LastWriteTime
        {
            get => Helper.FatToDateTime(entry.wrtDate, entry.wrtTime);
            set => throw new NotImplementedException();
        }

        public override DateTime? CreationTime
        {
            get => Helper.FatToDateTime(entry.crtDate, entry.crtTime, entry.crtTimeTenth);
            set => throw new NotImplementedException();
        }

        public override ulong Length
        {
            get => entry.fileSize;
            set => throw new NotImplementedException();
        }

        public override string Extension => entry.name.Substring(8).Trim();

        public override void Delete()
        {
            /* When deleting a file, only the first character of the name needs to be changed to 0xE5.
             * The file's directory entry can then be reused, and its clusters are marked as free until they're
             * overwritten. 
             * This code is untested until this class is hooked up to the UI... */
            byte[] bytes = Encoding.ASCII.GetBytes(entry.name);
            bytes[0] = 0xE5;
            entry.name = Encoding.ASCII.GetString(bytes);

            //And then mark all clusters in the chain as free...
        }

        public override void MoveTo(string path)
        {
            throw new NotImplementedException();
        }
    }
}