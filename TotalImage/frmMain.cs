using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using TotalImage.FileSystems.FAT;
using TotalImage.Containers;
using static Interop.ComCtl32;
using static Interop.Shell32;
using static Interop.User32;
using System.Diagnostics;
using TotalImage.FileSystems.BPB;

using TiFile = TotalImage.FileSystems.File;
using TiDirectory = TotalImage.FileSystems.Directory;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage
{
    public partial class frmMain : Form
    {
        public string filename = "";
        public string filepath = "";
        public bool unsavedChanges = false;
        public Container? image;
        public int CurrentPartitionIndex;
        private int sortColumn;
        private SortOrder sortOrder;
        private TiDirectory lastViewedDir;
        internal static Dictionary<string, (string name, int iconIndex)> fileTypes = new Dictionary<string, (string name, int iconIndex)>(StringComparer.InvariantCultureIgnoreCase);
        private string? lastSavedFilename;

        private ListViewItem upOneFolderListViewItem = new ListViewItem()
        {
            Text = ".."
        };

        private List<ListViewItem> currentFolderView = new List<ListViewItem>();

        public frmMain()
        {
            InitializeComponent();
        }

        #region Event Handlers
        private void frmMain_Load(object sender, EventArgs e)
        {
            //These three are outside Sync because otherwise they'd be applied whenever settings are saved...
            WindowState = Settings.CurrentSettings.WindowState;
            Location = Settings.CurrentSettings.WindowPosition;
            Size = Settings.CurrentSettings.WindowSize;
            SyncUIWithSettings();

            //Because designer doesn't have the Enter key in the list for some reason...
            propertiesToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.Enter;
            propertiesToolStripMenuItem1.ShortcutKeys = Keys.Alt | Keys.Enter;
            propertiesToolStripMenuItem2.ShortcutKeys = Keys.Alt | Keys.Enter;

#if !DEBUG
            DisableUI(); //Once support for command line arguments is added, those will need to be checked before this is done...
#endif
            GetDefaultIcons();
            lstDirectories.SelectedImageIndex = imgFilesSmall.Images.IndexOfKey("folder");

            //This is a workaround because the designer is apparently not setting the ColumnHeader.Name attributes...
            lstFiles.Columns[0].Name = "clmName";
            lstFiles.Columns[1].Name = "clmType";
            lstFiles.Columns[2].Name = "clmSize";
            lstFiles.Columns[3].Name = "clmModified";
            lstFiles.Columns[4].Name = "clmAttributes";

            //Open the file that was dragged onto the exe/shortcut or passed as a command line argument
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string argPath = args[1];
                if (File.Exists(argPath))
                {
                    OpenImage(argPath);
                }
                else
                {
#if NET5_0_OR_GREATER
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"The file \"{Path.GetFileName(argPath)}\" could not be opened because it's inaccessible or does not exist.",
                        Heading = "Could not open file",
                        Caption = "Error",
                        Buttons =
                    {
                        TaskDialogButton.OK
                    },
                        Icon = TaskDialogIcon.Error,
                        DefaultButton = TaskDialogButton.OK
                    });
#elif NET48
                    MessageBox.Show($"The file \"{Path.GetFileName(argPath)}\" could not be opened because it's inaccessible or does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                }
            }

            for (var i = 1; i < lstFiles.Columns.Count; i++)
                upOneFolderListViewItem.SubItems.Add(string.Empty);
        }

        //Syncs the main form UI with the current settings
        private void SyncUIWithSettings()
        {
            lstFiles.View = Settings.CurrentSettings.FilesView;
            splitContainer.Panel1Collapsed = !Settings.CurrentSettings.ShowDirectoryTree;
            statusBar.Visible = Settings.CurrentSettings.ShowStatusBar;
            commandBar.Visible = Settings.CurrentSettings.ShowCommandBar;
            sortOrder = Settings.CurrentSettings.FilesSortOrder;
            sortColumn = Settings.CurrentSettings.FilesSortingColumn;
            splitContainer.SplitterDistance = Settings.CurrentSettings.SplitterDistance;

            lstFiles.SetSortIcon(sortColumn, sortOrder);

            PopulateRecentList();
        }

        //Injects a folder into the image
        private void injectFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;

#if NET48
            //We only do this for .NET Framework because it has the old FBD and there's a notable empty space at the top without the description.
            //Meanwhile, .NET 5 has the new Vista+ FBD which doesn't handle the description well visually, especially if dark Explorer theme is used.
            fbd.Description = "Select a folder to inject into the image.";
#endif

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                /* Inject the entire selected folder into the image */
                throw new NotImplementedException("This feature is not implemented yet");
            }
        }

        //Shows a hex view of the current image
        private void hexView_Click(object sender, EventArgs e)
        {
            using frmHexView frm = new frmHexView();
            frm.ShowDialog();
        }

        //Allows viewing and editing both volume labels
        //TODO: Actually change the volume labels
        private void changeVolumeLabel_Click(object sender, EventArgs e)
        {
            if (!(image?.PartitionTable.Partitions[0].FileSystem is Fat12FileSystem fs))
            {
                MessageBox.Show("This only works for FAT12 images");
                return;
            }

            using dlgChangeVolLabel dlg = new dlgChangeVolLabel(fs.GetRDVolLabel(), fs.GetBPBVolLabel());
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                unsavedChanges = true;
                saveToolStripButton.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
            }
        }

        //Allows viewing and editing bootsector properties
        private void bootSectorProperties_Click(object sender, EventArgs e)
        {
            using dlgBootSector dlg = new dlgBootSector();
            dlg.ShowDialog();
        }

        //Shows current image information
        private void imageInformation_Click(object sender, EventArgs e)
        {
            using dlgImageInfo dlg = new dlgImageInfo();
            dlg.ShowDialog();
        }

        //Click event handler for all menu items in the Recent images menu
        private void recentImage_Click(object sender, EventArgs e)
        {
            string imagePath = ((ToolStripMenuItem)sender).Text.Substring(3, ((ToolStripMenuItem)sender).Text.Length - 3).Trim(' ');
            try
            {
                CloseImage();
                OpenImage(imagePath);
            }
            catch (IOException)
            {
#if NET48
                MessageBox.Show("Selected file could not be opened because it's inaccessible or no longer exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#elif NET5_0_OR_GREATER
                TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = $"Selected file could not be opened because it's inaccessible or no longer exists.",
                    Heading = "Could not open file",
                    Caption = "Error",
                    Buttons =
                    {
                        TaskDialogButton.OK
                    },
                    Icon = TaskDialogIcon.Error,
                    DefaultButton = TaskDialogButton.OK
                });
#endif
                //Remove the non-working entry
                Settings.RemoveRecentImage(imagePath);
                PopulateRecentList();
            }
        }

        //Creates a new disk image
        //TODO: Implement the "save changes first" code path
        private void newImage_Click(object sender, EventArgs e)
        {
            using dlgNewImage dlg = new dlgNewImage();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (unsavedChanges)
                {
                    DialogResult = MessageBox.Show("You have unsaved changes in the current image. Would you like to save them before creating a new image?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (DialogResult == DialogResult.Yes) /*save_Click(sender, e);*/;
                    else if (DialogResult == DialogResult.Cancel) return;
                }

                if (image != null)
                    CloseImage();
                image = null;

                Text = "(Untitled) - TotalImage";
                unsavedChanges = true;

                BiosParameterBlock bpb = dlg.BPBVersion == BiosParameterBlockVersion.Dos34 || dlg.BPBVersion == BiosParameterBlockVersion.Dos40
                    ? ExtendedBiosParameterBlock.FromGeometry(dlg.Geometry, dlg.BPBVersion, dlg.OEMID, dlg.SerialNumber, dlg.FileSystemType, dlg.VolumeLabel)
                    : BiosParameterBlock.FromGeometry(dlg.Geometry, dlg.BPBVersion, dlg.OEMID);

                image = RawContainer.CreateImage(bpb, dlg.Geometry.Tracks, dlg.WriteBPB);
                EnableUI();
            }
        }

        /* The Save button on the command bar acts as either:
         * -"Save" when the file is already saved and there are unsaved changes
         * -"Save as" when the file has not been saved yet */
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (unsavedChanges)
            {
                if (string.IsNullOrEmpty(filename)) //File hasn't been saved yet
                {
                    saveAs_Click(sender, e);
                }
                else
                {
                    save_Click(sender, e);
                }
            }
        }

        //Creates a new folder
        private void newFolder_Click(object sender, EventArgs e)
        {
            using dlgNewFolder dlg = new dlgNewFolder();
            dlg.ShowDialog();
        }

        private void viewLargeIcons_Click(object sender, EventArgs e)
        {
            lstFiles.View = View.LargeIcon;
            Settings.CurrentSettings.FilesView = View.LargeIcon;
        }

        private void viewSmallIcons_Click(object sender, EventArgs e)
        {
            lstFiles.View = View.SmallIcon;
            Settings.CurrentSettings.FilesView = View.SmallIcon;
        }

        private void viewList_Click(object sender, EventArgs e)
        {
            lstFiles.View = View.List;
            Settings.CurrentSettings.FilesView = View.List;
        }

        private void viewDetails_Click(object sender, EventArgs e)
        {
            lstFiles.View = View.Details;
            Settings.CurrentSettings.FilesView = View.Details;
        }

        private int IndexShift => lstFiles.VirtualListSize - currentFolderView.Count;

        private IEnumerable<TiFileSystemObject> SelectedItems
            => from x in lstFiles.SelectedIndices.Cast<int>()
                where x >= IndexShift && currentFolderView[x - IndexShift].Tag is TiFileSystemObject 
                select (TiFileSystemObject)currentFolderView[x - IndexShift].Tag;

        //Deletes a file or folder
        //TODO: Implement deletion here and in the FS/container
        private void delete_Click(object sender, EventArgs e)
        {
            if (lstFiles.Focused)
            {
                var selectedSize = 0ul;
                foreach (var entry in SelectedItems) selectedSize += entry.Length;

                //DialogResult = MessageBox.Show($"Are you sure that you want to delete {lstFiles.SelectedIndices.Count} item(s) occupying {Settings.CurrentSettings.SizeUnits.FormatSize(selectedSize)}?", "Delete items", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                //if (DialogResult == DialogResult.Yes)
                {
                    throw new NotImplementedException("This feature is not implemented yet");
                }
            }
            else if (lstDirectories.Focused)
            {
                throw new NotImplementedException("This feature is not implemented yet");
            }
        }

        //Undeletes a delete file or folder
        //TODO: Implement this here and in FS/container. Although checks are already performed in menu item code to disable the option entirely
        //when it's not applicable, some additional checks here probably wouldn't hurt either...
        private void undelete_Click(object sender, EventArgs e)
        {
            using dlgUndelete dlg = new dlgUndelete();
            dlg.ShowDialog();
        }

        //Renames a file or folder
        //TODO: Implement this here and in FS/container.
        private void rename_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            /*
            string oldname = "";
            if (lstFiles.Focused)
                oldname = currentFolderView[lstFiles.SelectedIndices[0]].Text;
            else if (lstDirectories.Focused)
                oldname = lstDirectories.SelectedNode.Text;

            using dlgRename dlg = new dlgRename(oldname);
            dlg.ShowDialog();

            string newname = dlg.NewName;
            */
        }

        //Changes image format
        //TODO: The best UI for this still needs to be determined. Once that's done, implement the functionality here and in FS/container.
        private void changeFormat_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException("This feature is not implemented yet");
        }

        //Defragments the selected partition
        //TODO: Implement this here and in FS/container.
        private void defragment_Click(object sender, EventArgs e)
        {
            using dlgDefragment dlg = new dlgDefragment();
            dlg.ShowDialog();
        }

        //Formats the selected partition/floppy disk
        //TODO: Implement this here and in FS/container.
        private void format_Click(object sender, EventArgs e)
        {
#if DEBUG
            using dlgFormat dlg = new dlgFormat();
            dlg.ShowDialog();
#endif

            if (MessageBox.Show("Are you sure you want to format this image? This will erase all data inside!", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                //Need to figure out how to actually do this, because right now it's unclear...
                // DoSomeFormatThing();
                // MessageBox.Show("The image was successfully formatted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Save the changes made to the current image since the last save or since it was opened
        //TODO: Perhaps this needs some rethinking too, depending on recent changes to the container?
        private void save_Click(object sender, EventArgs e)
        {
            if (image == null)
            {
                throw new Exception("No image is currently loaded");
            }

            image.SaveImage(filepath);

            saveToolStripButton.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            Text = $"{filename} - TotalImage";
            unsavedChanges = false;
        }

        //Saves the current image as a new file, along with any changes made to it since the last save
        private void saveAs_Click(object sender, EventArgs e)
        {
            if (image == null)
            {
                throw new Exception("No image is currently loaded");
            }

            using SaveFileDialog sfd = new SaveFileDialog();
            sfd.AutoUpgradeEnabled = true;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            sfd.OverwritePrompt = true;
            sfd.DefaultExt = "img";
            sfd.Filter =
                "Raw sector image (*.img, *.ima, *.vfd, *.flp, *.dsk, *.xdf, *.hdm)|*.img;*.ima;*.vfd;*.flp;*.dsk;*.xdf;*.hdm|" +
                "All files (*.*)|*.*";

            if (lastSavedFilename != null)
            {
                string nameNoExt = Path.GetFileNameWithoutExtension(lastSavedFilename);
                string number = System.Text.RegularExpressions.Regex.Match(nameNoExt, @"\d+$").Value;
                string prefix = nameNoExt.Substring(0, nameNoExt.LastIndexOf(number));
                int i = int.Parse(number) + 1;
                string newFilename = prefix + i.ToString(new string('0', number.Length));
                sfd.FileName = newFilename;
            }
            else
            {
                Debug.WriteLine("lastSavedFilename is null");
            }

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (sfd.FilterIndex == 0 ||
                    sfd.FileName.EndsWith(".img", StringComparison.OrdinalIgnoreCase) ||
                    sfd.FileName.EndsWith(".ima", StringComparison.OrdinalIgnoreCase) ||
                    sfd.FileName.EndsWith(".vfd", StringComparison.OrdinalIgnoreCase) ||
                    sfd.FileName.EndsWith(".flp", StringComparison.OrdinalIgnoreCase) ||
                    sfd.FileName.EndsWith(".dsk", StringComparison.OrdinalIgnoreCase) ||
                    sfd.FileName.EndsWith(".hdm", StringComparison.OrdinalIgnoreCase))
                {
                    image.SaveImage(sfd.FileName);

                    if(System.Text.RegularExpressions.Regex.Match(Path.GetFileNameWithoutExtension(sfd.FileName), @"\d+$").Success && Settings.CurrentSettings.AutoIncrementFilename)
                    {
                        Debug.WriteLine("Regex matched and autoincrement enabled");
                        lastSavedFilename = Path.GetFileName(sfd.FileName);
                        Debug.WriteLine($"lastSavedFilename is now \"{lastSavedFilename}\"");
                    }
                    else
                    {
                        Debug.WriteLine("Regex didn't match or autoincrement disabled");
                        lastSavedFilename = null;
                        Debug.WriteLine("lastSavedFilename is now null");
                    }

                    filepath = sfd.FileName;
                    filename = Path.GetFileName(filepath);
                    Text = $"{filename} - TotalImage";

                    Settings.AddRecentImage(filepath);
                    PopulateRecentList();
                    unsavedChanges = false;
                    saveToolStripButton.Enabled = false;
                }

            }
        }

        //Closes the application
        private void exit_Click(object sender, EventArgs e)
        {
            if (unsavedChanges)
            {
                DialogResult result = MessageBox.Show("You have unsaved changed. Would you like to save them before closing TotalImage?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    save_Click(sender, e);
                }
                else if (DialogResult == DialogResult.Cancel) return;
            }
            Application.Exit();
        }

        private void toggleCommandBar_Click(object sender, EventArgs e)
        {
            commandBar.Visible = !commandBar.Visible;
            Settings.CurrentSettings.ShowCommandBar = commandBar.Visible;
        }

        private void toggleDirectoryTree_Click(object sender, EventArgs e)
        {
            splitContainer.Panel1Collapsed = !splitContainer.Panel1Collapsed;
            Settings.CurrentSettings.ShowDirectoryTree = !splitContainer.Panel1Collapsed;
        }

        private void toggleStatusBar_Click(object sender, EventArgs e)
        {
            statusBar.Visible = !statusBar.Visible;
            Settings.CurrentSettings.ShowStatusBar = statusBar.Visible;
        }

        //TODO: Implement the "save changes first" code path
        private void openImage_Click(object sender, EventArgs e)
        {
            if (unsavedChanges)
            {
                DialogResult = MessageBox.Show("You have unsaved changes. Would you like to save the current image first before opening another one?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (DialogResult)
                {
                    case DialogResult.Cancel: return;
                    case DialogResult.Yes: /*SaveChanges()*/ break;
                }
            }

            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.AutoUpgradeEnabled = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;
            //We probably want this, but it degrades the dialog appearance to XP dialog... Some workaround for this would be nice.
            //ofd.ShowReadOnly = true;
            ofd.Filter =
                "Raw sector image (*.img, *.ima, *.vfd, *.flp, *.dsk, *.xdf, *.hdm)|*.img;*.ima;*.vfd;*.flp;*.dsk;*.xdf;*.hdm|" +
                "Microsoft VHD (*.vhd)|*.vhd|" +
                "All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CloseImage();
                OpenImage(ofd.FileName);
            }
        }

        private void about_Click(object sender, EventArgs e)
        {
            using dlgAbout dlg = new dlgAbout();
            dlg.ShowDialog();
        }

        //Extracts file(s) or folder(s) from the image to the specified path
        //TODO: Implement this here, in the extraction dialog and in FS/container.
        private void extract_Click(object sender, EventArgs e)
        {
            if (Settings.CurrentSettings.ExtractAlwaysAsk)
            {
                using dlgExtract dlg = new dlgExtract();
                dlg.lblPath.Text = $"Extract {SelectedItems.Count()} selected {(SelectedItems.Count() > 1 ? "items" : "item")} to the following folder:";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ExtractFiles(SelectedItems, dlg.TargetPath, dlg.ExtractType, dlg.OpenFolder);
                }
            }
            else
            {
                ExtractFiles(SelectedItems, Settings.CurrentSettings.DefaultExtractPath, Settings.CurrentSettings.DefaultExtractType, Settings.CurrentSettings.OpenFolderAfterExtract);
            }
        }

        private void ExtractFiles(IEnumerable<TiFileSystemObject> items, string path, Settings.FolderExtract extractType, bool openFolder)
        {
            var files = from x in items where x is TiFile select x as TiFile;

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            foreach (var file in files)
            {
                if(File.Exists(Path.Combine(path, file.Name)) && Settings.CurrentSettings.ConfirmOverwriteExtraction){
#if NET48
                    if (DialogResult.No == MessageBox.Show($"File {file.Name} already exists in the target directory. Do you want to overwrite it?", "File already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                        continue;
#elif NET5_0_OR_GREATER
                    TaskDialogButton result = TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"The file \"{file.Name}\" already exists in the target directory. Do you want to overwrite it?",
                        Heading = "File already exists",
                        Caption = "Warning",
                        Buttons =
                        {
                            TaskDialogButton.Yes,
                            TaskDialogButton.No
                        },
                        Icon = TaskDialogIcon.Warning,
                        DefaultButton = TaskDialogButton.Yes
                    });

                    if (result == TaskDialogButton.No)
                        continue;
#endif
                }

                using (var destStream = new FileStream(Path.Combine(path, file.Name), FileMode.Create))
                    file.GetStream().CopyTo(destStream);

                if (Settings.CurrentSettings.ExtractPreserveDates)
                {
                    if (file.CreationTime.HasValue)
                        File.SetCreationTime(Path.Combine(path, file.Name), file.CreationTime.Value);

                    if (file.LastAccessTime.HasValue)
                        File.SetLastAccessTime(Path.Combine(path, file.Name), file.LastAccessTime.Value);

                    if (file.LastWriteTime.HasValue)
                        File.SetLastWriteTime(Path.Combine(path, file.Name), file.LastWriteTime.Value);
                }

                if (Settings.CurrentSettings.ExtractPreserveAttributes)
                {
                    /* NOTE: Windows automatically sets the Archive attribute on all newly created files, so even if the attribute is cleared in the
                     * image, Windows will still automatically set it anyway. Should we perhaps try to work around this by manually clearing it after? */
                    File.SetAttributes(Path.Combine(path, file.Name), file.Attributes);
                }
            }

            if (extractType != Settings.FolderExtract.Ignore)
            {
                var dirs = from x in items where x is TiDirectory select x as TiDirectory;

                foreach (var dir in dirs)
                {
                    ExtractFiles(
                        dir.EnumerateFileSystemObjects(Settings.CurrentSettings.ShowHiddenItems, Settings.CurrentSettings.ShowDeletedItems),
                        extractType switch
                        {
                            Settings.FolderExtract.Merge => path,
                            Settings.FolderExtract.Preserve => Path.Combine(path, dir.Name),
                            _ => throw new ArgumentException()
                        }, extractType, false);
                }
            }

            if (openFolder)
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }
        }

        enum StatusBarStates
        {
            NoneSelected,
            OneSelected,
            MultipleSelected
        }

        StatusBarStates StatusBarState
        {
            get
            {
                var selectedItems = lstFiles.SelectedIndices.Count - IndexShift;
                switch (selectedItems > 0 ? selectedItems : 0)
                {
                    case 0:
                        return StatusBarStates.NoneSelected;
                    case 1:
                        return StatusBarStates.OneSelected;
                    default:
                        return StatusBarStates.MultipleSelected;
                }
            }
        }

        private void UpdateStatusBar(bool UpdateFreeSpace)
        {
            if (image == null)
            {
                lblStatusCapacity.Text = string.Empty;
                lblStatusFreeCapacity.Text = string.Empty;
                lbStatusPath.Text = string.Empty;
                lblStatusSize.Text = string.Empty;
            }
            else
            {
                //Makes no sense to do this every time selection changes, etc. Only when operations that affect the free space are performed
                if (UpdateFreeSpace)
                {
                    lblStatusCapacity.Text = $"Partition size: {Settings.CurrentSettings.SizeUnit.FormatSize((ulong)image.PartitionTable.Partitions[CurrentPartitionIndex].Length)}";
                    double FreeSpacePercentage = (double)image.PartitionTable.Partitions[CurrentPartitionIndex].FileSystem.TotalFreeSpace / image.PartitionTable.Partitions[CurrentPartitionIndex].Length * 100;
                    lblStatusFreeCapacity.Text = $"Free space: {Settings.CurrentSettings.SizeUnit.FormatSize((ulong)image.PartitionTable.Partitions[CurrentPartitionIndex].FileSystem.TotalFreeSpace)} ({FreeSpacePercentage / 100:p2})";
                    if ((int)FreeSpacePercentage <= 10)
                        SendMessage(lblStatusProgressBar.ProgressBar.Handle, 1040, new IntPtr(2), IntPtr.Zero); // Set the progress bar colour to red.
                    else if ((int)FreeSpacePercentage <= 20)
                        SendMessage(lblStatusProgressBar.ProgressBar.Handle, 1040, new IntPtr(3), IntPtr.Zero); // Set the progress bar colour to yellow.
                    else
                        SendMessage(lblStatusProgressBar.ProgressBar.Handle, 1040, new IntPtr(1), IntPtr.Zero); // Set the progress bar colour to green.

                    // Set progress bar value with a bit of a hack to disable the glow.
                    lblStatusProgressBar.Minimum = 100 - (int)FreeSpacePercentage;
                    lblStatusProgressBar.Value = 100 - (int)FreeSpacePercentage;
                    lblStatusProgressBar.Minimum = 0;
                }

                switch (StatusBarState)
                {
                    case StatusBarStates.NoneSelected:
                        {
                            var dir = (TiDirectory)lstDirectories.SelectedNode.Tag;
                            lbStatusPath.Text = dir.FullName;
                            lblStatusSize.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize(CalculateDirSize())} in {GetFileCount()} item(s)";
                            break;
                        }
                    case StatusBarStates.OneSelected:
                        {
                            var item = GetSelectedItemData(0);
                            lbStatusPath.Text = item.FullName;
                            lblStatusSize.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize(item.Length)} in 1 item";
                            break;
                        }
                    case StatusBarStates.MultipleSelected:
                        {
                            var dir = (TiDirectory)lstDirectories.SelectedNode.Tag;
                            var selectedSize = 0ul;
                            foreach (var entry in SelectedItems) selectedSize += entry.Length;

                            lbStatusPath.Text = dir.FullName;
                            lblStatusSize.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize(selectedSize)} in {SelectedItems.Count()} item(s)";
                            break;
                        }
                }
            }
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e) // This method will be used more than once, thus it is separated from the main event.
        {
            if (image != null)
            {
                if (lstFiles.SelectedIndices.Count == 0 || lstFiles.SelectedIndices.Count == 1 && lstFiles.SelectedIndices[0] < IndexShift)
                {
                    deleteToolStripMenuItem.Enabled = false;
                    deleteToolStripMenuItem2.Enabled = false;
                    extractToolStripMenuItem.Enabled = false;
                    extractToolStripMenuItem2.Enabled = false;
                    propertiesToolStripMenuItem.Enabled = false;
                    propertiesToolStripMenuItem2.Enabled = false;
                    renameToolStripMenuItem2.Enabled = false;
                    renameToolStripMenuItem.Enabled = false;
                    undeleteToolStripMenuItem.Enabled = false;
                    undeleteToolStripMenuItem2.Enabled = false;
                    newFolderToolStripMenuItem.Enabled = false;
                    newFolderToolStripMenuItem2.Enabled = false;

                    deleteToolStripButton.Enabled = false;
                    extractToolStripButton.Enabled = false;
                    propertiesToolStripButton.Enabled = false;

                    UpdateStatusBar(false);
                }
                else if (lstFiles.SelectedIndices.Count == 1)
                {
                    propertiesToolStripMenuItem.Enabled = true;
                    propertiesToolStripMenuItem2.Enabled = true;

                    //Check if selected item is a deleted entry and enable the UI accordingly
                    TiFileSystemObject entry = GetSelectedItemData(0);
                    undeleteToolStripMenuItem2.Enabled = entry.Name.StartsWith("?");
                    undeleteToolStripMenuItem.Enabled = entry.Name.StartsWith("?");
                    deleteToolStripMenuItem.Enabled = !entry.Name.StartsWith("?");
                    deleteToolStripMenuItem2.Enabled = !entry.Name.StartsWith("?");
                    extractToolStripMenuItem.Enabled = !entry.Name.StartsWith("?");
                    extractToolStripMenuItem2.Enabled = !entry.Name.StartsWith("?");
                    renameToolStripMenuItem.Enabled = !entry.Name.StartsWith("?");
                    renameToolStripMenuItem2.Enabled = !entry.Name.StartsWith("?");
                    newFolderToolStripMenuItem.Enabled = !entry.Name.StartsWith("?");
                    newFolderToolStripMenuItem2.Enabled = !entry.Name.StartsWith("?");

                    deleteToolStripButton.Enabled = !entry.Name.StartsWith("?");
                    extractToolStripButton.Enabled = !entry.Name.StartsWith("?");
                    propertiesToolStripButton.Enabled = true;

                    UpdateStatusBar(false);
                }
                else
                {
                    deleteToolStripMenuItem.Enabled = true;
                    deleteToolStripMenuItem2.Enabled = true;
                    extractToolStripMenuItem.Enabled = true;
                    extractToolStripMenuItem2.Enabled = true;
                    propertiesToolStripMenuItem.Enabled = true;
                    propertiesToolStripMenuItem2.Enabled = true;
                    renameToolStripMenuItem2.Enabled = false;
                    renameToolStripMenuItem.Enabled = false;
                    undeleteToolStripMenuItem.Enabled = false;
                    undeleteToolStripMenuItem2.Enabled = false;
                    newFolderToolStripMenuItem.Enabled = false;
                    newFolderToolStripMenuItem2.Enabled = false;

                    deleteToolStripButton.Enabled = true;
                    extractToolStripButton.Enabled = true;
                    propertiesToolStripButton.Enabled = false;

                    var path = lstDirectories.SelectedNode.FullPath;
                    if (path.Substring(path.Length - lstDirectories.PathSeparator.Length) != lstDirectories.PathSeparator)
                        path += lstDirectories.PathSeparator;

                    UpdateStatusBar(false);
                }
            }
        }

        private void cmsFileList_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lstFiles.SelectedIndices.Count == 0 || lstFiles.SelectedIndices.Count == 1 && lstFiles.SelectedIndices[0] < IndexShift)
            {
                e.Cancel = true;
                return;
            }
        }

        private void managePartitions_Click(object sender, EventArgs e)
        {
            using dlgManagePart dlg = new dlgManagePart();
            dlg.ShowDialog();
        }

        private void settings_Click(object sender, EventArgs e)
        {
            using dlgSettings dlg = new dlgSettings();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SyncUIWithSettings();
                ResetView();
                UpdateStatusBar(true);
                if (!Settings.CurrentSettings.AutoIncrementFilename)
                    lastSavedFilename = null;
            }
        }

        //TODO: Implement the Properties dialog for multiple selected objects like Windows does it
        private void properties_Click(object sender, EventArgs e)
        {
            var itemParent = (sender as ToolStripItem)?.GetCurrentParent();

            if (itemParent == cmsFileList || itemParent == commandBar)
            {
                // Show properties for the selected file list view items
                if (lstFiles.SelectedIndices.Count == 1)
                {
                    // Single selected item
                    using dlgProperties dlg = new dlgProperties(GetSelectedItemData(0));
                    dlg.ShowDialog();
                }
                else if (lstFiles.SelectedIndices.Count > 1)
                {
                    // More selected items
                    throw new NotImplementedException("This feature is not implemented yet.");
                }
            }
            else if (itemParent == cmsDirTree)
            {
                // Show properties for the selected directory in the tree view
                using dlgProperties dlg = new dlgProperties((TiFileSystemObject)lstDirectories.SelectedNode.Tag);
                dlg.ShowDialog();
            }
        }

        private void injectFiles_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.AutoUpgradeEnabled = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = true;
            ofd.Filter = "All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                throw new NotImplementedException("This feature is not implemented yet");
            }
        }

        private void closeImage_Click(object sender, EventArgs e)
        {
            if (unsavedChanges)
            {
                DialogResult = MessageBox.Show("You have unsaved changes in the current image. Would you like to save them before closing the image?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question); ;
                if (DialogResult == DialogResult.Yes) /*SaveChanges();*/;
                else if (DialogResult == DialogResult.Cancel) return;
            }
            CloseImage();
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
            IComparer<ListViewItem> sort = GetListViewItemSorter(sortColumn, sortOrder);
            currentFolderView.Sort(sort);

            lstFiles.Refresh();
            lstFiles.SetSortIcon(sortColumn, sortOrder);
        }

        private void SortListViewBy(int column)
        {
            if (column == sortColumn)
            {
                // Reverse the current sort direction for this column.
                if (sortOrder == SortOrder.Ascending)
                {
                    sortOrder = SortOrder.Descending;
                }
                else
                {
                    sortOrder = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                sortColumn = column;
                sortOrder = SortOrder.Ascending;
            }


            // Perform the sort with these new sort options.
            SortListView();

            Settings.CurrentSettings.FilesSortingColumn = column;
            Settings.CurrentSettings.FilesSortOrder = sortOrder;
        }

        private void lstFiles_ColumnClick(object sender, ColumnClickEventArgs e)
            => SortListViewBy(e.Column);

        private void sortByType_Click(object sender, EventArgs e)
            => SortListViewBy(lstFiles.Columns.IndexOfKey("clmType"));

        private void sortByModified_Click(object sender, EventArgs e)
            => SortListViewBy(lstFiles.Columns.IndexOfKey("clmModified"));

        private void sortByName_Click(object sender, EventArgs e)
            => SortListViewBy(lstFiles.Columns.IndexOfKey("clmName"));

        private void sortBySize_Click(object sender, EventArgs e)
            => SortListViewBy(lstFiles.Columns.IndexOfKey("clmSize"));

        //This prevents the user from opening a deleted directory (since we don't even know yet if it's recoverable, or what was inside, etc.)
        private void lstDirectories_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            TiDirectory dir = (TiDirectory)e.Node.Tag;
            if (dir.Name.StartsWith("?"))
            {
                e.Cancel = true;
                return;
            }
        }

        //TODO: Move that file count stuff elsewhere and just call it to get the number.
        private void lstDirectories_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //This makes sure the selected image doesn't change when a hidden folder is selected
            if (((TiFileSystemObject)lstDirectories.SelectedNode.Tag).Attributes.HasFlag(FileAttributes.Hidden))
            {
                lstDirectories.SelectedImageKey = "folder (Hidden)";
            }
            else
            {
                lstDirectories.SelectedImageKey = "folder";
            }

            var fileCount = 0ul;
            var dirSize = 0ul;
            PopulateListView((TiDirectory)e.Node.Tag);

            foreach (ListViewItem lvi in currentFolderView)
            {
                var entry = (TiFileSystemObject)lvi.Tag;
                if (!(entry is TiDirectory))
                {
                    fileCount++;
                    dirSize += entry.Length;
                }
            }
            
            UpdateStatusBar(false);

            if (lstDirectories.SelectedNode == null)
            {
                extractToolStripMenuItem1.Enabled = false;
                newFolderToolStripMenuItem1.Enabled = false;
                renameToolStripMenuItem1.Enabled = false;
                propertiesToolStripMenuItem1.Enabled = false;
                undeleteToolStripMenuItem1.Enabled = false;
                deleteToolStripMenuItem1.Enabled = false;
            }
            else
            {
                if (lstDirectories.SelectedNode == lstDirectories.Nodes[0])
                {
                    deleteToolStripMenuItem1.Enabled = false;
                    renameToolStripMenuItem1.Enabled = false;
                    propertiesToolStripMenuItem1.Enabled = false;
                    undeleteToolStripMenuItem1.Enabled = false;
                    newFolderToolStripMenuItem1.Enabled = true;
                    extractToolStripMenuItem1.Enabled = true;
                }
                else
                {
                    TiFileSystemObject entry = (TiFileSystemObject)lstDirectories.SelectedNode.Tag;
                    extractToolStripMenuItem1.Enabled = !entry.Name.StartsWith("?");
                    newFolderToolStripMenuItem1.Enabled = !entry.Name.StartsWith("?");
                    renameToolStripMenuItem1.Enabled = !entry.Name.StartsWith("?");
                    propertiesToolStripMenuItem1.Enabled = true;
                    deleteToolStripMenuItem1.Enabled = !entry.Name.StartsWith("?");
                    undeleteToolStripMenuItem1.Enabled = entry.Name.StartsWith("?");
                }
            }
        }

        private void lstFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstFiles.SelectedIndices.Count == 1 && e.Button != MouseButtons.Right)
            {
                if (GetSelectedItemData(0) is TiDirectory dir) //A folder was double-clicked
                {
                    var node = FindNode(lstDirectories.Nodes[0], dir);
                    if (node != null)
                    {
                        lstDirectories.SelectedNode = node;
                    }
                    else
                    {
                        throw new Exception("Associated treeview node was not found");
                    }
                }
                else //A file was double-clicked
                {
                    throw new NotImplementedException("This feature is not implemented yet");
                }
            }
        }

        private void cmsDirTree_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lstDirectories.Nodes.Count == 0)
            {
                e.Cancel = true;
                return;
            }
        }

        private void selectAll_Click(object sender, EventArgs e)
        {
            lstFiles.Focus();
            lstFiles.SelectAllItems();
        }

        private void lstFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            /*else if (e.Data.GetDataPresent(typeof(ListViewItem)) || e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;*/
            else
                e.Effect = DragDropEffects.None;
        }

        //Opens an image that's been dragged and dropped onto the file list
        //TODO: Implement item movement for ListViewItem and TreeNode drag-n-drop
        private void lstFiles_DragDrop(object sender, DragEventArgs e)
        {
            /*if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                //A file or folder is being moved within the listview
                throw new NotImplementedException("This feature is not implemented yet");
            }
            else if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                //A folder is being moved from the treeview to the listview. First needs to check if such a move is even legal;
                 //as this could potentially allow the user to move a parent folder into its own subfolder...
                throw new NotImplementedException("This feature is not implemented yet");
            }
            else */
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //Files are being dragged into the listview from outside the form
                string[] items = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                if (string.IsNullOrWhiteSpace(filename) && unsavedChanges == false) //No image is loaded
                {
                    if (items.Length == 1)
                    {
                        filepath = items[0];
                        OpenImage(filepath);
                    }
                    else //We don't support this yet - I suppose we should offer to create a new image first?
                    {
                        throw new NotImplementedException("This feature is not implemented yet");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(filename) || unsavedChanges) //An image is open (either saved or new)
                {
                    //Inject files/folder instead
                    throw new NotImplementedException("This feature is not implemented yet");
                }
            }
        }

        private void lstDirectories_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            /*else if (e.Data.GetDataPresent(typeof(ListViewItem)) || e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;*/
            else
                e.Effect = DragDropEffects.None;
        }

        //Opens an image that's been dragged and dropped onto the dir tree
        //TODO: Implement item movement for ListViewItem and TreeNode drag-n-drop
        private void lstDirectories_DragDrop(object sender, DragEventArgs e)
        {
            /*if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                //A file or folder is being moved from the listview to the treeview
                throw new NotImplementedException("This feature is not implemented yet");
            }
            else if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                //A folder is being moved within the treeview. First needs to check if such a move is even legal;
                //as this could potentially allow the user to move a parent folder into its own subfolder...
                throw new NotImplementedException("This feature is not implemented yet");
            }
            else */
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] items = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                if (string.IsNullOrWhiteSpace(filename) && unsavedChanges == false) //No image is loaded
                {
                    if (items.Length == 1)
                    {
                        filepath = items[0];
                        OpenImage(filepath);
                    }
                    else //We don't support this yet - I suppose we should offer to create a new image first?
                    {
                        throw new NotImplementedException("This feature is not implemented yet");
                    }
                }
            }
        }

        private void expandDirectoryTree_Click(object sender, EventArgs e)
        {
            lstDirectories.ExpandAll();
        }

        private void collapseDirectoryTree_Click(object sender, EventArgs e)
        {
            lstDirectories.CollapseAll();
        }

        private void lstDirectories_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                lstDirectories.SelectedNode = lstDirectories.GetNodeAt(e.X, e.Y);
                cmsDirTree.Show(lstDirectories, e.Location);
            }
        }

        // Used for the following two events.
        private void ResetView()
        {
            if (image != null)
            {
                lastViewedDir = (TiDirectory)lstDirectories.SelectedNode.Tag;

                var root = new TreeNode(@"\");
                root.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
                root.SelectedImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
                root.Tag = image.PartitionTable.Partitions[0].FileSystem.RootDirectory;

                lstDirectories.BeginUpdate();
                PopulateTreeView(root, image.PartitionTable.Partitions[0].FileSystem.RootDirectory);

                lstDirectories.Nodes.Clear();
                lstDirectories.Nodes.Add(root);
                lstDirectories.Sort();
                lstDirectories.EndUpdate();

                if (lastViewedDir != null)
                {
                    TreeNode? node = FindNode(lstDirectories.Nodes[0], lastViewedDir);
                    if (node == null)
                        lstDirectories.SelectedNode = lstDirectories.Nodes[0];
                    else
                        lstDirectories.SelectedNode = node;
                }
                else
                {
                    lstDirectories.SelectedNode = lstDirectories.Nodes[0];
                }

                PopulateListView((TiDirectory)lstDirectories.SelectedNode.Tag);
            }
        }

        private void showHiddenItems_Click(object sender, EventArgs e)
        {
            Settings.CurrentSettings.ShowHiddenItems = !Settings.CurrentSettings.ShowHiddenItems;

            showHiddenItemsToolStripMenuItem.Checked = Settings.CurrentSettings.ShowHiddenItems;
            showHiddenItemsToolStripMenuItem1.Checked = Settings.CurrentSettings.ShowHiddenItems;

            ResetView();
        }

        private void showDeletedItems_Click(object sender, EventArgs e)
        {
            Settings.CurrentSettings.ShowDeletedItems = !Settings.CurrentSettings.ShowDeletedItems;

            showDeletedItemsToolStripMenuItem.Checked = Settings.CurrentSettings.ShowDeletedItems;
            showDeletedItemsToolStripMenuItem1.Checked = Settings.CurrentSettings.ShowDeletedItems;

            ResetView();
        }

        private void lstFiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void lstDirectories_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (unsavedChanges)
            {
                DialogResult = MessageBox.Show("You have unsaved changes in the current image. Do you want to save them before closing TotalImage?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (DialogResult == DialogResult.Yes)
                {
                    /* Save changes */
                }
                else if (DialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            Settings.Save();
        }

        private void viewMenu_DropDownOpening(object sender, EventArgs e)
        {
            nameToolStripMenuItem.Checked = typeToolStripMenuItem.Checked = sizeToolStripMenuItem.Checked =
                modifiedToolStripMenuItem.Checked = false;
            switch (sortColumn)
            {
                case 0: nameToolStripMenuItem.Checked = true; break;
                case 1: typeToolStripMenuItem.Checked = true; break;
                case 2: sizeToolStripMenuItem.Checked = true; break;
                case 3: modifiedToolStripMenuItem.Checked = true; break;
            }

            largeIconsToolStripMenuItem.Checked = smallIconsToolStripMenuItem.Checked = detailsToolStripMenuItem.Checked = listToolStripMenuItem.Checked = false;
            switch (Settings.CurrentSettings.FilesView)
            {
                case View.LargeIcon: largeIconsToolStripMenuItem.Checked = true; break;
                case View.SmallIcon: smallIconsToolStripMenuItem.Checked = true; break;
                case View.Details: detailsToolStripMenuItem.Checked = true; break;
                case View.List: listToolStripMenuItem.Checked = true; break;
            }

            commandBarToolStripMenuItem.Checked = commandBar.Visible;
            directoryTreeToolStripMenuItem.Checked = !splitContainer.Panel1Collapsed;
            statusBarToolStripMenuItem.Checked = statusBar.Visible;

            showHiddenItemsToolStripMenuItem.Checked = Settings.CurrentSettings.ShowHiddenItems;
            showDeletedItemsToolStripMenuItem.Checked = Settings.CurrentSettings.ShowDeletedItems;
        }

        private void cmsToolbars_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            commandBarToolStripMenuItem1.Checked = commandBar.Visible;
            directoryTreeToolStripMenuItem1.Checked = !splitContainer.Panel1Collapsed;
            statusBarToolStripMenuItem1.Checked = statusBar.Visible;
        }

        private void viewToolStripButton_DropDownOpening(object sender, EventArgs e)
        {
            largeIconsToolStripMenuItem1.Checked = smallIconsToolStripMenuItem1.Checked = detailsToolStripMenuItem1.Checked = listToolStripMenuItem1.Checked = false;
            switch (Settings.CurrentSettings.FilesView)
            {
                case View.LargeIcon: largeIconsToolStripMenuItem1.Checked = true; break;
                case View.SmallIcon: smallIconsToolStripMenuItem1.Checked = true; break;
                case View.Details: detailsToolStripMenuItem1.Checked = true; break;
                case View.List: listToolStripMenuItem1.Checked = true; break;
            }

            showHiddenItemsToolStripMenuItem1.Checked = Settings.CurrentSettings.ShowHiddenItems;
            showDeletedItemsToolStripMenuItem1.Checked = Settings.CurrentSettings.ShowDeletedItems;
        }

        private void sortMenu_DropDownOpening(object sender, EventArgs e)
        {
            nameToolStripMenuItem1.Checked = typeToolStripMenuItem1.Checked = sizeToolStripMenuItem1.Checked =
                modifiedToolStripMenuItem1.Checked = false;
            switch (sortColumn)
            {
                case 0: nameToolStripMenuItem1.Checked = true; break;
                case 1: typeToolStripMenuItem1.Checked = true; break;
                case 2: sizeToolStripMenuItem1.Checked = true; break;
                case 3: modifiedToolStripMenuItem1.Checked = true; break;
            }
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Settings.CurrentSettings.SplitterDistance = splitContainer.SplitterDistance;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                Settings.CurrentSettings.WindowState = FormWindowState.Maximized;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Settings.CurrentSettings.WindowState = FormWindowState.Normal;
                Settings.CurrentSettings.WindowSize = this.Size;
            }
        }

        private void frmMain_Move(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                Settings.CurrentSettings.WindowPosition = this.Location;
            }
        }

        private void selectPartitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgSelectPartition dlg = new dlgSelectPartition()
            {
                PartitionTable = image.PartitionTable
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadPartitionInCurrentImage(dlg.SelectedEntry);
            }
        }

        private void selectPartitionToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectPartitionToolStripComboBox.SelectedIndex != CurrentPartitionIndex)
            {
                LoadPartitionInCurrentImage(selectPartitionToolStripComboBox.SelectedIndex);
                CurrentPartitionIndex = selectPartitionToolStripComboBox.SelectedIndex;
            }
        }

        private void lstFiles_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && lstDirectories.SelectedNode.Parent != null)
            {
                lstDirectories.SelectedNode = lstDirectories.SelectedNode.Parent;
            }
            else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                if (lstFiles.SelectedIndices.Count == 1)
                {
                    TiFileSystemObject fso = GetSelectedItemData(0);
                    if (fso is TiDirectory)
                    {
                        var node = FindNode(lstDirectories.Nodes[0], (TiDirectory)fso);
                        if (node != null)
                        {
                            lstDirectories.SelectedNode = node;
                        }
                        else
                        {
                            throw new Exception("Associated treeview node was not found");
                        }
                    }
                    else
                    {
                        //Extract the selected file (and open it?)
                    }
                }
                else if (lstFiles.SelectedIndices.Count > 1)
                {
                    //Extract all selected objects
                }
            }
        }

