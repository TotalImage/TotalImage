using System.IO;
using System.Runtime.InteropServices;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// File System Information Sector introduced in FAT32 to improve performance.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FsInfo
    {
        /// <summary>
        /// The first FS Info sector signature (0x52 0x52 0x61 0x41 = "RRaA").
        /// </summary>
        public uint leadSig;

        /// <summary>
        /// 480 reserved bytes following the first signature.
        /// </summary>
        /// <remarks>
        /// Must be set to 0 during format and never used afterwards.
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 480)]
        public byte[] reserved1;

        /// <summary>
        /// The second FS Info sector signature (0x72 0x72 0x41 0x61 = "rrAa").
        /// </summary>
        public uint strucSig;

        /// <summary>
        /// Last known number of free data clusters in the volume, 0xFFFFFFFF if unknown (e.g. after formatting).
        /// </summary>
        /// <remarks>
        /// May not always be relied on and should be sanity checked before every use.
        /// </remarks>
        public uint freeCount;

        /// <summary>
        /// The cluster number at which the driver should start looking for free clusters, 0xFFFFFFFF if unknown (e.g. after formatting). 
        /// </summary>
        /// <remarks>
        /// Typically set to the last allocated cluster. May not always be relied on and should be sanity checked before every use.
        /// </remarks>
        public uint nxtFree;

        /// <summary>
        /// 12 reserved bytes before the third signature.
        /// </summary>
        /// <remarks>
        /// Must be set to 0 during format and never used afterwards.
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] reserved2;

        /// <summary>
        /// The third FS Info sector signature (0x00 0x00 0x55 0xAA).
        /// </summary>
        public uint trailSig;

        /// <summary>
        /// Parses the FS Info sector using the provided BinaryReader.
        /// </summary>
        /// <param name="reader">The BinaryReader to use for parsing</param>
        /// <returns>FS Info sector</returns>
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

        /// <summary>
        /// Performs a simple check on all three FS Info sector signatures to determine if it's valid.
        /// </summary>
        public bool IsValid => leadSig == 0x41615252 && strucSig == 0x61417272 && trailSig == 0xAA550000;
    }
}
