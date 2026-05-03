using System;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Identifies the type and flags for an exFAT directory entry.
/// </summary>
[Flags]
public enum EntryType : byte
{
    /// <summary>
    /// Indicates a benign entry type.
    /// </summary>
    Benign = 0x20,
    /// <summary>
    /// Indicates a secondary entry.
    /// </summary>
    Secondary = 0x40,
    /// <summary>
    /// Indicates an allocated entry.
    /// </summary>
    InUse = 0x80,

    /// <summary>
    /// Marks the end of a directory.
    /// </summary>
    EndOfDirectory = 0x00,

    /// <summary>
    /// Allocation bitmap entry.
    /// </summary>
    AllocationBitmap = 0x81,
    /// <summary>
    /// Up-case table entry.
    /// </summary>
    UpCaseTable = 0x82,
    /// <summary>
    /// Volume label entry.
    /// </summary>
    VolumeLabel = 0x83,
    /// <summary>
    /// File primary entry.
    /// </summary>
    File = 0x85,

    /// <summary>
    /// Volume GUID entry.
    /// </summary>
    VolumeGuid = 0xA0,
    /// <summary>
    /// TexFAT padding entry.
    /// </summary>
    TexFatPadding = 0xA1,

    /// <summary>
    /// Stream extension entry.
    /// </summary>
    StreamExtension = 0xC0,
    /// <summary>
    /// File name entry.
    /// </summary>
    FileName = 0xC1,

    /// <summary>
    /// Vendor extension entry.
    /// </summary>
    VendorExtension = 0xE0,
    /// <summary>
    /// Vendor allocation entry.
    /// </summary>
    VendorAllocation = 0xE1
}
