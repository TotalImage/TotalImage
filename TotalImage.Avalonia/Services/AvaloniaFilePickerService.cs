using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using TotalImage.ViewModels.Services;

namespace TotalImage.Avalonia.Services;

/// <summary>
/// File and folder pickers through the window's storage provider. Only local paths are returned.
/// </summary>
public sealed class AvaloniaFilePickerService(WindowOwner owner) : IFilePickerService
{
    public async Task<string?> PickImageToOpenAsync()
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open image",
            AllowMultiple = false,
            FileTypeFilter = ToFileTypes(ImageFileTypes.Open)
        });
        return files.Select(file => file.TryGetLocalPath()).FirstOrDefault(path => path is not null);
    }

    public Task<string?> PickImageSaveLocationAsync(string? suggestedFileName)
        => PickSaveFileAsync(ImageFileTypes.Save, ImageFileTypes.DefaultSaveExtension, suggestedFileName);

    public async Task<string?> PickSaveFileAsync(IReadOnlyList<FileTypeFilter> filters, string defaultExtension, string? suggestedFileName = null)
    {
        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            SuggestedFileName = suggestedFileName,
            DefaultExtension = defaultExtension,
            FileTypeChoices = ToFileTypes(filters),
            ShowOverwritePrompt = true
        });
        return file?.TryGetLocalPath();
    }

    public async Task<string?> PickFolderAsync(string? initialFolder = null)
    {
        var provider = StorageProvider;
        var start = string.IsNullOrEmpty(initialFolder) ? null : await provider.TryGetFolderFromPathAsync(initialFolder);
        var folders = await provider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            AllowMultiple = false,
            SuggestedStartLocation = start
        });
        return folders.Select(folder => folder.TryGetLocalPath()).FirstOrDefault(path => path is not null);
    }

    public async Task<IReadOnlyList<string>> PickFilesToAddAsync()
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Inject files",
            AllowMultiple = true,
            FileTypeFilter = ToFileTypes([ImageFileTypes.AllFiles])
        });
        return files.Select(file => file.TryGetLocalPath()).OfType<string>().ToList();
    }

    private IStorageProvider StorageProvider => DialogHost.TopLevelFor(owner.Window)?.StorageProvider
        ?? throw new InvalidOperationException("There is no window to show the picker on.");

    // "*.*" only matches names with a dot on some platforms; "*" matches every file everywhere.
    private static List<FilePickerFileType> ToFileTypes(IEnumerable<FileTypeFilter> filters)
        => filters.Select(filter => new FilePickerFileType(filter.Name)
        {
            Patterns = filter.Patterns.Select(pattern => pattern == "*.*" ? "*" : pattern).ToList()
        }).ToList();
}
