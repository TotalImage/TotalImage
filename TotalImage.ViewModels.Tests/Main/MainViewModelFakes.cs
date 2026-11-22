using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TotalImage.Containers;
using TotalImage.FileSystems;
using TotalImage.FileSystems.FAT;
using TotalImage.Partitions;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Main;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;
using TotalImage.ViewModels.Tests.Dialogs;
using Xunit;
using Directory = TotalImage.FileSystems.Directory;
using File = TotalImage.FileSystems.File;

namespace TotalImage.ViewModels.Tests.Main;

/// <summary>
/// A main view model with fake services, and image files written to a temp folder.
/// </summary>
internal sealed class MainTestContext : IDisposable
{
    private readonly TempFolder temp = new();

    public MainTestSettings Settings { get; } = new();
    public FakeImageFiles ImageFiles { get; } = new();
    public FakeImageDialogs ImageDialogs { get; } = new();
    public FakeEntryDialogs EntryDialogs { get; } = new();
    public FakeAppDialogs AppDialogs { get; } = new();
    public FakeMainMessages Messages { get; } = new();
    public FakeMainFilePicker FilePicker { get; } = new();
    public FakeMainLauncher Launcher { get; } = new();
    public FakeBridge Bridge { get; } = new();
    public FakeFileTypes FileTypes { get; } = new();

    private MainViewModel? viewModel;

    /// <summary>
    /// The view model, created on first use so tests can set up the settings first.
    /// </summary>
    public MainViewModel ViewModel => viewModel ??= new MainViewModel(Settings, ImageFiles, ImageDialogs, EntryDialogs,
        AppDialogs, Messages, FilePicker, Launcher, FileTypes) { View = Bridge };

    public string Folder => temp.Path;

    public string PathOf(string name) => temp.File(name);

    public string WriteFile(string name, byte[] bytes)
    {
        var path = temp.File(name);
        System.IO.File.WriteAllBytes(path, bytes);
        return path;
    }

    /// <summary>
    /// A 360 KiB FAT12 floppy labelled <see cref="PartitionTestImages.FatVolumeLabel"/>.
    /// </summary>
    public string WriteFloppy(string name = "disk.img") => WriteFile(name, PartitionTestImages.CreateFatVolume());

    /// <summary>
    /// An MBR disk with a FAT12 partition and an unsupported one.
    /// </summary>
    public string WriteMbrDisk(string name = "mbr.img") => WriteFile(name, PartitionTestImages.MbrImageBytes());

    /// <summary>
    /// A GPT disk with a single, empty EFI system partition.
    /// </summary>
    public string WriteGptDisk(string name = "gpt.img") => WriteFile(name, PartitionTestImages.GptImageBytes());

    /// <summary>
    /// An MBR disk with two empty (unsupported) partitions.
    /// </summary>
    public string WriteUnsupportedMbrDisk(string name = "raw-mbr.img")
    {
        var image = new byte[17 * 512];
        var mbr = image.AsSpan(0, 512);
        PartitionTestImages.WriteMbrEntry(mbr[0x1BE..], false, MbrPartitionTable.MbrPartitionType.HpfsNtfsExFat, 1, 8);
        PartitionTestImages.WriteMbrEntry(mbr[0x1CE..], false, MbrPartitionTable.MbrPartitionType.HpfsNtfsExFat, 9, 8);
        mbr[510] = 0x55;
        mbr[511] = 0xAA;
        return WriteFile(name, image);
    }

    /// <summary>
    /// Opens a file through the Open command.
    /// </summary>
    public async Task OpenAsync(string path)
    {
        FilePicker.OpenPath = path;
        await ViewModel.OpenImageCommand.ExecuteAsync(null);
    }

