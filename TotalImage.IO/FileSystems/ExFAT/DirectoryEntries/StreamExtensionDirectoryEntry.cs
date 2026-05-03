using System;
using System.Buffers.Binary;
using System.IO;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Represents an exFAT stream extension directory entry.
/// </summary>
public class StreamExtensionDirectoryEntry : SecondaryDirectoryEntry, IStreamable
{
    /// <summary>
    /// Gets the file name length.
    /// </summary>
    public byte NameLength { get; }
    /// <summary>
    /// Gets the file name hash.
    /// </summary>
    public ushort NameHash { get; }
    /// <summary>
    /// Gets the valid data length for the stream.
    /// </summary>
    public ulong ValidDataLength { get; }

    /// <summary>
    /// Creates a stream extension directory entry from a raw directory entry buffer.
    /// </summary>
    /// <param name="entry">The 32-byte exFAT directory entry buffer.</param>
    public StreamExtensionDirectoryEntry(in ReadOnlySpan<byte> entry) : base(entry)
    {
        NameLength = entry[3];
        NameHash = BinaryPrimitives.ReadUInt16LittleEndian(entry[4..6]);
        ValidDataLength = BinaryPrimitives.ReadUInt64LittleEndian(entry[8..16]);
    }

    /// <inheritdoc />
    public Stream GetStream(ExFatFileSystem fileSystem) =>
        GetStreamInternal(fileSystem, (GeneralSecondaryFlags & 0x02) != 0, ValidDataLength);
}
