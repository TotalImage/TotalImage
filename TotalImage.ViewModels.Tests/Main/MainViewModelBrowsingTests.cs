using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TotalImage.Changes;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Main;
using TotalImage.ViewModels.Settings;
using TotalImage.ViewModels.Sorting;
using Xunit;
using Directory = TotalImage.FileSystems.Directory;

namespace TotalImage.ViewModels.Tests.Main;

/// <summary>
/// The folder tree, the current folder and its entries, the selection, navigation and sorting.
/// </summary>
public class MainViewModelBrowsingTests : IDisposable
{
    private readonly MainTestContext context = new();

    private MainViewModel Vm => context.ViewModel;

    public void Dispose() => context.Dispose();

    private FolderViewModel Root => Vm.RootFolder!;

    private FolderViewModel Docs => Root.Children.Single(folder => folder.Name == "DOCS");

    private EntryViewModel Entry(string name) => Vm.Entries.Single(entry => entry.Name == name);

    private IEnumerable<string> EntryNames => Vm.Entries.Select(entry => entry.Name);

    private static string PathOf(params string[] names) => Path.DirectorySeparatorChar + string.Join(Path.DirectorySeparatorChar, names);

    #region Folder tree

    [Fact]
    public async Task OpeningAnImageShowsItsRootFolder()
    {
        await context.OpenBrowseImageAsync();

        Assert.True(Root.IsRoot);
        Assert.Null(Root.Parent);
        Assert.Equal(Path.DirectorySeparatorChar.ToString(), Root.Name);
        Assert.Same(Vm.CurrentFileSystem!.RootDirectory, Root.Directory);
        Assert.Same(Root, Vm.CurrentFolder);
        Assert.False(Root.IsExpanded);
        Assert.Equal(IconKey.Folder, Root.IconKey);
    }

    [Fact]
    public async Task SubfoldersAreReadWhenFirstAskedFor()
    {
        await context.OpenBrowseImageAsync();
        Assert.False(Root.AreChildrenLoaded);

        var docs = Docs;

        Assert.True(Root.AreChildrenLoaded);
        Assert.False(docs.AreChildrenLoaded);
        Assert.False(Root.Children.Single(folder => folder.Name == "EMPTY").AreChildrenLoaded);

        Assert.Equal(["DEEP"], docs.Children.Select(folder => folder.Name));
        Assert.True(docs.AreChildrenLoaded);
        Assert.Same(docs.Children, docs.Children);
    }

    [Fact]
    public async Task SubfoldersAreSortedByNameAndKnowTheirParent()
    {
        await context.OpenBrowseImageAsync();

        Assert.Equal(BrowseTestImage.RootFolders, Root.Children.Select(folder => folder.Name));
        Assert.All(Root.Children, folder => Assert.Same(Root, folder.Parent));
        Assert.Equal(PathOf("DOCS", "DEEP"), Docs.Children[0].FullName);
        Assert.True(Docs.HasChildren);
        Assert.False(Docs.Children[0].HasChildren);
    }

