namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// BIOS Parameter Block for FAT12 and FAT16 file systems versions 2.0-3.31.
    /// </summary>
    public interface IBiosParameterBlock
    {
        BiosParameterBlockVersion BpbVersion { get; }

        ushort BytesPerLogicalSector { get; set; }

        byte LogicalSectorsPerCluster { get; set; }

        ushort ReservedLogicalSectors { get; set; }

        byte NumberOfFATs { get; set; }

        ushort RootDirectoryEntries { get; set; }

        uint TotalLogicalSectors { get; set; }

        byte MediaDescriptor { get; set; }

        uint LogicalSectorsPerFAT { get; set; }

        ushort PhysicalSectorsPerTrack { get; set; }

        ushort NumberOfHeads { get; set; }

        uint HiddenSectors { get; set; }
    }
}
