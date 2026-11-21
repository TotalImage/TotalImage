using System.Threading.Tasks;

namespace TotalImage.ViewModels.Main;

/// <summary>
/// TEMPORARY: the work of the commands that act on the folder tree, the file list and the
/// selected entries. <see cref="MainViewModel"/> owns the commands and their enabled rules, but
/// until browsing (stage 5) and editing (stage 6) have view models, the main window does the work.
/// </summary>
public interface IMainViewBridge
{
    // Stage 5: browsing

    void SelectAll();

    void GoToParentFolder();

    void ExpandFolderTree();

    void CollapseFolderTree();

    // Stage 6: editing

    Task InjectFilesAsync();

    Task ExtractAsync();

    Task CreateFolderAsync();

    Task DeleteAsync();

    void Rename();

    Task ShowPropertiesAsync();
}
