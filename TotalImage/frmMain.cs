using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TotalImage.FileSystems.FAT;
using TotalImage.ImageFormats;
using static Interop.Shell32;
using static Interop.User32;

namespace TotalImage
{
    public partial class frmMain : Form
    {
        public string filename = "";
        public string path = "";
        public bool unsavedChanges = false;
        public RawSector image;
        private ListViewColumnSorter sorter;

        public frmMain()
        {
            InitializeComponent();
        }

        #region Event Handlers
        private void frmMain_Load(object sender, EventArgs e)
        {
            Settings.Load();
            PopulateRecentList();

            sorter = new ListViewColumnSorter();
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
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                /* Inject the entire selected folder into the image */
                throw new NotImplementedException("This feature is not implemented yet");
            }
            fbd.Dispose();
        }

        //Shows a hex view of the current image
        private void hexView_Click(object sender, EventArgs e)
        {
            frmHexView frm = new frmHexView();
            frm.Show();
        }

        //Allows viewing and editing both volume labels
        private void changeVolumeLabel_Click(object sender, EventArgs e)
        {
            dlgChangeVolLabel dlg = new dlgChangeVolLabel(image.GetRDVolumeLabel(), image.GetBPBVolumeLabel());
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                unsavedChanges = true;
                saveToolStripButton.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
            }
            dlg.Dispose();
        }

        //Allows viewing and editing bootsector properties
        private void bootSectorProperties_Click(object sender, EventArgs e)
        {
            dlgBootSector dlg = new dlgBootSector();
            dlg.ShowDialog();
            dlg.Dispose();
        }

        //Shows current image information
        private void imageInformation_Click(object sender, EventArgs e)
        {
            dlgImageInfo dlg = new dlgImageInfo();
            dlg.ShowDialog();
            dlg.Dispose();
        }

        //Click event handler for all menu items in the Recent images menu
        private void recentImage_Click(object sender, EventArgs e)
        {
            CloseImage();
            string imagePath = ((ToolStripMenuItem)sender).Text.Substring(3, ((ToolStripMenuItem)sender).Text.Length - 3).Trim(' ');
            OpenImage(imagePath);
        }

        //Creates a new disk image
        private void newImage_Click(object sender, EventArgs e)
        {
            //This needs to be implemented properly...
            if (unsavedChanges)
            {
                DialogResult = MessageBox.Show("You have unsaved changes in the current image. Would you like to save them before creating a new image?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (DialogResult == DialogResult.Yes) /*save_Click(sender, e);*/;
                else if (DialogResult == DialogResult.Cancel) return;
            }

            if(image != null)
                CloseImage();
            image = new RawSector();
            dlgNewImage dlg = new dlgNewImage();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Text = "(Untitled) - TotalImage";
                unsavedChanges = true;
            }

            BiosParameterBlock bpb;
            if(dlg.BPBVersion == BiosParameterBlockVersion.Dos34 || dlg.BPBVersion == BiosParameterBlockVersion.Dos40)
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

            image.CreateImage(bpb, dlg.TracksPerSide, dlg.WriteBPB);
            EnableUI();
            dlg.Dispose();
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
            dlgNewFolder dlg = new dlgNewFolder();
            dlg.ShowDialog();
            dlg.Dispose();
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

        //Checks if a deleted file or folder can be undeleted and if so, offers to undelete it
        private void undelete_Click(object sender, EventArgs e)
        {
            dlgUndelete dlg = new dlgUndelete();
            dlg.ShowDialog();
            dlg.Dispose();
        }

        //Renames a file or folder
        private void rename_Click(object sender, EventArgs e)
        {
            string oldname = "";
            if (lstFiles.Focused)
                oldname = lstFiles.SelectedItems[0].Text;
            else if (lstDirectories.Focused)
                oldname = lstDirectories.SelectedNode.Text;

            dlgRename dlg = new dlgRename(oldname);
            dlg.ShowDialog();

            string newname = dlg.NewName;
            /* This is where the item gets actually renamed */

            dlg.Dispose();
        }

        //Changes image format
        private void changeFormat_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException("This feature is not implemented yet");
        }

        //Defragments the selected partition
        private void defragment_Click(object sender, EventArgs e)
        {
            dlgDefragment dlg = new dlgDefragment();
            dlg.ShowDialog();
            dlg.Dispose();
        }

        //Formats the selected partition
        private void format_Click(object sender, EventArgs e)
        {
            dlgFormat dlg = new dlgFormat();
            dlg.ShowDialog();
            dlg.Dispose();
        }

        //Save the changes made to the current image since the last save or since it was opened
        private void save_Click(object sender, EventArgs e)
        {
            byte[] imageBytes = image.GetImageBytes();
            File.WriteAllBytes(path, imageBytes);

            saveToolStripButton.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            Text = filename + " - TotalImage";
            unsavedChanges = false;
        }

        //Saves the current image as a new file, along with any changes made to it since the last save
        private void saveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
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
                    byte[] imageBytes = image.GetImageBytes();
                    File.WriteAllBytes(sfd.FileName, imageBytes);
                }

                path = sfd.FileName;
                filename = Path.GetFileName(path);
                Text = filename + " - TotalImage";

                Settings.AddRecentImage(path);
                PopulateRecentList();
            }

            sfd.Dispose();
        }

        //Closes the application
        private void exit_Click(object sender, EventArgs e)
        {
            if (unsavedChanges)
            {
                DialogResult = MessageBox.Show("You have unsaved changed. Would you like to save them before closing TotalImage?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (DialogResult == DialogResult.Yes)
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

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AutoUpgradeEnabled = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;
            //ofd.ShowReadOnly = true; //We probably want this, but it degrades the dialog appearance to XP dialog... Needs a workaround
            ofd.Filter =
                "Raw sector image (*.img, *.ima, *.vfd, *.flp, *.dsk, *.xdf, *.hdm)|*.img;*.ima;*.vfd;*.flp;*.dsk;*.xdf;*.hdm|" +
                "All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CloseImage();
                OpenImage(ofd.FileName);
            }

            ofd.Dispose();
        }

        private void about_Click(object sender, EventArgs e)
        {
            dlgAbout dlg = new dlgAbout();
            dlg.ShowDialog();
            dlg.Dispose();
        }

        private void menuBarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            menuBar.Visible = menuBarToolStripMenuItem1.Checked;
            menuBarToolStripMenuItem.Checked = menuBarToolStripMenuItem1.Checked;
        }

        /* Extracts file(s) or folder(s) from the image to the specified path
         * This code is just a POC - needs to be improved to use all the selected options from the dialog */
        private void extract_Click(object sender, EventArgs e)
        {
            dlgExtract dlg = new dlgExtract();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (lstFiles.SelectedItems.Count == 1)
                {
                    DirectoryEntry entry = (DirectoryEntry)lstFiles.SelectedItems[0].Tag;
                    if (Convert.ToBoolean(entry.attr & 0x10))
                    {
                        throw new NotImplementedException("This feature is not implemented yet");
                    }
                    else
                    {
                        /* Extract just one file based on the selected options from the dialog
                         * Right now only the "Ignore folders" option works... */
                        if (dlg.ExtractType == Settings.FolderExtract.Ignore)
                        {
                            image.ExtractFile((DirectoryEntry)lstFiles.SelectedItems[0].Tag, dlg.TargetPath);
                        }
                        else
                        {
                            throw new NotImplementedException("This feature is not implemented yet");
                        }
                    }
                }
                else if (lstFiles.SelectedItems.Count > 1)
                {
                    foreach (ListViewItem lvi in lstFiles.SelectedItems)
                    {
                        DirectoryEntry entry = (DirectoryEntry)lvi.Tag;
                        if (Convert.ToBoolean(entry.attr & 0x10))
                        {
                            throw new NotImplementedException("This feature is not implemented yet");
                        }
                        else
                        {
                            /* Extract just one file based on the selected options from the dialog
                             * Right now only the "Ignore folders" option works... */
                            if (dlg.ExtractType == Settings.FolderExtract.Ignore)
                            {
                                image.ExtractFile((DirectoryEntry)lvi.Tag, dlg.TargetPath);
                            }
                            else
                            {
                                throw new NotImplementedException("This feature is not implemented yet");
                            }
                        }
                    }
                }
                if (dlg.OpenFolder)
                {
                    Process.Start(dlg.TargetPath);
                }
            }
            dlg.Dispose();
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItems.Count == 0 || lstFiles.SelectedItems.Count == 1 && lstFiles.SelectedItems[0].Text == "..")
            {
                deleteToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem1.Enabled = false;
                extractToolStripMenuItem.Enabled = false;
                extractToolStripMenuItem2.Enabled = false;
                propertiesToolStripMenuItem.Enabled = false;
                propertiesToolStripMenuItem2.Enabled = false;
                renameToolStripMenuItem1.Enabled = false;
                renameToolStripMenuItem.Enabled = false;

                deleteToolStripButton.Enabled = false;
                extractToolStripButton.Enabled = false;
                propertiesToolStripButton.Enabled = false;

                lbStatuslPath.Text = lstDirectories.SelectedNode.FullPath + lstDirectories.PathSeparator;
                lblStatusSize.Text = string.Format("{0:n0}", CalculateDirSize()).ToString() + " bytes in " + GetFileCount() + " item(s)";
            }
            else if (lstFiles.SelectedItems.Count == 1)
            {
                deleteToolStripMenuItem.Enabled = true;
                deleteToolStripMenuItem1.Enabled = true;
                extractToolStripMenuItem.Enabled = true;
                extractToolStripMenuItem2.Enabled = true;
                propertiesToolStripMenuItem.Enabled = true;
                propertiesToolStripMenuItem2.Enabled = true;
                renameToolStripMenuItem1.Enabled = true;
                renameToolStripMenuItem.Enabled = true;

                deleteToolStripButton.Enabled = true;
                extractToolStripButton.Enabled = true;
                propertiesToolStripButton.Enabled = true;

                lbStatuslPath.Text = lstDirectories.SelectedNode.FullPath + lstDirectories.PathSeparator + lstFiles.SelectedItems[0].Text;
                lblStatusSize.Text = string.Format("{0:n0}", ((DirectoryEntry)lstFiles.SelectedItems[0].Tag).fileSize).ToString() + " bytes in 1 item";

            }
            else
            {
                deleteToolStripMenuItem.Enabled = true;
                deleteToolStripMenuItem1.Enabled = true;
                extractToolStripMenuItem.Enabled = true;
                extractToolStripMenuItem2.Enabled = true;
                propertiesToolStripMenuItem.Enabled = false;
                propertiesToolStripMenuItem2.Enabled = false;
                renameToolStripMenuItem1.Enabled = false;
                renameToolStripMenuItem.Enabled = false;

                deleteToolStripButton.Enabled = true;
                extractToolStripButton.Enabled = true;
                propertiesToolStripButton.Enabled = false;

                lbStatuslPath.Text = lstDirectories.SelectedNode.FullPath + lstDirectories.PathSeparator;

                uint selectedSize = 0;
                foreach (ListViewItem lvi in lstFiles.SelectedItems)
                {
                    DirectoryEntry entry = (DirectoryEntry)lvi.Tag;
                    selectedSize += entry.fileSize;
                }

                lblStatusSize.Text = string.Format("{0:n0}", selectedSize) + " bytes in " + lstFiles.SelectedItems.Count + " items";
            }
        }

        private void cmsFileList_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lstFiles.SelectedItems.Count == 0 || lstFiles.SelectedItems[0].Text == "..")
            {
                e.Cancel = true;
                return;
            }
        }

        private void menuBar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == editToolStripMenuItem)
            {
                if (lstFiles.SelectedItems.Count == 0)
                {
                    deleteToolStripMenuItem.Enabled = false;
                    extractToolStripMenuItem.Enabled = false;
                    propertiesToolStripMenuItem.Enabled = false;
                }
                else
                {
                    deleteToolStripMenuItem.Enabled = true;
                    extractToolStripMenuItem.Enabled = true;
                    propertiesToolStripMenuItem.Enabled = true;
                }
            }
        }

        private void managePartitions_Click(object sender, EventArgs e)
        {
            dlgManagePart dlg = new dlgManagePart();
            dlg.ShowDialog();
            dlg.Dispose();
        }

        private void settings_Click(object sender, EventArgs e)
        {
            dlgSettings dlg = new dlgSettings();
            dlg.ShowDialog();
            dlg.Dispose();
        }

        private void properties_Click(object sender, EventArgs e)
        {
            dlgProperties dlg = new dlgProperties((DirectoryEntry)lstFiles.SelectedItems[0].Tag);
            dlg.ShowDialog();
            dlg.Dispose();
        }

        private void injectFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
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

            ofd.Dispose();
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

        /* TO BE REWRITTEN ACCORDING TO NEW FILE SYSTEM CLASSES */
        private void lstDirectories_AfterSelect(object sender, TreeViewEventArgs e)
        {
            uint fileCount = 0;
            uint dirSize = 0;
            lstFiles.BeginUpdate();
            //Subdirs
            if (e.Node != lstDirectories.Nodes[0])
            {
                lstFiles.Items.Clear();
                image.ListDirectory((DirectoryEntry)e.Node.Tag);
            }
            //Root dir
            else
            {
                lstFiles.Items.Clear();
                image.ListRootDirectory();
            }
            lstFiles.EndUpdate();

            foreach (ListViewItem lvi in lstFiles.Items)
            {
                DirectoryEntry entry = (DirectoryEntry)lvi.Tag;
                if (!Convert.ToBoolean(entry.attr & 0x10))
                {
                    fileCount++;
                    dirSize += entry.fileSize;
                }
            }
            lblStatusSize.Text = string.Format("{0:n0}", dirSize).ToString() + " bytes in " + fileCount + " file(s)";
            lbStatuslPath.Text = lstDirectories.SelectedNode.FullPath + lstDirectories.PathSeparator;
        }

        /* TO BE REWRITTEN ACCORDING TO NEW FILE SYSTEM CLASSES */
        private void lstFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstFiles.SelectedItems.Count == 1)
            {
                if (lstFiles.SelectedItems[0].Text == "..")
                {
                    lstDirectories.SelectedNode = lstDirectories.SelectedNode.Parent;
                }
                else
                {
                    if (Convert.ToBoolean(((DirectoryEntry)lstFiles.SelectedItems[0].Tag).attr & 0x10)) //A folder was double-clicked
                    {
                        uint sc = (((uint)((DirectoryEntry)lstFiles.SelectedItems[0].Tag).fstClusHI) << 16) + ((DirectoryEntry)lstFiles.SelectedItems[0].Tag).fstClusLO;
                        TreeNode node = FindNode(lstDirectories.SelectedNode, sc);
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

            Settings.FilesSortingColumn = lstFiles.Columns.IndexOfKey("Type");
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

            Settings.FilesSortingColumn = lstFiles.Columns.IndexOfKey("Modified");
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

            Settings.FilesSortingColumn = lstFiles.Columns.IndexOfKey("Size");
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
        //Needs improvement, but the gist of it is there...
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
        //Needs improvement, but the gist of it is there...
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

        private void cmsDirTree_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
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
                    extractToolStripMenuItem1.Enabled = true;
                    newFolderToolStripMenuItem1.Enabled = true;
                    renameToolStripMenuItem1.Enabled = true;
                    propertiesToolStripMenuItem1.Enabled = true;
                    undeleteToolStripMenuItem1.Enabled = false;
                    deleteToolStripMenuItem1.Enabled = true;
                }
            }
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
            throw new NotImplementedException("This feature is not implemented yet");
        }

        private void showDeletedItems_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException("This feature is not implemented yet");
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
        #endregion

        /* TO BE REWRITTEN ACCORDING TO NEW FILE SYSTEM CLASSES */
        private void OpenImage(string path)
        {
            filename = Path.GetFileName(path);
            Text = filename + " - TotalImage";
            lstDirectories.Nodes.Clear();
            lstFiles.Items.Clear();

            TreeNode root = new TreeNode("\\");
            root.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
            root.SelectedImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
            lstDirectories.Nodes.Add(root);
            image = new RawSector();
            lstFiles.ListViewItemSorter = null;
            lstFiles.BeginUpdate();
            lstDirectories.BeginUpdate();
            image.LoadImage(path);
            lstDirectories.EndUpdate();
            SortDirTree();
            lstDirectories.SelectedNode = lstDirectories.Nodes[0];
            lstFiles.EndUpdate();
            lstFiles.ListViewItemSorter = sorter;
            lblStatusCapacity.Text = GetImageCapacity() + " KiB";
            EnableUI();

            Settings.AddRecentImage(path);
            PopulateRecentList();
        }

        /* Returns size of directory
         * This needs to be moved to the appropriate file system classes and extended with the option to include subdirs as well. */
        private uint CalculateDirSize()
        {
            uint dirSize = 0;

            foreach (ListViewItem lvi in lstFiles.Items)
            {
                DirectoryEntry entry = (DirectoryEntry)lvi.Tag;
                if (!Convert.ToBoolean(entry.attr & 0x10))
                {
                    dirSize += entry.fileSize;
                }
            }

            return dirSize;
        }

        /* Returns the number of files in a directory
         * This needs to be moved to the appropriate file system classes and extended with the option to include subdirs as well. */
        private uint GetFileCount()
        {
            uint fileCount = 0;

            foreach (ListViewItem lvi in lstFiles.Items)
            {
                DirectoryEntry entry = (DirectoryEntry)lvi.Tag;
                if (!Convert.ToBoolean(entry.attr & 0x10))
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

        //Adds a new node to the root directory in the directory tree
        public void AddToRootDir(DirectoryEntry entry)
        {
            string filename = entry.name.TrimEnd('.');
            TreeNode node = new TreeNode(filename);
            node.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
            node.Tag = entry;
            lstDirectories.Nodes[0].Nodes.Add(node);
        }

        public void SortDirTree()
        {
            lstDirectories.Sort();
        }

        //Finds the node with the specified entry
        private TreeNode FindNode(TreeNode startNode, uint startCluster)
        {
            foreach (TreeNode node in startNode.Nodes)
            {
                uint sc = ((uint)((DirectoryEntry)node.Tag).fstClusHI << 16) + ((DirectoryEntry)node.Tag).fstClusLO;

                if (sc == startCluster)
                {
                    return node;
                }
                else
                {
                    TreeNode nodeChild = FindNode(node, startCluster);
                    if (nodeChild != null)
                    {
                        return nodeChild;
                    }
                }
            }

            return null;
        }

        public void AddToDir(DirectoryEntry parent, DirectoryEntry child)
        {
            string childFilename = child.name.TrimEnd('.');
            TreeNode childNode = new TreeNode(childFilename);
            childNode.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
            childNode.Tag = child;
            TreeNode parentNode = FindNode(lstDirectories.Nodes[0], ((uint)(parent.fstClusHI << 16) + parent.fstClusLO));
            if (parentNode != null)
            {
                parentNode.Nodes.Add(childNode);
            }
            else
            {
                throw new Exception("Parent node not found");
            }
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

        /* TO BE REWRITTEN ACCORDING TO NEW FILE SYSTEM CLASSES
         * Adds a new item to the file list */
        public void AddToFileList(DirectoryEntry entry)
        {
            ListViewItem lvi = new ListViewItem();
            if (entry.name.Substring(0, 2) != "..")
            {
                string filename = entry.name.TrimEnd('.');
                lvi.Text = filename;
                ushort year = (ushort)(((entry.wrtDate & 0xFE00) >> 9) + 1980);
                byte month = (byte)((entry.wrtDate & 0x1E0) >> 5);
                byte day = (byte)(entry.wrtDate & 0x1F);
                byte hours = (byte)((entry.wrtTime & 0xF800) >> 11);
                byte minutes = (byte)((entry.wrtTime & 0x7E0) >> 5);
                byte seconds = (byte)((entry.wrtTime & 0x1F) * 2); //Resolution for seconds is 2s

                if (month <= 0 || month >= 13) month = 1;
                if (day <= 0 || day >= 31) day = 1; //We don't bother checking for February 31st etc. yet...

                if (Convert.ToBoolean(entry.attr & 0x10))
                {
                    string filetype = GetShellFileType(filename, FileAttributes.Directory);
                    lvi.SubItems.Add(filetype);
                    lvi.SubItems.Add("");
                    lvi.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
                }
                else
                {
                    string filetype = GetShellFileType(filename, FileAttributes.Normal);
                    lvi.SubItems.Add(filetype);
                    lvi.SubItems.Add(string.Format("{0:n0}", entry.fileSize).ToString() + " B");

                    //This will only add a new icon to the list if the associated type hasn't been encountered yet
                    if (!imgFilesSmall.Images.ContainsKey(filetype))
                    {
                        Icon icon = GetShellFileIcon(filename, false, FileAttributes.Normal);
                        imgFilesSmall.Images.Add(filetype, icon);
                        icon = GetShellFileIcon(filename, true, FileAttributes.Normal);
                        imgFilesLarge.Images.Add(filetype, icon);
                    }
                    lvi.ImageIndex = imgFilesSmall.Images.IndexOfKey(filetype);
                }

                DateTime date = new DateTime(year, month, day, hours, minutes, seconds);
                lvi.SubItems.Add(date.ToString());
            }
            else //The ".." virtual folder
            {
                lvi.Text = "..";
                lvi.ImageIndex = 0;
                lvi.SubItems.Add("");
                lvi.SubItems.Add("");
                lvi.SubItems.Add("");
            }
            lvi.Tag = entry;
            lstFiles.Items.Add(lvi);
        }

        /* TO BE REWRITTEN ACCORDING TO NEW FILE SYSTEM CLASSES */
        private uint GetImageCapacity()
        {
            return 0;
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

            //Not available for floppies - once there's HDD support, this code needs to be adjusted accordingly...
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
            if(image != null)
                image.CloseImage();
            image = null;
            lstDirectories.Nodes.Clear();
            lstFiles.Items.Clear();
            DisableUI();
        }
    }
}