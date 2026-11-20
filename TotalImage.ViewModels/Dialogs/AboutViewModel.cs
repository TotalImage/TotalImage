using System.Linq;
using System.Reflection;
using CommunityToolkit.Mvvm.Input;
using TotalImage.ViewModels.Services;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// The contents of the About dialog.
/// </summary>
public partial class AboutViewModel
{
    /// <summary>
    /// The name of the current release.
    /// </summary>
    public const string ReleaseName = "Alpha 1";

    /// <summary>
    /// The project's home page.
    /// </summary>
    public const string ProjectUrl = "https://github.com/TotalImage/TotalImage";

    private readonly ILauncherService launcher;

    /// <param name="launcher">Opens the project page.</param>
    /// <param name="applicationAssembly">The assembly whose version is shown, normally the entry assembly.</param>
    public AboutViewModel(ILauncherService launcher, Assembly? applicationAssembly)
    {
        this.launcher = launcher;

        var name = applicationAssembly?.GetName();
        var hash = applicationAssembly?.GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == "GitHash")?.Value;
        VersionText = $"Version: {name?.Version} {ReleaseName} ({hash})";
    }

    /// <summary>
    /// The version line: version, release name and commit hash.
    /// </summary>
    public string VersionText { get; }

    /// <summary>
    /// Whether the project link was opened.
    /// </summary>
    public bool ProjectPageVisited { get; private set; }

    /// <summary>
    /// Opens <see cref="ProjectUrl"/> in the browser.
    /// </summary>
    [RelayCommand]
    private void OpenProjectPage()
    {
        ProjectPageVisited = true;
        launcher.OpenUrl(ProjectUrl);
    }
}
