using System.Runtime.InteropServices;

namespace TotalImage.FileSystems
{
    [StructLayout(LayoutKind.Sequential, Size = 13)]
    public struct Dos20BiosParameterBlock
    {
        public ushort BytesPerLogicalSector;
        public byte LogicalSectorsPerCluster;
        public ushort ReservedLogicalSectors;
        public byte NumberOfFATs;
        public ushort RootDirectoryEntries;
        public ushort TotalLogicalSectors;
        public byte MediaDescriptor;
        public ushort LogicalSectorsPerFAT;
    }
}