using System.Threading.Tasks;
using TotalImage.ViewModels.Dialogs;

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
}
