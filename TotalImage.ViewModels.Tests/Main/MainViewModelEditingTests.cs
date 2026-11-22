using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TotalImage.Changes;
using TotalImage.FileSystems.FAT;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Main;
using TotalImage.ViewModels.Services;
using Xunit;

namespace TotalImage.ViewModels.Tests.Main;

/// <summary>
/// Adding files, creating folders, deleting and renaming: every edit goes into the change set.
/// </summary>
public class MainViewModelEditingTests : IDisposable
{
    private readonly MainTestContext context = new();

    private MainViewModel Vm => context.ViewModel;

    public void Dispose() => context.Dispose();

    private ChangeSet Changes => Vm.Image!.PendingChanges;

    private FolderViewModel Docs => Vm.RootFolder!.Children.Single(folder => folder.Name == "DOCS");

    private EntryViewModel Entry(string name) => Vm.Entries.Single(entry => entry.Name == name);

    private string HostFile(string name, int length) => context.WriteFile(name, new byte[length]);

    #region Inject

    [Fact]
    // FatDirectory.EnqueueAddFile and EnqueueCreateSubdirectory queue the change for the parent
    // of a subfolder (TotalImage.IO), so these tests use the root folder.
    public async Task InjectedFilesAreAddedToTheCurrentFolder()
    {
        await context.OpenBrowseImageAsync();
        context.FilePicker.FilesToAdd = [HostFile("NEW1.TXT", 7), HostFile("NEW2.TXT", 3)];

        await Vm.InjectFilesCommand.ExecuteAsync(null);

        var added = Changes.Changes.OfType<AddFileChange>().ToList();
        Assert.Equal([["NEW1.TXT"], ["NEW2.TXT"]], added.Select(change => change.DestinationPath));
        Assert.Equal([7L, 3L], added.Select(change => change.Source.Length));
        Assert.All(added, change => Assert.Equal(FatAttributes.Archive, change.Attributes));
        Assert.True(Vm.HasUnsavedChanges);
        Assert.Equal(PendingState.Added, Entry("NEW1.TXT").PendingState);
    }

    [Fact]
    public async Task InjectingIsConfirmedWithTheCountAndSize()
    {
        await context.OpenBrowseImageAsync();
        context.FilePicker.FilesToAdd = [HostFile("A.BIN", 1000), HostFile("B.BIN", 24)];

        await Vm.InjectFilesCommand.ExecuteAsync(null);

        var (count, size) = Assert.Single(context.Messages.InjectQuestions);
        Assert.Equal(2, count);
        Assert.Equal(context.Settings.Current.SizeUnit.FormatSize(1024), size);
    }

