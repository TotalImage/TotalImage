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
        public CHSAddress(byte[] address)
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
    }
}
