using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace TotalImage.FileSystems.FAT
{
    public class FatFactory : IFileSystemFactory
    {
        private static FileSystem GetFatFromBiosParameterBlock(Stream stream, BiosParameterBlock bpb)
        {
            uint rootDirSectors = (((uint)bpb.RootDirectoryEntries * 32) + ((uint)bpb.BytesPerLogicalSector - 1)) / (uint)bpb.BytesPerLogicalSector;

            uint fatSectors = bpb.LogicalSectorsPerFAT;
            uint totalSectors = bpb.TotalLogicalSectors != 0 ? bpb.TotalLogicalSectors : bpb.LargeTotalLogicalSectors;

            uint dataSectors = totalSectors - (bpb.ReservedLogicalSectors + (bpb.NumberOfFATs * fatSectors) + rootDirSectors);
            uint clusterCount = dataSectors / bpb.LogicalSectorsPerCluster;

            stream.Position = 0;
            if (clusterCount < 4085)
            {
                // return FAT12
                return new Fat12(stream, bpb);
            }
            else if (clusterCount < 65525)
            {
                // return FAT16
                throw new NotSupportedException();
            }
            else
            {
                // return FAT32
                throw new NotSupportedException();
            }
        }

        /// <inheritdoc />
        public FileSystem? TryLoadFileSystem(Stream stream)
        {
            BiosParameterBlock? _bpb = null;
            byte bpbOffset = 0x0B;
            try
            {
                _bpb = Parse(stream, bpbOffset); //Try to parse the BPB at the standard offset
            }
            catch (InvalidDataException)
            {
                //BPB likely invalid, check if this is an Acorn 800k disk without one
                if (CheckForAcorn800k(stream))
                {
                    bpbOffset = 0xFF; //BPB not actually at this offset, this is just to avoid trying to parse it again later
                    _bpb = new BiosParameterBlock
                    {
                        BpbVersion = BiosParameterBlockVersion.Dos20,
                        BytesPerLogicalSector = 1024,
                        LogicalSectorsPerCluster = 1,
                        ReservedLogicalSectors = 0,
                        NumberOfFATs = 1,
                        LogicalSectorsPerFAT = 2,
                        RootDirectoryEntries = 192,
                        PhysicalSectorsPerTrack = 5,
                        NumberOfHeads = 2,
                        TotalLogicalSectors = 800,
                        MediaDescriptor = 0xFD,
                        HiddenSectors = 0,
                        LargeTotalLogicalSectors = 0,
                    };
                    return new Fat12(stream, _bpb);
                }
                else
                {
                    //BPB likely invalid, try parsing it at 0x50 in case it's an Apricot disk
                    try
                    {
                        bpbOffset = 0x50;
                        _bpb = Parse(stream, bpbOffset);
                    }
                    catch (InvalidDataException)
                    {
                        //BPB still invalid, it may not even be there, try to figure out if it's a DOS 1.x disk by looking at file length
                        //(we can do this for raw sector images) and the media descriptor byte
                        switch (stream.Length)
                        {
                            case 163840: //5.25" 160 KiB
                                _bpb = new BiosParameterBlock
                                {
                                    BpbVersion = BiosParameterBlockVersion.Dos20,
                                    BytesPerLogicalSector = 512,
                                    LogicalSectorsPerCluster = 2,
                                    ReservedLogicalSectors = 1,
                                    NumberOfFATs = 2,
                                    LogicalSectorsPerFAT = 1,
                                    RootDirectoryEntries = 64,
                                    PhysicalSectorsPerTrack = 8,
                                    NumberOfHeads = 1,
                                    TotalLogicalSectors = 320,
                                    MediaDescriptor = 0xFE,
                                    HiddenSectors = 0,
                                    LargeTotalLogicalSectors = 0
                                };
                                break;
                            case 184320: //5.25" 180 KiB
                                _bpb = new BiosParameterBlock
                                {
                                    BpbVersion = BiosParameterBlockVersion.Dos20,
                                    BytesPerLogicalSector = 512,
                                    LogicalSectorsPerCluster = 2,
                                    ReservedLogicalSectors = 1,
                                    NumberOfFATs = 2,
                                    LogicalSectorsPerFAT = 1,
                                    RootDirectoryEntries = 64,
                                    PhysicalSectorsPerTrack = 9,
                                    NumberOfHeads = 1,
                                    TotalLogicalSectors = 360,
                                    MediaDescriptor = 0xFC,
                                    HiddenSectors = 0,
                                    LargeTotalLogicalSectors = 0
                                };
                                break;
                            case 327680: //5.25" 320 KiB
                                _bpb = new BiosParameterBlock
                                {
                                    BpbVersion = BiosParameterBlockVersion.Dos20,
                                    BytesPerLogicalSector = 512,
                                    LogicalSectorsPerCluster = 2,
                                    ReservedLogicalSectors = 1,
                                    NumberOfFATs = 2,
                                    LogicalSectorsPerFAT = 1,
                                    RootDirectoryEntries = 112,
                                    PhysicalSectorsPerTrack = 8,
                                    NumberOfHeads = 2,
                                    TotalLogicalSectors = 640,
                                    MediaDescriptor = 0xFF,
                                    HiddenSectors = 0,
                                    LargeTotalLogicalSectors = 0
                                };
                                break;
                            case 368640: //5.25" 360 KiB
                                _bpb = new BiosParameterBlock
                                {
                                    BpbVersion = BiosParameterBlockVersion.Dos20,
                                    BytesPerLogicalSector = 512,
                                    LogicalSectorsPerCluster = 2,
                                    ReservedLogicalSectors = 1,
                                    NumberOfFATs = 2,
                                    LogicalSectorsPerFAT = 2,
                                    RootDirectoryEntries = 112,
                                    PhysicalSectorsPerTrack = 9,
                                    NumberOfHeads = 2,
                                    TotalLogicalSectors = 720,
                                    MediaDescriptor = 0xFD,
                                    HiddenSectors = 0,
                                    LargeTotalLogicalSectors = 0
                                };
                                break;
                            case 256256: //8" 250 KiB
                                _bpb = new BiosParameterBlock
                                {
                                    BpbVersion = BiosParameterBlockVersion.Dos20,
                                    BytesPerLogicalSector = 128,
                                    LogicalSectorsPerCluster = 4,
                                    ReservedLogicalSectors = 1,
                                    NumberOfFATs = 2,
                                    LogicalSectorsPerFAT = 6,
                                    RootDirectoryEntries = 68,
                                    PhysicalSectorsPerTrack = 26,
                                    NumberOfHeads = 1,
                                    TotalLogicalSectors = 2002,
                                    MediaDescriptor = 0xFE,
                                    HiddenSectors = 0,
                                    LargeTotalLogicalSectors = 0
                                };
                                break;
                            case 1261568: //8" 1232 KiB
                                _bpb = new BiosParameterBlock
                                {
                                    BpbVersion = BiosParameterBlockVersion.Dos20,
                                    BytesPerLogicalSector = 1024,
                                    LogicalSectorsPerCluster = 1,
                                    ReservedLogicalSectors = 1,
                                    NumberOfFATs = 2,
                                    LogicalSectorsPerFAT = 2,
                                    RootDirectoryEntries = 192,
                                    PhysicalSectorsPerTrack = 8,
                                    NumberOfHeads = 2,
                                    TotalLogicalSectors = 1232,
                                    MediaDescriptor = 0xFE,
                                    HiddenSectors = 0,
                                    LargeTotalLogicalSectors = 0
                                };
                                break;
                        }
                    }
                }
                //For standard disk types the OEM ID should begin right after the jump instruction, at offset 0x03
                if (bpbOffset == 0x0B)
                {
                    using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
                    {
                        stream.Seek(3, SeekOrigin.Begin);
                        _bpb.OemId = Encoding.ASCII.GetString(reader.ReadBytes(8)).TrimEnd(' ').ToUpper();
                    }
                }
            }

            return new Fat12(stream, _bpb);
        }

        /// <summary>
        /// Checks whether the image contains Acorn 800k format, which starts with the first (and only) FAT.
        /// </summary>
        /// <returns></returns>
        private bool CheckForAcorn800k(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.ASCII, true);

            stream.Seek(0, SeekOrigin.Begin);
            uint threeBytes = reader.ReadUInt32();

            if ((threeBytes & 0xFFFFFF) == 0xFFFFFD) //The starting bytes of the FAT on Acorn 800k disks are 0xFDFFFF
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private BiosParameterBlock Parse(Stream stream, uint offset)
        {
            var bpb = new BiosParameterBlock();
            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                stream.Seek(offset, SeekOrigin.Begin); //BPB offset

                bpb.BytesPerLogicalSector = reader.ReadUInt16();
                bpb.LogicalSectorsPerCluster = reader.ReadByte();
                bpb.ReservedLogicalSectors = reader.ReadUInt16();
                bpb.NumberOfFATs = reader.ReadByte();
                bpb.RootDirectoryEntries = reader.ReadUInt16();
                bpb.TotalLogicalSectors = reader.ReadUInt16();
                bpb.MediaDescriptor = reader.ReadByte();
                bpb.LogicalSectorsPerFAT = reader.ReadUInt16();

                //Parsing a standard BPB
                if (offset == 0x0B)
                {
                    bpb.PhysicalSectorsPerTrack = reader.ReadUInt16();
                    bpb.NumberOfHeads = reader.ReadUInt16();
                    bpb.HiddenSectors = reader.ReadUInt32();
                    bpb.LargeTotalLogicalSectors = reader.ReadUInt32();
                }
                //Parsing an Apricot BPB, which doesn't have the number of heads and SPT, so we have to make some manual adjustments
                else if (offset == 0x50)
                {
                    if (bpb.MediaDescriptor == 0xFC) //315k
                    {
                        bpb.NumberOfHeads = 1;
                        bpb.PhysicalSectorsPerTrack = 70;
                    }
                    else if (bpb.MediaDescriptor == 0xFE) //720k
                    {
                        bpb.NumberOfHeads = 2;
                        bpb.PhysicalSectorsPerTrack = 80;
                    }
                }

                //TODO: These are just some very simple checks to see if the BPB is valid, this should probably be improved upon
                if (bpb.NumberOfHeads == 0 || bpb.PhysicalSectorsPerTrack == 0 || bpb.BytesPerLogicalSector == 0 || bpb.NumberOfFATs == 0 ||
                    bpb.TotalLogicalSectors == 0 || bpb.ReservedLogicalSectors == 0 || bpb.LogicalSectorsPerCluster == 0 ||
                    bpb.LogicalSectorsPerFAT == 0 || bpb.RootDirectoryEntries == 0)
                {
                    throw new InvalidDataException("At least one of BPB parameters is 0");
                }

                uint tracks = (uint)(bpb.TotalLogicalSectors / bpb.NumberOfHeads / bpb.PhysicalSectorsPerTrack);

                if (tracks == 0 || tracks > 82)
                {
                    throw new InvalidDataException("BPB paramaters don't match image size");
                }

                //So far, the BPB seems to be OK, so try to read it further as a DOS 4.0 BPB.
                var bpb40 = new BiosParameterBlock40(bpb);
                bpb40.PhysicalDriveNumber = reader.ReadByte();
                bpb40.Flags = reader.ReadByte();

                switch (reader.ReadByte())
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

                if (bpb40.BpbVersion == BiosParameterBlockVersion.Dos40)
                {
                    bpb40.VolumeLabel = new string(reader.ReadChars(11));
                    bpb40.FileSystemType = new string(reader.ReadChars(8));
                }

                return bpb40;
            }
        }
    }
}
