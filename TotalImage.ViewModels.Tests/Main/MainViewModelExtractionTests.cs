using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Main;
using Xunit;

namespace TotalImage.ViewModels.Tests.Main;

/// <summary>
/// Extracting entries and opening files.
/// </summary>
public class MainViewModelExtractionTests : IDisposable
{
    private readonly MainTestContext context = new();

    private MainViewModel Vm => context.ViewModel;

    private string Target => context.PathOf("out");

    public void Dispose() => context.Dispose();

    private FolderViewModel Docs => Vm.RootFolder!.Children.Single(folder => folder.Name == "DOCS");

    private EntryViewModel Entry(string name) => Vm.Entries.Single(entry => entry.Name == name);

    private string[] Extracted(string? folder = null)
    {
        folder ??= Target;
        return System.IO.Directory.Exists(folder)
            ? System.IO.Directory.GetFiles(folder, "*", SearchOption.AllDirectories).Select(path => Path.GetRelativePath(folder, path)).Order().ToArray()
            : [];
    }

    private async Task OpenAsync(bool alwaysAsk = true)
    {
        await context.OpenBrowseImageAsync();
        var settings = context.Settings.Current;
        settings.ExtractAlwaysAsk = alwaysAsk;
        settings.DefaultExtractPath = Target;
        settings.DefaultDirectoryExtractionMode = DirectoryExtractionMode.Preserve;
        settings.OpenFolderAfterExtract = false;
        settings.ExtractPreserveDates = false;
        context.EntryDialogs.ExtractAnswer = new ExtractOptions(Target, DirectoryExtractionMode.Preserve, OpenFolder: false);
    }

    [Fact]
    public async Task TheSelectedEntriesAreExtractedWhereTheUserSays()
    {
        await OpenAsync();
        Vm.SelectEntries([Entry("ALPHA.TXT"), Entry("DOCS")]);

        await Vm.ExtractCommand.ExecuteAsync(null);

        Assert.Equal([2], context.EntryDialogs.ExtractRequests);
        Assert.Equal(["ALPHA.TXT", Path.Combine("DOCS", "A.TXT"), Path.Combine("DOCS", "B.TXT"), Path.Combine("DOCS", "DEEP", "X.TXT")], Extracted());
        Assert.Equal(Target, context.Settings.Current.DefaultExtractPath);
    }

    [Fact]
    public async Task TheAnswerBecomesTheDefaultFolderAndMode()
    {
        await OpenAsync();
        var other = context.PathOf("other");
        context.EntryDialogs.ExtractAnswer = new ExtractOptions(other, DirectoryExtractionMode.Merge, OpenFolder: true);
        Vm.SelectEntries([Entry("DOCS")]);

        await Vm.ExtractCommand.ExecuteAsync(null);

        Assert.Equal(other, context.Settings.Current.DefaultExtractPath);
        Assert.Equal(["A.TXT", "B.TXT", "X.TXT"], Extracted(other));
        Assert.Equal([other], context.Launcher.OpenedFolders);
    }

    [Fact]
    public async Task CancellingTheQuestionExtractsNothing()
    {
        await OpenAsync();
        context.EntryDialogs.ExtractAnswer = null;
        Vm.SelectEntries([Entry("ALPHA.TXT")]);

        await Vm.ExtractCommand.ExecuteAsync(null);

        Assert.Empty(context.EntryDialogs.ProgressShown);
        Assert.Empty(Extracted());
    }

    [Fact]
    public async Task WithoutTheQuestionTheDefaultsAreUsed()
    {
        await OpenAsync(alwaysAsk: false);
        context.Settings.Current.DefaultDirectoryExtractionMode = DirectoryExtractionMode.Skip;
        context.Settings.Current.OpenFolderAfterExtract = true;
        Vm.SelectEntries([Entry("ALPHA.TXT"), Entry("DOCS")]);

        await Vm.ExtractCommand.ExecuteAsync(null);

        Assert.Empty(context.EntryDialogs.ExtractRequests);
        Assert.Equal(["ALPHA.TXT"], Extracted());
        Assert.Equal([Target], context.Launcher.OpenedFolders);
    }

    [Fact]
    public async Task WithNothingSelectedEverythingInTheFolderIsSelectedAndExtracted()
    {
        await OpenAsync();
        Vm.CurrentFolder = Docs;

        await Vm.ExtractCommand.ExecuteAsync(null);

        Assert.Equal(3, Vm.SelectedObjects.Count);
        Assert.Equal([3], context.EntryDialogs.ExtractRequests);
        Assert.Equal(["A.TXT", "B.TXT", Path.Combine("DEEP", "X.TXT")], Extracted());
    }

