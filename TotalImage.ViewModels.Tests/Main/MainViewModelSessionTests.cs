using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TotalImage.Changes;
using TotalImage.DiskGeometries;
using TotalImage.FileSystems.BPB;
using TotalImage.FileSystems.FAT;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Main;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Tests.Dialogs;
using Xunit;

namespace TotalImage.ViewModels.Tests.Main;

/// <summary>
/// Opening, creating, saving, reloading and closing images.
/// </summary>
public class MainViewModelSessionTests : IDisposable
{
    private readonly MainTestContext context = new();

    private MainViewModel Vm => context.ViewModel;

    public void Dispose() => context.Dispose();

    private static NewImageOptions FloppyOptions() => new(
        FloppyGeometry.KnownGeometries[FloppyGeometry.FriendlyName.HighDensity1440k], true, BiosParameterBlockVersion.Dos40,
        "MSDOS5.0", "1234ABCD", "FAT12", "NEWDISK");

    private static bool DirectoryExists(string path, string name)
    {
        using var image = new ImageFileService().OpenAsync(path).GetAwaiter().GetResult();
        return image.PartitionTable.Partitions[0].FileSystem.RootDirectory.EnumerateDirectories(true).Any(d => d.Name == name);
    }

    private void AddChange(string directoryName = "NEWDIR") => Vm.Image!.PendingChanges.Add(new CreateDirectoryChange([directoryName]));

    [Fact]
    public void NothingIsOpenAtFirst()
    {
        Assert.Null(Vm.Image);
        Assert.False(Vm.IsImageOpen);
        Assert.Null(Vm.FilePath);
        Assert.Null(Vm.FileName);
        Assert.False(Vm.HasUnsavedChanges);
        Assert.Null(Vm.CurrentFileSystem);
        Assert.Null(Vm.CurrentPartition);
        Assert.Empty(Vm.Partitions);
        Assert.Equal("TotalImage", Vm.Title);
        Assert.Equal(string.Empty, Vm.PartitionSizeText);
        Assert.Equal(string.Empty, Vm.FreeSpaceText);
        Assert.Equal(string.Empty, Vm.LocationText);
        Assert.Equal(string.Empty, Vm.SelectionSizeText);
    }

    [Fact]
    public async Task OpeningAnImageLoadsItsOnlyPartition()
    {
        var path = await context.OpenFloppyAsync();

        Assert.NotNull(Vm.Image);
        Assert.True(Vm.IsImageOpen);
        Assert.Equal(path, Vm.FilePath);
        Assert.Equal("disk.img", Vm.FileName);
        Assert.Equal("disk.img - TotalImage", Vm.Title);
        Assert.False(Vm.HasUnsavedChanges);
        Assert.Equal(0, Vm.CurrentPartitionIndex);
        Assert.IsAssignableFrom<FatFileSystem>(Vm.CurrentFileSystem);
        Assert.Same(Vm.Image.PartitionTable.Partitions[0], Vm.CurrentPartition);
        var option = Assert.Single(Vm.Partitions);
        Assert.Equal(0, option.Index);
        Assert.Equal($"{PartitionTestImages.FatVolumeLabel} (FAT12, {SizeUnit.Bytes.FormatSize(368_640)})", option.Text);
        Assert.Equal(option.Text, option.ToString());
        Assert.Equal([path], Vm.RecentImages);
        Assert.Equal(1, context.FilePicker.OpenRequests);
        Assert.False(Vm.IsBusy);
    }

