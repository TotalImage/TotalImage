using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TotalImage.Containers;
using TotalImage.FileSystems;
using TotalImage.FileSystems.BPB;
using TotalImage.FileSystems.FAT;
using TotalImage.FileSystems.RAW;
using TotalImage.Partitions;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;
using Directory = TotalImage.FileSystems.Directory;
using File = System.IO.File;

namespace TotalImage.ViewModels.Main;

/// <summary>
/// The main window: the open image (an "image session"), its file, unsaved changes and current
/// partition, the window title and status bar, and every menu and toolbar command with its
/// enabled rule.
/// </summary>
public sealed partial class MainViewModel : ObservableObject, IDisposable
{
    /// <summary>
    /// The application name shown in the window title.
    /// </summary>
    public const string AppName = "TotalImage";

    // Only raw sector images can be written so far. Saving under another extension does nothing.
    private static readonly string[] WritableImageExtensions = [".img", ".ima", ".vfd", ".flp", ".dsk", ".hdm"];

    private readonly ISettingsService settings;
    private readonly IImageFileService imageFiles;
    private readonly IImageDialogs imageDialogs;
    private readonly IEntryDialogs entryDialogs;
    private readonly IAppDialogs appDialogs;
    private readonly IMessageService messages;
    private readonly IFilePickerService filePicker;
    private readonly ILauncherService launcher;
    private readonly IFileTypeInfo fileTypes;
    private readonly ITempDirectoryService tempDirectory;
    private readonly IFileExtractionService extraction;

    // The last file name saved with "Save as" that ends with a number, offered incremented next time.
    private string? lastSavedFileName;

    // The status bar size text of the current folder, kept because it walks the whole folder.
    private Directory? cachedSizeDirectory;
    private string cachedSizeText = string.Empty;

    public MainViewModel(ISettingsService settings, IImageFileService imageFiles, IImageDialogs imageDialogs,
        IEntryDialogs entryDialogs, IAppDialogs appDialogs, IMessageService messages, IFilePickerService filePicker,
        ILauncherService launcher, IFileTypeInfo fileTypes, ITempDirectoryService tempDirectory, IFileExtractionService extraction)
    {
        this.settings = settings;
        this.imageFiles = imageFiles;
        this.imageDialogs = imageDialogs;
        this.entryDialogs = entryDialogs;
        this.appDialogs = appDialogs;
        this.messages = messages;
        this.filePicker = filePicker;
        this.launcher = launcher;
        this.fileTypes = fileTypes;
        this.tempDirectory = tempDirectory;
        this.extraction = extraction;

        ReadViewSettings();
        RecentImages = ReadRecentImages();
    }

    /// <summary>
    /// Raised when the window should close, after <see cref="ExitCommand"/> confirmed it.
    /// </summary>
    public event EventHandler? CloseRequested;

    /// <summary>
    /// Raised after the settings dialog was accepted and the settings were applied here, so the
    /// view can refresh what depends on them (icons, file list).
    /// </summary>
    public event EventHandler? SettingsApplied;

    /// <summary>
    /// TEMPORARY (stage 6): does the work of the editing commands.
    /// </summary>
    public IMainViewBridge? View { get; set; }

    #region Image session

    /// <summary>
    /// The open image, or <c>null</c>.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsImageOpen), nameof(Title))]
    public partial Container? Image { get; private set; }

    public bool IsImageOpen => Image is not null;

    /// <summary>
    /// The file of the open image, or <c>null</c> when it hasn't been saved yet.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FileName), nameof(Title))]
    public partial string? FilePath { get; private set; }

    /// <summary>
    /// The file name of the open image without its folder, or <c>null</c> when it hasn't been saved yet.
    /// </summary>
    public string? FileName => FilePath is null ? null : Path.GetFileName(FilePath);

    /// <summary>
    /// Whether the open image has changes that haven't been saved.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    public partial bool HasUnsavedChanges { get; private set; }

    /// <summary>
    /// Whether an image is being opened or saved.
    /// </summary>
    [ObservableProperty]
    public partial bool IsBusy { get; private set; }

    /// <summary>
    /// The partitions of the open image that can be loaded.
    /// </summary>
    [ObservableProperty]
    public partial IReadOnlyList<PartitionOption> Partitions { get; private set; } = [];

    /// <summary>
    /// The index of the loaded partition in the partition table.
    /// </summary>
    [ObservableProperty]
    public partial int CurrentPartitionIndex { get; private set; }

    /// <summary>
    /// The file system of the loaded partition, or <c>null</c> when no image is open.
    /// </summary>
    [ObservableProperty]
    public partial FileSystem? CurrentFileSystem { get; private set; }

