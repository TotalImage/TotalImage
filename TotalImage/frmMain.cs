using System;
using System.Drawing;
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

        private void newToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            NewImage();
        }

        private void newToolStripButton_Click(object sender, System.EventArgs e)
        {
            NewImage();
        }

        private void NewImage()
        {
            //Needs a check for unsaved changes
            image = new RawSector();
            dlgNewImage dlg = new dlgNewImage();
            DialogResult result = dlg.ShowDialog();
            if(result == DialogResult.OK)
            {
                Text = "(Untitled) - TotalImage";
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveNewImage();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (unsavedChanges)
            {
                if (string.IsNullOrEmpty(filename)) //File hasn't been saved yet
                {
                    SaveNewImage();
                }
                else
                {
                    SaveChanges();
                }
            }
        }

        private void SaveNewImage()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AutoUpgradeEnabled = true;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            sfd.OverwritePrompt = true;
            sfd.Title = "Save image...";
            sfd.DefaultExt = "img";
            sfd.Filter = "Raw sector image (*.img, *.ima, *.vfd, *.flp, *.dsk, *.xdf, *.hdm)|*.img;*.ima;*.vfd;*.flp;*.dsk;*.xdf;*.hdm|" +
                /*"WinImage compressed image (*.imz)|*.imz|" +
                "DiskDupe image (*.ddi)|*.ddi|" +
                "Anex86 floppy disk image (*.fdi)|*.fdi|" +
                "86Box surface image (*.86f)|*.86f|" +*/
                "All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (sfd.FilterIndex == 0 || sfd.FileName.EndsWith(".img") || sfd.FileName.EndsWith(".ima") ||
                    sfd.FileName.EndsWith(".vfd") || sfd.FileName.EndsWith(".flp") || sfd.FileName.EndsWith(".dsk") ||
                    sfd.FileName.EndsWith(".hdm"))
                {
                    byte[] imageBytes = image.GetImageBytes();
                    System.IO.File.WriteAllBytes(sfd.FileName, imageBytes);
                }

                filename = Path.GetFileName(sfd.FileName);
                path = sfd.FileName;
                Text = filename + " - TotalImage";
            }
        }

        private void createAFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgNewFolder dlg = new dlgNewFolder();
            dlg.ShowDialog();
        }

        private void largeIconsToolStripMenuItem_Click(object sender, EventArgs e)
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
        }

        private void smallIconsToolStripMenuItem_Click(object sender, EventArgs e)
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
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
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
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
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
        }

        private void tilesToolStripMenuItem_Click(object sender, EventArgs e)
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
        }

        private void menuBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuBar.Visible = menuBarToolStripMenuItem.Checked;
            menuBarToolStripMenuItem1.Checked = menuBarToolStripMenuItem.Checked;
        }

        private void commandBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            commandBar.Visible = commandBarToolStripMenuItem.Checked;
            commandBarToolStripMenuItem1.Checked = commandBarToolStripMenuItem.Checked;
        }
        private void directoryTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer.Panel1Collapsed = !directoryTreeToolStripMenuItem.Checked;
            directoryTreeToolStripMenuItem1.Checked = directoryTreeToolStripMenuItem.Checked;
        }

        private void fileListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer.Panel2Collapsed = !fileListToolStripMenuItem.Checked;
            fileListToolStripMenuItem1.Checked = fileListToolStripMenuItem.Checked;
        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusBar.Visible = statusBarToolStripMenuItem.Checked;
            statusBarToolStripMenuItem1.Checked = statusBarToolStripMenuItem.Checked;
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenImage();
        }

        private void OpenImage()
        {
            if (unsavedChanges)
            {
                DialogResult = MessageBox.Show("You have unsaved changes. Would you like to save the current image first before opening another one?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (DialogResult)
                {
                    case DialogResult.Cancel: return;
                    case DialogResult.Yes: /* Save changes to the current image, then close the current image */ break;
                    case DialogResult.No: /* Close the current image */ break;
                }
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AutoUpgradeEnabled = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            ofd.Title = "Open image...";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;
            //ofd.ShowReadOnly = true; //We probably want this, but it degrades the dialog appearance to XP dialog... Needs a workaround
            ofd.Filter = "Raw sector image (*.img, *.ima, *.vfd, *.flp, *.dsk, *.xdf, *.hdm)|*.img;*.ima;*.vfd;*.flp;*.dsk;*.xdf;*.hdm|" +
                /* "WinImage compressed image (*.imz)|*.imz|" +
                 "DiskDupe image (*.ddi)|*.ddi|" +
                 "IBM SaveDiskF image (*.dsk)|*.dsk|" +
                 "TeleDisk image (*.td0)|*.td0|" +
                 "ImageDisk image (*.imd)|*.imd|" +
                 "CopyQM image (*.cqm)|*.cqm|" +
                 "EZ-DisKlone Plus image (*.fdf)|*.fdf|" +
                 "Virtual Hard Disk image (*.vhd)|*.vhd|" +
                 "Anex86 floppy disk image (*.fdi)|*.fdi|" +
                 "Anex86 hard disk image (*.hdi)|*.hdi|" +
                 "86Box surface image (*.86f)|*.86f|" +
                 "MFM surface image (*.mfm)|*.mfm|" +*/
                "All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = ofd.SafeFileName;
                path = ofd.FileName;
                Text = filename + " - TotalImage";
                lstDirectories.Nodes.Clear();
                lstFiles.Items.Clear();

                TreeNode root = new TreeNode(filename);
                root.ContextMenuStrip = cmsDirTree;
                root.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
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
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenImage();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Needs a check for unsaved changes
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgAbout dlg = new dlgAbout();
            dlg.ShowDialog();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgOptions dlg = new dlgOptions();
            dlg.ShowDialog();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgProperties dlg = new dlgProperties((DirectoryEntry)lstFiles.SelectedItems[0].Tag);
            dlg.ShowDialog();
        }

        private void changeVolumeLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgChangeVolLabel dlg = new dlgChangeVolLabel();
            dlg.ShowDialog();
        }

        private void bootSectorPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgBootSector dlg = new dlgBootSector();
            dlg.ShowDialog();
        }

        private void imageInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgImageInfo dlg = new dlgImageInfo();
            dlg.ShowDialog();
        }

        private void totalImageOnGitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var process = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = "https://github.com/TotalImage/TotalImage",
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(process);
        }

        private void menuBarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            menuBar.Visible = menuBarToolStripMenuItem1.Checked;
            menuBarToolStripMenuItem.Checked = menuBarToolStripMenuItem1.Checked;
        }

        private void commandBarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            commandBar.Visible = commandBarToolStripMenuItem1.Checked;
            commandBarToolStripMenuItem.Checked = commandBarToolStripMenuItem1.Checked;
        }

        private void directoryTreeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            splitContainer.Panel1Collapsed = !directoryTreeToolStripMenuItem1.Checked;
            directoryTreeToolStripMenuItem.Checked = directoryTreeToolStripMenuItem1.Checked;
        }

        private void fileListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            splitContainer.Panel2Collapsed = !fileListToolStripMenuItem1.Checked;
            fileListToolStripMenuItem.Checked = fileListToolStripMenuItem1.Checked;
        }

        private void statusBarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            statusBar.Visible = statusBarToolStripMenuItem1.Checked;
            statusBarToolStripMenuItem.Checked = statusBarToolStripMenuItem1.Checked;
        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InjectFiles();
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgExtract dlg = new dlgExtract();
            dlg.ShowDialog();
        }

        //Returns size of directory
        private uint CalculateDirSize(bool searchSubdirs) 
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

        //Returns the number of files in a directory
        private uint GetFileCount(bool searchSubdirs) 
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

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lstFiles.SelectedItems.Count == 0)
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
                lblStatusSize.Text = string.Format("{0:n0}", CalculateDirSize(false)).ToString() + " bytes in " + GetFileCount(false) + " file(s)";
            }
            else if(lstFiles.SelectedItems.Count == 1)
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
                lblStatusSize.Text = string.Format("{0:n0}", ((DirectoryEntry)lstFiles.SelectedItems[0].Tag).fileSize).ToString() + " bytes in 1 file";
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
                foreach(ListViewItem lvi in lstFiles.SelectedItems)
                {
                    DirectoryEntry entry = (DirectoryEntry)lvi.Tag;
                    selectedSize += entry.fileSize;
                }

                lblStatusSize.Text = string.Format("{0:n0}", selectedSize) + " bytes in " + lstFiles.SelectedItems.Count + " files";
            }
        }

        private void propertiesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dlgProperties dlg = new dlgProperties((DirectoryEntry)lstDirectories.SelectedNode.Tag);
            dlg.ShowDialog();
        }

        private void extractToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dlgExtract dlg = new dlgExtract();
            dlg.ShowDialog();
        }

        private void createAFolderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dlgNewFolder dlg = new dlgNewFolder();
            dlg.ShowDialog();
        }

        private void extractToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            dlgExtract dlg = new dlgExtract();
            dlg.ShowDialog();
        }

        private void propertiesToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            dlgProperties dlg = new dlgProperties((DirectoryEntry)lstFiles.SelectedItems[0].Tag);
            dlg.ShowDialog();
        }

        private void createAFolderToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            dlgNewFolder dlg = new dlgNewFolder();
            dlg.ShowDialog();
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
            if(e.ClickedItem == editToolStripMenuItem) 
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

        private void managePartitionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgManagePart dlg = new dlgManagePart();
            dlg.ShowDialog();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            dlgOptions dlg = new dlgOptions();
            dlg.ShowDialog();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            dlgImageInfo dlg = new dlgImageInfo();
            dlg.ShowDialog();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            dlgBootSector dlg = new dlgBootSector();
            dlg.ShowDialog();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            dlgChangeVolLabel dlg = new dlgChangeVolLabel();
            dlg.ShowDialog();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            dlgManagePart dlg = new dlgManagePart();
            dlg.ShowDialog();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            dlgProperties dlg = new dlgProperties((DirectoryEntry)lstFiles.SelectedItems[0].Tag);
            dlg.ShowDialog();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            dlgNewFolder dlg = new dlgNewFolder();
            dlg.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            dlgExtract dlg = new dlgExtract();
            dlg.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            InjectFiles();
        }

        private void InjectFiles()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AutoUpgradeEnabled = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            ofd.Title = "Select file(s) to inject...";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = true;
            ofd.Filter = "All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                /* Inject the selected file(s) into the image */
            }
        }

        private void closeImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseImage();
        }

        private void CloseImage()
        {
            /* Offer to save any unsaved changes before closing the image file */
            /* if(unsavedChanges)
             * {
             *      DialogResult == MessageBox.Show("blabla");
             *      if(DialogResult == DialogResult.Yes){
             *          //Save changes
             *      }
             * }
             *
             */
            Text = "TotalImage";

            filename = "";
            path = "";
            image.CloseImage();
            image = null;
            lstDirectories.Nodes.Clear();
            lstFiles.Items.Clear();
            DisableUI();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            CloseImage();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        //Save the changes made to the image since the last save or since it was opened
        private void SaveChanges()
        {
            byte[] imageBytes = image.GetImageBytes();
            System.IO.File.WriteAllBytes(path, imageBytes);

            saveToolStripButton.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            Text = filename + " - TotalImage";
            unsavedChanges = false;
        }

        private void injectAFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InjectFolder();
        }

        private void InjectFolder()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select a folder to inject...";
            fbd.ShowNewFolderButton = true;
           
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                /* Inject the entire selected folder into the image */
                unsavedChanges = true;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            sorter = new ListViewColumnSorter();
            lstFiles.ListViewItemSorter = sorter;

            //Because designer doesn't have the Enter key in the list for some reason...
            propertiesToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.Enter; 
            propertiesToolStripMenuItem1.ShortcutKeys = Keys.Alt | Keys.Enter;
            propertiesToolStripMenuItem2.ShortcutKeys = Keys.Alt | Keys.Enter;

            DisableUI(); //Once support for command line arguments is added, those will need to be checked before this is done...
            GetFolderIcon();
            lstDirectories.SelectedImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
        }

        //Gets the default Windows folder icon with SHGetFileInfo
        public void GetFolderIcon()
        {
            Icon icon = GetShellFileIcon("C:\\Windows", FileAttributes.Directory);
            imgFilesSmall.Images.Add("folder", icon);
        }

        //Adds a new node to the root directory in the directory tree
        public void AddToRootDir(DirectoryEntry entry)
        {
            string filename = entry.name.TrimEnd('.');
            TreeNode node = new TreeNode(filename);
            //node.ContextMenuStrip = cmsDirTree;
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
            //childNode.ContextMenuStrip = cmsDirTree;
            childNode.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
            childNode.Tag = child;
            TreeNode parentNode = FindNode(lstDirectories.Nodes[0], ((uint)(parent.fstClusHI << 16) + parent.fstClusLO));
            if(parentNode != null)
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
        public Icon GetShellFileIcon(string filename, FileAttributes attributes)
        {
            var shinfo = new SHFILEINFO();
            var flags = SHGFI.ICON | SHGFI.SMALLICON | SHGFI.USEFILEATTRIBUTES;

            SHGetFileInfo(filename, attributes, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);
            Icon icon = (Icon)Icon.FromHandle(shinfo.hIcon).Clone();
            DestroyIcon(shinfo.hIcon);
            return icon;
        }


        //Adds a new item to the file list
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
                        Icon icon = GetShellFileIcon(filename, FileAttributes.Normal);
                        imgFilesSmall.Images.Add(filetype, icon);
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

        //Calculates the image capacity
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
            selectPartitionToolStripButton.Enabled = false;
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
            selectPartitionToolStripButton.Enabled = false;

            managePartitionsToolStripMenuItem.Enabled = false;
            selectPartitionToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            closeImageToolStripMenuItem.Enabled = false;

            foreach(ToolStripItem item in editToolStripMenuItem.DropDownItems)
            {
                if (item.CanSelect)
                {
                    item.Enabled = false;
                }
            }
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

        private void lstDirectories_AfterSelect(object sender, TreeViewEventArgs e)
        {
            uint fileCount = 0;
            uint dirSize = 0;
            lstFiles.BeginUpdate();
            //Subdirs
            if (e.Node.Text != filename)
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
                            /* Throw an error because the node was not found for some reason... */
                        }
                    }
                    else //A file was double-clicked
                    {
                        /* Probably extract the file and open it */
                    }
                }
            }
        }

        private void nameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            nameToolStripMenuItem.Checked = true;
            nameToolStripMenuItem1.Checked = true;
            typeToolStripMenuItem.Checked = false;
            typeToolStripMenuItem1.Checked = false;
            sizeToolStripMenuItem.Checked = false;
            sizeToolStripMenuItem1.Checked = false;
            modifiedToolStripMenuItem.Checked = false;
            modifiedToolStripMenuItem1.Checked = false;
        }

        private void sizeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            nameToolStripMenuItem.Checked = false;
            nameToolStripMenuItem1.Checked = false;
            typeToolStripMenuItem.Checked = false;
            typeToolStripMenuItem1.Checked = false;
            sizeToolStripMenuItem.Checked = true;
            sizeToolStripMenuItem1.Checked = true;
            modifiedToolStripMenuItem.Checked = false;
            modifiedToolStripMenuItem1.Checked = false;
        }

        private void typeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            nameToolStripMenuItem.Checked = false;
            nameToolStripMenuItem1.Checked = false;
            typeToolStripMenuItem.Checked = true;
            typeToolStripMenuItem1.Checked = true;
            sizeToolStripMenuItem.Checked = false;
            sizeToolStripMenuItem1.Checked = false;
            modifiedToolStripMenuItem.Checked = false;
            modifiedToolStripMenuItem1.Checked = false;
        }

        private void modifiedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nameToolStripMenuItem.Checked = false;
            nameToolStripMenuItem1.Checked = false;
            typeToolStripMenuItem.Checked = false;
            typeToolStripMenuItem1.Checked = false;
            sizeToolStripMenuItem.Checked = false;
            sizeToolStripMenuItem1.Checked = false;
            modifiedToolStripMenuItem.Checked = true;
            modifiedToolStripMenuItem1.Checked = true;
        }

        private void nameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nameToolStripMenuItem.Checked = true;
            nameToolStripMenuItem1.Checked = true;
            typeToolStripMenuItem.Checked = false;
            typeToolStripMenuItem1.Checked = false;
            sizeToolStripMenuItem.Checked = false;
            sizeToolStripMenuItem1.Checked = false;
            modifiedToolStripMenuItem.Checked = false;
            modifiedToolStripMenuItem1.Checked = false;
        }

        private void sizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nameToolStripMenuItem.Checked = false;
            nameToolStripMenuItem1.Checked = false;
            typeToolStripMenuItem.Checked = false;
            typeToolStripMenuItem1.Checked = false;
            sizeToolStripMenuItem.Checked = true;
            sizeToolStripMenuItem1.Checked = true;
            modifiedToolStripMenuItem.Checked = false;
            modifiedToolStripMenuItem1.Checked = false;
        }

        private void typeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nameToolStripMenuItem.Checked = false;
            nameToolStripMenuItem1.Checked = false;
            typeToolStripMenuItem.Checked = true;
            typeToolStripMenuItem1.Checked = true;
            sizeToolStripMenuItem.Checked = false;
            sizeToolStripMenuItem1.Checked = false;
            modifiedToolStripMenuItem.Checked = false;
            modifiedToolStripMenuItem1.Checked = false;
        }

        private void modifiedDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nameToolStripMenuItem.Checked = false;
            nameToolStripMenuItem1.Checked = false;
            typeToolStripMenuItem.Checked = false;
            typeToolStripMenuItem1.Checked = false;
            sizeToolStripMenuItem.Checked = false;
            sizeToolStripMenuItem1.Checked = false;
            modifiedToolStripMenuItem.Checked = true;
            modifiedToolStripMenuItem1.Checked = true;
        }

        private void largeIconsToolStripMenuItem1_Click(object sender, EventArgs e)
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
        }

        private void smallIconsToolStripMenuItem1_Click(object sender, EventArgs e)
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
        }

        private void listToolStripMenuItem1_Click(object sender, EventArgs e)
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
        }

        private void detailsToolStripMenuItem1_Click(object sender, EventArgs e)
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
        }

        private void tilesToolStripMenuItem1_Click(object sender, EventArgs e)
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
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstFiles.Focus();
            foreach(ListViewItem lvi in lstFiles.Items)
            {
                lvi.Selected = true;
            }
        }

        private void defragmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgDefragment dlg = new dlgDefragment();
            dlg.ShowDialog();
        }

        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
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
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        //Opens an image that's been dragged and dropped onto the file list
        //Needs improvement, but the gist of it is there...
        private void lstFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] items = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if(filename == "" && unsavedChanges == false) //No image is loaded
            {
                if(items.Length == 1)
                {
                    filename = Path.GetFileName(items[0]);
                    path = items[0];
                    Text = filename + " - TotalImage";
                    lstDirectories.Nodes.Clear();
                    lstFiles.Items.Clear();

                    TreeNode root = new TreeNode(filename);
                    root.ContextMenuStrip = cmsDirTree;
                    root.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
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
                }
            }
            else if(filename != "" || unsavedChanges) //An image is open (either saved or new)
            {
                /* Inject files/folder instead */
            }
        }

        private void lstDirectories_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        //Opens an image that's been dragged and dropped onto the dir tree
        //Needs improvement, but the gist of it is there...
        private void lstDirectories_DragDrop(object sender, DragEventArgs e)
        {
            string[] items = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (filename == "" && unsavedChanges == false) //No image is loaded
            {
                if (items.Length == 1)
                {
                    filename = Path.GetFileName(items[0]);
                    path = items[0];
                    Text = filename + " - TotalImage";
                    lstDirectories.Nodes.Clear();
                    lstFiles.Items.Clear();

                    TreeNode root = new TreeNode(filename);
                    root.ContextMenuStrip = cmsDirTree;
                    root.ImageIndex = imgFilesSmall.Images.IndexOfKey("folder");
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
                }
            }
        }

        private void expandDirectoryTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstDirectories.ExpandAll();
        }

        private void collapseDirectoryTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstDirectories.CollapseAll();
        }

        private void expandDirectoryTreeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            lstDirectories.ExpandAll();
        }

        private void collapseDirectoryTreeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            lstDirectories.CollapseAll();
        }

        private void cmsDirTree_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lstDirectories.SelectedNode == null)
            {
                extractToolStripMenuItem1.Enabled = false;
                createAFolderToolStripMenuItem1.Enabled = false;
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
                    createAFolderToolStripMenuItem1.Enabled = true;
                    extractToolStripMenuItem1.Enabled = true;
                }
                else
                {
                    extractToolStripMenuItem1.Enabled = true;
                    createAFolderToolStripMenuItem1.Enabled = true;
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

                if (lstDirectories.SelectedNode != null)
                {
                    System.Diagnostics.Debug.WriteLine(lstDirectories.SelectedNode.Text);

                }
                cmsDirTree.Show(lstDirectories, e.Location);
            }
        }
    }
}