namespace TotalImage.ViewModels.Services;

/// <summary>
/// Opens things outside the application with the operating system's default handler.
/// </summary>
public interface ILauncherService
{
    /// <summary>
    /// Opens a web page in the default browser.
    /// </summary>
    void OpenUrl(string url);

    /// <summary>
    /// Opens a folder in the system file manager.
    /// </summary>
    void OpenFolder(string path);

    /// <summary>
    /// Shows a file in the system file manager: its folder opens with the file selected where
    /// the platform supports it.
    /// </summary>
    void RevealFile(string path);
}
