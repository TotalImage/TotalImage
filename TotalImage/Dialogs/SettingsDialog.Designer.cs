namespace TotalImage.Dialogs
{
    partial class SettingsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cancelButton = new System.Windows.Forms.Button();
            okButton = new System.Windows.Forms.Button();
            buttonPanel = new System.Windows.Forms.Panel();
            tabControl = new System.Windows.Forms.TabControl();
            generalTabPage = new System.Windows.Forms.TabPage();
            colorThemeGroupBox = new System.Windows.Forms.GroupBox();
            colorThemeComboBox = new System.Windows.Forms.ComboBox();
            colorThemeRestartLabel = new System.Windows.Forms.Label();
            colorThemeLabel = new System.Windows.Forms.Label();
            behaviorGroupBox = new System.Windows.Forms.GroupBox();
            memoryMappingUnitLabel = new System.Windows.Forms.Label();
            memoryMappingThresholdUpDown = new System.Windows.Forms.NumericUpDown();
            memoryMappingLabel = new System.Windows.Forms.Label();
            autoIncrementFilenameCheckBox = new System.Windows.Forms.CheckBox();
            confirmOverwriteExtractionCheckBox = new System.Windows.Forms.CheckBox();
            confirmDeletionCheckBox = new System.Windows.Forms.CheckBox();
            confirmInjectionCheckBox = new System.Windows.Forms.CheckBox();
            miscGroupBox = new System.Windows.Forms.GroupBox();
            openTempFolderButton = new System.Windows.Forms.Button();
            clearTempFolderLabel = new System.Windows.Forms.Label();
            clearRecentImagesLabel = new System.Windows.Forms.Label();
            resetSettingsLabel = new System.Windows.Forms.Label();
            clearTempFolderButton = new System.Windows.Forms.Button();
            clearRecentImagesButton = new System.Windows.Forms.Button();
            resetSettingsButton = new System.Windows.Forms.Button();
            viewTabPage = new System.Windows.Forms.TabPage();
            mainWindowGroupBox = new System.Windows.Forms.GroupBox();
            showCommandBarCheckBox = new System.Windows.Forms.CheckBox();
            showStatusBarCheckBox = new System.Windows.Forms.CheckBox();
            showDirectoryTreeCheckBox = new System.Windows.Forms.CheckBox();
            showHiddenItemsCheckBox = new System.Windows.Forms.CheckBox();
            fileListGroupBox = new System.Windows.Forms.GroupBox();
            showDirectorySizesCheckBox = new System.Windows.Forms.CheckBox();
            viewModeLabel = new System.Windows.Forms.Label();
            sortDirectionComboBox = new System.Windows.Forms.ComboBox();
            viewModeComboBox = new System.Windows.Forms.ComboBox();
            sortDirectionLabel = new System.Windows.Forms.Label();
            sortColumnLabel = new System.Windows.Forms.Label();
            sortColumnComboBox = new System.Windows.Forms.ComboBox();
            sizeUnitGroupBox = new System.Windows.Forms.GroupBox();
            sizeUnitTipLabel = new System.Windows.Forms.Label();
            sizeUnitPreviewLabel = new System.Windows.Forms.Label();
            sizeUnitLabel = new System.Windows.Forms.Label();
            sizeUnitComboBox = new System.Windows.Forms.ComboBox();
            extractionTabPage = new System.Windows.Forms.TabPage();
            extractionDefaultsGroupBox = new System.Windows.Forms.GroupBox();
            extractAlwaysAskCheckBox = new System.Windows.Forms.CheckBox();
            extractPathLabel = new System.Windows.Forms.Label();
            extractPathTextBox = new System.Windows.Forms.TextBox();
            openFolderAfterExtractCheckBox = new System.Windows.Forms.CheckBox();
            browseButton = new System.Windows.Forms.Button();
            preserveFoldersRadioButton = new System.Windows.Forms.RadioButton();
            skipFoldersRadioButton = new System.Windows.Forms.RadioButton();
            mergeFoldersRadioButton = new System.Windows.Forms.RadioButton();
            extractionPreserveGroupBox = new System.Windows.Forms.GroupBox();
            preserveDatesCheckBox = new System.Windows.Forms.CheckBox();
            preserveAttributesCheckBox = new System.Windows.Forms.CheckBox();
            integrationTabPage = new System.Windows.Forms.TabPage();
            fileAssociationsGroupBox = new System.Windows.Forms.GroupBox();
            fileAssociationsLabel = new System.Windows.Forms.Label();
            fileAssociationsButton = new System.Windows.Forms.Button();
            integrationMiscGroupBox = new System.Windows.Forms.GroupBox();
            queryShellLabel = new System.Windows.Forms.Label();
            queryShellCheckBox = new System.Windows.Forms.CheckBox();
            buttonPanel.SuspendLayout();
            tabControl.SuspendLayout();
            generalTabPage.SuspendLayout();
            colorThemeGroupBox.SuspendLayout();
            behaviorGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)memoryMappingThresholdUpDown).BeginInit();
            miscGroupBox.SuspendLayout();
            viewTabPage.SuspendLayout();
            mainWindowGroupBox.SuspendLayout();
            fileListGroupBox.SuspendLayout();
            sizeUnitGroupBox.SuspendLayout();
            extractionTabPage.SuspendLayout();
            extractionDefaultsGroupBox.SuspendLayout();
            extractionPreserveGroupBox.SuspendLayout();
            integrationTabPage.SuspendLayout();
            fileAssociationsGroupBox.SuspendLayout();
            integrationMiscGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // cancelButton
            // 
            cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(393, 12);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(80, 26);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Location = new System.Drawing.Point(307, 12);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(80, 26);
            okButton.TabIndex = 0;
            okButton.Text = "OK";
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = System.Drawing.SystemColors.Control;
            buttonPanel.Controls.Add(cancelButton);
            buttonPanel.Controls.Add(okButton);
            buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonPanel.Location = new System.Drawing.Point(0, 500);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(485, 50);
            buttonPanel.TabIndex = 2;
            // 
            // tabControl
            // 
            tabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl.Controls.Add(generalTabPage);
            tabControl.Controls.Add(viewTabPage);
            tabControl.Controls.Add(extractionTabPage);
            tabControl.Controls.Add(integrationTabPage);
            tabControl.Location = new System.Drawing.Point(12, 13);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(461, 482);
            tabControl.TabIndex = 3;
            // 
            // generalTabPage
            // 
            generalTabPage.BackColor = System.Drawing.SystemColors.Window;
            generalTabPage.Controls.Add(colorThemeGroupBox);
            generalTabPage.Controls.Add(behaviorGroupBox);
            generalTabPage.Controls.Add(miscGroupBox);
            generalTabPage.Location = new System.Drawing.Point(4, 24);
            generalTabPage.Name = "generalTabPage";
            generalTabPage.Padding = new System.Windows.Forms.Padding(3);
            generalTabPage.Size = new System.Drawing.Size(453, 454);
            generalTabPage.TabIndex = 0;
            generalTabPage.Text = "General";
            // 
            // colorThemeGroupBox
            // 
            colorThemeGroupBox.Controls.Add(colorThemeComboBox);
            colorThemeGroupBox.Controls.Add(colorThemeRestartLabel);
            colorThemeGroupBox.Controls.Add(colorThemeLabel);
            colorThemeGroupBox.Location = new System.Drawing.Point(6, 172);
            colorThemeGroupBox.Name = "colorThemeGroupBox";
            colorThemeGroupBox.Size = new System.Drawing.Size(441, 82);
            colorThemeGroupBox.TabIndex = 3;
            colorThemeGroupBox.TabStop = false;
            colorThemeGroupBox.Text = "Color mode";
            // 
            // colorThemeComboBox
            // 
            colorThemeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            colorThemeComboBox.FormattingEnabled = true;
            colorThemeComboBox.Items.AddRange(new object[] { "Light", "System-defined", "Dark" });
            colorThemeComboBox.Location = new System.Drawing.Point(149, 19);
            colorThemeComboBox.Name = "colorThemeComboBox";
            colorThemeComboBox.Size = new System.Drawing.Size(175, 23);
            colorThemeComboBox.TabIndex = 16;
            // 
            // colorThemeRestartLabel
            // 
            colorThemeRestartLabel.AutoSize = true;
            colorThemeRestartLabel.Location = new System.Drawing.Point(9, 54);
            colorThemeRestartLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            colorThemeRestartLabel.Name = "colorThemeRestartLabel";
            colorThemeRestartLabel.Size = new System.Drawing.Size(315, 15);
            colorThemeRestartLabel.TabIndex = 15;
            colorThemeRestartLabel.Text = "TotalImage must be restarted for this setting to take effect.";
            // 
            // colorThemeLabel
            // 
            colorThemeLabel.AutoSize = true;
            colorThemeLabel.Location = new System.Drawing.Point(9, 22);
            colorThemeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            colorThemeLabel.Name = "colorThemeLabel";
            colorThemeLabel.Size = new System.Drawing.Size(135, 15);
            colorThemeLabel.TabIndex = 14;
            colorThemeLabel.Text = "Application color mode:";
            // 
            // behaviorGroupBox
            // 
            behaviorGroupBox.Controls.Add(memoryMappingUnitLabel);
            behaviorGroupBox.Controls.Add(memoryMappingThresholdUpDown);
            behaviorGroupBox.Controls.Add(memoryMappingLabel);
            behaviorGroupBox.Controls.Add(autoIncrementFilenameCheckBox);
            behaviorGroupBox.Controls.Add(confirmOverwriteExtractionCheckBox);
            behaviorGroupBox.Controls.Add(confirmDeletionCheckBox);
            behaviorGroupBox.Controls.Add(confirmInjectionCheckBox);
            behaviorGroupBox.Location = new System.Drawing.Point(6, 6);
            behaviorGroupBox.Name = "behaviorGroupBox";
            behaviorGroupBox.Size = new System.Drawing.Size(441, 160);
            behaviorGroupBox.TabIndex = 2;
            behaviorGroupBox.TabStop = false;
            behaviorGroupBox.Text = "Behavior";
            // 
            // memoryMappingUnitLabel
            // 
            memoryMappingUnitLabel.AutoSize = true;
            memoryMappingUnitLabel.Enabled = false;
            memoryMappingUnitLabel.Location = new System.Drawing.Point(349, 130);
            memoryMappingUnitLabel.Name = "memoryMappingUnitLabel";
            memoryMappingUnitLabel.Size = new System.Drawing.Size(28, 15);
            memoryMappingUnitLabel.TabIndex = 21;
            memoryMappingUnitLabel.Text = "MiB";
            // 
            // memoryMappingThresholdUpDown
            // 
            memoryMappingThresholdUpDown.Enabled = false;
            memoryMappingThresholdUpDown.Location = new System.Drawing.Point(243, 127);
            memoryMappingThresholdUpDown.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            memoryMappingThresholdUpDown.Name = "memoryMappingThresholdUpDown";
            memoryMappingThresholdUpDown.Size = new System.Drawing.Size(100, 23);
            memoryMappingThresholdUpDown.TabIndex = 5;
            // 
            // memoryMappingLabel
            // 
            memoryMappingLabel.AutoSize = true;
            memoryMappingLabel.Enabled = false;
            memoryMappingLabel.Location = new System.Drawing.Point(9, 130);
            memoryMappingLabel.Name = "memoryMappingLabel";
            memoryMappingLabel.Size = new System.Drawing.Size(228, 15);
            memoryMappingLabel.TabIndex = 19;
            memoryMappingLabel.Text = "Threshold for mapping files into memory:";
            // 
            // autoIncrementFilenameCheckBox
            // 
            autoIncrementFilenameCheckBox.AutoSize = true;
            autoIncrementFilenameCheckBox.Enabled = false;
            autoIncrementFilenameCheckBox.Location = new System.Drawing.Point(11, 102);
            autoIncrementFilenameCheckBox.Name = "autoIncrementFilenameCheckBox";
            autoIncrementFilenameCheckBox.Size = new System.Drawing.Size(331, 19);
            autoIncrementFilenameCheckBox.TabIndex = 4;
            autoIncrementFilenameCheckBox.Text = "When saving, auto-increment last filename when possible";
            // 
            // confirmOverwriteExtractionCheckBox
            // 
            confirmOverwriteExtractionCheckBox.AutoSize = true;
            confirmOverwriteExtractionCheckBox.Location = new System.Drawing.Point(11, 22);
            confirmOverwriteExtractionCheckBox.Name = "confirmOverwriteExtractionCheckBox";
            confirmOverwriteExtractionCheckBox.Size = new System.Drawing.Size(215, 19);
            confirmOverwriteExtractionCheckBox.TabIndex = 1;
            confirmOverwriteExtractionCheckBox.Text = "Confirm overwrite during extraction";
            // 
            // confirmDeletionCheckBox
            // 
            confirmDeletionCheckBox.AutoSize = true;
            confirmDeletionCheckBox.Enabled = false;
            confirmDeletionCheckBox.Location = new System.Drawing.Point(11, 49);
            confirmDeletionCheckBox.Name = "confirmDeletionCheckBox";
            confirmDeletionCheckBox.Size = new System.Drawing.Size(116, 19);
            confirmDeletionCheckBox.TabIndex = 2;
            confirmDeletionCheckBox.Text = "Confirm deletion";
            // 
            // confirmInjectionCheckBox
            // 
            confirmInjectionCheckBox.AutoSize = true;
            confirmInjectionCheckBox.Enabled = false;
            confirmInjectionCheckBox.Location = new System.Drawing.Point(11, 75);
            confirmInjectionCheckBox.Name = "confirmInjectionCheckBox";
            confirmInjectionCheckBox.Size = new System.Drawing.Size(119, 19);
            confirmInjectionCheckBox.TabIndex = 3;
            confirmInjectionCheckBox.Text = "Confirm injection";
            // 
            // miscGroupBox
            // 
            miscGroupBox.Controls.Add(openTempFolderButton);
            miscGroupBox.Controls.Add(clearTempFolderLabel);
            miscGroupBox.Controls.Add(clearRecentImagesLabel);
            miscGroupBox.Controls.Add(resetSettingsLabel);
            miscGroupBox.Controls.Add(clearTempFolderButton);
            miscGroupBox.Controls.Add(clearRecentImagesButton);
            miscGroupBox.Controls.Add(resetSettingsButton);
            miscGroupBox.Location = new System.Drawing.Point(6, 260);
            miscGroupBox.Name = "miscGroupBox";
            miscGroupBox.Size = new System.Drawing.Size(441, 160);
            miscGroupBox.TabIndex = 1;
            miscGroupBox.TabStop = false;
            miscGroupBox.Text = "Advanced options";
            // 
            // openTempFolderButton
            // 
            openTempFolderButton.Location = new System.Drawing.Point(355, 123);
            openTempFolderButton.Name = "openTempFolderButton";
            openTempFolderButton.Size = new System.Drawing.Size(80, 26);
            openTempFolderButton.TabIndex = 10;
            openTempFolderButton.Text = "Open";
            openTempFolderButton.UseVisualStyleBackColor = true;
            // 
            // clearTempFolderLabel
            // 
            clearTempFolderLabel.AutoSize = true;
            clearTempFolderLabel.Location = new System.Drawing.Point(9, 96);
            clearTempFolderLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            clearTempFolderLabel.Name = "clearTempFolderLabel";
            clearTempFolderLabel.Size = new System.Drawing.Size(328, 15);
            clearTempFolderLabel.TabIndex = 9;
            clearTempFolderLabel.Text = "Clear TotalImage's temporary folder in local application data.";
            // 
            // clearRecentImagesLabel
            // 
            clearRecentImagesLabel.AutoSize = true;
            clearRecentImagesLabel.Location = new System.Drawing.Point(9, 61);
            clearRecentImagesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            clearRecentImagesLabel.Name = "clearRecentImagesLabel";
            clearRecentImagesLabel.Size = new System.Drawing.Size(152, 15);
            clearRecentImagesLabel.TabIndex = 8;
            clearRecentImagesLabel.Text = "Clear the recent images list.";
            // 
            // resetSettingsLabel
            // 
            resetSettingsLabel.AutoSize = true;
            resetSettingsLabel.Location = new System.Drawing.Point(9, 24);
            resetSettingsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            resetSettingsLabel.Name = "resetSettingsLabel";
            resetSettingsLabel.Size = new System.Drawing.Size(276, 15);
            resetSettingsLabel.TabIndex = 7;
            resetSettingsLabel.Text = "Reset all TotalImage settings to their default values.";
            // 
            // clearTempFolderButton
            // 
            clearTempFolderButton.Location = new System.Drawing.Point(355, 91);
            clearTempFolderButton.Name = "clearTempFolderButton";
            clearTempFolderButton.Size = new System.Drawing.Size(80, 26);
            clearTempFolderButton.TabIndex = 9;
            clearTempFolderButton.Text = "Clear";
            clearTempFolderButton.UseVisualStyleBackColor = true;
            // 
            // clearRecentImagesButton
            // 
            clearRecentImagesButton.Location = new System.Drawing.Point(355, 56);
            clearRecentImagesButton.Name = "clearRecentImagesButton";
            clearRecentImagesButton.Size = new System.Drawing.Size(80, 26);
            clearRecentImagesButton.TabIndex = 8;
            clearRecentImagesButton.Text = "Clear";
            clearRecentImagesButton.UseVisualStyleBackColor = true;
            // 
            // resetSettingsButton
            // 
            resetSettingsButton.Location = new System.Drawing.Point(355, 19);
            resetSettingsButton.Name = "resetSettingsButton";
            resetSettingsButton.Size = new System.Drawing.Size(80, 26);
            resetSettingsButton.TabIndex = 7;
            resetSettingsButton.Text = "Reset";
            resetSettingsButton.UseVisualStyleBackColor = true;
            // 
            // viewTabPage
            // 
            viewTabPage.BackColor = System.Drawing.SystemColors.Window;
            viewTabPage.Controls.Add(mainWindowGroupBox);
            viewTabPage.Controls.Add(fileListGroupBox);
            viewTabPage.Controls.Add(sizeUnitGroupBox);
            viewTabPage.Location = new System.Drawing.Point(4, 24);
            viewTabPage.Name = "viewTabPage";
            viewTabPage.Padding = new System.Windows.Forms.Padding(3);
            viewTabPage.Size = new System.Drawing.Size(453, 454);
            viewTabPage.TabIndex = 2;
            viewTabPage.Text = "View";
            // 
            // mainWindowGroupBox
            // 
            mainWindowGroupBox.Controls.Add(showCommandBarCheckBox);
            mainWindowGroupBox.Controls.Add(showStatusBarCheckBox);
            mainWindowGroupBox.Controls.Add(showDirectoryTreeCheckBox);
            mainWindowGroupBox.Controls.Add(showHiddenItemsCheckBox);
            mainWindowGroupBox.Location = new System.Drawing.Point(6, 6);
            mainWindowGroupBox.Name = "mainWindowGroupBox";
            mainWindowGroupBox.Size = new System.Drawing.Size(441, 106);
            mainWindowGroupBox.TabIndex = 21;
            mainWindowGroupBox.TabStop = false;
            mainWindowGroupBox.Text = "Main window";
            // 
            // showCommandBarCheckBox
            // 
            showCommandBarCheckBox.AutoSize = true;
            showCommandBarCheckBox.Location = new System.Drawing.Point(11, 22);
            showCommandBarCheckBox.Name = "showCommandBarCheckBox";
            showCommandBarCheckBox.Size = new System.Drawing.Size(133, 19);
            showCommandBarCheckBox.TabIndex = 12;
            showCommandBarCheckBox.Text = "Show command bar";
            // 
            // showStatusBarCheckBox
            // 
            showStatusBarCheckBox.AutoSize = true;
            showStatusBarCheckBox.Location = new System.Drawing.Point(221, 22);
            showStatusBarCheckBox.Name = "showStatusBarCheckBox";
            showStatusBarCheckBox.Size = new System.Drawing.Size(109, 19);
            showStatusBarCheckBox.TabIndex = 16;
            showStatusBarCheckBox.Text = "Show status bar";
            // 
            // showDirectoryTreeCheckBox
            // 
            showDirectoryTreeCheckBox.AutoSize = true;
            showDirectoryTreeCheckBox.Location = new System.Drawing.Point(11, 49);
            showDirectoryTreeCheckBox.Name = "showDirectoryTreeCheckBox";
            showDirectoryTreeCheckBox.Size = new System.Drawing.Size(128, 19);
            showDirectoryTreeCheckBox.TabIndex = 15;
            showDirectoryTreeCheckBox.Text = "Show directory tree";
            // 
            // showHiddenItemsCheckBox
            // 
            showHiddenItemsCheckBox.AutoSize = true;
            showHiddenItemsCheckBox.Location = new System.Drawing.Point(221, 48);
            showHiddenItemsCheckBox.Name = "showHiddenItemsCheckBox";
            showHiddenItemsCheckBox.Size = new System.Drawing.Size(136, 19);
            showHiddenItemsCheckBox.TabIndex = 14;
            showHiddenItemsCheckBox.Text = "Show hidden objects";
            // 
            // fileListGroupBox
            // 
            fileListGroupBox.Controls.Add(showDirectorySizesCheckBox);
            fileListGroupBox.Controls.Add(viewModeLabel);
            fileListGroupBox.Controls.Add(sortDirectionComboBox);
            fileListGroupBox.Controls.Add(viewModeComboBox);
            fileListGroupBox.Controls.Add(sortDirectionLabel);
            fileListGroupBox.Controls.Add(sortColumnLabel);
            fileListGroupBox.Controls.Add(sortColumnComboBox);
            fileListGroupBox.Location = new System.Drawing.Point(6, 118);
            fileListGroupBox.Name = "fileListGroupBox";
            fileListGroupBox.Size = new System.Drawing.Size(441, 126);
            fileListGroupBox.TabIndex = 22;
            fileListGroupBox.TabStop = false;
            fileListGroupBox.Text = "File list";
            // 
            // showDirectorySizesCheckBox
            // 
            showDirectorySizesCheckBox.AutoSize = true;
            showDirectorySizesCheckBox.Location = new System.Drawing.Point(221, 88);
            showDirectorySizesCheckBox.Name = "showDirectorySizesCheckBox";
            showDirectorySizesCheckBox.Size = new System.Drawing.Size(182, 19);
            showDirectorySizesCheckBox.TabIndex = 20;
            showDirectorySizesCheckBox.Text = "Show directory sizes in file list";
            // 
            // viewModeLabel
            // 
            viewModeLabel.AutoSize = true;
            viewModeLabel.Location = new System.Drawing.Point(9, 19);
            viewModeLabel.Name = "viewModeLabel";
            viewModeLabel.Size = new System.Drawing.Size(144, 15);
            viewModeLabel.TabIndex = 6;
            viewModeLabel.Text = "Display items in file list as:";
            // 
            // sortDirectionComboBox
            // 
            sortDirectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            sortDirectionComboBox.FormattingEnabled = true;
            sortDirectionComboBox.Items.AddRange(new object[] { "Ascending", "Descending" });
            sortDirectionComboBox.Location = new System.Drawing.Point(11, 88);
            sortDirectionComboBox.Name = "sortDirectionComboBox";
            sortDirectionComboBox.Size = new System.Drawing.Size(170, 23);
            sortDirectionComboBox.TabIndex = 19;
            // 
            // viewModeComboBox
            // 
            viewModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            viewModeComboBox.FormattingEnabled = true;
            viewModeComboBox.Items.AddRange(new object[] { "Large icons", "Small icons", "List", "Details" });
            viewModeComboBox.Location = new System.Drawing.Point(11, 37);
            viewModeComboBox.Name = "viewModeComboBox";
            viewModeComboBox.Size = new System.Drawing.Size(170, 23);
            viewModeComboBox.TabIndex = 7;
            // 
            // sortDirectionLabel
            // 
            sortDirectionLabel.AutoSize = true;
            sortDirectionLabel.Location = new System.Drawing.Point(9, 70);
            sortDirectionLabel.Name = "sortDirectionLabel";
            sortDirectionLabel.Size = new System.Drawing.Size(134, 15);
            sortDirectionLabel.TabIndex = 18;
            sortDirectionLabel.Text = "Sorting order for file list:";
            // 
            // sortColumnLabel
            // 
            sortColumnLabel.AutoSize = true;
            sortColumnLabel.Location = new System.Drawing.Point(219, 19);
            sortColumnLabel.Name = "sortColumnLabel";
            sortColumnLabel.Size = new System.Drawing.Size(129, 15);
            sortColumnLabel.TabIndex = 8;
            sortColumnLabel.Text = "Sort items in file list by:";
            // 
            // sortColumnComboBox
            // 
            sortColumnComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            sortColumnComboBox.FormattingEnabled = true;
            sortColumnComboBox.Items.AddRange(new object[] { "Name", "Type", "Size", "Modified", "Attributes" });
            sortColumnComboBox.Location = new System.Drawing.Point(221, 37);
            sortColumnComboBox.Name = "sortColumnComboBox";
            sortColumnComboBox.Size = new System.Drawing.Size(170, 23);
            sortColumnComboBox.TabIndex = 9;
            // 
            // sizeUnitGroupBox
            // 
            sizeUnitGroupBox.Controls.Add(sizeUnitTipLabel);
            sizeUnitGroupBox.Controls.Add(sizeUnitPreviewLabel);
            sizeUnitGroupBox.Controls.Add(sizeUnitLabel);
            sizeUnitGroupBox.Controls.Add(sizeUnitComboBox);
            sizeUnitGroupBox.Location = new System.Drawing.Point(6, 250);
            sizeUnitGroupBox.Name = "sizeUnitGroupBox";
            sizeUnitGroupBox.Size = new System.Drawing.Size(441, 116);
            sizeUnitGroupBox.TabIndex = 23;
            sizeUnitGroupBox.TabStop = false;
            sizeUnitGroupBox.Text = "Size units";
            // 
            // sizeUnitTipLabel
            // 
            sizeUnitTipLabel.AutoSize = true;
            sizeUnitTipLabel.Location = new System.Drawing.Point(219, 19);
            sizeUnitTipLabel.Name = "sizeUnitTipLabel";
            sizeUnitTipLabel.Size = new System.Drawing.Size(214, 45);
            sizeUnitTipLabel.TabIndex = 22;
            sizeUnitTipLabel.Text = "For binary and decimal, TotalImage will\r\nautomatically determine the most\r\nappropriate unit and convert to it.";
            // 
            // sizeUnitPreviewLabel
            // 
            sizeUnitPreviewLabel.AutoSize = true;
            sizeUnitPreviewLabel.Location = new System.Drawing.Point(9, 73);
            sizeUnitPreviewLabel.Name = "sizeUnitPreviewLabel";
            sizeUnitPreviewLabel.Size = new System.Drawing.Size(303, 30);
            sizeUnitPreviewLabel.TabIndex = 21;
            sizeUnitPreviewLabel.Text = "Preview:\r\n234 B | 2345 B | 234567 B | 234567890 B | 2345678901234 B";
            // 
            // sizeUnitLabel
            // 
            sizeUnitLabel.AutoSize = true;
            sizeUnitLabel.Location = new System.Drawing.Point(9, 19);
            sizeUnitLabel.Name = "sizeUnitLabel";
            sizeUnitLabel.Size = new System.Drawing.Size(80, 15);
            sizeUnitLabel.TabIndex = 10;
            sizeUnitLabel.Text = "Show sizes as:";
            // 
            // sizeUnitComboBox
            // 
            sizeUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            sizeUnitComboBox.FormattingEnabled = true;
            sizeUnitComboBox.Items.AddRange(new object[] { "Bytes", "Decimal units (factor 1000)", "Binary units (factor 1024)" });
            sizeUnitComboBox.Location = new System.Drawing.Point(12, 37);
            sizeUnitComboBox.Name = "sizeUnitComboBox";
            sizeUnitComboBox.Size = new System.Drawing.Size(170, 23);
            sizeUnitComboBox.TabIndex = 11;
            // 
            // extractionTabPage
            // 
            extractionTabPage.BackColor = System.Drawing.SystemColors.Window;
            extractionTabPage.Controls.Add(extractionDefaultsGroupBox);
            extractionTabPage.Controls.Add(extractionPreserveGroupBox);
            extractionTabPage.Location = new System.Drawing.Point(4, 24);
            extractionTabPage.Name = "extractionTabPage";
            extractionTabPage.Padding = new System.Windows.Forms.Padding(3);
            extractionTabPage.Size = new System.Drawing.Size(453, 454);
            extractionTabPage.TabIndex = 3;
            extractionTabPage.Text = "Extraction";
            // 
            // extractionDefaultsGroupBox
            // 
            extractionDefaultsGroupBox.Controls.Add(extractAlwaysAskCheckBox);
            extractionDefaultsGroupBox.Controls.Add(extractPathLabel);
            extractionDefaultsGroupBox.Controls.Add(extractPathTextBox);
            extractionDefaultsGroupBox.Controls.Add(openFolderAfterExtractCheckBox);
            extractionDefaultsGroupBox.Controls.Add(browseButton);
            extractionDefaultsGroupBox.Controls.Add(preserveFoldersRadioButton);
            extractionDefaultsGroupBox.Controls.Add(skipFoldersRadioButton);
            extractionDefaultsGroupBox.Controls.Add(mergeFoldersRadioButton);
            extractionDefaultsGroupBox.Location = new System.Drawing.Point(6, 6);
            extractionDefaultsGroupBox.Name = "extractionDefaultsGroupBox";
            extractionDefaultsGroupBox.Size = new System.Drawing.Size(441, 208);
            extractionDefaultsGroupBox.TabIndex = 22;
            extractionDefaultsGroupBox.TabStop = false;
            extractionDefaultsGroupBox.Text = "Default options";
            // 
            // extractAlwaysAskCheckBox
            // 
            extractAlwaysAskCheckBox.AutoSize = true;
            extractAlwaysAskCheckBox.Location = new System.Drawing.Point(11, 22);
            extractAlwaysAskCheckBox.Name = "extractAlwaysAskCheckBox";
            extractAlwaysAskCheckBox.Size = new System.Drawing.Size(322, 19);
            extractAlwaysAskCheckBox.TabIndex = 0;
            extractAlwaysAskCheckBox.Text = "Always ask for these extraction options before extracting";
            // 
            // extractPathLabel
            // 
            extractPathLabel.AutoSize = true;
            extractPathLabel.Location = new System.Drawing.Point(8, 50);
            extractPathLabel.Name = "extractPathLabel";
            extractPathLabel.Size = new System.Drawing.Size(268, 15);
            extractPathLabel.TabIndex = 1;
            extractPathLabel.Text = "Extract selected item(s) to the following directory:";
            // 
            // extractPathTextBox
            // 
            extractPathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            extractPathTextBox.Location = new System.Drawing.Point(11, 68);
            extractPathTextBox.Name = "extractPathTextBox";
            extractPathTextBox.Size = new System.Drawing.Size(338, 23);
            extractPathTextBox.TabIndex = 2;
            // 
            // openFolderAfterExtractCheckBox
            // 
            openFolderAfterExtractCheckBox.AutoSize = true;
            openFolderAfterExtractCheckBox.Location = new System.Drawing.Point(11, 179);
            openFolderAfterExtractCheckBox.Name = "openFolderAfterExtractCheckBox";
            openFolderAfterExtractCheckBox.Size = new System.Drawing.Size(249, 19);
            openFolderAfterExtractCheckBox.TabIndex = 7;
            openFolderAfterExtractCheckBox.Text = "Open destination directory after extraction";
            // 
            // browseButton
            // 
            browseButton.Location = new System.Drawing.Point(355, 66);
            browseButton.Name = "browseButton";
            browseButton.Size = new System.Drawing.Size(80, 26);
            browseButton.TabIndex = 3;
            browseButton.Text = "Browse...";
            browseButton.UseVisualStyleBackColor = true;
            // 
            // preserveFoldersRadioButton
            // 
            preserveFoldersRadioButton.AutoSize = true;
            preserveFoldersRadioButton.Location = new System.Drawing.Point(11, 153);
            preserveFoldersRadioButton.Name = "preserveFoldersRadioButton";
            preserveFoldersRadioButton.Size = new System.Drawing.Size(212, 19);
            preserveFoldersRadioButton.TabIndex = 6;
            preserveFoldersRadioButton.TabStop = true;
            preserveFoldersRadioButton.Text = "Preserve original directory structure";
            // 
            // skipFoldersRadioButton
            // 
            skipFoldersRadioButton.AutoSize = true;
            skipFoldersRadioButton.Location = new System.Drawing.Point(11, 103);
            skipFoldersRadioButton.Name = "skipFoldersRadioButton";
            skipFoldersRadioButton.Size = new System.Drawing.Size(217, 19);
            skipFoldersRadioButton.TabIndex = 4;
            skipFoldersRadioButton.TabStop = true;
            skipFoldersRadioButton.Text = "Ignore directories and subdirectories";
            // 
            // mergeFoldersRadioButton
            // 
            mergeFoldersRadioButton.AutoSize = true;
            mergeFoldersRadioButton.Location = new System.Drawing.Point(11, 128);
            mergeFoldersRadioButton.Name = "mergeFoldersRadioButton";
            mergeFoldersRadioButton.Size = new System.Drawing.Size(224, 19);
            mergeFoldersRadioButton.TabIndex = 5;
            mergeFoldersRadioButton.TabStop = true;
            mergeFoldersRadioButton.Text = "Extract all files into the same directory";
            // 
            // extractionPreserveGroupBox
            // 
            extractionPreserveGroupBox.Controls.Add(preserveDatesCheckBox);
            extractionPreserveGroupBox.Controls.Add(preserveAttributesCheckBox);
            extractionPreserveGroupBox.Location = new System.Drawing.Point(6, 220);
            extractionPreserveGroupBox.Name = "extractionPreserveGroupBox";
            extractionPreserveGroupBox.Size = new System.Drawing.Size(441, 80);
            extractionPreserveGroupBox.TabIndex = 23;
            extractionPreserveGroupBox.TabStop = false;
            extractionPreserveGroupBox.Text = "Preserve information";
            // 
            // preserveDatesCheckBox
            // 
            preserveDatesCheckBox.AutoSize = true;
            preserveDatesCheckBox.Location = new System.Drawing.Point(11, 22);
            preserveDatesCheckBox.Name = "preserveDatesCheckBox";
            preserveDatesCheckBox.Size = new System.Drawing.Size(207, 19);
            preserveDatesCheckBox.TabIndex = 8;
            preserveDatesCheckBox.Text = "Preserve file dates when extracting";
            // 
            // preserveAttributesCheckBox
            // 
            preserveAttributesCheckBox.AutoSize = true;
            preserveAttributesCheckBox.Location = new System.Drawing.Point(11, 49);
            preserveAttributesCheckBox.Name = "preserveAttributesCheckBox";
            preserveAttributesCheckBox.Size = new System.Drawing.Size(229, 19);
            preserveAttributesCheckBox.TabIndex = 9;
            preserveAttributesCheckBox.Text = "Preserve file attributes when extracting";
            // 
            // integrationTabPage
            // 
            integrationTabPage.BackColor = System.Drawing.SystemColors.Window;
            integrationTabPage.Controls.Add(fileAssociationsGroupBox);
            integrationTabPage.Controls.Add(integrationMiscGroupBox);
            integrationTabPage.Location = new System.Drawing.Point(4, 24);
            integrationTabPage.Name = "integrationTabPage";
            integrationTabPage.Padding = new System.Windows.Forms.Padding(3);
            integrationTabPage.Size = new System.Drawing.Size(453, 454);
            integrationTabPage.TabIndex = 1;
            integrationTabPage.Text = "Integration";
            // 
            // fileAssociationsGroupBox
            // 
            fileAssociationsGroupBox.Controls.Add(fileAssociationsLabel);
            fileAssociationsGroupBox.Controls.Add(fileAssociationsButton);
            fileAssociationsGroupBox.Location = new System.Drawing.Point(6, 6);
            fileAssociationsGroupBox.Name = "fileAssociationsGroupBox";
            fileAssociationsGroupBox.Size = new System.Drawing.Size(441, 68);
            fileAssociationsGroupBox.TabIndex = 8;
            fileAssociationsGroupBox.TabStop = false;
            fileAssociationsGroupBox.Text = "File associations";
            // 
            // fileAssociationsLabel
            // 
            fileAssociationsLabel.AutoSize = true;
            fileAssociationsLabel.Location = new System.Drawing.Point(9, 21);
            fileAssociationsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            fileAssociationsLabel.Name = "fileAssociationsLabel";
            fileAssociationsLabel.Size = new System.Drawing.Size(309, 30);
            fileAssociationsLabel.TabIndex = 4;
            fileAssociationsLabel.Text = "File associations for TotalImage can be managed through\r\nControl Panel/Windows Settings.";
            // 
            // fileAssociationsButton
            // 
            fileAssociationsButton.Location = new System.Drawing.Point(356, 21);
            fileAssociationsButton.Margin = new System.Windows.Forms.Padding(2);
            fileAssociationsButton.Name = "fileAssociationsButton";
            fileAssociationsButton.Size = new System.Drawing.Size(80, 26);
            fileAssociationsButton.TabIndex = 5;
            fileAssociationsButton.Text = "Open";
            fileAssociationsButton.UseVisualStyleBackColor = true;
            fileAssociationsButton.Click += FileAssociationsButton_Click;
            // 
            // integrationMiscGroupBox
            // 
            integrationMiscGroupBox.Controls.Add(queryShellLabel);
            integrationMiscGroupBox.Controls.Add(queryShellCheckBox);
            integrationMiscGroupBox.Location = new System.Drawing.Point(6, 79);
            integrationMiscGroupBox.Margin = new System.Windows.Forms.Padding(2);
            integrationMiscGroupBox.Name = "integrationMiscGroupBox";
            integrationMiscGroupBox.Padding = new System.Windows.Forms.Padding(2);
            integrationMiscGroupBox.Size = new System.Drawing.Size(441, 120);
            integrationMiscGroupBox.TabIndex = 7;
            integrationMiscGroupBox.TabStop = false;
            integrationMiscGroupBox.Text = "Other options";
            // 
            // queryShellLabel
            // 
            queryShellLabel.AutoSize = true;
            queryShellLabel.Location = new System.Drawing.Point(9, 51);
            queryShellLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryShellLabel.Name = "queryShellLabel";
            queryShellLabel.Size = new System.Drawing.Size(413, 45);
            queryShellLabel.TabIndex = 6;
            queryShellLabel.Text = "If this option is enabled, TotalImage will obtain icons and names for file types\r\nfrom Windows, which can be slower in some situations. If disabled, generic\r\nicons and names will be used instead.";
            // 
            // queryShellCheckBox
            // 
            queryShellCheckBox.AutoSize = true;
            queryShellCheckBox.Location = new System.Drawing.Point(11, 22);
            queryShellCheckBox.Name = "queryShellCheckBox";
            queryShellCheckBox.Size = new System.Drawing.Size(291, 19);
            queryShellCheckBox.TabIndex = 3;
            queryShellCheckBox.Text = "Display system icons and file type names in file list";
            // 
            // SettingsDialog
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(485, 550);
            Controls.Add(tabControl);
            Controls.Add(buttonPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Settings";
            Load += SettingsDialog_Load;
            buttonPanel.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            generalTabPage.ResumeLayout(false);
            colorThemeGroupBox.ResumeLayout(false);
            colorThemeGroupBox.PerformLayout();
            behaviorGroupBox.ResumeLayout(false);
            behaviorGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)memoryMappingThresholdUpDown).EndInit();
            miscGroupBox.ResumeLayout(false);
            miscGroupBox.PerformLayout();
            viewTabPage.ResumeLayout(false);
            mainWindowGroupBox.ResumeLayout(false);
            mainWindowGroupBox.PerformLayout();
            fileListGroupBox.ResumeLayout(false);
            fileListGroupBox.PerformLayout();
            sizeUnitGroupBox.ResumeLayout(false);
            sizeUnitGroupBox.PerformLayout();
            extractionTabPage.ResumeLayout(false);
            extractionDefaultsGroupBox.ResumeLayout(false);
            extractionDefaultsGroupBox.PerformLayout();
            extractionPreserveGroupBox.ResumeLayout(false);
            extractionPreserveGroupBox.PerformLayout();
            integrationTabPage.ResumeLayout(false);
            fileAssociationsGroupBox.ResumeLayout(false);
            fileAssociationsGroupBox.PerformLayout();
            integrationMiscGroupBox.ResumeLayout(false);
            integrationMiscGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage generalTabPage;
        private System.Windows.Forms.TabPage integrationTabPage;
        private System.Windows.Forms.TabPage viewTabPage;
        private System.Windows.Forms.TabPage extractionTabPage;
        private System.Windows.Forms.CheckBox extractAlwaysAskCheckBox;
        private System.Windows.Forms.Label extractPathLabel;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox extractPathTextBox;
        private System.Windows.Forms.RadioButton skipFoldersRadioButton;
        private System.Windows.Forms.RadioButton preserveFoldersRadioButton;
        private System.Windows.Forms.RadioButton mergeFoldersRadioButton;
        private System.Windows.Forms.CheckBox openFolderAfterExtractCheckBox;
        private System.Windows.Forms.ComboBox sizeUnitComboBox;
        private System.Windows.Forms.Label sizeUnitLabel;
        private System.Windows.Forms.ComboBox sortColumnComboBox;
        private System.Windows.Forms.Label sortColumnLabel;
        private System.Windows.Forms.ComboBox viewModeComboBox;
        private System.Windows.Forms.Label viewModeLabel;
        private System.Windows.Forms.CheckBox showCommandBarCheckBox;
        private System.Windows.Forms.CheckBox showHiddenItemsCheckBox;
        private System.Windows.Forms.CheckBox showDirectoryTreeCheckBox;
        private System.Windows.Forms.CheckBox showStatusBarCheckBox;
        private System.Windows.Forms.Label sortDirectionLabel;
        private System.Windows.Forms.ComboBox sortDirectionComboBox;
        private System.Windows.Forms.GroupBox behaviorGroupBox;
        private System.Windows.Forms.GroupBox miscGroupBox;
        private System.Windows.Forms.Button clearRecentImagesButton;
        private System.Windows.Forms.Button resetSettingsButton;
        private System.Windows.Forms.CheckBox preserveAttributesCheckBox;
        private System.Windows.Forms.CheckBox preserveDatesCheckBox;
        private System.Windows.Forms.CheckBox queryShellCheckBox;
        private System.Windows.Forms.CheckBox confirmInjectionCheckBox;
        private System.Windows.Forms.CheckBox confirmDeletionCheckBox;
        private System.Windows.Forms.CheckBox confirmOverwriteExtractionCheckBox;
        private System.Windows.Forms.CheckBox autoIncrementFilenameCheckBox;
        private System.Windows.Forms.Label memoryMappingUnitLabel;
        private System.Windows.Forms.Label memoryMappingLabel;
        private System.Windows.Forms.NumericUpDown memoryMappingThresholdUpDown;
        private System.Windows.Forms.Button clearTempFolderButton;
        private System.Windows.Forms.Button fileAssociationsButton;
        private System.Windows.Forms.Label fileAssociationsLabel;
        private System.Windows.Forms.GroupBox integrationMiscGroupBox;
        private System.Windows.Forms.Label queryShellLabel;
        private System.Windows.Forms.Label resetSettingsLabel;
        private System.Windows.Forms.Label clearTempFolderLabel;
        private System.Windows.Forms.Label clearRecentImagesLabel;
        private System.Windows.Forms.CheckBox showDirectorySizesCheckBox;
        private System.Windows.Forms.GroupBox fileAssociationsGroupBox;
        private System.Windows.Forms.GroupBox fileListGroupBox;
        private System.Windows.Forms.GroupBox mainWindowGroupBox;
        private System.Windows.Forms.GroupBox sizeUnitGroupBox;
        private System.Windows.Forms.Label sizeUnitPreviewLabel;
        private System.Windows.Forms.Label sizeUnitTipLabel;
        private System.Windows.Forms.GroupBox extractionDefaultsGroupBox;
        private System.Windows.Forms.GroupBox extractionPreserveGroupBox;
        private System.Windows.Forms.Button openTempFolderButton;
        private System.Windows.Forms.GroupBox colorThemeGroupBox;
        private System.Windows.Forms.ComboBox colorThemeComboBox;
        private System.Windows.Forms.Label colorThemeRestartLabel;
        private System.Windows.Forms.Label colorThemeLabel;
    }
}
