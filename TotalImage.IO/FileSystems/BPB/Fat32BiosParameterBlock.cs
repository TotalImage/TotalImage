using System.IO;
using System.Runtime.InteropServices;

namespace TotalImage.FileSystems.BPB
{
    public class Fat32BiosParameterBlock : ExtendedBiosParameterBlock
    {
        public override BiosParameterBlockVersion Version => BiosParameterBlockVersion.Fat32;

        public ushort ExtFlags { get; private set; }

        public ushort FileSystemVersion { get; private set; }

        public uint RootDirectoryCluster { get; private set; }

        public ushort FsInfo { get; private set; }

        public ushort BackupBootSector { get; private set; }

        public byte[] Reserved { get; private set; } = new byte[11];

        public Fat32BiosParameterBlock(BiosParameterBlock bpb) : base(bpb)
        {
            
        }

        public new static Fat32BiosParameterBlock? ContinueParsing(BiosParameterBlock bpb, BinaryReader reader)
        {
            var fat32bpb = new Fat32BiosParameterBlock(bpb);
            fat32bpb.ReadFat32BpbFields(reader);
            return fat32bpb.ReadEbpbFields(reader) ? fat32bpb : null;
        }

        private void ReadFat32BpbFields(BinaryReader reader)
        {
            LogicalSectorsPerFAT = reader.ReadUInt32();
            ExtFlags = reader.ReadUInt16();
            FileSystemVersion = reader.ReadUInt16();
            RootDirectoryCluster = reader.ReadUInt32();
            FsInfo = reader.ReadUInt16();
            BackupBootSector = reader.ReadUInt16();
            Reserved = reader.ReadBytes(12);
        }
    }
}