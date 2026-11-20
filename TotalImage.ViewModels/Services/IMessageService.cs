using System.Threading.Tasks;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Simple questions and messages. There is one method per kind of question; the wording and
/// the buttons belong to each UI.
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// Warns that formatting erases all data in the current partition and asks whether to continue.
    /// </summary>
    Task<bool> ConfirmFormatAsync();

    /// <summary>
    /// Tells the user that the list of recently opened images was cleared.
    /// </summary>
    Task ShowRecentImagesClearedAsync();

    /// <summary>
    /// Warns that all settings will be reset to their defaults and asks whether to continue.
    /// </summary>
    Task<bool> ConfirmResetSettingsAsync();

    /// <summary>
    /// Tells the user that all settings were reset to their defaults.
    /// </summary>
    Task ShowSettingsResetAsync();

    /// <summary>
    /// Warns that other instances are running and may be using the temporary folder, and asks
    /// whether to clear it anyway.
    /// </summary>
    Task<bool> ConfirmClearTempFolderWithOtherInstancesAsync();

    /// <summary>
    /// Tells the user that the temporary folder was cleared.
    /// </summary>
    Task ShowTempFolderClearedAsync();

    /// <summary>
    /// Tells the user that the temporary folder could not be cleared.
    /// </summary>
    Task ShowTempFolderClearFailedAsync();

    /// <summary>
    /// Explains that a partition can't be loaded because it or its file system is not supported.
    /// </summary>
    Task ShowPartitionNotSupportedAsync();
}
