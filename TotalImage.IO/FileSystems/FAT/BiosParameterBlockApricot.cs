using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// Represents the BIOS Parameter Block structure for an Apricot disk
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 1, Size = 13)]
    public struct BiosParameterBlockApricot : IBiosParameterBlock
    {
        [FieldOffset(0)]
        private ushort _bytesPerSector;

        [FieldOffset(2)]
        private byte _sectorsPerCluster;

        [FieldOffset(3)]
        private ushort _reservedSectors;

        [FieldOffset(5)]
        private byte _numberOfFATs;

        [FieldOffset(6)]
        private ushort _rootDirectoryEntries;

        [FieldOffset(8)]
        private ushort _totalSectorsShort;

        [FieldOffset(10)]
        private byte _mediaType;

        [FieldOffset(11)]
        private ushort _sectorsPerFATShort;

        /// <inheritdoc />
        public BiosParameterBlockVersion BpbVersion
        {
            get => BiosParameterBlockVersion.Dos20;
        }

        /// <inheritdoc />
        public ushort BytesPerLogicalSector
        {
            get => _bytesPerSector;
            set => _bytesPerSector = value;
        }

        /// <inheritdoc />
        public byte LogicalSectorsPerCluster
        {
            get => _sectorsPerCluster;
            set => _sectorsPerCluster = value;
        }

        /// <inheritdoc />
        public ushort ReservedLogicalSectors
        {
            get => _reservedSectors;
            set => _reservedSectors = value;
        }

        /// <inheritdoc />
        public byte NumberOfFATs
        {
            get => _numberOfFATs;
            set => _numberOfFATs = value;
        }

        /// <inheritdoc />
        public ushort RootDirectoryEntries
        {
            get => _rootDirectoryEntries;
            set => _rootDirectoryEntries = value;
        }

        /// <inheritdoc />
        public uint TotalLogicalSectors
        {
            get => _totalSectorsShort;
            set
            {
                if (value > ushort.MaxValue)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    _totalSectorsShort = (ushort)value;
                }
            }
        }

        /// <inheritdoc />
        public byte MediaDescriptor
        {
            get => _mediaType;
            set => _mediaType = value;
        }

        /// <inheritdoc />
        public uint LogicalSectorsPerFAT
        {
            get => _sectorsPerFATShort;
            set
            {
                if (value > ushort.MaxValue)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _sectorsPerFATShort = (ushort)value;
            }
        }

        /// <inheritdoc />
        public ushort PhysicalSectorsPerTrack
        {
            get
            {
                switch(_mediaType)
                {
                    case 0xFC: // 315k
                        return 70;
                    case 0xFE: // 315k
                        return 80;
                    default:
                        return 0;
                }
            }
            set
            {
                return;
            }
        }

        /// <inheritdoc />
        public ushort NumberOfHeads
        {
            get
            {
                switch (_mediaType)
                {
                    case 0xFC: // 315k
                        return 1;
                    case 0xFE: // 315k
                        return 2;
                    default:
                        return 0;
                }
            }
            set
            {
                return;
            }
        }

        /// <inheritdoc />
        public uint HiddenSectors
        {
            get => 0;
            set
            {
                return;
            }
        }
    }
}
