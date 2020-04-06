using System.Runtime.InteropServices;

namespace TotalImage.FileSystems
{
    [StructLayout(LayoutKind.Sequential, Size = 21)]
    public struct Dos32BiosParameterBlock
    {
        public ushort BytesPerLogicalSector;
        public byte LogicalSectorsPerCluster;
        public ushort ReservedLogicalSectors;
        public byte NumberOfFATs;
        public ushort RootDirectoryEntries;
        public ushort TotalLogicalSectors;
        public byte MediaDescriptor;
        public ushort LogicalSectorsPerFAT;
        public ushort PhysicalSectorsPerTrack;
        public ushort NumberOfHeads;
        public ushort HiddenSectors;
        public ushort TotalSectors;
    }
}