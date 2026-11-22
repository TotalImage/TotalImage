using System.Threading.Tasks;

namespace TotalImage.ViewModels.Main;

/// <summary>
/// TEMPORARY: the work of the commands that change or extract the selected entries.
/// <see cref="MainViewModel"/> owns the commands and their enabled rules, but until editing
/// (stage 6) has view models, the main window does the work, on <see cref="MainViewModel.SelectedObjects"/>,
/// <see cref="MainViewModel.CurrentFolder"/> and <see cref="MainViewModel.ActivePane"/>.
/// </summary>
public interface IMainViewBridge
{
    Task InjectFilesAsync();

    Task ExtractAsync();

    Task CreateFolderAsync();

    Task DeleteAsync();

    void Rename();

    Task ShowPropertiesAsync();
}
