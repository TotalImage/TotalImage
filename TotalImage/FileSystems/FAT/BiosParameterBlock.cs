using System;

namespace TotalImage.FileSystems.FAT
{
    public enum BiosParameterBlockVersion
    {
        Dos20,
        Dos33,
        Dos34,
        Dos40,
        Fat32,
        Ntfs
    }

    /*
     * BIOS Parameter Block for FAT12 and FAT16 file systems 
     * Use this class for BPB versions 2.0 and 3.31.
     * 
     * For BPB versions 3.4, 4.0 and HPFS, use BiosParameterBlock40 instead
     */
    public class BiosParameterBlock
    {
        private string oemId;

        public BiosParameterBlockVersion BpbVersion { get; set; }
        public byte[] BootJump { get; set; } = new byte[] { 0xEB, 0x58, 0x90 };
        public string OemId 
        { 
            get => oemId; 
            set
            {
                if(value.Length > 8)
                    throw new ArgumentException("OEM ID must be 8 characters at most");

                oemId = value;
            }
        }
        public ushort BytesPerLogicalSector { get; set; }
        public byte LogicalSectorsPerCluster { get; set; }
        public ushort ReservedLogicalSectors { get; set; }
        public byte NumberOfFATs { get; set; }
        public ushort RootDirectoryEntries { get; set; }
        //public ushort TLS;
        public byte MediaDescriptor { get; set; }
        public ushort LogicalSectorsPerFAT { get; set; }
        public ushort PhysicalSectorsPerTrack { get; set; }
        public ushort NumberOfHeads { get; set; }
        public uint HiddenSectors { get; set; }
        public uint LargeTotalLogicalSectors { get; set; }
    }
}