    [Fact]
    public async Task OpeningAnImageRaisesChangeNotifications()
    {
        var changed = new List<string?>();
        Vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        await context.OpenFloppyAsync();

        Assert.Contains(nameof(MainViewModel.Image), changed);
        Assert.Contains(nameof(MainViewModel.IsImageOpen), changed);
        Assert.Contains(nameof(MainViewModel.FilePath), changed);
        Assert.Contains(nameof(MainViewModel.FileName), changed);
        Assert.Contains(nameof(MainViewModel.Title), changed);
        Assert.Contains(nameof(MainViewModel.Partitions), changed);
        Assert.Contains(nameof(MainViewModel.CurrentFileSystem), changed);
        Assert.Contains(nameof(MainViewModel.RecentImages), changed);
        Assert.Contains(nameof(MainViewModel.PartitionSizeText), changed);
        // The view loads the partition when the file system changes, so everything else must be set by then.
        Assert.True(changed.IndexOf(nameof(MainViewModel.CurrentFileSystem)) > changed.IndexOf(nameof(MainViewModel.Image)));
        Assert.True(changed.IndexOf(nameof(MainViewModel.CurrentFileSystem)) > changed.IndexOf(nameof(MainViewModel.Partitions)));
    }

    [Fact]
    public async Task CancellingTheOpenPickerKeepsTheCurrentImage()
    {
        await context.OpenFloppyAsync();
        var image = Vm.Image;

        context.FilePicker.OpenPath = null;
        await Vm.OpenImageCommand.ExecuteAsync(null);

        Assert.Same(image, Vm.Image);
    }

    [Fact]
    public async Task OpeningAnotherImageClosesTheFirstOne()
    {
        await context.OpenFloppyAsync("first.img");
        var first = Vm.Image!;

        var second = await context.OpenFloppyAsync("second.img");

        Assert.Equal(second, Vm.FilePath);
        Assert.NotSame(first, Vm.Image);
        Assert.Throws<ObjectDisposedException>(() => first.Content.Position = 0);
    }

    [Fact]
    public async Task OpenImageFileAsyncOpensAFileDirectly()
    {
        var path = context.WriteFloppy();

        Assert.True(await Vm.OpenImageFileAsync(path));

        Assert.Equal(path, Vm.FilePath);
        Assert.Equal(0, context.FilePicker.OpenRequests);
    }

    public static TheoryData<string> BlankPaths => ["", "   "];

    [Theory]
    [MemberData(nameof(BlankPaths))]
    public async Task ABlankPathIsRejected(string path)
    {
        Assert.False(await Vm.OpenImageFileAsync(path));

        var failure = Assert.Single(context.Messages.OpenFailures);
        Assert.Equal(ImageOpenProblem.InvalidPath, failure.Problem);
        Assert.False(Vm.IsImageOpen);
    }

    [Fact]
    public async Task AnEmptyFileIsRejected()
    {
        await context.OpenAsync(context.WriteFile("empty.img", []));

        Assert.Equal(new ImageOpenFailure(ImageOpenProblem.EmptyFile, "empty.img"), Assert.Single(context.Messages.OpenFailures));
        Assert.False(Vm.IsImageOpen);
        Assert.Equal("TotalImage", Vm.Title);
    }

    [Fact]
    public async Task AMissingFileIsReported()
    {
        await context.OpenAsync(context.PathOf("missing.img"));

        Assert.Equal(new ImageOpenFailure(ImageOpenProblem.FileNotFound, "missing.img"), Assert.Single(context.Messages.OpenFailures));
        Assert.False(Vm.IsImageOpen);
        Assert.Empty(Vm.RecentImages);
    }

    [Fact]
    public async Task ALockedFileIsReported()
    {
        var path = context.WriteFloppy();
        using (new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
            await context.OpenAsync(path);
        }

        if (!OperatingSystem.IsWindows())
            return; // Only Windows reports sharing violations.

        Assert.Equal(ImageOpenProblem.FileLocked, Assert.Single(context.Messages.OpenFailures).Problem);
        Assert.False(Vm.IsImageOpen);
    }

    [Fact]
    public async Task AnUnknownFormatIsReportedWithTheError()
    {
        await context.OpenAsync(context.WriteFile("disk.xyz", PartitionTestImages.CreateFatVolume()));

        var failure = Assert.Single(context.Messages.OpenFailures);
        Assert.Equal(ImageOpenProblem.ReadFailed, failure.Problem);
        Assert.Equal("disk.xyz", failure.FileName);
        Assert.Equal("This container format is not recognized and cannot be opened.", failure.ErrorMessage);
        Assert.False(Vm.IsImageOpen);
    }

