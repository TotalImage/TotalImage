using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;
using static Interop.UxTheme;

namespace TotalImage.Dialogs
{
    public partial class SettingsDialog : Form
    {
        private readonly ISettingsService settings;
        private readonly ITempDirectoryService tempDirectory;

        public SettingsDialog(ISettingsService settings, ITempDirectoryService tempDirectory)
        {
            this.settings = settings;
            this.tempDirectory = tempDirectory;
            InitializeComponent();
        }

        private void btnClearRecent_Click(object sender, System.EventArgs e)
        {
            settings.ClearRecentImages();

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
                settings.ResetToDefaults();
                settings.Save();

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

            /* lstColorMode's Win32 handle is created eagerly because it lives on the initially-visible
             * tab, so it misses the dark mode theming that is applied lazily to handles on other tabs.
             * Explicitly set the theme based on the active color mode to work around this. */
            SetWindowTheme(lstColorMode.Handle, Application.ColorMode == SystemColorMode.Dark || Application.IsDarkModeEnabled ? "DarkMode_CFD" : "", null);
        }

        //Syncs the dialog UI with CurrentSettings
        private void SyncUIWithSettings()
        {
            txtExtractPath.Text = settings.Current.DefaultExtractPath;
            cbxOpenDir.Checked = settings.Current.OpenFolderAfterExtract;
            cbxShowCommandBar.Checked = settings.Current.ShowCommandBar;
            cbxShowStatusBar.Checked = settings.Current.ShowStatusBar;
            cbxShowDirectoryTree.Checked = settings.Current.ShowDirectoryTree;
            cbxShowHiddenItems.Checked = settings.Current.ShowHiddenItems;
            lstSortBy.SelectedIndex = settings.Current.FilesSortingColumn;
            lstSortOrder.SelectedIndex = (int)settings.Current.FilesSortOrder - 1;
            cbxExtractAsk.Checked = settings.Current.ExtractAlwaysAsk;
            cbxPreserveAttributes.Checked = settings.Current.ExtractPreserveAttributes;
            cbxPreserveDates.Checked = settings.Current.ExtractPreserveDates;
            cbxShellFileIcons.Checked = settings.Current.QueryShellForFileTypeInfo;
            cbxAutoincrementFilename.Checked = settings.Current.AutoIncrementFilename;
            cbxConfirmDeletion.Checked = settings.Current.ConfirmDeletion;
            cbxConfirmInjection.Checked = settings.Current.ConfirmInjection;
            cbxConfirmOverwriteExtract.Checked = settings.Current.ConfirmOverwriteExtraction;
            txtMemoryMapping.Value = settings.Current.MemoryMappingThreshold / 1048576;
            cbxShowDirSizes.Checked = settings.Current.FileListShowDirSize;
            lstColorMode.SelectedIndex = (int)settings.Current.ColorMode;

            switch (settings.Current.FilesView)
            {
                case ViewMode.LargeIcon: lstViewType.SelectedIndex = 0; break;
                case ViewMode.SmallIcon: lstViewType.SelectedIndex = 1; break;
                case ViewMode.List: lstViewType.SelectedIndex = 2; break;
                case ViewMode.Details: lstViewType.SelectedIndex = 3; break;
                case ViewMode.Tile: lstViewType.SelectedIndex = 4; break;
            }

            switch (settings.Current.SizeUnit)
            {
                case SizeUnit.Bytes: lstSizeUnits.SelectedIndex = 0; break;
                case SizeUnit.Decimal: lstSizeUnits.SelectedIndex = 1; break;
                case SizeUnit.Binary: lstSizeUnits.SelectedIndex = 2; break;
            }

            switch (settings.Current.DefaultDirectoryExtractionMode)
            {
                case DirectoryExtractionMode.Skip: rbnIgnoreFolders.Checked = true; break;
                case DirectoryExtractionMode.Merge: rbnExtractFlat.Checked = true; break;
                case DirectoryExtractionMode.Preserve: rbnExtractPreserve.Checked = true; break;
            }
        }

        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            using FolderBrowserDialog fbd = new();
            fbd.ShowNewFolderButton = true;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtExtractPath.Text = fbd.SelectedPath;
            }
        }

        //Sync CurrentSettings with the UI state and save to file
        private void btnOK_Click(object sender, System.EventArgs e)
        {
            settings.Current.DefaultExtractPath = txtExtractPath.Text;
            settings.Current.FilesSortingColumn = lstSortBy.SelectedIndex;
            settings.Current.FilesSortOrder = (SortDirection)(lstSortOrder.SelectedIndex + 1);
            settings.Current.OpenFolderAfterExtract = cbxOpenDir.Checked;
            settings.Current.ShowCommandBar = cbxShowCommandBar.Checked;
            settings.Current.ShowDirectoryTree = cbxShowDirectoryTree.Checked;
            settings.Current.ShowHiddenItems = cbxShowHiddenItems.Checked;
            settings.Current.ShowStatusBar = cbxShowStatusBar.Checked;
            settings.Current.ExtractAlwaysAsk = cbxExtractAsk.Checked;
            settings.Current.ExtractPreserveAttributes = cbxPreserveAttributes.Checked;
            settings.Current.ExtractPreserveDates = cbxPreserveDates.Checked;
            settings.Current.QueryShellForFileTypeInfo = cbxShellFileIcons.Checked;
            settings.Current.AutoIncrementFilename = cbxAutoincrementFilename.Checked;
            settings.Current.ConfirmDeletion = cbxConfirmDeletion.Checked;
            settings.Current.ConfirmInjection = cbxConfirmInjection.Checked;
            settings.Current.ConfirmOverwriteExtraction = cbxConfirmOverwriteExtract.Checked;
            settings.Current.MemoryMappingThreshold = (long)txtMemoryMapping.Value * 1048576;
            settings.Current.FileListShowDirSize = cbxShowDirSizes.Checked;
            settings.Current.ColorMode = (ColorTheme)lstColorMode.SelectedIndex;

            switch (lstSizeUnits.SelectedIndex)
            {
                case 0: settings.Current.SizeUnit = SizeUnit.Bytes; break;
                case 1: settings.Current.SizeUnit = SizeUnit.Decimal; break;
                case 2: settings.Current.SizeUnit = SizeUnit.Binary; break;
            }

            switch (lstViewType.SelectedIndex)
            {
                case 0: settings.Current.FilesView = ViewMode.LargeIcon; break;
                case 1: settings.Current.FilesView = ViewMode.SmallIcon; break;
                case 2: settings.Current.FilesView = ViewMode.List; break;
                case 3: settings.Current.FilesView = ViewMode.Details; break;
                case 4: settings.Current.FilesView = ViewMode.Tile; break;
            }

            if (rbnExtractFlat.Checked)
                settings.Current.DefaultDirectoryExtractionMode = DirectoryExtractionMode.Merge;
            else if (rbnExtractPreserve.Checked)
                settings.Current.DefaultDirectoryExtractionMode = DirectoryExtractionMode.Preserve;
            else if (rbnIgnoreFolders.Checked)
                settings.Current.DefaultDirectoryExtractionMode = DirectoryExtractionMode.Skip;

            settings.Save();
        }

        private void btnClearTemp_Click(object sender, EventArgs e)
        {
            //If there's more than one instance running, deleting the temp folder now is a bad idea, so let's warn the user first just in case.
            if (Process.GetProcessesByName("TotalImage").Length > 1)
            {
                TaskDialogPage page = new()
                {
                    Text = $"More than one instance of TotalImage is currently running. Attempting to clear the temporary folder while other instances are " +
                    $"using it can cause unpredictable behavior.{Environment.NewLine}{Environment.NewLine}Are you sure you want to continue?",
                    Heading = "Multiple instances are running",
                    Caption = "Warning",
                    Buttons =
                    {
                        TaskDialogButton.Yes,
                        TaskDialogButton.No,
                    },
                    Icon = TaskDialogIcon.Warning,
                    DefaultButton = TaskDialogButton.No,
                    SizeToContent = true
                };

                TaskDialogButton result = TaskDialog.ShowDialog(this, page);

                if (result == TaskDialogButton.No)
                    return;
            }

            try
            {
                tempDirectory.Clear();
            }
            catch (Exception)
            {
                TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = "Temporary folder could not be cleared because it is in use by another process or inaccessible.",
                    Heading = "Cannot clear temporary folder",
                    Caption = "Error",
                    Buttons =
                    {
                        TaskDialogButton.OK
                    },
                    Icon = TaskDialogIcon.Error,
                    DefaultButton = TaskDialogButton.OK
                });

                return;
            }

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
            ProcessStartInfo psi = new()
            {
                FileName = Environment.OSVersion.Version.Major > 6 ? "ms-settings:defaultapps" : "control",
                Arguments = Environment.OSVersion.Version.Major == 6 ? "/name Microsoft.DefaultPrograms /page pageDefaultProgram" : "",
                UseShellExecute = true
            };

            Process.Start(psi);
        }

        private void lstSizeUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            SizeUnit previewUnit = settings.Current.SizeUnit;

            switch (lstSizeUnits.SelectedIndex)
            {
                case 0: previewUnit = SizeUnit.Bytes; break;
                case 1: previewUnit = SizeUnit.Decimal; break;
                case 2: previewUnit = SizeUnit.Binary; break;
            }

            lblSizeUnitsPreview.Text = $"Preview:{Environment.NewLine}{previewUnit.FormatSize(234)} | " +
                $"{previewUnit.FormatSize(2345)} | {previewUnit.FormatSize(2345678)} | " +
                $"{previewUnit.FormatSize(2345678901)} | {previewUnit.FormatSize(2345678901234)}";
        }

        private void cbxExtractAsk_CheckedChanged(object sender, EventArgs e)
        {
            rbnExtractFlat.Enabled = rbnExtractPreserve.Enabled = rbnIgnoreFolders.Enabled = btnBrowse.Enabled = txtExtractPath.Enabled =
                cbxOpenDir.Enabled = !cbxExtractAsk.Checked;
        }

        private void btnOpenTemp_Click(object sender, EventArgs e)
        {
            tempDirectory.EnsureRootExists();

            ProcessStartInfo psi = new()
            {
                FileName = tempDirectory.Root,
                UseShellExecute = true
            };

            Process.Start(psi);
        }
    }
}
