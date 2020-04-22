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
            var rootDirOffset = (uint)(fat.BiosParameterBlock.BytesPerLogicalSector + (fat.BiosParameterBlock.BytesPerLogicalSector * fat.BiosParameterBlock.LogicalSectorsPerFAT * 2));
            var stream = fat.GetStream();
            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                stream.Seek(rootDirOffset, SeekOrigin.Begin);

                //Read the entries top to bottom
                for (int i = 0; i < fat.BiosParameterBlock.RootDirectoryEntries; i++)
                {
                    stream.Seek(rootDirOffset + i * 0x20, SeekOrigin.Begin);
                    if (reader.ReadByte() == 0x00)
                    {
                        break; //Empty entry, no entries after this one
                    }

                    stream.Seek(-0x01, SeekOrigin.Current);
                    var entry = DirectoryEntry.Parse(reader.ReadBytes(32));

                    //Ignore deleted entries for now
                    if (entry.name[0] != 0xE5)
                    {
                        //Skip hidden, LFN and volume label entries for now
                        if (Convert.ToBoolean(entry.attr & 0x02) || entry.attr == 0x0F || Convert.ToBoolean(entry.attr & 0x08))
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
        public override long Length
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }
    }
}