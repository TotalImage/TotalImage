using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;
using Xunit;

namespace TotalImage.ViewModels.Tests.Dialogs;

public sealed class SettingsViewModelTests : IDisposable
{
    private readonly TempFolder folder = new();
    private readonly SettingsService settings;
    private readonly FakeTempDirectory tempDirectory = new();
    private readonly FakeInstances instances = new();
    private readonly FakeMessages messages = new();
    private readonly FakeLauncher launcher = new();
    private readonly FakeFilePicker filePicker = new();

    public SettingsViewModelTests()
    {
        settings = new SettingsService(folder.Path, watchForChanges: false);
    }

    public void Dispose()
    {
        settings.Dispose();
        folder.Dispose();
    }

    private SettingsViewModel CreateViewModel()
        => new(settings, tempDirectory, instances, messages, launcher, filePicker);

    private string SettingsFile => folder.File("settings.json");

    private string? SavedSettings => File.Exists(SettingsFile) ? File.ReadAllText(SettingsFile) : null;

    private void UseNonDefaultSettings()
    {
        var current = settings.Current;
        current.FilesView = ViewMode.List;
        current.FilesSortingColumn = 3;
        current.FilesSortOrder = SortDirection.Descending;
        current.ShowHiddenItems = false;
        current.ShowCommandBar = false;
        current.ShowDirectoryTree = false;
        current.ShowStatusBar = false;
        current.FileListShowDirSize = true;
        current.SizeUnit = SizeUnit.Binary;
        current.ColorMode = ColorTheme.Dark;
        current.ExtractAlwaysAsk = false;
        current.DefaultExtractPath = @"C:\Extracted";
        current.DefaultDirectoryExtractionMode = DirectoryExtractionMode.Merge;
        current.OpenFolderAfterExtract = false;
        current.ExtractPreserveDates = false;
        current.ExtractPreserveAttributes = true;
        current.QueryShellForFileTypeInfo = false;
        current.ConfirmInjection = false;
        current.ConfirmDeletion = false;
        current.ConfirmOverwriteExtraction = false;
        current.AutoIncrementFilename = false;
        current.MemoryMappingThreshold = 5 * SettingsViewModel.BytesPerMiB;
    }

    [Fact]
    public void StartsWithTheCurrentSettings()
    {
        UseNonDefaultSettings();

        var vm = CreateViewModel();

        Assert.Equal(ViewMode.List, vm.FilesView);
        Assert.Equal(3, vm.SortColumn);
        Assert.Equal(SortDirection.Descending, vm.SortDirection);
        Assert.False(vm.ShowHiddenItems);
        Assert.False(vm.ShowCommandBar);
        Assert.False(vm.ShowDirectoryTree);
        Assert.False(vm.ShowStatusBar);
        Assert.True(vm.ShowDirectorySizes);
        Assert.Equal(SizeUnit.Binary, vm.SizeUnit);
        Assert.Equal(ColorTheme.Dark, vm.ColorTheme);
        Assert.False(vm.ExtractAlwaysAsk);
        Assert.Equal(@"C:\Extracted", vm.ExtractPath);
        Assert.Equal(DirectoryExtractionMode.Merge, vm.DirectoryExtractionMode);
        Assert.False(vm.OpenFolderAfterExtract);
        Assert.False(vm.PreserveDates);
        Assert.True(vm.PreserveAttributes);
        Assert.False(vm.QueryShellForFileTypeInfo);
        Assert.False(vm.ConfirmInjection);
        Assert.False(vm.ConfirmDeletion);
        Assert.False(vm.ConfirmOverwriteExtraction);
        Assert.False(vm.AutoIncrementFilename);
        Assert.Equal(5m, vm.MemoryMappingThresholdMiB);
        Assert.True(vm.AcceptCommand.CanExecute(null));
        Assert.Null(vm.Answer);
    }

    [Fact]
    public void MemoryMappingThresholdIsShownInWholeMiB()
    {
        settings.Current.MemoryMappingThreshold = 3 * SettingsViewModel.BytesPerMiB + 12345;

        Assert.Equal(3m, CreateViewModel().MemoryMappingThresholdMiB);
    }

    [Fact]
    public void EditingDoesNotChangeTheSettingsBeforeAccepting()
    {
        var saved = SavedSettings;
        var vm = CreateViewModel();

        vm.ShowStatusBar = false;
        vm.ExtractPath = @"D:\Elsewhere";
        vm.ColorTheme = ColorTheme.Dark;

        Assert.True(settings.Current.ShowStatusBar);
        Assert.NotEqual(@"D:\Elsewhere", settings.Current.DefaultExtractPath);
        Assert.Equal(ColorTheme.System, settings.Current.ColorMode);
        Assert.Equal(saved, SavedSettings);
    }

