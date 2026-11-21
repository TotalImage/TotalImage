using System;
using System.Linq;
using System.Threading.Tasks;
using TotalImage.Changes;
using TotalImage.DiskGeometries;
using TotalImage.FileSystems.BPB;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Main;
using TotalImage.ViewModels.Services;
using Xunit;

namespace TotalImage.ViewModels.Tests.Main;

/// <summary>
/// What happens to unsaved changes when the image is replaced, closed or reloaded, or the window closes.
/// </summary>
public class MainViewModelUnsavedChangesTests : IDisposable
{
    private readonly MainTestContext context = new();

    private MainViewModel Vm => context.ViewModel;

    public void Dispose() => context.Dispose();

    private async Task<string> OpenChangedFloppyAsync()
    {
        var path = await context.OpenFloppyAsync();
        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["NEWDIR"]));
        return path;
    }

    [Fact]
    public async Task CancellingKeepsTheImageWhenClosing()
    {
        await OpenChangedFloppyAsync();
        var image = Vm.Image;

        await Vm.CloseImageCommand.ExecuteAsync(null);

        Assert.Equal([UnsavedChangesReason.CloseImage], context.Messages.UnsavedQuestions);
        Assert.Same(image, Vm.Image);
        Assert.True(Vm.HasUnsavedChanges);
    }

    [Fact]
    public async Task DiscardingClosesWithoutSaving()
    {
        await OpenChangedFloppyAsync();
        context.Messages.UnsavedAnswer = UnsavedChangesAnswer.Discard;

        await Vm.CloseImageCommand.ExecuteAsync(null);

        Assert.False(Vm.IsImageOpen);
        Assert.Empty(context.ImageFiles.Saved);
    }

    [Fact]
    public async Task SavingFirstWritesTheChangesThenCloses()
    {
        var path = await OpenChangedFloppyAsync();
        context.Messages.UnsavedAnswer = UnsavedChangesAnswer.Save;

        await Vm.CloseImageCommand.ExecuteAsync(null);

        Assert.Equal([path], context.ImageFiles.Saved);
        Assert.False(Vm.IsImageOpen);
    }

    [Fact]
    public async Task AFailedSaveKeepsTheImageOpen()
    {
        await OpenChangedFloppyAsync();
        context.Messages.UnsavedAnswer = UnsavedChangesAnswer.Save;
        context.ImageFiles.SaveException = new UnauthorizedAccessException();

        await Vm.CloseImageCommand.ExecuteAsync(null);

        Assert.Single(context.Messages.Errors);
        Assert.True(Vm.IsImageOpen);
        Assert.True(Vm.HasUnsavedChanges);
    }

    [Fact]
    public async Task SavingAnUntitledImageFirstAsksWhereToSaveIt()
    {
        context.ImageDialogs.NewImageAnswer = new(FloppyGeometry.KnownGeometries[FloppyGeometry.FriendlyName.HighDensity1440k],
            true, BiosParameterBlockVersion.Dos40, "MSDOS5.0", "1234ABCD", "FAT12", "NEWDISK");
        await Vm.NewImageCommand.ExecuteAsync(null);
        context.Messages.UnsavedAnswer = UnsavedChangesAnswer.Save;

        // Cancelling the save picker cancels closing.
        await Vm.CloseImageCommand.ExecuteAsync(null);
        Assert.True(Vm.IsImageOpen);

        context.FilePicker.SavePath = context.PathOf("new.img");
        await Vm.CloseImageCommand.ExecuteAsync(null);
        Assert.False(Vm.IsImageOpen);
        Assert.Equal([context.PathOf("new.img")], context.ImageFiles.Saved);
    }

    [Fact]
    public async Task OpeningAsksBeforeShowingThePicker()
    {
        await OpenChangedFloppyAsync();
        var image = Vm.Image;

        await Vm.OpenImageCommand.ExecuteAsync(null);

        Assert.Equal([UnsavedChangesReason.OpenImage], context.Messages.UnsavedQuestions);
        Assert.Equal(1, context.FilePicker.OpenRequests);
        Assert.Same(image, Vm.Image);
    }

    [Fact]
    public async Task OpeningAfterDiscardingReplacesTheImage()
    {
        await OpenChangedFloppyAsync();
        context.Messages.UnsavedAnswer = UnsavedChangesAnswer.Discard;
        var other = context.WriteFloppy("other.img");
        context.FilePicker.OpenPath = other;

        await Vm.OpenImageCommand.ExecuteAsync(null);

        Assert.Equal(other, Vm.FilePath);
        Assert.False(Vm.HasUnsavedChanges);
    }

    [Fact]
    public async Task OpenImageFileAsyncAsksToo()
    {
        await OpenChangedFloppyAsync();
        var image = Vm.Image;

        Assert.False(await Vm.OpenImageFileAsync(context.WriteFloppy("other.img")));

        Assert.Equal([UnsavedChangesReason.OpenImage], context.Messages.UnsavedQuestions);
        Assert.Same(image, Vm.Image);
    }

    [Fact]
    public async Task OpeningARecentImageAsksToo()
    {
        await OpenChangedFloppyAsync();
        var image = Vm.Image;
        var other = context.WriteFloppy("other.img");

        await Vm.OpenRecentImageCommand.ExecuteAsync(other);

        Assert.Equal([UnsavedChangesReason.OpenImage], context.Messages.UnsavedQuestions);
        Assert.Same(image, Vm.Image);
    }

    [Fact]
    public async Task NewImageAsksAfterTheNewImageDialog()
    {
        await OpenChangedFloppyAsync();
        var image = Vm.Image;

        await Vm.NewImageCommand.ExecuteAsync(null);
        Assert.Empty(context.Messages.UnsavedQuestions);

        context.ImageDialogs.NewImageAnswer = new(FloppyGeometry.KnownGeometries[FloppyGeometry.FriendlyName.HighDensity1440k],
            true, BiosParameterBlockVersion.Dos40, "MSDOS5.0", "1234ABCD", "FAT12", "NEWDISK");
        await Vm.NewImageCommand.ExecuteAsync(null);

        Assert.Equal([UnsavedChangesReason.NewImage], context.Messages.UnsavedQuestions);
        Assert.Same(image, Vm.Image);
    }

    [Fact]
    public async Task ReloadingAfterDiscardingDropsTheChanges()
    {
        var path = await OpenChangedFloppyAsync();
        context.Messages.UnsavedAnswer = UnsavedChangesAnswer.Discard;

        await Vm.ReloadImageCommand.ExecuteAsync(null);

        Assert.Equal([UnsavedChangesReason.ReloadImage], context.Messages.UnsavedQuestions);
        Assert.Equal(path, Vm.FilePath);
        Assert.False(Vm.HasUnsavedChanges);
        Assert.Empty(Vm.CurrentFileSystem!.RootDirectory.EnumerateDirectories(true));
    }

    [Fact]
    public async Task CancellingKeepsTheImageWhenReloading()
    {
        await OpenChangedFloppyAsync();
        var image = Vm.Image;

        await Vm.ReloadImageCommand.ExecuteAsync(null);

        Assert.Same(image, Vm.Image);
    }

    [Fact]
    public async Task ExitWithoutChangesClosesTheWindow()
    {
        var closeRequests = 0;
        Vm.CloseRequested += (_, _) => closeRequests++;

        await Vm.ExitCommand.ExecuteAsync(null);

        Assert.Equal(1, closeRequests);
        Assert.True(Vm.IsCloseConfirmed);
        Assert.Empty(context.Messages.UnsavedQuestions);
    }

    [Fact]
    public async Task CancellingExitKeepsTheWindowOpen()
    {
        await OpenChangedFloppyAsync();
        var closeRequests = 0;
        Vm.CloseRequested += (_, _) => closeRequests++;

        await Vm.ExitCommand.ExecuteAsync(null);

        Assert.Equal([UnsavedChangesReason.ExitApplication], context.Messages.UnsavedQuestions);
        Assert.Equal(0, closeRequests);
        Assert.False(Vm.IsCloseConfirmed);
    }

    [Fact]
    public async Task ClosingTheWindowAsksOnlyOnce()
    {
        await OpenChangedFloppyAsync();
        context.Messages.UnsavedAnswer = UnsavedChangesAnswer.Discard;

        Assert.True(await Vm.ConfirmCloseWindowAsync());
        Assert.True(await Vm.ConfirmCloseWindowAsync());

        Assert.Single(context.Messages.UnsavedQuestions);
        Assert.True(Vm.IsCloseConfirmed);
    }

    [Fact]
    public async Task SavingBeforeExitWritesTheChanges()
    {
        var path = await OpenChangedFloppyAsync();
        context.Messages.UnsavedAnswer = UnsavedChangesAnswer.Save;

        Assert.True(await Vm.ConfirmCloseWindowAsync());

        Assert.Equal([path], context.ImageFiles.Saved);
        Assert.False(Vm.HasUnsavedChanges);
    }

    [Fact]
    public async Task ANewImageCountsAsUnsaved()
    {
        context.ImageDialogs.NewImageAnswer = new(FloppyGeometry.KnownGeometries[FloppyGeometry.FriendlyName.HighDensity1440k],
            true, BiosParameterBlockVersion.Dos40, "MSDOS5.0", "1234ABCD", "FAT12", "NEWDISK");
        await Vm.NewImageCommand.ExecuteAsync(null);

        await Vm.CloseImageCommand.ExecuteAsync(null);

        Assert.Equal([UnsavedChangesReason.CloseImage], context.Messages.UnsavedQuestions);
        Assert.True(Vm.IsImageOpen);
    }
}
