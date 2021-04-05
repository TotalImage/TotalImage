using System;
using System.IO;
using System.Text;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// A factory class that can create a FAT file system
    /// </summary>
    public class FatFactory : IFileSystemFactory
    {
        private static FileSystem? GetFatFromBiosParameterBlock(Stream stream, BiosParameterBlock bpb)
        {
            uint rootDirSectors = (((uint)bpb.RootDirectoryEntries * 32) + ((uint)bpb.BytesPerLogicalSector - 1)) / (uint)bpb.BytesPerLogicalSector;

            uint fatSectors = bpb.LogicalSectorsPerFAT;
            uint totalSectors = bpb.TotalLogicalSectors;

            uint dataSectors = totalSectors - (bpb.ReservedLogicalSectors + (bpb.NumberOfFATs * fatSectors) + rootDirSectors);
            uint clusterCount = dataSectors / bpb.LogicalSectorsPerCluster;

            stream.Position = 0;
            if (clusterCount < 4085)
            {
                // return FAT12
                return new Fat12FileSystem(stream, bpb);
            }
            else if (clusterCount < 65525)
            {
                // return FAT16 - NOT SUPPORTED YET
                return null;
            }
            else
            {
                // return FAT32 - NOT SUPPORTED YET
                return null;
            }
        }

        /// <inheritdoc />
        public FileSystem? TryLoadFileSystem(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.ASCII, true);

            BiosParameterBlock? _bpb;
            byte bpbOffset = 0x0B;

            try
            {
                _bpb = BiosParameterBlock.Parse(reader, bpbOffset); //Try to parse the BPB at the standard offset
            }
            catch (InvalidDataException)
            {
                //BPB likely invalid, check if this is an Acorn 800k disk without one
                if (CheckForAcorn800k(reader))
                {
                    return new Fat12FileSystem(stream, BiosParameterBlock.DefaultAcornParameters);
                }
                else
                {
                    //BPB likely invalid, try parsing it at 0x50 in case it's an Apricot disk
                    try
                    {
                        bpbOffset = 0x50;
                        _bpb = BiosParameterBlock.Parse(reader, bpbOffset);
                    }
                    catch (InvalidDataException)
                    {
                        //BPB still invalid, it may not even be there, try to figure out if it's a DOS 1.x disk by looking at file length
                        //(we can do this for raw sector images) and the media descriptor byte
                        if (!BiosParameterBlock.DefaultParametersForSize.TryGetValue(stream.Length, out _bpb))
                        {
                            return null;
                        }
                    }
                }
            }

            return GetFatFromBiosParameterBlock(stream, _bpb);
        }

        /// <summary>
        /// Checks whether the image contains Acorn 800k format, which starts with the first (and only) FAT.
        /// </summary>
        /// <returns></returns>
        private static bool CheckForAcorn800k(BinaryReader reader)
        {
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            return (reader.ReadUInt32() & 0xFFFFFF) == 0xFFFFFD;
        }
    }
}
