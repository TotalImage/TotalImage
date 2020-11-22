using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /*
     * This class represents the traditional 32-byte FAT directory entry, used in FAT12, FAT16 and FAT32
     */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DirectoryEntry
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string name; // “Short” file name limited to 11 characters.
        public byte attr; // File attributes.
        public byte ntRes; // Reserved. Must be set to 0. 
        public byte crtTimeTenth; // Component of the file creation time. Count of tenths of a second. 
        public ushort crtTime; // Creation time. Granularity is 2 seconds. 
        public ushort crtDate; // Creation date.
        public ushort lstAccDate; // Last access date. Last access is defined as a read or write operation performed on the file/directory described by this entry
        public ushort fstClusHI; //High word of first data cluster number for file/directory described by this entry. Only valid for volumes formatted FAT32. Must be set to 0 on volumes formatted FAT12/FAT16.
        public ushort wrtTime; // Last modification (write) time.
        public ushort wrtDate; // Last modification (write) date.
        public ushort fstClusLO; // Low word of first data cluster number for file/directory described by this entry. 
        public uint fileSize; // 32-bit quantity containing size in bytes of file/directory described by this entry. 

        public static DirectoryEntry Parse(byte[] bytes)
        {
            DirectoryEntry entry;

            using (var stream = new MemoryStream(bytes))
            using (var reader = new BinaryReader(stream, Encoding.ASCII))
            {
                entry.name = new string(reader.ReadChars(11));
                entry.attr = reader.ReadByte();
                entry.ntRes = reader.ReadByte();
                entry.crtTimeTenth = reader.ReadByte();
                entry.crtTime = reader.ReadUInt16();
                entry.crtDate = reader.ReadUInt16();
                entry.lstAccDate = reader.ReadUInt16();
                entry.fstClusHI = reader.ReadUInt16();
                entry.wrtTime = reader.ReadUInt16();
                entry.wrtDate = reader.ReadUInt16();
                entry.fstClusLO = reader.ReadUInt16();
                entry.fileSize = reader.ReadUInt32();
            }

            return entry;
        }
    }
}
