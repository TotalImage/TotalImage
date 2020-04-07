using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TotalImage.FileSystems
{
    public class Fat12
    {
        public BiosParameterBlock Parse(byte[] imageBytes)
        {
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
                /*bpb.TotalLogicalSectors =*/ reader.ReadUInt16();
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
        public void Format(byte[] imageBytes, BiosParameterBlock bpb, string oemID, byte tracks)
        {
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
        public void ReadRootDir(byte[] imageBytes)
        {
            frmMain main = (frmMain)Application.OpenForms["frmMain"];
            BiosParameterBlock bpb = Parse(imageBytes);
            uint rootDirOffset = (uint)(bpb.BytesPerLogicalSector + (bpb.BytesPerLogicalSector * bpb.LogicalSectorsPerFAT * 2));
            using (var stream = new MemoryStream(imageBytes, writable: false))
            using (var reader = new BinaryReader(stream, Encoding.ASCII))
            {
                stream.Seek(rootDirOffset, SeekOrigin.Begin); // BPB offset

                //Read all root entries and add them to the appropriate control
                for (int i = 0; i < bpb.RootDirectoryEntries; i++)
                {
                    Console.WriteLine("Entry " + i + " at " + (rootDirOffset + (i * 0x20)));
                    stream.Seek(rootDirOffset + i * 0x20, SeekOrigin.Begin);
                    byte[] filename = reader.ReadBytes(8);
                    stream.Seek(0x03, SeekOrigin.Current);
                    byte attribute = reader.ReadByte();

                    Console.WriteLine("Filename: " + Encoding.ASCII.GetString(filename));
                    Console.WriteLine("Attribute: " + attribute);

                    if(filename[0] != 0xE5 && filename[0] != 0x00)
                    {
                        if (Convert.ToBoolean(attribute & 0x10))
                        {
                            main.lstDirectories.Nodes[0].Nodes.Add(Encoding.ASCII.GetString(filename));
                        }
                        main.lstFiles.Items.Add(Encoding.ASCII.GetString(filename));
                    }
                    
                }

                main.lstDirectories.Sort();
            }
        }
    }
}