    /// <summary>
    /// Opens a 1.44 MB floppy with <see cref="BrowseTestImage"/>'s folders and files.
    /// </summary>
    public async Task OpenBrowseImageAsync()
    {
        var path = PathOf("browse.img");
        BrowseTestImage.Write(path, Folder);
        await OpenAsync(path);
        Assert.True(ViewModel.IsImageOpen);
    }

    /// <summary>
    /// Opens a FAT12 floppy and returns its path.
    /// </summary>
    public async Task<string> OpenFloppyAsync(string name = "disk.img")
    {
        var path = WriteFloppy(name);
        await OpenAsync(path);
        Assert.True(ViewModel.IsImageOpen);
        return path;
    }

    public void Dispose()
    {
        viewModel?.Dispose();
        temp.Dispose();
    }
}

internal sealed class MainTestSettings : ISettingsService
{
    public SettingsModel Current { get; } = new();

    public event EventHandler? ExternallyChanged { add { } remove { } }

    public int Saves { get; private set; }

    public void Save() => Saves++;

    public void Reload() { }

    public void ResetToDefaults() { }

    public void AddRecentImage(string path)
    {
        Current.RecentImages.Remove(path);
        if (Current.RecentImages.Count >= 10)
            Current.RecentImages.RemoveAt(0);
        Current.RecentImages.Add(path);
    }

    public void RemoveRecentImage(string path) => Current.RecentImages.Remove(path);

    public void ClearRecentImages() => Current.RecentImages.Clear();

    public void RemoveMissingRecentImages()
    {
        foreach (var path in Current.RecentImages.Where(p => !System.IO.File.Exists(p)).ToList())
            Current.RecentImages.Remove(path);
    }
}

/// <summary>
/// The real image file service, which can be made to fail, to wait, or to open a given container.
/// </summary>
internal sealed class FakeImageFiles : IImageFileService
{
    private readonly ImageFileService real = new();

    public Exception? SaveException { get; set; }

    /// <summary>
    /// When set, opening waits for it.
    /// </summary>
    public TaskCompletionSource? OpenGate { get; set; }

    /// <summary>
    /// When set, opening returns this container instead of reading the file.
    /// </summary>
    public Func<string, Container>? OpenOverride { get; set; }

    public List<string> Saved { get; } = [];

    public async Task<Container> OpenAsync(string path, CancellationToken cancellationToken = default)
    {
        if (OpenGate is not null)
            await OpenGate.Task;

        return OpenOverride is not null ? OpenOverride(path) : await real.OpenAsync(path, cancellationToken);
    }

    public Task SaveAsync(Container image, string path, CancellationToken cancellationToken = default)
    {
        if (SaveException is not null)
            return Task.FromException(SaveException);

        Saved.Add(path);
        return real.SaveAsync(image, path, cancellationToken);
    }
}

internal sealed class FakeImageDialogs : IImageDialogs
{
    public NewImageOptions? NewImageAnswer { get; set; }
    public int NewImageRequests { get; private set; }

    /// <summary>
    /// Answers the partition question; cancels by default.
    /// </summary>
    public Func<Container, PartitionSelection?> SelectPartition { get; set; } = _ => null;
    public int SelectPartitionRequests { get; private set; }

    public FormatOptions? FormatAnswer { get; set; }
    public int FormatRequests { get; private set; }

    public int ManagePartitionsShown { get; private set; }
    public List<(Container Image, string? FilePath, int PartitionIndex)> ImageInfoShown { get; } = [];
    public List<(Container Image, int PartitionIndex)> BootSectorShown { get; } = [];

    public Task<FormatOptions?> AskFormatOptionsAsync()
    {
        FormatRequests++;
        return Task.FromResult(FormatAnswer);
    }

    public Task<PartitionSelection?> AskSelectPartitionAsync(Container image)
    {
        SelectPartitionRequests++;
        return Task.FromResult(SelectPartition(image));
    }

    public Task ShowManagePartitionsAsync()
    {
        ManagePartitionsShown++;
        return Task.CompletedTask;
    }