    [Fact]
    public async Task ASubfolderInTheTreeIsExtractedAsOneItem()
    {
        await OpenAsync();
        Vm.CurrentFolder = Docs;
        Vm.ActivePane = BrowsePane.Folders;

        await Vm.ExtractCommand.ExecuteAsync(null);

        Assert.Equal([1], context.EntryDialogs.ExtractRequests);
        Assert.Equal([Path.Combine("DOCS", "A.TXT"), Path.Combine("DOCS", "B.TXT"), Path.Combine("DOCS", "DEEP", "X.TXT")], Extracted());
        Assert.Equal(BrowsePane.Folders, Vm.ActivePane);
    }

    [Fact]
    public async Task TheRootFolderInTheTreeExtractsEverythingInItThroughTheFileList()
    {
        await OpenAsync();
        Vm.ActivePane = BrowsePane.Folders;

        await Vm.ExtractCommand.ExecuteAsync(null);

        Assert.Equal(BrowsePane.Files, Vm.ActivePane);
        Assert.Equal(6, Vm.SelectedObjects.Count);
        Assert.Equal([6], context.EntryDialogs.ExtractRequests);
        Assert.Contains("ALPHA.TXT", Extracted());
        Assert.Contains("HIDDEN.SYS", Extracted());
        Assert.Contains(Path.Combine("DOCS", "DEEP", "X.TXT"), Extracted());
    }

    [Fact]
    public async Task HiddenEntriesInsideFoldersFollowTheSetting()
    {
        await OpenAsync();
        context.EntryDialogs.ExtractAnswer = new ExtractOptions(Target, DirectoryExtractionMode.Merge, OpenFolder: false);
        Vm.ToggleShowHiddenItemsCommand.Execute(null);
        Vm.ActivePane = BrowsePane.Folders;

        await Vm.ExtractCommand.ExecuteAsync(null);

        Assert.DoesNotContain("HIDDEN.SYS", Extracted());
    }

    [Fact]
    public async Task ExtractingShowsItsProgress()
    {
        await OpenAsync();
        Vm.SelectEntries([Entry("ALPHA.TXT"), Entry("MID.BIN")]);

        await Vm.ExtractCommand.ExecuteAsync(null);

        var progress = Assert.Single(context.EntryDialogs.ProgressShown);
        Assert.Equal([new(0, 2, null), new(0, 2, "ALPHA.TXT"), new(1, 2, "MID.BIN")], progress);
    }

    [Fact]
    public async Task CancellingTheProgressStopsTheExtractionAndKeepsTheFolderClosed()
    {
        await OpenAsync();
        context.EntryDialogs.ExtractAnswer = new ExtractOptions(Target, DirectoryExtractionMode.Preserve, OpenFolder: true);
        context.EntryDialogs.CancelWhen = report => report.FileName == "MID.BIN";
        Vm.SelectEntries([Entry("ALPHA.TXT"), Entry("MID.BIN"), Entry("ZETA.ARC")]);

        await Vm.ExtractCommand.ExecuteAsync(null);

        Assert.Equal(["ALPHA.TXT"], Extracted());
        Assert.Empty(context.Launcher.OpenedFolders);
    }

    [Fact]
    public async Task ExistingFilesAreAskedAbout()
    {
        await OpenAsync();
        System.IO.Directory.CreateDirectory(Target);
        File.WriteAllText(Path.Combine(Target, "ALPHA.TXT"), "old");
        File.WriteAllText(Path.Combine(Target, "MID.BIN"), "old");
        context.Messages.OverwriteAnswers[0] = new(OverwriteChoice.Skip, ApplyToAll: true);
        Vm.SelectEntries([Entry("ALPHA.TXT"), Entry("MID.BIN"), Entry("ZETA.ARC")]);

        await Vm.ExtractCommand.ExecuteAsync(null);

        Assert.Equal(["ALPHA.TXT"], context.Messages.OverwriteQuestions);
        Assert.Equal("old", File.ReadAllText(Path.Combine(Target, "MID.BIN")));
        Assert.True(File.Exists(Path.Combine(Target, "ZETA.ARC")));
    }

    [Fact]
    public async Task CancellingTheOverwriteQuestionStopsAndKeepsTheFolderClosed()
    {
        await OpenAsync();
        context.EntryDialogs.ExtractAnswer = new ExtractOptions(Target, DirectoryExtractionMode.Preserve, OpenFolder: true);
        System.IO.Directory.CreateDirectory(Target);
        File.WriteAllText(Path.Combine(Target, "ALPHA.TXT"), "old");
        context.Messages.OverwriteAnswers[0] = new(OverwriteChoice.Cancel);
        Vm.SelectEntries([Entry("ALPHA.TXT"), Entry("MID.BIN")]);

        await Vm.ExtractCommand.ExecuteAsync(null);

        Assert.Equal(["ALPHA.TXT"], Extracted());
        Assert.Empty(context.Launcher.OpenedFolders);
    }

