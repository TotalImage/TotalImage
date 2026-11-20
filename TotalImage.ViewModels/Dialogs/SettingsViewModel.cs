using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// What changed when the settings dialog was accepted. The settings themselves are already
/// saved; the caller reads them from <see cref="ISettingsService.Current"/> to re-sync its UI.
/// </summary>
/// <param name="ColorThemeChanged">The color theme differs from the one in use when the dialog opened.</param>
public sealed record SettingsChangeResult(bool ColorThemeChanged);

/// <summary>
/// Edits a copy of the application settings. Accepting writes the copy back to
/// <see cref="ISettingsService.Current"/> and saves it. Clearing the recent images list and
/// resetting to defaults act on the settings immediately, as they always have.
/// </summary>
public partial class SettingsViewModel : DialogViewModel<SettingsChangeResult>
{
    /// <summary>
    /// The number of bytes in one unit of <see cref="MemoryMappingThresholdMiB"/>.
    /// </summary>
    public const long BytesPerMiB = 1048576;

    private readonly ISettingsService settings;
    private readonly ITempDirectoryService tempDirectory;
    private readonly IAppInstances instances;
    private readonly IMessageService messages;
    private readonly ILauncherService launcher;
    private readonly IFilePickerService filePicker;
    private readonly ColorTheme originalColorTheme;

    public SettingsViewModel(ISettingsService settings, ITempDirectoryService tempDirectory, IAppInstances instances,
        IMessageService messages, ILauncherService launcher, IFilePickerService filePicker)
    {
        this.settings = settings;
        this.tempDirectory = tempDirectory;
        this.instances = instances;
        this.messages = messages;
        this.launcher = launcher;
        this.filePicker = filePicker;
        originalColorTheme = settings.Current.ColorMode;

        LoadFromSettings();
    }

    /// <summary>
    /// How the file list presents its entries.
    /// </summary>
    [ObservableProperty]
    public partial ViewMode FilesView { get; set; }

    /// <summary>
    /// The index of the column the file list is sorted by.
    /// </summary>
    [ObservableProperty]
    public partial int SortColumn { get; set; }

    /// <summary>
    /// The direction the file list is sorted in.
    /// </summary>
    [ObservableProperty]
    public partial SortDirection SortDirection { get; set; }

    [ObservableProperty]
    public partial bool ShowHiddenItems { get; set; }

    [ObservableProperty]
    public partial bool ShowCommandBar { get; set; }

    [ObservableProperty]
    public partial bool ShowDirectoryTree { get; set; }

    [ObservableProperty]
    public partial bool ShowStatusBar { get; set; }

    [ObservableProperty]
    public partial bool ShowDirectorySizes { get; set; }

    /// <summary>
    /// The unit sizes are shown in.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SizeUnitPreview))]
    public partial SizeUnit SizeUnit { get; set; }

    [ObservableProperty]
    public partial ColorTheme ColorTheme { get; set; }

    /// <summary>
    /// Ask for the extraction options every time. While set, the defaults below can't be edited.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanEditExtractionDefaults))]
    public partial bool ExtractAlwaysAsk { get; set; }

    /// <summary>
    /// The default folder to extract to.
    /// </summary>
    [ObservableProperty]
    public partial string ExtractPath { get; set; }

    /// <summary>
    /// What to do with folders when extracting.
    /// </summary>
    [ObservableProperty]
    public partial DirectoryExtractionMode DirectoryExtractionMode { get; set; }

    [ObservableProperty]
    public partial bool OpenFolderAfterExtract { get; set; }

    [ObservableProperty]
    public partial bool PreserveDates { get; set; }

    [ObservableProperty]
    public partial bool PreserveAttributes { get; set; }

    [ObservableProperty]
    public partial bool QueryShellForFileTypeInfo { get; set; }

    [ObservableProperty]
    public partial bool ConfirmInjection { get; set; }

    [ObservableProperty]
    public partial bool ConfirmDeletion { get; set; }

    [ObservableProperty]
    public partial bool ConfirmOverwriteExtraction { get; set; }

    [ObservableProperty]
    public partial bool AutoIncrementFilename { get; set; }

    /// <summary>
    /// Images at least this large (in MiB) are memory-mapped when opened.
    /// </summary>
    [ObservableProperty]
    public partial decimal MemoryMappingThresholdMiB { get; set; }

    /// <summary>
    /// The default extraction folder, folder mode and "open folder" option can only be edited
    /// while the options aren't asked for every time.
    /// </summary>
    public bool CanEditExtractionDefaults => !ExtractAlwaysAsk;

    /// <summary>
    /// A few sample sizes formatted in <see cref="SizeUnit"/>.
    /// </summary>
    public string SizeUnitPreview =>
        $"Preview:{Environment.NewLine}{SizeUnit.FormatSize(234)} | " +
        $"{SizeUnit.FormatSize(2345)} | {SizeUnit.FormatSize(2345678)} | " +
        $"{SizeUnit.FormatSize(2345678901)} | {SizeUnit.FormatSize(2345678901234)}";

