using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TotalImage.FileSystems.FAT
{
    public class Fat12
    {
        private frmMain main;
        private Stream stream;
        private BiosParameterBlock bpb;

        protected Fat12()
        {
            main = (frmMain)Application.OpenForms["frmMain"];
        }

        public Fat12(Stream stream) : this()
        {
            this.stream = stream;
            bpb = Parse();
        }

        public BiosParameterBlock Parse()
        {
            var bpb = new BiosParameterBlock();
            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                stream.Seek(0x0B, SeekOrigin.Begin); //BPB offset

                bpb.BytesPerLogicalSector = reader.ReadUInt16();
                bpb.LogicalSectorsPerCluster = reader.ReadByte();
                bpb.ReservedLogicalSectors = reader.ReadUInt16();
                bpb.NumberOfFATs = reader.ReadByte();
                bpb.RootDirectoryEntries = reader.ReadUInt16();
                reader.ReadUInt16();
                bpb.MediaDescriptor = reader.ReadByte();
                bpb.LogicalSectorsPerFAT = reader.ReadUInt16();
                bpb.PhysicalSectorsPerTrack = reader.ReadUInt16();
                bpb.NumberOfHeads = reader.ReadUInt16();
                bpb.HiddenSectors = reader.ReadUInt32();
                bpb.LargeTotalLogicalSectors = reader.ReadUInt32();

                // Try to read the BPB further as a DOS 4.0 BPB.
                var bpb40 = new BiosParameterBlock40(bpb);
                bpb40.PhysicalDriveNumber = reader.ReadByte();
                bpb40.Flags = reader.ReadByte();

                switch(reader.ReadByte())
                {
                    case 40:
                        bpb40.BpbVersion = BiosParameterBlockVersion.Dos34;
                        break;
                    case 41:
                        bpb40.BpbVersion = BiosParameterBlockVersion.Dos40;
                        break;
                    default:
                        return bpb; // it's not a DOS 4.0 BPB, don't bother any further
                }

                bpb40.VolumeSerialNumber = reader.ReadUInt32();

                if(bpb40.BpbVersion == BiosParameterBlockVersion.Dos40)
                {
                    bpb40.VolumeLabel = new string(reader.ReadChars(11));
                    bpb40.FileSystemType = new string(reader.ReadChars(8));
                }

                return bpb;
            }
        }

        //Formats a volume with FAT12 file system - currently assumes it's a floppy disk...
        public static Fat12 Create(Stream stream, BiosParameterBlock bpb, byte tracks)
        {
            var fat = new Fat12();
            fat.stream = stream;

            uint totalSize = (uint)stream.Length;
            uint rootDirSize = (uint)(bpb.RootDirectoryEntries << 5);
            uint fatSize = (uint)(bpb.LogicalSectorsPerFAT * bpb.BytesPerLogicalSector);
            uint fat1Offset = (uint)(bpb.ReservedLogicalSectors * bpb.BytesPerLogicalSector);
            uint fat2Offset = fat1Offset + fatSize;
            uint dataAreaOffset = fat2Offset + fatSize + rootDirSize;

            //using (var stream = new MemoryStream(imageBytes, writable: true))
            using (var writer = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                stream.Seek(0, SeekOrigin.Begin);
                for (uint i = 0; i < totalSize; i++)
                {
                    writer.Write((byte)0);
                }

                //Fille the data area with 0xF6
                stream.Seek(dataAreaOffset, SeekOrigin.Begin);
                for (uint i = 0; i < totalSize - dataAreaOffset; i++)
                {
                    writer.Write((byte)0xF6);
                }

                stream.Seek(0, SeekOrigin.Begin);
                writer.Write(bpb.BootJump, 0, 3);
                writer.Write(bpb.OemId.PadRight(8, ' ').ToCharArray());
                writer.Write(bpb.BytesPerLogicalSector);
                writer.Write(bpb.LogicalSectorsPerCluster);
                writer.Write(bpb.ReservedLogicalSectors);
                writer.Write(bpb.NumberOfFATs);
                writer.Write(bpb.RootDirectoryEntries);
                writer.Write((ushort)(tracks * bpb.PhysicalSectorsPerTrack * bpb.NumberOfHeads));
                writer.Write(bpb.MediaDescriptor);
                writer.Write(bpb.LogicalSectorsPerFAT);
                writer.Write(bpb.PhysicalSectorsPerTrack);
                writer.Write(bpb.NumberOfHeads);
                writer.Write(bpb.HiddenSectors);
                writer.Write(bpb.LargeTotalLogicalSectors);

                //DOS 3.4+ specific values
                {
                    if (bpb is BiosParameterBlock40 bpb40)
                    {
                        writer.Write(bpb40.PhysicalDriveNumber);
                        writer.Write(bpb40.Flags);
                        
                        if (bpb.BpbVersion == BiosParameterBlockVersion.Dos34)
                            writer.Write((byte)40);
                        else if (bpb.BpbVersion == BiosParameterBlockVersion.Dos40)
                            writer.Write((byte)41);
                        else
                            throw new Exception("PANIIIIIIC");

                        writer.Write(bpb40.VolumeSerialNumber);

                        //DOS 4.0 adds volume label and FS type as well
                        if (bpb40.BpbVersion == BiosParameterBlockVersion.Dos40)
                        {
                            if (string.IsNullOrEmpty(bpb40.VolumeLabel))
                                writer.Write("NO NAME    ".ToCharArray());
                            else
                                writer.Write(bpb40.VolumeLabel.PadRight(11, ' ').ToCharArray());
                            writer.Write(bpb40.FileSystemType.PadRight(8, ' ').ToCharArray());
                        }
                    }
                }

                //Boot signature
                stream.Seek(0x1FE, SeekOrigin.Begin);
                writer.Write((byte)0x55);
                writer.Write((byte)0xAA);

                //Media descriptor needs to be written to each FAT as well
                stream.Seek(fat1Offset, SeekOrigin.Begin);
                writer.Write(bpb.MediaDescriptor);
                writer.Write((byte)0xFF);
                writer.Write((byte)0xFF);
                stream.Seek(fat2Offset, SeekOrigin.Begin);
                writer.Write(bpb.MediaDescriptor);
                writer.Write((byte)0xFF);
                writer.Write((byte)0xFF);

                //Volume label needs to be written to the root directory as well
                stream.Seek(fat2Offset + fatSize, SeekOrigin.Begin);

                //First 11 bytes (8.3 space-padded filename) are the label itself
                {
                    if(bpb is BiosParameterBlock40 bpb40 && !string.IsNullOrEmpty(bpb40.VolumeLabel))
                    {
                        writer.Write(bpb40.VolumeLabel.PadRight(11, ' ').ToCharArray());
                        writer.Write((byte)0x08); //Volume label attribute
                    }
                }
            }

            fat.bpb = fat.Parse();
            return fat;
        }

        //Reads the root directory entries and any subdirectories and adds them to the treeview
        public void ReadRootDir()
        {
            uint rootDirOffset = (uint)(bpb.BytesPerLogicalSector + (bpb.BytesPerLogicalSector * bpb.LogicalSectorsPerFAT * 2));
            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                stream.Seek(rootDirOffset, SeekOrigin.Begin);

                //Read the entries top to bottom
                for (int i = 0; i < bpb.RootDirectoryEntries; i++)
                {
                    stream.Seek(rootDirOffset + i * 0x20, SeekOrigin.Begin);
                    if (reader.ReadByte() == 0x00)
                    {
                        break; //Empty entry, no entries after this one
                    }

                    stream.Seek(-0x01, SeekOrigin.Current);
                    FatDirEntry entry = new FatDirEntry
                    {
                        filename = reader.ReadBytes(8),
                        extension = reader.ReadBytes(3),
                        attribute = reader.ReadByte(),
                        caseByte = reader.ReadByte(),
                        creationTimeMs = reader.ReadByte(),
                        creationTime = reader.ReadUInt16(),
                        creationDate = reader.ReadUInt16(),
                        lastAccessDate = reader.ReadUInt16(),
                        FAT3232StartCluster = reader.ReadUInt16(),
                        modifiedTime = reader.ReadUInt16(),
                        modifiedDate = reader.ReadUInt16(),
                        startCluster = reader.ReadUInt16(),
                        fileSize = reader.ReadUInt32()
                    };

                    //Ignore deleted entries for now
                    if (entry.filename[0] != 0xE5)
                    {
                        //Skip hidden, LFN and volume label entries for now
                        if (Convert.ToBoolean(entry.attribute & 0x02) || entry.attribute == 0x0F || Convert.ToBoolean(entry.attribute & 0x08))
                        {
                            continue;
                        }

                        //Folder entry
                        if (Convert.ToBoolean(entry.attribute & 0x10))
                        {
                            main.AddToRootDir(entry);
                            main.AddToFileList(entry);
                            ReadSubdir(entry);
                        }
                        //File entry
                        else if (!Convert.ToBoolean(entry.attribute & 0x10))
                        {
                            main.AddToFileList(entry);
                        }
                    }
                }
            }

            main.SortDirTree();
        }

        //Reads the subdirectory entries and adds subdirectories to the treeview
        public void ReadSubdir(FatDirEntry parent)
        {
            uint dataAreaOffset = (uint)(bpb.BytesPerLogicalSector + (bpb.BytesPerLogicalSector * bpb.LogicalSectorsPerFAT * 2) + 
                (bpb.RootDirectoryEntries << 5));
            ushort fat1Offset = bpb.BytesPerLogicalSector;

            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                ushort cluster = parent.startCluster;

                do
                {
                    uint clusterOffset = (uint)((cluster - 2) * bpb.LogicalSectorsPerCluster * bpb.BytesPerLogicalSector);

                    //No. of entries that fit in one cluster = BPS * SPC / 32 bytes per entry
                    for (int i = 0; i < (bpb.LogicalSectorsPerCluster * bpb.BytesPerLogicalSector / 32); i++)
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
                        FatDirEntry entry = new FatDirEntry
                        {
                            filename = reader.ReadBytes(8),
                            extension = reader.ReadBytes(3),
                            attribute = reader.ReadByte(),
                            caseByte = reader.ReadByte(),
                            creationTimeMs = reader.ReadByte(),
                            creationTime = reader.ReadUInt16(),
                            creationDate = reader.ReadUInt16(),
                            lastAccessDate = reader.ReadUInt16(),
                            FAT3232StartCluster = reader.ReadUInt16(),
                            modifiedTime = reader.ReadUInt16(),
                            modifiedDate = reader.ReadUInt16(),
                            startCluster = reader.ReadUInt16(),
                            fileSize = reader.ReadUInt32()
                        };

                        //Ignore deleted entries for now
                        if (entry.filename[0] != 0xE5)
                        {
                            //Skip hidden, LFN and volume label entries for now
                            if (Convert.ToBoolean(entry.attribute & 0x02) || entry.attribute == 0x0F || Convert.ToBoolean(entry.attribute & 0x08))
                            {
                                continue;
                            }

                            //Folder entry
                            if (Convert.ToBoolean(entry.attribute & 0x10))
                            {
                                main.AddToDir(parent, entry);
                                ReadSubdir(entry);
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

                main.SortDirTree();
            }
        }

        //Lists the contents of the directory without traversing subdirectories and adding them to the treeview
        public void ListDir(FatDirEntry parent)
        {
            uint dirSize = 0;
            ushort fat1Offset = bpb.BytesPerLogicalSector;
            ushort fatSize = (ushort)(bpb.BytesPerLogicalSector * bpb.LogicalSectorsPerFAT);
            uint rootDirOffset = (uint)(fat1Offset + (fatSize * 2));
            uint dataAreaOffset = (uint)(rootDirOffset + (bpb.RootDirectoryEntries << 5));

            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                ushort cluster = parent.startCluster;
                do
                {
                    uint clusterOffset = (uint)((cluster - 2) * bpb.LogicalSectorsPerCluster * bpb.BytesPerLogicalSector);

                    //No. of entries that fit in one cluster = BPS * SPC / 32 bytes per entry
                    for (int i = 0; i < (bpb.LogicalSectorsPerCluster * bpb.BytesPerLogicalSector / 32); i++)
                    {
                        stream.Seek(dataAreaOffset + clusterOffset + (i * 32), SeekOrigin.Begin);
                        byte[] filename = reader.ReadBytes(8);
                        if (filename[0] == 0x00)
                        {
                            break; //No entries will follow after this one, stop reading
                        }
                        if (Encoding.ASCII.GetString(filename).TrimEnd(' ') == ".")
                        {
                            continue; //Ignore "." and ".." entries
                        }

                        stream.Seek(-0x08, SeekOrigin.Current);
                        FatDirEntry entry = new FatDirEntry
                        {
                            filename = reader.ReadBytes(8),
                            extension = reader.ReadBytes(3),
                            attribute = reader.ReadByte(),
                            caseByte = reader.ReadByte(),
                            creationTimeMs = reader.ReadByte(),
                            creationTime = reader.ReadUInt16(),
                            creationDate = reader.ReadUInt16(),
                            lastAccessDate = reader.ReadUInt16(),
                            FAT3232StartCluster = reader.ReadUInt16(),
                            modifiedTime = reader.ReadUInt16(),
                            modifiedDate = reader.ReadUInt16(),
                            startCluster = reader.ReadUInt16(),
                            fileSize = reader.ReadUInt32()
                        };

                        //Ignore deleted entries for now
                        if (entry.filename[0] != 0xE5)
                        {
                            //Skip hidden, LFN and volume label entries for now
                            if (Convert.ToBoolean(entry.attribute & 0x02) || entry.attribute == 0x0F || Convert.ToBoolean(entry.attribute & 0x08))
                            {
                                continue;
                            }

                            dirSize += entry.fileSize;
                            main.AddToFileList(entry);
                        }
                    }
                    if (cluster % 2 == 0)
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

        //Lists the contents of the root directory without traversing subdirectories and adding them to the treeview
        public void ListRootDir()
        {
            uint rootDirOffset = (uint)(bpb.BytesPerLogicalSector + (bpb.BytesPerLogicalSector * bpb.LogicalSectorsPerFAT * 2));

            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                stream.Seek(rootDirOffset, SeekOrigin.Begin);

                //Read the entries top to bottom
                for (int i = 0; i < bpb.RootDirectoryEntries; i++)
                {
                    stream.Seek(rootDirOffset + i * 0x20, SeekOrigin.Begin);
                    if (reader.ReadByte() == 0x00)
                    {
                        break; //Empty entry, no entries after this one
                    }

                    stream.Seek(-0x01, SeekOrigin.Current);
                    FatDirEntry entry = new FatDirEntry
                    {
                        filename = reader.ReadBytes(8),
                        extension = reader.ReadBytes(3),
                        attribute = reader.ReadByte(),
                        caseByte = reader.ReadByte(),
                        creationTimeMs = reader.ReadByte(),
                        creationTime = reader.ReadUInt16(),
                        creationDate = reader.ReadUInt16(),
                        lastAccessDate = reader.ReadUInt16(),
                        FAT3232StartCluster = reader.ReadUInt16(),
                        modifiedTime = reader.ReadUInt16(),
                        modifiedDate = reader.ReadUInt16(),
                        startCluster = reader.ReadUInt16(),
                        fileSize = reader.ReadUInt32()
                    };

                    //Ignore deleted entries for now
                    if (entry.filename[0] != 0xE5)
                    {
                        //Skip hidden, LFN and volume label entries for now
                        if (Convert.ToBoolean(entry.attribute & 0x02) || entry.attribute == 0x0F || Convert.ToBoolean(entry.attribute & 0x08))
                        {
                            continue;
                        }

                        main.AddToFileList(entry);
                    }
                }
            }
        }

        /* Changes the volume label
         * 
         * If BPB version <= 3.31, only the root dir label is changed
         * If BPB version >= 3.40, both the root dir and BPB label are changed
         */
        public void ChangeVolLabel(string label)
        {
            uint rootDirOffset = (uint)(bpb.BytesPerLogicalSector + (bpb.BytesPerLogicalSector * bpb.LogicalSectorsPerFAT * 2));
            bool rootDirFull = false;

            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            using (var writer = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                stream.Seek(rootDirOffset, SeekOrigin.Begin);

                for (int i = 0; i < bpb.RootDirectoryEntries; i++)
                {
                    stream.Seek(rootDirOffset + i * 0x20, SeekOrigin.Begin);
                    if (reader.ReadByte() == 0x00)
                    {
                        stream.Seek(-0x01, SeekOrigin.Current);
                        //Root dir is empty, so let's add the first entry
                        writer.Write(label.ToCharArray());
                        writer.Write((byte)0x08); //Volume label attribute

                        break;
                    }

                    /*Directory not empty, we need to find the volume label, as it may not be the first entry
                     *...or it may not exist at all
                     */
                    stream.Seek(-0x01, SeekOrigin.Current);
                    FatDirEntry entry = new FatDirEntry
                    {
                        filename = reader.ReadBytes(8),
                        extension = reader.ReadBytes(3),
                        attribute = reader.ReadByte(),
                        caseByte = reader.ReadByte(),
                        creationTimeMs = reader.ReadByte(),
                        creationTime = reader.ReadUInt16(),
                        creationDate = reader.ReadUInt16(),
                        lastAccessDate = reader.ReadUInt16(),
                        FAT3232StartCluster = reader.ReadUInt16(),
                        modifiedTime = reader.ReadUInt16(),
                        modifiedDate = reader.ReadUInt16(),
                        startCluster = reader.ReadUInt16(),
                        fileSize = reader.ReadUInt32()
                    };

                    if(entry.attribute == 0x08)
                    {
                        stream.Seek(-0x20, SeekOrigin.Current);
                        writer.Write(label.ToCharArray());
                        writer.Write(0x08);
                        break;
                    }

                    if(i == bpb.RootDirectoryEntries - 1)
                    {
                        rootDirFull = true;
                    }
                }

                if (bpb is BiosParameterBlock40 && bpb.BpbVersion == BiosParameterBlockVersion.Dos40)
                {
                    stream.Seek(0x2B, SeekOrigin.Begin);
                    writer.Write(label.ToCharArray());
                }

                if (rootDirFull)
                {
                    throw new Exception("Root directory is full, volume label could not be written!");
                }
            }
        }
    }
}