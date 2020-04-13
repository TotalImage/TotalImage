using System;
using System.IO;

namespace TotalImage.FileSystems.FAT
{
    public class FatFile : File
    {
        DirectoryEntry entry;
        internal FatFile(DirectoryEntry entry)
        {
            this.entry = entry;
        }

        public override string Name
        {
            get => entry.name.Insert(8, ".");
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

        public override Directory Directory => throw new NotImplementedException();

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