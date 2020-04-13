using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /*
     * FAT12/FAT16/FAT32 directory class. Implements directory entry enumeration.
     */
    public class FatDirectory : Directory
    {
        protected Fat12 fat;
        DirectoryEntry entry;
        Directory root, parent;

        public FatDirectory(Fat12 fat, DirectoryEntry entry, Directory root, Directory parent)
        {
            this.fat = fat;
            this.entry = entry;
            this.root = root;
            this.parent = parent;
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

        public override Directory Parent => parent;
        public override Directory Root => root;

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
            var dataAreaOffset = (uint)(fat.BiosParameterBlock.BytesPerLogicalSector + (fat.BiosParameterBlock.BytesPerLogicalSector * fat.BiosParameterBlock.LogicalSectorsPerFAT * 2) + 
                (fat.BiosParameterBlock.RootDirectoryEntries << 5));
            var fat1Offset = fat.BiosParameterBlock.BytesPerLogicalSector;
            var stream = fat.GetStream();

            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                var cluster = entry.fstClusLO;

                do
                {
                    uint clusterOffset = (uint)((cluster - 2) * fat.BiosParameterBlock.LogicalSectorsPerCluster * fat.BiosParameterBlock.BytesPerLogicalSector);

                    //No. of entries that fit in one cluster = BPS * SPC / 32 bytes per entry
                    for (int i = 0; i < (fat.BiosParameterBlock.LogicalSectorsPerCluster * fat.BiosParameterBlock.BytesPerLogicalSector / 32); i++)
                    {
                        stream.Seek(dataAreaOffset + clusterOffset + (i * 32), SeekOrigin.Begin);
                        byte firstByte = reader.ReadByte();
                        if (firstByte == 0x00)
                        {
                            break; //Directory is empty, go up one level and onto the next
                        }
                        if(firstByte == 0x2E)
                        {
                            continue; //Ignore . and .. entries
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
                                yield return new FatDirectory(fat, entry, root, this);
                            }
                            else
                            {
                                yield return new FatFile(fat, entry, this);
                            }
                        }
                    }
                    if(cluster % 2 == 0)
                    {
                        stream.Seek(fat1Offset + (ushort)(cluster * 1.5), SeekOrigin.Begin);
                        ushort lower8 = reader.ReadByte();
                        ushort upper4 = (ushort)((reader.ReadByte() & 0x0F) << 8);
                        cluster = (ushort)(upper4 + lower8);
                    }
                    else
                    {
                        stream.Seek(fat1Offset + (ushort)Math.Floor(cluster * 1.5), SeekOrigin.Begin);
                        ushort lower4 = (ushort)((reader.ReadByte() & 0xF0) >> 4);
                        ushort upper8 = (ushort)(reader.ReadByte() << 4);
                        cluster = (ushort)(upper8 + lower4);
                    }
                }
                while (cluster <= 0x0FEF);
            }
        }

        public override void MoveTo(string path)
        {
            throw new NotImplementedException();
        }
    }
}