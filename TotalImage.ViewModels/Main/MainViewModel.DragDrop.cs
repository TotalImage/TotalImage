using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TotalImage.FileSystems;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Extraction;

namespace TotalImage.ViewModels.Main;

// Drag and drop: the view model decides and extracts; the view talks to the operating system.
public sealed partial class MainViewModel
{
    // The temporary folder the entries being dragged out were extracted to.
    private string? dragOutFolder;

    /// <summary>
    /// Extracts entries of the file list to a new temporary folder, so they can be dragged out of
    /// the window. The ".." entry and pending entries are left out.
    /// </summary>
    /// <returns>The paths of the extracted entries, or none when there is nothing to drag or the
    /// extraction was cancelled.</returns>
    public Task<IReadOnlyList<string>> PrepareDragOutAsync(IReadOnlyList<EntryViewModel> entries)
        => PrepareDragOutAsync(entries.Where(entry => !entry.IsParentEntry && !entry.IsPending).Select(entry => entry.Entry).ToList());

    /// <summary>
    /// Extracts a folder of the tree to a new temporary folder, so it can be dragged out of the
    /// window. For the root folder, its entries are extracted instead.
    /// </summary>
    /// <returns>The paths of the extracted entries, or none when there is nothing to drag or the
    /// extraction was cancelled.</returns>
    public Task<IReadOnlyList<string>> PrepareDragOutAsync(FolderViewModel folder)
    {
        if (folder.IsPending)
            return Task.FromResult<IReadOnlyList<string>>([]);

        // The root folder's entries are dragged instead of the temporary folder itself, so the
        // drop target doesn't take the folder rather than what is in it.
        List<FileSystemObject> items = folder.IsRoot
            ? folder.Directory.EnumerateFileSystemObjects(settings.Current.ShowHiddenItems).ToList()
            : [folder.Directory];
        return PrepareDragOutAsync(items);
    }

    /// <summary>
    /// Tells that the drag prepared by <see cref="PrepareDragOutAsync(IReadOnlyList{EntryViewModel})"/>
    /// ended. When nothing was dropped, the extracted files are deleted.
    /// </summary>
    public void EndDragOut(bool dropped)
    {
        var folder = dragOutFolder;
        dragOutFolder = null;
        if (!dropped && folder is not null)
            DeleteTemporaryFolder(folder);
    }

    /// <summary>
    /// Whether files dragged onto the window can be dropped: a single file is opened as an image,
    /// only when no image is open.
    /// </summary>
    public bool CanAcceptDrop(IReadOnlyList<string> paths) => paths.Count == 1 && Image is null && !IsBusy;

    /// <summary>
    /// Drops files onto the window: opens a single file as an image, when <see cref="CanAcceptDrop"/> allows it.
    /// </summary>
    public async Task DropAsync(IReadOnlyList<string> paths)
    {
        if (CanAcceptDrop(paths))
            await OpenImageFileAsync(paths[0]);
    }

    private async Task<IReadOnlyList<string>> PrepareDragOutAsync(List<FileSystemObject> items)
    {
        EndDragOut(dropped: false);
        if (Image is null || items.Count == 0)
            return [];

        var folder = tempDirectory.GetUniqueSubdirectoryPath();
        System.IO.Directory.CreateDirectory(folder);
        if (!await ExtractEntriesAsync(items, folder, DirectoryExtractionMode.Preserve, askOverwrite: false))
        {
            DeleteTemporaryFolder(folder);
            return [];
        }

        dragOutFolder = folder;
        return items.Select(item => Path.Combine(folder, item.Name)).ToList();
    }

    private static void DeleteTemporaryFolder(string folder)
    {
        try
        {
            System.IO.Directory.Delete(folder, true);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            // The temporary folder is cleared when the app closes.
        }
    }
}
