using System;

namespace TotalImage.FileSystems.FAT
{
    /*
     * BIOS Parameter Block for FAT12, FAT16, FAT16B and HPFS file systems 
     * Use this class for BPB versions 3.4 and 4.0
     * 
     * For BPB versions 2.0 and 3.31 for FAT12 and FAT16, use BiosParameterBlock instead
     */
    public class BiosParameterBlock40 : BiosParameterBlock
    {
        private string volumeLabel, fileSystemType;

        public byte PhysicalDriveNumber { get; set; }
        public byte Flags { get; set; }
        //public byte ExtendedBootSignature { get; set; } <-- this can be deducted from the BpbVersion field - commenting out for now.
        public uint VolumeSerialNumber { get; set; }
        public string VolumeLabel
        {
            get => volumeLabel; 
            set
            {
                if(string.IsNullOrEmpty(value))
                    throw new ArgumentException("VolumeLabel must not be null or empty string");
                else if(value.Length > 11)
                    throw new ArgumentException("VolumeLabel must be 11 characters at most");

                volumeLabel = value;
            }
        }
        public string FileSystemType
        {
            get => fileSystemType;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("FileSystemType must not be null or empty string");
                else if(value.Length > 8)
                    throw new ArgumentException("FileSystemType must be 8 characters at most");

                fileSystemType = value;
            }
        }

        public BiosParameterBlock40() { }

        public BiosParameterBlock40(BiosParameterBlock bpb)
        {
            if (bpb == null)
                throw new ArgumentNullException(nameof(bpb), "bpb cannot be null!");

            BpbVersion = bpb.BpbVersion;
            BootJump = bpb.BootJump;
            OemId = bpb.OemId;
            BytesPerLogicalSector = bpb.BytesPerLogicalSector;
            LogicalSectorsPerCluster = bpb.LogicalSectorsPerCluster;
            ReservedLogicalSectors = bpb.ReservedLogicalSectors;
            NumberOfFATs = bpb.NumberOfFATs;
            RootDirectoryEntries = bpb.RootDirectoryEntries;
            //TLS = bpb.TLS;
            MediaDescriptor = bpb.MediaDescriptor;
            LogicalSectorsPerFAT = bpb.LogicalSectorsPerFAT;
            PhysicalSectorsPerTrack = bpb.PhysicalSectorsPerTrack;
            NumberOfHeads = bpb.NumberOfHeads;
            HiddenSectors = bpb.HiddenSectors;
            LargeTotalLogicalSectors = bpb.LargeTotalLogicalSectors;
        }
    }
}