    [Fact]
    public async Task WithoutConfirmationExistingFilesAreOverwritten()
    {
        await OpenAsync();
        context.Settings.Current.ConfirmOverwriteExtraction = false;
        System.IO.Directory.CreateDirectory(Target);
        File.WriteAllText(Path.Combine(Target, "ALPHA.TXT"), "old");
        Vm.SelectEntries([Entry("ALPHA.TXT")]);

        await Vm.ExtractCommand.ExecuteAsync(null);

        Assert.Empty(context.Messages.OverwriteQuestions);
        Assert.Equal(10, new FileInfo(Path.Combine(Target, "ALPHA.TXT")).Length);
    }

    [Fact]
    public async Task AFailureIsExplained()
    {
        Assert.SkipUnless(OperatingSystem.IsWindows(), "Read-only files can be replaced elsewhere.");
        await OpenAsync();
        context.Settings.Current.ConfirmOverwriteExtraction = false;
        context.EntryDialogs.ExtractAnswer = new ExtractOptions(Target, DirectoryExtractionMode.Preserve, OpenFolder: true);
        System.IO.Directory.CreateDirectory(Target);
        var existing = Path.Combine(Target, "ALPHA.TXT");
        File.WriteAllText(existing, "old");
        File.SetAttributes(existing, FileAttributes.ReadOnly);
        Vm.SelectEntries([Entry("ALPHA.TXT")]);

        try
        {
            await Vm.ExtractCommand.ExecuteAsync(null);
        }
        finally
        {
            File.SetAttributes(existing, FileAttributes.Normal);
        }

        var (operation, exception) = Assert.Single(context.Messages.Errors);
        Assert.Contains(Target, operation);
        Assert.IsType<UnauthorizedAccessException>(exception);
        Assert.Empty(context.Launcher.OpenedFolders);
    }

    [Fact]
    public async Task OpeningAFileExtractsItToANewTemporaryFolderAndOpensIt()
    {
        await OpenAsync();
        context.Settings.Current.ConfirmOverwriteExtraction = true;

        await Vm.OpenEntryAsync(Entry("ALPHA.TXT"));
        await Vm.OpenEntryAsync(Entry("ALPHA.TXT"));

        Assert.Equal(2, context.Launcher.OpenedFiles.Count);
        Assert.All(context.Launcher.OpenedFiles, opened =>
        {
            Assert.True(opened.Existed);
            Assert.Equal("ALPHA.TXT", Path.GetFileName(opened.Path));
            Assert.Equal(context.TempDirectory.Root, Path.GetDirectoryName(Path.GetDirectoryName(opened.Path)));
        });
        Assert.NotEqual(context.Launcher.OpenedFiles[0].Path, context.Launcher.OpenedFiles[1].Path);
        Assert.Empty(context.Messages.OverwriteQuestions);
        Assert.Empty(context.EntryDialogs.ExtractRequests);
    }

    [Fact]
    public async Task OpeningAFolderGoesIntoIt()
    {
        await OpenAsync();

        await Vm.OpenEntryAsync(Entry("DOCS"));

        Assert.Equal("DOCS", Vm.CurrentFolder!.Name);
        Assert.Empty(context.EntryDialogs.ProgressShown);
    }

    [Fact]
    public async Task OpeningTheParentEntryGoesUp()
    {
        await OpenAsync();
        Vm.CurrentFolder = Docs;

        await Vm.OpenEntryAsync(Entry(EntryViewModel.ParentEntryName));

        Assert.True(Vm.CurrentFolder!.IsRoot);
    }

    [Fact]
    public async Task APendingFileCantBeOpened()
    {
        await OpenAsync();
        var source = Changes.FileDataSource.Create(new MemoryStream(new byte[3]), context.Folder, long.MaxValue);
        var now = DateTime.Now;
        Vm.Image!.PendingChanges.Add(new Changes.AddFileChange(["NEW.TXT"], source, FileSystems.FAT.FatAttributes.Archive, now, now, now));

        await Vm.OpenEntryAsync(Entry("NEW.TXT"));

        Assert.Empty(context.EntryDialogs.ProgressShown);
        Assert.Empty(context.Launcher.OpenedFiles);
    }

    [Fact]
    public async Task ACancelledFileIsNotOpened()
    {
        await OpenAsync();
        context.EntryDialogs.CancelWhen = _ => true;

        await Vm.OpenEntryAsync(Entry("ALPHA.TXT"));

        Assert.Empty(context.Launcher.OpenedFiles);
    }
}
