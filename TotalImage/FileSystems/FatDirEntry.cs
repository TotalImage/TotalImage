using System;

namespace TotalImage.FileSystems
{
    /*
     * This class represents the traditional 32-byte FAT directory entry
     */
    public class FatDirEntry
    {
        public byte[] filename; //8 bytes
        public byte[] extension; //3 bytes
        public byte attribute;
        public byte caseByte;
        public byte creationTimeMs;
        public ushort creationTime;
        public ushort creationDate;
        public ushort lastAccessDate;
        public ushort FAT3232StartCluster; //High word of the FAT32 starting cluster
        public ushort modifiedTime;
        public ushort modifiedDate;
        public ushort startCluster;
        public uint fileSize;

        public FatDirEntry()
        {

        }

        public FatDirEntry(byte[] fn, byte[] ext, byte attr, byte cb, byte ctMs, ushort ct, ushort cd, ushort lad, ushort f32sc, ushort mt, 
            ushort md, ushort sc, uint fs)
        {
            filename = new byte[8];
            Array.Copy(fn, filename, 8);
            extension = new byte[3];
            Array.Copy(ext, extension, 3);
            attribute = attr;
            caseByte = cb;
            creationTimeMs = ctMs;
            creationTime = ct;
            creationDate = cd;
            lastAccessDate = lad;
            FAT3232StartCluster = f32sc;
            modifiedTime = mt;
            modifiedDate = md;
            startCluster = sc;
            fileSize = fs;
        }
    }
}
