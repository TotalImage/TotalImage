using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TotalImage.FileSystems.FAT;
using TotalImage.Containers;
using TotalImage.DiskGeometries;
using static Interop.Shell32;
using static Interop.User32;

namespace TotalImage
{
    public partial class frmMain : Form
    {
        public string filename = "";
        public string path = "";
        public bool unsavedChanges = false;
        public Container? image;
        private readonly ListViewColumnSorter sorter = new ListViewColumnSorter();

        public frmMain()
        {
            InitializeComponent();
        }

        #region Event Handlers
        private void frmMain_Load(object sender, EventArgs e)
        {
            Settings.Load();
            PopulateRecentList();

            lstFiles.ListViewItemSorter = sorter;

            //Because designer doesn't have the Enter key in the list for some reason...
            propertiesToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.Enter;
            propertiesToolStripMenuItem1.ShortcutKeys = Keys.Alt | Keys.Enter;
            propertiesToolStripMenuItem2.ShortcutKeys = Keys.Alt | Keys.Enter;

#if !DEBUG
                DisableUI(); //Once support for command line arguments is added, those will need to be checked before this is done...
#endif
            GetFolderIcon();
            lstDirectories.SelectedImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
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
            frm.Show();
        }

        //Allows viewing and editing both volume labels
        //TODO: Actually change the volume labels
        private void changeVolumeLabel_Click(object sender, EventArgs e)
        {
            if (!(image?.PartitionTable.Partitions[0].FileSystem is Fat12 fs))
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
            CloseImage();
            string imagePath = ((ToolStripMenuItem)sender).Text.Substring(3, ((ToolStripMenuItem)sender).Text.Length - 3).Trim(' ');
            OpenImage(imagePath);
        }

        //Creates a new disk image
        //TODO: Implement the "save changes first" code path
        private void newImage_Click(object sender, EventArgs e)
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
            using dlgNewImage dlg = new dlgNewImage();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Text = "(Untitled) - TotalImage";
                unsavedChanges = true;

                BiosParameterBlock bpb;
                if (dlg.BPBVersion == BiosParameterBlockVersion.Dos34 || dlg.BPBVersion == BiosParameterBlockVersion.Dos40)
                    bpb = new BiosParameterBlock40() { BpbVersion = dlg.BPBVersion };
                else
                    bpb = new BiosParameterBlock() { BpbVersion = dlg.BPBVersion };

                bpb.OemId = dlg.OEMID.ToUpper();
                bpb.BytesPerLogicalSector = dlg.BytesPerSector;
                bpb.HiddenSectors = 0;
                bpb.LargeTotalLogicalSectors = 0;
                bpb.LogicalSectorsPerCluster = dlg.SectorsPerCluster;
                bpb.LogicalSectorsPerFAT = dlg.SectorsPerFAT;
                bpb.MediaDescriptor = dlg.MediaDescriptor;
                bpb.NumberOfFATs = dlg.NumberOfFATs;
                bpb.NumberOfHeads = dlg.NumberOfSides;
                bpb.PhysicalSectorsPerTrack = dlg.SectorsPerTrack;
                bpb.ReservedLogicalSectors = dlg.ReservedSectors;
                bpb.RootDirectoryEntries = dlg.RootDirEntries;
                bpb.TotalLogicalSectors = dlg.TotalSectors;

                if (bpb is BiosParameterBlock40 bpb40) //DOS 3.4+ BPB
                {
                    bpb40.PhysicalDriveNumber = 0;
                    bpb40.Flags = 0;
                    bpb40.VolumeSerialNumber = uint.Parse(dlg.SerialNumber, NumberStyles.HexNumber);

                    if (bpb40.BpbVersion == BiosParameterBlockVersion.Dos40)
                    {
                        bpb40.FileSystemType = dlg.FileSystemType.ToUpper();
                        bpb40.VolumeLabel = dlg.VolumeLabel.ToUpper();
                    }
                }

                image = RawContainer.CreateImage(bpb, dlg.TracksPerSide, dlg.WriteBPB);
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
            largeIconsToolStripMenuItem.Checked = true;
            largeIconsToolStripMenuItem1.Checked = true;
            smallIconsToolStripMenuItem.Checked = false;
            smallIconsToolStripMenuItem1.Checked = false;
            detailsToolStripMenuItem.Checked = false;
            detailsToolStripMenuItem1.Checked = false;
            tilesToolStripMenuItem.Checked = false;
            tilesToolStripMenuItem1.Checked = false;
            listToolStripMenuItem.Checked = false;
            listToolStripMenuItem1.Checked = false;
            lstFiles.View = View.LargeIcon;
            Settings.FilesView = View.LargeIcon;
        }

