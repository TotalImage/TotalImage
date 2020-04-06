using System.Runtime.InteropServices;

namespace TotalImage.FileSystems
{
    [StructLayout(LayoutKind.Explicit, Size = 51, CharSet = CharSet.Ansi)]
    public struct Dos40BiosParameterBlock
    {
        [FieldOffset(0x00)] public ushort BytesPerLogicalSector;
        [FieldOffset(0x02)] public byte LogicalSectorsPerCluster;
        [FieldOffset(0x03)] public ushort ReservedLogicalSectors;
        [FieldOffset(0x05)] public byte NumberOfFATs;
        [FieldOffset(0x06)] public ushort RootDirectoryEntries;
        [FieldOffset(0x08)] public ushort TotalLogicalSectors;
        [FieldOffset(0x0A)] public byte MediaDescriptor;
        [FieldOffset(0x0B)] public ushort LogicalSectorsPerFAT;
        [FieldOffset(0x0D)] public ushort PhysicalSectorsPerTrack;
        [FieldOffset(0x0F)] public ushort NumberOfHeads;
        [FieldOffset(0x11)] public uint HiddenSectors;
        [FieldOffset(0x15)] public uint LargeTotalLogicalSectors;
        [FieldOffset(0x19)] public byte PhysicalDriveNumber;
        [FieldOffset(0x1A)] public byte Flags;
        [FieldOffset(0x1B)] public byte ExtendedBootSignature;
        [FieldOffset(0x1C)] public uint VolumeSerialNumber;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        [FieldOffset(0x20)] public char[] VolumeLabel;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        [FieldOffset(0x2B)] public char[] FileSystemType;
    }
}