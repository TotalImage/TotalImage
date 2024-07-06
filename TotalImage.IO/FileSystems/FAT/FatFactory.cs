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
                // return FAT16
                return new Fat16FileSystem(stream, bpb);
            }
            else
            {
                // return FAT32
                return new Fat32FileSystem(stream, bpb);
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
                //Try Victor 9000 instead, which also has no BPB
                else if (CheckForVictor9k(reader) == 1 && stream.Length == 626688)
                {
                    return new Fat12FileSystem(stream, BiosParameterBlock.DefaultVictorSSParameters); //Single-sided
                }
                else if (CheckForVictor9k(reader) == 2 && stream.Length == 1224192)
                {
                    return new Fat12FileSystem(stream, BiosParameterBlock.DefaultVictorDSParameters); //Double-sided
                }
                else
                {
                    //Try parsing it at 0x50 in case it's an Apricot disk
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

        /// <summary>
        /// Checks whether the image contains Victor 9000 single-sided format, which does not have a BPB and needs special treatment.
        /// </summary>
        /// <returns>0 if the disk is not a Victor disk, 1 if it's a single-sided Victor disk, 2 if it's a double-sided Victor disk</returns>
        private static byte CheckForVictor9k(BinaryReader reader)
        {
            //First look for the first FAT with the 0xF8 media descriptor byte
            reader.BaseStream.Seek(0x200, SeekOrigin.Begin);
            if ((reader.ReadUInt32() & 0xFFFFFF) == 0xFFFFF8)
            {
                //First FAT matches, now look for the second FAT to determine if this is single-sided or double-sided
                reader.BaseStream.Seek(0x400, SeekOrigin.Begin);
                uint bytes2 = reader.ReadUInt32() & 0xFFFFFF;
                if (bytes2 == 0xFFFFF8)
                    return 1; //This could be a single-sided Victor 9000 disk
                else
                {
                    reader.BaseStream.Seek(0x600, SeekOrigin.Begin);
                    bytes2 = reader.ReadUInt32() & 0xFFFFFF;
                    if (bytes2 == 0xFFFFF8)
                        return 2; //This could be a double-sided Victor 9000 disk
                }
            }
            return 0; //FATs not found or do not match, so probably not a Victor 9000 disk
        }
    }
}