        private void viewSmallIcons_Click(object sender, EventArgs e)
        {
            largeIconsToolStripMenuItem.Checked = false;
            largeIconsToolStripMenuItem1.Checked = false;
            smallIconsToolStripMenuItem.Checked = true;
            smallIconsToolStripMenuItem1.Checked = true;
            detailsToolStripMenuItem.Checked = false;
            detailsToolStripMenuItem1.Checked = false;
            tilesToolStripMenuItem.Checked = false;
            tilesToolStripMenuItem1.Checked = false;
            listToolStripMenuItem.Checked = false;
            listToolStripMenuItem1.Checked = false;
            lstFiles.View = View.SmallIcon;
            Settings.FilesView = View.SmallIcon;
        }

        private void viewList_Click(object sender, EventArgs e)
        {
            largeIconsToolStripMenuItem.Checked = false;
            largeIconsToolStripMenuItem1.Checked = false;
            smallIconsToolStripMenuItem.Checked = false;
            smallIconsToolStripMenuItem1.Checked = false;
            detailsToolStripMenuItem.Checked = false;
            detailsToolStripMenuItem1.Checked = false;
            tilesToolStripMenuItem.Checked = false;
            tilesToolStripMenuItem1.Checked = false;
            listToolStripMenuItem.Checked = true;
            listToolStripMenuItem1.Checked = true;
            lstFiles.View = View.List;
            Settings.FilesView = View.List;
        }

        private void viewDetails_Click(object sender, EventArgs e)
        {
            largeIconsToolStripMenuItem.Checked = false;
            largeIconsToolStripMenuItem1.Checked = false;
            smallIconsToolStripMenuItem.Checked = false;
            smallIconsToolStripMenuItem1.Checked = false;
            detailsToolStripMenuItem.Checked = true;
            detailsToolStripMenuItem1.Checked = true;
            tilesToolStripMenuItem.Checked = false;
            tilesToolStripMenuItem1.Checked = false;
            listToolStripMenuItem.Checked = false;
            listToolStripMenuItem1.Checked = false;
            lstFiles.View = View.Details;
            Settings.FilesView = View.Details;
        }

        private void viewTiles_Click(object sender, EventArgs e)
        {
            largeIconsToolStripMenuItem.Checked = false;
            largeIconsToolStripMenuItem1.Checked = false;
            smallIconsToolStripMenuItem.Checked = false;
            smallIconsToolStripMenuItem1.Checked = false;
            detailsToolStripMenuItem.Checked = false;
            detailsToolStripMenuItem1.Checked = false;
            tilesToolStripMenuItem.Checked = true;
            tilesToolStripMenuItem1.Checked = true;
            listToolStripMenuItem.Checked = false;
            listToolStripMenuItem1.Checked = false;
            lstFiles.View = View.Tile;
            Settings.FilesView = View.Tile;
        }