    [Fact]
    public void AcceptWritesEveryValueBackAndSaves()
    {
        var vm = CreateViewModel();
        vm.FilesView = ViewMode.SmallIcon;
        vm.SortColumn = 2;
        vm.SortDirection = SortDirection.Descending;
        vm.ShowHiddenItems = false;
        vm.ShowCommandBar = false;
        vm.ShowDirectoryTree = false;
        vm.ShowStatusBar = false;
        vm.ShowDirectorySizes = true;
        vm.SizeUnit = SizeUnit.Decimal;
        vm.ColorTheme = ColorTheme.Classic;
        vm.ExtractAlwaysAsk = false;
        vm.ExtractPath = @"D:\Out";
        vm.DirectoryExtractionMode = DirectoryExtractionMode.Skip;
        vm.OpenFolderAfterExtract = false;
        vm.PreserveDates = false;
        vm.PreserveAttributes = true;
        vm.QueryShellForFileTypeInfo = false;
        vm.ConfirmInjection = false;
        vm.ConfirmDeletion = false;
        vm.ConfirmOverwriteExtraction = false;
        vm.AutoIncrementFilename = false;
        vm.MemoryMappingThresholdMiB = 42;

        vm.AcceptCommand.Execute(null);

        var current = settings.Current;
        Assert.Equal(ViewMode.SmallIcon, current.FilesView);
        Assert.Equal(2, current.FilesSortingColumn);
        Assert.Equal(SortDirection.Descending, current.FilesSortOrder);
        Assert.False(current.ShowHiddenItems);
        Assert.False(current.ShowCommandBar);
        Assert.False(current.ShowDirectoryTree);
        Assert.False(current.ShowStatusBar);
        Assert.True(current.FileListShowDirSize);
        Assert.Equal(SizeUnit.Decimal, current.SizeUnit);
        Assert.Equal(ColorTheme.Classic, current.ColorMode);
        Assert.False(current.ExtractAlwaysAsk);
        Assert.Equal(@"D:\Out", current.DefaultExtractPath);
        Assert.Equal(DirectoryExtractionMode.Skip, current.DefaultDirectoryExtractionMode);
        Assert.False(current.OpenFolderAfterExtract);
        Assert.False(current.ExtractPreserveDates);
        Assert.True(current.ExtractPreserveAttributes);
        Assert.False(current.QueryShellForFileTypeInfo);
        Assert.False(current.ConfirmInjection);
        Assert.False(current.ConfirmDeletion);
        Assert.False(current.ConfirmOverwriteExtraction);
        Assert.False(current.AutoIncrementFilename);
        Assert.Equal(42 * SettingsViewModel.BytesPerMiB, current.MemoryMappingThreshold);

        using var reloaded = new SettingsService(folder.Path, watchForChanges: false);
        Assert.Equal(@"D:\Out", reloaded.Current.DefaultExtractPath);
        Assert.Equal(ViewMode.SmallIcon, reloaded.Current.FilesView);
        Assert.Equal(42 * SettingsViewModel.BytesPerMiB, reloaded.Current.MemoryMappingThreshold);
    }

    [Fact]
    public void AcceptReportsAnUnchangedColorTheme()
    {
        var vm = CreateViewModel();
        vm.ShowStatusBar = false;

        vm.AcceptCommand.Execute(null);

        Assert.Equal(new SettingsChangeResult(ColorThemeChanged: false), vm.Answer);
    }

    [Fact]
    public void AcceptReportsAChangedColorTheme()
    {
        var vm = CreateViewModel();
        vm.ColorTheme = ColorTheme.Dark;

        vm.AcceptCommand.Execute(null);

        Assert.Equal(new SettingsChangeResult(ColorThemeChanged: true), vm.Answer);
    }