    public Task<NewImageOptions?> AskNewImageAsync()
    {
        NewImageRequests++;
        return Task.FromResult(NewImageAnswer);
    }

    public Task ShowImageInfoAsync(Container image, string? filePath, int partitionIndex)
    {
        ImageInfoShown.Add((image, filePath, partitionIndex));
        return Task.CompletedTask;
    }

    public Task ShowBootSectorAsync(Container image, int partitionIndex)
    {
        BootSectorShown.Add((image, partitionIndex));
        return Task.CompletedTask;
    }
}

internal sealed class FakeEntryDialogs : IEntryDialogs
{
    public VolumeLabelChange? VolumeLabelAnswer { get; set; }
    public List<(string RootDirectoryLabel, string BpbLabel)> VolumeLabelRequests { get; } = [];

    public Task<VolumeLabelChange?> AskVolumeLabelAsync(string rootDirectoryLabel, string bpbLabel)
    {
        VolumeLabelRequests.Add((rootDirectoryLabel, bpbLabel));
        return Task.FromResult(VolumeLabelAnswer);
    }

    public Task<string?> AskRenameAsync(string currentName) => throw new NotSupportedException();
    public Task<string?> AskNewFolderNameAsync() => throw new NotSupportedException();
    public Task<ExtractOptions?> AskExtractOptionsAsync(int itemCount) => throw new NotSupportedException();
    public Task<EntryPropertiesChange?> AskPropertiesAsync(IReadOnlyList<FileSystemObject> entries) => throw new NotSupportedException();
    public Task ShowExtractionProgressAsync(Func<IProgress<Extraction.ExtractionProgress>, CancellationToken, Task> extraction) => throw new NotSupportedException();
}

internal sealed class FakeAppDialogs : IAppDialogs
{
    public SettingsChangeResult? SettingsAnswer { get; set; }

    /// <summary>
    /// Runs while the settings dialog is "open", to change settings like the user would.
    /// </summary>
    public Action? WhileSettingsOpen { get; set; }

    public int SettingsShown { get; private set; }
    public int AboutShown { get; private set; }

    public Task ShowAboutAsync()
    {
        AboutShown++;
        return Task.CompletedTask;
    }

    public Task<SettingsChangeResult?> ShowSettingsAsync()
    {
        SettingsShown++;
        WhileSettingsOpen?.Invoke();
        return Task.FromResult(SettingsAnswer);
    }
}

internal sealed class FakeMainMessages : MessageServiceStub
{
    public UnsavedChangesAnswer UnsavedAnswer { get; set; } = UnsavedChangesAnswer.Cancel;
    public List<UnsavedChangesReason> UnsavedQuestions { get; } = [];
    public List<ImageOpenFailure> OpenFailures { get; } = [];
    public List<string> MissingRecentImages { get; } = [];
    public int FatOnlyShown { get; private set; }
    public List<(string Operation, Exception Exception)> Errors { get; } = [];
    public bool ConfirmFormatAnswer { get; set; }
    public int FormatConfirmations { get; private set; }

    public override Task<UnsavedChangesAnswer> AskUnsavedChangesAsync(UnsavedChangesReason reason)
    {
        UnsavedQuestions.Add(reason);
        return Task.FromResult(UnsavedAnswer);
    }

    public override Task ShowCannotOpenImageAsync(ImageOpenFailure failure)
    {
        OpenFailures.Add(failure);
        return Task.CompletedTask;
    }

    public override Task ShowRecentImageMissingAsync(string fileName)
    {
        MissingRecentImages.Add(fileName);
        return Task.CompletedTask;
    }

    public override Task ShowFatOnlyFeatureAsync()
    {
        FatOnlyShown++;
        return Task.CompletedTask;
    }

    public override Task ShowErrorAsync(string operation, Exception exception)
    {
        Errors.Add((operation, exception));
        return Task.CompletedTask;
    }

