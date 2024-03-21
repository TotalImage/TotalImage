using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TotalImage.Containers;
using TotalImage.Containers.NHD;
using TotalImage.Containers.VHD;
using TotalImage.Containers.Anex86;
using TotalImage.FileSystems.BPB;
using TotalImage.FileSystems.FAT;
using TiDirectory = TotalImage.FileSystems.Directory;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage
{
    enum StatusBarState
    {
        NoneSelected,
        OneSelected,
        MultipleSelected
    }

    public partial class frmMain : Form
    {
        public string filename = "";
        public string filepath = "";
        public bool unsavedChanges = false;
        public Container? image;
        public int CurrentPartitionIndex;
        private int sortColumn;
        private SortOrder sortOrder;
        private TiDirectory? lastViewedDir;
        private string? lastSavedFilename;
        private TiDirectory? draggedDir;
        public string tempDir;

        private ListViewItem upOneFolderListViewItem = new()
        {
            Text = "..",
            ToolTipText = "Parent directory"
        };

        private List<ListViewItem> currentFolderView = new();

        public frmMain()
        {
            InitializeComponent();

            //Scale the ImageList images according to current Dpi scale
            Graphics g = CreateGraphics();
            imgFilesSmall.ImageSize = new SizeF(16 * (g.DpiX / 96f), 16 * (g.DpiY / 96f)).ToSize();
            imgFilesLarge.ImageSize = new SizeF(32 * (g.DpiX / 96f), 32 * (g.DpiY / 96f)).ToSize();
        }

        #region Event Handlers
        private void frmMain_Load(object sender, EventArgs e)
        {
            //This fixes the problem of certain settings values not being returned correctly when using high DPI. Go figure...
            Settings.ReloadSettings();
            Settings.ReloadUIState();

            SyncUIOptions();
            SyncWindowState();
            DisableUI();

            GetDefaultIcons();
            lstDirectories.SelectedImageIndex = imgFilesSmall.Images.IndexOfKey("folder");

            //Open the file that was dragged onto the exe/shortcut or passed as a command line argument
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string argPath = args[1];
                OpenImage(argPath);
            }

            for (var i = 1; i < lstFiles.Columns.Count; i++)
                upOneFolderListViewItem.SubItems.Add(string.Empty);
        }

        //Injects a folder into the image
        private void injectFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog fbd = new();
            fbd.ShowNewFolderButton = true;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                if (Settings.CurrentSettings.ConfirmInjection)
                {
                    TaskDialogPage page = new()
                    {
                        Text = $"Are you sure you want to inject this folder into the image?",
                        Heading = $"A folder will be injected",
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
                        Settings.CurrentSettings.ConfirmInjection = false;

                    if (result == TaskDialogButton.No)
                        return;
                }

                throw new NotImplementedException("This feature is not implemented yet");
            }
        }

        //Shows a hex view of the current image
        private void hexView_Click(object sender, EventArgs e)
        {
            using dlgHexView frm = new();
            frm.ShowDialog();
        }

        //Allows viewing and editing both volume labels
        //TODO: Actually change the volume labels
        private void changeVolumeLabel_Click(object sender, EventArgs e)
        {
            if (image?.PartitionTable.Partitions[0].FileSystem is not FatFileSystem fs)
            {
                TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = "This feature is currently only available for FAT partitions.",
                    Heading = "Feature not available",
                    Caption = "Information",
                    Buttons =
                    {
                        TaskDialogButton.OK
                    },
                    Icon = TaskDialogIcon.Information,
                    DefaultButton = TaskDialogButton.OK
                });

                return;
            }

            using dlgChangeVolLabel dlg = new(fs.RootDirectoryVolumeLabel, fs.BpbVolumeLabel);
            dlg.ShowDialog();
        }

        /* Allows viewing and editing bootsector properties
         *
         * TODO: Enable this for other file systems/partition types/media too.
         */
        private void bootSectorProperties_Click(object sender, EventArgs e)
        {
            if (image is not null && image.PartitionTable.Partitions[CurrentPartitionIndex].FileSystem is not FatFileSystem)
            {
                TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = "This feature is currently only available for FAT file systems.",
                    Heading = "Feature not available",
                    Caption = "Information",
                    Buttons =
                    {
                        TaskDialogButton.OK
                    },
                    Icon = TaskDialogIcon.Information,
                    DefaultButton = TaskDialogButton.OK
                });

                return;
            }

            using dlgBootSector dlg = new();
            dlg.ShowDialog();
        }

        //Shows current image information
        private void imageInformation_Click(object sender, EventArgs e)
        {
            using dlgImageInfo dlg = new();
            dlg.ShowDialog();
        }

        //Click event handler for all menu items in the Recent images menu
        private void recentImage_Click(object sender, EventArgs e)
        {
            string imagePath = ((ToolStripMenuItem)sender).Text[3..].Trim(' '); //Remove the leading index in the menuitem text
            if (!File.Exists(imagePath))
            {
                TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = $"The file \"{Path.GetFileName(imagePath)}\" could not be opened because it no longer exists. It may have been renamed, moved or deleted since it was last opened in TotalImage. It will now be removed from your recent images list.{Environment.NewLine}{Environment.NewLine}" +
                    $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                    Heading = "File not found",
                    Caption = "Error",
                    Buttons =
                        {
                            TaskDialogButton.OK
                        },
                    Icon = TaskDialogIcon.Error,
                    SizeToContent = true
                });

                //Remove the non-working entry
                Settings.RemoveRecentImage(imagePath);
                PopulateRecentList();
                return;
            }

            CloseImage();
            OpenImage(imagePath);
        }

        //Creates a new disk image
        //TODO: Implement the "save changes first" code path
        private void newImage_Click(object sender, EventArgs e)
        {
            using dlgNewImage dlg = new();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (unsavedChanges)
                {
                    TaskDialogButton result = TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"Would you like to save them before creating a new image?",
                        Heading = "You have unsaved changes",
                        Caption = "Warning",
                        Buttons =
                        {
                            new  TaskDialogCommandLinkButton("&Save") { Tag = 1 },
                            new TaskDialogCommandLinkButton("&Discard"),
                            TaskDialogButton.Cancel
                        },
                        Icon = TaskDialogIcon.Warning,
                    });

                    if (result.Tag is not null) /* Save changes first... */ ;
                    else if (result == TaskDialogButton.Cancel) return;
                }

                if (image is not null)
                    CloseImage();
                image = null;

                BiosParameterBlock bpb = dlg.BPBVersion == BiosParameterBlockVersion.Dos34 || dlg.BPBVersion == BiosParameterBlockVersion.Dos40
                    ? ExtendedBiosParameterBlock.FromGeometry(dlg.Geometry, dlg.BPBVersion, dlg.OEMID, dlg.SerialNumber, dlg.FileSystemType, dlg.VolumeLabel)
                    : BiosParameterBlock.FromGeometry(dlg.Geometry, dlg.BPBVersion, dlg.OEMID);

                //Create a new image and immediately open it
                image = RawContainer.CreateImage(bpb, dlg.Geometry.Tracks, dlg.WriteBPB);
                unsavedChanges = true;
                OpenImage(null);
            }
        }

        /* The Save button/menu item acts as either:
         * -"Save" when the file is already saved and there are unsaved changes
         * -"Save as" when the file has not been saved yet */
        private void save_Click(object sender, EventArgs e)
        {
            if (image is not null)
            {
                if (string.IsNullOrEmpty(filename) || (ToolStripMenuItem)sender == saveAsToolStripMenuItem) //File hasn't been saved yet
                {
                    saveFileAs();
                }
                else
                {
                    saveFile();
                }
            }
        }

        //Creates a new folder
        private void newFolder_Click(object sender, EventArgs e)
        {
            if (image is not null && image.PartitionTable.Partitions[CurrentPartitionIndex].FileSystem is FatFileSystem)
            {
                var fs = (FatFileSystem)image.PartitionTable.Partitions[CurrentPartitionIndex].FileSystem;
                if (fs.IsSmall)
                {
                    MessageBox.Show("Disks with 16-byte directory entries cannot have subdirectories.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }    
            }
            using dlgNewFolder dlg = new();
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

        //Deletes a file or folder
        //TODO: Implement deletion here and in the FS/container
        private void delete_Click(object sender, EventArgs e)
        {
            if (lstFiles.Focused)
            {
                var selectedSize = 0ul;
                foreach (var entry in SelectedItems) selectedSize += entry.Length;

                if (Settings.CurrentSettings.ConfirmDeletion)
                {
                    TaskDialogPage page = new()
                    {
                        Text = $"Are you sure you want to delete {SelectedItems.Count()} item(s) occupying {Settings.CurrentSettings.SizeUnit.FormatSize(selectedSize)}?{Environment.NewLine}" +
                        $"You might still be able to undo this operation later.",
                        Heading = $"{SelectedItems.Count()} item(s) will be deleted",
                        Caption = "Deletion",
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
                        Settings.CurrentSettings.ConfirmDeletion = false;

                    if (result == TaskDialogButton.No)
                        return;
                }

                throw new NotImplementedException("This feature is not implemented yet");
            }
            else if (lstDirectories.Focused)
            {
                if (Settings.CurrentSettings.ConfirmDeletion)
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
                    TaskDialogButton result = TaskDialog.ShowDialog(this, page);

                    if (page.Verification.Checked)
                        Settings.CurrentSettings.ConfirmDeletion = false;

                    if (result == TaskDialogButton.No)
                        return;
                }

                throw new NotImplementedException("This feature is not implemented yet");
            }
        }

        //Undeletes a delete file or folder
        //TODO: Implement this here and in FS/container. Although checks are already performed in menu item code to disable the option entirely
        //when it's not applicable, some additional checks here probably wouldn't hurt either...
        private void undelete_Click(object sender, EventArgs e)
        {
            using dlgUndelete dlg = new();
            dlg.ShowDialog();
        }

        //Renames a file or folder
        //TODO: Implement this here and in FS/container.
        private void rename_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();

            /* Below is old code that used the Rename dialog. However, I now think it's more intuitive if we use the ListView LabelEdit events
             * instead. Example:
             * currentFolderView[lstFiles.SelectedIndices[0]].BeginEdit(); */


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
            using dlgDefragment dlg = new();
            dlg.ShowDialog();
        }

        //Formats the selected partition/floppy disk
        //TODO: Implement this here and in FS/container.
        private void format_Click(object sender, EventArgs e)
        {
            using dlgFormat dlg = new();
            if (dlg.ShowDialog() == DialogResult.Yes)
            {
                throw new NotImplementedException();
                // Need to figure out how to actually do this, because right now it's unclear...
                // DoSomeFormatThing();
            }
        }

        //Save the changes made to the current image since the last save or since it was opened
        //TODO: Perhaps this needs some rethinking too, depending on recent changes to the container?
        private void saveFile()
        {
            if (image is null)
            {
                throw new Exception("No image is currently loaded");
            }

            image.SaveImage(filepath);
            OpenImage(filepath); //Reload the image

            /*saveToolStripButton.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            Text = $"{filename} - TotalImage";
            unsavedChanges = false;*/
        }

        //Saves the current image as a new file, along with any changes made to it since the last save
        private bool saveFileAs()
        {
            if (image is null)
            {
                throw new Exception("No image is currently loaded");
            }

            using SaveFileDialog sfd = new();
            sfd.AutoUpgradeEnabled = true;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            sfd.OverwritePrompt = true;
            sfd.DefaultExt = "img";
            sfd.Filter =
                "Raw sector image (*.img, *.ima, *.vfd, *.flp, *.dsk, *.xdf, *.hdm)|*.img;*.ima;*.vfd;*.flp;*.dsk;*.xdf;*.hdm|" +
                "All files (*.*)|*.*";

            if (lastSavedFilename is not null)
            {
                string nameNoExt = Path.GetFileNameWithoutExtension(lastSavedFilename);
                string number = System.Text.RegularExpressions.Regex.Match(nameNoExt, @"\d+$").Value;
                string prefix = nameNoExt[..nameNoExt.LastIndexOf(number)];
                int i = int.Parse(number) + 1;
                string newFilename = prefix + i.ToString(new string('0', number.Length));
                sfd.FileName = newFilename;
            }

            DialogResult result = sfd.ShowDialog();

            if (result == DialogResult.OK)
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

                    if (System.Text.RegularExpressions.Regex.Match(Path.GetFileNameWithoutExtension(sfd.FileName), @"\d+$").Success && Settings.CurrentSettings.AutoIncrementFilename)
                    {
                        lastSavedFilename = Path.GetFileName(sfd.FileName);
                    }
                    else
                    {
                        lastSavedFilename = null;
                    }

                    filepath = sfd.FileName;
                    filename = Path.GetFileName(filepath);
                    Text = $"{filename} - TotalImage";

                    Settings.AddRecentImage(filepath);
                    PopulateRecentList();
                    unsavedChanges = false;
                    saveToolStripButton.Enabled = false;
                }

                return true;
            }
            else if (result == DialogResult.Cancel)
            {
                return false;
            }

            return false;
        }

        //Closes the application
        private void exit_Click(object sender, EventArgs e)
        {
            if (unsavedChanges)
            {
                TaskDialogButton result = TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = $"Would you like to save them before closing TotalImage?",
                    Heading = "You have unsaved changes",
                    Caption = "Warning",
                    Buttons =
                        {
                            new  TaskDialogCommandLinkButton("&Save") { Tag = 1 },
                            new TaskDialogCommandLinkButton("&Discard"),
                            TaskDialogButton.Cancel
                        },
                    Icon = TaskDialogIcon.Warning,
                });

                if (result.Tag is not null)
                {
                    if (string.IsNullOrEmpty(filename)) //File hasn't been saved yet
                    {
                        if (!saveFileAs())
                        {
                            return;
                        }
                    }
                    else
                    {
                        saveFile();
                    }
                }
                else if (result == TaskDialogButton.Cancel) return;
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
                TaskDialogButton result = TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = $"Would you like to save them before opening another image?",
                    Heading = "You have unsaved changes",
                    Caption = "Warning",
                    Buttons =
                        {
                            new  TaskDialogCommandLinkButton("&Save") { Tag = 1 },
                            new TaskDialogCommandLinkButton("&Discard"),
                            TaskDialogButton.Cancel
                        },
                    Icon = TaskDialogIcon.Warning,
                });

                if (result.Tag is not null) save_Click(result, e);
                else if (result == TaskDialogButton.Cancel) return;
            }

            using OpenFileDialog ofd = new();
            ofd.AutoUpgradeEnabled = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;
            //We probably want this, but it degrades the dialog appearance to XP dialog... Some workaround for this would be nice.
            //ofd.ShowReadOnly = true;
            ofd.Filter =
                "All supported containers (*.*)|*.nhd;*.vhd;*.iso;*.imz;*.fdi;*.hdi;*.img;*.ima;*.vfd;*.flp;*.dsk;*.hdm;*.288;*.144;*.12;*.720;*.360;*.json|" +
                "Plain sector image (*.img,*.ima,*.vfd,*.flp,*.dsk,*.hdm,*.288,*.144,*.12,*.720,*.360,*.json)|*.img;*.ima;*.vfd;*.flp;*.dsk;*.hdm;*.288;*.144;*.12;*.720;*.360;*.json|" +
                "WinImage compressed image (*.imz)|*.imz|" +
                "Anex86 disk image (*.fdi,*.hdi)|*.fdi;*.hdi|" +
                "ISO image (*.iso)|*.iso|" +
                "Microsoft VHD (*.vhd)|*.vhd|" +
                "T98-Next HD (*.nhd)|*.nhd|" +
                "PCjs disk images (*.json)|*.json|" +
                "All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CloseImage();
                OpenImage(ofd.FileName);
            }
        }

        private void about_Click(object sender, EventArgs e)
        {
            using dlgAbout dlg = new();
            dlg.ShowDialog();
        }

        //Extracts file(s) or folder(s) from the image to the specified path.
        private void extract_Click(object sender, EventArgs e)
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

            if (Settings.CurrentSettings.ExtractAlwaysAsk)
            {
                using dlgExtract dlg = new();
                dlg.lblPath.Text = $"Extract {(lstDirectories.Focused ? "1" : SelectedItems.Count())} selected {(SelectedItems.Count() > 1 ? "items" : "item")} to the following folder:";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Settings.CurrentSettings.DefaultExtractPath = dlg.TargetPath;

                    if (lstFiles.Focused)
                    {
                        FileExtraction.ExtractFiles(this, SelectedItems, dlg.TargetPath, dlg.DirectoryExtractionMode, dlg.OpenFolder);
                    }
                    else if (lstDirectories.Focused)
                    {
                        FileExtraction.ExtractFiles(this, new[] { (TiFileSystemObject)lstDirectories.SelectedNode.Tag }, dlg.TargetPath, dlg.DirectoryExtractionMode, dlg.OpenFolder);
                    }
                }
            }
            else
            {
                if (lstFiles.Focused)
                {
                    FileExtraction.ExtractFiles(this, SelectedItems);
                }
                else if (lstDirectories.Focused)
                {
                    FileExtraction.ExtractFiles(this, new[] { (TiFileSystemObject)lstDirectories.SelectedNode.Tag });
                }
            }
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e) // This method will be used more than once, thus it is separated from the main event.
        {
            if (image is not null)
            {
                if (lstFiles.SelectedIndices.Count == 0)
                {
                    deleteToolStripButton.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
                    extractToolStripButton.Enabled = true;
                    propertiesToolStripButton.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];

                    UpdateStatusBar(false);
                }
                else if (lstFiles.SelectedIndices.Count == 1 && lstFiles.SelectedIndices[0] < IndexShift)
                {
                    deleteToolStripButton.Enabled = false;
                    extractToolStripButton.Enabled = false;
                    propertiesToolStripButton.Enabled = false;

                    UpdateStatusBar(false);
                }
                else if (lstFiles.SelectedIndices.Count == 1)
                {
                    //Check if selected item is a deleted entry and enable the UI accordingly
                    TiFileSystemObject entry = GetSelectedItemData(0);
                    deleteToolStripButton.Enabled = !entry.Name.StartsWith("?");
                    extractToolStripButton.Enabled = !entry.Name.StartsWith("?");
                    propertiesToolStripButton.Enabled = true;

                    UpdateStatusBar(false);
                }
                else
                {
                    deleteToolStripButton.Enabled = true;
                    extractToolStripButton.Enabled = true;
                    propertiesToolStripButton.Enabled = true;

                    var path = lstDirectories.SelectedNode.FullPath;
                    if (!path.EndsWith(lstDirectories.PathSeparator))
                        path += lstDirectories.PathSeparator;

                    UpdateStatusBar(false);
                }
            }
        }

        private void cmsFileList_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (image is null)
            {
                e.Cancel = true;
                return;
            }

            newFolderToolStripMenuItem2.Enabled = true;
            extractToolStripMenuItem2.Enabled = true;

            if (lstFiles.SelectedIndices.Count == 0)
            {
                deleteToolStripMenuItem2.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
                propertiesToolStripMenuItem2.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
                renameToolStripMenuItem2.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
                undeleteToolStripMenuItem2.Enabled = false;
            }
            else if (lstFiles.SelectedIndices.Count == 1 && lstFiles.SelectedIndices[0] < IndexShift)
            {
                e.Cancel = true;
                return;
            }
            else if (lstFiles.SelectedIndices.Count == 1)
            {
                TiFileSystemObject entry = GetSelectedItemData(0);
                deleteToolStripMenuItem2.Enabled = !entry.Name.StartsWith("?");
                extractToolStripMenuItem2.Enabled = !entry.Name.StartsWith("?");
                propertiesToolStripMenuItem2.Enabled = true;
                renameToolStripMenuItem2.Enabled = !entry.Name.StartsWith("?");
                undeleteToolStripMenuItem2.Enabled = entry.Name.StartsWith("?");
            }
            else
            {
                deleteToolStripMenuItem2.Enabled = true;
                extractToolStripMenuItem2.Enabled = true;
                propertiesToolStripMenuItem2.Enabled = true;
                renameToolStripMenuItem2.Enabled = false;

                //Should be determined if there are any deleted entries in the selection and possibly enable this?
                undeleteToolStripMenuItem2.Enabled = false;
            }
        }

        private void managePartitions_Click(object sender, EventArgs e)
        {
            using dlgManagePartitions dlg = new();
            dlg.ShowDialog();
        }

        private void settings_Click(object sender, EventArgs e)
        {
            using dlgSettings dlg = new();
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                SyncUIOptions();
                ResetView();
                UpdateStatusBar(true);
                if (!Settings.CurrentSettings.AutoIncrementFilename)
                    lastSavedFilename = null;
            }
            else if (result == DialogResult.Cancel)
            {
                //If the user cleared the recent images list in the dialog, we still want to update the menu in the main form
                PopulateRecentList();
            }
        }

        //TODO: Implement the Properties dialog for multiple selected objects like Windows does it
        private void properties_Click(object sender, EventArgs e)
        {
            List<TiFileSystemObject> entries = new();
            if (lstDirectories.Focused)
            {
                if(lstDirectories.SelectedNode.Text != "\\") //Can't show properties for the root node at this time
                    entries.Add((TiFileSystemObject)lstDirectories.SelectedNode.Tag);
            }
            else if (lstFiles.Focused)
            {
                for (int i = 0; i < lstFiles.SelectedIndices.Count; i++)
                    entries.Add(GetSelectedItemData(i));
            }
            else
            {
                if (lstFiles.SelectedIndices.Count > 0)
                {
                    for (int i = 0; i < lstFiles.SelectedIndices.Count; i++)
                        entries.Add(GetSelectedItemData(i));
                }
                else if (lstDirectories.SelectedNode != null && lstDirectories.SelectedNode.Text != "\\")
                    entries.Add((TiFileSystemObject)lstDirectories.SelectedNode.Tag);
                else
                    return; //Can't show properties for whatever is selected, so let's just return
            }

            using dlgProperties dlg = new(entries);
            dlg.ShowDialog();
        }

        private void injectFiles_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.AutoUpgradeEnabled = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = true;
            ofd.Filter = "All files (*.*)|*.*";

            //TODO: Get the count and total size of seleted items to inject before showing the dialog
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (Settings.CurrentSettings.ConfirmInjection)
                {
                    TaskDialogPage page = new()
                    {
                        Text = $"Are you sure you want to inject {SelectedItems.Count()} item(s) occupying {Settings.CurrentSettings.SizeUnit.FormatSize(0)} into the image?",
                        Heading = $"{SelectedItems.Count()} item(s) will be injected",
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
                        Settings.CurrentSettings.ConfirmInjection = false;

                    if (result == TaskDialogButton.No)
                        return;
                }

                throw new NotImplementedException("This feature is not implemented yet");
            }
        }

        private void closeImage_Click(object sender, EventArgs e)
        {
            if (unsavedChanges)
            {
                TaskDialogButton result = TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = $"Would you like to save them before closing the current image?",
                    Heading = "You have unsaved changes",
                    Caption = "Warning",
                    Buttons =
                        {
                            new TaskDialogCommandLinkButton("&Save") { Tag = 1 },
                            new TaskDialogCommandLinkButton("&Discard"),
                            TaskDialogButton.Cancel
                        },
                    Icon = TaskDialogIcon.Warning,
                });

                if (result.Tag is not null) /* Save changes... */ ;
                else if (result == TaskDialogButton.Cancel) return;
            }
            CloseImage();
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

        private void sortByAttributes_Click(object sender, EventArgs e)
            => SortListViewBy(lstFiles.Columns.IndexOfKey("clmAttributes"));

        private void lstDirectories_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            //This prevents the user from opening a deleted directory (since we don't even know yet if it's recoverable, or what was inside, etc.)
            if (e.Node is null)
                return;

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
            if (e.Node is null)
                return;

            //This makes sure the selected image doesn't change when a hidden folder is selected
            if (((TiFileSystemObject)e.Node.Tag).Attributes.HasFlag(FileAttributes.Hidden))
            {
                lstDirectories.SelectedImageKey = "folder (Hidden)";
            }
            else
            {
                lstDirectories.SelectedImageKey = "folder";
            }

            PopulateListView((TiDirectory)e.Node.Tag);
            UpdateStatusBar(false);

            if (lstDirectories.SelectedNode is null)
            {
                extractToolStripButton.Enabled = false;
                newFolderToolStripButton.Enabled = false;
                propertiesToolStripButton.Enabled = false;
                deleteToolStripButton.Enabled = false;
            }
            else
            {
                extractToolStripButton.Enabled = true;
                newFolderToolStripButton.Enabled = true;
                deleteToolStripButton.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
                propertiesToolStripButton.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
            }
        }

        private void lstFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstFiles.SelectedIndices.Count == 1 && e.Button != MouseButtons.Right)
            {
                if (GetSelectedItemData(0) is TiDirectory dir) //A folder was double-clicked
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
                else //A file was double-clicked - extract to temp dir then open it
                {
                    tempDir = Path.Combine(Settings.TempDir, GetRandomDirName());
                    string targetFile = Path.Combine(tempDir, SelectedItems.First().Name);

                    FileExtraction.ExtractFilesToTemporaryDirectory(this, SelectedItems, DirectoryExtractionMode.Skip);

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

            deleteToolStripMenuItem1.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
            renameToolStripMenuItem1.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
            extractToolStripMenuItem1.Enabled = true;
            propertiesToolStripMenuItem1.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];

            expandDirectoryTreeToolStripMenuItem1.Enabled = lstDirectories.Nodes[0].Nodes.Count > 0;
            collapseDirectoryTreeToolStripMenuItem1.Enabled = lstDirectories.Nodes[0].Nodes.Count > 0;
        }

        private void selectAll_Click(object sender, EventArgs e)
        {
            lstFiles.Focus();
            lstFiles.SelectAllItems();
        }

        private void list_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data is not null && e.Data.GetDataPresent(DataFormats.FileDrop) && image is null)
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /* Drag and drop was performed on the ListView - a file was dragged into the ListView/TreeView from Explorer => try to open it
         * Right now, we only handle case a for opening a single file that was dragged into the window.
         * TODO: Implement other drag and drop scenarios (moving files within the image, etc.). */
        private void list_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data is not null && e.Data.GetDataPresent(DataFormats.FileDrop) && image is null)
            {
                //Files are being dragged into the listview from outside the form
                var data = e.Data.GetData(DataFormats.FileDrop, false);
                if (data is not null)
                {
                    string[] items = (string[])data;
                    if (items.Length == 1)
                    {
                        CloseImage();
                        filepath = items[0];
                        OpenImage(filepath);
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

                //This prevents opening the menu on empty area of the TreeView, as well as on any deleted folders
                if (newNode is not null)
                {
                    TiFileSystemObject entry = (TiFileSystemObject)newNode.Tag;
                    if (entry.Name.StartsWith("?"))
                    {
                        return;
                    }

                    lstDirectories.SelectedNode = newNode;
                    cmsDirTree.Show(lstDirectories, e.Location);
                }
                else
                {
                    return;
                }
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

        /* Fires when the user starts dragging a ListViewItem around. String array is needed for Explorer to perform the move operation once
         * the drop is performed. */
        private void lstFiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Item is not null)
            {
                tempDir = Path.Combine(Settings.TempDir, GetRandomDirName());
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
                lstFiles.DoDragDrop(data, DragDropEffects.Move);
            }
        }

        /* Fires when the user starts dragging a TreeNode around. String array is needed for Explorer to perform the move operation once
         * the drop is performed. */
        private void lstDirectories_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Item is not null)
            {
                tempDir = Path.Combine(Settings.TempDir, GetRandomDirName());
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
                    foreach (var fso in draggedDir.EnumerateFileSystemObjects(Settings.CurrentSettings.ShowHiddenItems, false))
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
                lstDirectories.DoDragDrop(data, DragDropEffects.Move);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (unsavedChanges)
            {
                TaskDialogButton result = TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = $"Would you like to save them before closing TotalImage?",
                    Heading = "You have unsaved changes",
                    Caption = "Warning",
                    Buttons =
                        {
                            new  TaskDialogCommandLinkButton("&Save") { Tag = 1 },
                            new TaskDialogCommandLinkButton("&Discard"),
                            TaskDialogButton.Cancel
                        },
                    Icon = TaskDialogIcon.Warning,
                });

                /*if (result.Tag is not null) // Save changes... ;
                else */
                if (result == TaskDialogButton.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            Settings.Save();
            SetUIState();
            Settings.SaveUIState();

            //Try to clear the temp folder if this is the only instance
            if (Process.GetProcessesByName("TotalImage").Length == 1)
            {
                try
                {
                    if (Directory.Exists(Settings.TempDir))
                        Directory.Delete(Settings.TempDir, true);

                    Directory.CreateDirectory(Settings.TempDir);
                }
                catch (Exception)
                {
                    //Ignore the exception, since the program is closing anyway... The folder can be cleared another day.
                }
            }
        }

        private void viewMenu_DropDownOpening(object sender, EventArgs e)
        {
            nameToolStripMenuItem.Checked = typeToolStripMenuItem.Checked = sizeToolStripMenuItem.Checked =
                modifiedToolStripMenuItem.Checked = attributesToolStripMenuItem.Checked = false;
            switch (sortColumn)
            {
                case 0: nameToolStripMenuItem.Checked = true; break;
                case 1: typeToolStripMenuItem.Checked = true; break;
                case 2: sizeToolStripMenuItem.Checked = true; break;
                case 3: modifiedToolStripMenuItem.Checked = true; break;
                case 4: attributesToolStripMenuItem.Checked = true; break;
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

            expandDirectoryTreeToolStripMenuItem.Enabled = image is not null && lstDirectories.Nodes[0].Nodes.Count > 0;
            collapseDirectoryTreeToolStripMenuItem.Enabled = image is not null && lstDirectories.Nodes[0].Nodes.Count > 0;
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
                modifiedToolStripMenuItem1.Checked = attributesToolStripMenuItem1.Checked = false;
            switch (sortColumn)
            {
                case 0: nameToolStripMenuItem1.Checked = true; break;
                case 1: typeToolStripMenuItem1.Checked = true; break;
                case 2: sizeToolStripMenuItem1.Checked = true; break;
                case 3: modifiedToolStripMenuItem1.Checked = true; break;
                case 4: attributesToolStripMenuItem1.Checked = true; break;
            }
        }

        private void selectPartitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image is null)
                return;

            dlgSelectPartition dlg = new()
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
            /* Note that we can't simply use the ComboBox.SelectedIndex, as unsupported partitions aren't added to the ComboBox and thus item
             * indices are shifted compared to actual partition indices. */
            if (image.PartitionTable.Partitions.Count > 1)
            {
                int realIndex = int.Parse(selectPartitionToolStripComboBox.SelectedItem.ToString()[..1]);

                if (realIndex != CurrentPartitionIndex)
                {
                    LoadPartitionInCurrentImage(realIndex);
                    CurrentPartitionIndex = realIndex;
                }
            }
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
                    TiFileSystemObject fso = GetSelectedItemData(0);
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
                    else
                    {
                        tempDir = Path.Combine(Settings.TempDir, GetRandomDirName());
                        string targetFile = Path.Combine(tempDir, SelectedItems.First().Name);

                        FileExtraction.ExtractFilesToTemporaryDirectory(this, SelectedItems, DirectoryExtractionMode.Skip);

                        ProcessStartInfo psi = new()
                        {
                            FileName = targetFile,
                            UseShellExecute = true
                        };
                        Process.Start(psi);
                    }
                }
            }
            /*else
            {*/
                /* Searches the currently displayed items for the first one that starts with the character of the pressed key.
                 * Currently this is very rudimentary, as it only works for English letters and digits, and the first item that is found.
                 * Could be improved to continue the search further etc. */
               /* string character = e.KeyCode.ToString();

                //This crap is done so numeric keys also work...
                if ((byte)e.KeyCode > 0x30 && (byte)e.KeyCode < 0x39)
                    character = ((int)e.KeyCode - 0x30).ToString();
                else if ((byte)e.KeyCode > 0x60 && (byte)e.KeyCode < 0x69)
                    character = ((int)e.KeyCode - 0x60).ToString();

                foreach (ListViewItem lvi in currentFolderView)
                {
                    if (lvi.Text.ToLower().StartsWith(character.ToLower()))
                    {
                        lstFiles.SelectedIndices.Clear();
                        lvi.Focused = true;
                        lvi.Selected = true;
                        return;
                    }
                }
            }*/
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

        private void lblNotifications_Click(object sender, EventArgs e)
        {
            using dlgNotifications dlg = new();
            dlg.ShowDialog();
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            closeImageToolStripMenuItem.Enabled = image is not null;
            saveToolStripMenuItem.Enabled = image is not null && unsavedChanges;
            saveAsToolStripMenuItem.Enabled = image is not null;
        }

        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (image is null)
            {
                injectAFolderToolStripMenuItem.Enabled = false;
                injectFilesToolStripMenuItem.Enabled = false;
                extractToolStripMenuItem.Enabled = false;
                renameToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                undeleteToolStripMenuItem.Enabled = false;
                propertiesToolStripMenuItem.Enabled = false;
                selectAllToolStripMenuItem.Enabled = false;
                newFolderToolStripMenuItem.Enabled = false;
                changeFormatToolStripMenuItem.Enabled = false;
                selectPartitionToolStripMenuItem.Enabled = false;
                managePartitionsToolStripMenuItem.Enabled = false;
                bootSectorPropertiesToolStripMenuItem.Enabled = false;
                formatToolStripMenuItem.Enabled = false;
                defragmentToolStripMenuItem.Enabled = false;
                changeVolumeLabelToolStripMenuItem.Enabled = false;

                return;
            }

            changeFormatToolStripMenuItem.Enabled = true;
            selectPartitionToolStripMenuItem.Enabled = image.PartitionTable is not Partitions.NoPartitionTable; ;
            managePartitionsToolStripMenuItem.Enabled = image.PartitionTable is not Partitions.NoPartitionTable; ;
            bootSectorPropertiesToolStripMenuItem.Enabled = true;
            changeVolumeLabelToolStripMenuItem.Enabled = true;
            formatToolStripMenuItem.Enabled = true;
            defragmentToolStripMenuItem.Enabled = true;
            injectAFolderToolStripMenuItem.Enabled = true;
            injectFilesToolStripMenuItem.Enabled = true;
            extractToolStripMenuItem.Enabled = true;
            selectAllToolStripMenuItem.Enabled = true;
            newFolderToolStripMenuItem.Enabled = true;

            if (lstDirectories.Focused)
            {
                undeleteToolStripMenuItem.Enabled = false;
                renameToolStripMenuItem.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
                deleteToolStripMenuItem.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
                propertiesToolStripMenuItem.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
            }
            else if (lstFiles.Focused)
            {
                if (lstFiles.SelectedIndices.Count == 0)
                {
                    deleteToolStripMenuItem.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
                    propertiesToolStripMenuItem.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
                    renameToolStripMenuItem.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
                    undeleteToolStripMenuItem.Enabled = false;
                }
                else if (lstFiles.SelectedIndices.Count == 1 && lstFiles.SelectedIndices[0] < IndexShift)
                {
                    renameToolStripMenuItem.Enabled = false;
                    deleteToolStripMenuItem.Enabled = false;
                    undeleteToolStripMenuItem.Enabled = false;
                    propertiesToolStripMenuItem.Enabled = false;
                }
                else if (lstFiles.SelectedIndices.Count == 1)
                {
                    TiFileSystemObject entry = GetSelectedItemData(0);
                    deleteToolStripMenuItem.Enabled = !entry.Name.StartsWith("?");
                    extractToolStripMenuItem.Enabled = !entry.Name.StartsWith("?");
                    propertiesToolStripMenuItem.Enabled = true;
                    renameToolStripMenuItem.Enabled = !entry.Name.StartsWith("?");
                    undeleteToolStripMenuItem.Enabled = entry.Name.StartsWith("?");
                }
                else
                {
                    deleteToolStripMenuItem.Enabled = true;
                    propertiesToolStripMenuItem.Enabled = true;
                    renameToolStripMenuItem.Enabled = false;
                    undeleteToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void toolsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            imageInformationToolStripMenuItem.Enabled = image is not null;
            hexViewToolStripMenuItem.Enabled = image is not null;
        }

        private void lstFiles_Enter(object sender, EventArgs e)
        {
            if (image is not null)
            {
                if (lstFiles.SelectedIndices.Count == 0)
                {
                    deleteToolStripButton.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
                    extractToolStripButton.Enabled = true;
                    propertiesToolStripButton.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
                }
                else if (lstFiles.SelectedIndices.Count == 1 && lstFiles.SelectedIndices[0] < IndexShift)
                {
                    deleteToolStripButton.Enabled = false;
                    extractToolStripButton.Enabled = false;
                    propertiesToolStripButton.Enabled = false;
                }
                else if (lstFiles.SelectedIndices.Count == 1)
                {
                    //Check if selected item is a deleted entry and enable the UI accordingly
                    TiFileSystemObject entry = GetSelectedItemData(0);
                    deleteToolStripButton.Enabled = !entry.Name.StartsWith("?");
                    extractToolStripButton.Enabled = !entry.Name.StartsWith("?");
                    propertiesToolStripButton.Enabled = true;
                }
                else
                {
                    deleteToolStripButton.Enabled = true;
                    extractToolStripButton.Enabled = true;
                    propertiesToolStripButton.Enabled = true;
                }
            }
        }

        private void lstDirectories_Enter(object sender, EventArgs e)
        {
            if (image is not null)
            {
                if (lstDirectories.SelectedNode is null)
                {
                    extractToolStripButton.Enabled = false;
                    newFolderToolStripButton.Enabled = false;
                    propertiesToolStripButton.Enabled = false;
                    deleteToolStripButton.Enabled = false;
                }
                else
                {
                    extractToolStripButton.Enabled = true;
                    newFolderToolStripButton.Enabled = true;
                    deleteToolStripButton.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
                    propertiesToolStripButton.Enabled = lstDirectories.SelectedNode != lstDirectories.Nodes[0];
                }
            }
        }

        //After an item's label (=Text property) is changed - for renaming objects
        //From here the name change should propagate to the associated FileSystemObject and to the stream
        private void lstFiles_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {

        }

        //Before an item's label (=Text property) will be changed - for renaming objects.
        //Here we should probably perform some sanity checks (ie. make sure the user's not trying to rename a deleted object and such)
        private void lstFiles_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {

        }

        //After a node's label (=Text property) is changed - for renaming objects
        //From here the name change should propagate to the associated FileSystemObject and to the stream
        private void lstDirectories_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {

        }

        //Before a node's label (=Text property) will be changed - for renaming objects.
        //Here we should probably perform some sanity checks (ie. make sure the user's not trying to rename a deleted object and such)
        private void lstDirectories_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {

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
                        extractionSucceeded = FileExtraction.ExtractFilesToTemporaryDirectory(this, SelectedItems, DirectoryExtractionMode.Preserve);
                    }
                    else if (sender is TreeView && draggedDir is not null)
                    {
                        if (draggedDir.Parent is null) //Root dir needs to be treated separately
                            extractionSucceeded = FileExtraction.ExtractFilesToTemporaryDirectory(this, draggedDir.EnumerateFileSystemObjects(Settings.CurrentSettings.ShowHiddenItems, false), DirectoryExtractionMode.Preserve);
                        else
                            extractionSucceeded = FileExtraction.ExtractFilesToTemporaryDirectory(this, new TiFileSystemObject[] { draggedDir }, DirectoryExtractionMode.Preserve);
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

        private void parentDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstDirectories.SelectedNode.Parent is not null)
            {
                lstDirectories.SelectedNode = lstDirectories.SelectedNode.Parent;
            }
        }

        private void parentDirectoryToolStripButton_Click(object sender, EventArgs e)
        {
            if (lstDirectories.SelectedNode.Parent is not null)
            {
                lstDirectories.SelectedNode = lstDirectories.SelectedNode.Parent;
            }
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            //This is needed for whatever reason because otherwise CTRL+W doesn't work except when the menu is open...
            if (e.KeyCode == Keys.W && e.Modifiers == Keys.Control)
            {
                closeImage_Click(sender, e);
            }
        }
        #endregion

        private int IndexShift => lstFiles.VirtualListSize - currentFolderView.Count;

        private IEnumerable<TiFileSystemObject> SelectedItems
            => from x in lstFiles.SelectedIndices.Cast<int>()
               where x >= IndexShift && currentFolderView[x - IndexShift].Tag is TiFileSystemObject
               select (TiFileSystemObject)currentFolderView[x - IndexShift].Tag;

        StatusBarState StatusBarState
            => lstFiles.SelectedIndices.Cast<int>().Where(x => x >= IndexShift).Count() switch
            {
                0 => StatusBarState.NoneSelected,
                1 => StatusBarState.OneSelected,
                _ => StatusBarState.MultipleSelected
            };

        //Sets the CurrentUIState to what the current state of UI is
        private void SetUIState()
        {
            Settings.CurrentUIState.SplitterDistance = splitContainer.SplitterDistance;

            if (WindowState != FormWindowState.Minimized)
            {
                Settings.CurrentUIState.WindowPosition = Location;
                Settings.CurrentUIState.WindowState = WindowState;
                Settings.CurrentUIState.WindowSize = Size;
            }

            foreach (ColumnHeader col in lstFiles.Columns)
            {
                Settings.CurrentUIState.MWColumnOrder[col.Index] = col.DisplayIndex;
                Settings.CurrentUIState.MWColumnWidth[col.Index] = col.Width;
            }
        }

        // Used for events that require the current folder view to be updated (e.g. show hidden/deleted items toggled, etc.)
        private void ResetView()
        {
            if (image is not null)
            {
                lastViewedDir = (TiDirectory)lstDirectories.SelectedNode.Tag;

                var root = new TreeNode(@"\");
                root.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
                root.SelectedImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
                root.Tag = image.PartitionTable.Partitions[CurrentPartitionIndex].FileSystem.RootDirectory;

                lstDirectories.BeginUpdate();
                PopulateTreeView(root, image.PartitionTable.Partitions[CurrentPartitionIndex].FileSystem.RootDirectory);

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
            }
        }

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

                //Deleted folders have strikethrough fontstyle
                if (subdir.Name.StartsWith("?"))
                {
                    Font font = new("Segoe UI", 9f, FontStyle.Strikeout);
                    subnode.NodeFont = font;
                }
                subnode.Tag = subdir;
                node.Nodes.Add(subnode);

                PopulateTreeView(subnode, subdir);
            }
        }

        private Bitmap CreateHiddenIcon(Bitmap normalIcon)
        {
            ColorMatrix cm = new();
            cm.Matrix33 = 0.65f; //65% opacity
            ImageAttributes attributes = new();
            attributes.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            Point[] points = { new Point(0, 0),
                               new Point(normalIcon.Width, 0),
                               new Point(0, normalIcon.Height),
                             };
            Rectangle rect = new(0, 0, normalIcon.Width, normalIcon.Height);

            Bitmap bmp = new(normalIcon.Width, normalIcon.Height);
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
            if (dir.Parent is not null)
            {
                count++;
                parentDirectoryToolStripMenuItem.Enabled = true;
                parentDirectoryToolStripButton.Enabled = true;
            }
            else
            {
                parentDirectoryToolStripMenuItem.Enabled = false;
                parentDirectoryToolStripButton.Enabled = false;
            }

            foreach (var fso in dir.EnumerateFileSystemObjects(Settings.CurrentSettings.ShowHiddenItems, Settings.CurrentSettings.ShowDeletedItems))
            {
                var item = new ListViewItem();
                item.Text = fso.Name;
                item.SubItems.Add(GetFileTypeName(fso.Name, fso.Attributes));

                string size = string.Empty;
                if (fso is TiDirectory subdir)
                {
                    size = Settings.CurrentSettings.SizeUnit.FormatSize(subdir.GetSize(true, false));

                    if (Settings.CurrentSettings.FileListShowDirSize)
                        item.SubItems.Add(size);
                    else
                        item.SubItems.Add(string.Empty);
                }
                else
                {
                    size = Settings.CurrentSettings.SizeUnit.FormatSize(fso.Length);
                    item.SubItems.Add(size);
                }

                item.ImageIndex = GetFileTypeIconIndex(fso.Name, fso.Attributes);

                item.SubItems.Add(fso.LastWriteTime.ToString());
                item.SubItems.Add(FileAttributesToString(fso.Attributes));
                item.UseItemStyleForSubItems = false;
                item.SubItems[4].Font = new(FontFamily.GenericMonospace, 9);

                //Do some simple styling for hidden and deleted items
                if (fso.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    item.ForeColor = Color.Gray;
                    item.SubItems[1].ForeColor = Color.Gray;
                    item.SubItems[2].ForeColor = Color.Gray;
                    item.SubItems[3].ForeColor = Color.Gray;
                    item.SubItems[4].ForeColor = Color.Gray;
                }
                if (fso.Name.StartsWith("?"))
                {
                    Font sfont = new("Segoe UI", 9f, FontStyle.Strikeout);
                    item.UseItemStyleForSubItems = false;
                    item.Font = sfont;
                }

                item.ToolTipText = $"Type: {item.SubItems[1].Text}{Environment.NewLine}Size: {size}{Environment.NewLine}Modified: {item.SubItems[3].Text}";

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

        //Opens an image
        private void OpenImage(string? path)
        {
            if (path is not null && (path == "" || path.Trim() == ""))
            {
                throw new ArgumentException("path must be either a valid path for existing files, or null when opening newly created files!");
            }

            //Opening an existing file, otherwise it's a newly created image
            if (path is not null)
            {
                filepath = path;
                filename = Path.GetFileName(path);
                FileInfo fileinfo = new(path);
                var fileext = Path.GetExtension(filename).ToLowerInvariant();
                var filesize = fileinfo.Length;

                //Stop with empty files rightaway
                if (filesize == 0)
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"The file \"{filename}\" appears to be empty (ie. zero bytes in size). If you downloaded or copied this file from some other location, make sure the source file is not damaged and that the transfer completed successfully.{Environment.NewLine}{Environment.NewLine}" +
                                $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                        Heading = "File is empty",
                        Caption = "Error",
                        Buttons =
                        {
                            TaskDialogButton.OK
                        },
                        Icon = TaskDialogIcon.Error,
                        SizeToContent = true
                    });

                    CloseImage();
                    return;
                }

                try
                {
                    //Disable this for now until it's properly implemented
                    bool memoryMapping = false; //fileinfo.Length > Settings.CurrentSettings.MemoryMappingThreshold;

                    image = fileext switch
                    {
                        ".vhd" => new VhdContainer(path, memoryMapping),
                        ".nhd" => new NhdContainer(path, memoryMapping),
                        ".imz" => new ImzContainer(path, memoryMapping),
                        ".hdi" or ".fdi" => new Anex86Container(path, memoryMapping),
                        ".img" or ".ima" or ".iso" or ".vfd" or ".flp" or ".360" or
                        ".720" or ".12" or ".144" or ".288" or ".dsk" or ".hdm" => new RawContainer(path, memoryMapping),
                        ".json" => new PCjsContainer(path, memoryMapping),
                        _ => throw new InvalidDataException("This container format is not recognized and cannot be opened."),
                    }; ;
                }
                catch (FileNotFoundException)
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"The file \"{filename}\" could not be opened because it no longer exists. It may have been moved or deleted in the mean time. Make sure the file exists and is accessible and try again.{Environment.NewLine}{Environment.NewLine}" +
                                $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                        Heading = "File not found",
                        Caption = "Error",
                        Buttons =
                        {
                            TaskDialogButton.OK
                        },
                        Icon = TaskDialogIcon.Error,
                        SizeToContent = true
                    });

                    CloseImage();
                    return;
                }
                catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32) //File is locked by another process
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"The file \"{filename}\" could not be opened because it's currently locked by another process. Close all processes using this file and try again.{Environment.NewLine}{Environment.NewLine}" +
                                $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                        Heading = "File is in use",
                        Caption = "Error",
                        Buttons =
                        {
                            TaskDialogButton.OK
                        },
                        Icon = TaskDialogIcon.Error,
                        SizeToContent = true
                    });

                    CloseImage();
                    return;
                }
                catch (UnauthorizedAccessException)
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"The file \"{filename}\" could not be opened because access was denied. Make sure you have the required permissions and that the file is not marked as read-only.{Environment.NewLine}{Environment.NewLine}" +
                                $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                        Heading = "Access denied",
                        Caption = "Error",
                        Buttons =
                        {
                            TaskDialogButton.OK
                        },
                        Icon = TaskDialogIcon.Error,
                        SizeToContent = true
                    });

                    CloseImage();
                    return;
                }
                catch (Exception e)
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"The file \"{filename}\" could not be opened due to the following exception:{Environment.NewLine}{Environment.NewLine}" +
                                $"{e.Message}{Environment.NewLine}{Environment.NewLine}If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                        Heading = "Cannot open file",
                        Caption = "Error",
                        Buttons =
                        {
                            TaskDialogButton.OK
                        },
                        Icon = TaskDialogIcon.Error,
                        SizeToContent = true
                    });

                    CloseImage();
                    return;
                }
            }

            Settings.AddRecentImage(filepath);
            PopulateRecentList();

            CurrentPartitionIndex = 0;
            selectPartitionToolStripComboBox.Items.Clear();

            if (image.PartitionTable is Partitions.NoPartitionTable)
            {
                if (image.PartitionTable.Partitions[0].FileSystem is FileSystems.RAW.RawFileSystem)
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"We attempted to open this file as the {image.DisplayName} container, but found no supported partition table or any supported file systems inside, so this image cannot be loaded. " +
                                $"It's also possible this container format might not be correct for this file.{Environment.NewLine}{Environment.NewLine}" +
                                $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                        Heading = "Cannot open file",
                        Caption = "Error",
                        Buttons =
                        {
                            TaskDialogButton.OK
                        },
                        Icon = TaskDialogIcon.Error,
                        SizeToContent = true
                    });

                    CloseImage();
                    return;
                }

                var label = image.PartitionTable.Partitions[0].FileSystem.VolumeLabel.TrimEnd(' ');
                if (string.IsNullOrWhiteSpace(label))
                    label = "<no label>";
                var fs = image.PartitionTable.Partitions[0].FileSystem.DisplayName;
                var length = Settings.CurrentSettings.SizeUnit.FormatSize((ulong)image.PartitionTable.Partitions[0].Length);

                selectPartitionToolStripComboBox.Items.Add($"{label} ({fs}, {length})");
                selectPartitionToolStripComboBox.SelectedIndex = 0;
            }
            else
            {
                if (image.PartitionTable.Partitions.Count == 0)
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"We found a supported partition table in this image ({image.PartitionTable.DisplayName}), but no partitions, so this image cannot be loaded.{Environment.NewLine}{Environment.NewLine}" +
                                $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                        Heading = "No partitions found",
                        Caption = "Error",
                        Buttons =
                        {
                            TaskDialogButton.OK
                        },
                        Icon = TaskDialogIcon.Error,
                        SizeToContent = true,
                    });

                    CloseImage();
                    return;
                }

                if (image.PartitionTable.Partitions.Count > 1)
                {
                    //If there's multiple partitions in a supported partition table, and they're all unsupported types, for now we just back out.
                    //Once we implement partition management (soon™), we can offer that to the user.
                    int rawCount = 0;
                    foreach(Partitions.PartitionEntry entry in image.PartitionTable.Partitions)
                    {
                        if(entry.FileSystem is FileSystems.RAW.RawFileSystem)
                            rawCount++;
                    }

                    if(rawCount == image.PartitionTable.Partitions.Count)
                    {
                        TaskDialog.ShowDialog(this, new TaskDialogPage()
                        {
                            Text = $"We found a supported partition table ({image.PartitionTable.DisplayName}) and several partitions in this image, but they all contain unsupported file systems, so this image cannot be loaded.{Environment.NewLine}{Environment.NewLine}" +
                                    $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                            Heading = "No supported file systems",
                            Caption = "Error",
                            Buttons =
                            {
                                TaskDialogButton.OK
                            },
                            Icon = TaskDialogIcon.Error,
                            SizeToContent = true
                        });

                        CloseImage();
                        return;
                    }

                    dlgSelectPartition selectFrm = new()
                    {
                        PartitionTable = image.PartitionTable
                    };

                    if (selectFrm.ShowDialog() == DialogResult.Cancel)
                    {
                        CloseImage();
                        return;
                    }

                    CurrentPartitionIndex = selectFrm.SelectedEntry;
                }

                if (image.PartitionTable.Partitions.Count == 1 && image.PartitionTable.Partitions[0].FileSystem is FileSystems.RAW.RawFileSystem)
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"We found a supported partition table ({image.PartitionTable.DisplayName}) and one partition in this image, but it contains an unsupported file system, so this image cannot be loaded.{Environment.NewLine}{Environment.NewLine}" +
                                $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                        Heading = "Unsupported file system",
                        Caption = "Error",
                        Buttons =
                        {
                            TaskDialogButton.OK
                        },
                        Icon = TaskDialogIcon.Error,
                        SizeToContent = true
                    });

                    CloseImage();
                    return;
                }

                for (int i = 0; i < image.PartitionTable.Partitions.Count; i++)
                {
                    try
                    {
                        //Skip adding RAW to the combobox since it can't be loaded anyway
                        if (image.PartitionTable.Partitions[i].FileSystem is FileSystems.RAW.RawFileSystem)
                            continue;

                        var label = image.PartitionTable.Partitions[i].FileSystem.VolumeLabel.TrimEnd(' ');
                        if (string.IsNullOrWhiteSpace(label))
                            label = "<no label>";
                        var fs = image.PartitionTable.Partitions[i].FileSystem.DisplayName;
                        var length = Settings.CurrentSettings.SizeUnit.FormatSize((ulong)image.PartitionTable.Partitions[i].Length);

                        selectPartitionToolStripComboBox.Items.Add($"{(image.PartitionTable.Partitions.Count > 1 ? i + ": " : string.Empty)}{label} ({fs}, {length})");

                    }
                    catch (InvalidDataException)
                    {

                    }
                }

                selectPartitionToolStripComboBox.SelectedIndex = image.PartitionTable.Partitions.Count > 1 ? selectPartitionToolStripComboBox.FindString($"{CurrentPartitionIndex}: ") : 0;
            }

            LoadPartitionInCurrentImage(CurrentPartitionIndex);

            if (filename != "")
                Text = $"{filename} - TotalImage";
            else
                Text = "(Untitled) - TotalImage";
        }

        private void LoadPartitionInCurrentImage(int index)
        {
            if (image is null)
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

            if (Settings.CurrentSettings.QueryShellForFileTypeInfo)
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
                ("file", imgFilesSmall, Settings.CurrentSettings.QueryShellForFileTypeInfo ? ShellInterop.SmallFileIcon : Properties.Resources.icon_page_white),
                ("file", imgFilesLarge, Settings.CurrentSettings.QueryShellForFileTypeInfo ? ShellInterop.LargeFileIcon : Properties.Resources.icon_page_white_32),
                ("folder", imgFilesSmall, Settings.CurrentSettings.QueryShellForFileTypeInfo ? ShellInterop.SmallFolderIcon : Properties.Resources.icon_folder),
                ("folder", imgFilesLarge, Settings.CurrentSettings.QueryShellForFileTypeInfo ? ShellInterop.LargeFolderIcon : Properties.Resources.icon_folder_32)
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

        //Enables various UI elements after an image is loaded
        public void EnableUI()
        {
            closeToolStripButton.Enabled = true;
            injectToolStripButton.Enabled = true;
            newFolderToolStripButton.Enabled = true;
            labelToolStripMenuButton.Enabled = true;
            bootsectToolStripButton.Enabled = true;
            infoToolStripButton.Enabled = true;
            lblStatusProgressBar.Visible = true;

            // Change border sides for status bar children to add seperator-like looks.
            lblStatusCapacity.BorderSides = ToolStripStatusLabelBorderSides.Right;
            lblStatusSize.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right;

            if (unsavedChanges)
                saveToolStripButton.Enabled = true;

            //Enabling this now since we have rudimentary HDD support.
            managePartitionsToolStripButton.Enabled = image is not null && image.PartitionTable is not Partitions.NoPartitionTable;
            selectPartitionToolStripComboBox.Enabled = true;
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
            lblStatusProgressBar.Visible = false;
            parentDirectoryToolStripMenuItem.Enabled = false;
            parentDirectoryToolStripButton.Enabled = false;

            // Change border sides for status bar children to remove seperator-like looks.
            lblStatusCapacity.BorderSides = ToolStripStatusLabelBorderSides.None;
            lblStatusSize.BorderSides = ToolStripStatusLabelBorderSides.None;
        }

        private void PopulateRecentList()
        {
            recentFilesToolStripMenuItem.DropDownItems.Clear();

            for (int i = Settings.CurrentSettings.RecentImages.Count - 1; i >= 0; i--)
            {
                //Remove any bogus entries
                if (string.IsNullOrWhiteSpace(Settings.CurrentSettings.RecentImages[i]))
                {
                    Settings.RemoveRecentImage(Settings.CurrentSettings.RecentImages[i]);
                    continue;
                }

                ToolStripMenuItem newItem = new();
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

        private void SyncWindowState()
        {
            WindowState = Settings.CurrentUIState.WindowState;
            Location = Settings.CurrentUIState.WindowPosition;
            Size = Settings.CurrentUIState.WindowSize;
            splitContainer.SplitterDistance = Settings.CurrentUIState.SplitterDistance;

            foreach (ColumnHeader col in lstFiles.Columns)
            {
                col.DisplayIndex = Settings.CurrentUIState.MWColumnOrder[col.Index];
                col.Width = Settings.CurrentUIState.MWColumnWidth[col.Index];
            }
        }

        //Syncs the main form UI with the current settings and loaded UI state
        private void SyncUIOptions()
        {
            lstFiles.View = Settings.CurrentSettings.FilesView;
            splitContainer.Panel1Collapsed = !Settings.CurrentSettings.ShowDirectoryTree;
            statusBar.Visible = Settings.CurrentSettings.ShowStatusBar;
            commandBar.Visible = Settings.CurrentSettings.ShowCommandBar;
            sortOrder = Settings.CurrentSettings.FilesSortOrder;
            sortColumn = Settings.CurrentSettings.FilesSortingColumn;

            lstFiles.SetSortIcon(sortColumn, sortOrder);
            GetDefaultIcons(); //This is needed so that icons update immediately, rather than after a restart of the app

            Settings.CheckRecentImages();
            PopulateRecentList();
        }

        private void UpdateStatusBar(bool updateFreeSpace)
        {
            if (image is null)
            {
                lblStatusCapacity.Text = string.Empty;
                lblStatusFreeCapacity.Text = string.Empty;
                lbStatusPath.Text = string.Empty;
                lblStatusSize.Text = string.Empty;
            }
            else
            {
                //Makes no sense to do this every time selection changes, etc. Only when operations that affect the free space are performed
                if (updateFreeSpace)
                {
                    lblStatusCapacity.Text = $"Partition size: {Settings.CurrentSettings.SizeUnit.FormatSize((ulong)image.PartitionTable.Partitions[CurrentPartitionIndex].Length)}";
                    double freeSpacePercentage = (double)image.PartitionTable.Partitions[CurrentPartitionIndex].FileSystem.TotalFreeSpace / image.PartitionTable.Partitions[CurrentPartitionIndex].Length * 100;
                    lblStatusFreeCapacity.Text = $"Free space: {Settings.CurrentSettings.SizeUnit.FormatSize((ulong)image.PartitionTable.Partitions[CurrentPartitionIndex].FileSystem.TotalFreeSpace)} ({freeSpacePercentage / 100:p2})";
                    if (freeSpacePercentage <= 10)
                        lblStatusProgressBar.ProgressBar.SetState(ProgressBarState.Error); // Set the progress bar colour to red.
                    else if (freeSpacePercentage <= 20)
                        lblStatusProgressBar.ProgressBar.SetState(ProgressBarState.Paused); // Set the progress bar colour to yellow.
                    else
                        lblStatusProgressBar.ProgressBar.SetState(ProgressBarState.Normal); // Set the progress bar colour to green.

                    // Set progress bar value with a bit of a hack to disable the glow.
                    lblStatusProgressBar.Minimum = 100 - (int)freeSpacePercentage;
                    lblStatusProgressBar.Value = 100 - (int)freeSpacePercentage;
                    lblStatusProgressBar.Minimum = 0;
                }

                switch (StatusBarState)
                {
                    case StatusBarState.NoneSelected:
                        {
                            var dir = (TiDirectory)lstDirectories.SelectedNode.Tag;
                            lbStatusPath.Text = dir.FullName;
                            lblStatusSize.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize(CalculateDirSize())} in {GetFileCount()} item(s)";
                            break;
                        }
                    case StatusBarState.OneSelected:
                        {
                            var item = GetSelectedItemData(0);
                            lbStatusPath.Text = item.FullName;
                            if (item is TiDirectory dir)
                                lblStatusSize.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize(dir.GetSize(true, false))} in 1 item";
                            else
                                lblStatusSize.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize(item.Length)} in 1 item";
                            break;
                        }
                    case StatusBarState.MultipleSelected:
                        {
                            var dir = (TiDirectory)lstDirectories.SelectedNode.Tag;
                            var selectedSize = 0ul;
                            foreach (var entry in SelectedItems)
                            {
                                if (entry is TiDirectory subdir)
                                    selectedSize += subdir.GetSize(true, false);
                                else
                                    selectedSize += entry.Length;
                            }

                            lbStatusPath.Text = dir.FullName;
                            lblStatusSize.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize(selectedSize)} in {SelectedItems.Count()} item(s)";
                            break;
                        }
                }
            }
        }

        //Generates a random name for a subfolder in the temp folder, used during double-click, ENTER keypress and drag-n-drop extraction
        private static string GetRandomDirName()
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rand = new();
            StringBuilder sb = new("~");
            int index;

            for (int i = 0; i < 10; i++)
            {
                index = rand.Next(chars.Length);
                sb.Append(Convert.ToChar(chars[index]));
            }

            return sb.ToString();
        }
    }
}
