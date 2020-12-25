using System;
using System.Collections.Generic;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    public interface IBiosParameterBlockExtended : IBiosParameterBlock
    {
        byte PhysicalDriveNumber { get; set; }

        byte Flags { get; set; }

        uint VolumeSerialNumber { get; set; }

        string VolumeLabel { get; set; }

        string FileSystemType { get; set; }
    }
}
