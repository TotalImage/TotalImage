using System;
using System.Text;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Represents an exFAT file name directory entry.
/// </summary>
public class FileNameDirectoryEntry : SecondaryDirectoryEntry
{
    /// <summary>
    /// Gets the file name fragment stored in this entry.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Creates a file name directory entry from a raw directory entry buffer.
    /// </summary>
    /// <param name="entry">The 32-byte exFAT directory entry buffer.</param>
    public FileNameDirectoryEntry(in ReadOnlySpan<byte> entry) : base(entry)
    {
        FileName = Encoding.Unicode.GetString(entry[2..32]);
    }

    /// <summary>
    /// Gets the first cluster. Not applicable to file name entries.
    /// </summary>
    public override uint FirstCluster => throw new InvalidOperationException();
    /// <summary>
    /// Gets the data length. Not applicable to file name entries.
    /// </summary>
    public override ulong DataLength => throw new InvalidOperationException();
}
