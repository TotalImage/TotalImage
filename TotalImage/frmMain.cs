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
        private TiDirectory lastViewedDir;
        internal static Dictionary<string, (string name, int iconIndex)> fileTypes = new Dictionary<string, (string name, int iconIndex)>(StringComparer.InvariantCultureIgnoreCase);
        private string? lastSavedFilename;
        private TiDirectory draggedDir;

        private ListViewItem upOneFolderListViewItem = new ListViewItem()
        {
            Text = ".."
        };

        private List<ListViewItem> currentFolderView = new List<ListViewItem>();

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
            //These three are outside Sync because otherwise they'd be applied whenever settings are saved...
            WindowState = Settings.CurrentSettings.WindowState;
            Location = Settings.CurrentSettings.WindowPosition;
            Size = Settings.CurrentSettings.WindowSize;
            SyncUIWithSettings();

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
            using FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                /* Inject the entire selected folder into the image */
                throw new NotImplementedException("This feature is not implemented yet");
            }
        }

        //Shows a hex view of the current image
        private void hexView_Click(object sender, EventArgs e)
        {
            using dlgHexView frm = new dlgHexView();
            frm.ShowDialog();
        }

        //Allows viewing and editing both volume labels
        //TODO: Actually change the volume labels
        private void changeVolumeLabel_Click(object sender, EventArgs e)
        {
            if (!(image?.PartitionTable.Partitions[0].FileSystem is FatFileSystem fs))
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

            using dlgChangeVolLabel dlg = new dlgChangeVolLabel(fs.RootDirectoryVolumeLabel, fs.BpbVolumeLabel);
            dlg.ShowDialog();
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
            if (!File.Exists(imagePath))
            {
                TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = $"File \"{Path.GetFileName(imagePath)}\" could not be opened because it no longer exists and will be removed from your recent images list.{Environment.NewLine}{Environment.NewLine}" +
                    $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                    Heading = "File not found",
                    Caption = "Error",
                    Buttons =
                        {
                            TaskDialogButton.OK
                        },
                    Icon = TaskDialogIcon.Error,
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
            using dlgNewImage dlg = new dlgNewImage();
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

                    if (result.Tag != null) /* Save changes first... */ ;
                    else if (result == TaskDialogButton.Cancel) return;
                }

                if (image != null)
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
            if (string.IsNullOrEmpty(filename) || (ToolStripMenuItem)sender == saveAsToolStripMenuItem) //File hasn't been saved yet
            {
                saveFileAs();
            }
            else
            {
                saveFile();
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
            using dlgDefragment dlg = new dlgDefragment();
            dlg.ShowDialog();
        }

        //Formats the selected partition/floppy disk
        //TODO: Implement this here and in FS/container.
        private void format_Click(object sender, EventArgs e)
        {
            using dlgFormat dlg = new dlgFormat();
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
            if (image == null)
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

                if (result.Tag != null)
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

                if (result.Tag != null) save_Click(result, e);
                else if (result == TaskDialogButton.Cancel) return;
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
                "Raw sector image (*.img, *.iso, *.ima, *.vfd, *.flp, *.dsk, *.xdf, *.hdm)|*.img;*.iso;*.ima;*.vfd;*.flp;*.dsk;*.xdf;*.hdm|" +
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

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e) // This method will be used more than once, thus it is separated from the main event.
        {
            if (image != null)
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
                    if (path.Substring(path.Length - lstDirectories.PathSeparator.Length) != lstDirectories.PathSeparator)
                        path += lstDirectories.PathSeparator;

                    UpdateStatusBar(false);
                }
            }
        }

        private void cmsFileList_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
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
            if (lstDirectories.Focused)
            {
                using dlgProperties dlg = new dlgProperties((TiFileSystemObject)lstDirectories.SelectedNode.Tag);
                dlg.ShowDialog();
            }
            else if (lstFiles.Focused)
            {
                if (lstFiles.SelectedIndices.Count == 1)
                {
                    using dlgProperties dlg = new dlgProperties(GetSelectedItemData(0));
                    dlg.ShowDialog();
                }
                else if (lstFiles.SelectedIndices.Count > 1)
                {
                    throw new NotImplementedException("This feature is not implemented yet.");
                }
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
                TaskDialogButton result = TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = $"Would you like to save them before closing the current image?",
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

                if (result.Tag != null) /* Save changes... */ ;
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

        private void lstDirectories_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            //This prevents the user from opening a deleted directory (since we don't even know yet if it's recoverable, or what was inside, etc.)
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

            if (lstDirectories.SelectedNode == null)
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
                    string targetFile = Path.Combine(Settings.CurrentSettings.DefaultExtractPath, SelectedItems.First().Name);
                    Debug.WriteLine(targetFile);
                    if (Settings.CurrentSettings.ExtractAlwaysAsk)
                    {
                        using dlgExtract dlg = new dlgExtract();
                        dlg.lblPath.Text = $"Extract {SelectedItems.Count()} selected {(SelectedItems.Count() > 1 ? "items" : "item")} to the following folder:";
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            targetFile = Path.Combine(dlg.TargetPath, SelectedItems.First().Name);
                            ExtractFiles(SelectedItems, dlg.TargetPath, dlg.ExtractType, dlg.OpenFolder);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        ExtractFiles(SelectedItems, Settings.CurrentSettings.DefaultExtractPath, Settings.CurrentSettings.DefaultExtractType, Settings.CurrentSettings.OpenFolderAfterExtract);
                    }

                    //Tell the shell to open the file after extraction
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = targetFile,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
            }
        }

        private void cmsDirTree_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lstDirectories.Nodes.Count == 0 || lstDirectories.SelectedNode == null)
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
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && image == null)
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /* Drag and drop was performed on the ListView - a file was dragged into the ListView/TreeView from Explorer => try to open it
         * Right now, we only handle case a for opening a single file that was dragged into the window.
         * TODO: Implement other drag and drop scenarios (moving files within the image, etc.). */
        private void list_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && image == null)
            {
                //Files are being dragged into the listview from outside the form
                string[] items = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (items.Length == 1)
                {
                    CloseImage();
                    filepath = items[0];
                    OpenImage(filepath);
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
                if (newNode != null)
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
         * the drop is performed.
         * TODO: Build an array of selected ListViewItems and path strings to perform the drag and drop with */
        private void lstFiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string tempdir = Path.Combine(Path.GetTempPath(), "TotalImage", filename);
                if (!Directory.Exists(tempdir))
                {
                    Directory.CreateDirectory(tempdir);
                }

                if (((ListViewItem)e.Item).Text == "..")
                {
                    return;
                }

                List<string> items = new List<string>();
                foreach (TiFileSystemObject fso in SelectedItems)
                {
                    string item = Path.Combine(tempdir, fso.Name);
                    items.Add(item);
                }
                string[] draggedItems = items.ToArray();

                DataObject data = new DataObject();
                data.SetData(DataFormats.FileDrop, draggedItems); //Needed for Explorer
                lstFiles.DoDragDrop(data, DragDropEffects.Move);
            }
        }

        /* Fires when the user starts dragging a TreeNode around. String array is needed for Explorer to perform the move operation once
         * the drop is performed.
         * TODO: Build an array of selected TreeNodes and path strings to perform the drag and drop with */
        private void lstDirectories_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string tempdir = Path.Combine(Path.GetTempPath(), "TotalImage", filename);
                if (!Directory.Exists(tempdir))
                {
                    Directory.CreateDirectory(tempdir);
                }

                //This array is needed for Explorer to perform the file copy/move operation later on.
                List<string> items = new List<string>();
                draggedDir = (TiDirectory)((TreeNode)e.Item).Tag;
                if (draggedDir.Parent == null)
                {
                    /* Add the root dir contents (non-recursively) to the list instead of the tempdir itself, so Explorer doesn't end up moving it
                     * instead of the contents. */
                    foreach (var fso in draggedDir.EnumerateFileSystemObjects(Settings.CurrentSettings.ShowHiddenItems, false))
                    {
                        items.Add(Path.Combine(tempdir, fso.Name));
                    }
                }
                else
                {
                    items.Add(Path.Combine(tempdir, draggedDir.Name));
                }
                string[] itemsArray = items.ToArray();

                DataObject data = new DataObject();
                data.SetData(DataFormats.FileDrop, itemsArray); //FileDrop is needed for Explorer
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

                if (result.Tag != null) /* Save changes... */ ;
                else if (result == TaskDialogButton.Cancel)
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

            expandDirectoryTreeToolStripMenuItem.Enabled = image != null && lstDirectories.Nodes[0].Nodes.Count > 0;
            collapseDirectoryTreeToolStripMenuItem.Enabled = image != null && lstDirectories.Nodes[0].Nodes.Count > 0;
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
            if (WindowState == FormWindowState.Normal)
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
            using dlgNotifications dlg = new dlgNotifications();
            dlg.ShowDialog();
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            closeImageToolStripMenuItem.Enabled = image != null;
            saveToolStripMenuItem.Enabled = image != null && unsavedChanges;
            saveAsToolStripMenuItem.Enabled = image != null;
        }

        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (image == null)
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
            imageInformationToolStripMenuItem.Enabled = image != null;
            hexViewToolStripMenuItem.Enabled = image != null;
        }

        private void lstFiles_Enter(object sender, EventArgs e)
        {
            if (image != null)
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
            if (image != null)
            {
                if (lstDirectories.SelectedNode == null)
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
                Directory.Delete(Path.Combine(Path.GetTempPath(), "TotalImage", filename), true);
                return;
            }

            /* If the left mouse button was released, complete the drag and drop operation. In our case, this means that any files or folders 
             * that may have been dragged out of the window have to be extracted into a temporary folder first, then the drop is performed so 
             * Explorer will move these items to the actual drag and drop destination. Dropping inside the window is not supported yet. */
            if ((e.KeyState & 1) == 0)
            {
                if (!ClientRectangle.Contains(PointToClient(MousePosition)))
                {
                    //Here is where the actual extraction to the temp dir happens
                    if (sender is ListView)
                    {
                        ExtractFiles(SelectedItems, Path.Combine(Path.GetTempPath(), "TotalImage", filename), Settings.FolderExtract.Preserve, false);
                    }
                    else if (sender is TreeView)
                    {
                        if (draggedDir.Parent == null) //Root dir needs to be treated separately
                        {
                            ExtractFiles(draggedDir.EnumerateFileSystemObjects(Settings.CurrentSettings.ShowHiddenItems, false), Path.Combine(Path.GetTempPath(), "TotalImage", filename), Settings.FolderExtract.Preserve, false);
                        }
                        else
                        {
                            List<TiFileSystemObject> dir = new List<TiFileSystemObject>();
                            dir.Add(draggedDir);
                            ExtractFiles(dir, Path.Combine(Path.GetTempPath(), "TotalImage", filename), Settings.FolderExtract.Preserve, false);
                        }
                    }

                    e.Action = DragAction.Drop;
                    return;
                }
                else
                {
                    e.Action = DragAction.Cancel;
                    Directory.Delete(Path.Combine(Path.GetTempPath(), "TotalImage", filename), true);
                    return;
                }
            }
            //Left mouse button wasn't released yes, continue the drag
            else
            {
                e.Action = DragAction.Continue;
                return;
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

        // Used for events that require the current folder view to be updated (e.g. show hidden/deleted items toggled, etc.)
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

                item.ToolTipText = $"Type: {item.SubItems[1].Text}{Environment.NewLine}Size: {item.SubItems[2].Text}{Environment.NewLine}Modified: {item.SubItems[3].Text}";

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
                FileInfo fileinfo = new FileInfo(path);

                try
                {
                    bool memoryMapping = fileinfo.Length > Settings.CurrentSettings.MemoryMappingThreshold;

                    var ext = Path.GetExtension(filename).ToLowerInvariant();
                    switch (ext)
                    {
                        case ".vhd":
                            image = new VhdContainer(path, memoryMapping);
                            break;
                        default:
                            image = new RawContainer(path, memoryMapping);
                            break;
                    }
                }
                catch (FileNotFoundException)
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"File \"{filename}\" could not be opened because it no longer exists.{Environment.NewLine}{Environment.NewLine}" +
                    $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                        Heading = "File not found",
                        Caption = "Error",
                        Buttons =
                        {
                            TaskDialogButton.OK
                        },
                        Icon = TaskDialogIcon.Error,
                    });
                    CloseImage();
                    return;
                }
                catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"File \"{filename}\" could not be opened because it's in use by another process. Close all processes using this file and try again.{Environment.NewLine}{Environment.NewLine}" +
                    $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                        Heading = "File in use by another process",
                        Caption = "Error",
                        Buttons =
                        {
                            TaskDialogButton.OK
                        },
                        Icon = TaskDialogIcon.Error,
                    });
                    CloseImage();
                    return;
                }

                if (fileinfo.Length == 0)
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"File \"{filename}\" appears to be empty (zero bytes in size). If you downloaded or copied this file from elsewhere, make sure the source file is not damaged and the transfer completed successfully.{Environment.NewLine}{Environment.NewLine}" +
                        $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                        Heading = "File is empty",
                        Caption = "Error",
                        Buttons =
                        {
                            TaskDialogButton.OK
                        },
                        Icon = TaskDialogIcon.Error,
                    });

                    CloseImage();
                    return;
                }
            }

            CurrentPartitionIndex = 0;
            if (image.PartitionTable.Partitions.Count == 0)
            {
                TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = $"We couldn't find any partitions in your image. If this is a hard disk image, verify that it's partitioned with either MBR or GPT partitioning scheme and contains at least one partition.{Environment.NewLine}{Environment.NewLine}" +
                    $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                    Heading = "No partitions detected",
                    Caption = "Error",
                    Buttons =
                        {
                            TaskDialogButton.OK
                        },
                    Icon = TaskDialogIcon.Error,
                });

                CloseImage();
                return;
            }
            else if (image.PartitionTable.Partitions.Count >= 1)
            {
                if (image.PartitionTable.Partitions.Count > 1)
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
                }

                selectPartitionToolStripComboBox.Items.Clear();

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

            LoadPartitionInCurrentImage(CurrentPartitionIndex);

            if (filename != "")
            {
                Text = $"{filename} - TotalImage";
            }
            else
            {
                Text = "(Untitled) - TotalImage";
            }
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

            foreach (var (key, attributes) in types)
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
            lblStatusProgressBar.Visible = true;

            // Change border sides for status bar children to add seperator-like looks.
            lblStatusCapacity.BorderSides = ToolStripStatusLabelBorderSides.Right;
            lblStatusSize.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right;

            if (unsavedChanges)
                saveToolStripButton.Enabled = true;

            //Enabling this now since we have rudimentary HDD support.
            managePartitionsToolStripButton.Enabled = image.PartitionTable is not Partitions.NoPartitionTable;
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

        private void ExtractFiles(IEnumerable<TiFileSystemObject> items, string path, Settings.FolderExtract extractType, bool openFolder)
        {
            var files = from x in items where x is TiFile select x as TiFile;

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            foreach (var file in files)
            {
                if (File.Exists(Path.Combine(path, file.Name)) && Settings.CurrentSettings.ConfirmOverwriteExtraction)
                {
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
                }

                using (var destStream = new FileStream(Path.Combine(path, file.Name), FileMode.Create))
                using (var fileStream = file.GetStream())
                {
                    fileStream.Position = 0; // reset position to zero because CopyTo will only go from current position
                    file.GetStream().CopyTo(destStream);
                }

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
                        dir.EnumerateFileSystemObjects(Settings.CurrentSettings.ShowHiddenItems, false),
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

        private void UpdateStatusBar(bool updateFreeSpace)
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
                            lblStatusSize.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize(item.Length)} in 1 item";
                            break;
                        }
                    case StatusBarState.MultipleSelected:
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
    }
}