    public override Task<bool> ConfirmFormatAsync()
    {
        FormatConfirmations++;
        return Task.FromResult(ConfirmFormatAnswer);
    }
}

internal sealed class FakeMainFilePicker : IFilePickerService
{
    public string? OpenPath { get; set; }
    public int OpenRequests { get; private set; }

    public string? SavePath { get; set; }
    public List<string?> SaveSuggestions { get; } = [];

    public Task<string?> PickImageToOpenAsync()
    {
        OpenRequests++;
        return Task.FromResult(OpenPath);
    }

    public Task<string?> PickImageSaveLocationAsync(string? suggestedFileName)
    {
        SaveSuggestions.Add(suggestedFileName);
        return Task.FromResult(SavePath);
    }

    public Task<string?> PickSaveFileAsync(IReadOnlyList<FileTypeFilter> filters, string defaultExtension, string? suggestedFileName = null) => throw new NotSupportedException();
    public Task<string?> PickFolderAsync(string? initialFolder = null) => throw new NotSupportedException();
    public Task<IReadOnlyList<string>> PickFilesToAddAsync() => throw new NotSupportedException();
}

internal sealed class FakeMainLauncher : ILauncherService
{
    public List<string> Revealed { get; } = [];

    public void RevealFile(string path) => Revealed.Add(path);
    public void OpenUrl(string url) => throw new NotSupportedException();
    public void OpenFolder(string path) => throw new NotSupportedException();
}

internal sealed class FakeBridge : IMainViewBridge
{
    public List<string> Calls { get; } = [];

    public void Rename() => Calls.Add(nameof(Rename));

    public Task InjectFilesAsync() => Record(nameof(InjectFilesAsync));
    public Task ExtractAsync() => Record(nameof(ExtractAsync));
    public Task CreateFolderAsync() => Record(nameof(CreateFolderAsync));
    public Task DeleteAsync() => Record(nameof(DeleteAsync));
    public Task ShowPropertiesAsync() => Record(nameof(ShowPropertiesAsync));

    private Task Record(string name)
    {
        Calls.Add(name);
        return Task.CompletedTask;
    }
}

/// <summary>
/// Names types like <see cref="GenericFileTypeInfo"/> and counts how often it was asked.
/// </summary>
internal sealed class FakeFileTypes : IFileTypeInfo
{
    public List<(string Name, FileAttributes Attributes)> Requests { get; } = [];

    public string GetTypeName(string name, FileAttributes attributes)
    {
        Requests.Add((name, attributes));
        return "Fake " + GenericFileTypeInfo.GetGenericTypeName(name, attributes);
    }
}

/// <summary>
/// A 1.44 MB FAT12 floppy made with <see cref="RawContainer.CreateImage"/>, with these entries
/// added through its change set and committed to a file:
/// <code>
/// \DOCS\               folder
/// \DOCS\DEEP\          folder
/// \DOCS\DEEP\X.TXT     1 byte
/// \DOCS\A.TXT          100 bytes, 2004
/// \DOCS\B.TXT          50 bytes, 2005
/// \EMPTY\              folder
/// \ALPHA.TXT           10 bytes, 2001, read-only
/// \MID.BIN             2000 bytes, 2002
/// \ZETA.ARC            300 bytes, 2003, system
/// \HIDDEN.SYS          5 bytes, 2000, hidden
/// </code>
/// </summary>
internal static class BrowseTestImage
{
    public static readonly string[] RootFolders = ["DOCS", "EMPTY"];

    public static readonly string[] RootFiles = ["ALPHA.TXT", "MID.BIN", "ZETA.ARC", "HIDDEN.SYS"];

