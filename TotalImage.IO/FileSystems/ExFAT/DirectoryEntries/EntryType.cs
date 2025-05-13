using System;

namespace TotalImage.FileSystems.ExFAT;

[Flags]
public enum EntryType : byte
{
    Benign = 0x20,
    Secondary = 0x40,
    InUse = 0x80,

    EndOfDirectory = 0x00,

    AllocationBitmap = 0x81,
    UpCaseTable = 0x82,
    VolumeLabel = 0x83,
    File = 0x85,

    VolumeGuid = 0xA0,
    TexFatPadding = 0xA1,

    StreamExtension = 0xC0,
    FileName = 0xC1,

    VendorExtension = 0xE0,
    VendorAllocation = 0xE1
}
