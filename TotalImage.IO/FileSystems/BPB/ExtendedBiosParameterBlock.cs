using System;
using System.Globalization;
using TotalImage.DiskGeometries;
using TotalImage.FileSystems.FAT;

namespace TotalImage.FileSystems.BPB
{
    /// <summary>
    /// BIOS Parameter Block versions 3.4 and 4.0 for FAT12, FAT16, FAT16B and HPFS file systems.
    /// </summary>
    /// <remarks>
    /// For versions 2.0-3.31 for FAT12 and FAT16 file systems, use BiosParameterBlock instead.
    /// </remarks>
    public class BiosParameterBlock40 : BiosParameterBlock
    {
        private string volumeLabel, fileSystemType;

        public byte PhysicalDriveNumber { get; set; }
        public byte Flags { get; set; }
        public ExtendedBootSignature ExtendedBootSignature { get; set; }
        public uint VolumeSerialNumber { get; set; }
        public string VolumeLabel
        {
            get => volumeLabel;
            set
            {
                if (value.Length > 11)
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
                else if (value.Length > 8)
                    throw new ArgumentException("FileSystemType must be 8 characters at most");

                fileSystemType = value;
            }
        }

        private BiosParameterBlock40() : base()
        {
            volumeLabel = "";
            fileSystemType = "";
        }

        public BiosParameterBlock40(BiosParameterBlock bpb) : base(bpb)
        {
            if (bpb == null)
                throw new ArgumentNullException(nameof(bpb), "bpb cannot be null!");

            volumeLabel = "";
            fileSystemType = "";
        }

        public static BiosParameterBlock40 FromGeometry(FloppyGeometry geometry, BiosParameterBlockVersion version, string oemId, string serialNumber, string fileSystemType, string volumeLabel)
        {
            var bpb = new BiosParameterBlock40(FromGeometry(geometry, version, oemId))
            {
                PhysicalDriveNumber = 0,
                Flags = 0,
                VolumeSerialNumber = uint.Parse(serialNumber, NumberStyles.HexNumber)
            };

            if (bpb.Version == BiosParameterBlockVersion.Dos40)
            {
                bpb.FileSystemType = Helper.UseAsLabel(fileSystemType);
                bpb.VolumeLabel = Helper.UseAsLabel(volumeLabel, 11);
            }

            return bpb;
        }
    }
}
