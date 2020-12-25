using System;
using System.Collections.Generic;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// This represents a BIOS Parameter Block that holds data discovered through other means and does not represent an on-disk structure.
    /// </summary>
    public class FakeBiosParameterBlock : IBiosParameterBlock
    {
        public BiosParameterBlockVersion BpbVersion { get; set; }

        public ushort BytesPerLogicalSector { get; set; }
        public byte LogicalSectorsPerCluster { get; set; }
        public ushort ReservedLogicalSectors { get; set; }
        public byte NumberOfFATs { get; set; }
        public ushort RootDirectoryEntries { get; set; }
        public uint TotalLogicalSectors { get; set; }
        public byte MediaDescriptor { get; set; }
        public uint LogicalSectorsPerFAT { get; set; }
        public ushort PhysicalSectorsPerTrack { get; set; }
        public ushort NumberOfHeads { get; set; }
        public uint HiddenSectors { get; set; }
    }
}
