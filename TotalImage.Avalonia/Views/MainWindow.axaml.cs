using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Avalonia.VisualTree;
using TotalImage.Avalonia.Controls;
using TotalImage.Avalonia.Converters;
using TotalImage.Avalonia.Platform;
using TotalImage.Avalonia.Services;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Main;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Avalonia.Views;

/// <summary>
/// The main window, bound to <see cref="MainViewModel"/>. The code here only wires controls to
/// the view model: selection and sorting of the file list, the folder tree, menus that are
/// built from lists, type-ahead, renaming in place, drag and drop, and the window state.
/// </summary>
public partial class MainWindow : Window
{
    private static readonly string[] ColumnNames = ["Name", "Type", "Size", "Modified", "Attributes"];

    private static readonly (ViewMode Mode, string Header)[] ViewModes =
    [
        (ViewMode.LargeIcon, "_Large icons"),
        (ViewMode.SmallIcon, "_Small icons"),
        (ViewMode.List, "L_ist"),
        (ViewMode.Details, "_Details")
    ];

    private readonly MainViewModel viewModel;
    private readonly AvaloniaUIStateService uiState;

    // Set while the controls are updated from the view model, so their events don't echo back.
    private bool updatingControls;

    // Type-ahead: the last character typed and the entry it selected.
    private char lastTypeAheadChar;
    private int lastTypeAheadIndex = -1;

    // Drag out: the left button going down on an entry or folder, until the pointer moves far enough.
    // Kept because a drag can only be started from a press.
    private PointerPressedEventArgs? dragPress;
    private BrowsePane dragPane;
    private bool isDraggingOut;

    // The width of the folder tree while it is hidden.
    private double folderTreeWidth;

    // For the XAML previewer.
    public MainWindow()
    {
        InitializeComponent();
        viewModel = null!;
        uiState = null!;
    }

    public MainWindow(MainViewModel viewModel, ISettingsService settings, AvaloniaUIStateService uiState, WindowOwner owner, MainWindows windows)
    {
        this.viewModel = viewModel;
        this.uiState = uiState;
        owner.Window = this;

        InitializeComponent();
        DataContext = viewModel;

        BuildViewAndSortMenus();
        NameCommandBarButtons();
        RestoreWindowState(isFirstWindow: windows.Count == 0);
        WireFileList();
        WireFolderTree();
        WireDragAndDrop();

        partitionComboBox.SelectionChanged += PartitionComboBox_SelectionChanged;
        Closing += MainWindow_Closing;
        Opened += MainWindow_Opened;

        viewModel.PropertyChanged += ViewModel_PropertyChanged;
        viewModel.CloseRequested += (_, _) => Close();
        viewModel.RenameRequested += ViewModel_RenameRequested;
        viewModel.SettingsApplied += (_, _) => App.ApplyColorTheme(settings.Current.ColorMode);

        // Show the current state.
        foreach (var property in new[]
        {
            nameof(MainViewModel.Entries), nameof(MainViewModel.ViewMode), nameof(MainViewModel.ShowDirectoryTree),
            nameof(MainViewModel.Partitions), nameof(MainViewModel.RecentImages), nameof(MainViewModel.FreeSpacePercentage),
            nameof(MainViewModel.SortColumn)
        })
        {
            ViewModel_PropertyChanged(viewModel, new PropertyChangedEventArgs(property));
        }
    }

    /// <summary>
    /// The image file to open once the window is shown, for example one passed on the command line.
    /// </summary>
    public string? StartupImagePath { get; set; }

    private async void MainWindow_Opened(object? sender, EventArgs e)
    {
        viewModel.LoadSettings();
        if (StartupImagePath is { } path)
            await viewModel.OpenImageFileAsync(path);
    }

