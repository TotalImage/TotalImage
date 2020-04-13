using System;
using System.Collections.Generic;
using System.IO;

namespace TotalImage.FileSystems.FAT
{
    public class FatDirectory : Directory
    {
        DirectoryEntry entry;

        public FatDirectory(DirectoryEntry entry)
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

        public override Directory Parent => throw new NotImplementedException();
        public override Directory Root => throw new NotImplementedException();

        public override Directory CreateSubdirectory(string path)
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects()
        {
            throw new NotImplementedException();
        }

        public override void MoveTo(string path)
        {
            throw new NotImplementedException();
        }
    }
}