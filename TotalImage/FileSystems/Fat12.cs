using System;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems
{
    public class Fat12
    {
        public void Parse(byte[] imageBytes)
        {
            var bpb = new Dos40BiosParameterBlock();

            using(var stream = new MemoryStream(imageBytes, writable = false))
            using(var reader = new BinaryReader(stream, Encoding.ASCII))
            {
                stream.Seek(0x0B, SeekOrigin.Begin); // BPB offset

                bpb.BytesPerLogicalSector =     reader.ReadUInt16();
                bpb.LogicalSectorsPerCluster =  reader.ReadByte();
                bpb.ReservedLogicalSectors =    reader.ReadUInt16();
                bpb.NumberOfFATs =              reader.ReadByte();
                bpb.RootDirectoryEntries =      reader.ReadUInt16();
                bpb.TotalLogicalSectors =       reader.ReadUInt16();
                bpb.MediaDescriptor =           reader.ReadByte();
                bpb.LogicalSectorsPerFAT =      reader.ReadUInt16();
                bpb.NumberOfHeads =             reader.ReadUInt16();
                bpb.HiddenSectors =             reader.ReadUInt32();
                bpb.LargeTotalLogicalSectors =  reader.ReadUInt32();
                bpb.PhysicalDriveNumber =       reader.ReadByte();
                bpb.Flags =                     reader.ReadByte();
                bpb.ExtendedBootSignature =     reader.ReadByte();
                bpb.VolumeSerialNumber =        reader.ReadUInt32();
                bpb.VolumeLabel =               reader.ReadChars(11);
                bpb.FileSystemType =            reader.ReadChars(8);
            }
        }
    }
}