    /// <summary>
    /// The loaded partition, or <c>null</c> when no image is open.
    /// </summary>
    public PartitionEntry? CurrentPartition => Image?.PartitionTable.Partitions[CurrentPartitionIndex];

    /// <summary>
    /// The window title: the file name (with a * when there are unsaved changes) and the app name.
    /// </summary>
    public string Title => Image is null
        ? AppName
        : $"{(HasUnsavedChanges ? "*" : string.Empty)}{FileName ?? "(Untitled)"} - {AppName}";

    /// <summary>
    /// The recently opened images, most recent first.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasRecentImages))]
    public partial IReadOnlyList<string> RecentImages { get; private set; }

    public bool HasRecentImages => RecentImages.Count > 0;

    /// <summary>
    /// Whether closing the window was confirmed, so it can close without asking again.
    /// </summary>
    public bool IsCloseConfirmed { get; private set; }

    /// <summary>
    /// Opens an image file, for example one passed on the command line or dropped on the window.
    /// Asks about unsaved changes first.
    /// </summary>
    /// <returns>Whether the image was opened.</returns>
    public async Task<bool> OpenImageFileAsync(string path)
    {
        if (!await ConfirmLosingChangesAsync(UnsavedChangesReason.OpenImage))
            return false;

        CloseCurrentImage();
        return await LoadAsync(path);
    }

    /// <summary>
    /// Loads another partition of the open image.
    /// </summary>
    public void SelectPartition(int index)
    {
        if (Image is null || index == CurrentPartitionIndex || index < 0 || index >= Image.PartitionTable.Partitions.Count)
            return;

        CurrentPartitionIndex = index;
        CurrentFileSystem = Image.PartitionTable.Partitions[index].FileSystem;
        UpdateCapacity();
        RefreshCommands();
    }

    /// <summary>
    /// Asks about unsaved changes before the window closes, unless that was already confirmed.
    /// </summary>
    /// <returns>Whether the window may close.</returns>
    public async Task<bool> ConfirmCloseWindowAsync()
    {
        if (IsCloseConfirmed)
            return true;

        IsCloseConfirmed = await ConfirmLosingChangesAsync(UnsavedChangesReason.ExitApplication);
        return IsCloseConfirmed;
    }

    [RelayCommand(CanExecute = nameof(IsIdle))]
    private async Task NewImage()
    {
        var options = await imageDialogs.AskNewImageAsync();
        if (options is null || !await ConfirmLosingChangesAsync(UnsavedChangesReason.NewImage))
            return;

        CloseCurrentImage();

        BiosParameterBlock bpb = options.BpbVersion is BiosParameterBlockVersion.Dos34 or BiosParameterBlockVersion.Dos40
            ? ExtendedBiosParameterBlock.FromGeometry(options.Geometry, options.BpbVersion, options.OemId, options.SerialNumber, options.FileSystemType, options.VolumeLabel)
            : BiosParameterBlock.FromGeometry(options.Geometry, options.BpbVersion, options.OemId);

        await LoadAsync(null, RawContainer.CreateImage(bpb, options.Geometry.Tracks, options.WriteBpb));
    }

    [RelayCommand(CanExecute = nameof(IsIdle))]
    private async Task OpenImage()
    {
        if (!await ConfirmLosingChangesAsync(UnsavedChangesReason.OpenImage))
            return;

        var path = await filePicker.PickImageToOpenAsync();
        if (path is null)
            return;

        CloseCurrentImage();
        await LoadAsync(path);
    }

    [RelayCommand(CanExecute = nameof(IsIdle))]
    private async Task OpenRecentImage(string path)
    {
        if (!File.Exists(path))
        {
            await messages.ShowRecentImageMissingAsync(Path.GetFileName(path));
            settings.RemoveRecentImage(path);
            RecentImages = ReadRecentImages();
            return;
        }

        if (!await ConfirmLosingChangesAsync(UnsavedChangesReason.OpenImage))
            return;

        CloseCurrentImage();
        await LoadAsync(path);
    }

    /// <summary>
    /// Saves the image to its file, or asks for one when it hasn't been saved yet.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanSave))]
    private Task Save() => SaveCoreAsync(saveAs: false);

    [RelayCommand(CanExecute = nameof(CanSaveAs))]
    private Task SaveAs() => SaveCoreAsync(saveAs: true);

    [RelayCommand(CanExecute = nameof(HasImageFile))]
    private async Task ReloadImage()
    {
        if (!await ConfirmLosingChangesAsync(UnsavedChangesReason.ReloadImage))
            return;

        var path = FilePath!;
        CloseCurrentImage();
        await LoadAsync(path);
    }