#endregion

        private void PopulateTreeView(TreeNode node, TiDirectory dir)
        {
            foreach (var subdir in dir.EnumerateDirectories(Settings.CurrentSettings.ShowHiddenItems, Settings.CurrentSettings.ShowDeletedItems))
            {
                var subnode = new TreeNode(subdir.Name);

                //Hidden folders have a 50% opacity for the icon
                if (subdir.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    subnode.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder (Hidden)");
                    subnode.ForeColor = Color.Gray;
                }
                else
                {
                    subnode.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
                }

                //Deleted folders have strikthrough fontstyle
                if (subdir.Name.StartsWith("?"))
                {
                    Font font = new Font("Segoe UI", 9f, FontStyle.Strikeout);
                    subnode.NodeFont = font;
                }
                subnode.Tag = subdir;
                node.Nodes.Add(subnode);

                PopulateTreeView(subnode, subdir);
            }
        }

        private Bitmap CreateHiddenIcon(Bitmap normalIcon)
        {
            ColorMatrix cm = new ColorMatrix();
            cm.Matrix33 = 0.65f; //65% opacity
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            Point[] points = { new Point(0, 0),
                               new Point(normalIcon.Width, 0),
                               new Point(0, normalIcon.Height),
                             };
            Rectangle rect = new Rectangle(0, 0, normalIcon.Width, normalIcon.Height);

            Bitmap bmp = new Bitmap(normalIcon.Width, normalIcon.Height);
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(normalIcon, points, rect, GraphicsUnit.Pixel, attributes);
            }

            return bmp;
        }

        private void PopulateListView(TiDirectory dir)
        {
            lstFiles.BeginUpdate();
            lstFiles.SelectedIndices.Clear();
            currentFolderView.Clear();

            upOneFolderListViewItem.Tag = dir.Parent;

            var count = 0;
            if (dir.Parent != null) count++;

            foreach (var fso in dir.EnumerateFileSystemObjects(Settings.CurrentSettings.ShowHiddenItems, Settings.CurrentSettings.ShowDeletedItems))
            {
                var item = new ListViewItem();
                item.Text = fso.Name;
                item.SubItems.Add(GetFileTypeName(fso.Name, fso.Attributes));
                if (fso is TiDirectory)
                    item.SubItems.Add(string.Empty);
                else
                    item.SubItems.Add(Settings.CurrentSettings.SizeUnit.FormatSize(fso.Length));

                item.ImageIndex = GetFileTypeIconIndex(fso.Name, fso.Attributes);

                item.SubItems.Add(fso.LastWriteTime.ToString());
                item.SubItems.Add(FileAttributesToString(fso.Attributes));
                item.UseItemStyleForSubItems = false;
                item.SubItems[4].Font = new Font(FontFamily.GenericMonospace, 9);

                //Do some simple styling for hidden and deleted items
                if (fso.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    item.ForeColor = Color.Gray;
                    item.SubItems[1].ForeColor = Color.Gray;
                    item.SubItems[2].ForeColor = Color.Gray;
                    item.SubItems[3].ForeColor = Color.Gray;
                }
                if (fso.Name.StartsWith("?"))
                {
                    Font sfont = new Font("Segoe UI", 9f, FontStyle.Strikeout);
                    item.UseItemStyleForSubItems = false;
                    item.Font = sfont;
                }

                item.Tag = fso;
                currentFolderView.Add(item);
                count++;
            }

            SortListView();
            lstFiles.VirtualListSize = count;

            lstFiles.EndUpdate();
        }

        private string FileAttributesToString(FileAttributes attr)
        {
            var sb = new StringBuilder();

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

            return sb.ToString();
        }

        //TODO: This needs some serious rethinking and probably restructuring.
        private void OpenImage(string path)
        {
            filepath = path;
            filename = Path.GetFileName(path);
            Text = $"{filename} - TotalImage";

            var ext = Path.GetExtension(filename).ToLowerInvariant();
            switch (ext)
            {
                case ".vhd":
                    image = new VhdContainer(path);
                    break;
                default:
                    image = new RawContainer(path);
                    break;
            }

            CurrentPartitionIndex = 0;
            if (image.PartitionTable.Partitions.Count == 0)
            {
                MessageBox.Show("There are no partitions in the selected image", "Error loading HDD image", MessageBoxButtons.OK);
                return;
            }
            else if (image.PartitionTable.Partitions.Count > 1)
            {
                dlgSelectPartition selectFrm = new dlgSelectPartition()
                {
                    PartitionTable = image.PartitionTable
                };

                if (selectFrm.ShowDialog() == DialogResult.Cancel)
                {
                    CloseImage();
                    return;
                }

                CurrentPartitionIndex = selectFrm.SelectedEntry;
                selectPartitionToolStripComboBox.Items.Clear();

                if (image.PartitionTable.Partitions.Count > 1)
                {
                    for (int i = 0; i < image.PartitionTable.Partitions.Count; i++)
                    {
                        try
                        {
                            selectPartitionToolStripComboBox.Items.Add($"{i}: {image.PartitionTable.Partitions[i].FileSystem.VolumeLabel.TrimEnd(' ')} ({image.PartitionTable.Partitions[i].FileSystem.DisplayName}, {Settings.CurrentSettings.SizeUnit.FormatSize((ulong)image.PartitionTable.Partitions[i].Length)})");
                        }
                        catch (InvalidDataException)
                        {
                        }

                        if (i == CurrentPartitionIndex)
                        {
                            selectPartitionToolStripComboBox.SelectedIndex = i;
                        }
                    }
                }
            }

            LoadPartitionInCurrentImage(CurrentPartitionIndex);
        }

        private void LoadPartitionInCurrentImage(int index)
        {
            if (image == null)
            {
                return;
            }

            var root = new TreeNode(@"\");
            root.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
            root.SelectedImageIndex = imgFilesSmall.Images.IndexOfKey("folder");

            root.Tag = image.PartitionTable.Partitions[index].FileSystem.RootDirectory;
            PopulateTreeView(root, image.PartitionTable.Partitions[index].FileSystem.RootDirectory);

            lstDirectories.BeginUpdate();
            lstDirectories.Nodes.Clear();
            lstDirectories.Nodes.Add(root);
            lstDirectories.Sort();
            lstDirectories.EndUpdate();
            lstDirectories.SelectedNode = lstDirectories.Nodes[0];

            PopulateListView(image.PartitionTable.Partitions[index].FileSystem.RootDirectory);

            EnableUI();
            UpdateStatusBar(true);

            Settings.AddRecentImage(filepath);
            PopulateRecentList();
        }

        /* Returns size of directory
         * TODO: Move to this to the appropriate file system class and implement support for subdirectories */
        private ulong CalculateDirSize()
        {
            var dirSize = 0ul;

            foreach (ListViewItem lvi in currentFolderView)
            {
                TiFileSystemObject entry = (TiFileSystemObject)lvi.Tag;
                if (!(entry is TiDirectory))
                {
                    dirSize += entry.Length;
                }
            }

            return dirSize;
        }

        /* Returns the number of files in a directory
         * TODO: Move to this to the appropriate file system class and implement support for subdirectories */
        private uint GetFileCount()
        {
            uint fileCount = 0;

            foreach (ListViewItem lvi in currentFolderView)
            {
                TiFileSystemObject entry = (TiFileSystemObject)lvi.Tag;
                if (!(entry is TiDirectory))
                {
                    fileCount++;
                }
            }

            return fileCount;
        }

        private string GetFileTypeName(string filename, FileAttributes attributes)
        {
            var extension = attributes.HasFlag(FileAttributes.Directory) ? "folder" : Path.GetExtension(filename);

            if (Settings.CurrentSettings.QueryShellForFileTypeInfo)
            {
                if (!fileTypes.ContainsKey(extension))
                    fileTypes.Add(extension, GetShellFileTypeInfo(filename, attributes));

                return fileTypes[extension].name;
            }
            else
            {
                if (attributes.HasFlag(FileAttributes.Directory))
                    return "File folder";
                else if (extension.Length > 0)
                    return $"{extension.Substring(1).ToUpper()} File";
                else
                    return "File";
            }
        }

        private int GetFileTypeIconIndex(string filename, FileAttributes attributes)
        {
            string key;
            if (Settings.CurrentSettings.QueryShellForFileTypeInfo)
            {
                var extension = attributes.HasFlag(FileAttributes.Directory) ? "folder" : Path.GetExtension(filename);

                if (!fileTypes.ContainsKey(extension))
                    fileTypes.Add(extension, GetShellFileTypeInfo(filename, attributes));

                key = fileTypes[extension].iconIndex.ToString();

                if (!imgFilesSmall.Images.ContainsKey(key))
                {
                    (ImageList, Icon)[] icons =
                    {
                        (imgFilesSmall, GetSystemIcon(fileTypes[extension].iconIndex, false)),
                        (imgFilesLarge, GetSystemIcon(fileTypes[extension].iconIndex, true))
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

        //Obtains the icon for the file type
        private static Icon GetSystemIcon(int index, bool large)
        {
            IImageList list;
            var iid = new Guid(IID_IImageList);
            SHGetImageList(large ? SHIL.LARGE : SHIL.SMALL, ref iid, out list);

            IntPtr hIcon;
            list.GetIcon(index, ILD.TRANSPARENT, out hIcon);

            Icon icon = (Icon)Icon.FromHandle(hIcon).Clone();
            DestroyIcon(hIcon);
            return icon;
        }

        private static (string name, int iconIndex) GetShellFileTypeInfo(string filename, FileAttributes attributes)
        {
            var shinfo = new SHFILEINFO();
            var flags = SHGFI.TYPENAME | SHGFI.SYSICONINDEX | SHGFI.USEFILEATTRIBUTES;

            SHGetFileInfo(filename, attributes, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);

            return (shinfo.szTypeName, shinfo.iIcon);
        }

        //Gets the default Windows folder icon and type name with SHGetFileInfo
        private void GetDefaultIcons()
        {
            // Folder and file icons
            (string, FileAttributes)[] types =
            {
                ("folder", FileAttributes.Directory),
                ("file", 0)
            };

            foreach(var (key, attributes) in types)
            {
                var (_, index) = GetShellFileTypeInfo(key, attributes);

                (ImageList, Icon)[] icons =
                {
                    (imgFilesSmall, GetSystemIcon(index, false)),
                    (imgFilesLarge, GetSystemIcon(index, true))
                };

                foreach (var (list, icon) in icons)
                {
                    list.Images.Add(key, icon);
                    list.Images.Add($"{key} (Hidden)", CreateHiddenIcon(icon.ToBitmap()));
                }
            }

            // "Up one folder" icon
            var largeIcons = new IntPtr[1];
            var smallIcons = new IntPtr[1];

            ExtractIconEx("shell32.dll", 45, largeIcons, smallIcons, 1);

            imgFilesSmall.Images.Add("up", (Icon)Icon.FromHandle(smallIcons[0]).Clone());
            imgFilesLarge.Images.Add("up", (Icon)Icon.FromHandle(largeIcons[0]).Clone());

            upOneFolderListViewItem.ImageIndex = imgFilesSmall.Images.IndexOfKey("up");
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
                        if (nodeChild != null)
                        {
                            return nodeChild;
                        }
                    }
                }

            return null;
        }

        //Enables various UI elements after an image is loaded
        public void EnableUI()
        {
            closeToolStripButton.Enabled = true;
            injectToolStripButton.Enabled = true;
            newFolderToolStripButton.Enabled = true;
            labelToolStripMenuButton.Enabled = true;
            bootsectToolStripButton.Enabled = true;
            infoToolStripButton.Enabled = true;
            saveAsToolStripMenuItem.Enabled = true;
            closeImageToolStripMenuItem.Enabled = true;
            lblStatusProgressBar.Visible = true;

            // Change border sides for status bar children to add seperator-like looks.
            lblStatusCapacity.BorderSides = ToolStripStatusLabelBorderSides.Right;
            lblStatusSize.BorderSides = ToolStripStatusLabelBorderSides.Left;

            //New image was created, enable the Save button to act as Save as
            if (unsavedChanges && string.IsNullOrEmpty(filename))
                saveToolStripButton.Enabled = true;

            imageInformationToolStripMenuItem.Enabled = true;
            hexViewToolStripMenuItem.Enabled = true;

            foreach (ToolStripItem item in editToolStripMenuItem.DropDownItems)
            {
                if (item.CanSelect)
                {
                    item.Enabled = true;
                }
            }

            //Enabling this now since we have rudimentary HDD support.
            if (image.PartitionTable.Partitions.Count > 1)
            {
                managePartitionsToolStripMenuItem.Enabled = true;
                selectPartitionToolStripMenuItem.Enabled = true;
                managePartitionsToolStripButton.Enabled = true;
                selectPartitionToolStripComboBox.Enabled = true;
            }
            else
            {
                managePartitionsToolStripMenuItem.Enabled = true;
                selectPartitionToolStripMenuItem.Enabled = false;
                managePartitionsToolStripButton.Enabled = true;
                selectPartitionToolStripComboBox.Enabled = false;
            }
        }

        //Disables various UI elements after an image is loaded
        private void DisableUI()
        {
            closeToolStripButton.Enabled = false;
            injectToolStripButton.Enabled = false;
            extractToolStripButton.Enabled = false;
            deleteToolStripButton.Enabled = false;
            propertiesToolStripButton.Enabled = false;
            newFolderToolStripButton.Enabled = false;
            labelToolStripMenuButton.Enabled = false;
            bootsectToolStripButton.Enabled = false;
            infoToolStripButton.Enabled = false;
            saveToolStripButton.Enabled = false;
            managePartitionsToolStripButton.Enabled = false;
            selectPartitionToolStripComboBox.Enabled = false;
            managePartitionsToolStripMenuItem.Enabled = false;
            selectPartitionToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            closeImageToolStripMenuItem.Enabled = false;
            imageInformationToolStripMenuItem.Enabled = false;
            hexViewToolStripMenuItem.Enabled = false;
            lblStatusProgressBar.Visible = false;

            // Change border sides for status bar children to remove seperator-like looks.
            lblStatusCapacity.BorderSides = ToolStripStatusLabelBorderSides.None;
            lblStatusSize.BorderSides = ToolStripStatusLabelBorderSides.None;

            foreach (ToolStripItem item in editToolStripMenuItem.DropDownItems)
            {
                if (item.CanSelect)
                {
                    item.Enabled = false;
                }
            }
        }

        private void PopulateRecentList()
        {
            recentFilesToolStripMenuItem.DropDownItems.Clear();

            for (int i = Settings.CurrentSettings.RecentImages.Count - 1; i >= 0; i--)
            {
                ToolStripMenuItem newItem = new ToolStripMenuItem();
                newItem.Text = $"{(Settings.CurrentSettings.RecentImages.Count - i)}: {Settings.CurrentSettings.RecentImages[i]}";
                newItem.Click += recentImage_Click;
                recentFilesToolStripMenuItem.DropDownItems.Add(newItem);
            }

            recentFilesToolStripMenuItem.Enabled = recentFilesToolStripMenuItem.DropDownItems.Count != 0;
        }

        private void CloseImage()
        {
            unsavedChanges = false;
            Text = "TotalImage";
            filename = "";
            filepath = "";
            image?.Dispose();
            image = null;
            lstDirectories.Nodes.Clear();
            currentFolderView.Clear();
            lstFiles.VirtualListSize = 0;
            selectPartitionToolStripComboBox.Items.Clear();
            DisableUI();
            UpdateStatusBar(false);
        }

        private TiFileSystemObject GetSelectedItemData(int idx)
        {
            if (lstFiles.SelectedIndices[idx] < IndexShift)
                return (TiFileSystemObject)upOneFolderListViewItem.Tag;
            return (TiFileSystemObject)currentFolderView[lstFiles.SelectedIndices[idx] - IndexShift].Tag;
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

        private void lblNotifications_ButtonClick(object sender, EventArgs e)
        {
            using dlgNotifications dlg = new dlgNotifications();
            dlg.ShowDialog();
        }
    }
}