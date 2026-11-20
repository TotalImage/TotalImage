using System.Threading.Tasks;
using TotalImage.Containers;
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

    /// <summary>
    /// Asks which partition of the image to load.
    /// </summary>
    /// <param name="image">The image whose partition table is listed.</param>
    Task<PartitionSelection?> AskSelectPartitionAsync(Container image);

    /// <summary>
    /// Shows the partition manager. It doesn't list partitions or change anything yet.
    /// </summary>
    Task ShowManagePartitionsAsync();

    /// <summary>
    /// Asks what kind of image to create, including its advanced options.
    /// </summary>
    Task<NewImageOptions?> AskNewImageAsync();
}
