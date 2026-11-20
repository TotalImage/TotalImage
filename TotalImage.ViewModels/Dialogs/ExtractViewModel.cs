using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// Where and how to extract the selected entries.
/// </summary>
/// <param name="TargetPath">The folder to extract into. It exists when the answer is given.</param>
/// <param name="DirectoryExtractionMode">What to do with selected directories.</param>
/// <param name="OpenFolder">Open <paramref name="TargetPath"/> after extracting.</param>
public sealed record ExtractOptions(string TargetPath, DirectoryExtractionMode DirectoryExtractionMode, bool OpenFolder);

/// <summary>
/// Asks where and how to extract the selected entries. Starts from the defaults in the settings.
/// </summary>
public partial class ExtractViewModel : DialogViewModel<ExtractOptions>
{
    private readonly IFilePickerService filePicker;
    private readonly IMessageService messages;

    /// <param name="itemCount">The number of selected entries, shown in <see cref="Prompt"/>.</param>
    public ExtractViewModel(int itemCount, ISettingsService settings, IFilePickerService filePicker, IMessageService messages)
    {
        this.filePicker = filePicker;
        this.messages = messages;

        Prompt = $"Extract {itemCount} selected {(itemCount > 1 ? "items" : "item")} to the following folder:";
        TargetPath = settings.Current.DefaultExtractPath;
        OpenFolder = settings.Current.OpenFolderAfterExtract;
        DirectoryExtractionMode = settings.Current.DefaultDirectoryExtractionMode;
    }

    /// <summary>
    /// The text above the target folder, for example "Extract 2 selected items to the following folder:".
    /// </summary>
    public string Prompt { get; }

    /// <summary>
    /// The folder to extract into.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanExtract))]
    public partial string TargetPath { get; set; }

    /// <summary>
    /// Whether to open the target folder after extracting.
    /// </summary>
    [ObservableProperty]
    public partial bool OpenFolder { get; set; }

    /// <summary>
    /// What to do with selected directories.
    /// </summary>
    [ObservableProperty]
    public partial DirectoryExtractionMode DirectoryExtractionMode { get; set; }

    /// <summary>
    /// Whether the OK button is enabled: a target folder has been entered.
    /// </summary>
    public bool CanExtract => CanAccept();

    /// <summary>
    /// Lets the user pick the target folder.
    /// </summary>
    [RelayCommand]
    private async Task BrowseAsync()
    {
        var folder = await filePicker.PickFolderAsync();
        if (folder is not null)
            TargetPath = folder;
    }

    /// <summary>
    /// Accepts the dialog when the target folder exists, or when the user agrees to create it
    /// (it is then created). Returns whether the dialog was accepted; when it was not, the dialog
    /// stays open.
    /// </summary>
    public async Task<bool> TryAcceptAsync()
    {
        if (!CanAccept())
            return false;

        if (!Directory.Exists(TargetPath))
        {
            if (!await messages.ConfirmCreateExtractFolderAsync(TargetPath))
                return false;

            Directory.CreateDirectory(TargetPath);
        }

        AcceptCommand.Execute(null);
        return true;
    }

    partial void OnTargetPathChanged(string value) => RefreshCanAccept();

    protected override bool CanAccept() => !string.IsNullOrWhiteSpace(TargetPath);

    protected override ExtractOptions CreateAnswer() => new(TargetPath, DirectoryExtractionMode, OpenFolder);
}