    [Fact]
    public async Task AnImageWithoutAFileSystemIsRejected()
    {
        var path = context.WriteFile("blank.img", new byte[64 * 1024]);

        await context.OpenAsync(path);

        Assert.Equal(new ImageOpenFailure(ImageOpenProblem.NoSupportedFileSystem, "blank.img", ContainerName: "Raw sector image"),
            Assert.Single(context.Messages.OpenFailures));
        Assert.False(Vm.IsImageOpen);
        // As before, the file was added to the recent images once it could be read.
        Assert.Equal([path], Vm.RecentImages);
    }

    [Fact]
    public async Task ASinglePartitionWithoutAFileSystemIsRejected()
    {
        await context.OpenAsync(context.WriteGptDisk());

        var failure = Assert.Single(context.Messages.OpenFailures);
        Assert.Equal(ImageOpenProblem.UnsupportedPartition, failure.Problem);
        Assert.Equal("GUID Partition Table", failure.PartitionTableName);
        Assert.False(Vm.IsImageOpen);
    }

    [Fact]
    public async Task SeveralPartitionsWithoutAFileSystemAreRejected()
    {
        await context.OpenAsync(context.WriteUnsupportedMbrDisk());

        var failure = Assert.Single(context.Messages.OpenFailures);
        Assert.Equal(ImageOpenProblem.NoSupportedPartitions, failure.Problem);
        Assert.Equal("Master Boot Record", failure.PartitionTableName);
        Assert.Equal(0, context.ImageDialogs.SelectPartitionRequests);
        Assert.False(Vm.IsImageOpen);
    }

    [Fact]
    public async Task SeveralPartitionsAskWhichOneToLoad()
    {
        context.ImageDialogs.SelectPartition = _ => new PartitionSelection(0, false);

        await context.OpenAsync(context.WriteMbrDisk());

        Assert.Equal(1, context.ImageDialogs.SelectPartitionRequests);
        Assert.Equal(0, Vm.CurrentPartitionIndex);
        Assert.IsAssignableFrom<FatFileSystem>(Vm.CurrentFileSystem);
        // The unsupported partition isn't offered.
        var option = Assert.Single(Vm.Partitions);
        Assert.Equal(0, option.Index);
        Assert.StartsWith("0: ", option.Text);
    }

    [Fact]
    public async Task NotChoosingAPartitionOpensNothing()
    {
        await context.OpenAsync(context.WriteMbrDisk());

        Assert.Equal(1, context.ImageDialogs.SelectPartitionRequests);
        Assert.Empty(context.Messages.OpenFailures);
        Assert.False(Vm.IsImageOpen);
    }

    [Fact]
    public async Task SelectPartitionLoadsAnotherPartition()
    {
        context.ImageDialogs.SelectPartition = _ => new PartitionSelection(0, false);
        await context.OpenAsync(context.WriteMbrDisk());
        var changed = new List<string?>();
        Vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        Vm.SelectPartition(1);

        Assert.Equal(1, Vm.CurrentPartitionIndex);
        Assert.Same(Vm.Image!.PartitionTable.Partitions[1].FileSystem, Vm.CurrentFileSystem);
        Assert.Contains(nameof(MainViewModel.CurrentFileSystem), changed);
        Assert.Contains(nameof(MainViewModel.PartitionSizeText), changed);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(2)]
    public async Task SelectPartitionIgnoresTheCurrentOrAMissingPartition(int index)
    {
        context.ImageDialogs.SelectPartition = _ => new PartitionSelection(0, false);
        await context.OpenAsync(context.WriteMbrDisk());
        var fileSystem = Vm.CurrentFileSystem;

        Vm.SelectPartition(index);

        Assert.Equal(0, Vm.CurrentPartitionIndex);
        Assert.Same(fileSystem, Vm.CurrentFileSystem);
    }

    [Fact]
    public void SelectPartitionWithoutAnImageDoesNothing()
    {
        Vm.SelectPartition(1);

        Assert.Equal(0, Vm.CurrentPartitionIndex);
    }

