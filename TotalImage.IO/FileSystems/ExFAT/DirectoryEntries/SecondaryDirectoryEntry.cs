using System;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Represents an exFAT secondary directory entry.
/// </summary>
public abstract class SecondaryDirectoryEntry : DirectoryEntry
{
    /// <summary>
    /// Gets the general secondary flags.
    /// </summary>
    public byte GeneralSecondaryFlags { get; }

    /// <summary>
    /// Creates a secondary directory entry from a raw directory entry buffer.
    /// </summary>
    /// <param name="entry">The 32-byte exFAT directory entry buffer.</param>
    public SecondaryDirectoryEntry(in ReadOnlySpan<byte> entry) : base(entry)
    {
        GeneralSecondaryFlags = entry[1];
    }
}
