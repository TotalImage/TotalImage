namespace TotalImage.FileSystems.BPB
{
    /// <summary>
    /// Distinct BIOS Parameter Block versions.
    /// </summary>
    public enum BiosParameterBlockVersion
    {
        /// <summary>
        /// A BIOS Parameter Block intended for compatibility with DOS 2.0 or greater
        /// </summary>
        Dos20,

        /// <summary>
        /// A BIOS Parameter Block intended for compatibility with DOS 3.4 or greater
        /// </summary>
        Dos34,

        /// <summary>
        /// A BIOS Parameter Block intended for compatibility with DOS 4.0 or greater
        /// </summary>
        Dos40,

        /// <summary>
        /// A BIOS Parameter Block intended for use alongside the FAT32 file system
        /// </summary>
        Fat32,

        /// <summary>
        /// A BIOS Parameter Block intended for use alongside the NTFS file system
        /// </summary>
        Ntfs
    }
}