    [Fact]
    public async Task PendingFoldersAreInTheTree()
    {
        await context.OpenBrowseImageAsync();
        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["BETA"]));
        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["BETA", "INNER"]));
        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["DOCS"]));

        Assert.Equal(["BETA", "DOCS", "EMPTY"], Root.Children.Select(folder => folder.Name));
        var beta = Root.Children[0];
        Assert.True(beta.IsPending);
        Assert.IsType<PendingDirectory>(beta.Directory);
        Assert.Equal(PendingState.Added, beta.PendingState);
        Assert.Equal(["INNER"], beta.Children.Select(folder => folder.Name));

        // A folder that exists already is shown once, and marked as added again.
        Assert.False(Docs.IsPending);
        Assert.Equal(PendingState.Added, Docs.PendingState);
    }

    [Fact]
    public async Task PendingDeletesAndRenamesMarkTheirFolders()
    {
        await context.OpenBrowseImageAsync();

        Vm.Image!.PendingChanges.Add(new DeleteEntryChange(["DOCS"]));
        Vm.Image!.PendingChanges.Add(new RenameChange(["EMPTY"], "FULL"));

        Assert.Equal(PendingState.Deleted, Docs.PendingState);
        Assert.Equal(PendingState.Renamed, Root.Children.Single(folder => folder.Name == "EMPTY").PendingState);
    }

    [Fact]
    public async Task AFolderIsFoundByItsFullName()
    {
        await context.OpenBrowseImageAsync();

        Assert.Same(Docs.Children[0], Root.Find(PathOf("DOCS", "DEEP")));
        Assert.Same(Root, Root.Find(Path.DirectorySeparatorChar.ToString()));
        Assert.Null(Root.Find(PathOf("DOCS", "MISSING")));
    }

    [Fact]
    public async Task ExpandingTheTreeExpandsEveryFolderWithSubfolders()
    {
        await context.OpenBrowseImageAsync();

        Vm.ExpandFolderTreeCommand.Execute(null);

        Assert.True(Root.IsExpanded);
        Assert.True(Docs.IsExpanded);
        Assert.False(Docs.Children[0].IsExpanded);
        Assert.False(Root.Children.Single(folder => folder.Name == "EMPTY").IsExpanded);
    }

    [Fact]
    public async Task CollapsingTheTreeCollapsesEverythingAndGoesToTheRoot()
    {
        await context.OpenBrowseImageAsync();
        Vm.ExpandFolderTreeCommand.Execute(null);
        Vm.CurrentFolder = Docs.Children[0];

        Vm.CollapseFolderTreeCommand.Execute(null);

        Assert.False(Root.IsExpanded);
        Assert.False(Docs.IsExpanded);
        Assert.Same(Root, Vm.CurrentFolder);
    }

    [Fact]
    public async Task HiddenFoldersAreMarked()
    {
        await context.OpenFloppyAsync();
        var root = new FakeDirectory("\\");
        root.AddDirectory("SHOWN");
        root.AddDirectory("SECRET").Attributes = FileAttributes.Directory | FileAttributes.Hidden;

        var tree = FakeFolders.Root(root);

        Assert.Equal(["SECRET", "SHOWN"], tree.Children.Select(folder => folder.Name));
        Assert.Equal([true, false], tree.Children.Select(folder => folder.IsHidden));
    }

    #endregion

    #region Current folder and navigation

    [Fact]
    public async Task TheRootFolderListsItsEntriesWithoutTheParentEntry()
    {
        await context.OpenBrowseImageAsync();

        Assert.Equal(BrowseTestImage.RootFolders.Concat(BrowseTestImage.RootFiles).Order(), EntryNames.Order());
        Assert.DoesNotContain(Vm.Entries, entry => entry.IsParentEntry);
    }

    [Fact]
    public async Task ASubfolderListsTheParentEntryFirst()
    {
        await context.OpenBrowseImageAsync();

        Vm.CurrentFolder = Docs;

        Assert.Equal(["..", "DEEP", "A.TXT", "B.TXT"], EntryNames);
        var parent = Vm.Entries[0];
        Assert.True(parent.IsParentEntry);
        Assert.False(parent.IsDirectory);
        Assert.Same(Root.Directory, parent.Entry);
        Assert.Equal(IconKey.ParentFolder, parent.IconKey);
        Assert.Equal("Parent directory", parent.ToolTip);
        Assert.Equal(string.Empty, parent.TypeName);
        Assert.Equal(string.Empty, parent.SizeText);
    }

    [Fact]
    public async Task EntryViewModelsAreCreatedOncePerFolderVisit()
    {
        await context.OpenBrowseImageAsync();
        var first = Vm.Entries;

        Vm.SortByCommand.Execute((int)EntryColumn.Size);
        Assert.NotSame(first, Vm.Entries);
        Assert.Equal(first.ToHashSet(), Vm.Entries.ToHashSet());

        Vm.CurrentFolder = Docs;
        Vm.CurrentFolder = Root;
        Assert.Empty(first.Intersect(Vm.Entries));
    }

    [Fact]
    public async Task GoingToTheParentFolder()
    {
        await context.OpenBrowseImageAsync();
        Vm.CurrentFolder = Docs.Children[0];

        Vm.GoToParentFolderCommand.Execute(null);
        Assert.Same(Docs, Vm.CurrentFolder);

        Vm.GoToParentFolderCommand.Execute(null);
        Assert.Same(Root, Vm.CurrentFolder);
    }

    [Fact]
    public async Task OpeningAFolderEntryGoesIntoIt()
    {
        await context.OpenBrowseImageAsync();

        Assert.True(Vm.TryOpenFolder(Entry("DOCS")));
        Assert.Same(Docs, Vm.CurrentFolder);

        Assert.True(Vm.TryOpenFolder(Entry("DEEP")));
        Assert.Same(Docs.Children[0], Vm.CurrentFolder);
    }

    [Fact]
    public async Task OpeningTheParentEntryGoesUp()
    {
        await context.OpenBrowseImageAsync();
        Vm.CurrentFolder = Docs;

        Assert.True(Vm.TryOpenFolder(Entry("..")));

        Assert.Same(Root, Vm.CurrentFolder);
    }

    [Fact]
    public async Task OpeningAFileIsLeftToTheView()
    {
        await context.OpenBrowseImageAsync();

        Assert.False(Vm.TryOpenFolder(Entry("ALPHA.TXT")));

        Assert.Same(Root, Vm.CurrentFolder);
    }

    [Fact]
    public async Task OpeningAPendingFolderEntryGoesIntoIt()
    {
        await context.OpenBrowseImageAsync();
        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["NEWDIR"]));

        Assert.True(Vm.TryOpenFolder(Entry("NEWDIR")));

        Assert.Equal("NEWDIR", Vm.CurrentFolder!.Name);
        Assert.True(Vm.CurrentFolder.IsPending);
        Assert.Equal([".."], EntryNames);
    }

    [Fact]
    public async Task AnEditKeepsTheCurrentFolderAndReplacesItsEntries()
    {
        await context.OpenBrowseImageAsync();
        Vm.CurrentFolder = Docs;
        var root = Root;
        var entries = Vm.Entries;

        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["DOCS", "NEWDIR"]));

        Assert.NotSame(root, Vm.RootFolder);
        Assert.Equal(PathOf("DOCS"), Vm.CurrentFolder!.FullName);
        Assert.Same(Vm.RootFolder!.Find(PathOf("DOCS")), Vm.CurrentFolder);
        Assert.NotSame(entries, Vm.Entries);
        Assert.Contains("NEWDIR", EntryNames);
    }

    [Fact]
    public async Task AFolderThatIsGoneAfterAReloadGoesToTheRoot()
    {
        await context.OpenBrowseImageAsync();
        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["OTHER"]));
        Vm.CurrentFolder = Root.Children.Single(folder => folder.Name == "OTHER");

        Vm.Image!.PendingChanges.Clear();

        Assert.Same(Vm.RootFolder, Vm.CurrentFolder);
    }

    [Fact]
    public async Task ShowingHiddenItemsReloadsTheTreeAndTheEntries()
    {
        await context.OpenBrowseImageAsync();
        Assert.Contains("HIDDEN.SYS", EntryNames);
        var root = Root;

        Vm.ToggleShowHiddenItemsCommand.Execute(null);

        Assert.DoesNotContain("HIDDEN.SYS", EntryNames);
        Assert.NotSame(root, Vm.RootFolder);

        Vm.ToggleShowHiddenItemsCommand.Execute(null);
        Assert.Contains("HIDDEN.SYS", EntryNames);
    }

    [Fact]
    public async Task ChangingThePartitionShowsItsRootFolder()
    {
        context.ImageDialogs.SelectPartition = _ => new ViewModels.Dialogs.PartitionSelection(0, false);
        await context.OpenAsync(context.WriteMbrDisk());
        var root = Root;

        Vm.SelectPartition(1);

        Assert.NotSame(root, Vm.RootFolder);
        Assert.Same(Vm.RootFolder, Vm.CurrentFolder);
    }

    [Fact]
    public async Task ClosingTheImageClearsTheTreeAndTheEntries()
    {
        await context.OpenBrowseImageAsync();
        Vm.SelectEntries([Entry("ALPHA.TXT")]);

        await Vm.CloseImageCommand.ExecuteAsync(null);

        Assert.Null(Vm.RootFolder);
        Assert.Null(Vm.CurrentFolder);
        Assert.Empty(Vm.Entries);
        Assert.Empty(Vm.SelectedEntries);
        Assert.Empty(Vm.SelectedObjects);
    }

    #endregion

    #region Selection

    [Fact]
    public async Task SelectingEntriesSelectsTheirObjects()
    {
        await context.OpenBrowseImageAsync();

        Vm.SelectEntries([Entry("ALPHA.TXT"), Entry("DOCS")]);

        Assert.Equal([Entry("ALPHA.TXT"), Entry("DOCS")], Vm.SelectedEntries);
        Assert.Equal([Entry("ALPHA.TXT").Entry, Entry("DOCS").Entry], Vm.SelectedObjects);
        Assert.False(Vm.IsParentEntrySelected);
    }

    [Fact]
    public async Task TheParentEntryIsLeftOutWhenOtherEntriesAreSelected()
    {
        await context.OpenBrowseImageAsync();
        Vm.CurrentFolder = Docs;

        Vm.SelectEntries([Entry(".."), Entry("A.TXT")]);

        Assert.Equal([Entry("A.TXT")], Vm.SelectedEntries);
    }

    [Fact]
    public async Task TheParentEntryCanBeSelectedOnItsOwn()
    {
        await context.OpenBrowseImageAsync();
        Vm.CurrentFolder = Docs;

        Vm.SelectEntries([Entry("..")]);

        Assert.Equal([Entry("..")], Vm.SelectedEntries);
        Assert.True(Vm.IsParentEntrySelected);
        Assert.Empty(Vm.SelectedObjects);
    }

    [Fact]
    public async Task SelectAllSelectsEverythingButTheParentEntry()
    {
        await context.OpenBrowseImageAsync();
        Vm.CurrentFolder = Docs;
        Vm.ActivePane = BrowsePane.Folders;

        Vm.SelectAllCommand.Execute(null);

        Assert.Equal(Vm.Entries.Skip(1), Vm.SelectedEntries);
        Assert.Equal(BrowsePane.Files, Vm.ActivePane);
    }

    [Fact]
    public async Task ChangingTheFolderClearsTheSelection()
    {
        await context.OpenBrowseImageAsync();
        Vm.SelectEntries([Entry("ALPHA.TXT")]);

        Vm.CurrentFolder = Docs;

        Assert.Empty(Vm.SelectedEntries);
        Assert.Empty(Vm.SelectedObjects);
    }

    [Fact]
    public async Task SortingKeepsTheSelection()
    {
        await context.OpenBrowseImageAsync();
        var alpha = Entry("ALPHA.TXT");
        Vm.SelectEntries([alpha]);

        Vm.SortByCommand.Execute((int)EntryColumn.Size);

        Assert.Equal([alpha], Vm.SelectedEntries);
    }

    [Fact]
    public async Task SelectingTheSameEntriesAgainChangesNothing()
    {
        await context.OpenBrowseImageAsync();
        Vm.SelectEntries([Entry("ALPHA.TXT")]);
        var changed = new List<string?>();
        Vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        Vm.SelectEntries([Entry("ALPHA.TXT")]);

        Assert.DoesNotContain(nameof(MainViewModel.SelectedEntries), changed);
    }

    #endregion

    #region Sorting

    // The root folder's files in ascending order of each column (see BrowseTestImage).
    public static TheoryData<EntryColumn, string[]> AscendingFiles => new()
    {
        { EntryColumn.Name, ["ALPHA.TXT", "HIDDEN.SYS", "MID.BIN", "ZETA.ARC"] },
        { EntryColumn.Size, ["HIDDEN.SYS", "ALPHA.TXT", "ZETA.ARC", "MID.BIN"] },
        { EntryColumn.Modified, ["HIDDEN.SYS", "ALPHA.TXT", "MID.BIN", "ZETA.ARC"] },
        // "Fake ARC File", "Fake BIN File", "Fake SYS File", "Fake TXT File"
        { EntryColumn.Type, ["ZETA.ARC", "MID.BIN", "HIDDEN.SYS", "ALPHA.TXT"] },
    };

    [Theory]
    [MemberData(nameof(AscendingFiles))]
    public async Task SortingAscendingPutsFoldersFirst(EntryColumn column, string[] files)
    {
        await context.OpenBrowseImageAsync();

        SortBy(column, SortDirection.Ascending);

        Assert.Equal(BrowseTestImage.RootFolders.ToHashSet(), EntryNames.Take(2).ToHashSet());
        Assert.Equal(files, EntryNames.Skip(2));
    }

    [Theory]
    [MemberData(nameof(AscendingFiles))]
    public async Task SortingDescendingReversesEverything(EntryColumn column, string[] files)
    {
        await context.OpenBrowseImageAsync();

        SortBy(column, SortDirection.Descending);

        Assert.Equal(files.Reverse(), EntryNames.Take(4));
        Assert.Equal(BrowseTestImage.RootFolders.ToHashSet(), EntryNames.Skip(4).ToHashSet());
    }

    [Theory]
    [InlineData(SortDirection.Ascending)]
    [InlineData(SortDirection.Descending)]
    public async Task SortingByAttributesComparesTheirText(SortDirection direction)
    {
        await context.OpenBrowseImageAsync();

        SortBy(EntryColumn.Attributes, direction);

        var files = Vm.Entries.Where(entry => !entry.IsDirectory).Select(entry => entry.AttributesText).ToList();
        var expected = files.Order(StringComparer.CurrentCulture).ToList();
        if (direction == SortDirection.Descending)
            expected.Reverse();
        Assert.Equal(expected, files);
        Assert.Equal(direction == SortDirection.Ascending, Vm.Entries[0].IsDirectory);
    }

    [Theory]
    [InlineData(EntryColumn.Name, SortDirection.Ascending)]
    [InlineData(EntryColumn.Name, SortDirection.Descending)]
    [InlineData(EntryColumn.Type, SortDirection.Ascending)]
    [InlineData(EntryColumn.Type, SortDirection.Descending)]
    [InlineData(EntryColumn.Size, SortDirection.Ascending)]
    [InlineData(EntryColumn.Size, SortDirection.Descending)]
    [InlineData(EntryColumn.Modified, SortDirection.Ascending)]
    [InlineData(EntryColumn.Modified, SortDirection.Descending)]
    [InlineData(EntryColumn.Attributes, SortDirection.Ascending)]
    [InlineData(EntryColumn.Attributes, SortDirection.Descending)]
    public async Task TheParentEntryAlwaysComesFirst(EntryColumn column, SortDirection direction)
    {
        await context.OpenBrowseImageAsync();
        Vm.CurrentFolder = Docs;

        SortBy(column, direction);

        Assert.True(Vm.Entries[0].IsParentEntry);
        Assert.Equal(direction == SortDirection.Ascending ? 1 : 3, EntryNames.ToList().IndexOf("DEEP"));
    }

    [Fact]
    public async Task NoSortDirectionSortsAscending()
    {
        context.Settings.Current.FilesSortOrder = SortDirection.None;

        await context.OpenBrowseImageAsync();

        Assert.Equal(["ALPHA.TXT", "HIDDEN.SYS", "MID.BIN", "ZETA.ARC"], EntryNames.Skip(2));
    }

    [Fact]
    public async Task AnUnknownSortColumnSortsByName()
    {
        context.Settings.Current.FilesSortingColumn = 42;

        await context.OpenBrowseImageAsync();

        Assert.Equal(["DOCS", "EMPTY", "ALPHA.TXT", "HIDDEN.SYS", "MID.BIN", "ZETA.ARC"], EntryNames);
    }

    [Fact]
    public async Task TheSortSettingsAreUsedWhenAFolderIsShown()
    {
        context.Settings.Current.FilesSortingColumn = (int)EntryColumn.Size;
        context.Settings.Current.FilesSortOrder = SortDirection.Descending;

        await context.OpenBrowseImageAsync();

        Assert.Equal(["MID.BIN", "ZETA.ARC", "ALPHA.TXT", "HIDDEN.SYS"], EntryNames.Take(4));
    }

    private void SortBy(EntryColumn column, SortDirection direction)
    {
        Vm.SortByCommand.Execute((int)column);
        if (Vm.SortDirection != direction)
            Vm.SortByCommand.Execute((int)column);
        Assert.Equal(direction, Vm.SortDirection);
    }

    #endregion

    #region Entries

    [Fact]
    public async Task FileEntriesShowTheirColumns()
    {
        await context.OpenBrowseImageAsync();

        var alpha = Entry("ALPHA.TXT");

        Assert.Equal("Fake TXT File", alpha.TypeName);
        Assert.Equal(SizeUnit.Bytes.FormatSize(10), alpha.SizeText);
        Assert.Equal(alpha.Entry.LastWriteTime.ToString(), alpha.ModifiedText);
        Assert.Equal("-A--R-", alpha.AttributesText);
        Assert.Equal(new IconKey(IconKind.File, ".txt"), alpha.IconKey);
        Assert.False(alpha.IsDirectory);
        Assert.False(alpha.IsHidden);
        Assert.False(alpha.IsPending);
        Assert.Equal(PendingState.None, alpha.PendingState);
        Assert.Equal($"Type: Fake TXT File{Environment.NewLine}Size: {alpha.SizeText}{Environment.NewLine}Modified: {alpha.ModifiedText}", alpha.ToolTip);
    }

    [Fact]
    public async Task FolderEntriesHaveNoSizeByDefault()
    {
        await context.OpenBrowseImageAsync();

        var docs = Entry("DOCS");

        Assert.Equal("Fake File folder", docs.TypeName);
        Assert.Equal(string.Empty, docs.SizeText);
        Assert.Equal(IconKey.Folder, docs.IconKey);
        Assert.True(docs.IsDirectory);
        Assert.StartsWith("D", docs.AttributesText);
    }

    [Fact]
    public async Task HiddenEntriesAreMarked()
    {
        await context.OpenBrowseImageAsync();

        Assert.True(Entry("HIDDEN.SYS").IsHidden);
        Assert.Equal("-A-H--", Entry("HIDDEN.SYS").AttributesText);
        Assert.Equal("-AS---", Entry("ZETA.ARC").AttributesText);
    }

    [Fact]
    public async Task TypeNamesComeFromTheFileTypeService()
    {
        await context.OpenBrowseImageAsync();

        Assert.Contains(("ZETA.ARC", Entry("ZETA.ARC").Entry.Attributes), context.FileTypes.Requests);
        Assert.Contains(context.FileTypes.Requests, request => request.Name == "DOCS" && request.Attributes.HasFlag(FileAttributes.Directory));
    }

    [Fact]
    public void IconKeysOfFilesUseTheExtensionInLowerCase()
    {
        Assert.Equal(new IconKey(IconKind.File, ".txt"), IconKey.ForFile("README.TXT"));
        Assert.Equal(new IconKey(IconKind.File, ""), IconKey.ForFile("README"));
        Assert.Equal(IconKey.Folder, IconKey.ForEntry("DOCS", FileAttributes.Directory | FileAttributes.Hidden));
        Assert.Equal(new IconKey(IconKind.File, ".bin"), IconKey.ForEntry("A.BIN", FileAttributes.Archive));
    }

    [Fact]
    public async Task FolderSizesAreMeasuredWhenTheSettingsAskForThem()
    {
        context.Settings.Current.FileListShowDirSize = true;
        await context.OpenBrowseImageAsync();
        var docs = Entry("DOCS");

        Assert.Equal(SizeUnit.Bytes.FormatSize(151), docs.SizeText);
        Assert.Equal(SizeUnit.Bytes.FormatSize(0), Entry("EMPTY").SizeText);
        Assert.StartsWith($"Type: Fake File folder{Environment.NewLine}Size: {docs.SizeText}", docs.ToolTip);
    }

    [Fact]
    public async Task FolderSizesUseTheSizeUnitFromTheSettings()
    {
        context.Settings.Current.FileListShowDirSize = true;
        context.Settings.Current.SizeUnit = SizeUnit.Binary;

        await context.OpenBrowseImageAsync();

        Assert.Equal(SizeUnit.Binary.FormatSize(151), Entry("DOCS").SizeText);
        Assert.Equal(SizeUnit.Binary.FormatSize(2000), Entry("MID.BIN").SizeText);
    }

    #endregion

    #region Pending entries

    [Fact]
    public async Task PendingAdditionsAreListedAsPendingEntries()
    {
        await context.OpenBrowseImageAsync();
        var written = new DateTime(2010, 5, 6, 7, 8, 10);
        using var source = FileDataSource.Create(new MemoryStream(new byte[42]), context.Folder, long.MaxValue);
        Vm.Image!.PendingChanges.Add(new AddFileChange(["NEW.TXT"], source, FileSystems.FAT.FatAttributes.Archive, written, written, written));
        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["NEWDIR"]));
        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["DOCS", "ELSEWHERE"]));

        var file = Entry("NEW.TXT");
        Assert.True(file.IsPending);
        Assert.IsType<PendingFile>(file.Entry);
        Assert.Equal(PendingState.Added, file.PendingState);
        Assert.Equal(SizeUnit.Bytes.FormatSize(42), file.SizeText);
        Assert.Equal(written.ToString(), file.ModifiedText);
        Assert.Equal("-A---", file.AttributesText);
        Assert.Equal("Fake TXT File", file.TypeName);
        Assert.Equal(new IconKey(IconKind.File, ".txt"), file.IconKey);

        var folder = Entry("NEWDIR");
        Assert.True(folder.IsPending);
        Assert.True(folder.IsDirectory);
        Assert.IsType<PendingDirectory>(folder.Entry);
        Assert.Equal(PathOf("NEWDIR"), folder.Entry.FullName);
        Assert.Equal(string.Empty, folder.SizeText);
        Assert.Equal("D----", folder.AttributesText);
        Assert.Equal(IconKey.Folder, folder.IconKey);

        Assert.DoesNotContain("ELSEWHERE", EntryNames);
    }

    [Fact]
    public async Task AnAdditionOfAnExistingNameMarksTheExistingEntry()
    {
        await context.OpenBrowseImageAsync();

        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["DOCS"]));

        Assert.Single(Vm.Entries, entry => entry.Name == "DOCS");
        Assert.False(Entry("DOCS").IsPending);
        Assert.Equal(PendingState.Added, Entry("DOCS").PendingState);
    }

    [Fact]
    public async Task PendingDeletesAndRenamesMarkTheirEntries()
    {
        await context.OpenBrowseImageAsync();

        Vm.Image!.PendingChanges.Add(new RenameChange(["ALPHA.TXT"], "OMEGA.TXT"));
        Vm.Image!.PendingChanges.Add(new DeleteEntryChange(["MID.BIN"]));
        Vm.Image!.PendingChanges.Add(new RenameChange(["ZETA.ARC"], "OTHER.ARC"));
        Vm.Image!.PendingChanges.Add(new DeleteEntryChange(["ZETA.ARC"]));

        Assert.Equal(PendingState.Renamed, Entry("ALPHA.TXT").PendingState);
        Assert.Equal(PendingState.Deleted, Entry("MID.BIN").PendingState);
        Assert.Equal(PendingState.Deleted, Entry("ZETA.ARC").PendingState);
        Assert.Equal(PendingState.None, Entry("HIDDEN.SYS").PendingState);
    }

    [Fact]
    public async Task APendingFolderListsWhatIsAddedToIt()
    {
        await context.OpenBrowseImageAsync();
        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["NEWDIR"]));
        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["NEWDIR", "INNER"]));

        Vm.CurrentFolder = Root.Children.Single(folder => folder.Name == "NEWDIR");

        Assert.Equal(["..", "INNER"], EntryNames);
        Assert.True(Entry("INNER").IsPending);
    }

    #endregion
}
