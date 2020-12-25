using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// BIOS Parameter Block version 4.0 for FAT12, FAT16, FAT16B and HPFS file systems.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 51)]
    public struct BiosParameterBlock40 : IBiosParameterBlockExtended
    {
        private BiosParameterBlock34 _bpb34;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        private char[] _volumeLabel;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        private char[] _fileSystemType;

        /// <inheritdoc />
        public BiosParameterBlockVersion BpbVersion => BiosParameterBlockVersion.Dos40;

        /// <inheritdoc />
        public ushort BytesPerLogicalSector
        {
            get => _bpb34.BytesPerLogicalSector;
            set => _bpb34.BytesPerLogicalSector = value;
        }

        /// <inheritdoc />
        public byte LogicalSectorsPerCluster
        {
            get => _bpb34.LogicalSectorsPerCluster;
            set => _bpb34.LogicalSectorsPerCluster = value;
        }

        /// <inheritdoc />
        public ushort ReservedLogicalSectors
        {
            get => _bpb34.ReservedLogicalSectors;
            set => _bpb34.ReservedLogicalSectors = value;
        }

        /// <inheritdoc />
        public byte NumberOfFATs
        {
            get => _bpb34.NumberOfFATs;
            set => _bpb34.NumberOfFATs = value;
        }

        /// <inheritdoc />
        public ushort RootDirectoryEntries
        {
            get => _bpb34.RootDirectoryEntries;
            set => _bpb34.RootDirectoryEntries = value;
        }

        /// <inheritdoc />
        public uint TotalLogicalSectors
        {
            get => _bpb34.TotalLogicalSectors;
            set => _bpb34.TotalLogicalSectors = value;
        }

        /// <inheritdoc />
        public byte MediaDescriptor
        {
            get => _bpb34.MediaDescriptor;
            set => _bpb34.MediaDescriptor = value;
        }

        /// <inheritdoc />
        public uint LogicalSectorsPerFAT
        {
            get => _bpb34.LogicalSectorsPerFAT;
            set => _bpb34.LogicalSectorsPerFAT = value;
        }

        /// <inheritdoc />
        public ushort PhysicalSectorsPerTrack
        {
            get => _bpb34.PhysicalSectorsPerTrack;
            set => _bpb34.PhysicalSectorsPerTrack = value;
        }

        /// <inheritdoc />
        public ushort NumberOfHeads
        {
            get => _bpb34.NumberOfHeads;
            set => _bpb34.NumberOfHeads = value;
        }

        /// <inheritdoc />
        public uint HiddenSectors
        {
            get => _bpb34.HiddenSectors;
            set => _bpb34.HiddenSectors = value;
        }

        /// <inheritdoc />
        public byte PhysicalDriveNumber
        {
            get => _bpb34.PhysicalDriveNumber;
            set => _bpb34.PhysicalDriveNumber = value;
        }


        /// <inheritdoc />
        public byte Flags
        {
            get => _bpb34.Flags;
            set => _bpb34.Flags = value;
        }

        /// <inheritdoc />
        public uint VolumeSerialNumber
        {
            get => _bpb34.VolumeSerialNumber;
            set => _bpb34.VolumeSerialNumber = value;
        }

        /// <inheritdoc />
        public string VolumeLabel
        {
            get => new string(_volumeLabel);
            set
            {
                if (value.Length > 11)
                {
                    throw new ArgumentException("VolumeLabel must be 11 characters at most", nameof(VolumeLabel));
                }

                _volumeLabel = new char[] { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };

                char[] valueArray = value.ToCharArray();
                Array.Copy(valueArray, _volumeLabel, Math.Min(11, valueArray.Length));
            }
        }

        /// <inheritdoc />
        public string FileSystemType
        {
            get => new string(_fileSystemType);
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("FileSystemType must not be null or empty string");
                }
                else if (value.Length > 8)
                {
                    throw new ArgumentException("FileSystemType must be 8 characters at most", nameof(FileSystemType));
                }

                _fileSystemType = new char[] { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };

                char[] valueArray = value.ToCharArray();
                Array.Copy(valueArray, _fileSystemType, Math.Min(8, valueArray.Length));
            }
        }
    }
}
