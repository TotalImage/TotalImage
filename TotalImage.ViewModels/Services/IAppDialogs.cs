using System.Threading.Tasks;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Application-level dialogs.
/// </summary>
public interface IAppDialogs
{
    /// <summary>
    /// Shows information about the application.
    /// </summary>
    Task ShowAboutAsync();

    /// <summary>
    /// Lets the user edit the application settings. Accepting saves them.
    /// </summary>
    /// <returns>What changed, or <c>null</c> when cancelled. Clearing the recent images list
    /// and resetting to defaults take effect even when cancelled.</returns>
    Task<SettingsChangeResult?> ShowSettingsAsync();
}
