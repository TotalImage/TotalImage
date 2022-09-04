using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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
            cbxShowDirSizes.Checked = Settings.CurrentSettings.FileListShowDirSize;

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

            switch (Settings.CurrentSettings.DefaultDirectoryExtractionMode)
            {
                case DirectoryExtractionMode.Skip: rbnIgnoreFolders.Checked = true; break;
                case DirectoryExtractionMode.Merge: rbnExtractFlat.Checked = true; break;
                case DirectoryExtractionMode.Preserve: rbnExtractPreserve.Checked = true; break;
            }
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
            Settings.CurrentSettings.FileListShowDirSize = cbxShowDirSizes.Checked;

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
                Settings.CurrentSettings.DefaultDirectoryExtractionMode = DirectoryExtractionMode.Merge;
            else if (rbnExtractPreserve.Checked)
                Settings.CurrentSettings.DefaultDirectoryExtractionMode = DirectoryExtractionMode.Preserve;
            else if (rbnIgnoreFolders.Checked)
                Settings.CurrentSettings.DefaultDirectoryExtractionMode = DirectoryExtractionMode.Skip;

            Settings.Save();
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

        private void btnFileAssoc_Click(object sender, EventArgs e)
        {
            /* While the old command opens the Default apps page of Win10 Settings just fine, we still do it "the right way" on Win10 just in case
             * the old one ever stops working. */
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = Environment.OSVersion.Version.Major > 6 ? "ms-settings:defaultapps" : "control",
                Arguments = Environment.OSVersion.Version.Major == 6 ? "/name Microsoft.DefaultPrograms /page pageDefaultProgram" : "",
                UseShellExecute = true
            };

            Process.Start(psi);
        }
    }
}
