using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Platform.Storage;

namespace TotalImage.Avalonia.Platform;

/// <summary>
/// Builds the drag data of entries extracted for dragging them out of the window.
/// </summary>
internal static class DragOutData
{
    // DROPEFFECT_COPY
    private const int DropEffectCopy = 1;

    /// <summary>
    /// Creates the drag data for extracted files and folders: the file list, and on Windows the
    /// "Preferred DropEffect = Copy" marker for Explorer.
    /// </summary>
    public static async Task<IDataObject> CreateAsync(IStorageProvider storage, IReadOnlyList<string> paths)
    {
        var items = new List<IStorageItem>();
        foreach (var path in paths)
        {
            IStorageItem? item = Directory.Exists(path)
                ? await storage.TryGetFolderFromPathAsync(path)
                : await storage.TryGetFileFromPathAsync(path);
            if (item is not null)
                items.Add(item);
        }

        var data = new DataObject();
        data.Set(DataFormats.Files, items);

        if (OperatingSystem.IsWindows())
        {
            /* Set the preferred drop effect to Copy to prevent Explorer from defaulting to "Create a link" for filenames
             * that are registered under SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths. */
            data.Set("Preferred DropEffect", BitConverter.GetBytes(DropEffectCopy));
        }

        return data;
    }
}
