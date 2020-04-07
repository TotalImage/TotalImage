namespace TotalImage.FileSystems
{
    /*
     * BIOS Parameter Block for FAT12, FAT16, FAT16B and HPFS file systems 
     * Use this class for BPB versions 3.4 and 4.0
     */
    public class BiosParameterBlock40 : BiosParameterBlock
    {
        public byte PhysicalDriveNumber;
        public byte Flags;
        public byte ExtendedBootSignature;
        public uint VolumeSerialNumber;

        public byte[] FileSystemType;

        public BiosParameterBlock40(byte v, ushort bps, byte spc, ushort rs, byte nf, ushort rde, ushort ts, byte md, ushort spf, ushort spt, 
            ushort h, uint hs, uint lts, byte pdn, byte f, byte ebs, uint vsn, byte[] fst, byte[] vl) : base(v, bps, spc, rs, nf, rde, ts, md, 
                spf, spt, h, hs, lts, vl)
        {
            PhysicalDriveNumber = pdn;
            Flags = f;
            ExtendedBootSignature = ebs;
            VolumeSerialNumber = vsn;
            FileSystemType = fst;
        }

        public BiosParameterBlock40() : base()
        {

        }
    }
}
