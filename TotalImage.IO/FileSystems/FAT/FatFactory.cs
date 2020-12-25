using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using TotalImage.Helpers;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// Tries to read a FAT file system from a stream
    /// </summary>
    public class FatFactory : IFileSystemFactory
    {
        /// <summary>
        /// The boot signature for an Acorn 800k format floppy
        /// </summary>
        private static readonly byte[] _acornBoot = new byte[] { 0xFD, 0xFF, 0xFF };

        /// <summary>
        /// Determine the FAT file system described by a BPB
        /// </summary>
        /// <param name="stream">The stream containing the file system</param>
        /// <param name="bpb">The BIOS Parameter Block structure</param>
        /// <returns>The file system indicated by the BPB, invalid data will return null</returns>
        private static FileSystem GetFatFromBiosParameterBlock(Stream stream, IBiosParameterBlock bpb)
        {
            uint rootDirSectors = (((uint)bpb.RootDirectoryEntries * 32) + ((uint)bpb.BytesPerLogicalSector - 1)) / (uint)bpb.BytesPerLogicalSector;

            uint dataSectors = bpb.TotalLogicalSectors - (bpb.ReservedLogicalSectors + (bpb.NumberOfFATs * bpb.LogicalSectorsPerFAT) + rootDirSectors);
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
            stream.Seek(0, SeekOrigin.Begin);

            BootSector bs = stream.ReadStruct<BootSector>();

            var bpbResult = ParseBpb20(stream, 0x0B);
            if (bpbResult != null)
            {
                return GetFatFromBiosParameterBlock(stream, bpbResult);
            }

            if (CheckForAcorn800k(bs))
            {
                return GetFatFromBiosParameterBlock(stream, new FakeBiosParameterBlock
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
                });
            }

            bpbResult = ParseBpb20(stream, 0x50);
            if (bpbResult != null)
            {
                return GetFatFromBiosParameterBlock(stream, bpbResult);
            }

            bpbResult = GetBpbFromLength(stream.Length);
            if (bpbResult != null)
            {
                return GetFatFromBiosParameterBlock(stream, bpbResult);
            }

            return null;
        }

        /// <summary>
        /// Checks whether the image contains Acorn 800k format, which starts with the first (and only) FAT.
        /// </summary>
        /// <returns>A boolean value indicating whether the image is in the Acord 800k format</returns>
        private bool CheckForAcorn800k(in BootSector bs)
        {
            return Enumerable.SequenceEqual(bs.BootJump, _acornBoot);
        }

        /// <summary>
        /// Read a DOS 2.0 BPB structure from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="offset">The offset that the BPB is located at</param>
        /// <returns>A BPB structure if valid, or null if not</returns>
        private IBiosParameterBlock? ParseBpb20(Stream stream, uint offset)
        {
            stream.Seek(offset, SeekOrigin.Begin); //BPB offset

            IBiosParameterBlock bpb = offset == 0x50
                ? (IBiosParameterBlock)stream.ReadStruct<BiosParameterBlockApricot>()
                : (IBiosParameterBlock)stream.ReadStruct<BiosParameterBlock20>();

            //TODO: These are just some very simple checks to see if the BPB is valid, this should probably be improved upon
            if (bpb.NumberOfHeads == 0 || bpb.PhysicalSectorsPerTrack == 0 || bpb.BytesPerLogicalSector == 0 || bpb.NumberOfFATs == 0 ||
                bpb.TotalLogicalSectors == 0 || bpb.ReservedLogicalSectors == 0 || bpb.LogicalSectorsPerCluster == 0 ||
                bpb.LogicalSectorsPerFAT == 0 || bpb.RootDirectoryEntries == 0)
            {
                Trace.WriteLine("At least one of BPB parameters is 0");
                return null;
            }

            uint tracks = (uint)(bpb.TotalLogicalSectors / bpb.NumberOfHeads / bpb.PhysicalSectorsPerTrack);
            if (tracks == 0 || tracks > 82)
            {
                Trace.WriteLine("BPB paramaters don't match image size");
                return null;
            }

            // Apricot can't contain a 4.0 BPB, so fine to return now.
            if (bpb is BiosParameterBlockApricot)
            {
                return bpb;
            }

            stream.Seek(2, SeekOrigin.Current);
            int versionByte = stream.ReadByte();
            if (versionByte != 40 && versionByte != 41)
            {
                // it's not a DOS 4.0 BPB, don't bother any further
                return bpb;
            }

            stream.Seek(offset, SeekOrigin.Begin);
            bpb = stream.ReadStruct<BiosParameterBlock40>();

            return bpb;
        }

        /// <summary>
        /// Generate a BPB structure from a given image size
        /// </summary>
        /// <param name="length">The length of the stream</param>
        /// <returns>A BPB structure if it can be determined from the given length, or null if not</returns>
        private IBiosParameterBlock? GetBpbFromLength(long length)
        {
            switch (length)
            {
                case 163840: //5.25" 160 KiB
                    return new FakeBiosParameterBlock
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
                        HiddenSectors = 0
                    };
                case 184320: //5.25" 180 KiB
                    return new FakeBiosParameterBlock
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
                        HiddenSectors = 0
                    };
                case 327680: //5.25" 320 KiB
                    return new FakeBiosParameterBlock
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
                    };
                case 368640: //5.25" 360 KiB
                    return new FakeBiosParameterBlock
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
                        HiddenSectors = 0
                    };
                case 256256: //8" 250 KiB
                    return new FakeBiosParameterBlock
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
                        HiddenSectors = 0
                    };
                case 1261568: //8" 1232 KiB
                    return new FakeBiosParameterBlock
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
                        HiddenSectors = 0
                    };
                default:
                    return null;
            }
        }
    }
}
