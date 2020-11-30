using System;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// Distinct BIOS Parameter Block versions.
    /// </summary>
    public enum BiosParameterBlockVersion
    {
        Dos20,
        Dos34,
        Dos40,
        Fat32,
        Ntfs
    }

    /// <summary>
    /// BIOS Parameter Block for FAT12 and FAT16 file systems versions 2.0-3.31.
    /// </summary>
    /// <remarks>
    /// For versions 3.4, 4.0, or HPFS, use BiosParameterBlock40 instead.
    /// </remarks>
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
                if (value?.Length > 8)
                    throw new ArgumentException("OEM ID must be 8 characters at most");

                oemId = value;
            }
        }
        public ushort BytesPerLogicalSector { get; set; }
        public byte LogicalSectorsPerCluster { get; set; }
        public ushort ReservedLogicalSectors { get; set; }
        public byte NumberOfFATs { get; set; }
        public ushort RootDirectoryEntries { get; set; }
        public ushort TotalLogicalSectors { get; set; }
        public byte MediaDescriptor { get; set; }
        public ushort LogicalSectorsPerFAT { get; set; }
        public ushort PhysicalSectorsPerTrack { get; set; }
        public ushort NumberOfHeads { get; set; }
        public uint HiddenSectors { get; set; }
        public uint LargeTotalLogicalSectors { get; set; }
    }
}
