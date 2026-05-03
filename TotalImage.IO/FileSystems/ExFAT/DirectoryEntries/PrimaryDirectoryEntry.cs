using System;
using System.Buffers.Binary;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Represents an exFAT primary directory entry.
/// </summary>
public abstract class PrimaryDirectoryEntry : DirectoryEntry
{
    /// <summary>
    /// Gets the number of secondary entries that follow this primary entry.
    /// </summary>
    public virtual byte SecondaryCount { get; }
    /// <summary>
    /// Gets the checksum for the entry set.
    /// </summary>
    public virtual ushort SetChecksum { get; }
    /// <summary>
    /// Gets the general primary flags.
    /// </summary>
    public virtual ushort GeneralPrimaryFlags { get; }

    /// <summary>
    /// Creates a primary directory entry from a raw directory entry buffer.
    /// </summary>
    /// <param name="entry">The 32-byte exFAT directory entry buffer.</param>
    public PrimaryDirectoryEntry(in ReadOnlySpan<byte> entry) : base(entry)
    {
        SecondaryCount = entry[1];
        SetChecksum = BinaryPrimitives.ReadUInt16LittleEndian(entry[2..4]);
        GeneralPrimaryFlags = BinaryPrimitives.ReadUInt16LittleEndian(entry[4..6]);
    }
}
