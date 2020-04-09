using System;

namespace TotalImage.FileSystems
{
    /*
     * BIOS Parameter Block for FAT12, FAT16, FAT16B and HPFS file systems 
     * Use this class for BPB versions 3.4 and 4.0
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
                if(value.Length > 11)
                    throw new ArgumentException("VolumeLabel must be 11 characters at most");

                volumeLabel = value;
            }
        }
        public string FileSystemType
        {
            get => fileSystemType;
            set
            {
                if(value.Length > 8)
                    throw new ArgumentException("FileSystemType must be 8 characters at most");

                fileSystemType = value;
            }
        }
    }
}
