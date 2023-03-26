using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TotalImage.Containers.FDI
{
    public class FdiHeader
    {
        /// <summary>
        /// Describes the disk format contained in the image.
        /// </summary>
        public int FddType { get; }

        /// <summary>
        /// Size of header in bytes.
        /// </summary>
        public int HeaderSize { get; }

        /// <summary>
        /// Size of disk in bytes.
        /// </summary>
        public int FddSize { get; }

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
        public int Surfaces { get; }

        /// <summary>
        /// Cylinder/track count.
        /// </summary>
        public int Cylinders { get; }


        /// <summary>
        /// Get the bytes of the FDI header for writing
        /// </summary>
        public byte[] GetBytes()
        {
            byte[] bytes = new byte[4096];
            //First four bytes are null.
            BinaryPrimitives.WriteInt32LittleEndian(bytes[4..8], FddType);
            BinaryPrimitives.WriteInt32LittleEndian(bytes[8..12], HeaderSize);
            BinaryPrimitives.WriteInt32LittleEndian(bytes[12..16], FddSize);
            BinaryPrimitives.WriteInt32LittleEndian(bytes[16..20], SectorSize);
            BinaryPrimitives.WriteInt32LittleEndian(bytes[20..24], Sectors);
            BinaryPrimitives.WriteInt32LittleEndian(bytes[24..28], Surfaces);
            BinaryPrimitives.WriteInt32LittleEndian(bytes[28..32], Cylinders);
            //4064 reserved bytes from here to the end of the header, must be 0.

            return bytes;
        }

        public FdiHeader(ReadOnlySpan<byte> bytes)
        {
            FddType = BinaryPrimitives.ReadInt32LittleEndian(bytes[4..8]);
            HeaderSize = BinaryPrimitives.ReadInt32LittleEndian(bytes[8..12]);
            FddSize = BinaryPrimitives.ReadInt32LittleEndian(bytes[12..16]);
            SectorSize = BinaryPrimitives.ReadInt32LittleEndian(bytes[16..20]);
            Sectors = BinaryPrimitives.ReadInt32LittleEndian(bytes[20..24]);
            Surfaces = BinaryPrimitives.ReadInt32LittleEndian(bytes[24..28]);
            Cylinders = BinaryPrimitives.ReadInt32LittleEndian(bytes[28..32]);
        }
    }
}