    [RelayCommand(CanExecute = nameof(CanUseImage))]
    private async Task CloseImage()
    {
        if (await ConfirmLosingChangesAsync(UnsavedChangesReason.CloseImage))
            CloseCurrentImage();
    }

    [RelayCommand(CanExecute = nameof(HasImageFile))]
    private void OpenContainingFolder() => launcher.RevealFile(FilePath!);

    [RelayCommand]
    private async Task Exit()
    {
        if (await ConfirmCloseWindowAsync())
            CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private bool IsIdle() => !IsBusy;

    private bool CanUseImage() => Image is not null && !IsBusy;

    private bool CanEditImage() => CanUseImage() && Image!.SupportsWriting;

    private bool CanSave() => CanEditImage() && HasUnsavedChanges;

    private bool CanSaveAs() => CanEditImage();

    private bool HasImageFile() => CanUseImage() && FilePath is not null;

    /// <summary>
    /// Asks whether to save unsaved changes, and saves them when asked to.
    /// </summary>
    /// <returns>Whether to go on: there were no changes, they were saved, or they may be discarded.</returns>
    private async Task<bool> ConfirmLosingChangesAsync(UnsavedChangesReason reason)
    {
        if (!HasUnsavedChanges)
            return true;

        return await messages.AskUnsavedChangesAsync(reason) switch
        {
            UnsavedChangesAnswer.Save => await SaveCoreAsync(saveAs: false),
            UnsavedChangesAnswer.Discard => true,
            _ => false
        };
    }

    /// <returns>Whether the image was saved (or a file name that can't be written was picked).</returns>
    private async Task<bool> SaveCoreAsync(bool saveAs)
    {
        if (Image is not { SupportsWriting: true } image)
            return false;

        var path = FilePath;
        if (saveAs || path is null)
        {
            path = await filePicker.PickImageSaveLocationAsync(SuggestFileName());
            if (path is null)
                return false;

            // Other formats can't be written yet; picking one does nothing, as it always has.
            if (!WritableImageExtensions.Contains(Path.GetExtension(path).ToLowerInvariant()))
                return true;

            saveAs = true;
        }

        var partitionIndex = CurrentPartitionIndex;
        image.PendingChanges.Changed -= OnPendingChangesChanged;
        IsBusy = true;
        try
        {
            await imageFiles.SaveAsync(image, path);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            image.PendingChanges.Changed += OnPendingChangesChanged;
            IsBusy = false;
            await messages.ShowErrorAsync($"Saving \"{Path.GetFileName(path)}\"", ex);
            return false;
        }

        IsBusy = false;

        if (saveAs)
        {
            var fileName = Path.GetFileName(path);
            lastSavedFileName = settings.Current.AutoIncrementFilename && TrailingNumber().IsMatch(Path.GetFileNameWithoutExtension(fileName))
                ? fileName
                : null;
            settings.AddRecentImage(path);
            RecentImages = ReadRecentImages();
        }

        // Saving released the container, so open the saved file again, on the same partition.
        CloseCurrentImage();
        return await LoadAsync(path, partitionIndex: partitionIndex);
    }

    /// <summary>
    /// The last name saved with "Save as", with its trailing number incremented (keeping its
    /// zero padding and dropping the extension), or <c>null</c>.
    /// </summary>
    private string? SuggestFileName()
    {
        if (lastSavedFileName is null)
            return null;

        var name = Path.GetFileNameWithoutExtension(lastSavedFileName);
        var number = TrailingNumber().Match(name).Value;
        if (!long.TryParse(number, out var value))
            return null;

        return name[..^number.Length] + (value + 1).ToString(new string('0', number.Length));
    }

    [GeneratedRegex(@"\d+$")]
    private static partial Regex TrailingNumber();

    /// <summary>
    /// Opens an image file, or takes a newly created image, and loads a partition of it.
    /// Explains the problem when it can't be loaded.
    /// </summary>
    /// <param name="path">The file to open, or <c>null</c> for a newly created image.</param>
    /// <param name="created">The newly created image, when <paramref name="path"/> is <c>null</c>.</param>
    /// <param name="partitionIndex">The partition to load without asking, when the image has several.</param>
    private async Task<bool> LoadAsync(string? path, Container? created = null, int? partitionIndex = null)
    {
        Container image;
        string fileName = path is null ? string.Empty : Path.GetFileName(path);

        if (created is not null)
        {
            image = created;
        }
        else
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                await messages.ShowCannotOpenImageAsync(new(ImageOpenProblem.InvalidPath, path ?? string.Empty));
                return false;
            }

            IsBusy = true;
            try
            {
                image = await imageFiles.OpenAsync(path);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await messages.ShowCannotOpenImageAsync(DescribeOpenFailure(ex, fileName));
                return false;
            }

            IsBusy = false;
            settings.AddRecentImage(path);
            RecentImages = ReadRecentImages();
        }

