using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Main;
using Xunit;

namespace TotalImage.ViewModels.Tests.Main;

/// <summary>
/// Dragging entries out of the window and dropping files onto it.
/// </summary>
public class MainViewModelDragDropTests : IDisposable
{
    private readonly MainTestContext context = new();

    private MainViewModel Vm => context.ViewModel;

    public void Dispose() => context.Dispose();

    private FolderViewModel Docs => Vm.RootFolder!.Children.Single(folder => folder.Name == "DOCS");

    private EntryViewModel Entry(string name) => Vm.Entries.Single(entry => entry.Name == name);

    [Fact]
    public async Task DraggingEntriesExtractsThemToANewTemporaryFolder()
    {
        await context.OpenBrowseImageAsync();
        context.Settings.Current.ConfirmOverwriteExtraction = true;

        var paths = await Vm.PrepareDragOutAsync([Entry("ALPHA.TXT"), Entry("DOCS")]);

        Assert.Equal(["ALPHA.TXT", "DOCS"], paths.Select(Path.GetFileName));
        var folder = Path.GetDirectoryName(paths[0])!;
        Assert.Equal(context.TempDirectory.Root, Path.GetDirectoryName(folder));
        Assert.True(File.Exists(paths[0]));
        Assert.True(File.Exists(Path.Combine(paths[1], "DEEP", "X.TXT")));
        Assert.Single(context.EntryDialogs.ProgressShown);
        Assert.Empty(context.Messages.OverwriteQuestions);
        Assert.Empty(context.EntryDialogs.ExtractRequests);
    }

    [Fact]
    public async Task TheParentEntryAndPendingEntriesAreNotDragged()
    {
        await context.OpenBrowseImageAsync();
        Vm.CurrentFolder = Docs;
        Vm.Image!.PendingChanges.Add(new Changes.CreateDirectoryChange(["DOCS", "NEW"]));

        var paths = await Vm.PrepareDragOutAsync([Entry(EntryViewModel.ParentEntryName)]);
        Assert.Empty(paths);

        paths = await Vm.PrepareDragOutAsync([Entry("NEW"), Entry("A.TXT")]);
        Assert.Equal(["A.TXT"], paths.Select(Path.GetFileName));
    }

    [Fact]
    public async Task NothingIsExtractedWhenNothingCanBeDragged()
    {
        await context.OpenBrowseImageAsync();

        var paths = await Vm.PrepareDragOutAsync([]);

        Assert.Empty(paths);
        Assert.Empty(context.EntryDialogs.ProgressShown);
    }

