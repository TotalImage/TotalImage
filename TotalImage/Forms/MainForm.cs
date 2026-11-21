using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.Changes;
using TotalImage.Controls;
using TotalImage.Services;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Main;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;
using TotalImage.ViewModels.Sorting;
using TotalImage.FileSystems.FAT;
using TiDirectory = TotalImage.FileSystems.Directory;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Forms
{
    public partial class MainForm : Form, IMainViewBridge
    {
        private TiDirectory? lastViewedDir;
        private TiDirectory? draggedDir;

        public string tempDir;

        // Cached fonts to avoid allocating a new GDI Font object per list item
        private readonly Font _monoFont = new(FontFamily.GenericMonospace, 9);

        // Cached image list indices, populated after icons are loaded
        private int _smallFolderIndex = -1;
        private int _smallFolderHiddenIndex = -1;

        // Shared ColorMatrix/ImageAttributes used by CreateHiddenIcon - constructed once
        private static readonly ColorMatrix _hiddenIconColorMatrix = new() { Matrix33 = 0.65f };
        private static readonly ImageAttributes _hiddenIconAttributes = CreateHiddenIconAttributes();

        private ListViewItem upOneFolderListViewItem = new()
        {
            Text = "..",
            ToolTipText = "Parent directory"
        };

        private List<ListViewItem> currentFolderView = new();
        private bool _updatingSelection = false;

        //For tracking which listviewitem was last selected by keypress
        private char _lastTypeAheadChar = '\0';
        private int _lastTypeAheadIndex = -1;

        // TEMPORARY (stage 5): the pane that has (or last had) the focus, reported to the view model.
        private BrowsePane focusedPane = BrowsePane.Files;

        // Set while the partition combo box is filled from the view model.
        private bool updatingPartitionComboBox;

        private readonly MainViewModel viewModel;
        private readonly ISettingsService settings;
        private readonly ITempDirectoryService tempDirectory;
        private readonly UIStateService uiState;
        private readonly IEntryDialogs entryDialogs;
        private readonly IFilePickerService filePicker;

        public MainForm(MainViewModel viewModel, ISettingsService settings, ITempDirectoryService tempDirectory, UIStateService uiState,
            DialogOwner dialogOwner, IEntryDialogs entryDialogs, IFilePickerService filePicker)
        {
            this.viewModel = viewModel;
            this.settings = settings;
            this.tempDirectory = tempDirectory;
            this.uiState = uiState;
            this.entryDialogs = entryDialogs;
            this.filePicker = filePicker;
            dialogOwner.Window = this;

            InitializeComponent();

            //Scale the ImageList images according to current Dpi scale
            using (Graphics g = CreateGraphics())
            {
                imgFilesSmall.ImageSize = new SizeF(16 * (g.DpiX / 96f), 16 * (g.DpiY / 96f)).ToSize();
                imgFilesLarge.ImageSize = new SizeF(32 * (g.DpiX / 96f), 32 * (g.DpiY / 96f)).ToSize();
            }

            viewModel.View = this;
            BindCommands();
        }

        #region View model bindings

        private void BindCommands()
        {
            var vm = viewModel;

            // File
            vm.NewImageCommand.BindCommand(newToolStripMenuItem, newToolStripButton);
            vm.OpenImageCommand.BindCommand(openToolStripMenuItem, openToolStripButton);
            vm.OpenContainingFolderCommand.BindCommand(openContainingFolderToolStripMenuItem);
            vm.SaveCommand.BindCommand(saveToolStripMenuItem, saveToolStripButton);
            vm.SaveAsCommand.BindCommand(saveAsToolStripMenuItem, saveAsToolStripButton);
            vm.ReloadImageCommand.BindCommand(reloadImageToolStripMenuItem, reloadImageToolStripButton);
            vm.CloseImageCommand.BindCommand(closeImageToolStripMenuItem, closeImageToolStripButton);
            vm.ExitCommand.BindCommand(exitToolStripMenuItem);

            // Edit and the context menus
            vm.SelectAllCommand.BindCommand(selectAllToolStripMenuItem, selectAllToolStripMenuItem1);
            vm.InjectFilesCommand.BindCommand(injectToolStripMenuItem, injectToolStripMenuItem1, injectToolStripMenuItem2, injectToolStripButton);
            vm.ExtractCommand.BindCommand(extractToolStripMenuItem, extractToolStripMenuItem1, extractToolStripMenuItem2, extractToolStripButton);
            vm.NewFolderCommand.BindCommand(newFolderToolStripMenuItem, newFolderToolStripMenuItem1, newFolderToolStripMenuItem2, newFolderToolStripButton);
            vm.DeleteCommand.BindCommand(deleteToolStripMenuItem, deleteToolStripMenuItem1, deleteToolStripMenuItem2, deleteToolStripButton);
            vm.RenameCommand.BindCommand(renameToolStripMenuItem, renameToolStripMenuItem1, renameToolStripMenuItem2);
            vm.ShowPropertiesCommand.BindCommand(propertiesToolStripMenuItem, propertiesToolStripMenuItem1, propertiesToolStripMenuItem2, propertiesToolStripButton);
            vm.ChangeVolumeLabelCommand.BindCommand(changeVolumeLabelToolStripMenuItem, labelToolStripMenuButton);
            vm.ChangeFormatCommand.BindCommand(changeGeometryToolStripMenuItem);
            vm.DefragmentCommand.BindCommand(defragmentToolStripMenuItem);
            vm.FormatCommand.BindCommand(formatDiskToolStripMenuItem);
            vm.ShowBootSectorCommand.BindCommand(bootSectorPropertiesToolStripMenuItem, bootsectToolStripButton);
            vm.ChoosePartitionCommand.BindCommand(selectPartitionToolStripMenuItem);
            vm.ManagePartitionsCommand.BindCommand(managePartitionsToolStripMenuItem, managePartitionsToolStripButton);

            // View
            vm.GoToParentFolderCommand.BindCommand(parentDirectoryToolStripMenuItem, parentDirectoryToolStripMenuItem1, parentDirectoryToolStripButton);
            vm.ExpandFolderTreeCommand.BindCommand(expandDirectoryTreeToolStripMenuItem, expandDirectoryTreeToolStripMenuItem1);
            vm.CollapseFolderTreeCommand.BindCommand(collapseDirectoryTreeToolStripMenuItem, collapseDirectoryTreeToolStripMenuItem1);
            vm.ToggleShowHiddenItemsCommand.BindCommand(showHiddenObjectsToolStripMenuItem, showHiddenObjectsToolStripMenuItem1, showHiddenObjectsToolStripMenuItem2);
            vm.ToggleCommandBarCommand.BindCommand(commandBarToolStripMenuItem, commandBarToolStripMenuItem1);
            vm.ToggleDirectoryTreeCommand.BindCommand(directoryTreeToolStripMenuItem, directoryTreeToolStripMenuItem1);
            vm.ToggleStatusBarCommand.BindCommand(statusBarToolStripMenuItem, statusBarToolStripMenuItem1);

            foreach (var (mode, items) in ViewModeItems)
                vm.SetViewModeCommand.BindCommand(mode, items);

            // Item i sorts by file list column i.
            for (int column = 0; column < SortItems.Length; column++)
                vm.SortByCommand.BindCommand(column, SortItems[column]);

            // Tools and help
            vm.ShowDeletedObjectsCommand.BindCommand(deletedObjectsToolStripMenuItem);
            vm.ShowHexViewCommand.BindCommand(hexViewToolStripMenuItem);
            vm.ShowImageInformationCommand.BindCommand(imageInformationToolStripMenuItem, infoToolStripButton);
            vm.ShowSettingsCommand.BindCommand(settingsToolStripMenuItem1, settingsToolStripButton);
            vm.ShowAboutCommand.BindCommand(aboutToolStripMenuItem);
            vm.ShowNotificationsCommand.BindCommand(notificationsLabel);
        }

        private (ViewMode Mode, ToolStripMenuItem[] Items)[] ViewModeItems =>
        [
            (ViewMode.LargeIcon, [largeIconsToolStripMenuItem, largeIconsToolStripMenuItem1, largeIconsToolStripMenuItem2]),
            (ViewMode.SmallIcon, [smallIconsToolStripMenuItem, smallIconsToolStripMenuItem1, smallIconsToolStripMenuItem2]),
            (ViewMode.List, [listToolStripMenuItem, listToolStripMenuItem1, listToolStripMenuItem2]),
            (ViewMode.Details, [detailsToolStripMenuItem, detailsToolStripMenuItem1, detailsToolStripMenuItem2])
        ];

        // The sort menu items of each file list column, in column order.
        private ToolStripMenuItem[][] SortItems =>
        [
            [nameToolStripMenuItem, nameToolStripMenuItem1, nameToolStripMenuItem2],
            [typeToolStripMenuItem, typeToolStripMenuItem1, typeToolStripMenuItem2],
            [sizeToolStripMenuItem, sizeToolStripMenuItem1, sizeToolStripMenuItem2],
            [modifiedToolStripMenuItem, modifiedToolStripMenuItem1, modifiedToolStripMenuItem2],
            [attributesToolStripMenuItem, attributesToolStripMenuItem1, attributesToolStripMenuItem2]
        ];

        // Bound once the window exists (from Load), so the controls can be updated right away.
        private void BindState()
        {
            var vm = viewModel;

            vm.Observe(nameof(MainViewModel.Title), () => Text = vm.Title);

            // View options
            vm.Observe(nameof(MainViewModel.ViewMode), () => lstFiles.View = vm.ViewMode.ToWinForms());
            vm.Observe(nameof(MainViewModel.ShowCommandBar), () => commandBar.Visible = vm.ShowCommandBar);
            vm.Observe(nameof(MainViewModel.ShowDirectoryTree), () => splitContainer.Panel1Collapsed = !vm.ShowDirectoryTree);
            vm.Observe(nameof(MainViewModel.ShowStatusBar), () => statusBar.Visible = vm.ShowStatusBar);
            vm.Observe(nameof(MainViewModel.ShowHiddenItems), ResetView);
            vm.Observe(nameof(MainViewModel.SortColumn), SortListView);
            vm.Observe(nameof(MainViewModel.SortDirection), SortListView);

            vm.BindChecked(nameof(MainViewModel.ShowHiddenItems), () => vm.ShowHiddenItems,
                showHiddenObjectsToolStripMenuItem, showHiddenObjectsToolStripMenuItem1, showHiddenObjectsToolStripMenuItem2);
            vm.BindChecked(nameof(MainViewModel.ShowCommandBar), () => vm.ShowCommandBar, commandBarToolStripMenuItem, commandBarToolStripMenuItem1);
            vm.BindChecked(nameof(MainViewModel.ShowDirectoryTree), () => vm.ShowDirectoryTree, directoryTreeToolStripMenuItem, directoryTreeToolStripMenuItem1);
            vm.BindChecked(nameof(MainViewModel.ShowStatusBar), () => vm.ShowStatusBar, statusBarToolStripMenuItem, statusBarToolStripMenuItem1);
            foreach (var (mode, items) in ViewModeItems)
                vm.BindChecked(nameof(MainViewModel.ViewMode), () => vm.ViewMode == mode, items);
            var sortItems = SortItems;
            for (int column = 0; column < sortItems.Length; column++)
            {
                var sortColumn = column;
                vm.BindChecked(nameof(MainViewModel.SortColumn), () => vm.SortColumn == sortColumn, sortItems[column]);
            }

            // Image session
            vm.Observe(nameof(MainViewModel.RecentImages), PopulateRecentList);
            vm.Observe(nameof(MainViewModel.Partitions), SyncPartitionComboBox);
            vm.Observe(nameof(MainViewModel.CurrentPartitionIndex), SyncPartitionComboBox);
            vm.Observe(nameof(MainViewModel.CurrentFileSystem), LoadPartition);
            vm.Observe(nameof(MainViewModel.IsImageOpen), () =>
            {
                var open = vm.IsImageOpen;
                partitionComboBox.Enabled = open;
                freeSpaceProgressBar.Visible = open;

                // Border sides give the status bar children separator-like looks while an image is open.
                partitionSizeLabel.BorderSides = open ? ToolStripStatusLabelBorderSides.Right : ToolStripStatusLabelBorderSides.None;
                selectionSizeLabel.BorderSides = open ? ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right : ToolStripStatusLabelBorderSides.None;
            });
            vm.ContentsChanged += (_, _) => ResetView();
            vm.SettingsApplied += (_, _) =>
            {
                GetDefaultIcons(); //This is needed so that icons update immediately, rather than after a restart of the app
                ResetView();
            };
            vm.CloseRequested += (_, _) => Close();

            // Status bar
            vm.Observe(nameof(MainViewModel.PartitionSizeText), () => partitionSizeLabel.Text = vm.PartitionSizeText);
            vm.Observe(nameof(MainViewModel.FreeSpaceText), () => freeSpaceLabel.Text = vm.FreeSpaceText);
            vm.Observe(nameof(MainViewModel.FreeSpacePercentage), UpdateFreeSpaceBar);
            vm.Observe(nameof(MainViewModel.LocationText), () => locationLabel.Text = vm.LocationText);
            vm.Observe(nameof(MainViewModel.SelectionSizeText), () => selectionSizeLabel.Text = vm.SelectionSizeText);
        }

        private void UpdateFreeSpaceBar()
        {
            if (!viewModel.IsImageOpen)
                return;

            double freeSpacePercentage = viewModel.FreeSpacePercentage;
            if (freeSpacePercentage <= 10)
                freeSpaceProgressBar.ProgressBar.SetState(ProgressBarState.Error); // Set the progress bar colour to red.
            else if (freeSpacePercentage <= 20)
                freeSpaceProgressBar.ProgressBar.SetState(ProgressBarState.Paused); // Set the progress bar colour to yellow.
            else
                freeSpaceProgressBar.ProgressBar.SetState(ProgressBarState.Normal); // Set the progress bar colour to green.

            // Set progress bar value with a bit of a hack to disable the glow.
            freeSpaceProgressBar.Minimum = 100 - (int)freeSpacePercentage;
            freeSpaceProgressBar.Value = 100 - (int)freeSpacePercentage;
            freeSpaceProgressBar.Minimum = 0;
        }

        private void SyncPartitionComboBox()
        {
            updatingPartitionComboBox = true;
            try
            {
                partitionComboBox.Items.Clear();
                foreach (var option in viewModel.Partitions)
                    partitionComboBox.Items.Add(option);

                partitionComboBox.SelectedItem = viewModel.Partitions.FirstOrDefault(o => o.Index == viewModel.CurrentPartitionIndex);
            }
            finally
            {
                updatingPartitionComboBox = false;
            }
        }

        private void PopulateRecentList()
        {
            foreach (var oldItem in recentFilesToolStripMenuItem.DropDownItems.Cast<ToolStripItem>().ToList())
                oldItem.Dispose();

            var recentImages = viewModel.RecentImages;
            for (int i = 0; i < recentImages.Count; i++)
            {
                ToolStripMenuItem newItem = new() { Text = $"{i + 1}: {recentImages[i]}" };
                viewModel.OpenRecentImageCommand.BindCommand(recentImages[i], newItem);
                recentFilesToolStripMenuItem.DropDownItems.Add(newItem);
            }

            recentFilesToolStripMenuItem.Enabled = viewModel.HasRecentImages;
        }

        /// <summary>
        /// TEMPORARY (stage 5): tells the view model what the folder tree and the file list show
        /// and what is selected, for the enabled rules and the status bar.
        /// </summary>
        private void ReportBrowseState()
        {
            if (viewModel.Image is null || lstDirectories.Nodes.Count == 0)
            {
                viewModel.UpdateBrowseState(BrowseState.Empty);
                return;
            }

            var node = lstDirectories.SelectedNode;
            var folder = node is null ? FolderSelection.None
                : node == lstDirectories.Nodes[0] ? FolderSelection.Root
                : FolderSelection.Subfolder;
            var entries = SelectedItems.ToList();

            viewModel.UpdateBrowseState(new BrowseState(
                focusedPane,
                folder,
                node?.Tag as TiDirectory,
                entries,
                IsParentEntrySelected: lstFiles.SelectedIndices.Count == 1 && lstFiles.SelectedIndices[0] < IndexShift,
                // Pending synthetic items (not yet on disk) can't be extracted, deleted, renamed or inspected
                IsSelectedEntryPending: entries.Count == 1 && entries[0] is PendingFile or PendingDirectory,
                HasSubfolders: lstDirectories.Nodes[0].Nodes.Count > 0));
        }

        #endregion

        #region Event Handlers
        private async void MainForm_Load(object sender, EventArgs e)
        {
            //This fixes the problem of certain settings values not being returned correctly when using high DPI. Go figure...
            settings.Reload();
            uiState.Reload();

            BindState();
            viewModel.LoadSettings();
            SyncWindowState();

            GetDefaultIcons();
            lstDirectories.SelectedImageIndex = imgFilesSmall.Images.IndexOfKey("folder");

            for (var i = 1; i < lstFiles.Columns.Count; i++)
                upOneFolderListViewItem.SubItems.Add(string.Empty);

            //Open the file that was dragged onto the exe/shortcut or passed as a command line argument
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string argPath = args[1];
                await viewModel.OpenImageFileAsync(argPath);
            }
        }

        //Creates a new folder
        async Task IMainViewBridge.CreateFolderAsync()
        {
            var newName = await entryDialogs.AskNewFolderNameAsync();
            if (newName is null) return;
            if (viewModel.Image is not { } image || lstDirectories.SelectedNode is null) return;

            var nodeTag = lstDirectories.SelectedNode.Tag;
            if (nodeTag is FatDirectory fatDir)
            {
                fatDir.EnqueueCreateSubdirectory(newName);
            }
            else if (nodeTag is PendingDirectory pendingDir)
            {
                // Build path components from the pending directory's FullName + new name
                var path = FullNameToPathComponents(pendingDir.FullName)
                    .Append(newName).ToArray();
                image.PendingChanges.Add(new Changes.CreateDirectoryChange(path));
            }
        }

        //Deletes a file or folder
        //TODO: Implement deletion here and in the FS/container
        Task IMainViewBridge.DeleteAsync()
        {
            if (lstFiles.Focused && SelectedItems.Any())
            {
                var selectedSize = 0ul;
                var fileCount = 0;
                var dirCount = 0;

                foreach (var entry in SelectedItems)
                {
                    if (entry is TiDirectory)
                    {
                        dirCount++;
                        selectedSize += ((TiDirectory)entry).GetSize(true, false); //For directories, calculate the total size of everything inside
                    }
                    else
                    {
                        fileCount++;
                        selectedSize += entry.Length;
                    }
                }

                //Build a fancy string for showing the item count
                string itemCount = "";
                if (dirCount > 0)
                {
                    itemCount += $"{dirCount} director{(dirCount == 1 ? "y" : "ies")}";
                    if (fileCount > 0)
                        itemCount += $" and {fileCount} file{(fileCount == 1 ? "" : "s")}";
                }
                else
                {
                    itemCount += $"{fileCount} file{(fileCount == 1 ? "" : "s")}";
                }

                if (settings.Current.ConfirmDeletion)
                {
                    TaskDialogPage page = new()
                    {
                        Text = $"Are you sure you want to delete {itemCount} occupying {settings.Current.SizeUnit.FormatSize(selectedSize)}?{Environment.NewLine}" +
                        $"You might still be able to undo this operation later.",
                        Heading = $"{SelectedItems.Count()} item{(SelectedItems.Count() > 1 ? "s" : "")} will be deleted",
                        Caption = "Deletion",
                        Buttons =
                        {
                            TaskDialogButton.Yes,
                            TaskDialogButton.No
                        },
                        DefaultButton = TaskDialogButton.Yes,
                        SizeToContent = true,
                        Icon = TaskDialogIcon.Warning,
                        Verification = new TaskDialogVerificationCheckBox()
                        {
                            Text = "Do not ask for confirmation again"
                        }
                    };

                    //This prevents the enter keypress from being passed onto the listview and triggering file extraction and opening...
                    ActiveControl = null;

                    TaskDialogButton result = TaskDialog.ShowDialog(this, page);

                    if (page.Verification.Checked)
                        settings.Current.ConfirmDeletion = false;

                    if (result == TaskDialogButton.No)
                        return Task.CompletedTask;
                }

                foreach (var entry in SelectedItems)
                {
                    if (entry is FatFile fatFile)
                        fatFile.EnqueueDelete();
                    else if (entry is FatDirectory fatDir)
                        fatDir.EnqueueDelete();
                }
            }
            else if (lstDirectories.Focused && ((TiDirectory)lstDirectories.SelectedNode.Tag).Parent is not null)
            {
                if (settings.Current.ConfirmDeletion)
                {
                    TaskDialogPage page = new()
                    {
                        Text = $"Are you sure you want to delete this directory and all its contents?{Environment.NewLine}" +
                        $"You might still be able to undo this operation later.",
                        Heading = $"Directory will be deleted",
                        Caption = "Warning",
                        Buttons =
                        {
                            TaskDialogButton.Yes,
                            TaskDialogButton.No
                        },
                        Icon = TaskDialogIcon.Warning,
                        Verification = new TaskDialogVerificationCheckBox()
                        {
                            Text = "Do not ask for confirmation again"
                        }
                    };

                    //This prevents the enter keypress from being passed onto the treeview and causing unexpected actions...
                    ActiveControl = null;

                    TaskDialogButton result = TaskDialog.ShowDialog(this, page);

                    if (page.Verification.Checked)
                        settings.Current.ConfirmDeletion = false;

                    if (result == TaskDialogButton.No)
                        return Task.CompletedTask;
                }

                if (lstDirectories.SelectedNode?.Tag is FatDirectory selectedFatDir)
                    selectedFatDir.EnqueueDelete();
            }

            return Task.CompletedTask;
        }

        //Renames a file or folder
        void IMainViewBridge.Rename()
        {
            if (lstFiles.Focused && lstFiles.SelectedIndices.Count == 1)
            {
                // Trigger inline label edit in the file list
                int idx = lstFiles.SelectedIndices[0];
                if (idx >= IndexShift)
                    currentFolderView[idx - IndexShift].BeginEdit();
            }
            else if (lstDirectories.Focused && lstDirectories.SelectedNode != null
                     && lstDirectories.SelectedNode != lstDirectories.Nodes[0])
            {
                lstDirectories.SelectedNode.BeginEdit();
            }
        }

        //Extracts file(s) or folder(s) from the image to the specified path.
        async Task IMainViewBridge.ExtractAsync()
        {
            /* We need to check the treeview and listview focus so we know which items to extract.
             * Some of this might be a bit of a hack, but it works for now... */

            if (lstFiles.Focused)
            {
                if (!SelectedItems.Any())
                    lstFiles.SelectAllItems();
            }
            else if (lstDirectories.Focused)
            {
                if (((TiDirectory)lstDirectories.SelectedNode.Tag).Parent is null) //Root dir is selected, so we have to handle this separately
                {
                    lstFiles.Focus();
                    lstFiles.SelectAllItems();
                }
            }

            if (settings.Current.ExtractAlwaysAsk)
            {
                var options = await entryDialogs.AskExtractOptionsAsync(lstDirectories.Focused ? 1 : SelectedItems.Count());
                if (options is not null)
                {
                    settings.Current.DefaultExtractPath = options.TargetPath;

                    if (lstFiles.Focused)
                    {
                        FileExtraction.ExtractFiles(this, settings.Current, SelectedItems, options.TargetPath, options.DirectoryExtractionMode, options.OpenFolder);
                    }
                    else if (lstDirectories.Focused)
                    {
                        FileExtraction.ExtractFiles(this, settings.Current, new[] { (TiFileSystemObject)lstDirectories.SelectedNode.Tag }, options.TargetPath, options.DirectoryExtractionMode, options.OpenFolder);
                    }
                }
            }
            else
            {
                if (lstFiles.Focused)
                {
                    FileExtraction.ExtractFiles(this, settings.Current, SelectedItems);
                }
                else if (lstDirectories.Focused)
                {
                    FileExtraction.ExtractFiles(this, settings.Current, new[] { (TiFileSystemObject)lstDirectories.SelectedNode.Tag });
                }
            }
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e) // This method will be used more than once, thus it is separated from the main event.
        {
            if (_updatingSelection)
                return;

            //If the ".." item is selected alongside other items, deselect it immediately
            if (lstFiles.SelectedIndices.Count > 1 && lstFiles.SelectedIndices.Contains(0) && IndexShift > 0)
            {
                _updatingSelection = true;
                lstFiles.SelectedIndices.Remove(0);
                _updatingSelection = false;
            }

            ReportBrowseState();
        }

        private void cmsFileList_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (viewModel.Image is null || (lstFiles.SelectedIndices.Count == 1 && lstFiles.SelectedIndices[0] < IndexShift))
            {
                e.Cancel = true;
                return;
            }

            // The menu acts on the file list, so its commands follow the file list's rules.
            focusedPane = BrowsePane.Files;
            ReportBrowseState();
        }

        //TODO: Implement the Properties dialog for multiple selected objects like Windows does it
        async Task IMainViewBridge.ShowPropertiesAsync()
        {
            List<TiFileSystemObject> entries = new();
            if (lstDirectories.Focused)
            {
                if (((TiDirectory)lstDirectories.SelectedNode.Tag).Parent is null) //Can't show properties for the root node at this time
                {
                    entries.Add((TiFileSystemObject)lstDirectories.SelectedNode.Tag);
                }
            }
            else if (lstFiles.Focused)
            {
                for (int i = 0; i < lstFiles.SelectedIndices.Count; i++)
                {
                    if (lstFiles.SelectedIndices[i] >= IndexShift)
                    {
                        entries.Add(GetSelectedItemData(i));
                    }
                }
            }
            else
            {
                if (lstFiles.SelectedIndices.Count > 0)
                {
                    for (int i = 0; i < lstFiles.SelectedIndices.Count; i++)
                    {
                        if (lstFiles.SelectedIndices[i] >= IndexShift)
                        {
                            entries.Add(GetSelectedItemData(i));
                        }
                    }
                }
                else if (lstDirectories.SelectedNode != null && ((TiDirectory)lstDirectories.SelectedNode.Tag).Parent is not null)
                    entries.Add((TiFileSystemObject)lstDirectories.SelectedNode.Tag);
                else
                    return; //Can't show properties for whatever is selected, so let's just return
            }

            if (entries.Count == 0)
            {
                return;
            }

            // TODO: Apply the changes. The dialog's answer has never been applied so far.
            await entryDialogs.AskPropertiesAsync(entries);
        }

        async Task IMainViewBridge.InjectFilesAsync()
        {
            //TODO: Get the count and total size of seleted items to inject before showing the dialog
            var fileNames = await filePicker.PickFilesToAddAsync();
            if (fileNames.Count > 0)
            {
                int fileCount = fileNames.Count;
                ulong totalSize = 0;
                foreach (string path in fileNames)
                {
                    try { totalSize += (ulong)new FileInfo(path).Length; }
                    catch { /* best effort */ }
                }

                if (settings.Current.ConfirmInjection)
                {
                    TaskDialogPage page = new()
                    {
                        Text = $"Are you sure you want to inject {fileCount} file{(fileCount == 1 ? "" : "s")} occupying {settings.Current.SizeUnit.FormatSize(totalSize)} into the image?",
                        Heading = $"{fileCount} file{(fileCount == 1 ? "" : "s")} will be injected",
                        Caption = "Injection",
                        Buttons =
                        {
                            TaskDialogButton.Yes,
                            TaskDialogButton.No
                        },
                        Icon = TaskDialogIcon.Warning,
                        Verification = new TaskDialogVerificationCheckBox()
                        {
                            Text = "Do not ask for confirmation again"
                        }
                    };
                    TaskDialogButton result = TaskDialog.ShowDialog(this, page);

                    if (page.Verification.Checked)
                        settings.Current.ConfirmInjection = false;

                    if (result == TaskDialogButton.No)
                        return;
                }

                if (lstDirectories.SelectedNode?.Tag is FatDirectory targetFatDir)
                {
                    foreach (string hostPath in fileNames)
                    {
                        using var stream = new FileStream(hostPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        var source = FileDataSource.Create(stream, tempDirectory.Root, settings.Current.MemoryMappingThreshold);
                        var now = DateTime.Now;
                        var attrs = FatAttributes.Archive;
                        targetFatDir.EnqueueAddFile(Path.GetFileName(hostPath), source, attrs, now, now, now);
                    }
                }
                else if (lstDirectories.SelectedNode?.Tag is PendingDirectory pendingDir && viewModel.Image is { } image)
                {
                    var dirComponents = FullNameToPathComponents(pendingDir.FullName);
                    foreach (string hostPath in fileNames)
                    {
                        using var stream = new FileStream(hostPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        var source = FileDataSource.Create(stream, tempDirectory.Root, settings.Current.MemoryMappingThreshold);
                        var now = DateTime.Now;
                        var destPath = dirComponents.Append(Path.GetFileName(hostPath)).ToArray();
                        image.PendingChanges.Add(new Changes.AddFileChange(
                            destPath, source, FatAttributes.Archive, now, now, now));
                    }
                }
            }
        }

        private void lstFiles_ColumnClick(object sender, ColumnClickEventArgs e)
            => viewModel.SortByCommand.Execute(e.Column);

        //TODO: Move that file count stuff elsewhere and just call it to get the number.
        private void lstDirectories_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node is null)
                return;

            var selectedDir = (TiDirectory)e.Node.Tag;
            selectedDir = (TiDirectory)selectedDir.ResolveTarget();

            //This makes sure the selected image doesn't change when a hidden folder is selected
            if (((TiFileSystemObject)e.Node.Tag).Attributes.HasFlag(FileAttributes.Hidden))
            {
                lstDirectories.SelectedImageKey = "folder (Hidden)";
            }
            else
            {
                lstDirectories.SelectedImageKey = "folder";
            }

            PopulateListView(selectedDir);
            ReportBrowseState();
        }

        private void lstFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstFiles.SelectedIndices.Count == 1 && e.Button != MouseButtons.Right)
            {
                if (GetSelectedItemData(0) is TiDirectory dir) //A folder was double-clicked
                {
                    var targetDir = (TiDirectory)dir.ResolveTarget();
                    BrowseToDirectory(targetDir);
                    var node = FindNode(lstDirectories.Nodes[0], targetDir);
                    if (node is not null)
                    {
                        lstDirectories.SelectedNode = node;
                    }
                }
                else //A file was double-clicked - extract to temp dir then open it
                {
                    tempDir = tempDirectory.GetUniqueSubdirectoryPath();
                    var selected = SelectedItems.ToArray();
                    string targetFile = Path.Combine(tempDir, selected.First().Name);

                    FileExtraction.ExtractFilesToTemporaryDirectory(this, settings.Current, selected, DirectoryExtractionMode.Skip);

                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = targetFile,
                            UseShellExecute = true,
                            ErrorDialog = true
                        });
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        // Throw if Process.Start fails with anything other than ERROR_CANCELLED
                        if (ex.NativeErrorCode != 0x000004C7)
                            throw;
                    }
                }
            }
        }

        private void cmsDirTree_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lstDirectories.Nodes.Count == 0 || lstDirectories.SelectedNode is null)
            {
                e.Cancel = true;
                return;
            }

            // The menu acts on the folder tree, so its commands follow the folder tree's rules.
            focusedPane = BrowsePane.Folders;
            ReportBrowseState();
        }

        void IMainViewBridge.SelectAll()
        {
            lstFiles.Focus();
            lstFiles.SelectAllItems();
        }

        private void list_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data is not null && e.Data.GetDataPresent(DataFormats.FileDrop) && !viewModel.IsImageOpen)
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /* Drag and drop was performed on the ListView - a file was dragged into the ListView/TreeView from Explorer => try to open it
         * Right now, we only handle case a for opening a single file that was dragged into the window.
         * TODO: Implement other drag and drop scenarios (moving files within the image, etc.). */
        private async void list_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data is not null && e.Data.GetDataPresent(DataFormats.FileDrop) && !viewModel.IsImageOpen)
            {
                //Files are being dragged into the listview from outside the form
                var data = e.Data.GetData(DataFormats.FileDrop, false);
                if (data is not null)
                {
                    string[] items = (string[])data;
                    if (items.Length == 1)
                    {
                        await viewModel.OpenImageFileAsync(items[0]);
                    }
                }
            }
        }

        void IMainViewBridge.ExpandFolderTree()
        {
            lstDirectories.ExpandAll();
        }

        void IMainViewBridge.CollapseFolderTree()
        {
            lstDirectories.CollapseAll();

            //CollapseAll() clears SelectedNode, but merely setting that again won't trigger the selection events for some reason
            //(.NET/WinForms bug?). Hence we also reset the view to make sure TreeView and ListView don't fall out of sync.
            lstDirectories.SelectedNode = lstDirectories.Nodes[0];
            ResetView();
        }

        private void lstDirectories_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode newNode = lstDirectories.GetNodeAt(e.X, e.Y);

                //This prevents opening the menu on empty area of the TreeView
                if (newNode is not null)
                {
                    lstDirectories.SelectedNode = newNode;
                    cmsDirTree.Show(lstDirectories, e.Location);
                }
                else
                {
                    return;
                }
            }
        }

        /* Fires when the user starts dragging a ListViewItem around. String array is needed for Explorer to perform the move operation once
         * the drop is performed. */
        private void lstFiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Item is not null)
            {
                tempDir = tempDirectory.GetUniqueSubdirectoryPath();
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                if (((ListViewItem)e.Item).Text == "..")
                {
                    return;
                }

                List<string> items = new();
                foreach (TiFileSystemObject fso in SelectedItems)
                {
                    string item = Path.Combine(tempDir, fso.Name);
                    items.Add(item);
                }
                StringCollection draggedItems = new();
                draggedItems.AddRange(items.ToArray());

                DataObject data = new();
                data.SetFileDropList(draggedItems); //Needed for Explorer
                /* Set the preferred drop effect to Copy to prevent Explorer from defaulting to "Create a link" for filenames
                 * that are registered under SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths. */
                data.SetData("Preferred DropEffect", new MemoryStream(BitConverter.GetBytes((int)DragDropEffects.Copy)));
                lstFiles.DoDragDrop(data, DragDropEffects.Copy);
            }
        }

        /* Fires when the user starts dragging a TreeNode around. String array is needed for Explorer to perform the move operation once
         * the drop is performed. */
        private void lstDirectories_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Item is not null)
            {
                tempDir = tempDirectory.GetUniqueSubdirectoryPath();
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                //This array is needed for Explorer to perform the file copy/move operation later on.
                List<string> items = new();
                draggedDir = (TiDirectory)((TreeNode)e.Item).Tag;
                if (draggedDir.Parent is null)
                {
                    /* Add the root dir contents (non-recursively) to the list instead of the tempdir itself, so Explorer doesn't end up moving it
                     * instead of the contents. */
                    foreach (var fso in draggedDir.EnumerateFileSystemObjects(settings.Current.ShowHiddenItems))
                    {
                        items.Add(Path.Combine(tempDir, fso.Name));
                    }
                }
                else
                {
                    items.Add(Path.Combine(tempDir, draggedDir.Name));
                }
                StringCollection draggedItems = new();
                draggedItems.AddRange(items.ToArray());

                DataObject data = new();
                data.SetFileDropList(draggedItems); //FileDrop is needed for Explorer
                /* Set the preferred drop effect to Copy to prevent Explorer from defaulting to "Create a link" for filenames
                 * that are registered under SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths. */
                data.SetData("Preferred DropEffect", new MemoryStream(BitConverter.GetBytes((int)DragDropEffects.Copy)));
                lstDirectories.DoDragDrop(data, DragDropEffects.Copy);
            }
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!viewModel.IsCloseConfirmed && viewModel.HasUnsavedChanges)
            {
                // Ask first, and close again once the answer allows it.
                e.Cancel = true;
                if (await viewModel.ConfirmCloseWindowAsync())
                    BeginInvoke(Close);

                return;
            }

            settings.Save();
            SetUIState();
            uiState.Save();

            //Try to clear the temp folder if this is the only instance
            if (Process.GetProcessesByName("TotalImage").Length == 1)
            {
                try
                {
                    tempDirectory.Clear();
                }
                catch (Exception)
                {
                    //Ignore the exception, since the program is closing anyway... The folder can be cleared another day.
                }
            }
        }

        private void PartitionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* Note that we can't simply use the ComboBox.SelectedIndex, as unsupported partitions aren't added to the ComboBox and thus item
             * indices are shifted compared to actual partition indices. */
            if (!updatingPartitionComboBox && partitionComboBox.SelectedItem is PartitionOption option)
                viewModel.SelectPartition(option.Index);
        }

        private void lstFiles_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && lstDirectories.SelectedNode.Parent is not null)
            {
                lstDirectories.SelectedNode = lstDirectories.SelectedNode.Parent;
            }
            else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                if (lstFiles.SelectedIndices.Count == 1)
                {
                    TiFileSystemObject? fso = GetSelectedItemData(0);
                    if (fso is TiDirectory dir)
                    {
                        var node = FindNode(lstDirectories.Nodes[0], dir);
                        if (node is not null)
                        {
                            lstDirectories.SelectedNode = node;
                        }
                        else
                        {
                            throw new Exception("Associated treeview node was not found");
                        }
                    }
                    else if (fso is PendingFile)
                    {
                        // Pending files are not yet on disk — cannot open
                    }
                    else
                    {
                        tempDir = tempDirectory.GetUniqueSubdirectoryPath();
                        string targetFile = Path.Combine(tempDir, SelectedItems.First().Name);

                        FileExtraction.ExtractFilesToTemporaryDirectory(this, settings.Current, SelectedItems, DirectoryExtractionMode.Skip);

                        ProcessStartInfo psi = new()
                        {
                            FileName = targetFile,
                            UseShellExecute = true
                        };
                        Process.Start(psi);
                    }
                }
            }
        }

        private void lstFiles_KeyPress(object sender, KeyPressEventArgs e)
        {
            //This selects the first listviewitem that has text starting with the character pressed, then the next one, etc.

            char ch = char.ToLowerInvariant(e.KeyChar);
            if (!char.IsLetterOrDigit(ch) && ch != '_' && ch != '-' && ch != '.')
            {
                return;
            }

            int startIndex = 0;
            if (ch == _lastTypeAheadChar && _lastTypeAheadIndex >= 0)
            {
                startIndex = _lastTypeAheadIndex + 1;
            }
            else
            {
                _lastTypeAheadIndex = -1;
            }

            //Search from startIndex, wrapping around once if needed
            int count = currentFolderView.Count;
            for (int i = 0; i < count; i++)
            {
                int idx = (startIndex + i) % count;
                if (currentFolderView[idx].Text.StartsWith(ch.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    int lvIndex = idx + IndexShift;
                    lstFiles.SelectedIndices.Clear();
                    lstFiles.FocusedItem = currentFolderView[idx];
                    currentFolderView[idx].Selected = true;
                    lstFiles.EnsureVisible(lvIndex);
                    _lastTypeAheadChar = ch;
                    _lastTypeAheadIndex = idx;
                    e.Handled = true;
                    return;
                }
            }
        }

        private void lstFiles_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex < IndexShift)
                e.Item = upOneFolderListViewItem;
            else
                e.Item = currentFolderView[e.ItemIndex - IndexShift];
        }

        private void lstFiles_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {

        }

        private void lstFiles_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {

        }

        private void lstFiles_Enter(object sender, EventArgs e)
        {
            focusedPane = BrowsePane.Files;
            ReportBrowseState();
        }

        private void lstDirectories_Enter(object sender, EventArgs e)
        {
            focusedPane = BrowsePane.Folders;
            ReportBrowseState();
        }

        //After an item's label (=Text property) is changed - for renaming objects
        //From here the name change should propagate to the associated FileSystemObject and to the stream
        private void lstFiles_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label is null)
                return; // user cancelled

            int idx = e.Item + IndexShift;
            if (idx < IndexShift || idx - IndexShift >= currentFolderView.Count)
            {
                e.CancelEdit = true;
                return;
            }

            var fso = currentFolderView[e.Item].Tag as TiFileSystemObject;
            if (fso is FatFile fatFile)
                fatFile.EnqueueRename(e.Label);
            else if (fso is FatDirectory fatDir)
                fatDir.EnqueueRename(e.Label);
            else
                e.CancelEdit = true;
        }

        //Before an item's label (=Text property) will be changed - for renaming objects.
        //Here we should probably perform some sanity checks
        private void lstFiles_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (viewModel.Image?.SupportsWriting == false)
            {
                e.CancelEdit = true;
            }
        }

        //After a node's label (=Text property) is changed - for renaming objects
        //From here the name change should propagate to the associated FileSystemObject and to the stream
        private void lstDirectories_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label is null)
                return; // user cancelled

            if (e.Node?.Tag is FatDirectory fatDir)
                fatDir.EnqueueRename(e.Label);
            else
                e.CancelEdit = true;
        }

        //Before a node's label (=Text property) will be changed - for renaming objects.
        //Here we should probably perform some sanity checks
        private void lstDirectories_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (viewModel.Image?.SupportsWriting == false)
            {
                e.CancelEdit = true;
            }
        }

        private void list_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            //ESC cancels the drag and drop action
            if (e.EscapePressed)
            {
                e.Action = DragAction.Cancel;
                Directory.Delete(tempDir, true);
                return;
            }

            /* If the left mouse button was released, complete the drag and drop operation. In our case, this means that any files or folders
             * that may have been dragged out of the window have to be extracted into a temporary folder first, then the drop is performed so
             * Explorer will move these items to the actual drag and drop destination. Dropping inside the window is not supported yet. */
            if ((e.KeyState & 1) == 0)
            {
                if (!ClientRectangle.Contains(PointToClient(MousePosition)))
                {
                    Capture = true;
                    var extractionSucceeded = false;

                    //Here is where the actual extraction to the temp dir happens
                    if (sender is ListView)
                    {
                        extractionSucceeded = FileExtraction.ExtractFilesToTemporaryDirectory(this, settings.Current, SelectedItems, DirectoryExtractionMode.Preserve);
                    }
                    else if (sender is TreeView && draggedDir is not null)
                    {
                        if (draggedDir.Parent is null) //Root dir needs to be treated separately
                            extractionSucceeded = FileExtraction.ExtractFilesToTemporaryDirectory(this, settings.Current, draggedDir.EnumerateFileSystemObjects(settings.Current.ShowHiddenItems), DirectoryExtractionMode.Preserve);
                        else
                            extractionSucceeded = FileExtraction.ExtractFilesToTemporaryDirectory(this, settings.Current, new TiFileSystemObject[] { draggedDir }, DirectoryExtractionMode.Preserve);
                    }

                    //User cancelled extraction via the dialog, so the drop needs to be cancelled too or Explorer will try to move items that don't exist
                    if (extractionSucceeded)
                        e.Action = DragAction.Drop;
                    else
                    {
                        e.Action = DragAction.Cancel;
                        Directory.Delete(tempDir, true);
                    }
                }
                else
                    e.Action = DragAction.Cancel;

                return;
            }
            //Left mouse button wasn't released yes, continue the drag
            else
                e.Action = DragAction.Continue;
        }

        void IMainViewBridge.GoToParentFolder()
        {
            if (lstDirectories.SelectedNode?.Parent is not null)
            {
                lstDirectories.SelectedNode = lstDirectories.SelectedNode.Parent;
            }
        }

        private void BrowseToDirectory(TiDirectory dir)
        {
            var node = FindNode(lstDirectories.Nodes[0], dir);
            if (node is not null)
            {
                lstDirectories.SelectedNode = node;
                return;
            }

            PopulateListView(dir);
            ReportBrowseState();
        }
        #endregion

        private int IndexShift => lstFiles.VirtualListSize - currentFolderView.Count;

        private IEnumerable<TiFileSystemObject> SelectedItems
            => from x in lstFiles.SelectedIndices.Cast<int>()
               where x >= IndexShift && currentFolderView[x - IndexShift].Tag is TiFileSystemObject
               select (TiFileSystemObject)currentFolderView[x - IndexShift].Tag;

        /// <summary>
        /// Classifies the pending-change state of a file-system entry in the UI.
        /// </summary>
        private enum PendingItemState { None, Added, Deleted, Renamed }

        /// <summary>
        /// Walks the directory's parent chain to build a path-component array matching the
        /// convention used by <see cref="TotalImage.Changes.PendingChange"/> path fields.
        /// The root directory returns an empty array.
        /// For <see cref="PendingDirectory"/> (which has no live parent chain), the FullName
        /// is split instead.
        /// </summary>
        private static string[] GetDirectoryPathComponents(TiDirectory dir)
        {
            if (dir is PendingDirectory)
                return FullNameToPathComponents(dir.FullName);

            var parts = new List<string>();
            var current = dir;
            while (current.Parent is not null)
            {
                parts.Add(current.Name);
                current = current.Parent;
            }
            parts.Reverse();
            return parts.ToArray();
        }

        /// <summary>
        /// Splits a FullName like "\SUB\CHILD" into ["SUB", "CHILD"].
        /// The root ("\") returns an empty array.
        /// </summary>
        private static string[] FullNameToPathComponents(string fullName)
            => fullName.Split(System.IO.Path.DirectorySeparatorChar,
                              StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// Returns the pending-change state of a named entry (file or directory) that lives
        /// inside <paramref name="dirPath"/>. The last-wins rule is applied so that e.g. a
        /// renamed-then-deleted entry resolves to <see cref="PendingItemState.Deleted"/>.
        /// </summary>
        private PendingItemState GetPendingState(string name, string[] dirPath)
        {
            if (viewModel.Image is not { } image) return PendingItemState.None;

            var state = PendingItemState.None;

            foreach (var change in image.PendingChanges.Changes)
            {
                switch (change)
                {
                    case AddFileChange add:
                    {
                        if (add.DestinationPath.Length == 0) break;
                        var addDir = add.DestinationPath[..^1];
                        var addName = add.DestinationPath[^1];
                        if (PathsEqualOrdinalIgnoreCase(addDir, dirPath) && NamesEqual(addName, name))
                            state = PendingItemState.Added;
                        break;
                    }
                    case CreateDirectoryChange create:
                    {
                        if (create.Path.Length == 0) break;
                        var createDir = create.Path[..^1];
                        var createName = create.Path[^1];
                        if (PathsEqualOrdinalIgnoreCase(createDir, dirPath) && NamesEqual(createName, name))
                            state = PendingItemState.Added;
                        break;
                    }
                    case DeleteEntryChange delete:
                    {
                        if (delete.Path.Length == 0) break;
                        var deleteDir = delete.Path[..^1];
                        var deleteName = delete.Path[^1];
                        if (PathsEqualOrdinalIgnoreCase(deleteDir, dirPath) && NamesEqual(deleteName, name))
                            state = PendingItemState.Deleted;
                        break;
                    }
                    case RenameChange rename:
                    {
                        if (rename.OldPath.Length == 0) break;
                        var renameDir = rename.OldPath[..^1];
                        var renameName = rename.OldPath[^1];
                        if (PathsEqualOrdinalIgnoreCase(renameDir, dirPath) && NamesEqual(renameName, name))
                            state = PendingItemState.Renamed;
                        break;
                    }
                }
            }

            return state;

            static bool NamesEqual(string a, string b)
                => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

        private static bool PathsEqualOrdinalIgnoreCase(string[] a, string[] b)
        {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
                if (!string.Equals(a[i], b[i], StringComparison.OrdinalIgnoreCase)) return false;
            return true;
        }

        //Sets the CurrentUIState to what the current state of UI is
        private void SetUIState()
        {
            uiState.Current.SplitterDistance = splitContainer.SplitterDistance;

            if (WindowState != FormWindowState.Minimized)
            {
                uiState.Current.WindowPosition = Location;
                uiState.Current.WindowState = WindowState;
                uiState.Current.WindowSize = Size;
            }

            foreach (ColumnHeader col in lstFiles.Columns)
            {
                uiState.Current.MWColumnOrder[col.Index] = col.DisplayIndex;
                uiState.Current.MWColumnWidth[col.Index] = col.Width;
            }
        }

        // Used for events that require the current folder view to be updated (e.g. show hidden items toggled, etc.)
        private void ResetView()
        {
            if (viewModel.CurrentFileSystem is not { } fileSystem)
                return;

            lastViewedDir = lstDirectories.SelectedNode?.Tag as TiDirectory;

            var root = new TreeNode(@"\");
            root.ImageIndex = _smallFolderIndex;
            root.SelectedImageIndex = _smallFolderIndex;
            root.Tag = fileSystem.RootDirectory;

            lstDirectories.BeginUpdate();
            PopulateTreeView(root, fileSystem.RootDirectory);

            lstDirectories.Nodes.Clear();
            lstDirectories.Nodes.Add(root);
            lstDirectories.Sort();
            lstDirectories.EndUpdate();

            if (lastViewedDir is not null)
            {
                TreeNode? node = FindNode(lstDirectories.Nodes[0], lastViewedDir);
                if (node is null)
                    lstDirectories.SelectedNode = lstDirectories.Nodes[0];
                else
                    lstDirectories.SelectedNode = node;
            }
            else
            {
                lstDirectories.SelectedNode = lstDirectories.Nodes[0];
            }

            PopulateListView((TiDirectory)lstDirectories.SelectedNode.Tag);
            ReportBrowseState();
        }

        private void PopulateTreeView(TreeNode node, TiDirectory dir)
        {
            dir = (TiDirectory)dir.ResolveTarget();
            var dirPathComponents = GetDirectoryPathComponents(dir);

            foreach (var subdir in dir.EnumerateDirectories(settings.Current.ShowHiddenItems))
            {
                var subnode = new TreeNode(subdir.Name);

                //Hidden folders have a 50% opacity for the icon
                if (subdir.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    subnode.ImageIndex = _smallFolderHiddenIndex;
                    subnode.ForeColor = Color.Gray;
                }
                else
                {
                    subnode.ImageIndex = _smallFolderIndex;
                }

                //Highlight pending changes on directory nodes
                switch (GetPendingState(subdir.Name, dirPathComponents))
                {
                    case PendingItemState.Added:
                        subnode.BackColor = Color.FromArgb(198, 239, 206);
                        subnode.ForeColor = Color.FromArgb(0, 97, 0);
                        break;
                    case PendingItemState.Deleted:
                        subnode.BackColor = Color.FromArgb(255, 199, 206);
                        subnode.ForeColor = Color.FromArgb(156, 0, 6);
                        break;
                    case PendingItemState.Renamed:
                        subnode.BackColor = Color.FromArgb(255, 235, 156);
                        subnode.ForeColor = Color.FromArgb(156, 87, 0);
                        break;
                }
                subnode.Tag = subdir;
                node.Nodes.Add(subnode);

                PopulateTreeView(subnode, subdir);
            }

            // Synthesise tree nodes for pending CreateDirectoryChange entries in this directory
            if (viewModel.Image is { } image)
            {
                foreach (var change in image.PendingChanges.Changes)
                {
                    if (change is not CreateDirectoryChange create || create.Path.Length == 0) continue;

                    // Parent path components must match this directory
                    var pendingParentComponents = create.Path[..^1];
                    if (!PathsEqualOrdinalIgnoreCase(pendingParentComponents, dirPathComponents)) continue;

                    var pendingName = create.Path[^1];

                    // Skip if a real subdir with this name is already shown
                    if (node.Nodes.Cast<TreeNode>().Any(n =>
                            string.Equals(((TiDirectory)n.Tag).Name, pendingName,
                                StringComparison.OrdinalIgnoreCase)))
                        continue;

                    var pendingDir = new PendingDirectory(pendingName, dir.FullName);
                    var pendingNode = new TreeNode(pendingName)
                    {
                        ImageIndex = imgFilesSmall.Images.IndexOfKey("folder"),
                        BackColor = Color.FromArgb(198, 239, 206),
                        ForeColor = Color.FromArgb(0, 97, 0),
                        Tag = pendingDir
                    };
                    node.Nodes.Add(pendingNode);
                    // Pending dirs start empty — no recursive PopulateTreeView needed
                }
            }
        }

        private static ImageAttributes CreateHiddenIconAttributes()
        {
            var attrs = new ImageAttributes();
            attrs.SetColorMatrix(_hiddenIconColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return attrs;
        }

        private Bitmap CreateHiddenIcon(Bitmap normalIcon)
        {
            Point[] points = { new Point(0, 0),
                               new Point(normalIcon.Width, 0),
                               new Point(0, normalIcon.Height),
                             };
            Rectangle rect = new(0, 0, normalIcon.Width, normalIcon.Height);

            Bitmap bmp = new(normalIcon.Width, normalIcon.Height);
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(normalIcon, points, rect, GraphicsUnit.Pixel, _hiddenIconAttributes);
            }

            return bmp;
        }

        private void PopulateListView(TiDirectory dir)
        {
            dir = (TiDirectory)dir.ResolveTarget();
            lstFiles.BeginUpdate();
            lstFiles.SelectedIndices.Clear();
            currentFolderView.Clear();
            _lastTypeAheadChar = '\0';
            _lastTypeAheadIndex = -1;

            upOneFolderListViewItem.Tag = dir.Parent;

            var count = 0;
            if (dir.Parent is not null)
            {
                count++;
            }

            var dirPathComponents = GetDirectoryPathComponents(dir);

            foreach (var fso in dir.EnumerateFileSystemObjects(settings.Current.ShowHiddenItems))
            {
                var item = new ListViewItem();
                item.Text = fso.Name;
                item.SubItems.Add(GetFileTypeName(fso.Name, fso.Attributes));

                string size = string.Empty;
                if (fso is TiDirectory subdir)
                {
                    if (settings.Current.FileListShowDirSize)
                    {
                        size = settings.Current.SizeUnit.FormatSize(subdir.GetSize(true, false));
                        item.SubItems.Add(size);
                    }
                    else
                    {
                        item.SubItems.Add(string.Empty);
                    }
                }
                else
                {
                    size = settings.Current.SizeUnit.FormatSize(fso.Length);
                    item.SubItems.Add(size);
                }

                item.ImageIndex = GetFileTypeIconIndex(fso.Name, fso.Attributes);

                item.SubItems.Add(fso.LastWriteTime.ToString());
                item.SubItems.Add(FileAttributesToString(fso.Attributes));
                item.UseItemStyleForSubItems = false;
                item.SubItems[4].Font = _monoFont;

                //Do some simple styling for hidden items
                if (fso.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    item.ForeColor = Color.Gray;
                    item.SubItems[1].ForeColor = Color.Gray;
                    item.SubItems[2].ForeColor = Color.Gray;
                    item.SubItems[3].ForeColor = Color.Gray;
                    item.SubItems[4].ForeColor = Color.Gray;
                }

                //Highlight pending changes: green = added, red = pending delete, orange = renamed
                switch (GetPendingState(fso.Name, dirPathComponents))
                {
                    case PendingItemState.Added:
                        item.BackColor = Color.FromArgb(198, 239, 206);   // light green
                        item.ForeColor = Color.FromArgb(0, 97, 0);
                        break;
                    case PendingItemState.Deleted:
                        item.BackColor = Color.FromArgb(255, 199, 206);   // light red
                        item.ForeColor = Color.FromArgb(156, 0, 6);
                        break;
                    case PendingItemState.Renamed:
                        item.BackColor = Color.FromArgb(255, 235, 156);   // light orange
                        item.ForeColor = Color.FromArgb(156, 87, 0);
                        break;
                }

                item.ToolTipText = $"Type: {item.SubItems[1].Text}{Environment.NewLine}Size: {size}";

                if(!string.IsNullOrWhiteSpace(item.SubItems[3].Text))
                    item.ToolTipText += $"{Environment.NewLine}Modified: {item.SubItems[3].Text}";

                item.Tag = fso;
                currentFolderView.Add(item);
                count++;
            }

            // Synthesise list items for pending additions that don't yet exist on disk
            if (viewModel.Image is { } image)
            {
                foreach (var change in image.PendingChanges.Changes)
                {
                    string[]? pendingDir = null;
                    string? pendingName = null;
                    bool isDirectory = false;
                    string sizeText = string.Empty;
                    DateTime lastWrite = DateTime.Now;
                    ulong pendingLength = 0;

                    if (change is AddFileChange add && add.DestinationPath.Length > 0)
                    {
                        pendingDir = add.DestinationPath[..^1];
                        pendingName = add.DestinationPath[^1];
                        pendingLength = (ulong)add.Source.Length;
                        sizeText = settings.Current.SizeUnit.FormatSize(pendingLength);
                        lastWrite = add.LastWriteTime;
                    }
                    else if (change is CreateDirectoryChange create && create.Path.Length > 0)
                    {
                        pendingDir = create.Path[..^1];
                        pendingName = create.Path[^1];
                        isDirectory = true;
                    }

                    if (pendingDir is null || pendingName is null) continue;

                    // Only show in the current directory
                    if (!PathsEqualOrdinalIgnoreCase(pendingDir, dirPathComponents)) continue;

                    // Skip if the name is already shown as a real FSO (e.g. re-injecting after a save)
                    if (currentFolderView.Any(i => string.Equals(
                            (i.Tag as TiFileSystemObject)?.Name, pendingName,
                            StringComparison.OrdinalIgnoreCase)))
                        continue;

                    var attrs = isDirectory ? FileAttributes.Directory : FileAttributes.Normal;
                    var pendingItem = new ListViewItem();
                    pendingItem.Text = pendingName;
                    pendingItem.SubItems.Add(GetFileTypeName(pendingName, attrs));
                    pendingItem.SubItems.Add(sizeText);
                    pendingItem.ImageIndex = GetFileTypeIconIndex(pendingName, attrs);
                    pendingItem.SubItems.Add(lastWrite.ToString());
                    pendingItem.SubItems.Add(isDirectory ? "D----" : "-A---");
                    pendingItem.UseItemStyleForSubItems = false;
                    pendingItem.SubItems[4].Font = new(FontFamily.GenericMonospace, 9);
                    pendingItem.BackColor = Color.FromArgb(198, 239, 206);
                    pendingItem.ForeColor = Color.FromArgb(0, 97, 0);
                    pendingItem.ToolTipText = $"Type: {pendingItem.SubItems[1].Text}{Environment.NewLine}Size: {sizeText}{Environment.NewLine}Modified: {lastWrite}";
                    pendingItem.Tag = isDirectory
                        ? (TiFileSystemObject)new PendingDirectory(pendingName, dir.FullName)
                        : new PendingFile(pendingName, pendingLength, lastWrite);

                    currentFolderView.Add(pendingItem);
                    count++;
                }
            }

            SortListView();
            lstFiles.VirtualListSize = count;

            lstFiles.EndUpdate();
        }

        private string FileAttributesToString(FileAttributes attr)
        {
            var sb = new StringBuilder();
            bool isReparsePoint = attr.HasFlag(FileAttributes.ReparsePoint);

            if (attr.HasFlag(FileAttributes.Directory))
                sb.Append('D');
            else
                sb.Append('-');

            if (attr.HasFlag(FileAttributes.Archive))
                sb.Append('A');
            else
                sb.Append('-');

            if (attr.HasFlag(FileAttributes.System))
                sb.Append('S');
            else
                sb.Append('-');

            if (attr.HasFlag(FileAttributes.Hidden))
                sb.Append('H');
            else
                sb.Append('-');

            if (attr.HasFlag(FileAttributes.ReadOnly))
                sb.Append('R');
            else
                sb.Append('-');

            sb.Append(isReparsePoint ? 'L' : '-');

            return sb.ToString();
        }

        // Shows the partition the view model loaded, or nothing when no image is open.
        private void LoadPartition()
        {
            if (viewModel.CurrentFileSystem is not { } fileSystem)
            {
                lstDirectories.Nodes.Clear();
                currentFolderView.Clear();
                lstFiles.VirtualListSize = 0;
                ReportBrowseState();
                return;
            }

            var root = new TreeNode(@"\");
            root.ImageIndex = _smallFolderIndex;
            root.SelectedImageIndex = _smallFolderIndex;

            root.Tag = fileSystem.RootDirectory;
            PopulateTreeView(root, fileSystem.RootDirectory);

            lstDirectories.BeginUpdate();
            lstDirectories.Nodes.Clear();
            lstDirectories.Nodes.Add(root);
            lstDirectories.Sort();
            lstDirectories.EndUpdate();
            lstDirectories.SelectedNode = lstDirectories.Nodes[0];

            PopulateListView(fileSystem.RootDirectory);
            ReportBrowseState();
        }

        private string GetFileTypeName(string filename, FileAttributes attributes)
        {
            if (attributes.HasFlag(FileAttributes.ReparsePoint))
            {
                return attributes.HasFlag(FileAttributes.Directory) ? "Junction" : "Symbolic link";
            }

            var extension = attributes.HasFlag(FileAttributes.Directory) ? "folder" : Path.GetExtension(filename);

            if (settings.Current.QueryShellForFileTypeInfo)
            {
                return ShellInterop.GetFileTypeName(filename, attributes);
            }
            else
            {
                if (attributes.HasFlag(FileAttributes.Directory))
                    return "File folder";
                else if (extension.Length > 0)
                    return $"{extension[1..].ToUpper()} File";
                else
                    return "File";
            }
        }

        private int GetFileTypeIconIndex(string filename, FileAttributes attributes)
        {
            string key;

            if (settings.Current.QueryShellForFileTypeInfo)
            {
                var index = ShellInterop.GetFileTypeIconIndex(filename, attributes);
                key = index.ToString();

                if (!imgFilesSmall.Images.ContainsKey(key))
                {
                    (ImageList, Icon)[] icons =
                    {
                        (imgFilesSmall, ShellInterop.GetFileTypeIcon(index, false)),
                        (imgFilesLarge, ShellInterop.GetFileTypeIcon(index, true))
                    };

                    foreach (var (list, icon) in icons)
                    {
                        list.Images.Add(key, icon);
                        list.Images.Add($"{key} (Hidden)", CreateHiddenIcon(icon.ToBitmap()));
                    }
                }
            }
            else
            {
                key = attributes.HasFlag(FileAttributes.Directory) ? "folder" : "file";
            }

            key = attributes.HasFlag(FileAttributes.Hidden) ? $"{key} (Hidden)" : key;

            return imgFilesSmall.Images.IndexOfKey(key);
        }

        //Adds small and large icons for generic file and folder in normal and hidden variants to the image lists for treeview and listview
        private void GetDefaultIcons()
        {
            _smallFolderIndex = -1;
            _smallFolderHiddenIndex = -1;
            //Clear any existing icons from the lists to make sure we're not adding duplicate keys
            imgFilesSmall.Images.RemoveByKey("folder");
            imgFilesSmall.Images.RemoveByKey("folder (Hidden)");
            imgFilesSmall.Images.RemoveByKey("file");
            imgFilesSmall.Images.RemoveByKey("file (Hidden)");
            imgFilesLarge.Images.RemoveByKey("folder");
            imgFilesLarge.Images.RemoveByKey("folder (Hidden)");
            imgFilesLarge.Images.RemoveByKey("file");
            imgFilesLarge.Images.RemoveByKey("file (Hidden)");

            //If shell integration is disabled, we use our own icons for file and folder, otherwise we obtain them from the shell
            (string, ImageList, Icon)[] types =
            {
                ("file", imgFilesSmall, settings.Current.QueryShellForFileTypeInfo ? ShellInterop.SmallFileIcon : Properties.Resources.icon_page_white),
                ("file", imgFilesLarge, settings.Current.QueryShellForFileTypeInfo ? ShellInterop.LargeFileIcon : Properties.Resources.icon_page_white_32),
                ("folder", imgFilesSmall, settings.Current.QueryShellForFileTypeInfo ? ShellInterop.SmallFolderIcon : Properties.Resources.icon_folder),
                ("folder", imgFilesLarge, settings.Current.QueryShellForFileTypeInfo ? ShellInterop.LargeFolderIcon : Properties.Resources.icon_folder_32)
            };

            //Also create a hidden variant for every icon above
            foreach (var (key, list, icon) in types)
            {
                list.Images.Add(key, icon);
                list.Images.Add($"{key} (Hidden)", CreateHiddenIcon(icon.ToBitmap()));
            }

            //The "Up one folder" icon is special - it's always our own and doesn't need a hidden variant
            imgFilesSmall.Images.Add("up", Properties.Resources.folder_up);
            imgFilesLarge.Images.Add("up", Properties.Resources.folder_up_32);
            upOneFolderListViewItem.ImageIndex = imgFilesSmall.Images.IndexOfKey("up");

            // Cache folder icon indices so callers don't need repeated O(n) key lookups
            _smallFolderIndex = imgFilesSmall.Images.IndexOfKey("folder");
            _smallFolderHiddenIndex = imgFilesSmall.Images.IndexOfKey("folder (Hidden)");
        }

        //Finds the node with the specified entry
        private TreeNode? FindNode(TreeNode startNode, TiDirectory dir)
        {
            if (((TiDirectory)startNode.Tag).FullName == dir.FullName)
            {
                return startNode;
            }
            else foreach (TreeNode node in startNode.Nodes)
            {
                // hack
                if (((TiDirectory)node.Tag).FullName == dir.FullName)
                {
                    return node;
                }
                else
                {
                    TreeNode? nodeChild = FindNode(node, dir);
                    if (nodeChild is not null)
                    {
                        return nodeChild;
                    }
                }
            }

            return null;
        }

        private TiFileSystemObject? GetSelectedItemData(int idx)
        {
            if (lstFiles.SelectedIndices[idx] < IndexShift)
                return (TiFileSystemObject)upOneFolderListViewItem.Tag;
            return currentFolderView[lstFiles.SelectedIndices[idx] - IndexShift].Tag as TiFileSystemObject;
        }

        private static IComparer<ListViewItem> GetListViewItemSorter(int sortColumn, SortOrder sortOrder)
        {
            // Get the sorter for the selected column
            var sorter = FileListViewItemComparer.GetColumnSorter(sortColumn);

            if (sortOrder == SortOrder.Descending)
                sorter = new InvertedComparer<ListViewItem>(sorter);

            return sorter;
        }

        private void SortListView()
        {
            var sortOrder = viewModel.SortDirection.ToWinForms();
            IComparer<ListViewItem> sort = GetListViewItemSorter(viewModel.SortColumn, sortOrder);
            currentFolderView.Sort(sort);

            lstFiles.Refresh();
            lstFiles.SetSortIcon(viewModel.SortColumn, sortOrder);
        }

        private void SyncWindowState()
        {
            WindowState = uiState.Current.WindowState;
            Location = uiState.Current.WindowPosition;
            Size = uiState.Current.WindowSize;
            splitContainer.SplitterDistance = uiState.Current.SplitterDistance;

            foreach (ColumnHeader col in lstFiles.Columns)
            {
                col.DisplayIndex = uiState.Current.MWColumnOrder[col.Index];
                col.Width = uiState.Current.MWColumnWidth[col.Index];
            }
        }
    }
}
