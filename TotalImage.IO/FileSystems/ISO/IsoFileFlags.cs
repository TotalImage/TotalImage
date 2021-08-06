using System;

namespace TotalImage.FileSystems.ISO
{
    /// <summary>
    /// Flags set for a file on the ISO 9660 file system
    /// </summary>
    [Flags]
    public enum IsoFileFlags : byte
    {
        /// <summary>
        /// If set, the existence of the file should not be shown to the user
        /// </summary>
        Existence = 1 << 0,

        /// <summary>
        /// If set, the record is a directory
        /// </summary>
        Directory = 1 << 1,

        /// <summary>
        /// If set, the record is an associated file
        /// </summary>
        AssociatedFile = 1 << 2,

        /// <summary>
        /// If set, the record is an extended attribute record
        /// </summary>
        Record = 1 << 3,

        /// <summary>
        /// If set, permissions for this file are set in an extended attribute record
        /// </summary>
        Protection = 1 << 4,

        /// <summary>
        /// If set, this is not the final record for the entry
        /// </summary>
        MultiExtent = 1 << 7
    }
}
