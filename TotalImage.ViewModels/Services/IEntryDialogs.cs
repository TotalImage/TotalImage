using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TotalImage.FileSystems;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Extraction;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Questions about files and folders in the image. Each method shows one dialog and returns
/// its answer, or <c>null</c> when it was cancelled.
/// </summary>
public interface IEntryDialogs
{
    /// <summary>
    /// Asks for a new name for an entry.
    /// </summary>
    Task<string?> AskRenameAsync(string currentName);

    /// <summary>
    /// Asks for the name of a new folder.
    /// </summary>
    Task<string?> AskNewFolderNameAsync();

    /// <summary>
    /// Asks for new volume labels of a FAT volume.
    /// </summary>
    Task<VolumeLabelChange?> AskVolumeLabelAsync(string rootDirectoryLabel, string bpbLabel);

    /// <summary>
    /// Asks where and how to extract the selected entries.
    /// </summary>
    /// <param name="itemCount">The number of selected entries.</param>
    Task<ExtractOptions?> AskExtractOptionsAsync(int itemCount);

    /// <summary>
    /// Shows the properties of one or more entries and asks for changes to them.
    /// </summary>
    Task<EntryPropertiesChange?> AskPropertiesAsync(IReadOnlyList<FileSystemObject> entries);

    /// <summary>
    /// Shows the progress of an extraction in a modal window while it runs, so the image is not
    /// used for anything else meanwhile. The window's Cancel button cancels the token; the window
    /// closes once the extraction has finished.
    /// </summary>
    /// <param name="extraction">Runs the extraction, reporting its progress.</param>
    /// <returns>Completes (or fails) like <paramref name="extraction"/>, after the window closed.</returns>
    Task ShowExtractionProgressAsync(Func<IProgress<ExtractionProgress>, CancellationToken, Task> extraction);
}
