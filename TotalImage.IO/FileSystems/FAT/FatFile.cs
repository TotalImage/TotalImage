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
            set => Rename(value); //Is this the right way to do it?
        }

        /// <inheritdoc />
        public string? LongName
        {
            get => lfnEntries.Length > 0 ? LongDirectoryEntry.CombineEntries(lfnEntries) : null;
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
            get => (FileAttributes)entry.Attributes;
            set => ChangeAttributes(value);
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
        public override void Erase()
        {
            Delete();

            //TODO: Here we also zero out every cluster of the file to ensure that the data is actually erased
        }

        /// <inheritdoc />
        public override void Delete()
        {
            //First, check if the file is already deleted, just in case
            if (entry.FileNameBytes[0] == 0xE5)
            {
                throw new InvalidOperationException("File is already deleted.");
            }

            var stream = fat.GetStream();

            uint[] clusters = fat.MainFat.GetClusterChain(FirstCluster); //Should we first check FAT integrity here just to be sure?

            //Perform the change in all the present FATs
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
            var newEntry = entry.GetBytes();
            newEntry[0] = 0xE5;
            entry = new DirectoryEntry(fat, newEntry.AsSpan());

            //Write the directory entry change to the stream
            stream.Seek(entryOffset, SeekOrigin.Begin);
            stream.Write(newEntry, 0, newEntry.Length);
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

        /// <inheritdoc />
        protected override void Rename(string name)
        {
            //First, check if the file is already deleted, just in case
            if (entry.FileNameBytes[0] == 0xE5)
            {
                throw new InvalidOperationException("File is deleted and cannot be renamed.");
            }

            //Get the stream, make a new directory entry for the file with new name, replace the old entry and write it to the stream
            var stream = fat.GetStream();

            //Not sure if this is the way to do it - what about ShortName and LongName?
            Name = name;

            var entryOffset = 0xB40; //TODO: Get the actual dir entry offset here!!!
            var newEntry = entry.GetBytes();
            Encoding.ASCII.GetBytes(name, 0, 11, newEntry, 0);
            entry = new DirectoryEntry(fat, newEntry.AsSpan());

            stream.Seek(entryOffset, SeekOrigin.Begin);
            stream.Write(newEntry, 0, newEntry.Length);
        }

        /// <inheritdoc />
        protected override void ChangeAttributes(FileAttributes attributes)
        {
            //First, check if the directory is already deleted, just in case
            if (entry.FileNameBytes[0] == 0xE5)
            {
                throw new InvalidOperationException("File is deleted and its attributes cannot be changed.");
            }

            var stream = fat.GetStream();

            //Not sure if this is the way to do it - what about ShortName and LongName?
            Attributes = attributes;
            var entryOffset = 0xB40; //TODO: Get the actual dir entry offset here!!!
            var newEntry = entry.GetBytes();
            newEntry[11] = (byte)attributes;
            entry = new DirectoryEntry(fat, newEntry.AsSpan());

            stream.Seek(entryOffset, SeekOrigin.Begin);
            stream.Write(newEntry, 0, newEntry.Length);
        }
    }
}
