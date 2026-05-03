using System;
using System.IO;

namespace TotalImage.FileSystems.NTFS;

/// <summary>
/// Represents a file on an NTFS volume.
/// </summary>
public class NtfsFile : File
{
    private readonly NtfsFileSystem _fileSystem;
    private readonly NtfsFileRecord _record;
    private readonly NtfsFileNameRecord _name;

    internal NtfsFile(NtfsFileSystem fileSystem, NtfsFileRecord record, Directory parent, NtfsFileNameRecord name)
        : base(fileSystem, parent)
    {
        _fileSystem = fileSystem;
        _record = record;
        _name = name;
    }

    /// <inheritdoc />
    public override string Name
    {
        get => _name.Name;
        set => throw new NotSupportedException("NTFS is currently exposed as read-only.");
    }

    /// <inheritdoc />
    public override FileAttributes Attributes
    {
        get => _fileSystem.GetEffectiveAttributes(_record, _name) | FileAttributes.ReadOnly;
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
        get => _name.RealSize;
        set => throw new NotSupportedException("NTFS is currently exposed as read-only.");
    }

    /// <inheritdoc />
    public override Stream GetStream() =>
        _fileSystem.OpenDataStream(_record);

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
