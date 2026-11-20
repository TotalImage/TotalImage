using System.Threading.Tasks;

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
}
