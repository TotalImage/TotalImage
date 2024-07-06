using System;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// Attribute values used by FAT filesystems.
    /// </summary>
    [Flags]
    public enum FatAttributes : byte
    {
        /// <summary>
        /// Writes to the file should fail.
        /// </summary>
        ReadOnly = 0x01,

        /// <summary>
        /// Normal directory listings should not show this file.
        /// </summary>
        Hidden = 0x02,

        /// <summary>
        /// This is an operating system file and should not be moved (e.g. during defragmentation).
        /// </summary>
        System = 0x04,

        /// <summary>
        /// The name of this file is actually the label for the volume.
        /// </summary>
        VolumeId = 0x08,

        /// <summary>
        /// This file is actually a container for other files.
        /// </summary>
        Subdirectory = 0x10,

        /// <summary>
        /// This file has been modified since the last time that a backup has been performed.
        /// </summary>
        Archive = 0x20,

        /// <summary>
        /// Reserved.
        /// </summary>
        Device = 0x40,

        /// <summary>
        /// Reserved.
        /// </summary>
        Reserved = 0x80,

        /// <summary>
        /// The file is actually a part of the long name entry for another file.
        /// </summary>
        LongName = ReadOnly | Hidden | System | VolumeId
    }
}
