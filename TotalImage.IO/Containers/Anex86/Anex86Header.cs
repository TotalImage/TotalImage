using System;
using System.Buffers.Binary;

namespace TotalImage.Containers.Anex86
{
    /// <summary>
    /// A class representing the header structure of the FDI/HDI image
    /// </summary>
    public class Anex86Header
    {
        /// <summary>
        /// Describes the disk format contained in the image.
        /// </summary>
        public int DiskType { get; }

        /// <summary>
        /// Size of header in bytes.
        /// </summary>
        public int HeaderSize { get; }

        /// <summary>
        /// Size of disk's data area in bytes.
        /// </summary>
        public int DataSize { get; }

        /// <summary>
        /// Size of each sector.
        /// </summary>
        public int SectorSize { get; }

        /// <summary>
        /// Sector count.
        /// </summary>
        public int Sectors { get; }

        /// <summary>
        /// Head count.
        /// </summary>
        public int Heads { get; }

        /// <summary>
        /// Cylinder/track count.
        /// </summary>
        public int Cylinders { get; }

        /// <summary>
        /// Read an FDI/HDI header from a span of bytes
        /// </summary>
        /// <param name="bytes">The span of bytes containing the header</param>
        public Anex86Header(ReadOnlySpan<byte> bytes)
        {
            DiskType = BinaryPrimitives.ReadInt32LittleEndian(bytes[4..8]);
            HeaderSize = BinaryPrimitives.ReadInt32LittleEndian(bytes[8..12]);
            DataSize = BinaryPrimitives.ReadInt32LittleEndian(bytes[12..16]);
            SectorSize = BinaryPrimitives.ReadInt32LittleEndian(bytes[16..20]);
            Sectors = BinaryPrimitives.ReadInt32LittleEndian(bytes[20..24]);
            Heads = BinaryPrimitives.ReadInt32LittleEndian(bytes[24..28]);
            Cylinders = BinaryPrimitives.ReadInt32LittleEndian(bytes[28..32]);
        }

        /// <summary>
        /// Get the bytes of the FDI/HDI header for writing
        /// </summary>
        public byte[] GetBytes()
        {
            byte[] bytes = new byte[4096];
            //First four bytes are normally null.
            BinaryPrimitives.WriteInt32LittleEndian(bytes[4..8], DiskType);
            BinaryPrimitives.WriteInt32LittleEndian(bytes[8..12], HeaderSize);
            BinaryPrimitives.WriteInt32LittleEndian(bytes[12..16], DataSize);
            BinaryPrimitives.WriteInt32LittleEndian(bytes[16..20], SectorSize);
            BinaryPrimitives.WriteInt32LittleEndian(bytes[20..24], Sectors);
            BinaryPrimitives.WriteInt32LittleEndian(bytes[24..28], Heads);
            BinaryPrimitives.WriteInt32LittleEndian(bytes[28..32], Cylinders);
            //Reserved bytes until the start of the data area, must be 0.

            return bytes;
        }
    }
}
