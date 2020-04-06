using System.Runtime.InteropServices;

namespace TotalImage.FileSystems
{
    [StructLayout(LayoutKind.Explicit, Size = 19)]
    public struct Dos30BiosParameterBlock
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
        [FieldOffset(0x11)] public ushort HiddenSectors;
    }
}