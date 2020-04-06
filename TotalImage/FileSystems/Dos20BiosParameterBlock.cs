using System.Runtime.InteropServices;

namespace TotalImage.FileSystems
{
    [StructLayout(LayoutKind.Explicit, Size = 13)]
    public struct Dos20BiosParameterBlock
    {
        [FieldOffset(0x00)] public ushort BytesPerLogicalSector;
        [FieldOffset(0x02)] public byte LogicalSectorsPerCluster;
        [FieldOffset(0x03)] public ushort ReservedLogicalSectors;
        [FieldOffset(0x05)] public byte NumberOfFATs;
        [FieldOffset(0x06)] public ushort RootDirectoryEntries;
        [FieldOffset(0x08)] public ushort TotalLogicalSectors;
        [FieldOffset(0x0A)] public byte MediaDescriptor;
        [FieldOffset(0x0B)] public ushort LogicalSectorsPerFAT;
    }
}