        //Deletes a file or folder
        //TODO: Implement deletion here and in the FS/container
        private void delete_Click(object sender, EventArgs e)
        {
            if (lstFiles.Focused)
            {
                if (lstFiles.SelectedItems.Count == 1)
                {
                    DialogResult = MessageBox.Show("Are you sure you want to delete 1 item occupying X bytes?", "Delete item", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DialogResult == DialogResult.No || DialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else if (lstFiles.SelectedItems.Count > 1)
                {
                    //First get the total size of all selected items
                    DialogResult = MessageBox.Show("Are you sure you want to delete " + lstFiles.SelectedItems.Count + " items occupying X bytes?", "Delete items", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DialogResult == DialogResult.No || DialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                foreach (ListViewItem lvi in lstFiles.SelectedItems)
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
            string oldname = "";
            if (lstFiles.Focused)
                oldname = lstFiles.SelectedItems[0].Text;
            else if (lstDirectories.Focused)
                oldname = lstDirectories.SelectedNode.Text;

            using dlgRename dlg = new dlgRename(oldname);
            dlg.ShowDialog();

            string newname = dlg.NewName;
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

            image.SaveImage(path);

            saveToolStripButton.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            Text = filename + " - TotalImage";
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
                }

                path = sfd.FileName;
                filename = Path.GetFileName(path);
                Text = filename + " - TotalImage";

                Settings.AddRecentImage(path);
                PopulateRecentList();
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
            commandBarToolStripMenuItem.Checked = commandBar.Visible;
            commandBarToolStripMenuItem1.Checked = commandBar.Visible;
            Settings.ShowCommandBar = commandBar.Visible;
        }

        private void toggleDirectoryTree_Click(object sender, EventArgs e)
        {
            splitContainer.Panel1Collapsed = !splitContainer.Panel1Collapsed;
            directoryTreeToolStripMenuItem.Checked = !splitContainer.Panel1Collapsed;
            directoryTreeToolStripMenuItem1.Checked = !splitContainer.Panel1Collapsed;
            Settings.ShowDirectoryTree = !splitContainer.Panel1Collapsed;
        }

        private void toggleFileList_Click(object sender, EventArgs e)
        {
            splitContainer.Panel2Collapsed = !splitContainer.Panel2Collapsed;
            fileListToolStripMenuItem.Checked = !splitContainer.Panel2Collapsed;
            fileListToolStripMenuItem1.Checked = !splitContainer.Panel2Collapsed;
            Settings.ShowFileList = !splitContainer.Panel2Collapsed;
        }

        private void toggleStatusBar_Click(object sender, EventArgs e)
        {
            statusBar.Visible = !statusBar.Visible;
            statusBarToolStripMenuItem.Checked = statusBar.Visible;
            statusBarToolStripMenuItem1.Checked = statusBar.Visible;
            Settings.ShowStatusBar = statusBar.Visible;
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

        private void menuBarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            menuBar.Visible = menuBarToolStripMenuItem1.Checked;
            menuBarToolStripMenuItem.Checked = menuBarToolStripMenuItem1.Checked;
        }

        //Extracts file(s) or folder(s) from the image to the specified path
        //TODO: Implement this here, in the extraction dialog and in FS/container.
        private void extract_Click(object sender, EventArgs e)
        {
            // dlgExtract dlg = new dlgExtract();
            // if (dlg.ShowDialog() == DialogResult.OK)
            // {
            //     if (lstFiles.SelectedItems.Count == 1)
            //     {
            //         DirectoryEntry entry = (DirectoryEntry)lstFiles.SelectedItems[0].Tag;
            //         if (Convert.ToBoolean(entry.attr & 0x10))
            //         {
            //             throw new NotImplementedException("This feature is not implemented yet");
            //         }
            //         else
            //         {
            //             /* Extract just one file based on the selected options from the dialog
            //              * Right now only the "Ignore folders" option works... */
            //             if (dlg.ExtractType == Settings.FolderExtract.Ignore)
            //             {
            //                 image.ExtractFile((DirectoryEntry)lstFiles.SelectedItems[0].Tag, dlg.TargetPath);
            //             }
            //             else
            //             {
            //                 throw new NotImplementedException("This feature is not implemented yet");
            //             }
            //         }
            //     }
            //     else if (lstFiles.SelectedItems.Count > 1)
            //     {
            //         foreach (ListViewItem lvi in lstFiles.SelectedItems)
            //         {
            //             DirectoryEntry entry = (DirectoryEntry)lvi.Tag;
            //             if (Convert.ToBoolean(entry.attr & 0x10))
            //             {
            //                 throw new NotImplementedException("This feature is not implemented yet");
            //             }
            //             else
            //             {
            //                 /* Extract just one file based on the selected options from the dialog
            //                  * Right now only the "Ignore folders" option works... */
            //                 if (dlg.ExtractType == Settings.FolderExtract.Ignore)
            //                 {
            //                     image.ExtractFile((DirectoryEntry)lvi.Tag, dlg.TargetPath);
            //                 }
            //                 else
            //                 {
            //                     throw new NotImplementedException("This feature is not implemented yet");
            //                 }
            //             }
            //         }
            //     }
            //     if (dlg.OpenFolder)
            //     {
            //         Process.Start(dlg.TargetPath);
            //     }
            // }
            // dlg.Dispose();

            throw new NotImplementedException("This feature is not implemented yet");
        }

        //TODO: Implement status bar stuff based on the selected item in the listview. This includes proper path, size, etc.
        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItems.Count == 0 || lstFiles.SelectedItems.Count == 1 && lstFiles.SelectedItems[0].Text == "..")
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

                lbStatuslPath.Text = lstDirectories.SelectedNode.FullPath + lstDirectories.PathSeparator;
                lblStatusSize.Text = string.Format("{0:n0} bytes in {1} items", CalculateDirSize(), GetFileCount());
            }
            else if (lstFiles.SelectedItems.Count == 1)
            {
                propertiesToolStripMenuItem.Enabled = true;
                propertiesToolStripMenuItem2.Enabled = true;

                //Check if selected item is a deleted entry and enable the UI accordingly
                FileSystems.FileSystemObject entry = (FileSystems.FileSystemObject)lstFiles.SelectedItems[0].Tag;
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

                lbStatuslPath.Text = ((FileSystems.FileSystemObject)lstFiles.SelectedItems[0].Tag).FullName;
                lblStatusSize.Text = string.Format("{0:n0} bytes in 1 item", ((FileSystems.FileSystemObject)lstFiles.SelectedItems[0].Tag).Length);
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

                lbStatuslPath.Text = lstDirectories.SelectedNode.FullPath + lstDirectories.PathSeparator;

                var selectedSize = 0ul;
                foreach (ListViewItem lvi in lstFiles.SelectedItems)
                {
                    FileSystems.FileSystemObject entry = (FileSystems.FileSystemObject)lvi.Tag;
                    selectedSize += entry.Length;
                }

                lblStatusSize.Text = string.Format("{0:n0} bytes in {1} items", selectedSize, lstFiles.SelectedItems.Count);
            }
        }

        private void cmsFileList_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lstFiles.SelectedItems.Count == 0 || lstFiles.SelectedItems.Count == 1 && lstFiles.SelectedItems[0].Text == "..")
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
            dlg.ShowDialog();
        }

        //TODO: Implement the Properties dialog for multiple selected objects like Windows does it
        private void properties_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (item.GetCurrentParent() == cmsFileList && lstFiles.SelectedItems.Count > 0)
            {
                //Right now we only support showing the Properties dialog for a single item. Support for multiple items' properties
                //needs to be implemented here and in the dialog itself.
                if (lstFiles.SelectedItems.Count == 1)
                {
                    using dlgProperties dlg = new dlgProperties((FileSystems.FileSystemObject)lstFiles.SelectedItems[0].Tag);
                    dlg.ShowDialog();
                }
                else
                {
                    throw new NotImplementedException("This feature is not implemented yet.");
                }
            }
            else if(item.GetCurrentParent() == cmsDirTree)
            {
                using dlgProperties dlg = new dlgProperties((FileSystems.FileSystemObject)lstDirectories.SelectedNode.Tag);
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

        private void lstFiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == sorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (sorter.Order == SortOrder.Ascending)
                {
                    sorter.Order = SortOrder.Descending;
                }
                else
                {
                    sorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                sorter.SortColumn = e.Column;
                sorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lstFiles.Sort();
            lstFiles.SetSortIcon(sorter.SortColumn, sorter.Order);
        }

        //This prevents the user from opening a deleted directory (since we don't even know yet if it's recoverable, or what was inside, etc.)
        private void lstDirectories_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            FileSystems.Directory dir = (FileSystems.Directory)e.Node.Tag;
            if (dir.Name.StartsWith("?"))
            {
                e.Cancel = true;
                return;
            }
        }

        private void lstDirectories_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var fileCount = 0ul;
            var dirSize = 0ul;
            lstFiles.BeginUpdate();
            lstFiles.Items.Clear();
            PopulateListView((FileSystems.Directory)e.Node.Tag);
            lstFiles.EndUpdate();

