using System.IO;

namespace TotalImage.ViewModels.Browsing;

/// <summary>
/// What an icon shows. Each UI maps it to its own icons.
/// </summary>
public enum IconKind
{
    /// <summary>
    /// A folder.
    /// </summary>
    Folder,

    /// <summary>
    /// A file, whose icon depends on its <see cref="IconKey.Extension"/>.
    /// </summary>
    File,

    /// <summary>
    /// A volume (partition).
    /// </summary>
    Volume,

    /// <summary>
    /// The ".." entry that goes to the parent folder.
    /// </summary>
    ParentFolder
}

/// <summary>
/// Neutral icon information of a folder or an entry. Whether it is hidden is told separately.
/// </summary>
/// <param name="Kind">What the icon shows.</param>
/// <param name="Extension">For files, the extension in lower case with its dot (for example ".txt"), or empty.</param>
public readonly record struct IconKey(IconKind Kind, string Extension = "")
{
    public static IconKey Folder { get; } = new(IconKind.Folder);

    public static IconKey Volume { get; } = new(IconKind.Volume);

    public static IconKey ParentFolder { get; } = new(IconKind.ParentFolder);

    /// <summary>
    /// The icon of a file with the given name.
    /// </summary>
    public static IconKey ForFile(string name) => new(IconKind.File, Path.GetExtension(name).ToLowerInvariant());

    /// <summary>
    /// The icon of an entry with the given name and attributes: a folder or a file.
    /// </summary>
    public static IconKey ForEntry(string name, FileAttributes attributes)
        => attributes.HasFlag(FileAttributes.Directory) ? Folder : ForFile(name);
}
