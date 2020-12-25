using System;
using System.Runtime.InteropServices;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// BIOS Parameter Block version 3.4 for FAT12, FAT16, FAT16B and HPFS file systems.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 1, Size = 32)]
    public struct BiosParameterBlock34 : IBiosParameterBlockExtended
    {
        [FieldOffset(0)]
        private BiosParameterBlock20 _bpb20;

        [FieldOffset(25)]
        private byte _driveNumber;

        [FieldOffset(26)]
        private byte _ntFlags;

        [FieldOffset(27)]
        private byte _extendedBootSignature;

        [FieldOffset(28)]
        private uint _volumeSerial;

        /// <inheritdoc />
        public BiosParameterBlockVersion BpbVersion => BiosParameterBlockVersion.Dos34;

        /// <inheritdoc />
        public ushort BytesPerLogicalSector
        {
            get => _bpb20.BytesPerLogicalSector;
            set => _bpb20.BytesPerLogicalSector = value;
        }

        /// <inheritdoc />
        public byte LogicalSectorsPerCluster
        {
            get => _bpb20.LogicalSectorsPerCluster;
            set => _bpb20.LogicalSectorsPerCluster = value;
        }

        /// <inheritdoc />
        public ushort ReservedLogicalSectors
        {
            get => _bpb20.ReservedLogicalSectors;
            set => _bpb20.ReservedLogicalSectors = value;
        }

        /// <inheritdoc />
        public byte NumberOfFATs
        {
            get => _bpb20.NumberOfFATs;
            set => _bpb20.NumberOfFATs = value;
        }

        /// <inheritdoc />
        public ushort RootDirectoryEntries
        {
            get => _bpb20.RootDirectoryEntries;
            set => _bpb20.RootDirectoryEntries = value;
        }

        /// <inheritdoc />
        public uint TotalLogicalSectors
        {
            get => _bpb20.TotalLogicalSectors;
            set => _bpb20.TotalLogicalSectors = value;
        }

        /// <inheritdoc />
        public byte MediaDescriptor
        {
            get => _bpb20.MediaDescriptor;
            set => _bpb20.MediaDescriptor = value;
        }

        /// <inheritdoc />
        public uint LogicalSectorsPerFAT
        {
            get => _bpb20.LogicalSectorsPerFAT;
            set => _bpb20.LogicalSectorsPerFAT = value;
        }

        /// <inheritdoc />
        public ushort PhysicalSectorsPerTrack
        {
            get => _bpb20.PhysicalSectorsPerTrack;
            set => _bpb20.PhysicalSectorsPerTrack = value;
        }

        /// <inheritdoc />
        public ushort NumberOfHeads
        {
            get => _bpb20.NumberOfHeads;
            set => _bpb20.NumberOfHeads = value;
        }

        /// <inheritdoc />
        public uint HiddenSectors
        {
            get => _bpb20.HiddenSectors;
            set => _bpb20.HiddenSectors = value;
        }

        /// <inheritdoc />
        public byte PhysicalDriveNumber
        {
            get => _driveNumber;
            set => _driveNumber = value;
        }

        /// <inheritdoc />
        public byte Flags
        {
            get => _ntFlags;
            set => _ntFlags = value;
        }

        /// <inheritdoc />
        public uint VolumeSerialNumber
        {
            get => _volumeSerial;
            set => _volumeSerial = value;
        }
        public string VolumeLabel
        {
            get => new string(' ', 11);
            set { return; }
        }

        public string FileSystemType
        {
            get => new string(' ', 8);
            set { return; }
        }
    }
}
