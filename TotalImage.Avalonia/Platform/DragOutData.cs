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

    private static readonly DataFormat<byte[]> PreferredDropEffect = DataFormat.CreateBytesPlatformFormat("Preferred DropEffect");

    /// <summary>
    /// Creates the drag data for extracted files and folders: the file list, and on Windows the
    /// "Preferred DropEffect = Copy" marker for Explorer.
    /// </summary>
    public static async Task<IDataTransfer> CreateAsync(IStorageProvider storage, IReadOnlyList<string> paths)
    {
        var data = new DataTransfer();
        foreach (var path in paths)
        {
            IStorageItem? item = Directory.Exists(path)
                ? await storage.TryGetFolderFromPathAsync(path)
                : await storage.TryGetFileFromPathAsync(path);
            if (item is not null)
                data.Add(DataTransferItem.CreateFile(item));
        }

        if (OperatingSystem.IsWindows())
        {
            /* Set the preferred drop effect to Copy to prevent Explorer from defaulting to "Create a link" for filenames
             * that are registered under SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths. */
            data.Add(DataTransferItem.Create(PreferredDropEffect, BitConverter.GetBytes(DropEffectCopy)));
        }

        return data;
    }
}
