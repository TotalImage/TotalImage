using System.Runtime.InteropServices;

namespace TotalImage.FileSystems.BPB
{
    public class Fat32BiosParameterBlock : ExtendedBiosParameterBlock
    {
        public ushort ExtFlags { get; set; }

        public ushort FileSystemVersion { get; set; }

        public uint RootDirectoryCluster { get; set; }

        public uint FsInfo { get; set; }

        public uint BackupBootSector { get; set; }

        public byte[] Reserved { get; set; } = new byte[11];

        public Fat32BiosParameterBlock(BiosParameterBlock bpb) : base(bpb)
        {
            
        }
    }
}