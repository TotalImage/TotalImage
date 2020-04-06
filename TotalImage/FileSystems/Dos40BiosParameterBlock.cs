using System.Runtime.InteropServices;

namespace TotalImage.FileSystems
{
    [StructLayout(LayoutKind.Sequential, Size = 51, CharSet = CharSet.Ansi, Pack = 1)]
    public struct Dos40BiosParameterBlock
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

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string VolumeLabel;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string FileSystemType;
    }
}