namespace TotalImage.FileSystems
{
    /*
     * BIOS Parameter Block for FAT12 and FAT16 file systems 
     * Use this class for BPB versions 2.0, 3.2 and 3.31
     */
    public class BiosParameterBlock
    {
        public byte BpbVersion; //0x20 = 2.0, 0x33 = 3.31, 0x34 = 3.4, 0x40 = 4.0, 0x71 = FAT32 BPB, 0x80 = NTFS BPB
        public ushort BytesPerLogicalSector;
        public byte LogicalSectorsPerCluster;
        public ushort ReservedLogicalSectors;
        public byte NumberOfFATs;
        public ushort RootDirectoryEntries;
        //public ushort TLS;
        public byte MediaDescriptor;
        public ushort LogicalSectorsPerFAT;
        public ushort PhysicalSectorsPerTrack;
        public ushort NumberOfHeads;
        public uint HiddenSectors;
        public uint LargeTotalLogicalSectors;
        public byte[] VolumeLabel;

        public BiosParameterBlock(byte v, ushort bps, byte spc, ushort rs, byte nf, ushort rde, ushort ts, byte md, ushort spf, ushort spt, 
            ushort h, uint hs, uint lts, byte[] vl)
        {
            BpbVersion = v;
            BytesPerLogicalSector = bps;
            ReservedLogicalSectors = rs;
            NumberOfFATs = nf;
            RootDirectoryEntries = rde;
            //TLS = ts;
            MediaDescriptor = md;
            LogicalSectorsPerFAT = spf;
            PhysicalSectorsPerTrack = spt;
            NumberOfHeads = h;
            HiddenSectors = hs;
            LargeTotalLogicalSectors = lts;
            VolumeLabel = vl;
        }

        public BiosParameterBlock()
        {

        }
    }
}