    [Fact]
    public async Task ThePartitionDialogLoadsTheChosenPartition()
    {
        context.ImageDialogs.SelectPartition = _ => new PartitionSelection(0, false);
        await context.OpenAsync(context.WriteMbrDisk());

        context.ImageDialogs.SelectPartition = _ => new PartitionSelection(1, false);
        await Vm.ChoosePartitionCommand.ExecuteAsync(null);
        Assert.Equal(1, Vm.CurrentPartitionIndex);

        context.ImageDialogs.SelectPartition = _ => null;
        await Vm.ChoosePartitionCommand.ExecuteAsync(null);
        Assert.Equal(1, Vm.CurrentPartitionIndex);
    }

    [Fact]
    public async Task PendingChangesMarkTheImageUnsaved()
    {
        await context.OpenFloppyAsync();
        var entries = Vm.Entries;
        var changed = new List<string?>();
        Vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        AddChange();

        Assert.True(Vm.HasUnsavedChanges);
        Assert.Equal("*disk.img - TotalImage", Vm.Title);
        Assert.NotSame(entries, Vm.Entries);
        Assert.Contains(nameof(MainViewModel.HasUnsavedChanges), changed);
        Assert.Contains(nameof(MainViewModel.Title), changed);
    }

    [Fact]
    public async Task SavingWritesTheChangesAndReopensTheFile()
    {
        var path = await context.OpenFloppyAsync();
        var image = Vm.Image;
        AddChange();

        await Vm.SaveCommand.ExecuteAsync(null);

        Assert.Equal([path], context.ImageFiles.Saved);
        Assert.Empty(context.FilePicker.SaveSuggestions);
        Assert.True(Vm.IsImageOpen);
        Assert.NotSame(image, Vm.Image);
        Assert.Equal(path, Vm.FilePath);
        Assert.False(Vm.HasUnsavedChanges);
        Assert.Equal("disk.img - TotalImage", Vm.Title);
        Assert.False(Vm.IsBusy);
        Vm.Dispose();
        Assert.True(DirectoryExists(path, "NEWDIR"));
    }

    [Fact]
    public async Task SavingKeepsTheLoadedPartitionWithoutAskingAgain()
    {
        context.ImageDialogs.SelectPartition = _ => new PartitionSelection(0, false);
        await context.OpenAsync(context.WriteMbrDisk());
        AddChange();

        await Vm.SaveCommand.ExecuteAsync(null);

        Assert.Equal(1, context.ImageDialogs.SelectPartitionRequests);
        Assert.Equal(0, Vm.CurrentPartitionIndex);
        Assert.True(Vm.IsImageOpen);
    }

    [Fact]
    public async Task TheReopenedImageTracksNewChanges()
    {
        await context.OpenFloppyAsync();
        AddChange();
        await Vm.SaveCommand.ExecuteAsync(null);

        AddChange("OTHER");

        Assert.True(Vm.HasUnsavedChanges);
    }

    [Fact]
    public async Task ASaveFailureIsReportedAndKeepsTheChanges()
    {
        await context.OpenFloppyAsync();
        var image = Vm.Image;
        AddChange();
        var error = new IOException("Disk full");
        context.ImageFiles.SaveException = error;

        await Vm.SaveCommand.ExecuteAsync(null);

        var (operation, exception) = Assert.Single(context.Messages.Errors);
        Assert.Equal("Saving \"disk.img\"", operation);
        Assert.Same(error, exception);
        Assert.Same(image, Vm.Image);
        Assert.True(Vm.HasUnsavedChanges);
        Assert.False(Vm.IsBusy);

        // The image is still watched for changes.
        var entries = Vm.Entries;
        AddChange("OTHER");
        Assert.NotSame(entries, Vm.Entries);
    }

    [Fact]
    public async Task SaveAsWritesANewFileAndSwitchesToIt()
    {
        var original = await context.OpenFloppyAsync();
        AddChange();
        var target = context.PathOf("copy.img");
        context.FilePicker.SavePath = target;

        await Vm.SaveAsCommand.ExecuteAsync(null);

        Assert.Equal([null], context.FilePicker.SaveSuggestions);
        Assert.Equal(target, Vm.FilePath);
        Assert.Equal("copy.img - TotalImage", Vm.Title);
        Assert.False(Vm.HasUnsavedChanges);
        Assert.Equal([target, original], Vm.RecentImages);
        Vm.Dispose();
        Assert.True(DirectoryExists(target, "NEWDIR"));
        Assert.False(DirectoryExists(original, "NEWDIR"));
    }

