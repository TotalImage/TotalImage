using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /*
     * FAT12/FAT16/FAT32 root directory class. Implements root dir enumeration
     *
     * The root directory is a special directory in the FAT filesystem, as it is
     * present in a well-known location, as opposed to being referred to from an
     * entry in the parent directory.
     *
     * Separate class for now, aim to integrate this into FatDirectory.cs
     */
    public class FatRootDirectory : Directory
    {
        public FatRootDirectory(Fat12 fat) : base(fat, null) { }

        public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects()
        {
            var fat = FileSystem as Fat12;
            var rootDirOffset = (uint)(fat.BiosParameterBlock.BytesPerLogicalSector + (fat.BiosParameterBlock.BytesPerLogicalSector * fat.BiosParameterBlock.LogicalSectorsPerFAT * fat.BiosParameterBlock.NumberOfFATs));
            var stream = fat.GetStream();
            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                stream.Seek(rootDirOffset, SeekOrigin.Begin);

                //Read the entries top to bottom
                for (int i = 0; i < fat.BiosParameterBlock.RootDirectoryEntries; i++)
                {
                    stream.Seek(rootDirOffset + i * 0x20, SeekOrigin.Begin);
                    byte firstByte = reader.ReadByte();

                    /* 0x00/0xF6 = no more entries after this one, stop
                     * 0xE5/0x05 = deleted entry, skip for now 
                     * 0x2E      = virtual . and .. folders, skip*/
                    if (firstByte == 0x00 || firstByte == 0xF6) break;
                    else if (firstByte == 0x2E) continue;
                    else if ((firstByte == 0xE5 || firstByte == 0x05) && !Settings.ShowDeletedItems) continue;

                    stream.Seek(-0x01, SeekOrigin.Current);
                    var entry = DirectoryEntry.Parse(reader.ReadBytes(32));

                    //Skip LFN and volume label entries for now
                    if (Convert.ToBoolean(entry.attr & 0x08))
                    {
                        continue;
                    }

                    //Folder entry
                    if (Convert.ToBoolean(entry.attr & 0x10))
                    {
                        yield return new FatDirectory(fat, entry, this);
                    }
                    //File entry
                    else if (!Convert.ToBoolean(entry.attr & 0x10))
                    {
                        yield return new FatFile(fat, entry, this);
                    }
                }
            }
        }

        public override Directory CreateSubdirectory(string path)
        {
            throw new NotImplementedException();
        }

        public override void Delete()
            => throw new InvalidOperationException();

        public override void MoveTo(string path)
            => throw new InvalidOperationException();

        public override string Name
        {
            get => string.Empty;
            set => throw new InvalidOperationException();
        }

        public override FileAttributes Attributes
        {
            get => FileAttributes.Directory;
            set => throw new NotSupportedException();
        }
        public override DateTime? LastAccessTime
        {
            get => null;
            set => throw new NotSupportedException();
        }
        public override DateTime? LastWriteTime
        {
            get => null;
            set => throw new NotSupportedException();
        }
        public override DateTime? CreationTime
        {
            get => null;
            set => throw new NotSupportedException();
        }
        public override ulong Length
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        //Checks if an entry with the specified name already exists in the root directory
        public bool EntryExists(string fullname)
        {
            if (string.IsNullOrEmpty(fullname))
                throw new ArgumentNullException(nameof(fullname), "fullname cannot be null!");

            string name = fullname.Substring(0, fullname.IndexOf('.')).PadRight(8, ' ');
            string ext = fullname.Substring(fullname.IndexOf('.'), fullname.Length - 1).PadRight(3, ' ');

            return false; //Bogus, needs to actually check all the entries which aren't in this class yet...
        }

        //Returns whether the root directory is full
        public bool IsFull()
        {
            /* Bogus, this needs to actually check the number of normal (non-deleted) entries in the root directory and compare it
             * to the value in the BPB/floppyTable, then return the result... */
            return false;
        }
    }
}