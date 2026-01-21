namespace TotalImage
{
    partial class dlgSettings
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
            btnCancel = new System.Windows.Forms.Button();
            btnOK = new System.Windows.Forms.Button();
            pnlBottom = new System.Windows.Forms.Panel();
            tabs = new System.Windows.Forms.TabControl();
            tabGeneral = new System.Windows.Forms.TabPage();
            gbxMisc = new System.Windows.Forms.GroupBox();
            btnOpenTemp = new System.Windows.Forms.Button();
            lblClearTemp = new System.Windows.Forms.Label();
            lblClearRecent = new System.Windows.Forms.Label();
            lblResetSettings = new System.Windows.Forms.Label();
            btnClearTemp = new System.Windows.Forms.Button();
            btnClearRecent = new System.Windows.Forms.Button();
            btnResetSettings = new System.Windows.Forms.Button();
            gbxBehavior = new System.Windows.Forms.GroupBox();
            lblMemoryMapping1 = new System.Windows.Forms.Label();
            txtMemoryMapping = new System.Windows.Forms.NumericUpDown();
            lblMemoryMapping = new System.Windows.Forms.Label();
            cbxAutoincrementFilename = new System.Windows.Forms.CheckBox();
            cbxConfirmOverwriteExtract = new System.Windows.Forms.CheckBox();
            cbxConfirmDeletion = new System.Windows.Forms.CheckBox();
            cbxConfirmInjection = new System.Windows.Forms.CheckBox();
            tabView = new System.Windows.Forms.TabPage();
            gbxSizeUnits = new System.Windows.Forms.GroupBox();
            lblSizeUnitsTip = new System.Windows.Forms.Label();
            lblSizeUnitsPreview = new System.Windows.Forms.Label();
            lblSizeUnits = new System.Windows.Forms.Label();
            lstSizeUnits = new System.Windows.Forms.ComboBox();
            gbxFileList = new System.Windows.Forms.GroupBox();
            cbxShowDirSizes = new System.Windows.Forms.CheckBox();
            lblViewType = new System.Windows.Forms.Label();
            lstSortOrder = new System.Windows.Forms.ComboBox();
            lstViewType = new System.Windows.Forms.ComboBox();
            lblSortOrder = new System.Windows.Forms.Label();
            lblSortBy = new System.Windows.Forms.Label();
            lstSortBy = new System.Windows.Forms.ComboBox();
            gbxMainWindow = new System.Windows.Forms.GroupBox();
            cbxShowCommandBar = new System.Windows.Forms.CheckBox();
            cbxShowStatusBar = new System.Windows.Forms.CheckBox();
            cbxShowDirectoryTree = new System.Windows.Forms.CheckBox();
            cbxShowHiddenItems = new System.Windows.Forms.CheckBox();
            tabExtraction = new System.Windows.Forms.TabPage();
            gbxExtractionPreserve = new System.Windows.Forms.GroupBox();
            cbxPreserveDates = new System.Windows.Forms.CheckBox();
            cbxPreserveAttributes = new System.Windows.Forms.CheckBox();
            gbxExtractionDefaults = new System.Windows.Forms.GroupBox();
            cbxExtractAsk = new System.Windows.Forms.CheckBox();
            lblExtractPath = new System.Windows.Forms.Label();
            txtExtractPath = new System.Windows.Forms.TextBox();
            cbxOpenDir = new System.Windows.Forms.CheckBox();
            btnBrowse = new System.Windows.Forms.Button();
            rbnExtractPreserve = new System.Windows.Forms.RadioButton();
            rbnIgnoreFolders = new System.Windows.Forms.RadioButton();
            rbnExtractFlat = new System.Windows.Forms.RadioButton();
            tabIntegration = new System.Windows.Forms.TabPage();
            gbxFileAssociations = new System.Windows.Forms.GroupBox();
            lblFileAssoc = new System.Windows.Forms.Label();
            btnFileAssoc = new System.Windows.Forms.Button();
            gbxIntegrationMisc = new System.Windows.Forms.GroupBox();
            lblSystemIcons = new System.Windows.Forms.Label();
            cbxShellFileIcons = new System.Windows.Forms.CheckBox();
            pnlBottom.SuspendLayout();
            tabs.SuspendLayout();
            tabGeneral.SuspendLayout();
            gbxMisc.SuspendLayout();
            gbxBehavior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtMemoryMapping).BeginInit();
            tabView.SuspendLayout();
            gbxSizeUnits.SuspendLayout();
            gbxFileList.SuspendLayout();
            gbxMainWindow.SuspendLayout();
            tabExtraction.SuspendLayout();
            gbxExtractionPreserve.SuspendLayout();
            gbxExtractionDefaults.SuspendLayout();
            tabIntegration.SuspendLayout();
            gbxFileAssociations.SuspendLayout();
            gbxIntegrationMisc.SuspendLayout();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(393, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(80, 26);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(307, 12);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.Click += btnOK_Click;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 500);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(485, 50);
            pnlBottom.TabIndex = 2;
            // 
            // tabs
            // 
            tabs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabs.Controls.Add(tabGeneral);
            tabs.Controls.Add(tabView);
            tabs.Controls.Add(tabExtraction);
            tabs.Controls.Add(tabIntegration);
            tabs.Location = new System.Drawing.Point(12, 13);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new System.Drawing.Size(461, 482);
            tabs.TabIndex = 3;
            // 
            // tabGeneral
            // 
            tabGeneral.BackColor = System.Drawing.SystemColors.Window;
            tabGeneral.Controls.Add(gbxMisc);
            tabGeneral.Controls.Add(gbxBehavior);
            tabGeneral.Location = new System.Drawing.Point(4, 24);
            tabGeneral.Name = "tabGeneral";
            tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            tabGeneral.Size = new System.Drawing.Size(453, 454);
            tabGeneral.TabIndex = 0;
            tabGeneral.Text = "General";
            // 
            // gbxMisc
            // 
            gbxMisc.Controls.Add(btnOpenTemp);
            gbxMisc.Controls.Add(lblClearTemp);
            gbxMisc.Controls.Add(lblClearRecent);
            gbxMisc.Controls.Add(lblResetSettings);
            gbxMisc.Controls.Add(btnClearTemp);
            gbxMisc.Controls.Add(btnClearRecent);
            gbxMisc.Controls.Add(btnResetSettings);
            gbxMisc.Location = new System.Drawing.Point(6, 173);
            gbxMisc.Name = "gbxMisc";
            gbxMisc.Size = new System.Drawing.Size(442, 160);
            gbxMisc.TabIndex = 1;
            gbxMisc.TabStop = false;
            gbxMisc.Text = "Advanced options";
            // 
            // btnOpenTemp
            // 
            btnOpenTemp.Location = new System.Drawing.Point(355, 123);
            btnOpenTemp.Name = "btnOpenTemp";
            btnOpenTemp.Size = new System.Drawing.Size(80, 26);
            btnOpenTemp.TabIndex = 10;
            btnOpenTemp.Text = "Open";
            btnOpenTemp.UseVisualStyleBackColor = true;
            btnOpenTemp.Click += btnOpenTemp_Click;
            // 
            // lblClearTemp
            // 
            lblClearTemp.AutoSize = true;
            lblClearTemp.Location = new System.Drawing.Point(9, 96);
            lblClearTemp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblClearTemp.Name = "lblClearTemp";
            lblClearTemp.Size = new System.Drawing.Size(328, 15);
            lblClearTemp.TabIndex = 9;
            lblClearTemp.Text = "Clear TotalImage's temporary folder in local application data.";
            // 
            // lblClearRecent
            // 
            lblClearRecent.AutoSize = true;
            lblClearRecent.Location = new System.Drawing.Point(9, 61);
            lblClearRecent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblClearRecent.Name = "lblClearRecent";
            lblClearRecent.Size = new System.Drawing.Size(152, 15);
            lblClearRecent.TabIndex = 8;
            lblClearRecent.Text = "Clear the recent images list.";
            // 
            // lblResetSettings
            // 
            lblResetSettings.AutoSize = true;
            lblResetSettings.Location = new System.Drawing.Point(9, 24);
            lblResetSettings.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblResetSettings.Name = "lblResetSettings";
            lblResetSettings.Size = new System.Drawing.Size(276, 15);
            lblResetSettings.TabIndex = 7;
            lblResetSettings.Text = "Reset all TotalImage settings to their default values.";
            // 
            // btnClearTemp
            // 
            btnClearTemp.Location = new System.Drawing.Point(355, 91);
            btnClearTemp.Name = "btnClearTemp";
            btnClearTemp.Size = new System.Drawing.Size(80, 26);
            btnClearTemp.TabIndex = 6;
            btnClearTemp.Text = "Clear";
            btnClearTemp.UseVisualStyleBackColor = true;
            btnClearTemp.Click += btnClearTemp_Click;
            // 
            // btnClearRecent
            // 
            btnClearRecent.Location = new System.Drawing.Point(355, 56);
            btnClearRecent.Name = "btnClearRecent";
            btnClearRecent.Size = new System.Drawing.Size(80, 26);
            btnClearRecent.TabIndex = 5;
            btnClearRecent.Text = "Clear";
            btnClearRecent.UseVisualStyleBackColor = true;
            btnClearRecent.Click += btnClearRecent_Click;
            // 
            // btnResetSettings
            // 
            btnResetSettings.Location = new System.Drawing.Point(355, 19);
            btnResetSettings.Name = "btnResetSettings";
            btnResetSettings.Size = new System.Drawing.Size(80, 26);
            btnResetSettings.TabIndex = 4;
            btnResetSettings.Text = "Reset";
            btnResetSettings.UseVisualStyleBackColor = true;
            btnResetSettings.Click += btnReset_Click;
            // 
            // gbxBehavior
            // 
            gbxBehavior.Controls.Add(lblMemoryMapping1);
            gbxBehavior.Controls.Add(txtMemoryMapping);
            gbxBehavior.Controls.Add(lblMemoryMapping);
            gbxBehavior.Controls.Add(cbxAutoincrementFilename);
            gbxBehavior.Controls.Add(cbxConfirmOverwriteExtract);
            gbxBehavior.Controls.Add(cbxConfirmDeletion);
            gbxBehavior.Controls.Add(cbxConfirmInjection);
            gbxBehavior.Location = new System.Drawing.Point(6, 6);
            gbxBehavior.Name = "gbxBehavior";
            gbxBehavior.Size = new System.Drawing.Size(442, 160);
            gbxBehavior.TabIndex = 0;
            gbxBehavior.TabStop = false;
            gbxBehavior.Text = "Behavior";
            // 
            // lblMemoryMapping1
            // 
            lblMemoryMapping1.AutoSize = true;
            lblMemoryMapping1.Enabled = false;
            lblMemoryMapping1.Location = new System.Drawing.Point(349, 130);
            lblMemoryMapping1.Name = "lblMemoryMapping1";
            lblMemoryMapping1.Size = new System.Drawing.Size(28, 15);
            lblMemoryMapping1.TabIndex = 21;
            lblMemoryMapping1.Text = "MiB";
            // 
            // txtMemoryMapping
            // 
            txtMemoryMapping.Enabled = false;
            txtMemoryMapping.Location = new System.Drawing.Point(243, 127);
            txtMemoryMapping.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            txtMemoryMapping.Name = "txtMemoryMapping";
            txtMemoryMapping.Size = new System.Drawing.Size(100, 23);
            txtMemoryMapping.TabIndex = 20;
            // 
            // lblMemoryMapping
            // 
            lblMemoryMapping.AutoSize = true;
            lblMemoryMapping.Enabled = false;
            lblMemoryMapping.Location = new System.Drawing.Point(9, 130);
            lblMemoryMapping.Name = "lblMemoryMapping";
            lblMemoryMapping.Size = new System.Drawing.Size(228, 15);
            lblMemoryMapping.TabIndex = 19;
            lblMemoryMapping.Text = "Threshold for mapping files into memory:";
            // 
            // cbxAutoincrementFilename
            // 
            cbxAutoincrementFilename.AutoSize = true;
            cbxAutoincrementFilename.Enabled = false;
            cbxAutoincrementFilename.Location = new System.Drawing.Point(11, 102);
            cbxAutoincrementFilename.Name = "cbxAutoincrementFilename";
            cbxAutoincrementFilename.Size = new System.Drawing.Size(331, 19);
            cbxAutoincrementFilename.TabIndex = 18;
            cbxAutoincrementFilename.Text = "When saving, auto-increment last filename when possible";
            // 
            // cbxConfirmOverwriteExtract
            // 
            cbxConfirmOverwriteExtract.AutoSize = true;
            cbxConfirmOverwriteExtract.Location = new System.Drawing.Point(11, 22);
            cbxConfirmOverwriteExtract.Name = "cbxConfirmOverwriteExtract";
            cbxConfirmOverwriteExtract.Size = new System.Drawing.Size(215, 19);
            cbxConfirmOverwriteExtract.TabIndex = 17;
            cbxConfirmOverwriteExtract.Text = "Confirm overwrite during extraction";
            // 
            // cbxConfirmDeletion
            // 
            cbxConfirmDeletion.AutoSize = true;
            cbxConfirmDeletion.Enabled = false;
            cbxConfirmDeletion.Location = new System.Drawing.Point(11, 49);
            cbxConfirmDeletion.Name = "cbxConfirmDeletion";
            cbxConfirmDeletion.Size = new System.Drawing.Size(116, 19);
            cbxConfirmDeletion.TabIndex = 16;
            cbxConfirmDeletion.Text = "Confirm deletion";
            // 
            // cbxConfirmInjection
            // 
            cbxConfirmInjection.AutoSize = true;
            cbxConfirmInjection.Enabled = false;
            cbxConfirmInjection.Location = new System.Drawing.Point(11, 75);
            cbxConfirmInjection.Name = "cbxConfirmInjection";
            cbxConfirmInjection.Size = new System.Drawing.Size(119, 19);
            cbxConfirmInjection.TabIndex = 15;
            cbxConfirmInjection.Text = "Confirm injection";
            // 
            // tabView
            // 
            tabView.BackColor = System.Drawing.SystemColors.Window;
            tabView.Controls.Add(gbxSizeUnits);
            tabView.Controls.Add(gbxFileList);
            tabView.Controls.Add(gbxMainWindow);
            tabView.Location = new System.Drawing.Point(4, 24);
            tabView.Name = "tabView";
            tabView.Padding = new System.Windows.Forms.Padding(3);
            tabView.Size = new System.Drawing.Size(453, 454);
            tabView.TabIndex = 2;
            tabView.Text = "View";
            // 
            // gbxSizeUnits
            // 
            gbxSizeUnits.Controls.Add(lblSizeUnitsTip);
            gbxSizeUnits.Controls.Add(lblSizeUnitsPreview);
            gbxSizeUnits.Controls.Add(lblSizeUnits);
            gbxSizeUnits.Controls.Add(lstSizeUnits);
            gbxSizeUnits.Location = new System.Drawing.Point(6, 251);
            gbxSizeUnits.Name = "gbxSizeUnits";
            gbxSizeUnits.Size = new System.Drawing.Size(442, 116);
            gbxSizeUnits.TabIndex = 23;
            gbxSizeUnits.TabStop = false;
            gbxSizeUnits.Text = "Size units";
            // 
            // lblSizeUnitsTip
            // 
            lblSizeUnitsTip.AutoSize = true;
            lblSizeUnitsTip.Location = new System.Drawing.Point(219, 19);
            lblSizeUnitsTip.Name = "lblSizeUnitsTip";
            lblSizeUnitsTip.Size = new System.Drawing.Size(214, 45);
            lblSizeUnitsTip.TabIndex = 22;
            lblSizeUnitsTip.Text = "For binary and decimal, TotalImage will\r\nautomatically determine the most\r\nappropriate unit and convert to it.";
            // 
            // lblSizeUnitsPreview
            // 
            lblSizeUnitsPreview.AutoSize = true;
            lblSizeUnitsPreview.Location = new System.Drawing.Point(9, 73);
            lblSizeUnitsPreview.Name = "lblSizeUnitsPreview";
            lblSizeUnitsPreview.Size = new System.Drawing.Size(303, 30);
            lblSizeUnitsPreview.TabIndex = 21;
            lblSizeUnitsPreview.Text = "Preview:\r\n234 B | 2345 B | 234567 B | 234567890 B | 2345678901234 B";
            // 
            // lblSizeUnits
            // 
            lblSizeUnits.AutoSize = true;
            lblSizeUnits.Location = new System.Drawing.Point(9, 19);
            lblSizeUnits.Name = "lblSizeUnits";
            lblSizeUnits.Size = new System.Drawing.Size(80, 15);
            lblSizeUnits.TabIndex = 10;
            lblSizeUnits.Text = "Show sizes as:";
            // 
            // lstSizeUnits
            // 
            lstSizeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstSizeUnits.FormattingEnabled = true;
            lstSizeUnits.Items.AddRange(new object[] { "Bytes", "Decimal units (factor 1000)", "Binary units (factor 1024)" });
            lstSizeUnits.Location = new System.Drawing.Point(12, 37);
            lstSizeUnits.Name = "lstSizeUnits";
            lstSizeUnits.Size = new System.Drawing.Size(170, 23);
            lstSizeUnits.TabIndex = 11;
            lstSizeUnits.SelectedIndexChanged += lstSizeUnits_SelectedIndexChanged;
            // 
            // gbxFileList
            // 
            gbxFileList.Controls.Add(cbxShowDirSizes);
            gbxFileList.Controls.Add(lblViewType);
            gbxFileList.Controls.Add(lstSortOrder);
            gbxFileList.Controls.Add(lstViewType);
            gbxFileList.Controls.Add(lblSortOrder);
            gbxFileList.Controls.Add(lblSortBy);
            gbxFileList.Controls.Add(lstSortBy);
            gbxFileList.Location = new System.Drawing.Point(6, 119);
            gbxFileList.Name = "gbxFileList";
            gbxFileList.Size = new System.Drawing.Size(442, 126);
            gbxFileList.TabIndex = 22;
            gbxFileList.TabStop = false;
            gbxFileList.Text = "File list";
            // 
            // cbxShowDirSizes
            // 
            cbxShowDirSizes.AutoSize = true;
            cbxShowDirSizes.Location = new System.Drawing.Point(221, 88);
            cbxShowDirSizes.Name = "cbxShowDirSizes";
            cbxShowDirSizes.Size = new System.Drawing.Size(182, 19);
            cbxShowDirSizes.TabIndex = 20;
            cbxShowDirSizes.Text = "Show directory sizes in file list";
            // 
            // lblViewType
            // 
            lblViewType.AutoSize = true;
            lblViewType.Location = new System.Drawing.Point(9, 19);
            lblViewType.Name = "lblViewType";
            lblViewType.Size = new System.Drawing.Size(144, 15);
            lblViewType.TabIndex = 6;
            lblViewType.Text = "Display items in file list as:";
            // 
            // lstSortOrder
            // 
            lstSortOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstSortOrder.FormattingEnabled = true;
            lstSortOrder.Items.AddRange(new object[] { "Ascending", "Descending" });
            lstSortOrder.Location = new System.Drawing.Point(11, 88);
            lstSortOrder.Name = "lstSortOrder";
            lstSortOrder.Size = new System.Drawing.Size(170, 23);
            lstSortOrder.TabIndex = 19;
            // 
            // lstViewType
            // 
            lstViewType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstViewType.FormattingEnabled = true;
            lstViewType.Items.AddRange(new object[] { "Large icons", "Small icons", "List", "Details" });
            lstViewType.Location = new System.Drawing.Point(11, 37);
            lstViewType.Name = "lstViewType";
            lstViewType.Size = new System.Drawing.Size(170, 23);
            lstViewType.TabIndex = 7;
            // 
            // lblSortOrder
            // 
            lblSortOrder.AutoSize = true;
            lblSortOrder.Location = new System.Drawing.Point(9, 70);
            lblSortOrder.Name = "lblSortOrder";
            lblSortOrder.Size = new System.Drawing.Size(134, 15);
            lblSortOrder.TabIndex = 18;
            lblSortOrder.Text = "Sorting order for file list:";
            // 
            // lblSortBy
            // 
            lblSortBy.AutoSize = true;
            lblSortBy.Location = new System.Drawing.Point(219, 19);
            lblSortBy.Name = "lblSortBy";
            lblSortBy.Size = new System.Drawing.Size(129, 15);
            lblSortBy.TabIndex = 8;
            lblSortBy.Text = "Sort items in file list by:";
            // 
            // lstSortBy
            // 
            lstSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstSortBy.FormattingEnabled = true;
            lstSortBy.Items.AddRange(new object[] { "Name", "Type", "Size", "Modified", "Attributes" });
            lstSortBy.Location = new System.Drawing.Point(221, 37);
            lstSortBy.Name = "lstSortBy";
            lstSortBy.Size = new System.Drawing.Size(170, 23);
            lstSortBy.TabIndex = 9;
            // 
            // gbxMainWindow
            // 
            gbxMainWindow.Controls.Add(cbxShowCommandBar);
            gbxMainWindow.Controls.Add(cbxShowStatusBar);
            gbxMainWindow.Controls.Add(cbxShowDirectoryTree);
            gbxMainWindow.Controls.Add(cbxShowHiddenItems);
            gbxMainWindow.Location = new System.Drawing.Point(6, 6);
            gbxMainWindow.Name = "gbxMainWindow";
            gbxMainWindow.Size = new System.Drawing.Size(442, 106);
            gbxMainWindow.TabIndex = 21;
            gbxMainWindow.TabStop = false;
            gbxMainWindow.Text = "Main window";
            // 
            // cbxShowCommandBar
            // 
            cbxShowCommandBar.AutoSize = true;
            cbxShowCommandBar.Location = new System.Drawing.Point(11, 22);
            cbxShowCommandBar.Name = "cbxShowCommandBar";
            cbxShowCommandBar.Size = new System.Drawing.Size(133, 19);
            cbxShowCommandBar.TabIndex = 12;
            cbxShowCommandBar.Text = "Show command bar";
            // 
            // cbxShowStatusBar
            // 
            cbxShowStatusBar.AutoSize = true;
            cbxShowStatusBar.Location = new System.Drawing.Point(221, 22);
            cbxShowStatusBar.Name = "cbxShowStatusBar";
            cbxShowStatusBar.Size = new System.Drawing.Size(109, 19);
            cbxShowStatusBar.TabIndex = 16;
            cbxShowStatusBar.Text = "Show status bar";
            // 
            // cbxShowDirectoryTree
            // 
            cbxShowDirectoryTree.AutoSize = true;
            cbxShowDirectoryTree.Location = new System.Drawing.Point(11, 49);
            cbxShowDirectoryTree.Name = "cbxShowDirectoryTree";
            cbxShowDirectoryTree.Size = new System.Drawing.Size(128, 19);
            cbxShowDirectoryTree.TabIndex = 15;
            cbxShowDirectoryTree.Text = "Show directory tree";
            // 
            // cbxShowHiddenItems
            // 
            cbxShowHiddenItems.AutoSize = true;
            cbxShowHiddenItems.Location = new System.Drawing.Point(221, 48);
            cbxShowHiddenItems.Name = "cbxShowHiddenItems";
            cbxShowHiddenItems.Size = new System.Drawing.Size(136, 19);
            cbxShowHiddenItems.TabIndex = 14;
            cbxShowHiddenItems.Text = "Show hidden objects";
            // 
            // tabExtraction
            // 
            tabExtraction.BackColor = System.Drawing.SystemColors.Window;
            tabExtraction.Controls.Add(gbxExtractionPreserve);
            tabExtraction.Controls.Add(gbxExtractionDefaults);
            tabExtraction.Location = new System.Drawing.Point(4, 24);
            tabExtraction.Name = "tabExtraction";
            tabExtraction.Padding = new System.Windows.Forms.Padding(3);
            tabExtraction.Size = new System.Drawing.Size(453, 454);
            tabExtraction.TabIndex = 3;
            tabExtraction.Text = "Extraction";
            // 
            // gbxExtractionPreserve
            // 
            gbxExtractionPreserve.Controls.Add(cbxPreserveDates);
            gbxExtractionPreserve.Controls.Add(cbxPreserveAttributes);
            gbxExtractionPreserve.Location = new System.Drawing.Point(6, 221);
            gbxExtractionPreserve.Name = "gbxExtractionPreserve";
            gbxExtractionPreserve.Size = new System.Drawing.Size(442, 80);
            gbxExtractionPreserve.TabIndex = 23;
            gbxExtractionPreserve.TabStop = false;
            gbxExtractionPreserve.Text = "Preserve information";
            // 
            // cbxPreserveDates
            // 
            cbxPreserveDates.AutoSize = true;
            cbxPreserveDates.Location = new System.Drawing.Point(11, 22);
            cbxPreserveDates.Name = "cbxPreserveDates";
            cbxPreserveDates.Size = new System.Drawing.Size(207, 19);
            cbxPreserveDates.TabIndex = 8;
            cbxPreserveDates.Text = "Preserve file dates when extracting";
            // 
            // cbxPreserveAttributes
            // 
            cbxPreserveAttributes.AutoSize = true;
            cbxPreserveAttributes.Location = new System.Drawing.Point(11, 49);
            cbxPreserveAttributes.Name = "cbxPreserveAttributes";
            cbxPreserveAttributes.Size = new System.Drawing.Size(229, 19);
            cbxPreserveAttributes.TabIndex = 9;
            cbxPreserveAttributes.Text = "Preserve file attributes when extracting";
            // 
            // gbxExtractionDefaults
            // 
            gbxExtractionDefaults.Controls.Add(cbxExtractAsk);
            gbxExtractionDefaults.Controls.Add(lblExtractPath);
            gbxExtractionDefaults.Controls.Add(txtExtractPath);
            gbxExtractionDefaults.Controls.Add(cbxOpenDir);
            gbxExtractionDefaults.Controls.Add(btnBrowse);
            gbxExtractionDefaults.Controls.Add(rbnExtractPreserve);
            gbxExtractionDefaults.Controls.Add(rbnIgnoreFolders);
            gbxExtractionDefaults.Controls.Add(rbnExtractFlat);
            gbxExtractionDefaults.Location = new System.Drawing.Point(6, 6);
            gbxExtractionDefaults.Name = "gbxExtractionDefaults";
            gbxExtractionDefaults.Size = new System.Drawing.Size(442, 208);
            gbxExtractionDefaults.TabIndex = 22;
            gbxExtractionDefaults.TabStop = false;
            gbxExtractionDefaults.Text = "Default options";
            // 
            // cbxExtractAsk
            // 
            cbxExtractAsk.AutoSize = true;
            cbxExtractAsk.Location = new System.Drawing.Point(11, 22);
            cbxExtractAsk.Name = "cbxExtractAsk";
            cbxExtractAsk.Size = new System.Drawing.Size(322, 19);
            cbxExtractAsk.TabIndex = 0;
            cbxExtractAsk.Text = "Always ask for these extraction options before extracting";
            cbxExtractAsk.CheckedChanged += cbxExtractAsk_CheckedChanged;
            // 
            // lblExtractPath
            // 
            lblExtractPath.AutoSize = true;
            lblExtractPath.Location = new System.Drawing.Point(8, 50);
            lblExtractPath.Name = "lblExtractPath";
            lblExtractPath.Size = new System.Drawing.Size(268, 15);
            lblExtractPath.TabIndex = 1;
            lblExtractPath.Text = "Extract selected item(s) to the following directory:";
            // 
            // txtExtractPath
            // 
            txtExtractPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtExtractPath.Location = new System.Drawing.Point(11, 68);
            txtExtractPath.Name = "txtExtractPath";
            txtExtractPath.Size = new System.Drawing.Size(338, 23);
            txtExtractPath.TabIndex = 2;
            // 
            // cbxOpenDir
            // 
            cbxOpenDir.AutoSize = true;
            cbxOpenDir.Location = new System.Drawing.Point(11, 179);
            cbxOpenDir.Name = "cbxOpenDir";
            cbxOpenDir.Size = new System.Drawing.Size(249, 19);
            cbxOpenDir.TabIndex = 7;
            cbxOpenDir.Text = "Open destination directory after extraction";
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new System.Drawing.Point(355, 66);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new System.Drawing.Size(80, 26);
            btnBrowse.TabIndex = 3;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // rbnExtractPreserve
            // 
            rbnExtractPreserve.AutoSize = true;
            rbnExtractPreserve.Location = new System.Drawing.Point(11, 153);
            rbnExtractPreserve.Name = "rbnExtractPreserve";
            rbnExtractPreserve.Size = new System.Drawing.Size(212, 19);
            rbnExtractPreserve.TabIndex = 6;
            rbnExtractPreserve.TabStop = true;
            rbnExtractPreserve.Text = "Preserve original directory structure";
            // 
            // rbnIgnoreFolders
            // 
            rbnIgnoreFolders.AutoSize = true;
            rbnIgnoreFolders.Location = new System.Drawing.Point(11, 103);
            rbnIgnoreFolders.Name = "rbnIgnoreFolders";
            rbnIgnoreFolders.Size = new System.Drawing.Size(217, 19);
            rbnIgnoreFolders.TabIndex = 4;
            rbnIgnoreFolders.TabStop = true;
            rbnIgnoreFolders.Text = "Ignore directories and subdirectories";
            // 
            // rbnExtractFlat
            // 
            rbnExtractFlat.AutoSize = true;
            rbnExtractFlat.Location = new System.Drawing.Point(11, 128);
            rbnExtractFlat.Name = "rbnExtractFlat";
            rbnExtractFlat.Size = new System.Drawing.Size(224, 19);
            rbnExtractFlat.TabIndex = 5;
            rbnExtractFlat.TabStop = true;
            rbnExtractFlat.Text = "Extract all files into the same directory";
            // 
            // tabIntegration
            // 
            tabIntegration.BackColor = System.Drawing.SystemColors.Window;
            tabIntegration.Controls.Add(gbxFileAssociations);
            tabIntegration.Controls.Add(gbxIntegrationMisc);
            tabIntegration.Location = new System.Drawing.Point(4, 24);
            tabIntegration.Name = "tabIntegration";
            tabIntegration.Padding = new System.Windows.Forms.Padding(3);
            tabIntegration.Size = new System.Drawing.Size(453, 454);
            tabIntegration.TabIndex = 1;
            tabIntegration.Text = "Integration";
            // 
            // gbxFileAssociations
            // 
            gbxFileAssociations.Controls.Add(lblFileAssoc);
            gbxFileAssociations.Controls.Add(btnFileAssoc);
            gbxFileAssociations.Location = new System.Drawing.Point(6, 6);
            gbxFileAssociations.Name = "gbxFileAssociations";
            gbxFileAssociations.Size = new System.Drawing.Size(442, 68);
            gbxFileAssociations.TabIndex = 8;
            gbxFileAssociations.TabStop = false;
            gbxFileAssociations.Text = "File associations";
            // 
            // lblFileAssoc
            // 
            lblFileAssoc.AutoSize = true;
            lblFileAssoc.Location = new System.Drawing.Point(9, 21);
            lblFileAssoc.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblFileAssoc.Name = "lblFileAssoc";
            lblFileAssoc.Size = new System.Drawing.Size(309, 30);
            lblFileAssoc.TabIndex = 4;
            lblFileAssoc.Text = "File associations for TotalImage can be managed through\r\nControl Panel/Windows Settings.";
            // 
            // btnFileAssoc
            // 
            btnFileAssoc.Location = new System.Drawing.Point(357, 21);
            btnFileAssoc.Margin = new System.Windows.Forms.Padding(2);
            btnFileAssoc.Name = "btnFileAssoc";
            btnFileAssoc.Size = new System.Drawing.Size(80, 26);
            btnFileAssoc.TabIndex = 5;
            btnFileAssoc.Text = "Open";
            btnFileAssoc.UseVisualStyleBackColor = true;
            btnFileAssoc.Click += btnFileAssoc_Click;
            // 
            // gbxIntegrationMisc
            // 
            gbxIntegrationMisc.Controls.Add(lblSystemIcons);
            gbxIntegrationMisc.Controls.Add(cbxShellFileIcons);
            gbxIntegrationMisc.Location = new System.Drawing.Point(6, 79);
            gbxIntegrationMisc.Margin = new System.Windows.Forms.Padding(2);
            gbxIntegrationMisc.Name = "gbxIntegrationMisc";
            gbxIntegrationMisc.Padding = new System.Windows.Forms.Padding(2);
            gbxIntegrationMisc.Size = new System.Drawing.Size(442, 120);
            gbxIntegrationMisc.TabIndex = 7;
            gbxIntegrationMisc.TabStop = false;
            gbxIntegrationMisc.Text = "Other options";
            // 
            // lblSystemIcons
            // 
            lblSystemIcons.AutoSize = true;
            lblSystemIcons.Location = new System.Drawing.Point(9, 51);
            lblSystemIcons.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblSystemIcons.Name = "lblSystemIcons";
            lblSystemIcons.Size = new System.Drawing.Size(413, 45);
            lblSystemIcons.TabIndex = 6;
            lblSystemIcons.Text = "If this option is enabled, TotalImage will obtain icons and names for file types\r\nfrom Windows, which can be slower in some situations. If disabled, generic\r\nicons and names will be used instead.";
            // 
            // cbxShellFileIcons
            // 
            cbxShellFileIcons.AutoSize = true;
            cbxShellFileIcons.Location = new System.Drawing.Point(11, 22);
            cbxShellFileIcons.Name = "cbxShellFileIcons";
            cbxShellFileIcons.Size = new System.Drawing.Size(291, 19);
            cbxShellFileIcons.TabIndex = 3;
            cbxShellFileIcons.Text = "Display system icons and file type names in file list";
            // 
            // dlgSettings
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(485, 550);
            Controls.Add(tabs);
            Controls.Add(pnlBottom);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgSettings";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Settings";
            Load += dlgSettings_Load;
            pnlBottom.ResumeLayout(false);
            tabs.ResumeLayout(false);
            tabGeneral.ResumeLayout(false);
            gbxMisc.ResumeLayout(false);
            gbxMisc.PerformLayout();
            gbxBehavior.ResumeLayout(false);
            gbxBehavior.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtMemoryMapping).EndInit();
            tabView.ResumeLayout(false);
            gbxSizeUnits.ResumeLayout(false);
            gbxSizeUnits.PerformLayout();
            gbxFileList.ResumeLayout(false);
            gbxFileList.PerformLayout();
            gbxMainWindow.ResumeLayout(false);
            gbxMainWindow.PerformLayout();
            tabExtraction.ResumeLayout(false);
            gbxExtractionPreserve.ResumeLayout(false);
            gbxExtractionPreserve.PerformLayout();
            gbxExtractionDefaults.ResumeLayout(false);
            gbxExtractionDefaults.PerformLayout();
            tabIntegration.ResumeLayout(false);
            gbxFileAssociations.ResumeLayout(false);
            gbxFileAssociations.PerformLayout();
            gbxIntegrationMisc.ResumeLayout(false);
            gbxIntegrationMisc.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabIntegration;
        private System.Windows.Forms.TabPage tabView;
        private System.Windows.Forms.TabPage tabExtraction;
        private System.Windows.Forms.CheckBox cbxExtractAsk;
        private System.Windows.Forms.Label lblExtractPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtExtractPath;
        private System.Windows.Forms.RadioButton rbnIgnoreFolders;
        private System.Windows.Forms.RadioButton rbnExtractPreserve;
        private System.Windows.Forms.RadioButton rbnExtractFlat;
        private System.Windows.Forms.CheckBox cbxOpenDir;
        private System.Windows.Forms.ComboBox lstSizeUnits;
        private System.Windows.Forms.Label lblSizeUnits;
        private System.Windows.Forms.ComboBox lstSortBy;
        private System.Windows.Forms.Label lblSortBy;
        private System.Windows.Forms.ComboBox lstViewType;
        private System.Windows.Forms.Label lblViewType;
        private System.Windows.Forms.CheckBox cbxShowCommandBar;
        private System.Windows.Forms.CheckBox cbxShowHiddenItems;
        private System.Windows.Forms.CheckBox cbxShowDirectoryTree;
        private System.Windows.Forms.CheckBox cbxShowStatusBar;
        private System.Windows.Forms.Label lblSortOrder;
        private System.Windows.Forms.ComboBox lstSortOrder;
        private System.Windows.Forms.GroupBox gbxBehavior;
        private System.Windows.Forms.GroupBox gbxMisc;
        private System.Windows.Forms.Button btnClearRecent;
        private System.Windows.Forms.Button btnResetSettings;
        private System.Windows.Forms.CheckBox cbxPreserveAttributes;
        private System.Windows.Forms.CheckBox cbxPreserveDates;
        private System.Windows.Forms.CheckBox cbxShellFileIcons;
        private System.Windows.Forms.CheckBox cbxConfirmInjection;
        private System.Windows.Forms.CheckBox cbxConfirmDeletion;
        private System.Windows.Forms.CheckBox cbxConfirmOverwriteExtract;
        private System.Windows.Forms.CheckBox cbxAutoincrementFilename;
        private System.Windows.Forms.Label lblMemoryMapping1;
        private System.Windows.Forms.Label lblMemoryMapping;
        private System.Windows.Forms.NumericUpDown txtMemoryMapping;
        private System.Windows.Forms.Button btnClearTemp;
        private System.Windows.Forms.Button btnFileAssoc;
        private System.Windows.Forms.Label lblFileAssoc;
        private System.Windows.Forms.GroupBox gbxIntegrationMisc;
        private System.Windows.Forms.Label lblSystemIcons;
        private System.Windows.Forms.Label lblResetSettings;
        private System.Windows.Forms.Label lblClearTemp;
        private System.Windows.Forms.Label lblClearRecent;
        private System.Windows.Forms.CheckBox cbxShowDirSizes;
        private System.Windows.Forms.GroupBox gbxFileAssociations;
        private System.Windows.Forms.GroupBox gbxFileList;
        private System.Windows.Forms.GroupBox gbxMainWindow;
        private System.Windows.Forms.GroupBox gbxSizeUnits;
        private System.Windows.Forms.Label lblSizeUnitsPreview;
        private System.Windows.Forms.Label lblSizeUnitsTip;
        private System.Windows.Forms.GroupBox gbxExtractionDefaults;
        private System.Windows.Forms.GroupBox gbxExtractionPreserve;
        private System.Windows.Forms.Button btnOpenTemp;
    }
}
