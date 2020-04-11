using System;

namespace TotalImage.FileSystems
{
    [Flags]
    public enum FileAttributes
    {
        ReadOnly = 0x01,
        Hidden = 0x02,
        System = 0x04,
        VolumeId = 0x08,
        Directory = 0x10,
        Archive = 0x20,
        Reserved = 0xC0
    }
}