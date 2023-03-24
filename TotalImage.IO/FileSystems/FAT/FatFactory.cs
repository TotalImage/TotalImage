using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// A factory class that can create a FAT file system
    /// </summary>
    public class FatFactory : IFileSystemFactory
    {
        private static FileSystem? GetFatFromBiosParameterBlock(Stream stream, BiosParameterBlock bpb)
        {
            var fatSectors = bpb.NumberOfFats * bpb.LogicalSectorsPerFat;
            var rootDirectorySectors = bpb.RootDirectoryEntries * 32 / bpb.BytesPerLogicalSector;
            var dataSectors = bpb.TotalLogicalSectors - bpb.ReservedLogicalSectors - fatSectors - rootDirectorySectors;

            var clusterCount = dataSectors / bpb.LogicalSectorsPerCluster;

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
            var bootSectorBytes = new byte[512];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bootSectorBytes);

            BiosParameterBlock? _bpb;
            byte bpbOffset = 0x0B;

            try
            {
                _bpb = BiosParameterBlock.Parse(bootSectorBytes, bpbOffset); //Try to parse the BPB at the standard offset
            }
            catch (InvalidDataException)
            {
                //BPB likely invalid, check if this is an Acorn 800k disk without one
                if (CheckForAcorn800k(bootSectorBytes))
                {
                    return new Fat12FileSystem(stream, BiosParameterBlock.DefaultAcornParameters);
                }
                else
                {
                    //BPB likely invalid, try parsing it at 0x50 in case it's an Apricot disk
                    try
                    {
                        bpbOffset = 0x50;
                        _bpb = BiosParameterBlock.Parse(bootSectorBytes, bpbOffset);
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
        private static bool CheckForAcorn800k(ReadOnlySpan<byte> bytes) =>
            (BinaryPrimitives.ReadUInt32LittleEndian(bytes[0..4]) & 0xFFFFFF) == 0xFFFFFD;
    }
}