    #region View model changes

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(MainViewModel.Entries):
                ShowEntries();
                break;
            case nameof(MainViewModel.SelectedEntries):
                ShowSelectedEntries();
                break;
            case nameof(MainViewModel.ViewMode):
                ShowViewMode();
                break;
            case nameof(MainViewModel.SortColumn):
            case nameof(MainViewModel.SortDirection):
                ShowSortColumn();
                break;
            case nameof(MainViewModel.ShowDirectoryTree):
                ShowFolderTree(viewModel.ShowDirectoryTree);
                break;
            case nameof(MainViewModel.CurrentFolder):
                ShowCurrentFolder();
                break;
            case nameof(MainViewModel.ActivePane):
                // Commands that switch to the file list (like selecting everything) move the focus there.
                if (viewModel.ActivePane == BrowsePane.Files && folderTree.IsKeyboardFocusWithin)
                    ActiveFileList.Focus();
                break;
            case nameof(MainViewModel.Partitions):
            case nameof(MainViewModel.CurrentPartitionIndex):
                ShowPartitions();
                break;
            case nameof(MainViewModel.RecentImages):
                ShowRecentImages();
                break;
            case nameof(MainViewModel.FreeSpacePercentage):
                ShowFreeSpace();
                break;
        }
    }

    private void ShowEntries()
    {
        lastTypeAheadChar = '\0';
        lastTypeAheadIndex = -1;

        updatingControls = true;
        try
        {
            fileList.ItemsSource = viewModel.Entries;
            fileIconList.ItemsSource = viewModel.Entries;
        }
        finally
        {
            updatingControls = false;
        }

        ShowSelectedEntries();
    }

    private void ShowSelectedEntries()
    {
        var selected = viewModel.SelectedEntries;
        updatingControls = true;
        try
        {
            if (!fileList.SelectedItems.Cast<object>().SequenceEqual(selected))
            {
                fileList.SelectedItems.Clear();
                foreach (var entry in selected)
                    fileList.SelectedItems.Add(entry);
            }

            if (fileIconList.SelectedItems is { } iconSelection && !iconSelection.Cast<object>().SequenceEqual(selected))
            {
                iconSelection.Clear();
                foreach (var entry in selected)
                    iconSelection.Add(entry);
            }
        }
        finally
        {
            updatingControls = false;
        }
    }

    private void ShowViewMode()
    {
        var mode = viewModel.ViewMode;
        var details = mode == ViewMode.Details;
        var hadFocus = fileList.IsKeyboardFocusWithin || fileIconList.IsKeyboardFocusWithin;

        fileList.IsVisible = details;
        fileIconList.IsVisible = !details;
        if (!details)
        {
            var list = mode == ViewMode.List;
            fileIconList.ItemTemplate = Resources[mode is ViewMode.LargeIcon or ViewMode.Tile ? "LargeIconTemplate" : "SmallIconTemplate"] as IDataTemplate;
            fileIconList.ItemsPanel = (ITemplate<Panel?>)Resources[list ? "WrapColumnsPanel" : "WrapRowsPanel"]!;
            ScrollViewer.SetHorizontalScrollBarVisibility(fileIconList, list ? global::Avalonia.Controls.Primitives.ScrollBarVisibility.Auto : global::Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled);
            ScrollViewer.SetVerticalScrollBarVisibility(fileIconList, list ? global::Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled : global::Avalonia.Controls.Primitives.ScrollBarVisibility.Auto);
        }

        ShowSelectedEntries();
        if (hadFocus)
            ActiveFileList.Focus();
    }

    // The column headers show the sort column and direction.
    private void ShowSortColumn()
    {
        foreach (var column in fileList.Columns)
        {
            if (column.Tag is not int index)
                continue;

            var arrow = index != viewModel.SortColumn ? string.Empty
                : viewModel.SortDirection == SortDirection.Descending ? " ▼" : " ▲";
            column.Header = ColumnNames[index] + arrow;
        }
    }

    private void ShowFolderTree(bool show)
    {
        var column = browseGrid.ColumnDefinitions[0];
        if (show)
        {
            column.Width = new GridLength(folderTreeWidth > 0 ? folderTreeWidth : uiState.Current.FolderTreeWidth);
        }
        else
        {
            if (column.ActualWidth > 0)
                folderTreeWidth = column.ActualWidth;
            column.Width = new GridLength(0);
        }

        folderTree.IsVisible = show;
        splitter.IsVisible = show;
    }

    private void ShowCurrentFolder()
    {
        if (viewModel.CurrentFolder is not { } folder)
            return;

        // Show the folder in the tree: expand the folders above it and select it.
        for (var parent = folder.Parent; parent is not null; parent = parent.Parent)
            parent.IsExpanded = true;

        updatingControls = true;
        try
        {
            folderTree.SelectedItem = folder;
        }
        finally
        {
            updatingControls = false;
        }

        Dispatcher.UIThread.Post(() => folderTree.TreeContainerFromItem(folder)?.BringIntoView(), DispatcherPriority.Background);
    }

    private void ShowPartitions()
    {
        updatingControls = true;
        try
        {
            partitionComboBox.ItemsSource = viewModel.Partitions;
            partitionComboBox.SelectedItem = viewModel.Partitions.FirstOrDefault(option => option.Index == viewModel.CurrentPartitionIndex);
        }
        finally
        {
            updatingControls = false;
        }
    }

    private void ShowRecentImages()
    {
        var recentImages = viewModel.RecentImages;
        recentFilesMenuItem.Items.Clear();
        for (int i = 0; i < recentImages.Count; i++)
        {
            recentFilesMenuItem.Items.Add(new MenuItem
            {
                // A text block, so underscores in paths aren't taken as access keys.
                Header = new TextBlock { Text = $"{i + 1}: {recentImages[i]}" },
                Command = viewModel.OpenRecentImageCommand,
                CommandParameter = recentImages[i]
            });
        }
    }

    // The bar shows the used space, red when little is free.
    private void ShowFreeSpace()
    {
        var free = viewModel.FreeSpacePercentage;
        freeSpaceBar.Value = 100 - free;
        freeSpaceBar.Classes.Set("low", free <= 10);
        freeSpaceBar.Classes.Set("medium", free is > 10 and <= 20);
        freeSpaceBar.Classes.Set("high", free > 20);
    }

    private void PartitionComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // The combo box leaves out partitions that can't be loaded, so its index isn't the partition's.
        if (!updatingControls && partitionComboBox.SelectedItem is PartitionOption option)
            viewModel.SelectPartition(option.Index);
    }

    #endregion

    #region Menus

    // The view and sort items appear in the View menu, the command bar and the file list's context
    // menu. They are built here so the three stay the same.
    private void BuildViewAndSortMenus()
    {

        foreach (var item in CreateSortItems("MainSort"))
            sortByMenuItem.Items.Add(item);

        contextViewMenuItem.Items.Add(CreateShowHiddenItem());
        contextViewMenuItem.Items.Add(new Separator());
        foreach (var item in CreateViewModeItems("ContextViewMode"))
            contextViewMenuItem.Items.Add(item);
        foreach (var item in CreateSortItems("ContextSort"))
            contextSortMenuItem.Items.Add(item);

        var viewFlyout = new MenuFlyout();
        viewFlyout.Items.Add(CreateShowHiddenItem());
        viewFlyout.Items.Add(new Separator());
        foreach (var item in CreateViewModeItems("BarViewMode"))
            viewFlyout.Items.Add(item);
        viewDropDownButton.Flyout = viewFlyout;

        var sortFlyout = new MenuFlyout();
        foreach (var item in CreateSortItems("BarSort"))
            sortFlyout.Items.Add(item);
        sortDropDownButton.Flyout = sortFlyout;
    }

    // The command bar buttons only show icons; screen readers read their tooltips.
    private void NameCommandBarButtons()
    {
        foreach (var child in commandBar.Children)
        {
            if (ToolTip.GetTip(child) is string tip)
                AutomationProperties.SetName(child, tip);
        }
    }

    private MenuItem CreateShowHiddenItem() => new()
    {
        Header = "Show _hidden objects",
        ToggleType = MenuItemToggleType.CheckBox,
        Command = viewModel.ToggleShowHiddenItemsCommand,
        [!MenuItem.IsCheckedProperty] = new Binding(nameof(MainViewModel.ShowHiddenItems)) { Source = viewModel, Mode = BindingMode.OneWay }
    };

    private IEnumerable<MenuItem> CreateViewModeItems(string group) => ViewModes.Select(view => new MenuItem
    {
        Header = view.Header,
        ToggleType = MenuItemToggleType.Radio,
        GroupName = group,
        Command = viewModel.SetViewModeCommand,
        CommandParameter = view.Mode,
        [!MenuItem.IsCheckedProperty] = new Binding(nameof(MainViewModel.ViewMode))
        {
            Source = viewModel,
            Mode = BindingMode.OneWay,
            Converter = ValueConverters.EqualsParameter,
            ConverterParameter = view.Mode
        }
    });

    // Item i sorts by file list column i.
    private IEnumerable<MenuItem> CreateSortItems(string group) => ColumnNames.Select((name, column) => new MenuItem
    {
        Header = name,
        ToggleType = MenuItemToggleType.Radio,
        GroupName = group,
        Command = viewModel.SortByCommand,
        CommandParameter = column,
        [!MenuItem.IsCheckedProperty] = new Binding(nameof(MainViewModel.SortColumn))
        {
            Source = viewModel,
            Mode = BindingMode.OneWay,
            Converter = ValueConverters.EqualsParameter,
            ConverterParameter = column
        }
    });

    #endregion

    #region File list

    // The control showing the entries in the current view mode.
    private Control ActiveFileList => viewModel.ViewMode == ViewMode.Details ? fileList : fileIconList;

    private void WireFileList()
    {
        fileList.SelectionChanged += (_, _) => OnFileListSelectionChanged(fileList.SelectedItems);
        fileList.LoadingRow += (_, e) => EntryColors.Apply(e.Row, e.Row.DataContext as EntryViewModel);
        fileIconList.ContainerPrepared += (_, e) =>
        {
            if (e.Container is ListBoxItem item)
            {
                EntryColors.Apply(item, item.DataContext as EntryViewModel);
                AutomationProperties.SetName(item, (item.DataContext as EntryViewModel)?.Name);
            }
        };
        fileIconList.SelectionChanged += (_, _) => OnFileListSelectionChanged(fileIconList.SelectedItems);

        // Sorting is the view model's: the grid only reports header clicks.
        fileList.Sorting += (_, e) =>
        {
            e.Handled = true;
            if (e.Column.Tag is int column)
                viewModel.SortByCommand.Execute(column);
        };

        foreach (var list in new Control[] { fileList, fileIconList })
        {
            list.GotFocus += (_, _) => viewModel.ActivePane = BrowsePane.Files;
            list.DoubleTapped += FileList_DoubleTapped;
            list.AddHandler(KeyDownEvent, FileList_KeyDown, RoutingStrategies.Tunnel);
            list.AddHandler(TextInputEvent, FileList_TextInput, RoutingStrategies.Tunnel);
            list.AddHandler(PointerPressedEvent, FileList_PointerPressed, RoutingStrategies.Tunnel);
            list.AddHandler(PointerMovedEvent, BrowsePane_PointerMoved, RoutingStrategies.Tunnel);
            list.AddHandler(PointerReleasedEvent, (_, _) => dragPress = null, RoutingStrategies.Tunnel);
        }

        fileListContextMenu.Opening += (_, e) =>
        {
            if (!viewModel.IsImageOpen || viewModel.IsParentEntrySelected)
            {
                e.Cancel = true;
                return;
            }

            // The menu acts on the file list, so its commands follow the file list's rules.
            viewModel.ActivePane = BrowsePane.Files;
        };
    }

    private void OnFileListSelectionChanged(System.Collections.IList? selectedItems)
    {
        if (updatingControls || selectedItems is null)
            return;

        viewModel.SelectEntries(selectedItems.OfType<EntryViewModel>().ToList());

        // The view model may leave out the ".." entry; show what it selected.
        if (!selectedItems.Cast<object>().SequenceEqual(viewModel.SelectedEntries))
            Dispatcher.UIThread.Post(ShowSelectedEntries);
    }

    private async void FileList_DoubleTapped(object? sender, TappedEventArgs e)
    {
        if (EntryAt(e.Source) is not null && viewModel.SelectedEntries is [var entry])
            await viewModel.OpenEntryAsync(entry);
    }

    private async void FileList_KeyDown(object? sender, KeyEventArgs e)
    {
        // Handled before the controls see them: the grid would move to the next row on Enter.
        switch (e.Key)
        {
            case Key.Enter when e.KeyModifiers == KeyModifiers.Alt:
                e.Handled = true;
                Execute(viewModel.ShowPropertiesCommand);
                break;
            case Key.Enter when e.KeyModifiers == KeyModifiers.None:
                e.Handled = true;
                if (viewModel.SelectedEntries is [var entry])
                    await viewModel.OpenEntryAsync(entry);
                break;
            case Key.Back when e.KeyModifiers == KeyModifiers.None:
                e.Handled = true;
                Execute(viewModel.GoToParentFolderCommand);
                break;
            case Key.F2 when e.KeyModifiers == KeyModifiers.None:
                e.Handled = true;
                Execute(viewModel.RenameCommand);
                break;
            case Key.Delete when e.KeyModifiers == KeyModifiers.None:
                e.Handled = true;
                Execute(viewModel.DeleteCommand);
                break;
        }

        static void Execute(System.Windows.Input.ICommand command)
        {
            if (command.CanExecute(null))
                command.Execute(null);
        }
    }

    // Selects the first entry whose name starts with the character typed, then the next one, etc.
    private void FileList_TextInput(object? sender, TextInputEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Text))
            return;

        char ch = char.ToLowerInvariant(e.Text[0]);
        if (!char.IsLetterOrDigit(ch) && ch != '_' && ch != '-' && ch != '.')
            return;

        int startIndex = ch == lastTypeAheadChar && lastTypeAheadIndex >= 0 ? lastTypeAheadIndex + 1 : 0;
        if (startIndex == 0)
            lastTypeAheadIndex = -1;

        // Search from startIndex, wrapping around once if needed. The ".." entry is never selected this way.
        var entries = viewModel.Entries;
        for (int i = 0; i < entries.Count; i++)
        {
            int index = (startIndex + i) % entries.Count;
            if (!entries[index].IsParentEntry && entries[index].Name.StartsWith(ch.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                viewModel.SelectEntries([entries[index]]);
                ScrollIntoView(entries[index]);
                lastTypeAheadChar = ch;
                lastTypeAheadIndex = index;
                e.Handled = true;
                return;
            }
        }
    }

    private void FileList_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        dragPress = null;
        if (EntryAt(e.Source) is not { } entry)
            return;

        if (point.Properties.IsLeftButtonPressed)
        {
            if (!entry.IsParentEntry)
            {
                dragPress = e;
                dragPane = BrowsePane.Files;
            }
        }
        else if (point.Properties.IsRightButtonPressed && !viewModel.SelectedEntries.Contains(entry))
        {
            // The context menu acts on the entry under the pointer.
            viewModel.SelectEntries([entry]);
        }
    }

    private void ScrollIntoView(EntryViewModel entry)
    {
        if (viewModel.ViewMode == ViewMode.Details)
            fileList.ScrollIntoView(entry, null);
        else
            fileIconList.ScrollIntoView(entry);
    }

    // The entry of the row or item a pointer event happened on, or null (for example on a header).
    private static EntryViewModel? EntryAt(object? source)
    {
        for (var visual = source as Visual; visual is not null; visual = visual.GetVisualParent())
        {
            if (visual is DataGridColumnHeader)
                return null;
            if (visual is DataGridRow or ListBoxItem)
                return visual.DataContext as EntryViewModel;
        }

        return null;
    }

    // The control showing an entry, once it has been scrolled into view.
    private async Task<Control?> ContainerOfAsync(EntryViewModel entry)
    {
        ScrollIntoView(entry);
        await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background);

        if (viewModel.ViewMode != ViewMode.Details)
            return fileIconList.ContainerFromItem(entry);

        return fileList.GetVisualDescendants().OfType<DataGridRow>().FirstOrDefault(row => row.DataContext == entry);
    }

    #endregion

    #region Folder tree

    private void WireFolderTree()
    {
        folderTree.SelectionChanged += (_, _) =>
        {
            if (!updatingControls && folderTree.SelectedItem is FolderViewModel folder)
                viewModel.CurrentFolder = folder;
        };
        folderTree.GotFocus += (_, _) => viewModel.ActivePane = BrowsePane.Folders;
        folderTree.AddHandler(PointerPressedEvent, FolderTree_PointerPressed, RoutingStrategies.Tunnel);
        folderTree.AddHandler(PointerMovedEvent, BrowsePane_PointerMoved, RoutingStrategies.Tunnel);
        folderTree.AddHandler(PointerReleasedEvent, (_, _) => dragPress = null, RoutingStrategies.Tunnel);

        folderTreeContextMenu.Opening += (_, e) =>
        {
            if (viewModel.RootFolder is null || viewModel.CurrentFolder is null)
            {
                e.Cancel = true;
                return;
            }

            // The menu acts on the folder tree, so its commands follow the folder tree's rules.
            viewModel.ActivePane = BrowsePane.Folders;
        };
    }

    private void FolderTree_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        dragPress = null;
        if (FolderAt(e.Source) is not { } folder)
            return;

        if (point.Properties.IsLeftButtonPressed)
        {
            dragPress = e;
            dragPane = BrowsePane.Folders;
        }
        else if (point.Properties.IsRightButtonPressed)
        {
            // The context menu acts on the folder under the pointer.
            viewModel.CurrentFolder = folder;
        }
    }

    private static FolderViewModel? FolderAt(object? source)
    {
        for (var visual = source as Visual; visual is not null; visual = visual.GetVisualParent())
        {
            if (visual is TreeViewItem item)
                return item.DataContext as FolderViewModel;
        }

        return null;
    }

    #endregion

    #region Renaming

    // Renaming edits the name in place.
    private async void ViewModel_RenameRequested(object? sender, RenameRequestedEventArgs e)
    {
        if (e.Entry is { } entry)
        {
            if (!viewModel.CanRenameEntry(entry) || await ContainerOfAsync(entry) is not { } target)
                return;

            if (await InlineRename.EditAsync(target, entry.Name) is { } newName)
                viewModel.RenameEntry(entry, newName);
        }
        else if (e.Folder is { } folder)
        {
            if (!viewModel.CanRenameFolder(folder) || folderTree.TreeContainerFromItem(folder) is not Control target)
                return;

            if (await InlineRename.EditAsync(target, folder.Name) is { } newName)
                viewModel.RenameFolder(folder, newName);
        }
    }

    #endregion

    #region Drag and drop

    private void WireDragAndDrop()
    {
        AddHandler(DragDrop.DragOverEvent, Window_DragOver);
        AddHandler(DragDrop.DropEvent, Window_Drop);
    }

    // Dragging entries or a folder out of the window: they are extracted to a temporary folder first.
    private async void BrowsePane_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (dragPress is not { } press || !e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            return;

        var delta = e.GetPosition(this) - press.GetPosition(this);
        if (Math.Abs(delta.X) < 4 && Math.Abs(delta.Y) < 4)
            return;

        dragPress = null;
        IReadOnlyList<string> paths;
        if (dragPane == BrowsePane.Files)
            paths = await viewModel.PrepareDragOutAsync(viewModel.SelectedEntries);
        else if (viewModel.CurrentFolder is { } folder)
            paths = await viewModel.PrepareDragOutAsync(folder);
        else
            return;

        if (paths.Count == 0)
            return;

        var effect = DragDropEffects.None;
        isDraggingOut = true;
        try
        {
            var data = await DragOutData.CreateAsync(StorageProvider, paths);
            effect = await DragDrop.DoDragDropAsync(press, data, DragDropEffects.Copy);
        }
        finally
        {
            isDraggingOut = false;
            viewModel.EndDragOut(dropped: effect != DragDropEffects.None);
        }
    }

    private void Window_DragOver(object? sender, DragEventArgs e)
    {
        // Dropping the entries being dragged out back into the window is not supported.
        e.DragEffects = !isDraggingOut && DroppedPaths(e) is { } paths && viewModel.CanAcceptDrop(paths)
            ? DragDropEffects.Copy
            : DragDropEffects.None;
    }

    // Files dragged into the window from outside it.
    private async void Window_Drop(object? sender, DragEventArgs e)
    {
        if (!isDraggingOut && DroppedPaths(e) is { } paths)
            await viewModel.DropAsync(paths);
    }

    private static List<string>? DroppedPaths(DragEventArgs e)
        => e.DataTransfer.TryGetFiles() is { } files
            ? files.Select(file => file.TryGetLocalPath()).OfType<string>().ToList()
            : null;

    #endregion

    #region Window state

    private async void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
    {
        if (!viewModel.IsCloseConfirmed && viewModel.HasUnsavedChanges)
        {
            // Ask first, and close again once the answer allows it.
            e.Cancel = true;
            if (await viewModel.ConfirmCloseWindowAsync())
                Close();

            return;
        }

        SaveWindowState();
        uiState.Save();
        viewModel.OnWindowClosing();
    }

    private void RestoreWindowState(bool isFirstWindow)
    {
        var state = uiState.Current;
        Width = state.WindowWidth;
        Height = state.WindowHeight;
        folderTreeWidth = state.FolderTreeWidth;
        browseGrid.ColumnDefinitions[0].Width = new GridLength(state.FolderTreeWidth);

        // Later windows are centered rather than put on top of the first one.
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        if (isFirstWindow && state.WindowX is int x && state.WindowY is int y
            && Screens.All.Any(screen => screen.WorkingArea.Contains(new PixelPoint(x + 32, y + 32))))
        {
            WindowStartupLocation = WindowStartupLocation.Manual;
            Position = new PixelPoint(x, y);
        }

        if (state.IsMaximized)
            WindowState = WindowState.Maximized;

        foreach (var column in fileList.Columns)
        {
            if (column.Tag is int index && index < state.ColumnWidths.Count && state.ColumnWidths[index] > 0)
                column.Width = new DataGridLength(state.ColumnWidths[index]);
        }
    }

    private void SaveWindowState()
    {
        var state = uiState.Current;
        if (WindowState != WindowState.Minimized)
        {
            state.IsMaximized = WindowState == WindowState.Maximized;
            if (WindowState == WindowState.Normal)
            {
                state.WindowWidth = Width;
                state.WindowHeight = Height;
                state.WindowX = Position.X;
                state.WindowY = Position.Y;
            }
        }

        var treeWidth = viewModel.ShowDirectoryTree ? browseGrid.ColumnDefinitions[0].ActualWidth : folderTreeWidth;
        if (treeWidth > 0)
            state.FolderTreeWidth = treeWidth;

        if (fileList.IsVisible)
        {
            var widths = new List<double>(state.ColumnWidths);
            while (widths.Count < ColumnNames.Length)
                widths.Add(100);
            foreach (var column in fileList.Columns)
            {
                if (column.Tag is int index && column.ActualWidth > 0)
                    widths[index] = column.ActualWidth;
            }

            state.ColumnWidths = widths;
        }
    }

    #endregion
}


