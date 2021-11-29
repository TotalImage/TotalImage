using System;

/// <summary>
/// Interface that describes file system object features that are specific
/// to the FAT family (FAT12, FAT16, FAT32)
/// </summary>
public interface IFatFileSystemObject
{
    /// <value>
    /// The MS-DOS compatible "short" name (8.3).
    /// </value>
    string ShortName { get; set; }

    /// <value>
    /// The VFAT "long" name (LFN) if the object has one, otherwise <c>null</c>.
    /// </value>
    string? LongName { get; set; }

    /// <summary>
    /// The starting cluster in the cluster chain of this object.
    /// </summary>
    uint FirstCluster { get; set; }
}
