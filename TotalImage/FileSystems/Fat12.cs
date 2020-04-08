using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TotalImage.FileSystems
{
    public class Fat12
    {
        private frmMain main; 
        /*private BinaryReader reader;
        private MemoryStream stream;
        private BinaryWriter writer;*/
        private byte[] imageBytes;

        public Fat12(byte[] imageBytes)
        {
            this.imageBytes = imageBytes;
            main = (frmMain)Application.OpenForms["frmMain"];
            /*stream = new MemoryStream(this.imageBytes, true);
            reader = new BinaryReader(stream, Encoding.ASCII);
            writer = new BinaryWriter(stream, Encoding.ASCII);*/
        }

        public BiosParameterBlock Parse()
        {
            //var stream = new MemoryStream(imageBytes, false);
            var bpb = new BiosParameterBlock40();//Dos40BiosParameterBlock();

            using (var stream = new MemoryStream(imageBytes, writable: false))
            using (var reader = new BinaryReader(stream, Encoding.ASCII))
            {
                stream.Seek(0x0B, SeekOrigin.Begin); // BPB offset

                bpb.BytesPerLogicalSector = reader.ReadUInt16();
                bpb.LogicalSectorsPerCluster = reader.ReadByte();
                bpb.ReservedLogicalSectors = reader.ReadUInt16();
                bpb.NumberOfFATs = reader.ReadByte();
                bpb.RootDirectoryEntries = reader.ReadUInt16();
                /*bpb.TotalLogicalSectors =*/
                reader.ReadUInt16();
                bpb.MediaDescriptor = reader.ReadByte();
                bpb.LogicalSectorsPerFAT = reader.ReadUInt16();
                bpb.PhysicalSectorsPerTrack = reader.ReadUInt16();
                bpb.NumberOfHeads = reader.ReadUInt16();
                bpb.HiddenSectors = reader.ReadUInt32();
                bpb.LargeTotalLogicalSectors = reader.ReadUInt32();
                bpb.PhysicalDriveNumber = reader.ReadByte();
                bpb.Flags = reader.ReadByte();
                bpb.ExtendedBootSignature = reader.ReadByte();
                bpb.VolumeSerialNumber = reader.ReadUInt32();
                bpb.VolumeLabel = new byte[11];
                bpb.FileSystemType = new byte[8];
                bpb.VolumeLabel = Encoding.ASCII.GetBytes(reader.ReadChars(11));
                bpb.FileSystemType = Encoding.ASCII.GetBytes(reader.ReadChars(8));
            }

            return bpb;
        }

        //Formats a volume with FAT12 file system - currently assumes it's a floppy disk...
        public void Format(BiosParameterBlock bpb, string oemID, byte tracks)
        {
            //stream = new MemoryStream(imageBytes, true);
            uint total_size = (uint)imageBytes.Length;
            uint root_dir_bytes = (uint)(bpb.RootDirectoryEntries << 5);
            uint fat_size = (uint)(bpb.LogicalSectorsPerFAT * bpb.BytesPerLogicalSector);
            uint fat1_offs = (uint)(bpb.ReservedLogicalSectors * bpb.BytesPerLogicalSector);
            uint fat2_offs = fat1_offs + fat_size;
            uint zero_bytes = fat2_offs + fat_size + root_dir_bytes;

            using (var stream = new MemoryStream(imageBytes, writable: true))
            using (var writer = new BinaryWriter(stream, Encoding.ASCII))
            {
                imageBytes.Initialize(); //Fill the image with zeroes
                stream.Seek(zero_bytes, SeekOrigin.Begin);

                //Fill the data area with 0xF6
                for (uint i = 0; i < total_size - zero_bytes; i++)
                {
                    writer.Write((byte)0xF6);
                }

                stream.Seek(0, SeekOrigin.Begin);
                writer.Write((byte)0xEB); //Jump to bootcode
                writer.Write((byte)0x58);
                writer.Write((byte)0x90); //NOP

                byte[] oemIdBytes = Encoding.ASCII.GetBytes(oemID); //OEM ID
                writer.Write(oemIdBytes);
                if (oemIdBytes.Length < 8) //If it's shorter than 8 bytes, it needs to be padded with spaces
                {
                    for (int i = 0; i < 8 - oemIdBytes.Length; i++)
                        writer.Write((byte)0x20);
                }

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
                if (bpb.BpbVersion >= 0x34)
                {
                    writer.Write(((BiosParameterBlock40)bpb).PhysicalDriveNumber);
                    writer.Write(((BiosParameterBlock40)bpb).Flags);
                    writer.Write(((BiosParameterBlock40)bpb).ExtendedBootSignature);
                    writer.Write(((BiosParameterBlock40)bpb).VolumeSerialNumber);

                    //DOS 4.0 adds volume label and FS type as well
                    if (bpb.BpbVersion == 0x40)
                    {
                        writer.Write(((BiosParameterBlock40)bpb).VolumeLabel);

                        //If volume label is shorter than 11 bytes, it needs to be padded with spaces
                        if (((BiosParameterBlock40)bpb).VolumeLabel.Length < 11)
                        {
                            for (int i = 0; i < 11 - ((BiosParameterBlock40)bpb).VolumeLabel.Length; i++)
                                writer.Write((byte)0x20);
                        }

                        writer.Write(((BiosParameterBlock40)bpb).FileSystemType);

                        //If volume label is shorter than 8 bytes, it needs to be padded with spaces
                        if (((BiosParameterBlock40)bpb).FileSystemType.Length < 8)
                        {
                            for (int i = 0; i < 8 - ((BiosParameterBlock40)bpb).FileSystemType.Length; i++)
                                writer.Write((byte)0x20);
                        }
                    }
                }

                //Boot signature
                stream.Seek(0x1FE, SeekOrigin.Begin);
                writer.Write((byte)0x55);
                writer.Write((byte)0xAA);

                //Media descriptor needs to be written to each FAT as well
                stream.Seek(fat1_offs, SeekOrigin.Begin);
                writer.Write(bpb.MediaDescriptor);
                writer.Write((byte)0xFF);
                writer.Write((byte)0xFF);
                stream.Seek(fat2_offs, SeekOrigin.Begin);
                writer.Write(bpb.MediaDescriptor);
                writer.Write((byte)0xFF);
                writer.Write((byte)0xFF);

                //Volume label needs to be written to the root directory as well
                stream.Seek(fat2_offs + fat_size, SeekOrigin.Begin);

                //First 11 bytes (8.3 space-padded filename) are the label itself
                writer.Write(((BiosParameterBlock40)bpb).VolumeLabel);
                if (((BiosParameterBlock40)bpb).VolumeLabel.Length < 11)
                {
                    for (int i = 0; i < 11 - ((BiosParameterBlock40)bpb).VolumeLabel.Length; i++)
                        writer.Write((byte)0x20);
                }

                writer.Write((byte)0x08); //Volume label attribute
            }
        }

        //Reads the root directory entries
        public void ReadRootDir(BiosParameterBlock bpb)
        {
            //stream = new MemoryStream(imageBytes, false);
            uint rootDirOffset = (uint)(bpb.BytesPerLogicalSector + (bpb.BytesPerLogicalSector * bpb.LogicalSectorsPerFAT * 2));
            using (var stream = new MemoryStream(imageBytes, writable: false))
            using (var reader = new BinaryReader(stream, Encoding.ASCII))
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
                            ReadSubdir(bpb, entry);
                        }
                        //File entry
                        else if (!Convert.ToBoolean(entry.attribute & 0x10))
                        {
                            main.AddToFileList(entry);
                        }
                    }
                }
            }
        }

        //Reads the subdirectory entries
        public void ReadSubdir(BiosParameterBlock bpb, FatDirEntry parent)
        {
            //stream = new MemoryStream(imageBytes, false);
            uint dataAreaOffset = (uint)(bpb.BytesPerLogicalSector + (bpb.BytesPerLogicalSector * bpb.LogicalSectorsPerFAT * 2) + 
                (bpb.RootDirectoryEntries << 5));
            ushort fat1Offset = bpb.BytesPerLogicalSector;
            using (var stream = new MemoryStream(imageBytes, writable: false))
            using (var reader = new BinaryReader(stream, Encoding.ASCII))
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
                                //Need to find a good way to find the parent node in the treeview and add the child to it
                                string namestring1 = Encoding.ASCII.GetString(entry.filename).TrimEnd(' ');
                                string namestring2 = Encoding.ASCII.GetString(parent.filename).TrimEnd(' ');
                                Console.WriteLine("Found a subdirectory \"" + namestring1 + "\" in parent directory \"" + namestring2 + "\"");
                                //main.AddToDir(parent, entry);
                                ReadSubdir(bpb, entry);
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
    }
}