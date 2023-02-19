using System;
using System.IO;
using Directory = TotalImage.FileSystems.Directory;

namespace TotalImage.UI.ViewModel;

public class DirectoryPropertiesWindowViewModel
{
    private readonly Directory _fsObject;

    public string Name
        => _fsObject.Name;

    public string? Location
        => _fsObject.Parent?.FullName;

    public ulong Length
        => _fsObject.Length;

    public ulong LengthOnDisk
        => _fsObject.LengthOnDisk;

    public DateTime? Created
        => _fsObject.CreationTime;

    public DateTime? Modified
        => _fsObject.LastWriteTime;

    public DateTime? Accessed
        => _fsObject.LastAccessTime;

    public bool IsReadOnly
        => _fsObject.Attributes.HasFlag(FileAttributes.ReadOnly);

    public bool IsHidden
        => _fsObject.Attributes.HasFlag(FileAttributes.Hidden);

    public bool IsSystem
        => _fsObject.Attributes.HasFlag(FileAttributes.System);

    public bool IsArchive
        => _fsObject.Attributes.HasFlag(FileAttributes.Archive);

    public DirectoryPropertiesWindowViewModel(Directory directory)
    {
        _fsObject = directory;
    }
}
