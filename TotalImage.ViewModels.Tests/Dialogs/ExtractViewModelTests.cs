using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;
using Xunit;

namespace TotalImage.ViewModels.Tests.Dialogs;

public class ExtractViewModelTests : IDisposable
{
    private readonly TempFolder folder = new();
    private readonly ExtractFakeSettings settings = new();
    private readonly ExtractFakePicker picker = new();
    private readonly IMessageService messageService;
    private readonly ExtractFakeMessages messages;

    public ExtractViewModelTests()
    {
        (messageService, messages) = ExtractFakeMessages.Create();
        settings.Current.DefaultExtractPath = folder.Path;
        settings.Current.OpenFolderAfterExtract = false;
        settings.Current.DefaultDirectoryExtractionMode = DirectoryExtractionMode.Merge;
    }

    public void Dispose() => folder.Dispose();

    private ExtractViewModel Create(int itemCount = 1) => new(itemCount, settings, picker, messageService);

    [Fact]
    public void StartsFromTheSettings()
    {
        var vm = Create();

        Assert.Equal(folder.Path, vm.TargetPath);
        Assert.False(vm.OpenFolder);
        Assert.Equal(DirectoryExtractionMode.Merge, vm.DirectoryExtractionMode);
        Assert.True(vm.CanExtract);
        Assert.True(vm.AcceptCommand.CanExecute(null));
        Assert.Null(vm.Answer);
    }

    [Theory]
    [InlineData(1, "Extract 1 selected item to the following folder:")]
    [InlineData(2, "Extract 2 selected items to the following folder:")]
    public void PromptCountsTheItems(int count, string expected)
    {
        Assert.Equal(expected, Create(count).Prompt);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void CannotExtractWithoutATargetPath(string path)
    {
        var vm = Create();
        var changed = new List<string?>();
        vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);
        var canExecuteChanged = 0;
        vm.AcceptCommand.CanExecuteChanged += (_, _) => canExecuteChanged++;

        vm.TargetPath = path;

        Assert.False(vm.CanExtract);
        Assert.False(vm.AcceptCommand.CanExecute(null));
        Assert.Contains(nameof(ExtractViewModel.CanExtract), changed);
        Assert.Equal(1, canExecuteChanged);
    }

    [Fact]
    public async Task TryAcceptFailsWithoutATargetPath()
    {
        var vm = Create();
        vm.TargetPath = "";

        Assert.False(await vm.TryAcceptAsync());
        Assert.Null(vm.Answer);
        Assert.Equal(0, messages.Asked);
    }

    [Fact]
    public void ChangingTheModeIsNotified()
    {
        var vm = Create();
        var changed = new List<string?>();
        vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        vm.DirectoryExtractionMode = DirectoryExtractionMode.Skip;

        Assert.Equal([nameof(ExtractViewModel.DirectoryExtractionMode)], changed);
    }

    [Fact]
    public async Task BrowseSetsThePickedFolder()
    {
        var vm = Create();
        picker.Folder = @"C:\picked";

        await vm.BrowseCommand.ExecuteAsync(null);

        Assert.Equal(@"C:\picked", vm.TargetPath);
        Assert.Equal(1, picker.Picked);
    }

    [Fact]
    public async Task CancelledBrowseKeepsTheTargetPath()
    {
        var vm = Create();
        picker.Folder = null;

        await vm.BrowseCommand.ExecuteAsync(null);

        Assert.Equal(folder.Path, vm.TargetPath);
    }

    [Fact]
    public async Task AcceptsAnExistingFolderWithoutAsking()
    {
        var vm = Create();
        vm.OpenFolder = true;
        vm.DirectoryExtractionMode = DirectoryExtractionMode.Preserve;

        Assert.True(await vm.TryAcceptAsync());

        Assert.Equal(0, messages.Asked);
        Assert.Equal(new ExtractOptions(folder.Path, DirectoryExtractionMode.Preserve, true), vm.Answer);
    }

    [Fact]
    public async Task CreatesAMissingFolderWhenTheUserAgrees()
    {
        var vm = Create();
        var target = folder.File(Path.Combine("new", "folder"));
        vm.TargetPath = target;
        messages.CreateFolder = true;

        Assert.True(await vm.TryAcceptAsync());

        Assert.Equal([target], messages.AskedPaths);
        Assert.True(Directory.Exists(target));
        Assert.Equal(target, vm.Answer?.TargetPath);
    }

    [Fact]
    public async Task StaysOpenWhenTheUserDeclinesToCreateTheFolder()
    {
        var vm = Create();
        var target = folder.File("missing");
        vm.TargetPath = target;
        messages.CreateFolder = false;

        Assert.False(await vm.TryAcceptAsync());

        Assert.Equal(1, messages.Asked);
        Assert.False(Directory.Exists(target));
        Assert.Null(vm.Answer);
    }
}

internal sealed class ExtractFakeSettings : ISettingsService
{
    public SettingsModel Current { get; } = new();
    public event EventHandler? ExternallyChanged { add { } remove { } }
    public void Save() { }
    public void Reload() { }
    public void ResetToDefaults() { }
    public void AddRecentImage(string path) { }
    public void RemoveRecentImage(string path) { }
    public void ClearRecentImages() { }
    public void RemoveMissingRecentImages() { }
}

internal sealed class ExtractFakePicker : IFilePickerService
{
    public string? Folder { get; set; }
    public int Picked { get; private set; }

    public Task<string?> PickFolderAsync(string? initialFolder = null)
    {
        Picked++;
        return Task.FromResult(Folder);
    }

    public Task<string?> PickImageToOpenAsync() => throw new NotSupportedException();
    public Task<string?> PickImageSaveLocationAsync(string? suggestedFileName) => throw new NotSupportedException();
    public Task<string?> PickSaveFileAsync(IReadOnlyList<FileTypeFilter> filters, string defaultExtension, string? suggestedFileName = null) => throw new NotSupportedException();
    public Task<IReadOnlyList<string>> PickFilesToAddAsync() => throw new NotSupportedException();
}

// A proxy, so the fake keeps compiling when IMessageService gains methods for other dialogs.
internal class ExtractFakeMessages : DispatchProxy
{
    public bool CreateFolder { get; set; }
    public List<string> AskedPaths { get; } = [];
    public int Asked => AskedPaths.Count;

    public static (IMessageService Service, ExtractFakeMessages Fake) Create()
    {
        var service = Create<IMessageService, ExtractFakeMessages>();
        return (service, (ExtractFakeMessages)(object)service);
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (targetMethod?.Name != nameof(IMessageService.ConfirmCreateExtractFolderAsync))
            throw new NotSupportedException(targetMethod?.Name);

        AskedPaths.Add((string)args![0]!);
        return Task.FromResult(CreateFolder);
    }
}
