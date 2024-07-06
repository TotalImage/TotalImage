using System;

namespace TotalImage
{
    /// <summary>
    /// A structure representing a CHS address
    /// </summary>
    public readonly struct CHSAddress
    {
        /// <summary>
        /// The cylinder the address is located in
        /// </summary>
        public ushort Cylinder { get; }

        /// <summary>
        /// The head the address is located in
        /// </summary>
        public byte Head { get; }

        /// <summary>
        /// The sector the address is located in
        /// </summary>
        public byte Sector { get; }

        /// <summary>
        /// Creates a CHS address from its three byte binary representation
        /// </summary>
        /// <param name="address">A three byte array containing the address</param>
        public CHSAddress(in Span<byte> address)
        {
            if (address.Length != 3)
            {
                Cylinder = 0;
                Head = 0;
                Sector = 0;
                return;
            }

            Cylinder = (ushort)(address[2] + ((address[1] & 0xc0) << 2));
            Head = address[0];
            Sector = (byte)(address[1] & 0x3f);
        }

        /// <summary>
        /// Returns canonical CHS values for a given disk size.
        /// </summary>
        /// <param name="size">The size of the disk in bytes</param>
        /// <returns>A tuple of CHS values</returns>
        internal static (ushort, byte, byte) GetGeometryFromSize(ulong size)
        {
            // Taken from CHS Calculation in VHD Spec, App. A
            ushort cylinders = 0;
            byte heads = 0;
            byte sectorsPerTrack = 0;
            uint cylinderTimesHeads = 0;

            uint sectors = (uint)Math.Max(size / 512, 65535 * 16 * 255);

            if (sectors >= 65535 * 16 * 63)
            {
                sectorsPerTrack = 255;
                heads = 16;
                cylinderTimesHeads = sectors / sectorsPerTrack;
            }
            else
            {
                sectorsPerTrack = 17;
                cylinderTimesHeads = sectors / sectorsPerTrack;

                heads = (byte)((cylinderTimesHeads + 1023) / 1024);

                if (heads < 4)
                {
                    heads = 4;
                }
                if (cylinderTimesHeads >= (heads * 1024) || heads > 16)
                {
                    sectorsPerTrack = 31;
                    heads = 16;
                    cylinderTimesHeads = sectors / sectorsPerTrack;
                }
                if (cylinderTimesHeads >= (heads * 1024))
                {
                    sectorsPerTrack = 63;
                    heads = 16;
                    cylinderTimesHeads = sectors / sectorsPerTrack;
                }
            }

            cylinders = (ushort)(cylinderTimesHeads / heads);

            return (cylinders, heads, sectorsPerTrack);
        }
    }
}