    public static void Write(string path, string tempFolder)
    {
        var geometry = DiskGeometries.FloppyGeometry.KnownGeometries[DiskGeometries.FloppyGeometry.FriendlyName.HighDensity1440k];
        var bpb = FileSystems.BPB.BiosParameterBlock.FromGeometry(geometry, FileSystems.BPB.BiosParameterBlockVersion.Dos20, "TESTOEM ");
        using var image = RawContainer.CreateImage(bpb, geometry.Tracks, true);
        var changes = image.PendingChanges;

        void AddFile(string[] filePath, int length, int year, FatAttributes attributes = 0)
        {
            var source = Changes.FileDataSource.Create(new MemoryStream(new byte[length]), tempFolder, long.MaxValue);
            var written = new DateTime(year, 1, 2, 3, 4, 6);
            changes.Add(new Changes.AddFileChange(filePath, source, FatAttributes.Archive | attributes, written, written, written));
        }

        changes.Add(new Changes.CreateDirectoryChange(["DOCS"]));
        changes.Add(new Changes.CreateDirectoryChange(["DOCS", "DEEP"]));
        AddFile(["DOCS", "DEEP", "X.TXT"], 1, 2006);
        AddFile(["DOCS", "A.TXT"], 100, 2004);
        AddFile(["DOCS", "B.TXT"], 50, 2005);
        changes.Add(new Changes.CreateDirectoryChange(["EMPTY"]));
        AddFile(["ALPHA.TXT"], 10, 2001, FatAttributes.ReadOnly);
        AddFile(["MID.BIN"], 2000, 2002);
        AddFile(["ZETA.ARC"], 300, 2003, FatAttributes.System);
        AddFile(["HIDDEN.SYS"], 5, 2000, FatAttributes.Hidden);

        image.CommitChanges(path).GetAwaiter().GetResult();
    }
}

internal static class FakeFolders
{
    /// <summary>
    /// The root of a folder tree over a fake directory.
    /// </summary>
    public static FolderViewModel Root(FakeDirectory directory) => new(directory, new FolderTreeOptions(true, null));
}

/// <summary>
/// A container that can't be written, wrapping the bytes of an image.
/// </summary>
internal sealed class ReadOnlyContainer(byte[] bytes) : Container(new MemoryStream(bytes, false))
{
    public override Stream Content => containerStream;
    public override string DisplayName => "Read-only test container";
    public override bool SupportsWriting => false;
    protected override Container CloneWriteable(Stream stream) => throw new NotSupportedException();
}

internal sealed class FakeDirectory : Directory
{
    private readonly List<FileSystemObject> children = [];

    public FakeDirectory(string name, Directory? parent = null) : base(null!, parent) => Name = name;

    public override string Name { get; set; }
    public override FileAttributes Attributes { get; set; } = FileAttributes.Directory;
    public override DateTime? LastAccessTime { get; set; }
    public override DateTime? LastWriteTime { get; set; }
    public override DateTime? CreationTime { get; set; }
    public override ulong Length { get; set; }

    public FakeFile AddFile(string name, ulong length)
    {
        var file = new FakeFile(name, length, this);
        children.Add(file);
        return file;
    }

    public FakeDirectory AddDirectory(string name)
    {
        var directory = new FakeDirectory(name, this);
        children.Add(directory);
        return directory;
    }

    public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden) => children;
    public override Directory CreateSubdirectory(string path) => throw new NotSupportedException();
    public override void Delete() => throw new NotSupportedException();
    public override void MoveTo(string path) => throw new NotSupportedException();
}

internal sealed class FakeFile(string name, ulong length, Directory directory) : File(null!, directory)
{
    public override string Name { get; set; } = name;
    public override FileAttributes Attributes { get; set; } = FileAttributes.Archive;
    public override DateTime? LastAccessTime { get; set; }
    public override DateTime? LastWriteTime { get; set; }
    public override DateTime? CreationTime { get; set; }
    public override ulong Length { get; set; } = length;
    public override Stream GetStream() => throw new NotSupportedException();
    public override void Delete() => throw new NotSupportedException();
    public override void MoveTo(string path) => throw new NotSupportedException();
}
