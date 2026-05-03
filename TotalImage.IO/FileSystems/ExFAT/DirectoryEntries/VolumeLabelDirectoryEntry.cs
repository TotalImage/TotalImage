using System;
using System.Text;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Represents an exFAT volume label directory entry.
/// </summary>
public class VolumeLabelDirectoryEntry : PrimaryDirectoryEntry
{
    /// <summary>
    /// Gets the number of characters in the volume label.
    /// </summary>
    public byte CharacterCount { get; }
    /// <summary>
    /// Gets the raw volume label value.
    /// </summary>
    public string VolumeLabel { get; }

    /// <summary>
    /// Creates a volume label directory entry from a raw directory entry buffer.
    /// </summary>
    /// <param name="entry">The 32-byte exFAT directory entry buffer.</param>
    public VolumeLabelDirectoryEntry(in ReadOnlySpan<byte> entry) : base(entry)
    {
        CharacterCount = entry[1];
        VolumeLabel = Encoding.Unicode.GetString(entry[2..24]);
    }

    /// <summary>
    /// Gets the general primary flags. Not applicable to volume label entries.
    /// </summary>
    public override ushort GeneralPrimaryFlags => throw new InvalidOperationException();
    /// <summary>
    /// Gets the first cluster. Not applicable to volume label entries.
    /// </summary>
    public override uint FirstCluster => throw new InvalidOperationException();
    /// <summary>
    /// Gets the data length. Not applicable to volume label entries.
    /// </summary>
    public override ulong DataLength => throw new InvalidOperationException();
}
