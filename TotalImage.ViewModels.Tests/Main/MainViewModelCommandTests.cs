using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using TotalImage.Changes;
using TotalImage.FileSystems;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Main;
using TotalImage.ViewModels.Settings;
using TotalImage.ViewModels.Tests.Dialogs;
using Xunit;

namespace TotalImage.ViewModels.Tests.Main;

/// <summary>
/// The enabled rules of every command, and the commands that aren't about opening and saving.
/// </summary>
public class MainViewModelCommandTests : IDisposable
{
    private readonly MainTestContext context = new();

    private MainViewModel Vm => context.ViewModel;

    public void Dispose() => context.Dispose();

    private Dictionary<string, IRelayCommand> ImageCommands => new()
    {
        [nameof(Vm.SaveAsCommand)] = Vm.SaveAsCommand,
        [nameof(Vm.ReloadImageCommand)] = Vm.ReloadImageCommand,
        [nameof(Vm.CloseImageCommand)] = Vm.CloseImageCommand,
        [nameof(Vm.OpenContainingFolderCommand)] = Vm.OpenContainingFolderCommand,
        [nameof(Vm.ChangeVolumeLabelCommand)] = Vm.ChangeVolumeLabelCommand,
        [nameof(Vm.ShowBootSectorCommand)] = Vm.ShowBootSectorCommand,
        [nameof(Vm.FormatCommand)] = Vm.FormatCommand,
        [nameof(Vm.ShowImageInformationCommand)] = Vm.ShowImageInformationCommand,
        [nameof(Vm.SelectAllCommand)] = Vm.SelectAllCommand,
        [nameof(Vm.InjectFilesCommand)] = Vm.InjectFilesCommand,
    };

    private Dictionary<string, IRelayCommand> UnbuiltCommands => new()
    {
        [nameof(Vm.ChangeFormatCommand)] = Vm.ChangeFormatCommand,
        [nameof(Vm.DefragmentCommand)] = Vm.DefragmentCommand,
        [nameof(Vm.ShowDeletedObjectsCommand)] = Vm.ShowDeletedObjectsCommand,
        [nameof(Vm.ShowHexViewCommand)] = Vm.ShowHexViewCommand,
        [nameof(Vm.ShowNotificationsCommand)] = Vm.ShowNotificationsCommand,
    };

    private Dictionary<string, IRelayCommand> AlwaysAvailableCommands => new()
    {
        [nameof(Vm.NewImageCommand)] = Vm.NewImageCommand,
        [nameof(Vm.OpenImageCommand)] = Vm.OpenImageCommand,
        [nameof(Vm.ExitCommand)] = Vm.ExitCommand,
        [nameof(Vm.ShowSettingsCommand)] = Vm.ShowSettingsCommand,
        [nameof(Vm.ShowAboutCommand)] = Vm.ShowAboutCommand,
        [nameof(Vm.ToggleShowHiddenItemsCommand)] = Vm.ToggleShowHiddenItemsCommand,
        [nameof(Vm.ToggleCommandBarCommand)] = Vm.ToggleCommandBarCommand,
        [nameof(Vm.ToggleDirectoryTreeCommand)] = Vm.ToggleDirectoryTreeCommand,
        [nameof(Vm.ToggleStatusBarCommand)] = Vm.ToggleStatusBarCommand,
    };

    private static void AssertEnabled(bool expected, Dictionary<string, IRelayCommand> commands, object? parameter = null)
    {
        foreach (var (name, command) in commands)
            Assert.True(expected == command.CanExecute(parameter), $"{name} should be {(expected ? "enabled" : "disabled")}.");
    }

    #region Enabled rules of the image commands

    [Fact]
    public void WithoutAnImageOnlyTheAppCommandsAreEnabled()
    {
        AssertEnabled(false, ImageCommands);
        AssertEnabled(false, UnbuiltCommands);
        AssertEnabled(true, AlwaysAvailableCommands);
        Assert.False(Vm.SaveCommand.CanExecute(null));
        Assert.False(Vm.ChoosePartitionCommand.CanExecute(null));
        Assert.False(Vm.ManagePartitionsCommand.CanExecute(null));
        Assert.True(Vm.OpenRecentImageCommand.CanExecute("C:\\disk.img"));
        Assert.True(Vm.SetViewModeCommand.CanExecute(ViewMode.List));
        Assert.True(Vm.SortByCommand.CanExecute(1));
    }

