using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace TotalImage
{
    public partial class dlgSettings : Form
    {
        public dlgSettings()
        {
            InitializeComponent();
        }

        private void btnClearRecent_Click(object sender, System.EventArgs e)
        {
            Settings.ClearRecentImages();

            TaskDialog.ShowDialog(this, new TaskDialogPage()
            {
                Text = "List of recently opened images has been successfully cleared.",
                Heading = "Recent images cleared",
                Caption = "Success",
                Buttons =
                {
                    TaskDialogButton.OK
                },
                Icon = TaskDialogIcon.Information,
                DefaultButton = TaskDialogButton.OK
            });
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            TaskDialogButton result = TaskDialog.ShowDialog(this, new TaskDialogPage()
            {
                Text = "All settings will be reset to their default values.",
                Heading = "Are you sure you want to continue?",
                Caption = "Warning",
                Buttons =
                {
                    TaskDialogButton.Yes,
                    TaskDialogButton.No
                },
                Icon = TaskDialogIcon.Warning,
                DefaultButton = TaskDialogButton.No
            });

            if (result == TaskDialogButton.Yes)
            {
                Settings.LoadDefaults();
                Settings.Save();

                TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = "All settings were successfully reset to their default values.",
                    Heading = "Settings reset",
                    Caption = "Success",
                    Buttons =
                {
                    TaskDialogButton.OK
                },
                    Icon = TaskDialogIcon.Information,
                    DefaultButton = TaskDialogButton.OK
                });
            }

            SyncUIWithSettings();
        }

        private void dlgSettings_Load(object sender, System.EventArgs e)
        {
            SyncUIWithSettings();
        }

        //Syncs the dialog UI with CurrentSettings
        private void SyncUIWithSettings()
        {
            txtExtractPath.Text = Settings.CurrentSettings.DefaultExtractPath;
            cbxOpenDir.Checked = Settings.CurrentSettings.OpenFolderAfterExtract;
            cbxShowCommandBar.Checked = Settings.CurrentSettings.ShowCommandBar;
            cbxShowStatusBar.Checked = Settings.CurrentSettings.ShowStatusBar;
            cbxShowDirectoryTree.Checked = Settings.CurrentSettings.ShowDirectoryTree;
            cbxShowHiddenItems.Checked = Settings.CurrentSettings.ShowHiddenItems;
            cbxShowDeletedItems.Checked = Settings.CurrentSettings.ShowDeletedItems;
            lstSortBy.SelectedIndex = Settings.CurrentSettings.FilesSortingColumn;
            lstSortOrder.SelectedIndex = (int)Settings.CurrentSettings.FilesSortOrder - 1;
            cbxExtractAsk.Checked = Settings.CurrentSettings.ExtractAlwaysAsk;
            cbxPreserveAttributes.Checked = Settings.CurrentSettings.ExtractPreserveAttributes;
            cbxPreserveDates.Checked = Settings.CurrentSettings.ExtractPreserveDates;
            cbxShellFileIcons.Checked = Settings.CurrentSettings.QueryShellForFileTypeInfo;
            cbxAutoincrementFilename.Checked = Settings.CurrentSettings.AutoIncrementFilename;
            cbxConfirmDeletion.Checked = Settings.CurrentSettings.ConfirmDeletion;
            cbxConfirmInjection.Checked = Settings.CurrentSettings.ConfirmInjection;
            cbxConfirmOverwriteExtract.Checked = Settings.CurrentSettings.ConfirmOverwriteExtraction;
            txtMemoryMapping.Value = Settings.CurrentSettings.MemoryMappingThreshold / 1048576;

            switch (Settings.CurrentSettings.FilesView)
            {
                case View.LargeIcon: lstViewType.SelectedIndex = 0; break;
                case View.SmallIcon: lstViewType.SelectedIndex = 1; break;
                case View.List: lstViewType.SelectedIndex = 2; break;
                case View.Details: lstViewType.SelectedIndex = 3; break;
                case View.Tile: lstViewType.SelectedIndex = 4; break;
            }

            switch (Settings.CurrentSettings.SizeUnit)
            {
                case SizeUnit.Bytes: lstSizeUnits.SelectedIndex = 0; break;
                case SizeUnit.Decimal: lstSizeUnits.SelectedIndex = 1; break;
                case SizeUnit.Binary: lstSizeUnits.SelectedIndex = 2; break;
            }

            switch (Settings.CurrentSettings.DefaultExtractType)
            {
                case Settings.FolderExtract.Ignore: rbnIgnoreFolders.Checked = true; break;
                case Settings.FolderExtract.Merge: rbnExtractFlat.Checked = true; break;
                case Settings.FolderExtract.Preserve: rbnExtractPreserve.Checked = true; break;
            }

            //CheckFileAssociations();
        }

        //TODO: This method should check the registry for current file associations and mark relevant listviewitems in the listview as checked
        private void CheckFileAssociations()
        {
            throw new NotImplementedException();
        }


        /* TODO: This is the gist of it. It bypasses Microsoft's silly attempt at preventing programs from claiming extensions programatically
         * like they always used to - no prompt for the user will show up after opening the file after association changed in this case. 
         * Currently only .img is associated for the raw images type, other related extensions need to be associated too. */
        private void SetFileAssociations()
        {
            //First create all the ProgIDs if neccessary
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Classes\TotalImage.Raw");
            if (key != null)
            {
                key.SetValue("", "Raw sector image");
                key = key.CreateSubKey(@"shell\open\command");
                if (key != null)
                {
                    key.SetValue("", $"\"{Path.Combine(Application.StartupPath, "TotalImage.exe")}\" \"%1\"");
                }
            }

            key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Classes\TotalImage.Iso");
            if (key != null)
            {
                key.SetValue("", "ISO image");
                key = key.CreateSubKey(@"shell\open\command");
                if (key != null)
                {
                    key.SetValue("", $"\"{Path.Combine(Application.StartupPath, "TotalImage.exe")}\" \"%1\"");
                }
            }

            /* Then associate selected file extensions:
             * 1. Associate the extension with the relevant ProgID
             * 2. Add TotalImage to the MRUList for the extension
             * 3. Add our ProgID to the ProgID list for the extension
             * 4. Delete the UserChoice key (if it exists) for the extension
             * 5. All your extension are belong to us! :-) */
            if (lstFileTypes.CheckedItems.ContainsKey("TotalImage.Raw"))
            {
                key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Classes\.img");
                if (key != null)
                {
                    key.SetValue("", "TotalImage.Raw");
                }

                key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.img");
                if (key != null)
                {
                    //Lazy approach: just default to the last possible letter as the value name (which is j), rather than bother finding the next free letter...
                    key = key.CreateSubKey("OpenWithList");
                    key.SetValue("j", "TotalImage.exe", RegistryValueKind.String);
                    string mrulist = key.GetValue("MRUList").ToString();
                    mrulist = mrulist.Insert(0, "j");
                    key.SetValue("MRUList", mrulist, RegistryValueKind.String);
                }

                key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.img");
                if (key != null)
                {
                    key = key.CreateSubKey("OpenWithProgids");
                    key.SetValue("TotalImage.Raw", new byte[0], RegistryValueKind.None);
                }

                key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.img", true);
                if (key != null)
                {
                    key.DeleteSubKey("UserChoice", false);
                }
            }

            if (lstFileTypes.CheckedItems.ContainsKey("TotalImage.Iso"))
            {
                key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Classes\.iso");
                if (key != null)
                {
                    key.SetValue("", "TotalImage.Iso");
                }

                key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.iso");
                if (key != null)
                {
                    key = key.CreateSubKey("OpenWithList");
                    key.SetValue("j", "TotalImage.exe", RegistryValueKind.String);
                    string mrulist = key.GetValue("MRUList").ToString();
                    mrulist = mrulist.Insert(0, "j");
                    key.SetValue("MRUList", mrulist, RegistryValueKind.String);
                }

                key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.iso");
                if (key != null)
                {
                    key = key.CreateSubKey("OpenWithProgids");
                    key.SetValue("TotalImage.Iso", new byte[0], RegistryValueKind.None);
                }

                key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.iso", true);
                if (key != null)
                {
                    key.DeleteSubKey("UserChoice", false);
                }
            }

            key.Close();
        }

        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            using FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtExtractPath.Text = fbd.SelectedPath;
            }
        }

        //Sync CurrentSettings with the UI state and save to file
        private void btnOK_Click(object sender, System.EventArgs e)
        {
            Settings.CurrentSettings.DefaultExtractPath = txtExtractPath.Text;
            Settings.CurrentSettings.FilesSortingColumn = lstSortBy.SelectedIndex;
            Settings.CurrentSettings.FilesSortOrder = (SortOrder)(lstSortOrder.SelectedIndex + 1);
            Settings.CurrentSettings.OpenFolderAfterExtract = cbxOpenDir.Checked;
            Settings.CurrentSettings.ShowCommandBar = cbxShowCommandBar.Checked;
            Settings.CurrentSettings.ShowDeletedItems = cbxShowDeletedItems.Checked;
            Settings.CurrentSettings.ShowDirectoryTree = cbxShowDirectoryTree.Checked;
            Settings.CurrentSettings.ShowHiddenItems = cbxShowHiddenItems.Checked;
            Settings.CurrentSettings.ShowStatusBar = cbxShowStatusBar.Checked;
            Settings.CurrentSettings.ExtractAlwaysAsk = cbxExtractAsk.Checked;
            Settings.CurrentSettings.ExtractPreserveAttributes = cbxPreserveAttributes.Checked;
            Settings.CurrentSettings.ExtractPreserveDates = cbxPreserveDates.Checked;
            Settings.CurrentSettings.QueryShellForFileTypeInfo = cbxShellFileIcons.Checked;
            Settings.CurrentSettings.AutoIncrementFilename = cbxAutoincrementFilename.Checked;
            Settings.CurrentSettings.ConfirmDeletion = cbxConfirmDeletion.Checked;
            Settings.CurrentSettings.ConfirmInjection = cbxConfirmInjection.Checked;
            Settings.CurrentSettings.ConfirmOverwriteExtraction = cbxConfirmOverwriteExtract.Checked;
            Settings.CurrentSettings.MemoryMappingThreshold = (long)txtMemoryMapping.Value * 1048576;

            switch (lstSizeUnits.SelectedIndex)
            {
                case 0: Settings.CurrentSettings.SizeUnit = SizeUnit.Bytes; break;
                case 1: Settings.CurrentSettings.SizeUnit = SizeUnit.Decimal; break;
                case 2: Settings.CurrentSettings.SizeUnit = SizeUnit.Binary; break;
            }

            switch (lstViewType.SelectedIndex)
            {
                case 0: Settings.CurrentSettings.FilesView = View.LargeIcon; break;
                case 1: Settings.CurrentSettings.FilesView = View.SmallIcon; break;
                case 2: Settings.CurrentSettings.FilesView = View.List; break;
                case 3: Settings.CurrentSettings.FilesView = View.Details; break;
                case 4: Settings.CurrentSettings.FilesView = View.Tile; break;
            }

            if (rbnExtractFlat.Checked)
                Settings.CurrentSettings.DefaultExtractType = Settings.FolderExtract.Merge;
            else if (rbnExtractPreserve.Checked)
                Settings.CurrentSettings.DefaultExtractType = Settings.FolderExtract.Preserve;
            else if (rbnIgnoreFolders.Checked)
                Settings.CurrentSettings.DefaultExtractType = Settings.FolderExtract.Ignore;

            Settings.Save();

            SetFileAssociations();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lstFileTypes.Items)
            {
                lvi.Checked = true;
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lstFileTypes.Items)
            {
                lvi.Checked = false;
            }
        }

        private void btnClearTemp_Click(object sender, EventArgs e)
        {
            string tempdir = Path.Combine(Path.GetTempPath(), "TotalImage");
            if (Directory.Exists(tempdir))
            {
                Directory.Delete(tempdir, true);
            }

            Directory.CreateDirectory(tempdir);

            TaskDialog.ShowDialog(this, new TaskDialogPage()
            {
                Text = "Temporary folder has been successfully cleared.",
                Heading = "Temporary folder cleared",
                Caption = "Success",
                Buttons =
                {
                    TaskDialogButton.OK
                },
                Icon = TaskDialogIcon.Information,
                DefaultButton = TaskDialogButton.OK
            });
        }
    }
}