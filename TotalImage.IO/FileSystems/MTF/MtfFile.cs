using System;
using System.IO;

namespace TotalImage.FileSystems.MTF;

/// <summary>
/// A read-only file backed by an uncompressed MTF standard data stream.
/// </summary>
public sealed class MtfFile : File
{
    /// <summary>Gets the indexed metadata and payload ranges for this file.</summary>
    internal MtfEntry Entry
    {
        get;
    }

    /// <summary>
    /// Creates a file view using metadata already indexed by its owning filesystem.
    /// </summary>
    /// <param name="fileSystem">The backup filesystem containing this file.</param>
    /// <param name="parent">The directory containing this file.</param>
    /// <param name="entry">The indexed file metadata and standard-data ranges.</param>
    internal MtfFile(MtfFileSystem fileSystem, Directory parent, MtfEntry entry) : base(fileSystem, parent)
    {
        Entry = entry;
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">The property cannot be changed in a read-only archive.</exception>
    public override string Name
    {
        get
        {
            return Entry.Name;
        }
        set
        {
            throw ReadOnly();
        }
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">The property cannot be changed in a read-only archive.</exception>
    public override FileAttributes Attributes
    {
        get
        {
            return Entry.Attributes;
        }
        set
        {
            throw ReadOnly();
        }
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">The property cannot be changed in a read-only archive.</exception>
    public override DateTime? CreationTime
    {
        get
        {
            return Entry.CreationTime;
        }
        set
        {
            throw ReadOnly();
        }
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">The property cannot be changed in a read-only archive.</exception>
    public override DateTime? LastWriteTime
    {
        get
        {
            return Entry.LastWriteTime;
        }
        set
        {
            throw ReadOnly();
        }
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">The property cannot be changed in a read-only archive.</exception>
    public override DateTime? LastAccessTime
    {
        get
        {
            return Entry.LastAccessTime;
        }
        set
        {
            throw ReadOnly();
        }
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">The property cannot be changed in a read-only archive.</exception>
    public override ulong Length
    {
        get
        {
            return (ulong)Entry.Length;
        }
        set
        {
            throw ReadOnly();
        }
    }

    /// <inheritdoc />
    /// <remarks>
    /// Each call creates an independent, seekable, read-only view. Disposing that view leaves the archive open.
    /// </remarks>
    public override Stream GetStream()
    {
        return new ReadOnlyExtentStream(FileSystem.GetStream(), Entry.Extents);
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">Files cannot be deleted from a read-only archive.</exception>
    public override void Delete()
    {
        throw ReadOnly();
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">Files cannot be moved in a read-only archive.</exception>
    public override void MoveTo(string path)
    {
        throw ReadOnly();
    }

    /// <summary>Creates the exception used for unsupported mutation requests.</summary>
    /// <returns>An exception explaining that the archive is read-only.</returns>
    private static NotSupportedException ReadOnly()
    {
        return new NotSupportedException("NT Backup archives are read-only.");
    }
}