    [Fact]
    public async Task CancellingSaveAsChangesNothing()
    {
        var path = await context.OpenFloppyAsync();
        AddChange();
        context.FilePicker.SavePath = null;

        await Vm.SaveAsCommand.ExecuteAsync(null);

        Assert.Empty(context.ImageFiles.Saved);
        Assert.Equal(path, Vm.FilePath);
        Assert.True(Vm.HasUnsavedChanges);
    }

    [Fact]
    public async Task SaveAsIgnoresFormatsThatCantBeWritten()
    {
        var path = await context.OpenFloppyAsync();
        AddChange();
        context.FilePicker.SavePath = context.PathOf("disk.vhd");

        await Vm.SaveAsCommand.ExecuteAsync(null);

        Assert.Empty(context.ImageFiles.Saved);
        Assert.False(File.Exists(context.PathOf("disk.vhd")));
        Assert.Equal(path, Vm.FilePath);
        Assert.True(Vm.HasUnsavedChanges);
    }

    [Theory]
    [InlineData("disk1.img", "disk2")]
    [InlineData("disk009.img", "disk010")]
    [InlineData("disk99.ima", "disk100")]
    [InlineData("7.img", "8")]
    public async Task SaveAsOffersTheNextNumberedName(string savedAs, string suggested)
    {
        await context.OpenFloppyAsync();
        context.FilePicker.SavePath = context.PathOf(savedAs);
        await Vm.SaveAsCommand.ExecuteAsync(null);

        await Vm.SaveAsCommand.ExecuteAsync(null);

        Assert.Equal([null, suggested], context.FilePicker.SaveSuggestions);
    }

    [Fact]
    public async Task SaveAsOffersNoNameAfterANameWithoutANumber()
    {
        await context.OpenFloppyAsync();
        context.FilePicker.SavePath = context.PathOf("copy.img");
        await Vm.SaveAsCommand.ExecuteAsync(null);

        await Vm.SaveAsCommand.ExecuteAsync(null);

        Assert.Equal([null, null], context.FilePicker.SaveSuggestions);
    }

    [Fact]
    public async Task SaveAsOffersNoNameWhenAutoIncrementIsOff()
    {
        context.Settings.Current.AutoIncrementFilename = false;
        await context.OpenFloppyAsync();
        context.FilePicker.SavePath = context.PathOf("disk1.img");
        await Vm.SaveAsCommand.ExecuteAsync(null);

        await Vm.SaveAsCommand.ExecuteAsync(null);

        Assert.Equal([null, null], context.FilePicker.SaveSuggestions);
    }

    [Fact]
    public async Task TurningAutoIncrementOffForgetsTheLastName()
    {
        await context.OpenFloppyAsync();
        context.FilePicker.SavePath = context.PathOf("disk1.img");
        await Vm.SaveAsCommand.ExecuteAsync(null);
        context.AppDialogs.SettingsAnswer = new SettingsChangeResult(false);
        context.AppDialogs.WhileSettingsOpen = () => context.Settings.Current.AutoIncrementFilename = false;
        await Vm.ShowSettingsCommand.ExecuteAsync(null);
        context.Settings.Current.AutoIncrementFilename = true;

        await Vm.SaveAsCommand.ExecuteAsync(null);

        Assert.Equal([null, null], context.FilePicker.SaveSuggestions);
    }

    [Fact]
    public async Task NewImageCreatesAnUntitledUnsavedImage()
    {
        context.ImageDialogs.NewImageAnswer = FloppyOptions();

        await Vm.NewImageCommand.ExecuteAsync(null);

        Assert.True(Vm.IsImageOpen);
        Assert.Null(Vm.FilePath);
        Assert.Null(Vm.FileName);
        Assert.True(Vm.HasUnsavedChanges);
        Assert.Equal("*(Untitled) - TotalImage", Vm.Title);
        Assert.IsAssignableFrom<FatFileSystem>(Vm.CurrentFileSystem);
        Assert.Equal(1_474_560, Vm.CurrentPartition!.Length);
        Assert.Empty(Vm.RecentImages);
    }

