using System;
using System.Collections.Generic;
using System.IO;

namespace TotalImage.FileSystems.MTF;

/// <summary>
/// Indexed metadata and logical payload ranges for an MTF file or directory.
/// </summary>
/// <param name="name">The entry's single path component, or an empty string for the synthetic root.</param>
internal sealed class MtfEntry(string name)
{
    /// <summary>Gets the entry name.</summary>
    internal string Name
    {
        get;
    } = name;

    /// <summary>Gets or sets the recorded filesystem attributes.</summary>
    internal FileAttributes Attributes
    {
        get;
        set;
    }

    /// <summary>Gets or sets the recorded creation time, or null when absent or invalid.</summary>
    internal DateTime? CreationTime
    {
        get;
        set;
    }

    /// <summary>Gets or sets the recorded last-write time, or null when absent or invalid.</summary>
    internal DateTime? LastWriteTime
    {
        get;
        set;
    }

    /// <summary>Gets or sets the recorded last-access time, or null when absent or invalid.</summary>
    internal DateTime? LastAccessTime
    {
        get;
        set;
    }

    /// <summary>Gets or sets the length of the standard file data in bytes.</summary>
    internal long Length
    {
        get;
        set;
    }

    /// <summary>Gets the standard-data ranges within the logical MTF stream.</summary>
    internal List<StreamExtent> Extents
    {
        get;
    } = [];
}
