using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TotalImage.FileSystems.MTF;

/// <summary>
/// A read-only directory in an NT Backup archive, including synthetic set and volume directories.
/// </summary>
public sealed class MtfDirectory : Directory
{
    /// <summary>Gets this directory's indexed metadata.</summary>
    internal MtfEntry Entry
    {
        get;
    }

    /// <summary>Gets the direct child entries populated while indexing the archive.</summary>
    internal List<FileSystemObject> Children
    {
        get;
    } = [];

    /// <summary>Creates a directory view for a backup or synthetic grouping entry.</summary>
    /// <param name="fileSystem">The backup filesystem containing this directory.</param>
    /// <param name="parent">The parent directory, or null for the synthetic root.</param>
    /// <param name="name">The directory name, or an empty string for the synthetic root.</param>
    internal MtfDirectory(MtfFileSystem fileSystem, Directory? parent, string name) : base(fileSystem, parent)
    {
        Entry = new MtfEntry(name);
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
            return Entry.Attributes | FileAttributes.Directory;
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
    /// <remarks>Directory entries do not expose standard file data and report a length of zero.</remarks>
    /// <exception cref="NotSupportedException">The property cannot be changed in a read-only archive.</exception>
    public override ulong Length
    {
        get
        {
            return 0;
        }
        set
        {
            throw ReadOnly();
        }
    }

    /// <inheritdoc />
    public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden)
    {
        return Children.Where(child => showHidden || (child.Attributes & FileAttributes.Hidden) == 0);
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">Directories cannot be created in a read-only archive.</exception>
    public override Directory CreateSubdirectory(string path)
    {
        throw ReadOnly();
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">Directories cannot be deleted from a read-only archive.</exception>
    public override void Delete()
    {
        throw ReadOnly();
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">Directories cannot be moved in a read-only archive.</exception>
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
