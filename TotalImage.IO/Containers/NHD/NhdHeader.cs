using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace TotalImage.Containers.NHD
{
    /// <summary>
    /// A class representing the header structure of the NHD image
    /// </summary>
    public class NhdHeader
    {
        private const string COOKIE_VALUE = "T98HDDIMAGE.R0";

        /// <summary>
        /// The magic value that marks the start of the header
        /// </summary>
        public string Cookie { get; }

        /// <summary>
        /// Optional image comment, up to 256 ASCII characters
        /// </summary>
        public string? Comment { get; }

        /// <summary>
        /// The size of this header in bytes
        /// </summary>
        public uint HeaderSize { get; }

        /// <summary>
        /// The number of cylinders of the disk represented by this NHD
        /// </summary>
        public uint Cylinders { get; }

        /// <summary>
        /// The number of heads of the disk represented by this NHD
        /// </summary>
        public ushort Heads { get; }

        /// <summary>
        /// The number of sectors per track represented by this NHD
        /// </summary>
        public ushort SectorsPerTrack { get; }

        /// <summary>
        /// The size of a sector in bytes
        /// </summary>
        public ushort SectorSize { get; }

        /// <summary>
        /// Get the bytes of the NHD header for writing
        /// </summary>
        public byte[] GetBytes()
        {
            byte[] bytes = new byte[512];
            Array.Copy(Encoding.ASCII.GetBytes(COOKIE_VALUE), 0, bytes, 0, 14);
            //Two reserved bytes here, must be 0
            if (Comment != null)
                Array.Copy(Encoding.ASCII.GetBytes(Comment), 0, bytes, 16, 256);
            BinaryPrimitives.WriteUInt32LittleEndian(bytes[272..276], HeaderSize);
            BinaryPrimitives.WriteUInt32LittleEndian(bytes[276..280], Cylinders);
            BinaryPrimitives.WriteUInt16LittleEndian(bytes[280..282], Heads);
            BinaryPrimitives.WriteUInt16LittleEndian(bytes[282..284], SectorsPerTrack);
            BinaryPrimitives.WriteUInt16LittleEndian(bytes[284..286], SectorSize);
            //226 reserved bytes from here to the end of the header, must be 0

            return bytes;
        }

        /// <summary>
        /// Create an empty new NHD header for a specified disk size
        /// </summary>
        /// <param name="size">Size of disk in bytes</param>
        public NhdHeader(ulong size)
        {
            Cookie = COOKIE_VALUE;
            Comment = "Created by TotalImage 1.0";
            HeaderSize = 512;
            SectorSize = 512;
            (Cylinders, Heads, SectorsPerTrack) = CHSAddress.GetGeometryFromSize(size);
        }

        /// <summary>
        /// Read an NHD header from a span of bytes
        /// </summary>
        /// <param name="bytes">The span of bytes containing the header</param>
        public NhdHeader(ReadOnlySpan<byte> bytes)
        {
            Cookie = Encoding.ASCII.GetString(bytes[0..14]);
            if (!string.Equals(COOKIE_VALUE, Cookie, StringComparison.InvariantCulture))
            {
                throw new InvalidDataException("Could not find a valid NHD header");
            }

            Comment = Encoding.ASCII.GetString(bytes[16..272]).TrimEnd().TrimEnd('\0'); //Also clear the trailing null bytes just in case
            HeaderSize = BinaryPrimitives.ReadUInt32LittleEndian(bytes[272..276]);
            Cylinders = BinaryPrimitives.ReadUInt32LittleEndian(bytes[276..280]);
            Heads = BinaryPrimitives.ReadUInt16LittleEndian(bytes[280..282]);
            SectorsPerTrack = BinaryPrimitives.ReadUInt16LittleEndian(bytes[282..284]);
            SectorSize = BinaryPrimitives.ReadUInt16LittleEndian(bytes[284..286]);
        }
    }
}
