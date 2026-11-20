using System.Threading.Tasks;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Questions about the image and its partitions. Each method shows one dialog and returns
/// its answer, or <c>null</c> when it was cancelled.
/// </summary>
public interface IImageDialogs
{
    /// <summary>
    /// Asks how to format the current partition.
    /// </summary>
    Task<FormatOptions?> AskFormatOptionsAsync();
}
