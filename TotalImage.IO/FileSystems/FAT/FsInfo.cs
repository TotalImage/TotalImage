using System.IO;
using System.Runtime.InteropServices;

namespace TotalImage.FileSystems.FAT
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FsInfo
    {
        public uint leadSig;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 480)]
        public byte[] reserved1;

        public uint strucSig;

        public uint freeCount;

        public uint nxtFree;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] reserved2;

        public uint trailSig;

        public static FsInfo Parse(BinaryReader reader)
            => new FsInfo()
            {
                leadSig = reader.ReadUInt32(),
                reserved1 = reader.ReadBytes(480),
                strucSig = reader.ReadUInt32(),
                freeCount = reader.ReadUInt32(),
                nxtFree = reader.ReadUInt32(),
                reserved2 = reader.ReadBytes(12),
                trailSig = reader.ReadUInt32()
            };

        public bool IsValid => leadSig == 0x41615252 && strucSig == 0x61417272 && trailSig == 0xAA550000;
    }
}