    [Fact]
    public async Task DecliningTheInjectionAddsNothing()
    {
        await context.OpenBrowseImageAsync();
        context.FilePicker.FilesToAdd = [HostFile("A.BIN", 1)];
        context.Messages.InjectAnswer = new(false);

        await Vm.InjectFilesCommand.ExecuteAsync(null);

        Assert.False(Changes.IsDirty);
        Assert.True(context.Settings.Current.ConfirmInjection);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task NotAskingAgainTurnsTheInjectionQuestionOff(bool confirmed)
    {
        await context.OpenBrowseImageAsync();
        context.FilePicker.FilesToAdd = [HostFile("A.BIN", 1)];
        context.Messages.InjectAnswer = new(confirmed, DontAskAgain: true);

        await Vm.InjectFilesCommand.ExecuteAsync(null);
        context.FilePicker.FilesToAdd = [HostFile("B.BIN", 1)];
        await Vm.InjectFilesCommand.ExecuteAsync(null);

        Assert.False(context.Settings.Current.ConfirmInjection);
        Assert.Single(context.Messages.InjectQuestions);
        Assert.Equal(confirmed ? 2 : 1, Changes.Changes.Count);
    }

    [Fact]
    public async Task PickingNoFilesDoesNothing()
    {
        await context.OpenBrowseImageAsync();

        await Vm.InjectFilesCommand.ExecuteAsync(null);

        Assert.Empty(context.Messages.InjectQuestions);
        Assert.False(Changes.IsDirty);
    }

    [Fact]
    public async Task FilesCanBeInjectedIntoAPendingFolder()
    {
        await context.OpenBrowseImageAsync();
        context.Settings.Current.ConfirmInjection = false;
        Changes.Add(new CreateDirectoryChange(["DOCS", "NEW"]));
        Vm.CurrentFolder = Docs.Children.Single(folder => folder.Name == "NEW");
        context.FilePicker.FilesToAdd = [HostFile("IN.TXT", 2)];

        await Vm.InjectFilesCommand.ExecuteAsync(null);

        Assert.Equal(["DOCS", "NEW", "IN.TXT"], Changes.Changes.OfType<AddFileChange>().Single().DestinationPath);
        Assert.Empty(context.Messages.InjectQuestions);
    }

    #endregion

    #region New folder

    [Fact]
    public async Task ANewFolderIsCreatedInTheCurrentFolder()
    {
        await context.OpenBrowseImageAsync();
        context.EntryDialogs.NewFolderAnswer = "NEWDIR";

        await Vm.NewFolderCommand.ExecuteAsync(null);

        Assert.Equal(["NEWDIR"], Changes.Changes.OfType<CreateDirectoryChange>().Single().Path);
        Assert.True(Vm.CurrentFolder!.IsRoot);
        Assert.True(Entry("NEWDIR").IsPending);
    }

    [Fact]
    public async Task ANewFolderCanBeCreatedInAPendingFolder()
    {
        await context.OpenBrowseImageAsync();
        Changes.Add(new CreateDirectoryChange(["NEW"]));
        Vm.CurrentFolder = Vm.RootFolder!.Children.Single(folder => folder.Name == "NEW");
        context.EntryDialogs.NewFolderAnswer = "SUB";

        await Vm.NewFolderCommand.ExecuteAsync(null);

        Assert.Equal(["NEW", "SUB"], Changes.Changes.OfType<CreateDirectoryChange>().Last().Path);
    }

    [Fact]
    public async Task CancellingTheNewFolderNameCreatesNothing()
    {
        await context.OpenBrowseImageAsync();

        await Vm.NewFolderCommand.ExecuteAsync(null);

        Assert.Equal(1, context.EntryDialogs.NewFolderRequests);
        Assert.False(Changes.IsDirty);
    }

    #endregion

    #region Delete

    [Fact]
    public async Task DeletingSelectedEntriesQueuesTheirDeletion()
    {
        await context.OpenBrowseImageAsync();
        Vm.SelectEntries([Entry("ALPHA.TXT"), Entry("DOCS")]);

        await Vm.DeleteCommand.ExecuteAsync(null);

        Assert.Equal([["ALPHA.TXT"], ["DOCS"]], Changes.Changes.OfType<DeleteEntryChange>().Select(change => change.Path));
        Assert.Equal(PendingState.Deleted, Entry("ALPHA.TXT").PendingState);
    }

    [Fact]
    public async Task DeletingIsConfirmedWithCountsAndSize()
    {
        await context.OpenBrowseImageAsync();
        Vm.SelectEntries([Entry("ALPHA.TXT"), Entry("MID.BIN"), Entry("DOCS")]);

        await Vm.DeleteCommand.ExecuteAsync(null);

        // DOCS holds 151 bytes.
        var summary = Assert.Single(context.Messages.DeleteQuestions);
        Assert.Equal(new DeletionSummary(2, 1, context.Settings.Current.SizeUnit.FormatSize(10 + 2000 + 151)), summary);
    }

    [Fact]
    public async Task DecliningTheDeletionDeletesNothing()
    {
        await context.OpenBrowseImageAsync();
        Vm.SelectEntries([Entry("ALPHA.TXT")]);
        context.Messages.DeleteAnswer = new(false);

        await Vm.DeleteCommand.ExecuteAsync(null);

        Assert.False(Changes.IsDirty);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task NotAskingAgainTurnsTheDeletionQuestionOff(bool confirmed)
    {
        await context.OpenBrowseImageAsync();
        Vm.SelectEntries([Entry("ALPHA.TXT")]);
        context.Messages.DeleteAnswer = new(confirmed, DontAskAgain: true);

        await Vm.DeleteCommand.ExecuteAsync(null);
        Vm.SelectEntries([Entry("MID.BIN")]);
        await Vm.DeleteCommand.ExecuteAsync(null);

        Assert.False(context.Settings.Current.ConfirmDeletion);
        Assert.Single(context.Messages.DeleteQuestions);
        Assert.Equal(confirmed ? 2 : 1, Changes.Changes.Count);
    }

    [Fact]
    public async Task PendingEntriesAmongTheSelectionAreLeftAlone()
    {
        await context.OpenBrowseImageAsync();
        context.Settings.Current.ConfirmDeletion = false;
        Changes.Add(new CreateDirectoryChange(["NEW"]));
        Vm.SelectEntries([Entry("NEW"), Entry("ALPHA.TXT")]);

        await Vm.DeleteCommand.ExecuteAsync(null);

        Assert.Equal(["ALPHA.TXT"], Changes.Changes.OfType<DeleteEntryChange>().Single().Path);
        Assert.Empty(context.Messages.DeleteQuestions);
    }

    [Fact]
    public async Task DeletingFromTheTreeDeletesTheCurrentFolder()
    {
        await context.OpenBrowseImageAsync();
        Vm.CurrentFolder = Docs;
        Vm.ActivePane = BrowsePane.Folders;
        Vm.SelectEntries([Entry("A.TXT")]);

        await Vm.DeleteCommand.ExecuteAsync(null);

        Assert.Equal(1, context.Messages.FolderDeleteQuestions);
        Assert.Empty(context.Messages.DeleteQuestions);
        Assert.Equal(["DOCS"], Changes.Changes.OfType<DeleteEntryChange>().Single().Path);
    }

    [Fact]
    public async Task DecliningTheFolderDeletionDeletesNothing()
    {
        await context.OpenBrowseImageAsync();
        Vm.CurrentFolder = Docs;
        Vm.ActivePane = BrowsePane.Folders;
        context.Messages.DeleteAnswer = new(false, DontAskAgain: true);

        await Vm.DeleteCommand.ExecuteAsync(null);

        Assert.False(Changes.IsDirty);
        Assert.False(context.Settings.Current.ConfirmDeletion);
    }

    #endregion

    [Fact]
    public async Task EditsAreSavedIntoTheImage()
    {
        var path = context.PathOf("browse.img");
        await context.OpenBrowseImageAsync();
        context.Settings.Current.ConfirmInjection = false;
        context.Settings.Current.ConfirmDeletion = false;
        context.FilePicker.FilesToAdd = [HostFile("ADDED.TXT", 5)];
        await Vm.InjectFilesCommand.ExecuteAsync(null);
        context.EntryDialogs.NewFolderAnswer = "NEWDIR";
        await Vm.NewFolderCommand.ExecuteAsync(null);
        Vm.SelectEntries([Entry("MID.BIN")]);
        await Vm.DeleteCommand.ExecuteAsync(null);

        await Vm.SaveCommand.ExecuteAsync(null);

        Assert.False(Vm.HasUnsavedChanges);
        var names = Vm.Entries.Select(entry => entry.Name).ToList();
        Assert.Contains("ADDED.TXT", names);
        Assert.Contains("NEWDIR", names);
        Assert.DoesNotContain("MID.BIN", names);
        Assert.All(Vm.Entries, entry => Assert.False(entry.IsPending));
        Assert.Equal(path, Vm.FilePath);
    }
}
