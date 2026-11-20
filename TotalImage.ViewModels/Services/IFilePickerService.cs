using System.Collections.Generic;
using System.Threading.Tasks;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// A named group of file name patterns for a picker, for example "ISO image" with "*.iso".
/// </summary>
/// <param name="Name">The name shown to the user, without the patterns.</param>
/// <param name="Patterns">Glob patterns such as "*.img". "*.*" matches every file.</param>
public sealed record FileTypeFilter(string Name, IReadOnlyList<string> Patterns);

/// <summary>
/// File and folder pickers. They return plain local paths, or <c>null</c> (an empty list) when
/// cancelled.
/// </summary>
public interface IFilePickerService
{
    /// <summary>
    /// Asks for an existing image file to open, offering <see cref="ImageFileTypes.Open"/>.
    /// </summary>
    Task<string?> PickImageToOpenAsync();

    /// <summary>
    /// Asks where to save the image, offering <see cref="ImageFileTypes.Save"/>. Asks before
    /// overwriting an existing file.
    /// </summary>
    /// <param name="suggestedFileName">A file name (without folder) to start with, or <c>null</c>.</param>
    Task<string?> PickImageSaveLocationAsync(string? suggestedFileName);

    /// <summary>
    /// Asks where to save some other file, such as an export. Asks before overwriting an
    /// existing file.
    /// </summary>
    /// <param name="filters">The file types offered; the first one is selected.</param>
    /// <param name="defaultExtension">The extension added when the user types none, without the dot.</param>
    /// <param name="suggestedFileName">A file name (without folder) to start with, or <c>null</c>.</param>
    Task<string?> PickSaveFileAsync(IReadOnlyList<FileTypeFilter> filters, string defaultExtension, string? suggestedFileName = null);

    /// <summary>
    /// Asks for a folder.
    /// </summary>
    /// <param name="initialFolder">The folder to start in, or <c>null</c>.</param>
    Task<string?> PickFolderAsync(string? initialFolder = null);

    /// <summary>
    /// Asks for one or more existing files to add to the image.
    /// </summary>
    Task<IReadOnlyList<string>> PickFilesToAddAsync();
}

/// <summary>
/// The file types offered when opening and saving images.
/// </summary>
public static class ImageFileTypes
{
    public static readonly FileTypeFilter AllFiles = new("All files", ["*.*"]);

    public static readonly IReadOnlyList<FileTypeFilter> Open =
    [
        new("All supported containers", ["*.nhd", "*.vhd", "*.iso", "*.imz", "*.fdi", "*.hdi", "*.img", "*.ima", "*.vfd", "*.flp", "*.dsk", "*.hdm", "*.288", "*.144", "*.12", "*.720", "*.360", "*.json", "*.zip", "*.tap"]),
        new("Plain sector image", ["*.img", "*.ima", "*.vfd", "*.flp", "*.dsk", "*.hdm", "*.288", "*.144", "*.12", "*.720", "*.360", "*.json"]),
        new("Compressed plain sector image", ["*.imz", "*.zip"]),
        new("Anex86 disk image", ["*.fdi", "*.hdi"]),
        new("ISO image", ["*.iso"]),
        new("Microsoft VHD", ["*.vhd"]),
        new("T98-Next HD", ["*.nhd"]),
        new("PCjs disk images", ["*.json"]),
        new("NT Backup tape image", ["*.tap"]),
        AllFiles
    ];

    public static readonly IReadOnlyList<FileTypeFilter> Save =
    [
        new("Raw sector image", ["*.img", "*.ima", "*.vfd", "*.flp", "*.dsk", "*.xdf", "*.hdm"]),
        AllFiles
    ];

    /// <summary>
    /// The extension added to a saved image whose name has none.
    /// </summary>
    public const string DefaultSaveExtension = "img";
}
