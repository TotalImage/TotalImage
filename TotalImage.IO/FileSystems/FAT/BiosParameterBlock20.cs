using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 1, Size = 25)]
    public struct BiosParameterBlock20 : IBiosParameterBlock
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

        [FieldOffset(13)]
        private ushort _sectorsPerTrack;

        [FieldOffset(15)]
        private ushort _numberOfHeads;

        [FieldOffset(17)]
        private ushort _hiddenSectorsShort;

        [FieldOffset(17)]
        private uint _hiddenSectorsLong;

        [FieldOffset(21)]
        private uint _totalSectorsLong;

        public BiosParameterBlockVersion BpbVersion
        {
            get => BiosParameterBlockVersion.Dos20;
        }

        public ushort BytesPerLogicalSector
        {
            get => _bytesPerSector;
            set => _bytesPerSector = value;
        }

        public byte LogicalSectorsPerCluster
        {
            get => _sectorsPerCluster;
            set => _sectorsPerCluster = value;
        }

        public ushort ReservedLogicalSectors
        {
            get => _reservedSectors;
            set => _reservedSectors = value;
        }

        public byte NumberOfFATs
        {
            get => _numberOfFATs;
            set => _numberOfFATs = value;
        }

        public ushort RootDirectoryEntries
        {
            get => _rootDirectoryEntries;
            set => _rootDirectoryEntries = value;
        }

        public uint TotalLogicalSectors
        {
            get => _totalSectorsShort == 0 ? _totalSectorsLong : _totalSectorsShort;
            set
            {
                if (value > ushort.MaxValue)
                {
                    _totalSectorsShort = 0;
                    _totalSectorsLong = value;
                }
                else
                {
                    _totalSectorsShort = (ushort)value;
                    _totalSectorsLong = 0;
                }
            }
        }

        public byte MediaDescriptor
        {
            get => _mediaType;
            set => _mediaType = value;
        }

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

        public ushort PhysicalSectorsPerTrack
        {
            get => _sectorsPerTrack;
            set => _sectorsPerTrack = value;
        }

        public ushort NumberOfHeads
        {
            get => _numberOfHeads;
            set => _numberOfHeads = value;
        }

        public uint HiddenSectors
        {
            get => _totalSectorsShort == 0 ? _hiddenSectorsLong : _hiddenSectorsShort;
            set
            {
                if (value > ushort.MaxValue)
                {
                    _hiddenSectorsLong = value;
                }
                else
                {
                    _hiddenSectorsLong = 0;
                    _hiddenSectorsShort = (ushort)value;
                }
            }
        }
    }
}
