namespace TotalImage.FileSystems.BPB
{
    /// <summary>
    /// Identifies the DOS extended boot signature format.
    /// </summary>
    public enum ExtendedBootSignature : byte
    {
        /// <summary>
        /// PC-DOS 3.4 extended boot signature.
        /// </summary>
        Dos34 = 0x28,

        /// <summary>
        /// DOS 4.0 extended boot signature.
        /// </summary>
        Dos40 = 0x29
    }
}
