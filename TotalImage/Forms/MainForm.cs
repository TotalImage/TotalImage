using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.Adapters;
using TotalImage.Changes;
using TotalImage.Controls;
using TotalImage.Services;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Main;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;
using TotalImage.FileSystems.FAT;
using TiDirectory = TotalImage.FileSystems.Directory;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Forms
{
    public partial class MainForm : Form, IMainViewBridge
    {
        private TiDirectory? draggedDir;

        public string tempDir;

        //For tracking which entry was last selected by keypress
        private char _lastTypeAheadChar = '\0';
        private int _lastTypeAheadIndex = -1;

        // Set while the partition combo box is filled from the view model.
        private bool updatingPartitionComboBox;

        private readonly MainViewModel viewModel;
        private readonly ISettingsService settings;
        private readonly ITempDirectoryService tempDirectory;
        private readonly UIStateService uiState;
        private readonly IEntryDialogs entryDialogs;
        private readonly IFilePickerService filePicker;

        private readonly FileIcons icons;
        private FileListAdapter fileListAdapter = null!;

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
                smallFileIcons.ImageSize = new SizeF(16 * (g.DpiX / 96f), 16 * (g.DpiY / 96f)).ToSize();
                largeFileIcons.ImageSize = new SizeF(32 * (g.DpiX / 96f), 32 * (g.DpiY / 96f)).ToSize();
            }

            icons = new FileIcons(smallFileIcons, largeFileIcons, settings);

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

            // Selecting everything moves the focus to the file list, where the entries get selected.
            selectAllToolStripMenuItem.Click += (_, _) => fileList.Focus();
            selectAllToolStripMenuItem1.Click += (_, _) => fileList.Focus();

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

            // The folder tree and the file list
            fileListAdapter = new FileListAdapter(fileList, vm, icons);
            _ = new FolderTreeAdapter(folderTree, vm, icons);
            vm.Observe(nameof(MainViewModel.Entries), () =>
            {
                _lastTypeAheadChar = '\0';
                _lastTypeAheadIndex = -1;
            });

            // Commands that switch to the file list (like extracting the root folder from the tree) move the focus there.
            vm.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(MainViewModel.ActivePane) && vm.ActivePane == BrowsePane.Files && folderTree.Focused)
                    fileList.Focus();
            };

            // View options
            vm.Observe(nameof(MainViewModel.ViewMode), () => fileList.View = vm.ViewMode.ToWinForms());
            vm.Observe(nameof(MainViewModel.ShowCommandBar), () => commandBar.Visible = vm.ShowCommandBar);
            vm.Observe(nameof(MainViewModel.ShowDirectoryTree), () => splitContainer.Panel1Collapsed = !vm.ShowDirectoryTree);
            vm.Observe(nameof(MainViewModel.ShowStatusBar), () => statusBar.Visible = vm.ShowStatusBar);

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
            vm.Observe(nameof(MainViewModel.IsImageOpen), () =>
            {
                var open = vm.IsImageOpen;
                partitionComboBox.Enabled = open;
                freeSpaceProgressBar.Visible = open;

                // Border sides give the status bar children separator-like looks while an image is open.
                partitionSizeLabel.BorderSides = open ? ToolStripStatusLabelBorderSides.Right : ToolStripStatusLabelBorderSides.None;
                selectionSizeLabel.BorderSides = open ? ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right : ToolStripStatusLabelBorderSides.None;
            });
            //This is needed so that icons update immediately, rather than after a restart of the app
            vm.SettingsApplied += (_, _) => icons.Reset();
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

        #endregion

        #region Event Handlers
        private async void MainForm_Load(object sender, EventArgs e)
        {
            //This fixes the problem of certain settings values not being returned correctly when using high DPI. Go figure...
            settings.Reload();
            uiState.Reload();

            icons.Reset();
            BindState();
            viewModel.LoadSettings();
            SyncWindowState();

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
            if (viewModel.Image is not { } image || viewModel.CurrentFolder is not { } folder) return;

            if (folder.Directory is FatDirectory fatDir)
            {
                fatDir.EnqueueCreateSubdirectory(newName);
            }
            else if (folder.Directory is PendingDirectory pendingDir)
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
            var selectedItems = viewModel.SelectedObjects;
            if (viewModel.ActivePane == BrowsePane.Files && selectedItems.Count > 0)
            {
                var selectedSize = 0ul;
                var fileCount = 0;
                var dirCount = 0;

                foreach (var entry in selectedItems)
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
                        Heading = $"{selectedItems.Count} item{(selectedItems.Count > 1 ? "s" : "")} will be deleted",
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

                foreach (var entry in selectedItems)
                {
                    if (entry is FatFile fatFile)
                        fatFile.EnqueueDelete();
                    else if (entry is FatDirectory fatDir)
                        fatDir.EnqueueDelete();
                }
            }
            else if (viewModel.ActivePane == BrowsePane.Folders && viewModel.CurrentFolder?.Directory.Parent is not null)
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

                if (viewModel.CurrentFolder?.Directory is FatDirectory selectedFatDir)
                    selectedFatDir.EnqueueDelete();
            }

            return Task.CompletedTask;
        }

        //Renames a file or folder
        void IMainViewBridge.Rename()
        {
            if (viewModel.ActivePane == BrowsePane.Files)
            {
                // Trigger inline label edit in the file list
                if (viewModel.SelectedEntries is [{ IsParentEntry: false } entry] && fileListAdapter.IndexOf(entry) is var index and >= 0)
                    fileList.Items[index].BeginEdit();
            }
            else if (viewModel.CurrentFolder is { IsRoot: false } && folderTree.SelectedNode is not null)
            {
                folderTree.SelectedNode.BeginEdit();
            }
        }

        private void FileListContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (viewModel.Image is null || viewModel.IsParentEntrySelected)
            {
                e.Cancel = true;
                return;
            }

            // The menu acts on the file list, so its commands follow the file list's rules.
            viewModel.ActivePane = BrowsePane.Files;
        }

        //TODO: Implement the Properties dialog for multiple selected objects like Windows does it
        async Task IMainViewBridge.ShowPropertiesAsync()
        {
            List<TiFileSystemObject> entries = new();
            if (viewModel.ActivePane == BrowsePane.Folders)
            {
                if (viewModel.CurrentFolder is { } folder && folder.Directory.Parent is null) //Can't show properties for the root node at this time
                {
                    entries.Add(folder.Directory);
                }
            }
            else
            {
                entries.AddRange(viewModel.SelectedObjects);
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

                var target = viewModel.CurrentFolder?.Directory;
                if (target is FatDirectory targetFatDir)
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
                else if (target is PendingDirectory pendingDir && viewModel.Image is { } image)
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

        private async void FileList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (viewModel.SelectedEntries is [var entry] && e.Button != MouseButtons.Right)
                await viewModel.OpenEntryAsync(entry);
        }

        private void FolderTreeContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (folderTree.Nodes.Count == 0 || folderTree.SelectedNode is null)
            {
                e.Cancel = true;
                return;
            }

            // The menu acts on the folder tree, so its commands follow the folder tree's rules.
            viewModel.ActivePane = BrowsePane.Folders;
        }

        private void BrowsePane_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data is not null && e.Data.GetDataPresent(DataFormats.FileDrop) && !viewModel.IsImageOpen)
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /* Drag and drop was performed on the ListView - a file was dragged into the ListView/TreeView from Explorer => try to open it
         * Right now, we only handle case a for opening a single file that was dragged into the window.
         * TODO: Implement other drag and drop scenarios (moving files within the image, etc.). */
        private async void BrowsePane_DragDrop(object sender, DragEventArgs e)
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

        private void FolderTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode newNode = folderTree.GetNodeAt(e.X, e.Y);

                //This prevents opening the menu on empty area of the TreeView
                if (newNode is not null)
                {
                    folderTree.SelectedNode = newNode;
                    folderTreeContextMenu.Show(folderTree, e.Location);
                }
                else
                {
                    return;
                }
            }
        }

        /* Fires when the user starts dragging a ListViewItem around. String array is needed for Explorer to perform the move operation once
         * the drop is performed. */
        private void FileList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Item is not null)
            {
                tempDir = tempDirectory.GetUniqueSubdirectoryPath();
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                if (((ListViewItem)e.Item).Tag is EntryViewModel { IsParentEntry: true })
                {
                    return;
                }

                List<string> items = new();
                foreach (TiFileSystemObject fso in viewModel.SelectedObjects)
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
                fileList.DoDragDrop(data, DragDropEffects.Copy);
            }
        }

        /* Fires when the user starts dragging a TreeNode around. String array is needed for Explorer to perform the move operation once
         * the drop is performed. */
        private void FolderTree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left && FolderTreeAdapter.FolderOf(e.Item as TreeNode) is { } folder)
            {
                tempDir = tempDirectory.GetUniqueSubdirectoryPath();
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                //This array is needed for Explorer to perform the file copy/move operation later on.
                List<string> items = new();
                draggedDir = folder.Directory;
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
                folderTree.DoDragDrop(data, DragDropEffects.Copy);
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

        private async void FileList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                if (viewModel.GoToParentFolderCommand.CanExecute(null))
                    viewModel.GoToParentFolderCommand.Execute(null);
            }
            else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                if (viewModel.SelectedEntries is [var entry])
                    await viewModel.OpenEntryAsync(entry);
            }
        }

        private void FileList_KeyPress(object sender, KeyPressEventArgs e)
        {
            //This selects the first entry whose name starts with the character pressed, then the next one, etc.

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

            //Search from startIndex, wrapping around once if needed. The ".." entry is never selected this way.
            var entries = viewModel.Entries;
            int count = entries.Count;
            for (int i = 0; i < count; i++)
            {
                int idx = (startIndex + i) % count;
                if (!entries[idx].IsParentEntry && entries[idx].Name.StartsWith(ch.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    viewModel.SelectEntries([entries[idx]]);
                    fileList.FocusedItem = fileList.Items[idx];
                    fileList.EnsureVisible(idx);
                    _lastTypeAheadChar = ch;
                    _lastTypeAheadIndex = idx;
                    e.Handled = true;
                    return;
                }
            }
        }

        private void FileList_Enter(object sender, EventArgs e)
        {
            viewModel.ActivePane = BrowsePane.Files;
        }

        private void FolderTree_Enter(object sender, EventArgs e)
        {
            viewModel.ActivePane = BrowsePane.Folders;
        }

        //After an item's label (=Text property) is changed - for renaming objects
        //From here the name change should propagate to the associated FileSystemObject and to the stream
        private void FileList_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label is null)
                return; // user cancelled

            if (e.Item < 0 || e.Item >= viewModel.Entries.Count)
            {
                e.CancelEdit = true;
                return;
            }

            var fso = fileListAdapter.EntryAt(e.Item).Entry;
            if (fso is FatFile fatFile)
                fatFile.EnqueueRename(e.Label);
            else if (fso is FatDirectory fatDir)
                fatDir.EnqueueRename(e.Label);
            else
                e.CancelEdit = true;
        }

        //Before an item's label (=Text property) will be changed - for renaming objects.
        //Here we should probably perform some sanity checks
        private void FileList_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            // The ".." entry stands for the parent folder, which isn't renamed from here.
            if (viewModel.Image?.SupportsWriting == false || fileListAdapter.EntryAt(e.Item).IsParentEntry)
            {
                e.CancelEdit = true;
            }
        }

        //After a node's label (=Text property) is changed - for renaming objects
        //From here the name change should propagate to the associated FileSystemObject and to the stream
        private void FolderTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label is null)
                return; // user cancelled

            if (FolderTreeAdapter.FolderOf(e.Node)?.Directory is FatDirectory fatDir)
                fatDir.EnqueueRename(e.Label);
            else
                e.CancelEdit = true;
        }

        //Before a node's label (=Text property) will be changed - for renaming objects.
        //Here we should probably perform some sanity checks
        private void FolderTree_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (viewModel.Image?.SupportsWriting == false)
            {
                e.CancelEdit = true;
            }
        }

        private void BrowsePane_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
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
                        extractionSucceeded = FileExtraction.ExtractFilesToTemporaryDirectory(this, settings.Current, viewModel.SelectedObjects, DirectoryExtractionMode.Preserve);
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
        #endregion

        /// <summary>
        /// Splits a FullName like "\SUB\CHILD" into ["SUB", "CHILD"].
        /// The root ("\") returns an empty array.
        /// </summary>
        private static string[] FullNameToPathComponents(string fullName)
            => fullName.Split(System.IO.Path.DirectorySeparatorChar,
                              StringSplitOptions.RemoveEmptyEntries);

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

            foreach (ColumnHeader col in fileList.Columns)
            {
                uiState.Current.MWColumnOrder[col.Index] = col.DisplayIndex;
                uiState.Current.MWColumnWidth[col.Index] = col.Width;
            }
        }

        private void SyncWindowState()
        {
            WindowState = uiState.Current.WindowState;
            Location = uiState.Current.WindowPosition;
            Size = uiState.Current.WindowSize;
            splitContainer.SplitterDistance = uiState.Current.SplitterDistance;

            foreach (ColumnHeader col in fileList.Columns)
            {
                col.DisplayIndex = uiState.Current.MWColumnOrder[col.Index];
                col.Width = uiState.Current.MWColumnWidth[col.Index];
            }
        }
    }
}
