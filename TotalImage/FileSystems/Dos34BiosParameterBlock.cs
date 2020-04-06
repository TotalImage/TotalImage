using System.Runtime.InteropServices;

namespace TotalImage.FileSystems
{
    [StructLayout(LayoutKind.Sequential, Size = 32)]
    public struct Dos34BiosParameterBlock
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
        public uint HiddenSectors;
        public uint LargeTotalLogicalSectors;
        public byte PhysicalDriveNumber;
        public byte Flags;
        public byte ExtendedBootSignature;
        public uint VolumeSerialNumber;
    }
}