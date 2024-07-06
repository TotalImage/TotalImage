using System;
using System.Globalization;
using System.IO;
using System.Text;
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
    public class ExtendedBiosParameterBlock : BiosParameterBlock
    {
        private byte drvNum;
        private byte reserved1;
        private ExtendedBootSignature bootSig;
        private uint volId;
        private byte[] volLab = new byte[11];
        private byte[] filSysType = new byte[8];

        /// <summary>
        /// The version of this Extended BIOS Parameter Block, either 3.4 or 4.0.
        /// </summary>
        public override BiosParameterBlockVersion Version
            => ExtendedBootSignature switch
            {
                ExtendedBootSignature.Dos34 => BiosParameterBlockVersion.Dos34,
                ExtendedBootSignature.Dos40 => BiosParameterBlockVersion.Dos40,
                _ => throw new InvalidDataException()
            };

        /// <summary>
        /// The drive number used by the BIOS in the boot process.
        /// </summary>
        public byte PhysicalDriveNumber { get => drvNum; private set => drvNum = value; }

        public byte Flags { get => reserved1; private set => reserved1 = value; }

        /// <summary>
        /// The byte which can be used to distinguish between versions 3.4 and 4.0 of Extended BIOS Parameter Block.
        /// </summary>
        public ExtendedBootSignature ExtendedBootSignature { get => bootSig; private set => bootSig = value; }

        /// <summary>
        /// The serial number that uniquely identifies this volume.
        /// </summary>
        public uint? VolumeSerialNumber
        {
            get => ExtendedBootSignature == ExtendedBootSignature.Dos40 || ExtendedBootSignature == ExtendedBootSignature.Dos34 ? (uint?)volId : null;
            private set
            {
                if ((ExtendedBootSignature == ExtendedBootSignature.Dos40 || ExtendedBootSignature == ExtendedBootSignature.Dos34) && value.HasValue)
                    volId = value.Value;
                else throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// A (usually) more descriptive way to identify the disk in a set.
        /// </summary>
        public string? VolumeLabel
        {
            get => ExtendedBootSignature == ExtendedBootSignature.Dos40 ? Encoding.ASCII.GetString(volLab) : null;
            private set
            {
                if (ExtendedBootSignature == ExtendedBootSignature.Dos40 && value != null)
                {
                    if (value.Length > 11) throw new ArgumentException();
                    volLab = Encoding.ASCII.GetBytes(value.PadRight(11));
                }
                else throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The file system type of this volume.
        /// </summary>
        /// <remarks>
        /// In practice this value can be unreliable and should not be used to identify the file system type.
        /// </remarks>
        public string? FileSystemType
        {
            get => ExtendedBootSignature == ExtendedBootSignature.Dos40 ? Encoding.ASCII.GetString(filSysType) : null;
            private set
            {
                if (ExtendedBootSignature == ExtendedBootSignature.Dos40 && value != null)
                {
                    if (value.Length > 11) throw new ArgumentException();
                    filSysType = Encoding.ASCII.GetBytes(value.PadRight(8));
                }
                else throw new InvalidOperationException();
            }
        }

        private ExtendedBiosParameterBlock() : base() { }

        /// <summary>
        /// Creates a new Extended BIOS Parameter Block from the provided BIOS Parameter Block.
        /// </summary>
        /// <param name="bpb">The BIOS Parameter Block to turn into an Extended BIOS Parameter Block</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ExtendedBiosParameterBlock(BiosParameterBlock bpb) : base(bpb)
        {
            if (bpb == null)
                throw new ArgumentNullException(nameof(bpb), "bpb cannot be null!");
        }

        /// <summary>
        /// Creates an Extended BIOS Parameter Block from the provided floppy geometry and other parameters.
        /// </summary>
        /// <param name="geometry">The floppy geometry to use</param>
        /// <param name="version">Version of the BIOS Parameter Block</param>
        /// <param name="oemId">OEM identifier</param>
        /// <param name="serialNumber">Volume serial number</param>
        /// <param name="fileSystemType">File system type</param>
        /// <param name="volumeLabel">Volume label</param>
        /// <returns>An Extended BIOS Parameter Block</returns>
        public static ExtendedBiosParameterBlock FromGeometry(FloppyGeometry geometry, BiosParameterBlockVersion version, string oemId, string serialNumber, string fileSystemType, string volumeLabel)
        {
            var bpb = new ExtendedBiosParameterBlock(FromGeometry(geometry, version, oemId))
            {
                PhysicalDriveNumber = 0,
                Flags = 0,
            };

            if (version == BiosParameterBlockVersion.Dos40)
            {
                bpb.ExtendedBootSignature = ExtendedBootSignature.Dos40;
                bpb.VolumeSerialNumber = uint.Parse(serialNumber, NumberStyles.HexNumber);
                bpb.FileSystemType = Helper.UseAsLabel(fileSystemType);
                bpb.VolumeLabel = Helper.UseAsLabel(volumeLabel, 11);
            }
            else if (version == BiosParameterBlockVersion.Dos34)
            {
                bpb.ExtendedBootSignature = ExtendedBootSignature.Dos34;
                bpb.VolumeSerialNumber = uint.Parse(serialNumber, NumberStyles.HexNumber);
            }

            return bpb;
        }

        public static ExtendedBiosParameterBlock? ContinueParsing(BiosParameterBlock bpb, BinaryReader reader)
        {
            var ebpb = new ExtendedBiosParameterBlock(bpb);
            return ebpb.ReadEbpbFields(reader) ? ebpb : null;
        }

        /// <summary>
        /// Reads the EBP-specific fields using the provided BinaryReader.
        /// </summary>
        /// <param name="reader">The BinaryReader to use for readig</param>
        /// <returns></returns>
        protected bool ReadEbpbFields(BinaryReader reader)
        {
            drvNum = reader.ReadByte();
            reserved1 = reader.ReadByte();
            bootSig = (ExtendedBootSignature)reader.ReadByte();

            if (bootSig == ExtendedBootSignature.Dos40)
            {
                volId = reader.ReadUInt32();
                volLab = reader.ReadBytes(11);
                filSysType = reader.ReadBytes(8);
                return true;
            }
            else if (bootSig == ExtendedBootSignature.Dos34)
            {
                // This is a shorter EBPB format used by PC-DOS 3.4 and
                // some early OS/2 versions.
                volId = reader.ReadUInt32();
                return true;
            }
            else
            {
                // Unknown extended boot signature, retreat
                return false;
            }
        } 
    }
}
