using System;
using System.Collections.Generic;
using System.IO;

namespace TotalImage.FileSystems.NTFS;

/// <summary>
/// Represents a directory on an NTFS volume.
/// </summary>
public class NtfsDirectory : Directory
{
    private readonly NtfsFileSystem _fileSystem;
    private readonly NtfsFileRecord _record;
    private readonly NtfsFileNameRecord? _name;

    internal NtfsFileRecord Record => _record;
    internal NtfsFileNameRecord? NameRecord => _name;

    internal NtfsDirectory(NtfsFileSystem fileSystem, NtfsFileRecord record, Directory? parent, NtfsFileNameRecord? name)
        : base(fileSystem, parent)
    {
        _fileSystem = fileSystem;
        _record = record;
        _name = name;
    }

    /// <inheritdoc />
    public override string Name
    {
        get => _name?.Name ?? string.Empty;
        set => throw new NotSupportedException("NTFS is currently exposed as read-only.");
    }

    /// <inheritdoc />
    public override FileAttributes Attributes
    {
        get => _fileSystem.GetEffectiveAttributes(_record, _name) | FileAttributes.Directory | FileAttributes.ReadOnly;
        set => throw new NotSupportedException("NTFS is currently exposed as read-only.");
    }

    /// <inheritdoc />
    public override DateTime? LastAccessTime
    {
        get => _record.StandardInformation?.LastAccessTime;
        set => throw new NotSupportedException("NTFS is currently exposed as read-only.");
    }

    /// <inheritdoc />
    public override DateTime? LastWriteTime
    {
        get => _record.StandardInformation?.LastWriteTime;
        set => throw new NotSupportedException("NTFS is currently exposed as read-only.");
    }

    /// <inheritdoc />
    public override DateTime? CreationTime
    {
        get => _record.StandardInformation?.CreationTime;
        set => throw new NotSupportedException("NTFS is currently exposed as read-only.");
    }

    /// <inheritdoc />
    public override ulong Length
    {
        get => _name?.RealSize ?? 0;
        set => throw new NotSupportedException("NTFS is currently exposed as read-only.");
    }

    /// <inheritdoc />
    public override FileSystemObject ResolveTarget() =>
        _fileSystem.ResolveDirectoryObject(this);

    /// <inheritdoc />
    public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden)
    {
        foreach ((NtfsFileRecord record, NtfsFileNameRecord fileName) in _fileSystem.EnumerateDirectoryEntries(_fileSystem.ResolveDirectoryRecord(_record)))
        {
            FileAttributes attributes = _fileSystem.GetEffectiveAttributes(record, fileName);
            if (!showHidden && (attributes & FileAttributes.Hidden) != 0)
            {
                continue;
            }

            if (!record.IsInUse)
            {
                continue;
            }

            if (record.IsDirectory)
            {
                yield return new NtfsDirectory(_fileSystem, record, this, fileName);
            }
            else
            {
                yield return new NtfsFile(_fileSystem, record, this, fileName);
            }
        }
    }

    /// <inheritdoc />
    public override Directory CreateSubdirectory(string path)
    {
        throw new NotSupportedException("NTFS is currently exposed as read-only.");
    }

    /// <inheritdoc />
    public override void Delete()
    {
        throw new NotSupportedException("NTFS is currently exposed as read-only.");
    }

    /// <inheritdoc />
    public override void MoveTo(string path)
    {
        throw new NotSupportedException("NTFS is currently exposed as read-only.");
    }
}