    [Fact]
    public async Task DraggingASubfolderOfTheTreeExtractsIt()
    {
        await context.OpenBrowseImageAsync();

        var paths = await Vm.PrepareDragOutAsync(Docs);

        var path = Assert.Single(paths);
        Assert.Equal("DOCS", Path.GetFileName(path));
        Assert.True(File.Exists(Path.Combine(path, "A.TXT")));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DraggingTheRootFolderDragsItsEntries(bool showHidden)
    {
        await context.OpenBrowseImageAsync();
        if (!showHidden)
            Vm.ToggleShowHiddenItemsCommand.Execute(null);

        var paths = await Vm.PrepareDragOutAsync(Vm.RootFolder!);

        string[] expected = ["DOCS", "EMPTY", "ALPHA.TXT", "MID.BIN", "ZETA.ARC", "HIDDEN.SYS"];
        Assert.Equal(expected.Where(name => showHidden || name != "HIDDEN.SYS").Order(), paths.Select(Path.GetFileName).Order());
        Assert.True(File.Exists(paths.Single(path => path.EndsWith("ALPHA.TXT"))));
    }

    [Fact]
    public async Task APendingFolderIsNotDragged()
    {
        await context.OpenBrowseImageAsync();
        Vm.Image!.PendingChanges.Add(new Changes.CreateDirectoryChange(["NEW"]));

        var paths = await Vm.PrepareDragOutAsync(Vm.RootFolder!.Children.Single(folder => folder.Name == "NEW"));

        Assert.Empty(paths);
    }

    [Fact]
    public async Task ACancelledExtractionDragsNothingAndLeavesNoFiles()
    {
        await context.OpenBrowseImageAsync();
        context.EntryDialogs.CancelWhen = _ => true;

        var paths = await Vm.PrepareDragOutAsync([Entry("ALPHA.TXT")]);

        Assert.Empty(paths);
        Assert.Empty(TemporaryFolders());
    }

    [Fact]
    public async Task AnUndroppedDragDeletesItsFiles()
    {
        await context.OpenBrowseImageAsync();
        var paths = await Vm.PrepareDragOutAsync([Entry("ALPHA.TXT")]);

        Vm.EndDragOut(dropped: false);

        Assert.False(File.Exists(paths[0]));
        Assert.Empty(TemporaryFolders());
    }

    [Fact]
    public async Task ADroppedDragKeepsItsFiles()
    {
        await context.OpenBrowseImageAsync();
        var paths = await Vm.PrepareDragOutAsync([Entry("ALPHA.TXT")]);

        Vm.EndDragOut(dropped: true);
        Vm.EndDragOut(dropped: false);

        Assert.True(File.Exists(paths[0]));
    }

    [Fact]
    public async Task ANewDragDeletesTheFilesOfOneThatNeverEnded()
    {
        await context.OpenBrowseImageAsync();
        var first = await Vm.PrepareDragOutAsync([Entry("ALPHA.TXT")]);

        var second = await Vm.PrepareDragOutAsync([Entry("MID.BIN")]);

        Assert.False(File.Exists(first[0]));
        Assert.True(File.Exists(second[0]));
    }

    private string[] TemporaryFolders() => System.IO.Directory.Exists(context.TempDirectory.Root)
        ? System.IO.Directory.GetDirectories(context.TempDirectory.Root)
        : [];

    [Fact]
    public async Task ASingleFileCanBeDroppedOnlyWithoutAnOpenImage()
    {
        Assert.True(Vm.CanAcceptDrop([context.WriteFloppy()]));
        Assert.False(Vm.CanAcceptDrop([]));
        Assert.False(Vm.CanAcceptDrop(["a.img", "b.img"]));

        await context.OpenFloppyAsync("open.img");

        Assert.False(Vm.CanAcceptDrop([context.WriteFloppy()]));
    }

    [Fact]
    public async Task NothingCanBeDroppedWhileAnImageIsOpening()
    {
        context.ImageFiles.OpenGate = new TaskCompletionSource();
        var opening = context.OpenAsync(context.WriteFloppy("slow.img"));

        Assert.False(Vm.CanAcceptDrop([context.WriteFloppy()]));

        context.ImageFiles.OpenGate.SetResult();
        await opening;
    }

    [Fact]
    public async Task DroppingAFileOpensIt()
    {
        var path = context.WriteFloppy();

        await Vm.DropAsync([path]);

        Assert.True(Vm.IsImageOpen);
        Assert.Equal(path, Vm.FilePath);
    }

    [Fact]
    public async Task DroppingWhatCantBeAcceptedDoesNothing()
    {
        await Vm.DropAsync([context.WriteFloppy("a.img"), context.WriteFloppy("b.img")]);
        Assert.False(Vm.IsImageOpen);

        var open = await context.OpenFloppyAsync("open.img");
        await Vm.DropAsync([context.WriteFloppy("c.img")]);
        Assert.Equal(open, Vm.FilePath);
    }

    [Fact]
    public async Task ClosingTheWindowSavesTheSettingsAndClearsTheTemporaryFolder()
    {
        await context.OpenBrowseImageAsync();
        var paths = await Vm.PrepareDragOutAsync([Entry("ALPHA.TXT")]);
        Vm.EndDragOut(dropped: true);

        Vm.OnWindowClosing();

        Assert.Equal(1, context.Settings.Saves);
        Assert.False(File.Exists(paths[0]));
        Assert.Empty(TemporaryFolders());
    }

    [Fact]
    public async Task TheTemporaryFolderIsKeptWhileOtherInstancesRun()
    {
        await context.OpenBrowseImageAsync();
        var paths = await Vm.PrepareDragOutAsync([Entry("ALPHA.TXT")]);
        Vm.EndDragOut(dropped: true);
        context.AppInstances.OthersRunning = true;

        Vm.OnWindowClosing();

        Assert.Equal(1, context.Settings.Saves);
        Assert.True(File.Exists(paths[0]));
    }
}