    [Fact]
    public async Task CancellingNewImageKeepsTheCurrentImage()
    {
        await context.OpenFloppyAsync();
        var image = Vm.Image;

        await Vm.NewImageCommand.ExecuteAsync(null);

        Assert.Same(image, Vm.Image);
    }

    [Fact]
    public async Task SavingANewImageAsksWhereToSaveIt()
    {
        context.ImageDialogs.NewImageAnswer = FloppyOptions();
        await Vm.NewImageCommand.ExecuteAsync(null);
        var target = context.PathOf("new.img");
        context.FilePicker.SavePath = target;

        await Vm.SaveCommand.ExecuteAsync(null);

        Assert.Single(context.FilePicker.SaveSuggestions);
        Assert.Equal(target, Vm.FilePath);
        Assert.False(Vm.HasUnsavedChanges);
        Assert.Equal(1_474_560, new FileInfo(target).Length);
    }

    [Fact]
    public async Task ReloadingDiscardsNothingWithoutChanges()
    {
        var path = await context.OpenFloppyAsync();
        var image = Vm.Image;

        await Vm.ReloadImageCommand.ExecuteAsync(null);

        Assert.Empty(context.Messages.UnsavedQuestions);
        Assert.NotSame(image, Vm.Image);
        Assert.Equal(path, Vm.FilePath);
    }

    [Fact]
    public async Task ClosingClearsTheSession()
    {
        await context.OpenFloppyAsync();
        var image = Vm.Image!;
        Assert.NotNull(Vm.CurrentFolder);

        await Vm.CloseImageCommand.ExecuteAsync(null);

        Assert.False(Vm.IsImageOpen);
        Assert.Null(Vm.FilePath);
        Assert.Null(Vm.CurrentFileSystem);
        Assert.Empty(Vm.Partitions);
        Assert.Equal(0, Vm.CurrentPartitionIndex);
        Assert.Equal("TotalImage", Vm.Title);
        Assert.Null(Vm.RootFolder);
        Assert.Null(Vm.CurrentFolder);
        Assert.Empty(Vm.Entries);
        Assert.Empty(Vm.SelectedEntries);
        Assert.Equal(string.Empty, Vm.PartitionSizeText);
        Assert.Equal(string.Empty, Vm.LocationText);
        Assert.Throws<ObjectDisposedException>(() => image.Content.Position = 0);
    }

    [Fact]
    public async Task DisposingClosesTheImage()
    {
        await context.OpenFloppyAsync();
        var image = Vm.Image!;
        var changed = new List<string?>();
        Vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        Vm.Dispose();

        Assert.Throws<ObjectDisposedException>(() => image.Content.Position = 0);
        // The view may be disposed already, so it isn't notified.
        Assert.Empty(changed);
    }

    [Fact]
    public async Task OpenContainingFolderRevealsTheImageFile()
    {
        var path = await context.OpenFloppyAsync();

        Vm.OpenContainingFolderCommand.Execute(null);

        Assert.Equal([path], context.Launcher.Revealed);
    }

    [Fact]
    public async Task TheImageIsBusyWhileOpening()
    {
        var path = context.WriteFloppy();
        context.FilePicker.OpenPath = path;
        context.ImageFiles.OpenGate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var opening = Vm.OpenImageFileAsync(path);

        Assert.True(Vm.IsBusy);
        Assert.False(Vm.NewImageCommand.CanExecute(null));
        Assert.False(Vm.OpenImageCommand.CanExecute(null));
        Assert.False(Vm.OpenRecentImageCommand.CanExecute(path));

        context.ImageFiles.OpenGate.SetResult();
        await opening;

        Assert.False(Vm.IsBusy);
        Assert.True(Vm.OpenImageCommand.CanExecute(null));
    }
}