#if NET5_0
            //.NET 5 workaround because they broke ImageList...
            lstFiles.LargeImageList = null;
            lstFiles.LargeImageList = imgFilesLarge;
            lstFiles.SmallImageList = null;
            lstFiles.SmallImageList = imgFilesSmall;
#endif

            foreach (ListViewItem lvi in lstFiles.Items)
            {
                var entry = (FileSystems.FileSystemObject)lvi.Tag;
                if (!(entry is FileSystems.Directory))
                {
                    fileCount++;
                    dirSize += entry.Length;
                }
            }
            lblStatusSize.Text = string.Format("{0:n0} bytes in {1} file(s)", dirSize, fileCount);
            lbStatuslPath.Text = lstDirectories.SelectedNode.FullPath + lstDirectories.PathSeparator;

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
                    propertiesToolStripMenuItem1.Enabled = true;
                    undeleteToolStripMenuItem1.Enabled = false;
                    newFolderToolStripMenuItem1.Enabled = true;
                    extractToolStripMenuItem1.Enabled = true;
                }
                else
                {
                    FileSystems.FileSystemObject entry = (FileSystems.FileSystemObject)lstDirectories.SelectedNode.Tag;
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
            if (lstFiles.SelectedItems.Count == 1 && e.Button != MouseButtons.Right)
            {
                if ((FileSystems.FileSystemObject)lstFiles.SelectedItems[0].Tag is FileSystems.Directory dir) //A folder was double-clicked
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

        private void sortByType_Click(object sender, EventArgs e)
        {
            nameToolStripMenuItem.Checked = false;
            nameToolStripMenuItem1.Checked = false;
            typeToolStripMenuItem.Checked = true;
            typeToolStripMenuItem1.Checked = true;
            sizeToolStripMenuItem.Checked = false;
            sizeToolStripMenuItem1.Checked = false;
            modifiedToolStripMenuItem.Checked = false;
            modifiedToolStripMenuItem1.Checked = false;

            if (sorter.SortColumn == 1)
            {
                // Reverse the current sort direction for this column.
                if (sorter.Order == SortOrder.Ascending)
                {
                    sorter.Order = SortOrder.Descending;
                }
                else
                {
                    sorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                sorter.SortColumn = 1;
                sorter.Order = SortOrder.Ascending;
            }

            Settings.FilesSortingColumn = lstFiles.Columns.IndexOfKey("Type");

            // Perform the sort with these new sort options.
            lstFiles.Sort();
            lstFiles.SetSortIcon(sorter.SortColumn, sorter.Order);
        }

        private void sortByModified_Click(object sender, EventArgs e)
        {
            nameToolStripMenuItem.Checked = false;
            nameToolStripMenuItem1.Checked = false;
            typeToolStripMenuItem.Checked = false;
            typeToolStripMenuItem1.Checked = false;
            sizeToolStripMenuItem.Checked = false;
            sizeToolStripMenuItem1.Checked = false;
            modifiedToolStripMenuItem.Checked = true;
            modifiedToolStripMenuItem1.Checked = true;

            if (sorter.SortColumn == 3)
            {
                // Reverse the current sort direction for this column.
                if (sorter.Order == SortOrder.Ascending)
                {
                    sorter.Order = SortOrder.Descending;
                }
                else
                {
                    sorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                sorter.SortColumn = 3;
                sorter.Order = SortOrder.Ascending;
            }

            Settings.FilesSortingColumn = lstFiles.Columns.IndexOfKey("Modified");

            // Perform the sort with these new sort options.
            lstFiles.Sort();
            lstFiles.SetSortIcon(sorter.SortColumn, sorter.Order);
        }

        private void sortByName_Click(object sender, EventArgs e)
        {
            nameToolStripMenuItem.Checked = true;
            nameToolStripMenuItem1.Checked = true;
            typeToolStripMenuItem.Checked = false;
            typeToolStripMenuItem1.Checked = false;
            sizeToolStripMenuItem.Checked = false;
            sizeToolStripMenuItem1.Checked = false;
            modifiedToolStripMenuItem.Checked = false;
            modifiedToolStripMenuItem1.Checked = false;

            Settings.FilesSortingColumn = lstFiles.Columns.IndexOfKey("Name");

            if (sorter.SortColumn == 0)
            {
                // Reverse the current sort direction for this column.
                if (sorter.Order == SortOrder.Ascending)
                {
                    sorter.Order = SortOrder.Descending;
                }
                else
                {
                    sorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                sorter.SortColumn = 0;
                sorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lstFiles.Sort();
            lstFiles.SetSortIcon(sorter.SortColumn, sorter.Order);
        }

        private void sortBySize_Click(object sender, EventArgs e)
        {
            nameToolStripMenuItem.Checked = false;
            nameToolStripMenuItem1.Checked = false;
            typeToolStripMenuItem.Checked = false;
            typeToolStripMenuItem1.Checked = false;
            sizeToolStripMenuItem.Checked = true;
            sizeToolStripMenuItem1.Checked = true;
            modifiedToolStripMenuItem.Checked = false;
            modifiedToolStripMenuItem1.Checked = false;

            if (sorter.SortColumn == 2)
            {
                // Reverse the current sort direction for this column.
                if (sorter.Order == SortOrder.Ascending)
                {
                    sorter.Order = SortOrder.Descending;
                }
                else
                {
                    sorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                sorter.SortColumn = 2;
                sorter.Order = SortOrder.Ascending;
            }

            Settings.FilesSortingColumn = lstFiles.Columns.IndexOfKey("Size");

            // Perform the sort with these new sort options.
            lstFiles.Sort();
            lstFiles.SetSortIcon(sorter.SortColumn, sorter.Order);
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
            foreach (ListViewItem lvi in lstFiles.Items)
            {
                lvi.Selected = true;
            }
        }

        private void lstFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else if (e.Data.GetDataPresent(typeof(ListViewItem)) || e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        //Opens an image that's been dragged and dropped onto the file list
        //TODO: Implement item movement for ListViewItem and TreeNode drag-n-drop
        private void lstFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                /* A file or folder is being moved within the listview */
                throw new NotImplementedException("This feature is not implemented yet");
            }
            else if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                /* A folder is being moved from the treeview to the listview. First needs to check if such a move is even legal;
                 * as this could potentially allow the user to move a parent folder into its own subfolder... */
                throw new NotImplementedException("This feature is not implemented yet");
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //Files are being dragged into the listview from outside the form
                string[] items = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                if (string.IsNullOrWhiteSpace(filename) && unsavedChanges == false) //No image is loaded
                {
                    if (items.Length == 1)
                        OpenImage(items[0]);
                }
                else if (!string.IsNullOrWhiteSpace(filename) || unsavedChanges) //An image is open (either saved or new)
                {
                    /* Inject files/folder instead */
                    throw new NotImplementedException("This feature is not implemented yet");
                }
            }
        }

        private void lstDirectories_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else if (e.Data.GetDataPresent(typeof(ListViewItem)) || e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        //Opens an image that's been dragged and dropped onto the dir tree
        //TODO: Implement item movement for ListViewItem and TreeNode drag-n-drop
        private void lstDirectories_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                /* A file or folder is being moved from the listview to the treeview */
                throw new NotImplementedException("This feature is not implemented yet");
            }
            else if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                /* A folder is being moved within the treeview. First needs to check if such a move is even legal;
                 * as this could potentially allow the user to move a parent folder into its own subfolder... */
                throw new NotImplementedException("This feature is not implemented yet");
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] items = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                if (string.IsNullOrWhiteSpace(filename) && unsavedChanges == false) //No image is loaded
                {
                    if (items.Length == 1)
                    {
                        OpenImage(items[0]);
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

        private void showHiddenItems_Click(object sender, EventArgs e)
        {
            Settings.ShowHiddenItems = !Settings.ShowHiddenItems;
            showHiddenToolStripMenuItem.Checked = Settings.ShowHiddenItems;
            showHiddenItemsToolStripMenuItem1.Checked = Settings.ShowHiddenItems;

            if (image != null)
            {
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
                lstDirectories.SelectedNode = lstDirectories.Nodes[0];

                lstFiles.BeginUpdate();
                lstFiles.ListViewItemSorter = null;
                lstFiles.Items.Clear();
                PopulateListView(image.PartitionTable.Partitions[0].FileSystem.RootDirectory);
                lstFiles.ListViewItemSorter = sorter;
                lstFiles.EndUpdate();

#if NET5_0
                //.NET 5 workaround because they broke ImageList...
                lstFiles.LargeImageList = null;
                lstFiles.LargeImageList = imgFilesLarge;
                lstFiles.SmallImageList = null;
                lstFiles.SmallImageList = imgFilesSmall;
#endif

                lblStatusCapacity.Text = "Dummy KiB";
            }
        }

        private void showDeletedItems_Click(object sender, EventArgs e)
        {
            Settings.ShowDeletedItems = !Settings.ShowDeletedItems;
            showDeletedToolStripMenuItem.Checked = Settings.ShowDeletedItems;
            showDeletedItemsToolStripMenuItem.Checked = Settings.ShowDeletedItems;

            if (image != null)
            {
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
                lstDirectories.SelectedNode = lstDirectories.Nodes[0];

                lstFiles.BeginUpdate();
                lstFiles.ListViewItemSorter = null;
                lstFiles.Items.Clear();
                PopulateListView(image.PartitionTable.Partitions[0].FileSystem.RootDirectory);
                lstFiles.ListViewItemSorter = sorter;
                lstFiles.EndUpdate();

#if NET5_0
                //.NET 5 workaround because they broke ImageList...
                lstFiles.LargeImageList = null;
                lstFiles.LargeImageList = imgFilesLarge;
                lstFiles.SmallImageList = null;
                lstFiles.SmallImageList = imgFilesSmall;
#endif

                lblStatusCapacity.Text = "Dummy KiB";
            }
        }

        private void lstFiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void lstDirectories_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
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
            nameToolStripMenuItem1.Checked = nameToolStripMenuItem.Checked = typeToolStripMenuItem.Checked = typeToolStripMenuItem1.Checked =
                sizeToolStripMenuItem.Checked = sizeToolStripMenuItem1.Checked = modifiedToolStripMenuItem.Checked =
                modifiedToolStripMenuItem1.Checked = false;
            switch (sorter.SortColumn)
            {
                case 0: nameToolStripMenuItem.Checked = nameToolStripMenuItem1.Checked = true; break;
                case 1: typeToolStripMenuItem.Checked = typeToolStripMenuItem1.Checked = true; break;
                case 2: sizeToolStripMenuItem.Checked = sizeToolStripMenuItem1.Checked = true; break;
                case 3: modifiedToolStripMenuItem.Checked = modifiedToolStripMenuItem1.Checked = true; break;
            }
        }

#endregion

        private void PopulateTreeView(TreeNode node, FileSystems.Directory dir)
        {
            foreach (var subdir in dir.EnumerateDirectories(Settings.ShowHiddenItems, Settings.ShowDeletedItems))
            {
                var subnode = new TreeNode(subdir.Name);
                subnode.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
                subnode.Tag = subdir;
                node.Nodes.Add(subnode);

                PopulateTreeView(subnode, subdir);
            }
        }

        private void PopulateListView(FileSystems.Directory dir)
        {
            if (dir.Parent != null)
            {
                //The ".." virtual folder
                var parentDirItem = new ListViewItem();
                parentDirItem.Text = "..";
                parentDirItem.ImageIndex = 0;
                parentDirItem.SubItems.Add("");
                parentDirItem.SubItems.Add("");
                parentDirItem.SubItems.Add("");
                parentDirItem.Tag = dir.Parent;
                lstFiles.Items.Add(parentDirItem);
            }

            foreach (var fso in dir.EnumerateFileSystemObjects(Settings.ShowHiddenItems, Settings.ShowDeletedItems))
            {
                var item = new ListViewItem();
                item.Text = fso.Name;

                var filetype = GetShellFileType(fso.Name, fso.Attributes);
                item.SubItems.Add(filetype);

                if (fso is FileSystems.Directory)
                {
                    item.SubItems.Add(string.Empty);
                    item.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
                }
                else
                {
                    item.SubItems.Add(string.Format("{0:n0} B", fso.Length).ToString());

                    //This will only add a new icon to the list if the associated type hasn't been encountered yet
                    if (!imgFilesSmall.Images.ContainsKey(filetype))
                    {
                        Icon icon = GetShellFileIcon(fso.Name, false, fso.Attributes);
                        imgFilesSmall.Images.Add(filetype, icon);
                        icon = GetShellFileIcon(fso.Name, true, fso.Attributes);
                        imgFilesLarge.Images.Add(filetype, icon);
                    }
                    item.ImageIndex = imgFilesSmall.Images.IndexOfKey(filetype);
                }

                item.SubItems.Add(fso.LastWriteTime.ToString());

                item.Tag = fso;
                lstFiles.Items.Add(item);
            }
        }

        //TODO: This needs some serious rethinking and probably restructuring.
        private void OpenImage(string path)
        {
            filename = Path.GetFileName(path);
            Text = filename + " - TotalImage";

            image = new RawContainer(path);

            var root = new TreeNode(@"\");
            root.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
            root.SelectedImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
            root.Tag = image.PartitionTable.Partitions[0].FileSystem.RootDirectory;
            PopulateTreeView(root, image.PartitionTable.Partitions[0].FileSystem.RootDirectory);

            lstDirectories.BeginUpdate();
            lstDirectories.Nodes.Clear();
            lstDirectories.Nodes.Add(root);
            lstDirectories.Sort();
            lstDirectories.EndUpdate();
            lstDirectories.SelectedNode = lstDirectories.Nodes[0];

            lstFiles.BeginUpdate();
            lstFiles.ListViewItemSorter = null;
            lstFiles.Items.Clear();
            PopulateListView(image.PartitionTable.Partitions[0].FileSystem.RootDirectory);
            lstFiles.ListViewItemSorter = sorter;
            lstFiles.EndUpdate();

#if NET5_0
            //.NET 5 workaround because they broke ImageList...
            lstFiles.LargeImageList = null;
            lstFiles.LargeImageList = imgFilesLarge;
            lstFiles.SmallImageList = null;
            lstFiles.SmallImageList = imgFilesSmall;
#endif

            lblStatusCapacity.Text = "Dummy KiB";
            EnableUI();

            Settings.AddRecentImage(path);
            PopulateRecentList();
        }

        /* Returns size of directory
         * TODO: Move to this to the appropriate file system class and implement support for subdirectories */
        private ulong CalculateDirSize()
        {
            var dirSize = 0ul;

            foreach (ListViewItem lvi in lstFiles.Items)
            {
                FileSystems.FileSystemObject entry = (FileSystems.FileSystemObject)lvi.Tag;
                if (!(entry is FileSystems.Directory))
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

            foreach (ListViewItem lvi in lstFiles.Items)
            {
                FileSystems.FileSystemObject entry = (FileSystems.FileSystemObject)lvi.Tag;
                if (!(entry is FileSystems.Directory))
                {
                    fileCount++;
                }
            }

            return fileCount;
        }

        //Gets the default Windows folder icon with SHGetFileInfo that will be used for folders
        public void GetFolderIcon()
        {
            Icon icon = GetShellFileIcon("C:\\Windows", false, FileAttributes.Directory);
            imgFilesSmall.Images.Add("folder", icon);
            icon = GetShellFileIcon("C:\\Windows", true, FileAttributes.Directory);
            imgFilesLarge.Images.Add("folder", icon);
        }

        //Finds the node with the specified entry
        private TreeNode? FindNode(TreeNode startNode, FileSystems.Directory dir)
        {
            if (((FileSystems.Directory)startNode.Tag).FullName == dir.FullName)
            {
                return startNode;
            }
            else foreach (TreeNode node in startNode.Nodes)
                {
                    // hack
                    if (((FileSystems.Directory)node.Tag).FullName == dir.FullName)
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

        //Obtains the fancy file type name
        public string GetShellFileType(string filename, FileAttributes attributes)
        {
            var shinfo = new SHFILEINFO();
            var flags = SHGFI.TYPENAME | SHGFI.USEFILEATTRIBUTES;

            if (SHGetFileInfo(filename, attributes, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags) == IntPtr.Zero)
            {
                return "File";
            }

            return shinfo.szTypeName;
        }

        //Obtains the icon for the file type
        public Icon GetShellFileIcon(string filename, bool GetLargeIcon, FileAttributes attributes)
        {
            var shinfo = new SHFILEINFO();
            var flags = SHGFI.ICON | SHGFI.USEFILEATTRIBUTES;
            if (GetLargeIcon)
                flags |= SHGFI.LARGEICON;
            else
                flags |= SHGFI.SMALLICON;

            SHGetFileInfo(filename, attributes, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);
            Icon icon = (Icon)Icon.FromHandle(shinfo.hIcon).Clone();
            DestroyIcon(shinfo.hIcon);
            return icon;
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

            imageInformationToolStripMenuItem.Enabled = true;
            hexViewToolStripMenuItem.Enabled = true;

            foreach (ToolStripItem item in editToolStripMenuItem.DropDownItems)
            {
                if (item.CanSelect)
                {
                    item.Enabled = true;
                }
            }

            //TODO: Once HDD support is implemented, update this code
            managePartitionsToolStripMenuItem.Enabled = false;
            selectPartitionToolStripMenuItem.Enabled = false;
            managePartitionsToolStripButton.Enabled = false;
            selectPartitionToolStripComboBox.Enabled = false;
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

            for (int i = Settings.RecentImages.Count - 1; i >= 0; i--)
            {
                ToolStripMenuItem newItem = new ToolStripMenuItem();
                newItem.Text = (Settings.RecentImages.Count - i).ToString() + ": " + Settings.RecentImages[i];
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
            path = "";
            image?.Dispose();
            image = null;
            lstDirectories.Nodes.Clear();
            lstFiles.Items.Clear();
            DisableUI();
        }
    }
}