    [Fact]
    public async Task AnOpenImageEnablesTheImageCommands()
    {
        await context.OpenFloppyAsync();

        AssertEnabled(true, ImageCommands);
        AssertEnabled(false, UnbuiltCommands);
        AssertEnabled(true, AlwaysAvailableCommands);
    }

    [Fact]
    public async Task OpeningAnImageRefreshesTheCommands()
    {
        var refreshed = new HashSet<string>();
        foreach (var (name, command) in ImageCommands)
            command.CanExecuteChanged += (_, _) => refreshed.Add(name);

        await context.OpenFloppyAsync();

        Assert.Equal(ImageCommands.Keys.ToHashSet(), refreshed);
    }

    [Fact]
    public async Task ClosingTheImageDisablesTheImageCommands()
    {
        await context.OpenFloppyAsync();

        await Vm.CloseImageCommand.ExecuteAsync(null);

        AssertEnabled(false, ImageCommands);
    }

    [Fact]
    public async Task SaveNeedsUnsavedChanges()
    {
        await context.OpenFloppyAsync();
        Assert.False(Vm.SaveCommand.CanExecute(null));
        var refreshed = 0;
        Vm.SaveCommand.CanExecuteChanged += (_, _) => refreshed++;

        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["NEWDIR"]));

        Assert.True(Vm.SaveCommand.CanExecute(null));
        Assert.True(refreshed > 0);
    }

    [Fact]
    public async Task AnUntitledImageCantBeReloadedOrShownInItsFolder()
    {
        context.ImageDialogs.NewImageAnswer = new(DiskGeometries.FloppyGeometry.KnownGeometries[DiskGeometries.FloppyGeometry.FriendlyName.HighDensity1440k],
            true, FileSystems.BPB.BiosParameterBlockVersion.Dos40, "MSDOS5.0", "1234ABCD", "FAT12", "NEWDISK");
        await Vm.NewImageCommand.ExecuteAsync(null);

        Assert.False(Vm.ReloadImageCommand.CanExecute(null));
        Assert.False(Vm.OpenContainingFolderCommand.CanExecute(null));
        Assert.True(Vm.SaveCommand.CanExecute(null));
        Assert.True(Vm.SaveAsCommand.CanExecute(null));
        Assert.True(Vm.CloseImageCommand.CanExecute(null));
    }

    [Fact]
    public async Task APartitionTableEnablesThePartitionCommands()
    {
        await context.OpenFloppyAsync();
        Assert.False(Vm.ChoosePartitionCommand.CanExecute(null));
        Assert.False(Vm.ManagePartitionsCommand.CanExecute(null));

        context.ImageDialogs.SelectPartition = _ => new PartitionSelection(0, false);
        await context.OpenAsync(context.WriteMbrDisk());

        Assert.True(Vm.ChoosePartitionCommand.CanExecute(null));
        Assert.True(Vm.ManagePartitionsCommand.CanExecute(null));
    }

    [Fact]
    public async Task AReadOnlyImageCanOnlyBeBrowsed()
    {
        context.ImageFiles.OpenOverride = path => new ReadOnlyContainer(System.IO.File.ReadAllBytes(path));
        await context.OpenBrowseImageAsync();
        Vm.CurrentFolder = Docs;
        Vm.ActivePane = BrowsePane.Folders;

        Assert.False(Vm.Image!.SupportsWriting);
        Assert.False(Vm.SaveCommand.CanExecute(null));
        Assert.False(Vm.SaveAsCommand.CanExecute(null));
        Assert.False(Vm.InjectFilesCommand.CanExecute(null));
        Assert.False(Vm.NewFolderCommand.CanExecute(null));
        Assert.False(Vm.DeleteCommand.CanExecute(null));
        Assert.False(Vm.RenameCommand.CanExecute(null));
        Assert.False(Vm.ChangeVolumeLabelCommand.CanExecute(null));
        Assert.False(Vm.ShowBootSectorCommand.CanExecute(null));
        Assert.False(Vm.FormatCommand.CanExecute(null));

        Assert.True(Vm.ExtractCommand.CanExecute(null));
        Assert.True(Vm.ShowPropertiesCommand.CanExecute(null));
        Assert.True(Vm.SelectAllCommand.CanExecute(null));
        Assert.True(Vm.ShowImageInformationCommand.CanExecute(null));
        Assert.True(Vm.ReloadImageCommand.CanExecute(null));
        Assert.True(Vm.CloseImageCommand.CanExecute(null));
    }

    #endregion

    #region Enabled rules of the entry commands

    public enum Rule { Extract, Delete, Rename, Properties }

    private bool CanExecute(Rule rule) => rule switch
    {
        Rule.Extract => Vm.ExtractCommand.CanExecute(null),
        Rule.Delete => Vm.DeleteCommand.CanExecute(null),
        Rule.Rename => Vm.RenameCommand.CanExecute(null),
        _ => Vm.ShowPropertiesCommand.CanExecute(null)
    };

    private void AssertRules(bool extract, bool delete, bool rename, bool properties)
    {
        Assert.Equal(extract, CanExecute(Rule.Extract));
        Assert.Equal(delete, CanExecute(Rule.Delete));
        Assert.Equal(rename, CanExecute(Rule.Rename));
        Assert.Equal(properties, CanExecute(Rule.Properties));
    }

    private FolderViewModel Docs => Vm.RootFolder!.Children.Single(folder => folder.Name == "DOCS");

    private EntryViewModel Entry(string name) => Vm.Entries.Single(entry => entry.Name == name);

    [Fact]
    public async Task TheRootFolderInTheTreeCanOnlyBeExtracted()
    {
        await context.OpenBrowseImageAsync();

        Vm.ActivePane = BrowsePane.Folders;

        AssertRules(extract: true, delete: false, rename: false, properties: false);
        Assert.True(Vm.NewFolderCommand.CanExecute(null));
    }

    [Fact]
    public async Task ASubfolderInTheTreeAllowsEverything()
    {
        await context.OpenBrowseImageAsync();

        Vm.CurrentFolder = Docs;
        Vm.ActivePane = BrowsePane.Folders;

        AssertRules(extract: true, delete: true, rename: true, properties: true);
        Assert.True(Vm.NewFolderCommand.CanExecute(null));
    }

    [Fact]
    public async Task NoFolderInTheTreeAllowsNothing()
    {
        await context.OpenBrowseImageAsync();

        Vm.CurrentFolder = null;
        Vm.ActivePane = BrowsePane.Folders;

        AssertRules(extract: false, delete: false, rename: false, properties: false);
        Assert.False(Vm.NewFolderCommand.CanExecute(null));
    }

    [Fact]
    public async Task TheSelectionInTheListDoesntMatterInTheTree()
    {
        await context.OpenBrowseImageAsync();
        Vm.SelectEntries([Entry("ALPHA.TXT")]);

        Vm.ActivePane = BrowsePane.Folders;

        AssertRules(extract: true, delete: false, rename: false, properties: false);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task NothingSelectedInTheListActsOnTheFolder(bool inSubfolder)
    {
        await context.OpenBrowseImageAsync();
        if (inSubfolder)
            Vm.CurrentFolder = Docs;

        Vm.ActivePane = BrowsePane.Files;

        AssertRules(extract: true, delete: inSubfolder, rename: inSubfolder, properties: inSubfolder);
    }

    [Fact]
    public async Task TheParentEntryAllowsNothing()
    {
        await context.OpenBrowseImageAsync();
        Vm.CurrentFolder = Docs;

        Vm.SelectEntries([Entry("..")]);

        Assert.True(Vm.IsParentEntrySelected);
        Assert.Empty(Vm.SelectedObjects);
        AssertRules(extract: false, delete: false, rename: false, properties: false);
    }

    [Fact]
    public async Task OneEntryAllowsEverything()
    {
        await context.OpenBrowseImageAsync();

        Vm.SelectEntries([Entry("ALPHA.TXT")]);

        AssertRules(extract: true, delete: true, rename: true, properties: true);
    }

    [Fact]
    public async Task APendingEntryAllowsNothing()
    {
        await context.OpenBrowseImageAsync();
        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["NEWDIR"]));

        Vm.SelectEntries([Entry("NEWDIR")]);

        AssertRules(extract: false, delete: false, rename: false, properties: false);
    }

    [Fact]
    public async Task SeveralEntriesCantBeRenamed()
    {
        await context.OpenBrowseImageAsync();

        Vm.SelectEntries([Entry("ALPHA.TXT"), Entry("DOCS")]);

        AssertRules(extract: true, delete: true, rename: false, properties: true);
    }

    [Fact]
    public async Task SeveralEntriesWithAPendingOneAllowWhatSeveralEntriesAllow()
    {
        await context.OpenBrowseImageAsync();
        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["NEWDIR"]));

        Vm.SelectEntries([Entry("ALPHA.TXT"), Entry("NEWDIR")]);

        AssertRules(extract: true, delete: true, rename: false, properties: true);
    }

    [Fact]
    public async Task TheParentFolderCommandNeedsAParent()
    {
        await context.OpenBrowseImageAsync();
        Assert.False(Vm.GoToParentFolderCommand.CanExecute(null));

        Vm.CurrentFolder = Docs;

        Assert.True(Vm.GoToParentFolderCommand.CanExecute(null));
    }

    [Fact]
    public async Task ExpandAndCollapseNeedSubfolders()
    {
        await context.OpenFloppyAsync();
        Assert.False(Vm.ExpandFolderTreeCommand.CanExecute(null));
        Assert.False(Vm.CollapseFolderTreeCommand.CanExecute(null));

        // A folder that only exists as a pending change counts.
        Vm.Image!.PendingChanges.Add(new CreateDirectoryChange(["NEWDIR"]));

        Assert.True(Vm.ExpandFolderTreeCommand.CanExecute(null));
        Assert.True(Vm.CollapseFolderTreeCommand.CanExecute(null));
    }

    [Fact]
    public void WithoutAnImageTheEntryCommandsAreDisabled()
    {
        Vm.ActivePane = BrowsePane.Folders;

        AssertRules(extract: false, delete: false, rename: false, properties: false);
        Assert.False(Vm.NewFolderCommand.CanExecute(null));
        Assert.False(Vm.ExpandFolderTreeCommand.CanExecute(null));
        Assert.False(Vm.GoToParentFolderCommand.CanExecute(null));
        Assert.False(Vm.SelectAllCommand.CanExecute(null));
    }

    public static TheoryData<string> BrowseChanges => ["pane", "selection", "folder"];

    [Theory]
    [MemberData(nameof(BrowseChanges))]
    public async Task ChangingWhatIsSelectedRefreshesTheEntryCommands(string change)
    {
        await context.OpenBrowseImageAsync();
        IRelayCommand[] commands = [Vm.SelectAllCommand, Vm.GoToParentFolderCommand, Vm.ExpandFolderTreeCommand, Vm.CollapseFolderTreeCommand,
            Vm.InjectFilesCommand, Vm.ExtractCommand, Vm.NewFolderCommand, Vm.DeleteCommand, Vm.RenameCommand, Vm.ShowPropertiesCommand];
        var refreshed = new HashSet<IRelayCommand>();
        foreach (var command in commands)
            command.CanExecuteChanged += (sender, _) => refreshed.Add((IRelayCommand)sender!);

        switch (change)
        {
            case "pane":
                Vm.ActivePane = BrowsePane.Folders;
                break;
            case "selection":
                Vm.SelectEntries([Entry("ALPHA.TXT")]);
                break;
            default:
                Vm.CurrentFolder = Docs;
                break;
        }

        Assert.Equal(commands.ToHashSet(), refreshed);
    }

    [Fact]
    public async Task TheEditingCommandsAreDoneByTheView()
    {
        await context.OpenBrowseImageAsync();
        Vm.CurrentFolder = Docs;
        Vm.ActivePane = BrowsePane.Folders;

        await Vm.InjectFilesCommand.ExecuteAsync(null);
        await Vm.NewFolderCommand.ExecuteAsync(null);
        await Vm.DeleteCommand.ExecuteAsync(null);
        Vm.RenameCommand.Execute(null);
        await Vm.ShowPropertiesCommand.ExecuteAsync(null);

        Assert.Equal(["InjectFilesAsync", "CreateFolderAsync", "DeleteAsync", "Rename", "ShowPropertiesAsync"], context.Bridge.Calls);
    }

    #endregion

    #region Image and partition tools

    [Fact]
    public async Task ChangeVolumeLabelAsksForTheFatLabelsAndQueuesTheChange()
    {
        await context.OpenFloppyAsync();
        var fs = (FileSystems.FAT.FatFileSystem)Vm.CurrentFileSystem!;
        context.EntryDialogs.VolumeLabelAnswer = new ViewModels.Dialogs.VolumeLabelChange("NEWLABEL", false, string.Empty);

        await Vm.ChangeVolumeLabelCommand.ExecuteAsync(null);

        var (rootLabel, bpbLabel) = Assert.Single(context.EntryDialogs.VolumeLabelRequests);
        Assert.Equal(fs.RootDirectoryVolumeLabel ?? string.Empty, rootLabel);
        Assert.Equal(fs.BpbVolumeLabel ?? string.Empty, bpbLabel);
        Assert.True(Vm.HasUnsavedChanges);
    }

    [Fact]
    public async Task CancellingTheVolumeLabelChangesNothing()
    {
        await context.OpenFloppyAsync();

        await Vm.ChangeVolumeLabelCommand.ExecuteAsync(null);

        Assert.Single(context.EntryDialogs.VolumeLabelRequests);
        Assert.False(Vm.HasUnsavedChanges);
    }

    [Fact]
    public async Task TheBootSectorOfTheLoadedPartitionIsShown()
    {
        await context.OpenFloppyAsync();

        await Vm.ShowBootSectorCommand.ExecuteAsync(null);

        Assert.Equal([(Vm.Image!, 0)], context.ImageDialogs.BootSectorShown);
        Assert.Equal(0, context.Messages.FatOnlyShown);
    }

    [Fact]
    public async Task ImageInformationGetsTheFileAndPartition()
    {
        var path = await context.OpenFloppyAsync();

        await Vm.ShowImageInformationCommand.ExecuteAsync(null);

        Assert.Equal([(Vm.Image!, (string?)path, 0)], context.ImageDialogs.ImageInfoShown);
    }

    [Fact]
    public async Task ManagePartitionsShowsTheDialog()
    {
        context.ImageDialogs.SelectPartition = _ => new PartitionSelection(0, false);
        await context.OpenAsync(context.WriteMbrDisk());

        await Vm.ManagePartitionsCommand.ExecuteAsync(null);

        Assert.Equal(1, context.ImageDialogs.ManagePartitionsShown);
    }

    [Fact]
    public async Task FormatStopsWhenCancelledOrNotConfirmed()
    {
        await context.OpenFloppyAsync();

        await Vm.FormatCommand.ExecuteAsync(null);
        Assert.Equal(0, context.Messages.FormatConfirmations);

        context.ImageDialogs.FormatAnswer = new FormatOptions("FAT12", null, "LABEL", true);
        await Vm.FormatCommand.ExecuteAsync(null);
        Assert.Equal(1, context.Messages.FormatConfirmations);
    }

    [Fact]
    public async Task ConfirmedFormattingIsNotImplemented()
    {
        await context.OpenFloppyAsync();
        context.ImageDialogs.FormatAnswer = new FormatOptions("FAT12", null, "LABEL", true);
        context.Messages.ConfirmFormatAnswer = true;

        await Assert.ThrowsAsync<NotImplementedException>(() => Vm.FormatCommand.ExecuteAsync(null));
    }

    #endregion

    #region View options

    [Fact]
    public void TheViewOptionsComeFromTheSettings()
    {
        var current = context.Settings.Current;
        current.FilesView = ViewMode.List;
        current.ShowHiddenItems = false;
        current.ShowCommandBar = false;
        current.ShowDirectoryTree = false;
        current.ShowStatusBar = false;
        current.FilesSortingColumn = 3;
        current.FilesSortOrder = SortDirection.Descending;

        Assert.Equal(ViewMode.List, Vm.ViewMode);
        Assert.False(Vm.ShowHiddenItems);
        Assert.False(Vm.ShowCommandBar);
        Assert.False(Vm.ShowDirectoryTree);
        Assert.False(Vm.ShowStatusBar);
        Assert.Equal(3, Vm.SortColumn);
        Assert.Equal(SortDirection.Descending, Vm.SortDirection);
    }

    [Theory]
    [InlineData(ViewMode.LargeIcon)]
    [InlineData(ViewMode.SmallIcon)]
    [InlineData(ViewMode.List)]
    [InlineData(ViewMode.Details)]
    public void SetViewModeChangesTheViewAndTheSetting(ViewMode mode)
    {
        context.Settings.Current.FilesView = mode == ViewMode.Details ? ViewMode.List : ViewMode.Details;

        Vm.SetViewModeCommand.Execute(mode);

        Assert.Equal(mode, Vm.ViewMode);
        Assert.Equal(mode, context.Settings.Current.FilesView);
    }

    [Fact]
    public void TheTogglesFlipTheirOptionAndSetting()
    {
        var current = context.Settings.Current;

        Vm.ToggleShowHiddenItemsCommand.Execute(null);
        Vm.ToggleCommandBarCommand.Execute(null);
        Vm.ToggleDirectoryTreeCommand.Execute(null);
        Vm.ToggleStatusBarCommand.Execute(null);

        Assert.False(Vm.ShowHiddenItems);
        Assert.False(Vm.ShowCommandBar);
        Assert.False(Vm.ShowDirectoryTree);
        Assert.False(Vm.ShowStatusBar);
        Assert.False(current.ShowHiddenItems);
        Assert.False(current.ShowCommandBar);
        Assert.False(current.ShowDirectoryTree);
        Assert.False(current.ShowStatusBar);

        Vm.ToggleShowHiddenItemsCommand.Execute(null);
        Assert.True(Vm.ShowHiddenItems);
        Assert.True(current.ShowHiddenItems);
    }

    [Fact]
    public void SortingByAnotherColumnSortsAscending()
    {
        context.Settings.Current.FilesSortOrder = SortDirection.Descending;

        Vm.SortByCommand.Execute(2);

        Assert.Equal(2, Vm.SortColumn);
        Assert.Equal(SortDirection.Ascending, Vm.SortDirection);
        Assert.Equal(2, context.Settings.Current.FilesSortingColumn);
        Assert.Equal(SortDirection.Ascending, context.Settings.Current.FilesSortOrder);
    }

    [Theory]
    [InlineData(SortDirection.Ascending, SortDirection.Descending)]
    [InlineData(SortDirection.Descending, SortDirection.Ascending)]
    [InlineData(SortDirection.None, SortDirection.Ascending)]
    public void SortingByTheSameColumnReversesTheDirection(SortDirection before, SortDirection after)
    {
        context.Settings.Current.FilesSortingColumn = 1;
        context.Settings.Current.FilesSortOrder = before;

        Vm.SortByCommand.Execute(1);

        Assert.Equal(1, Vm.SortColumn);
        Assert.Equal(after, Vm.SortDirection);
        Assert.Equal(after, context.Settings.Current.FilesSortOrder);
    }

    #endregion

    #region Recent images and settings

    [Fact]
    public void RecentImagesAreListedMostRecentFirstWithoutBlankEntries()
    {
        context.Settings.Current.RecentImages.AddRange(["C:\\old.img", " ", "C:\\new.img"]);

        Assert.Equal(["C:\\new.img", "C:\\old.img"], Vm.RecentImages);
        Assert.True(Vm.HasRecentImages);
        Assert.Equal(["C:\\old.img", "C:\\new.img"], context.Settings.Current.RecentImages);
    }

    [Fact]
    public void NoRecentImages()
    {
        Assert.Empty(Vm.RecentImages);
        Assert.False(Vm.HasRecentImages);
    }

    [Fact]
    public void LoadSettingsDropsMissingRecentImages()
    {
        var existing = context.WriteFloppy();
        context.Settings.Current.RecentImages.AddRange([context.PathOf("gone.img"), existing]);
        context.Settings.Current.FilesView = ViewMode.SmallIcon;
        _ = Vm;
        context.Settings.Current.FilesView = ViewMode.List;

        Vm.LoadSettings();

        Assert.Equal([existing], Vm.RecentImages);
        Assert.Equal(ViewMode.List, Vm.ViewMode);
    }

    [Fact]
    public async Task OpeningARecentImageOpensIt()
    {
        var path = context.WriteFloppy();

        await Vm.OpenRecentImageCommand.ExecuteAsync(path);

        Assert.Equal(path, Vm.FilePath);
    }

    [Fact]
    public async Task AMissingRecentImageIsReportedAndRemoved()
    {
        var missing = context.PathOf("gone.img");
        context.Settings.Current.RecentImages.Add(missing);
        Assert.Equal([missing], Vm.RecentImages);

        await Vm.OpenRecentImageCommand.ExecuteAsync(missing);

        Assert.Equal(["gone.img"], context.Messages.MissingRecentImages);
        Assert.Empty(Vm.RecentImages);
        Assert.Empty(context.Settings.Current.RecentImages);
        Assert.False(Vm.IsImageOpen);
    }

    [Fact]
    public void AcceptedSettingsAreApplied()
    {
        var applied = 0;
        Vm.SettingsApplied += (_, _) => applied++;
        context.AppDialogs.SettingsAnswer = new SettingsChangeResult(false);
        context.AppDialogs.WhileSettingsOpen = () =>
        {
            context.Settings.Current.ShowHiddenItems = false;
            context.Settings.Current.FilesView = ViewMode.SmallIcon;
        };

        Vm.ShowSettingsCommand.Execute(null);

        Assert.Equal(1, context.AppDialogs.SettingsShown);
        Assert.Equal(1, applied);
        Assert.False(Vm.ShowHiddenItems);
        Assert.Equal(ViewMode.SmallIcon, Vm.ViewMode);
    }

    [Fact]
    public void CancelledSettingsOnlyRefreshTheRecentImages()
    {
        var applied = 0;
        Vm.SettingsApplied += (_, _) => applied++;
        context.Settings.Current.RecentImages.Add("C:\\disk.img");
        _ = Vm;
        context.AppDialogs.WhileSettingsOpen = () =>
        {
            context.Settings.ClearRecentImages();
            context.Settings.Current.FilesView = ViewMode.SmallIcon;
        };

        Vm.ShowSettingsCommand.Execute(null);

        Assert.Equal(0, applied);
        Assert.Empty(Vm.RecentImages);
        Assert.Equal(ViewMode.Details, Vm.ViewMode);
    }

    [Fact]
    public void AboutShowsTheDialog()
    {
        Vm.ShowAboutCommand.Execute(null);

        Assert.Equal(1, context.AppDialogs.AboutShown);
    }

    #endregion

    #region Status bar

    [Fact]
    public async Task TheStatusBarShowsThePartitionCapacity()
    {
        await context.OpenFloppyAsync();
        var partition = Vm.CurrentPartition!;
        var free = partition.FileSystem.TotalFreeSpace;
        var percentage = (double)free / partition.Length * 100;

        Assert.Equal($"Partition size: {SizeUnit.Bytes.FormatSize((ulong)partition.Length)}", Vm.PartitionSizeText);
        Assert.Equal($"Free space: {SizeUnit.Bytes.FormatSize((ulong)free)} ({percentage / 100:p2})", Vm.FreeSpaceText);
        Assert.Equal(percentage, Vm.FreeSpacePercentage);
    }

    private EntryViewModel EntryOf(FileSystemObject entry) => Vm.Entries.Single(e => e.Entry == entry);

    [Fact]
    public async Task WithNothingSelectedTheStatusBarShowsTheFolder()
    {
        await context.OpenFloppyAsync();
        var root = new FakeDirectory("\\");
        root.AddFile("A.TXT", 100);
        root.AddFile("B.TXT", 23);
        root.AddDirectory("SUB").AddFile("C.TXT", 1000);

        Vm.CurrentFolder = FakeFolders.Root(root);

        Assert.Equal("\\", Vm.LocationText);
        Assert.Equal($"{SizeUnit.Bytes.FormatSize(123)} in 2 item(s)", Vm.SelectionSizeText);
    }

    [Fact]
    public async Task OneSelectedEntryIsShownWithItsSize()
    {
        await context.OpenFloppyAsync();
        var root = new FakeDirectory("\\");
        var file = root.AddFile("A.TXT", 100);
        var sub = root.AddDirectory("SUB");
        sub.AddFile("C.TXT", 1000);
        sub.AddDirectory("DEEP").AddFile("D.TXT", 1);
        Vm.CurrentFolder = FakeFolders.Root(root);

        Vm.SelectEntries([EntryOf(file)]);
        Assert.Equal(file.FullName, Vm.LocationText);
        Assert.Equal($"{SizeUnit.Bytes.FormatSize(100)} in 1 item", Vm.SelectionSizeText);

        Vm.SelectEntries([EntryOf(sub)]);
        Assert.Equal(sub.FullName, Vm.LocationText);
        Assert.Equal($"{SizeUnit.Bytes.FormatSize(1001)} in 1 item", Vm.SelectionSizeText);
    }

    [Fact]
    public async Task SeveralSelectedEntriesAreAddedUp()
    {
        await context.OpenFloppyAsync();
        var root = new FakeDirectory("\\");
        var file = root.AddFile("A.TXT", 100);
        var sub = root.AddDirectory("SUB");
        sub.AddFile("C.TXT", 1000);
        Vm.CurrentFolder = FakeFolders.Root(root);

        Vm.SelectEntries([EntryOf(file), EntryOf(sub)]);

        Assert.Equal("\\", Vm.LocationText);
        Assert.Equal($"{SizeUnit.Bytes.FormatSize(1100)} in 2 item(s)", Vm.SelectionSizeText);
    }

    [Fact]
    public async Task TheParentEntryOnItsOwnShowsTheFolder()
    {
        await context.OpenBrowseImageAsync();
        Vm.CurrentFolder = Vm.RootFolder!.Children.Single(folder => folder.Name == "DOCS");

        Vm.SelectEntries([Vm.Entries[0]]);

        Assert.Equal(Vm.CurrentFolder.FullName, Vm.LocationText);
        Assert.Equal($"{SizeUnit.Bytes.FormatSize(150)} in 2 item(s)", Vm.SelectionSizeText);
    }

    [Fact]
    public async Task TheFolderSizeIsKeptUntilTheCapacityIsRefreshed()
    {
        await context.OpenFloppyAsync();
        var root = new FakeDirectory("\\");
        root.AddFile("A.TXT", 100);
        Vm.CurrentFolder = FakeFolders.Root(root);

        root.AddFile("B.TXT", 23);
        Vm.CurrentFolder = FakeFolders.Root(root);
        Assert.Equal($"{SizeUnit.Bytes.FormatSize(100)} in 1 item(s)", Vm.SelectionSizeText);

        context.AppDialogs.SettingsAnswer = new SettingsChangeResult(false);
        Vm.ShowSettingsCommand.Execute(null);
        Vm.CurrentFolder = FakeFolders.Root(root);
        Assert.Equal($"{SizeUnit.Bytes.FormatSize(123)} in 2 item(s)", Vm.SelectionSizeText);
    }

    [Fact]
    public async Task TheStatusBarUsesTheSizeUnitFromTheSettings()
    {
        await context.OpenFloppyAsync();
        context.AppDialogs.SettingsAnswer = new SettingsChangeResult(false);
        context.AppDialogs.WhileSettingsOpen = () => context.Settings.Current.SizeUnit = SizeUnit.Binary;

        Vm.ShowSettingsCommand.Execute(null);

        Assert.Equal($"Partition size: {SizeUnit.Binary.FormatSize((ulong)Vm.CurrentPartition!.Length)}", Vm.PartitionSizeText);
    }

    #endregion
}