        var index = await ChoosePartitionAsync(image, fileName, partitionIndex);
        if (index is null)
        {
            image.Dispose();
            return false;
        }

        Image = image;
        FilePath = path;
        HasUnsavedChanges = created is not null;
        Partitions = CreatePartitionOptions(image);
        CurrentPartitionIndex = index.Value;
        image.PendingChanges.Changed += OnPendingChangesChanged;
        CurrentFileSystem = image.PartitionTable.Partitions[index.Value].FileSystem;
        UpdateCapacity();
        UpdateSelectionStatus();
        RefreshCommands();
        return true;
    }

    private static ImageOpenFailure DescribeOpenFailure(Exception exception, string fileName) => exception switch
    {
        EmptyImageFileException => new(ImageOpenProblem.EmptyFile, fileName),
        FileNotFoundException => new(ImageOpenProblem.FileNotFound, fileName),
        IOException when (exception.HResult & 0x0000FFFF) == 32 => new(ImageOpenProblem.FileLocked, fileName),
        UnauthorizedAccessException => new(ImageOpenProblem.AccessDenied, fileName),
        _ => new(ImageOpenProblem.ReadFailed, fileName, ErrorMessage: exception.Message)
    };

    /// <summary>
    /// Picks the partition to load: the only one, the given one, or the one the user selects
    /// when there are several. Explains the problem when none can be loaded.
    /// </summary>
    /// <returns>The partition index, or <c>null</c> when nothing can or should be loaded.</returns>
    private async Task<int?> ChoosePartitionAsync(Container image, string fileName, int? preferredIndex)
    {
        var table = image.PartitionTable;
        var partitions = table.Partitions;

        if (table is NoPartitionTable)
        {
            if (IsUnsupported(partitions[0]))
            {
                await messages.ShowCannotOpenImageAsync(new(ImageOpenProblem.NoSupportedFileSystem, fileName, ContainerName: image.DisplayName));
                return null;
            }

            return 0;
        }

        if (partitions.Count == 0)
        {
            await messages.ShowCannotOpenImageAsync(new(ImageOpenProblem.NoPartitions, fileName, PartitionTableName: table.DisplayName));
            return null;
        }

        if (partitions.Count > 1)
        {
            //If there's multiple partitions in a supported partition table, and they're all unsupported types, for now we just back out.
            //Once we implement partition management (soon™), we can offer that to the user.
            if (partitions.All(IsUnsupported))
            {
                await messages.ShowCannotOpenImageAsync(new(ImageOpenProblem.NoSupportedPartitions, fileName, PartitionTableName: table.DisplayName));
                return null;
            }

            if (preferredIndex is int preferred && preferred < partitions.Count && !IsUnsupported(partitions[preferred]))
                return preferred;

            var selection = await imageDialogs.AskSelectPartitionAsync(image);
            return selection?.Index;
        }

        if (IsUnsupported(partitions[0]))
        {
            await messages.ShowCannotOpenImageAsync(new(ImageOpenProblem.UnsupportedPartition, fileName, PartitionTableName: table.DisplayName));
            return null;
        }

        return 0;
    }

    private static bool IsUnsupported(PartitionEntry partition)
    {
        try
        {
            return partition.FileSystem is RawFileSystem;
        }
        catch (InvalidDataException)
        {
            return true;
        }
    }

    private List<PartitionOption> CreatePartitionOptions(Container image)
    {
        var table = image.PartitionTable;
        var unit = settings.Current.SizeUnit;
        var options = new List<PartitionOption>();
        for (int i = 0; i < table.Partitions.Count; i++)
        {
            var partition = table.Partitions[i];
            try
            {
                //Skip RAW since it can't be loaded anyway
                if (table is not NoPartitionTable && partition.FileSystem is RawFileSystem)
                    continue;

                var label = partition.FileSystem.VolumeLabel.TrimEnd(' ');
                var prefix = table is not NoPartitionTable && table.Partitions.Count > 1 ? $"{i}: " : string.Empty;
                options.Add(new(i, $"{prefix}{label} ({partition.FileSystem.DisplayName}, {unit.FormatSize((ulong)partition.Length)})"));
            }
            catch (InvalidDataException)
            {
            }

            if (table is NoPartitionTable)
                break;
        }

        return options;
    }

    private void OnPendingChangesChanged(object? sender, EventArgs e)
    {
        HasUnsavedChanges = Image?.PendingChanges.IsDirty ?? false;
        ReloadFolders();
    }

    /// <summary>
    /// Closes the open image without asking, dropping its unsaved changes.
    /// </summary>
    private void CloseCurrentImage()
    {
        if (Image is { } image)
        {
            image.PendingChanges.Changed -= OnPendingChangesChanged;
            image.Dispose();
        }

        Image = null;
        FilePath = null;
        HasUnsavedChanges = false;
        CurrentFileSystem = null;
        CurrentPartitionIndex = 0;
        Partitions = [];
        UpdateCapacity();
        UpdateSelectionStatus();
        RefreshCommands();
    }

    partial void OnHasUnsavedChangesChanged(bool value) => RefreshCommands();

    partial void OnIsBusyChanged(bool value) => RefreshCommands();

    /// <summary>
    /// Releases the open image. Raises no change notifications: the view may already be gone.
    /// </summary>
    public void Dispose()
    {
        if (Image is { } image)
        {
            image.PendingChanges.Changed -= OnPendingChangesChanged;
            image.Dispose();
        }
    }

    #endregion

    #region Image and partition tools

    [RelayCommand(CanExecute = nameof(CanEditImage))]
    private async Task ChangeVolumeLabel()
    {
        if (CurrentFileSystem is not FatFileSystem fs)
        {
            await messages.ShowFatOnlyFeatureAsync();
            return;
        }

        //TODO: Actually change the volume labels
        var change = await entryDialogs.AskVolumeLabelAsync(fs.RootDirectoryVolumeLabel ?? string.Empty, fs.BpbVolumeLabel ?? string.Empty);
        if (change is not null)
            fs.EnqueueSetVolumeLabel(change.RootDirectoryLabel);
    }

    /* Allows viewing and editing bootsector properties
     *
     * TODO: Enable this for other file systems/partition types/media too.
     */
    [RelayCommand(CanExecute = nameof(CanEditImage))]
    private async Task ShowBootSector()
    {
        if (CurrentFileSystem is not FatFileSystem)
        {
            await messages.ShowFatOnlyFeatureAsync();
            return;
        }

        await imageDialogs.ShowBootSectorAsync(Image!, CurrentPartitionIndex);
    }

    //Formats the selected partition/floppy disk
    //TODO: Implement this here and in FS/container.
    [RelayCommand(CanExecute = nameof(CanEditImage))]
    private async Task Format()
    {
        var options = await imageDialogs.AskFormatOptionsAsync();
        if (options is not null && await messages.ConfirmFormatAsync())
        {
            // Need to figure out how to actually do this, because right now it's unclear...
            throw new NotImplementedException();
        }
    }

    [RelayCommand(CanExecute = nameof(HasPartitionTable))]
    private async Task ChoosePartition()
    {
        var selection = await imageDialogs.AskSelectPartitionAsync(Image!);
        if (selection is not null)
            SelectPartition(selection.Index);
    }

    [RelayCommand(CanExecute = nameof(HasPartitionTable))]
    private Task ManagePartitions() => imageDialogs.ShowManagePartitionsAsync();

    [RelayCommand(CanExecute = nameof(CanUseImage))]
    private Task ShowImageInformation() => imageDialogs.ShowImageInfoAsync(Image!, FilePath, CurrentPartitionIndex);

    private bool HasPartitionTable() => CanUseImage() && Image!.PartitionTable is not NoPartitionTable;

    // Features that aren't built yet. Their commands stay disabled until they are.

    //TODO: The best UI for this still needs to be determined. Once that's done, implement the functionality here and in FS/container.
    [RelayCommand(CanExecute = nameof(IsBuilt))]
    private void ChangeFormat() { }

    //TODO: Implement this here and in FS/container.
    [RelayCommand(CanExecute = nameof(IsBuilt))]
    private void Defragment() { }

    [RelayCommand(CanExecute = nameof(IsBuilt))]
    private void ShowDeletedObjects() { }

    [RelayCommand(CanExecute = nameof(IsBuilt))]
    private void ShowHexView() { }

    [RelayCommand(CanExecute = nameof(IsBuilt))]
    private void ShowNotifications() { }

    private bool IsBuilt() => false;

    #endregion

    #region Entry commands (temporary bridge to the view)

    [RelayCommand(CanExecute = nameof(CanInjectFiles))]
    private Task InjectFiles() => View?.InjectFilesAsync() ?? Task.CompletedTask;

    /// <summary>
    /// Extracts the selected entries (everything in the folder when nothing is selected), or the
    /// folder selected in the tree (its entries for the root folder), asking where first when the
    /// settings say so.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExtract))]
    private async Task Extract()
    {
        // The pane the command acts on tells which items to extract.
        if (ActivePane == BrowsePane.Files)
        {
            if (SelectedObjects.Count == 0)
                SelectAll();
        }
        else if (CurrentFolder?.IsRoot == true) //Root dir is selected, so we have to handle this separately
        {
            SelectAll();
        }

        var fromFolderTree = ActivePane == BrowsePane.Folders;
        IReadOnlyList<FileSystemObject> items = fromFolderTree ? [CurrentFolder!.Directory] : SelectedObjects;
        var current = settings.Current;

        if (current.ExtractAlwaysAsk)
        {
            var options = await entryDialogs.AskExtractOptionsAsync(fromFolderTree ? 1 : items.Count);
            if (options is null)
                return;

            current.DefaultExtractPath = options.TargetPath;
            await ExtractToFolderAsync(items, options.TargetPath, options.DirectoryExtractionMode, options.OpenFolder);
        }
        else
        {
            await ExtractToFolderAsync(items, current.DefaultExtractPath, current.DefaultDirectoryExtractionMode, current.OpenFolderAfterExtract);
        }
    }

    /// <summary>
    /// Opens an entry of the file list: a folder becomes the current folder, and a file is
    /// extracted to a temporary folder and opened with its default program. Pending entries
    /// can't be opened yet.
    /// </summary>
    public async Task OpenEntryAsync(EntryViewModel entry)
    {
        if (TryOpenFolder(entry) || entry.IsPending || Image is null)
            return;

        var folder = tempDirectory.GetUniqueSubdirectoryPath();
        if (await ExtractEntriesAsync([entry.Entry], folder, DirectoryExtractionMode.Skip, askOverwrite: false))
            launcher.OpenFile(Path.Combine(folder, entry.Name));
    }

    private async Task ExtractToFolderAsync(IReadOnlyList<FileSystemObject> items, string targetPath, DirectoryExtractionMode mode, bool openFolder)
    {
        if (await ExtractEntriesAsync(items, targetPath, mode, askOverwrite: true) && openFolder)
            launcher.OpenFolder(targetPath);
    }

    /// <summary>
    /// Extracts entries while the progress window is shown, which keeps the image from being
    /// used for anything else meanwhile. Explains the problem when it fails.
    /// </summary>
    /// <param name="askOverwrite">Whether to ask before overwriting files (when the settings say so).</param>
    /// <returns>Whether everything was extracted: not cancelled and no error.</returns>
    private async Task<bool> ExtractEntriesAsync(IEnumerable<FileSystemObject> entries, string targetPath, DirectoryExtractionMode mode, bool askOverwrite)
    {
        var current = settings.Current;
        var options = new ExtractionOptions(targetPath, mode, current.ShowHiddenItems, current.ExtractPreserveDates, current.ExtractPreserveAttributes);
        Func<string, Task<OverwriteAnswer>>? ask = askOverwrite && current.ConfirmOverwriteExtraction ? messages.AskOverwriteAsync : null;

        var completed = false;
        try
        {
            await entryDialogs.ShowExtractionProgressAsync(async (progress, cancellationToken) =>
                completed = await extraction.ExtractAsync(entries, options, ask, progress, cancellationToken));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            await messages.ShowErrorAsync($"Extracting to \"{targetPath}\"", ex);
            return false;
        }

        return completed;
    }

    [RelayCommand(CanExecute = nameof(CanCreateFolder))]
    private Task NewFolder() => View?.CreateFolderAsync() ?? Task.CompletedTask;

    [RelayCommand(CanExecute = nameof(CanDelete))]
    private Task Delete() => View?.DeleteAsync() ?? Task.CompletedTask;

    [RelayCommand(CanExecute = nameof(CanRename))]
    private void Rename() => View?.Rename();

    [RelayCommand(CanExecute = nameof(CanShowProperties))]
    private Task ShowProperties() => View?.ShowPropertiesAsync() ?? Task.CompletedTask;

    private bool CanInjectFiles() => CanEditImage() && (CurrentPartition?.SupportsWriting ?? false);

    private bool CanCreateFolder() => CanEditImage() && CurrentFolder is not null;

    private bool CanExtract() => CanUseImage()
        && SelectionAllows(inFolderTree: CurrentFolder is not null, withNothingSelected: true, withSeveralSelected: true);

    private bool CanDelete() => CanEditImage() && SelectionAllowsChangingEntries(withSeveralSelected: true);

    private bool CanRename() => CanEditImage() && SelectionAllowsChangingEntries(withSeveralSelected: false);

    private bool CanShowProperties() => CanUseImage() && SelectionAllowsChangingEntries(withSeveralSelected: true);

    // Delete, rename and properties apply to the selected entries, or to the current folder
    // unless it is the root.
    private bool SelectionAllowsChangingEntries(bool withSeveralSelected)
    {
        var isSubfolder = CurrentFolder is { IsRoot: false };
        return SelectionAllows(inFolderTree: isSubfolder, withNothingSelected: isSubfolder, withSeveralSelected);
    }

    /// <summary>
    /// The enabled rule of a command that acts on the selection: in the folder tree, or in the
    /// file list with nothing selected, one entry selected (never ".." or a pending entry), or
    /// several entries selected.
    /// </summary>
    private bool SelectionAllows(bool inFolderTree, bool withNothingSelected, bool withSeveralSelected)
    {
        if (ActivePane == BrowsePane.Folders)
            return inFolderTree;

        var selected = SelectedEntries.Where(entry => !entry.IsParentEntry).ToList();
        return selected.Count switch
        {
            0 => !IsParentEntrySelected && withNothingSelected,
            1 => !selected[0].IsPending,
            _ => withSeveralSelected
        };
    }

    #endregion

    #region View options

    [ObservableProperty]
    public partial ViewMode ViewMode { get; private set; }

    [ObservableProperty]
    public partial bool ShowHiddenItems { get; private set; }

    [ObservableProperty]
    public partial bool ShowCommandBar { get; private set; }

    [ObservableProperty]
    public partial bool ShowDirectoryTree { get; private set; }

    [ObservableProperty]
    public partial bool ShowStatusBar { get; private set; }

    /// <summary>
    /// The column the file list is sorted by: 0 name, 1 type, 2 size, 3 modified, 4 attributes.
    /// </summary>
    [ObservableProperty]
    public partial int SortColumn { get; private set; }

    [ObservableProperty]
    public partial SortDirection SortDirection { get; private set; }

    [RelayCommand]
    private void SetViewMode(ViewMode mode)
    {
        ViewMode = mode;
        settings.Current.FilesView = mode;
    }

    [RelayCommand]
    private void ToggleShowHiddenItems()
    {
        ShowHiddenItems = !ShowHiddenItems;
        settings.Current.ShowHiddenItems = ShowHiddenItems;
    }

    [RelayCommand]
    private void ToggleCommandBar()
    {
        ShowCommandBar = !ShowCommandBar;
        settings.Current.ShowCommandBar = ShowCommandBar;
    }

    [RelayCommand]
    private void ToggleDirectoryTree()
    {
        ShowDirectoryTree = !ShowDirectoryTree;
        settings.Current.ShowDirectoryTree = ShowDirectoryTree;
    }

    [RelayCommand]
    private void ToggleStatusBar()
    {
        ShowStatusBar = !ShowStatusBar;
        settings.Current.ShowStatusBar = ShowStatusBar;
    }

    /// <summary>
    /// Sorts the file list by a column, ascending, or reverses the direction when it is sorted
    /// by that column already.
    /// </summary>
    [RelayCommand]
    private void SortBy(int column)
    {
        if (column == SortColumn)
        {
            SortDirection = SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
        }
        else
        {
            SortColumn = column;
            SortDirection = SortDirection.Ascending;
        }

        settings.Current.FilesSortingColumn = SortColumn;
        settings.Current.FilesSortOrder = SortDirection;
    }

    /// <summary>
    /// Reads the view options and the recent images list from the settings (dropping recent
    /// images that no longer exist) and refreshes the status bar.
    /// </summary>
    public void LoadSettings()
    {
        ReadViewSettings();
        settings.RemoveMissingRecentImages();
        RecentImages = ReadRecentImages();
        UpdateCapacity();
        UpdateSelectionStatus();
    }

    private void ReadViewSettings()
    {
        var current = settings.Current;
        ViewMode = current.FilesView;
        ShowHiddenItems = current.ShowHiddenItems;
        ShowCommandBar = current.ShowCommandBar;
        ShowDirectoryTree = current.ShowDirectoryTree;
        ShowStatusBar = current.ShowStatusBar;
        SortColumn = current.FilesSortingColumn;
        SortDirection = current.FilesSortOrder;
    }

    // The settings list has the most recent image last.
    private List<string> ReadRecentImages()
    {
        //Remove any bogus entries
        foreach (var path in settings.Current.RecentImages.Where(string.IsNullOrWhiteSpace).ToList())
            settings.RemoveRecentImage(path);

        return Enumerable.Reverse(settings.Current.RecentImages).ToList();
    }

    #endregion

    #region Application

    [RelayCommand]
    private async Task ShowSettings()
    {
        var change = await appDialogs.ShowSettingsAsync();
        if (change is null)
        {
            //If the user cleared the recent images list in the dialog, the menu still needs updating
            RecentImages = ReadRecentImages();
            return;
        }

        if (!settings.Current.AutoIncrementFilename)
            lastSavedFileName = null;

        LoadSettings();
        SettingsApplied?.Invoke(this, EventArgs.Empty);

        // Type names, sizes and icons may have changed.
        ReloadFolders();
    }

    [RelayCommand]
    private Task ShowAbout() => appDialogs.ShowAboutAsync();

    #endregion

    #region Status bar

    /// <summary>
    /// "Partition size: …", or empty when no image is open.
    /// </summary>
    [ObservableProperty]
    public partial string PartitionSizeText { get; private set; } = string.Empty;

    /// <summary>
    /// "Free space: … (…%)", or empty when no image is open.
    /// </summary>
    [ObservableProperty]
    public partial string FreeSpaceText { get; private set; } = string.Empty;

    /// <summary>
    /// The free space of the partition in percent (0-100).
    /// </summary>
    [ObservableProperty]
    public partial double FreeSpacePercentage { get; private set; }

    /// <summary>
    /// The path of the selected entry, or of the current folder.
    /// </summary>
    [ObservableProperty]
    public partial string LocationText { get; private set; } = string.Empty;

    /// <summary>
    /// The size and count of the selected entries, or of the current folder's files.
    /// </summary>
    [ObservableProperty]
    public partial string SelectionSizeText { get; private set; } = string.Empty;

    private void UpdateCapacity()
    {
        cachedSizeDirectory = null;

        if (CurrentPartition is not { } partition)
        {
            PartitionSizeText = string.Empty;
            FreeSpaceText = string.Empty;
            FreeSpacePercentage = 0;
            return;
        }

        var unit = settings.Current.SizeUnit;
        var freeSpace = partition.FileSystem.TotalFreeSpace;
        double percentage = (double)freeSpace / partition.Length * 100;

        PartitionSizeText = $"Partition size: {unit.FormatSize((ulong)partition.Length)}";
        FreeSpaceText = $"Free space: {unit.FormatSize((ulong)freeSpace)} ({percentage / 100:p2})";
        FreeSpacePercentage = percentage;
    }

    private void UpdateSelectionStatus()
    {
        var directory = CurrentFolder?.Directory;
        if (Image is null || directory is null)
        {
            LocationText = string.Empty;
            SelectionSizeText = string.Empty;
            return;
        }

        var unit = settings.Current.SizeUnit;
        var entries = SelectedObjects;
        switch (entries.Count)
        {
            case 0:
                LocationText = directory.FullName;
                if (!ReferenceEquals(directory, cachedSizeDirectory))
                {
                    cachedSizeDirectory = directory;
                    cachedSizeText = $"{unit.FormatSize(directory.GetSize(false, false))} in {directory.CountFiles(false)} item(s)";
                }

                SelectionSizeText = cachedSizeText;
                break;

            case 1:
                LocationText = entries[0].FullName;
                SelectionSizeText = $"{unit.FormatSize(SizeOf(entries[0]))} in 1 item";
                break;

            default:
                LocationText = directory.FullName;
                SelectionSizeText = $"{unit.FormatSize(entries.Aggregate(0ul, (total, entry) => total + SizeOf(entry)))} in {entries.Count} item(s)";
                break;
        }

        // Folders count everything inside them.
        static ulong SizeOf(FileSystemObject entry) => entry is Directory folder ? folder.GetSize(true, false) : entry.Length;
    }

    #endregion

    #region Command refresh

    private IEnumerable<IRelayCommand> EntryCommands =>
    [
        SelectAllCommand, GoToParentFolderCommand, ExpandFolderTreeCommand, CollapseFolderTreeCommand,
        InjectFilesCommand, ExtractCommand, NewFolderCommand, DeleteCommand, RenameCommand, ShowPropertiesCommand
    ];

    private IEnumerable<IRelayCommand> SessionCommands =>
    [
        NewImageCommand, OpenImageCommand, OpenRecentImageCommand, SaveCommand, SaveAsCommand, ReloadImageCommand,
        CloseImageCommand, OpenContainingFolderCommand,
        ChangeVolumeLabelCommand, ShowBootSectorCommand, FormatCommand, ChoosePartitionCommand, ManagePartitionsCommand,
        ShowImageInformationCommand
    ];

    private void RefreshCommands()
    {
        foreach (var command in SessionCommands.Concat(EntryCommands))
            command.NotifyCanExecuteChanged();
    }

    #endregion
}
