using System;
using System.IO;
using System.Windows.Forms;
using TotalImage.ImageFormats;

namespace TotalImage
{
    public partial class frmMain : Form
    {
        public static bool surfaceImage;
        public string filename = "";
        public IMG imgMan;

        public frmMain()
        {
            InitializeComponent();
            imgMan = new IMG();
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
            dlgNewImage dlg = new dlgNewImage();
            dlg.ShowDialog();
        }

        private void saveAsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveImage();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            /*if(unsaved changes in existing file)
              {
                 SaveChangesToExistingFile();
              }
              else //Unsaved changes in new file
            */
            SaveImage();
        }

        private void SaveImage()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AutoUpgradeEnabled = true;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            sfd.OverwritePrompt = true;
            sfd.Title = "Save image...";
            sfd.DefaultExt = "img";
            sfd.Filter = "Raw sector image (*.img, *.ima, *.vfd, *.flp, *.dsk, *.hdm)|*.img;*.ima;*.vfd;*.flp;*.dsk;*.hdm|" +
                "WinImage compressed image (*.imz)|*.imz|" +
                "DiskDupe image (*.ddi)|*.ddi|" +
                "Anex86 floppy disk image (*.fdi)|*.fdi|" +
                "86Box surface image (*.86f)|*.86f|" +
                "All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (sfd.FilterIndex == 0 || sfd.FileName.EndsWith(".img") || sfd.FileName.EndsWith(".ima") ||
                    sfd.FileName.EndsWith(".vfd") || sfd.FileName.EndsWith(".flp") || sfd.FileName.EndsWith(".dsk") ||
                    sfd.FileName.EndsWith(".hdm"))
                {
                    byte[] imageBytes = imgMan.GetImageBytes();
                    File.WriteAllBytes(sfd.FileName, imageBytes);
                }
                else if (sfd.FilterIndex == 1 || sfd.FileName.EndsWith(".imz"))
                {
                    /* Save to IMZ */
                }
                else if (sfd.FilterIndex == 2 || sfd.FileName.EndsWith(".ddi"))
                {
                    /* Save to DDI */
                }
                else if (sfd.FilterIndex == 3 || sfd.FileName.EndsWith(".fdi"))
                {
                    /* Save to Anex86 FDI */
                }
                else if (sfd.FilterIndex == 4 || sfd.FileName.EndsWith(".86f"))
                {
                    /* Save to 86F */
                }
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
            smallIconsToolStripMenuItem.Checked = false;
            detailsToolStripMenuItem.Checked = false;
            tilesToolStripMenuItem.Checked = false;
            listToolStripMenuItem.Checked = false;
            lstFiles.View = View.LargeIcon;
        }

        private void smallIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            largeIconsToolStripMenuItem.Checked = false;
            smallIconsToolStripMenuItem.Checked = true;
            detailsToolStripMenuItem.Checked = false;
            tilesToolStripMenuItem.Checked = false;
            listToolStripMenuItem.Checked = false;
            lstFiles.View = View.SmallIcon;
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            largeIconsToolStripMenuItem.Checked = false;
            smallIconsToolStripMenuItem.Checked = false;
            detailsToolStripMenuItem.Checked = false;
            tilesToolStripMenuItem.Checked = false;
            listToolStripMenuItem.Checked = true;
            lstFiles.View = View.List;
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            largeIconsToolStripMenuItem.Checked = false;
            smallIconsToolStripMenuItem.Checked = false;
            detailsToolStripMenuItem.Checked = true;
            tilesToolStripMenuItem.Checked = false;
            listToolStripMenuItem.Checked = false;
            lstFiles.View = View.Details;
        }

        private void tilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            largeIconsToolStripMenuItem.Checked = false;
            smallIconsToolStripMenuItem.Checked = false;
            detailsToolStripMenuItem.Checked = false;
            tilesToolStripMenuItem.Checked = true;
            listToolStripMenuItem.Checked = false;
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
            //Needs a check for unsaved changes
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AutoUpgradeEnabled = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            ofd.Title = "Open image...";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;
            //ofd.ShowReadOnly = true; //We probably want this, but it degrades the dialog appearance to XP dialog... Needs a workaround
            ofd.Filter = "Raw sector image (*.img, *.ima, *.vfd, *.flp, *.dsk, *.xdf)|*.img;*.ima;*.vfd;*.flp;*.dsk;*.xdf|" +
                "WinImage compressed image (*.imz)|*.imz|" +
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
                "MFM surface image (*.mfm)|*.mfm|" +
                "All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                /* Do stuff */
                filename = ofd.SafeFileName;
                Text = filename + " - TotalImage";
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
            dlgProperties dlg = new dlgProperties();
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
            System.Diagnostics.Process.Start("https://github.com/TotalImage/TotalImage");
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

        private void undeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgUndelete dlg = new dlgUndelete();
            dlg.ShowDialog();
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lstFiles.SelectedItems.Count == 0)
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

        private void propertiesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dlgProperties dlg = new dlgProperties();
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
            dlgProperties dlg = new dlgProperties();
            dlg.ShowDialog();
        }

        private void createAFolderToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            dlgNewFolder dlg = new dlgNewFolder();
            dlg.ShowDialog();
        }

        private void cmsFileList_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lstFiles.SelectedItems.Count == 0)
            {
                e.Cancel = true;
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
            dlgProperties dlg = new dlgProperties();
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
             * CloseImage();
             */
            Text = "TotalImage";
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            CloseImage();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void SaveChanges()
        {
            /* Save the changes made to the image since the last save (or since it was opened) */
            Text = filename + " - TotalImage";
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
            }
        }
    }
}