    [Fact]
    public void ColorThemeSetBackToTheOriginalCountsAsUnchanged()
    {
        var vm = CreateViewModel();
        vm.ColorTheme = ColorTheme.Dark;
        vm.ColorTheme = ColorTheme.System;

        vm.AcceptCommand.Execute(null);

        Assert.False(vm.Answer!.ColorThemeChanged);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void ExtractionDefaultsAreEditableOnlyWhileNotAlwaysAsking(bool alwaysAsk, bool editable)
    {
        settings.Current.ExtractAlwaysAsk = alwaysAsk;

        Assert.Equal(editable, CreateViewModel().CanEditExtractionDefaults);
    }

    [Fact]
    public void TogglingAlwaysAskUpdatesWhetherDefaultsAreEditable()
    {
        settings.Current.ExtractAlwaysAsk = true;
        var vm = CreateViewModel();
        var changed = Track(vm);

        vm.ExtractAlwaysAsk = false;
        Assert.True(vm.CanEditExtractionDefaults);
        Assert.Contains(nameof(SettingsViewModel.CanEditExtractionDefaults), changed);

        changed.Clear();
        vm.ExtractAlwaysAsk = true;
        Assert.False(vm.CanEditExtractionDefaults);
        Assert.Contains(nameof(SettingsViewModel.CanEditExtractionDefaults), changed);
    }

    [Theory]
    [InlineData(SizeUnit.Bytes)]
    [InlineData(SizeUnit.Decimal)]
    [InlineData(SizeUnit.Binary)]
    public void SizeUnitPreviewFormatsSampleSizesInTheChosenUnit(SizeUnit unit)
    {
        settings.Current.SizeUnit = unit;

        var preview = CreateViewModel().SizeUnitPreview;

        Assert.Equal($"Preview:{Environment.NewLine}{unit.FormatSize(234)} | {unit.FormatSize(2345)} | " +
            $"{unit.FormatSize(2345678)} | {unit.FormatSize(2345678901)} | {unit.FormatSize(2345678901234)}", preview);
    }

    [Fact]
    public void ChangingTheSizeUnitUpdatesThePreview()
    {
        var vm = CreateViewModel();
        var before = vm.SizeUnitPreview;
        var changed = Track(vm);

        vm.SizeUnit = SizeUnit.Binary;

        Assert.Contains(nameof(SettingsViewModel.SizeUnitPreview), changed);
        Assert.NotEqual(before, vm.SizeUnitPreview);
        Assert.Contains(SizeUnit.Binary.FormatSize(2345678), vm.SizeUnitPreview);
    }

    [Fact]
    public async Task ClearRecentImagesClearsRightAwayAndSaysSo()
    {
        settings.Current.RecentImages.AddRange([@"C:\a.img", @"C:\b.img"]);
        var vm = CreateViewModel();

        await vm.ClearRecentImagesCommand.ExecuteAsync(null);

        Assert.Empty(settings.Current.RecentImages);
        Assert.Equal(["RecentImagesCleared"], messages.Calls);
    }

    [Fact]
    public async Task ResetDoesNothingWhenNotConfirmed()
    {
        UseNonDefaultSettings();
        messages.ConfirmReset = false;
        var saved = SavedSettings;
        var vm = CreateViewModel();
        vm.ShowStatusBar = true;

        await vm.ResetToDefaultsCommand.ExecuteAsync(null);

        Assert.Equal(["ConfirmReset"], messages.Calls);
        Assert.Equal(ViewMode.List, settings.Current.FilesView);
        Assert.True(vm.ShowStatusBar);
        Assert.Equal(ViewMode.List, vm.FilesView);
        Assert.Equal(saved, SavedSettings);
    }

    [Fact]
    public async Task ResetRestoresDefaultsSavesAndShowsThem()
    {
        UseNonDefaultSettings();
        settings.Current.RecentImages.Add(@"C:\a.img");
        settings.Save();
        var vm = CreateViewModel();
        var defaults = new SettingsModel();

        await vm.ResetToDefaultsCommand.ExecuteAsync(null);

        Assert.Equal(["ConfirmReset", "SettingsReset"], messages.Calls);
        Assert.Equal(defaults.FilesView, settings.Current.FilesView);
        Assert.Equal([@"C:\a.img"], settings.Current.RecentImages);
        using (var reloaded = new SettingsService(folder.Path, watchForChanges: false))
            Assert.Equal(defaults.FilesView, reloaded.Current.FilesView);

        Assert.Equal(defaults.FilesView, vm.FilesView);
        Assert.Equal(defaults.FilesSortingColumn, vm.SortColumn);
        Assert.Equal(defaults.FilesSortOrder, vm.SortDirection);
        Assert.Equal(defaults.ShowHiddenItems, vm.ShowHiddenItems);
        Assert.Equal(defaults.ShowCommandBar, vm.ShowCommandBar);
        Assert.Equal(defaults.ShowDirectoryTree, vm.ShowDirectoryTree);
        Assert.Equal(defaults.ShowStatusBar, vm.ShowStatusBar);
        Assert.Equal(defaults.FileListShowDirSize, vm.ShowDirectorySizes);
        Assert.Equal(defaults.SizeUnit, vm.SizeUnit);
        Assert.Equal(defaults.ColorMode, vm.ColorTheme);
        Assert.Equal(defaults.ExtractAlwaysAsk, vm.ExtractAlwaysAsk);
        Assert.Equal(defaults.DefaultExtractPath, vm.ExtractPath);
        Assert.Equal(defaults.DefaultDirectoryExtractionMode, vm.DirectoryExtractionMode);
        Assert.Equal(defaults.OpenFolderAfterExtract, vm.OpenFolderAfterExtract);
        Assert.Equal(defaults.ExtractPreserveDates, vm.PreserveDates);
        Assert.Equal(defaults.ExtractPreserveAttributes, vm.PreserveAttributes);
        Assert.Equal(defaults.QueryShellForFileTypeInfo, vm.QueryShellForFileTypeInfo);
        Assert.Equal(defaults.ConfirmInjection, vm.ConfirmInjection);
        Assert.Equal(defaults.ConfirmDeletion, vm.ConfirmDeletion);
        Assert.Equal(defaults.ConfirmOverwriteExtraction, vm.ConfirmOverwriteExtraction);
        Assert.Equal(defaults.AutoIncrementFilename, vm.AutoIncrementFilename);
        Assert.Equal(defaults.MemoryMappingThreshold / SettingsViewModel.BytesPerMiB, vm.MemoryMappingThresholdMiB);
        Assert.False(vm.CanEditExtractionDefaults);
    }

    [Fact]
    public async Task ResetThenAcceptReportsTheColorThemeChangeAgainstTheOpeningTheme()
    {
        settings.Current.ColorMode = ColorTheme.Dark;
        var vm = CreateViewModel();

        await vm.ResetToDefaultsCommand.ExecuteAsync(null);
        vm.AcceptCommand.Execute(null);

        Assert.True(vm.Answer!.ColorThemeChanged);
    }

    [Fact]
    public async Task ClearTempFolderClearsWithoutAskingWhenAlone()
    {
        var vm = CreateViewModel();

        await vm.ClearTempFolderCommand.ExecuteAsync(null);

        Assert.Equal(1, tempDirectory.ClearCount);
        Assert.Equal(["TempFolderCleared"], messages.Calls);
    }

    [Fact]
    public async Task ClearTempFolderAsksFirstWhenOtherInstancesRun()
    {
        instances.OthersRunning = true;
        var vm = CreateViewModel();

        await vm.ClearTempFolderCommand.ExecuteAsync(null);

        Assert.Equal(1, tempDirectory.ClearCount);
        Assert.Equal(["ConfirmClearTempWithOthers", "TempFolderCleared"], messages.Calls);
    }

    [Fact]
    public async Task ClearTempFolderStopsWhenTheUserDeclines()
    {
        instances.OthersRunning = true;
        messages.ConfirmClearTemp = false;
        var vm = CreateViewModel();

        await vm.ClearTempFolderCommand.ExecuteAsync(null);

        Assert.Equal(0, tempDirectory.ClearCount);
        Assert.Equal(["ConfirmClearTempWithOthers"], messages.Calls);
    }

    [Fact]
    public async Task ClearTempFolderReportsFailure()
    {
        tempDirectory.ClearException = new IOException("in use");
        var vm = CreateViewModel();

        await vm.ClearTempFolderCommand.ExecuteAsync(null);

        Assert.Equal(["TempFolderClearFailed"], messages.Calls);
    }

    [Fact]
    public async Task ClearTempFolderWorksOnARealFolder()
    {
        var temp = new TempDirectoryService(folder.File("temp"));
        temp.EnsureRootExists();
        File.WriteAllText(Path.Combine(temp.Root, "leftover.bin"), "x");
        var vm = new SettingsViewModel(settings, temp, instances, messages, launcher, filePicker);

        await vm.ClearTempFolderCommand.ExecuteAsync(null);

        Assert.True(Directory.Exists(temp.Root));
        Assert.Empty(Directory.EnumerateFileSystemEntries(temp.Root));
        Assert.Equal(["TempFolderCleared"], messages.Calls);
    }

    [Fact]
    public void OpenTempFolderCreatesItAndOpensIt()
    {
        var vm = CreateViewModel();

        vm.OpenTempFolderCommand.Execute(null);

        Assert.Equal(["EnsureRootExists"], tempDirectory.Calls);
        Assert.Equal([tempDirectory.Root], launcher.OpenedFolders);
    }

    [Fact]
    public async Task BrowseSetsTheExtractPathToThePickedFolder()
    {
        settings.Current.DefaultExtractPath = @"C:\Start";
        filePicker.Result = @"E:\Picked";
        var vm = CreateViewModel();

        await vm.BrowseExtractPathCommand.ExecuteAsync(null);

        Assert.Equal(@"C:\Start", filePicker.InitialFolder);
        Assert.Equal(@"E:\Picked", vm.ExtractPath);
    }

    [Fact]
    public async Task BrowseKeepsTheExtractPathWhenCancelled()
    {
        settings.Current.DefaultExtractPath = @"C:\Start";
        filePicker.Result = null;
        var vm = CreateViewModel();

        await vm.BrowseExtractPathCommand.ExecuteAsync(null);

        Assert.Equal(@"C:\Start", vm.ExtractPath);
    }

    [Fact]
    public void CommandsAreAlwaysAvailable()
    {
        settings.Current.ExtractAlwaysAsk = true;
        var vm = CreateViewModel();

        Assert.True(vm.AcceptCommand.CanExecute(null));
        Assert.True(vm.ClearRecentImagesCommand.CanExecute(null));
        Assert.True(vm.ResetToDefaultsCommand.CanExecute(null));
        Assert.True(vm.ClearTempFolderCommand.CanExecute(null));
        Assert.True(vm.OpenTempFolderCommand.CanExecute(null));
        Assert.True(vm.BrowseExtractPathCommand.CanExecute(null));
    }

    private static List<string?> Track(INotifyPropertyChanged source)
    {
        var changed = new List<string?>();
        source.PropertyChanged += (_, e) => changed.Add(e.PropertyName);
        return changed;
    }

    private sealed class FakeTempDirectory : ITempDirectoryService
    {
        public List<string> Calls { get; } = [];
        public int ClearCount { get; private set; }
        public Exception? ClearException { get; set; }

        public string Root => @"C:\Temp\TotalImage";

        public void EnsureRootExists() => Calls.Add(nameof(EnsureRootExists));

        public string GetUniqueSubdirectoryPath() => throw new NotSupportedException();

        public void Clear()
        {
            if (ClearException is not null)
                throw ClearException;
            ClearCount++;
        }
    }

    private sealed class FakeInstances : IAppInstances
    {
        public bool OthersRunning { get; set; }

        public bool AreOtherInstancesRunning() => OthersRunning;
    }

    private sealed class FakeMessages : IMessageService
    {
        public List<string> Calls { get; } = [];
        public bool ConfirmReset { get; set; } = true;
        public bool ConfirmClearTemp { get; set; } = true;

        public Task<bool> ConfirmFormatAsync() => throw new NotSupportedException();

        public Task ShowRecentImagesClearedAsync() => Record("RecentImagesCleared");

        public Task<bool> ConfirmResetSettingsAsync()
        {
            Calls.Add("ConfirmReset");
            return Task.FromResult(ConfirmReset);
        }

        public Task ShowSettingsResetAsync() => Record("SettingsReset");

        public Task<bool> ConfirmClearTempFolderWithOtherInstancesAsync()
        {
            Calls.Add("ConfirmClearTempWithOthers");
            return Task.FromResult(ConfirmClearTemp);
        }

        public Task ShowTempFolderClearedAsync() => Record("TempFolderCleared");

        public Task ShowTempFolderClearFailedAsync() => Record("TempFolderClearFailed");

        private Task Record(string call)
        {
            Calls.Add(call);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeLauncher : ILauncherService
    {
        public List<string> OpenedFolders { get; } = [];

        public void OpenUrl(string url) => throw new NotSupportedException();

        public void OpenFolder(string path) => OpenedFolders.Add(path);
    }

    private sealed class FakeFilePicker : IFilePickerService
    {
        public string? Result { get; set; }
        public string? InitialFolder { get; private set; }

        public Task<string?> PickFolderAsync(string? initialFolder = null)
        {
            InitialFolder = initialFolder;
            return Task.FromResult(Result);
        }

        public Task<string?> PickImageToOpenAsync() => throw new NotSupportedException();

        public Task<string?> PickImageSaveLocationAsync(string? suggestedFileName) => throw new NotSupportedException();

        public Task<string?> PickSaveFileAsync(IReadOnlyList<FileTypeFilter> filters, string defaultExtension, string? suggestedFileName = null)
            => throw new NotSupportedException();

        public Task<IReadOnlyList<string>> PickFilesToAddAsync() => throw new NotSupportedException();
    }
}
