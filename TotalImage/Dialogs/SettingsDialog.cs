using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using TotalImage.Controls;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Settings;
using static Interop.UxTheme;

namespace TotalImage.Dialogs
{
    public partial class SettingsDialog : Form
    {
        // The values behind the items the designer puts in each combo box, in the same order.
        private static readonly ViewMode[] ViewModes = [ViewMode.LargeIcon, ViewMode.SmallIcon, ViewMode.List, ViewMode.Details];
        private static readonly SortDirection[] SortDirections = [SortDirection.Ascending, SortDirection.Descending];
        private static readonly SizeUnit[] SizeUnits = [SizeUnit.Bytes, SizeUnit.Decimal, SizeUnit.Binary];
        private static readonly ColorTheme[] ColorThemes = [ColorTheme.Classic, ColorTheme.System, ColorTheme.Dark];

        public SettingsDialog(SettingsViewModel viewModel)
        {
            InitializeComponent();

            // General
            colorThemeComboBox.BindChoices(ColorThemes, viewModel, nameof(SettingsViewModel.ColorTheme));
            memoryMappingThresholdUpDown.BindTwoWay(nameof(NumericUpDown.Value), viewModel, nameof(SettingsViewModel.MemoryMappingThresholdMiB));
            autoIncrementFilenameCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SettingsViewModel.AutoIncrementFilename));
            confirmDeletionCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SettingsViewModel.ConfirmDeletion));
            confirmInjectionCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SettingsViewModel.ConfirmInjection));
            clearRecentImagesButton.Command = viewModel.ClearRecentImagesCommand;
            clearTempFolderButton.Command = viewModel.ClearTempFolderCommand;
            openTempFolderButton.Command = viewModel.OpenTempFolderCommand;
            resetSettingsButton.Command = viewModel.ResetToDefaultsCommand;

            // View
            showCommandBarCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SettingsViewModel.ShowCommandBar));
            showStatusBarCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SettingsViewModel.ShowStatusBar));
            showDirectoryTreeCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SettingsViewModel.ShowDirectoryTree));
            showHiddenItemsCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SettingsViewModel.ShowHiddenItems));
            showDirectorySizesCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SettingsViewModel.ShowDirectorySizes));
            viewModeComboBox.BindChoices(ViewModes, viewModel, nameof(SettingsViewModel.FilesView));
            sortColumnComboBox.BindChoices(Enumerable.Range(0, sortColumnComboBox.Items.Count).ToArray(), viewModel, nameof(SettingsViewModel.SortColumn));
            sortDirectionComboBox.BindChoices(SortDirections, viewModel, nameof(SettingsViewModel.SortDirection));
            sizeUnitComboBox.BindChoices(SizeUnits, viewModel, nameof(SettingsViewModel.SizeUnit));
            sizeUnitPreviewLabel.BindOneWay(nameof(Label.Text), viewModel, nameof(SettingsViewModel.SizeUnitPreview));

            // Extraction
            extractAlwaysAskCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SettingsViewModel.ExtractAlwaysAsk));
            extractPathTextBox.BindTwoWay(nameof(TextBox.Text), viewModel, nameof(SettingsViewModel.ExtractPath));
            browseButton.Command = viewModel.BrowseExtractPathCommand;
            openFolderAfterExtractCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SettingsViewModel.OpenFolderAfterExtract));
            mergeFoldersRadioButton.BindRadioButton(viewModel, nameof(SettingsViewModel.DirectoryExtractionMode), DirectoryExtractionMode.Merge);
            preserveFoldersRadioButton.BindRadioButton(viewModel, nameof(SettingsViewModel.DirectoryExtractionMode), DirectoryExtractionMode.Preserve);
            skipFoldersRadioButton.BindRadioButton(viewModel, nameof(SettingsViewModel.DirectoryExtractionMode), DirectoryExtractionMode.Skip);
            foreach (Control control in new Control[] { extractPathTextBox, browseButton, openFolderAfterExtractCheckBox,
                mergeFoldersRadioButton, preserveFoldersRadioButton, skipFoldersRadioButton })
            {
                control.BindOneWay(nameof(Control.Enabled), viewModel, nameof(SettingsViewModel.CanEditExtractionDefaults));
            }
            preserveDatesCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SettingsViewModel.PreserveDates));
            preserveAttributesCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SettingsViewModel.PreserveAttributes));
            confirmOverwriteExtractionCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SettingsViewModel.ConfirmOverwriteExtraction));

            // Integration
            queryShellCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SettingsViewModel.QueryShellForFileTypeInfo));

            okButton.Command = viewModel.AcceptCommand;
        }

        private void SettingsDialog_Load(object? sender, EventArgs e)
        {
            /* colorThemeComboBox's Win32 handle is created eagerly because it lives on the initially-visible
             * tab, so it misses the dark mode theming that is applied lazily to handles on other tabs.
             * Explicitly set the theme based on the active color mode to work around this. */
            SetWindowTheme(colorThemeComboBox.Handle, Application.ColorMode == SystemColorMode.Dark || Application.IsDarkModeEnabled ? "DarkMode_CFD" : "", null);
        }

        // Opening the Windows "Default apps" settings is Windows-only, so it stays in this view.
        private void FileAssociationsButton_Click(object? sender, EventArgs e)
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
    }
}