    /// <summary>
    /// Clears the list of recently opened images right away and says so.
    /// </summary>
    [RelayCommand]
    private async Task ClearRecentImagesAsync()
    {
        settings.ClearRecentImages();
        await messages.ShowRecentImagesClearedAsync();
    }

    /// <summary>
    /// After confirmation, resets all settings to their defaults, saves them right away and
    /// shows the defaults in the dialog.
    /// </summary>
    [RelayCommand]
    private async Task ResetToDefaultsAsync()
    {
        if (!await messages.ConfirmResetSettingsAsync())
            return;

        settings.ResetToDefaults();
        settings.Save();
        await messages.ShowSettingsResetAsync();
        LoadFromSettings();
    }

    /// <summary>
    /// Deletes everything in the temporary folder, asking first when other instances may be
    /// using it, and reports the outcome.
    /// </summary>
    [RelayCommand]
    private async Task ClearTempFolderAsync()
    {
        if (instances.AreOtherInstancesRunning() && !await messages.ConfirmClearTempFolderWithOtherInstancesAsync())
            return;

        try
        {
            tempDirectory.Clear();
        }
        catch (Exception)
        {
            await messages.ShowTempFolderClearFailedAsync();
            return;
        }

        await messages.ShowTempFolderClearedAsync();
    }

    /// <summary>
    /// Opens the temporary folder in the file manager, creating it first if needed.
    /// </summary>
    [RelayCommand]
    private void OpenTempFolder()
    {
        tempDirectory.EnsureRootExists();
        launcher.OpenFolder(tempDirectory.Root);
    }

    /// <summary>
    /// Asks for the default extraction folder.
    /// </summary>
    [RelayCommand]
    private async Task BrowseExtractPathAsync()
    {
        var path = await filePicker.PickFolderAsync(ExtractPath);
        if (path is not null)
            ExtractPath = path;
    }

    /// <summary>
    /// Writes the edited values to the settings and saves them.
    /// </summary>
    protected override SettingsChangeResult CreateAnswer()
    {
        var current = settings.Current;
        current.FilesView = FilesView;
        current.FilesSortingColumn = SortColumn;
        current.FilesSortOrder = SortDirection;
        current.ShowHiddenItems = ShowHiddenItems;
        current.ShowCommandBar = ShowCommandBar;
        current.ShowDirectoryTree = ShowDirectoryTree;
        current.ShowStatusBar = ShowStatusBar;
        current.FileListShowDirSize = ShowDirectorySizes;
        current.SizeUnit = SizeUnit;
        current.ColorMode = ColorTheme;
        current.ExtractAlwaysAsk = ExtractAlwaysAsk;
        current.DefaultExtractPath = ExtractPath;
        current.DefaultDirectoryExtractionMode = DirectoryExtractionMode;
        current.OpenFolderAfterExtract = OpenFolderAfterExtract;
        current.ExtractPreserveDates = PreserveDates;
        current.ExtractPreserveAttributes = PreserveAttributes;
        current.QueryShellForFileTypeInfo = QueryShellForFileTypeInfo;
        current.ConfirmInjection = ConfirmInjection;
        current.ConfirmDeletion = ConfirmDeletion;
        current.ConfirmOverwriteExtraction = ConfirmOverwriteExtraction;
        current.AutoIncrementFilename = AutoIncrementFilename;
        current.MemoryMappingThreshold = (long)MemoryMappingThresholdMiB * BytesPerMiB;
        settings.Save();

        return new SettingsChangeResult(ColorTheme != originalColorTheme);
    }

    [MemberNotNull(nameof(ExtractPath))]
    private void LoadFromSettings()
    {
        var current = settings.Current;
        FilesView = current.FilesView;
        SortColumn = current.FilesSortingColumn;
        SortDirection = current.FilesSortOrder;
        ShowHiddenItems = current.ShowHiddenItems;
        ShowCommandBar = current.ShowCommandBar;
        ShowDirectoryTree = current.ShowDirectoryTree;
        ShowStatusBar = current.ShowStatusBar;
        ShowDirectorySizes = current.FileListShowDirSize;
        SizeUnit = current.SizeUnit;
        ColorTheme = current.ColorMode;
        ExtractAlwaysAsk = current.ExtractAlwaysAsk;
        ExtractPath = current.DefaultExtractPath;
        DirectoryExtractionMode = current.DefaultDirectoryExtractionMode;
        OpenFolderAfterExtract = current.OpenFolderAfterExtract;
        PreserveDates = current.ExtractPreserveDates;
        PreserveAttributes = current.ExtractPreserveAttributes;
        QueryShellForFileTypeInfo = current.QueryShellForFileTypeInfo;
        ConfirmInjection = current.ConfirmInjection;
        ConfirmDeletion = current.ConfirmDeletion;
        ConfirmOverwriteExtraction = current.ConfirmOverwriteExtraction;
        AutoIncrementFilename = current.AutoIncrementFilename;
        MemoryMappingThresholdMiB = current.MemoryMappingThreshold / BytesPerMiB;
    }
}
