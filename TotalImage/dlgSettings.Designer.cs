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
            cbxShowDeletedItems = new System.Windows.Forms.CheckBox();
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
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnCancel.Location = new System.Drawing.Point(491, 15);
            btnCancel.Margin = new System.Windows.Forms.Padding(4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(100, 32);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnOK.Location = new System.Drawing.Point(384, 15);
            btnOK.Margin = new System.Windows.Forms.Padding(4);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(100, 32);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 626);
            pnlBottom.Margin = new System.Windows.Forms.Padding(4);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(606, 62);
            pnlBottom.TabIndex = 2;
            // 
            // tabs
            // 
            tabs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabs.Controls.Add(tabGeneral);
            tabs.Controls.Add(tabView);
            tabs.Controls.Add(tabExtraction);
            tabs.Controls.Add(tabIntegration);
            tabs.Location = new System.Drawing.Point(15, 16);
            tabs.Margin = new System.Windows.Forms.Padding(4);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new System.Drawing.Size(576, 602);
            tabs.TabIndex = 3;
            // 
            // tabGeneral
            // 
            tabGeneral.Controls.Add(gbxMisc);
            tabGeneral.Controls.Add(gbxBehavior);
            tabGeneral.Location = new System.Drawing.Point(4, 29);
            tabGeneral.Margin = new System.Windows.Forms.Padding(4);
            tabGeneral.Name = "tabGeneral";
            tabGeneral.Padding = new System.Windows.Forms.Padding(4);
            tabGeneral.Size = new System.Drawing.Size(568, 569);
            tabGeneral.TabIndex = 0;
            tabGeneral.Text = "General";
            tabGeneral.UseVisualStyleBackColor = true;
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
            gbxMisc.Location = new System.Drawing.Point(8, 216);
            gbxMisc.Margin = new System.Windows.Forms.Padding(4);
            gbxMisc.Name = "gbxMisc";
            gbxMisc.Padding = new System.Windows.Forms.Padding(4);
            gbxMisc.Size = new System.Drawing.Size(552, 200);
            gbxMisc.TabIndex = 1;
            gbxMisc.TabStop = false;
            gbxMisc.Text = "Advanced options";
            // 
            // btnOpenTemp
            // 
            btnOpenTemp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnOpenTemp.Location = new System.Drawing.Point(444, 154);
            btnOpenTemp.Margin = new System.Windows.Forms.Padding(4);
            btnOpenTemp.Name = "btnOpenTemp";
            btnOpenTemp.Size = new System.Drawing.Size(100, 32);
            btnOpenTemp.TabIndex = 10;
            btnOpenTemp.Text = "Open";
            btnOpenTemp.UseVisualStyleBackColor = true;
            btnOpenTemp.Click += btnOpenTemp_Click;
            // 
            // lblClearTemp
            // 
            lblClearTemp.AutoSize = true;
            lblClearTemp.Location = new System.Drawing.Point(11, 120);
            lblClearTemp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblClearTemp.Name = "lblClearTemp";
            lblClearTemp.Size = new System.Drawing.Size(416, 20);
            lblClearTemp.TabIndex = 9;
            lblClearTemp.Text = "Clear TotalImage's temporary folder in local application data.";
            // 
            // lblClearRecent
            // 
            lblClearRecent.AutoSize = true;
            lblClearRecent.Location = new System.Drawing.Point(11, 76);
            lblClearRecent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblClearRecent.Name = "lblClearRecent";
            lblClearRecent.Size = new System.Drawing.Size(191, 20);
            lblClearRecent.TabIndex = 8;
            lblClearRecent.Text = "Clear the recent images list.";
            // 
            // lblResetSettings
            // 
            lblResetSettings.AutoSize = true;
            lblResetSettings.Location = new System.Drawing.Point(11, 30);
            lblResetSettings.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblResetSettings.Name = "lblResetSettings";
            lblResetSettings.Size = new System.Drawing.Size(350, 20);
            lblResetSettings.TabIndex = 7;
            lblResetSettings.Text = "Reset all TotalImage settings to their default values.";
            // 
            // btnClearTemp
            // 
            btnClearTemp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnClearTemp.Location = new System.Drawing.Point(444, 114);
            btnClearTemp.Margin = new System.Windows.Forms.Padding(4);
            btnClearTemp.Name = "btnClearTemp";
            btnClearTemp.Size = new System.Drawing.Size(100, 32);
            btnClearTemp.TabIndex = 6;
            btnClearTemp.Text = "Clear";
            btnClearTemp.UseVisualStyleBackColor = true;
            btnClearTemp.Click += btnClearTemp_Click;
            // 
            // btnClearRecent
            // 
            btnClearRecent.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnClearRecent.Location = new System.Drawing.Point(444, 70);
            btnClearRecent.Margin = new System.Windows.Forms.Padding(4);
            btnClearRecent.Name = "btnClearRecent";
            btnClearRecent.Size = new System.Drawing.Size(100, 32);
            btnClearRecent.TabIndex = 5;
            btnClearRecent.Text = "Clear";
            btnClearRecent.UseVisualStyleBackColor = true;
            btnClearRecent.Click += btnClearRecent_Click;
            // 
            // btnResetSettings
            // 
            btnResetSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnResetSettings.Location = new System.Drawing.Point(444, 24);
            btnResetSettings.Margin = new System.Windows.Forms.Padding(4);
            btnResetSettings.Name = "btnResetSettings";
            btnResetSettings.Size = new System.Drawing.Size(100, 32);
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
            gbxBehavior.Location = new System.Drawing.Point(8, 8);
            gbxBehavior.Margin = new System.Windows.Forms.Padding(4);
            gbxBehavior.Name = "gbxBehavior";
            gbxBehavior.Padding = new System.Windows.Forms.Padding(4);
            gbxBehavior.Size = new System.Drawing.Size(552, 200);
            gbxBehavior.TabIndex = 0;
            gbxBehavior.TabStop = false;
            gbxBehavior.Text = "Behavior";
            // 
            // lblMemoryMapping1
            // 
            lblMemoryMapping1.AutoSize = true;
            lblMemoryMapping1.Enabled = false;
            lblMemoryMapping1.Location = new System.Drawing.Point(436, 163);
            lblMemoryMapping1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMemoryMapping1.Name = "lblMemoryMapping1";
            lblMemoryMapping1.Size = new System.Drawing.Size(35, 20);
            lblMemoryMapping1.TabIndex = 21;
            lblMemoryMapping1.Text = "MiB";
            // 
            // txtMemoryMapping
            // 
            txtMemoryMapping.Enabled = false;
            txtMemoryMapping.Location = new System.Drawing.Point(303, 161);
            txtMemoryMapping.Margin = new System.Windows.Forms.Padding(4);
            txtMemoryMapping.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            txtMemoryMapping.Name = "txtMemoryMapping";
            txtMemoryMapping.Size = new System.Drawing.Size(125, 27);
            txtMemoryMapping.TabIndex = 20;
            txtMemoryMapping.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblMemoryMapping
            // 
            lblMemoryMapping.AutoSize = true;
            lblMemoryMapping.Enabled = false;
            lblMemoryMapping.Location = new System.Drawing.Point(11, 163);
            lblMemoryMapping.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMemoryMapping.Name = "lblMemoryMapping";
            lblMemoryMapping.Size = new System.Drawing.Size(284, 20);
            lblMemoryMapping.TabIndex = 19;
            lblMemoryMapping.Text = "Threshold for mapping files into memory:";
            // 
            // cbxAutoincrementFilename
            // 
            cbxAutoincrementFilename.AutoSize = true;
            cbxAutoincrementFilename.Enabled = false;
            cbxAutoincrementFilename.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxAutoincrementFilename.Location = new System.Drawing.Point(14, 127);
            cbxAutoincrementFilename.Margin = new System.Windows.Forms.Padding(4);
            cbxAutoincrementFilename.Name = "cbxAutoincrementFilename";
            cbxAutoincrementFilename.Size = new System.Drawing.Size(420, 25);
            cbxAutoincrementFilename.TabIndex = 18;
            cbxAutoincrementFilename.Text = "When saving, auto-increment last filename when possible";
            cbxAutoincrementFilename.UseVisualStyleBackColor = true;
            // 
            // cbxConfirmOverwriteExtract
            // 
            cbxConfirmOverwriteExtract.AutoSize = true;
            cbxConfirmOverwriteExtract.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxConfirmOverwriteExtract.Location = new System.Drawing.Point(14, 28);
            cbxConfirmOverwriteExtract.Margin = new System.Windows.Forms.Padding(4);
            cbxConfirmOverwriteExtract.Name = "cbxConfirmOverwriteExtract";
            cbxConfirmOverwriteExtract.Size = new System.Drawing.Size(276, 25);
            cbxConfirmOverwriteExtract.TabIndex = 17;
            cbxConfirmOverwriteExtract.Text = "Confirm overwrite during extraction";
            cbxConfirmOverwriteExtract.UseVisualStyleBackColor = true;
            // 
            // cbxConfirmDeletion
            // 
            cbxConfirmDeletion.AutoSize = true;
            cbxConfirmDeletion.Enabled = false;
            cbxConfirmDeletion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxConfirmDeletion.Location = new System.Drawing.Point(14, 61);
            cbxConfirmDeletion.Margin = new System.Windows.Forms.Padding(4);
            cbxConfirmDeletion.Name = "cbxConfirmDeletion";
            cbxConfirmDeletion.Size = new System.Drawing.Size(152, 25);
            cbxConfirmDeletion.TabIndex = 16;
            cbxConfirmDeletion.Text = "Confirm deletion";
            cbxConfirmDeletion.UseVisualStyleBackColor = true;
            // 
            // cbxConfirmInjection
            // 
            cbxConfirmInjection.AutoSize = true;
            cbxConfirmInjection.Enabled = false;
            cbxConfirmInjection.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxConfirmInjection.Location = new System.Drawing.Point(14, 94);
            cbxConfirmInjection.Margin = new System.Windows.Forms.Padding(4);
            cbxConfirmInjection.Name = "cbxConfirmInjection";
            cbxConfirmInjection.Size = new System.Drawing.Size(154, 25);
            cbxConfirmInjection.TabIndex = 15;
            cbxConfirmInjection.Text = "Confirm injection";
            cbxConfirmInjection.UseVisualStyleBackColor = true;
            // 
            // tabView
            // 
            tabView.Controls.Add(gbxSizeUnits);
            tabView.Controls.Add(gbxFileList);
            tabView.Controls.Add(gbxMainWindow);
            tabView.Location = new System.Drawing.Point(4, 29);
            tabView.Margin = new System.Windows.Forms.Padding(4);
            tabView.Name = "tabView";
            tabView.Padding = new System.Windows.Forms.Padding(4);
            tabView.Size = new System.Drawing.Size(568, 569);
            tabView.TabIndex = 2;
            tabView.Text = "View";
            tabView.UseVisualStyleBackColor = true;
            // 
            // gbxSizeUnits
            // 
            gbxSizeUnits.Controls.Add(lblSizeUnitsTip);
            gbxSizeUnits.Controls.Add(lblSizeUnitsPreview);
            gbxSizeUnits.Controls.Add(lblSizeUnits);
            gbxSizeUnits.Controls.Add(lstSizeUnits);
            gbxSizeUnits.Location = new System.Drawing.Point(8, 314);
            gbxSizeUnits.Margin = new System.Windows.Forms.Padding(4);
            gbxSizeUnits.Name = "gbxSizeUnits";
            gbxSizeUnits.Padding = new System.Windows.Forms.Padding(4);
            gbxSizeUnits.Size = new System.Drawing.Size(552, 145);
            gbxSizeUnits.TabIndex = 23;
            gbxSizeUnits.TabStop = false;
            gbxSizeUnits.Text = "Size units";
            // 
            // lblSizeUnitsTip
            // 
            lblSizeUnitsTip.AutoSize = true;
            lblSizeUnitsTip.Location = new System.Drawing.Point(256, 24);
            lblSizeUnitsTip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSizeUnitsTip.Name = "lblSizeUnitsTip";
            lblSizeUnitsTip.Size = new System.Drawing.Size(283, 60);
            lblSizeUnitsTip.TabIndex = 22;
            lblSizeUnitsTip.Text = "For binary and decimal, TotalImage will\r\nautomatically determine the most\r\nappropriate unit and convert accordingly.";
            // 
            // lblSizeUnitsPreview
            // 
            lblSizeUnitsPreview.AutoSize = true;
            lblSizeUnitsPreview.Location = new System.Drawing.Point(14, 87);
            lblSizeUnitsPreview.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSizeUnitsPreview.Name = "lblSizeUnitsPreview";
            lblSizeUnitsPreview.Size = new System.Drawing.Size(382, 40);
            lblSizeUnitsPreview.TabIndex = 21;
            lblSizeUnitsPreview.Text = "Preview:\r\n234 B, 2345 B, 234567 B, 234567890 B, 2345678901234 B";
            // 
            // lblSizeUnits
            // 
            lblSizeUnits.AutoSize = true;
            lblSizeUnits.Location = new System.Drawing.Point(11, 24);
            lblSizeUnits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSizeUnits.Name = "lblSizeUnits";
            lblSizeUnits.Size = new System.Drawing.Size(101, 20);
            lblSizeUnits.TabIndex = 10;
            lblSizeUnits.Text = "Show sizes as:";
            // 
            // lstSizeUnits
            // 
            lstSizeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstSizeUnits.FlatStyle = System.Windows.Forms.FlatStyle.System;
            lstSizeUnits.FormattingEnabled = true;
            lstSizeUnits.Items.AddRange(new object[] { "Bytes", "Decimal units (factor 1000)", "Binary units (factor 1024)" });
            lstSizeUnits.Location = new System.Drawing.Point(15, 46);
            lstSizeUnits.Margin = new System.Windows.Forms.Padding(4);
            lstSizeUnits.Name = "lstSizeUnits";
            lstSizeUnits.Size = new System.Drawing.Size(212, 28);
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
            gbxFileList.Location = new System.Drawing.Point(8, 149);
            gbxFileList.Margin = new System.Windows.Forms.Padding(4);
            gbxFileList.Name = "gbxFileList";
            gbxFileList.Padding = new System.Windows.Forms.Padding(4);
            gbxFileList.Size = new System.Drawing.Size(552, 157);
            gbxFileList.TabIndex = 22;
            gbxFileList.TabStop = false;
            gbxFileList.Text = "File list";
            // 
            // cbxShowDirSizes
            // 
            cbxShowDirSizes.AutoSize = true;
            cbxShowDirSizes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxShowDirSizes.Location = new System.Drawing.Point(259, 110);
            cbxShowDirSizes.Margin = new System.Windows.Forms.Padding(4);
            cbxShowDirSizes.Name = "cbxShowDirSizes";
            cbxShowDirSizes.Size = new System.Drawing.Size(238, 25);
            cbxShowDirSizes.TabIndex = 20;
            cbxShowDirSizes.Text = "Show directory sizes in file list";
            cbxShowDirSizes.UseVisualStyleBackColor = true;
            // 
            // lblViewType
            // 
            lblViewType.AutoSize = true;
            lblViewType.Location = new System.Drawing.Point(11, 24);
            lblViewType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblViewType.Name = "lblViewType";
            lblViewType.Size = new System.Drawing.Size(183, 20);
            lblViewType.TabIndex = 6;
            lblViewType.Text = "Display items in file list as:";
            // 
            // lstSortOrder
            // 
            lstSortOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstSortOrder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            lstSortOrder.FormattingEnabled = true;
            lstSortOrder.Items.AddRange(new object[] { "Ascending", "Descending" });
            lstSortOrder.Location = new System.Drawing.Point(15, 110);
            lstSortOrder.Margin = new System.Windows.Forms.Padding(4);
            lstSortOrder.Name = "lstSortOrder";
            lstSortOrder.Size = new System.Drawing.Size(212, 28);
            lstSortOrder.TabIndex = 19;
            // 
            // lstViewType
            // 
            lstViewType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstViewType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            lstViewType.FormattingEnabled = true;
            lstViewType.Items.AddRange(new object[] { "Large icons", "Small icons", "List", "Details" });
            lstViewType.Location = new System.Drawing.Point(14, 46);
            lstViewType.Margin = new System.Windows.Forms.Padding(4);
            lstViewType.Name = "lstViewType";
            lstViewType.Size = new System.Drawing.Size(212, 28);
            lstViewType.TabIndex = 7;
            // 
            // lblSortOrder
            // 
            lblSortOrder.AutoSize = true;
            lblSortOrder.Location = new System.Drawing.Point(11, 87);
            lblSortOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSortOrder.Name = "lblSortOrder";
            lblSortOrder.Size = new System.Drawing.Size(171, 20);
            lblSortOrder.TabIndex = 18;
            lblSortOrder.Text = "Sorting order for file list:";
            // 
            // lblSortBy
            // 
            lblSortBy.AutoSize = true;
            lblSortBy.Location = new System.Drawing.Point(256, 24);
            lblSortBy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSortBy.Name = "lblSortBy";
            lblSortBy.Size = new System.Drawing.Size(163, 20);
            lblSortBy.TabIndex = 8;
            lblSortBy.Text = "Sort items in file list by:";
            // 
            // lstSortBy
            // 
            lstSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstSortBy.FlatStyle = System.Windows.Forms.FlatStyle.System;
            lstSortBy.FormattingEnabled = true;
            lstSortBy.Items.AddRange(new object[] { "Name", "Type", "Size", "Modified", "Attributes" });
            lstSortBy.Location = new System.Drawing.Point(259, 47);
            lstSortBy.Margin = new System.Windows.Forms.Padding(4);
            lstSortBy.Name = "lstSortBy";
            lstSortBy.Size = new System.Drawing.Size(212, 28);
            lstSortBy.TabIndex = 9;
            // 
            // gbxMainWindow
            // 
            gbxMainWindow.Controls.Add(cbxShowCommandBar);
            gbxMainWindow.Controls.Add(cbxShowStatusBar);
            gbxMainWindow.Controls.Add(cbxShowDirectoryTree);
            gbxMainWindow.Controls.Add(cbxShowDeletedItems);
            gbxMainWindow.Controls.Add(cbxShowHiddenItems);
            gbxMainWindow.Location = new System.Drawing.Point(8, 8);
            gbxMainWindow.Margin = new System.Windows.Forms.Padding(4);
            gbxMainWindow.Name = "gbxMainWindow";
            gbxMainWindow.Padding = new System.Windows.Forms.Padding(4);
            gbxMainWindow.Size = new System.Drawing.Size(552, 133);
            gbxMainWindow.TabIndex = 21;
            gbxMainWindow.TabStop = false;
            gbxMainWindow.Text = "Main window";
            // 
            // cbxShowCommandBar
            // 
            cbxShowCommandBar.AutoSize = true;
            cbxShowCommandBar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxShowCommandBar.Location = new System.Drawing.Point(14, 28);
            cbxShowCommandBar.Margin = new System.Windows.Forms.Padding(4);
            cbxShowCommandBar.Name = "cbxShowCommandBar";
            cbxShowCommandBar.Size = new System.Drawing.Size(173, 25);
            cbxShowCommandBar.TabIndex = 12;
            cbxShowCommandBar.Text = "Show command bar";
            cbxShowCommandBar.UseVisualStyleBackColor = true;
            // 
            // cbxShowStatusBar
            // 
            cbxShowStatusBar.AutoSize = true;
            cbxShowStatusBar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxShowStatusBar.Location = new System.Drawing.Point(14, 94);
            cbxShowStatusBar.Margin = new System.Windows.Forms.Padding(4);
            cbxShowStatusBar.Name = "cbxShowStatusBar";
            cbxShowStatusBar.Size = new System.Drawing.Size(144, 25);
            cbxShowStatusBar.TabIndex = 16;
            cbxShowStatusBar.Text = "Show status bar";
            cbxShowStatusBar.UseVisualStyleBackColor = true;
            // 
            // cbxShowDirectoryTree
            // 
            cbxShowDirectoryTree.AutoSize = true;
            cbxShowDirectoryTree.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxShowDirectoryTree.Location = new System.Drawing.Point(14, 61);
            cbxShowDirectoryTree.Margin = new System.Windows.Forms.Padding(4);
            cbxShowDirectoryTree.Name = "cbxShowDirectoryTree";
            cbxShowDirectoryTree.Size = new System.Drawing.Size(169, 25);
            cbxShowDirectoryTree.TabIndex = 15;
            cbxShowDirectoryTree.Text = "Show directory tree";
            cbxShowDirectoryTree.UseVisualStyleBackColor = true;
            // 
            // cbxShowDeletedItems
            // 
            cbxShowDeletedItems.AutoSize = true;
            cbxShowDeletedItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxShowDeletedItems.Location = new System.Drawing.Point(259, 28);
            cbxShowDeletedItems.Margin = new System.Windows.Forms.Padding(4);
            cbxShowDeletedItems.Name = "cbxShowDeletedItems";
            cbxShowDeletedItems.Size = new System.Drawing.Size(183, 25);
            cbxShowDeletedItems.TabIndex = 17;
            cbxShowDeletedItems.Text = "Show deleted objects";
            cbxShowDeletedItems.UseVisualStyleBackColor = true;
            // 
            // cbxShowHiddenItems
            // 
            cbxShowHiddenItems.AutoSize = true;
            cbxShowHiddenItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxShowHiddenItems.Location = new System.Drawing.Point(259, 61);
            cbxShowHiddenItems.Margin = new System.Windows.Forms.Padding(4);
            cbxShowHiddenItems.Name = "cbxShowHiddenItems";
            cbxShowHiddenItems.Size = new System.Drawing.Size(178, 25);
            cbxShowHiddenItems.TabIndex = 14;
            cbxShowHiddenItems.Text = "Show hidden objects";
            cbxShowHiddenItems.UseVisualStyleBackColor = true;
            // 
            // tabExtraction
            // 
            tabExtraction.Controls.Add(gbxExtractionPreserve);
            tabExtraction.Controls.Add(gbxExtractionDefaults);
            tabExtraction.Location = new System.Drawing.Point(4, 29);
            tabExtraction.Margin = new System.Windows.Forms.Padding(4);
            tabExtraction.Name = "tabExtraction";
            tabExtraction.Padding = new System.Windows.Forms.Padding(4);
            tabExtraction.Size = new System.Drawing.Size(568, 569);
            tabExtraction.TabIndex = 3;
            tabExtraction.Text = "Extraction";
            tabExtraction.UseVisualStyleBackColor = true;
            // 
            // gbxExtractionPreserve
            // 
            gbxExtractionPreserve.Controls.Add(cbxPreserveDates);
            gbxExtractionPreserve.Controls.Add(cbxPreserveAttributes);
            gbxExtractionPreserve.Location = new System.Drawing.Point(8, 276);
            gbxExtractionPreserve.Margin = new System.Windows.Forms.Padding(4);
            gbxExtractionPreserve.Name = "gbxExtractionPreserve";
            gbxExtractionPreserve.Padding = new System.Windows.Forms.Padding(4);
            gbxExtractionPreserve.Size = new System.Drawing.Size(552, 100);
            gbxExtractionPreserve.TabIndex = 23;
            gbxExtractionPreserve.TabStop = false;
            gbxExtractionPreserve.Text = "Preserve information";
            // 
            // cbxPreserveDates
            // 
            cbxPreserveDates.AutoSize = true;
            cbxPreserveDates.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxPreserveDates.Location = new System.Drawing.Point(14, 28);
            cbxPreserveDates.Margin = new System.Windows.Forms.Padding(4);
            cbxPreserveDates.Name = "cbxPreserveDates";
            cbxPreserveDates.Size = new System.Drawing.Size(269, 25);
            cbxPreserveDates.TabIndex = 8;
            cbxPreserveDates.Text = "Preserve file dates when extracting";
            cbxPreserveDates.UseVisualStyleBackColor = true;
            // 
            // cbxPreserveAttributes
            // 
            cbxPreserveAttributes.AutoSize = true;
            cbxPreserveAttributes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxPreserveAttributes.Location = new System.Drawing.Point(14, 61);
            cbxPreserveAttributes.Margin = new System.Windows.Forms.Padding(4);
            cbxPreserveAttributes.Name = "cbxPreserveAttributes";
            cbxPreserveAttributes.Size = new System.Drawing.Size(296, 25);
            cbxPreserveAttributes.TabIndex = 9;
            cbxPreserveAttributes.Text = "Preserve file attributes when extracting";
            cbxPreserveAttributes.UseVisualStyleBackColor = true;
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
            gbxExtractionDefaults.Location = new System.Drawing.Point(8, 8);
            gbxExtractionDefaults.Margin = new System.Windows.Forms.Padding(4);
            gbxExtractionDefaults.Name = "gbxExtractionDefaults";
            gbxExtractionDefaults.Padding = new System.Windows.Forms.Padding(4);
            gbxExtractionDefaults.Size = new System.Drawing.Size(552, 260);
            gbxExtractionDefaults.TabIndex = 22;
            gbxExtractionDefaults.TabStop = false;
            gbxExtractionDefaults.Text = "Default options";
            // 
            // cbxExtractAsk
            // 
            cbxExtractAsk.AutoSize = true;
            cbxExtractAsk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxExtractAsk.Location = new System.Drawing.Point(14, 28);
            cbxExtractAsk.Margin = new System.Windows.Forms.Padding(4);
            cbxExtractAsk.Name = "cbxExtractAsk";
            cbxExtractAsk.Size = new System.Drawing.Size(415, 25);
            cbxExtractAsk.TabIndex = 0;
            cbxExtractAsk.Text = "Always ask for these extraction options before extracting";
            cbxExtractAsk.UseVisualStyleBackColor = true;
            cbxExtractAsk.CheckedChanged += cbxExtractAsk_CheckedChanged;
            // 
            // lblExtractPath
            // 
            lblExtractPath.AutoSize = true;
            lblExtractPath.Location = new System.Drawing.Point(10, 63);
            lblExtractPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblExtractPath.Name = "lblExtractPath";
            lblExtractPath.Size = new System.Drawing.Size(339, 20);
            lblExtractPath.TabIndex = 1;
            lblExtractPath.Text = "Extract selected item(s) to the following directory:";
            // 
            // txtExtractPath
            // 
            txtExtractPath.Location = new System.Drawing.Point(14, 85);
            txtExtractPath.Margin = new System.Windows.Forms.Padding(4);
            txtExtractPath.Name = "txtExtractPath";
            txtExtractPath.Size = new System.Drawing.Size(422, 27);
            txtExtractPath.TabIndex = 2;
            // 
            // cbxOpenDir
            // 
            cbxOpenDir.AutoSize = true;
            cbxOpenDir.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxOpenDir.Location = new System.Drawing.Point(14, 224);
            cbxOpenDir.Margin = new System.Windows.Forms.Padding(4);
            cbxOpenDir.Name = "cbxOpenDir";
            cbxOpenDir.Size = new System.Drawing.Size(322, 25);
            cbxOpenDir.TabIndex = 7;
            cbxOpenDir.Text = "Open destination directory after extraction";
            cbxOpenDir.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnBrowse.Location = new System.Drawing.Point(444, 82);
            btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new System.Drawing.Size(100, 32);
            btnBrowse.TabIndex = 3;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // rbnExtractPreserve
            // 
            rbnExtractPreserve.AutoSize = true;
            rbnExtractPreserve.FlatStyle = System.Windows.Forms.FlatStyle.System;
            rbnExtractPreserve.Location = new System.Drawing.Point(14, 191);
            rbnExtractPreserve.Margin = new System.Windows.Forms.Padding(4);
            rbnExtractPreserve.Name = "rbnExtractPreserve";
            rbnExtractPreserve.Size = new System.Drawing.Size(273, 25);
            rbnExtractPreserve.TabIndex = 6;
            rbnExtractPreserve.TabStop = true;
            rbnExtractPreserve.Text = "Preserve original directory structure";
            rbnExtractPreserve.UseVisualStyleBackColor = true;
            // 
            // rbnIgnoreFolders
            // 
            rbnIgnoreFolders.AutoSize = true;
            rbnIgnoreFolders.FlatStyle = System.Windows.Forms.FlatStyle.System;
            rbnIgnoreFolders.Location = new System.Drawing.Point(14, 129);
            rbnIgnoreFolders.Margin = new System.Windows.Forms.Padding(4);
            rbnIgnoreFolders.Name = "rbnIgnoreFolders";
            rbnIgnoreFolders.Size = new System.Drawing.Size(282, 25);
            rbnIgnoreFolders.TabIndex = 4;
            rbnIgnoreFolders.TabStop = true;
            rbnIgnoreFolders.Text = "Ignore directories and subdirectories";
            rbnIgnoreFolders.UseVisualStyleBackColor = true;
            // 
            // rbnExtractFlat
            // 
            rbnExtractFlat.AutoSize = true;
            rbnExtractFlat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            rbnExtractFlat.Location = new System.Drawing.Point(14, 160);
            rbnExtractFlat.Margin = new System.Windows.Forms.Padding(4);
            rbnExtractFlat.Name = "rbnExtractFlat";
            rbnExtractFlat.Size = new System.Drawing.Size(292, 25);
            rbnExtractFlat.TabIndex = 5;
            rbnExtractFlat.TabStop = true;
            rbnExtractFlat.Text = "Extract all files into the same directory";
            rbnExtractFlat.UseVisualStyleBackColor = true;
            // 
            // tabIntegration
            // 
            tabIntegration.Controls.Add(gbxFileAssociations);
            tabIntegration.Controls.Add(gbxIntegrationMisc);
            tabIntegration.Location = new System.Drawing.Point(4, 29);
            tabIntegration.Margin = new System.Windows.Forms.Padding(4);
            tabIntegration.Name = "tabIntegration";
            tabIntegration.Padding = new System.Windows.Forms.Padding(4);
            tabIntegration.Size = new System.Drawing.Size(568, 569);
            tabIntegration.TabIndex = 1;
            tabIntegration.Text = "Integration";
            tabIntegration.UseVisualStyleBackColor = true;
            // 
            // gbxFileAssociations
            // 
            gbxFileAssociations.Controls.Add(lblFileAssoc);
            gbxFileAssociations.Controls.Add(btnFileAssoc);
            gbxFileAssociations.Location = new System.Drawing.Point(8, 8);
            gbxFileAssociations.Margin = new System.Windows.Forms.Padding(4);
            gbxFileAssociations.Name = "gbxFileAssociations";
            gbxFileAssociations.Padding = new System.Windows.Forms.Padding(4);
            gbxFileAssociations.Size = new System.Drawing.Size(552, 85);
            gbxFileAssociations.TabIndex = 8;
            gbxFileAssociations.TabStop = false;
            gbxFileAssociations.Text = "File associations";
            // 
            // lblFileAssoc
            // 
            lblFileAssoc.AutoSize = true;
            lblFileAssoc.Location = new System.Drawing.Point(11, 24);
            lblFileAssoc.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblFileAssoc.Name = "lblFileAssoc";
            lblFileAssoc.Size = new System.Drawing.Size(389, 40);
            lblFileAssoc.TabIndex = 4;
            lblFileAssoc.Text = "File associations for TotalImage can be managed through\r\nControl Panel/Windows Settings.";
            // 
            // btnFileAssoc
            // 
            btnFileAssoc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnFileAssoc.Location = new System.Drawing.Point(446, 26);
            btnFileAssoc.Margin = new System.Windows.Forms.Padding(2);
            btnFileAssoc.Name = "btnFileAssoc";
            btnFileAssoc.Size = new System.Drawing.Size(100, 32);
            btnFileAssoc.TabIndex = 5;
            btnFileAssoc.Text = "Open";
            btnFileAssoc.UseVisualStyleBackColor = true;
            btnFileAssoc.Click += btnFileAssoc_Click;
            // 
            // gbxIntegrationMisc
            // 
            gbxIntegrationMisc.Controls.Add(lblSystemIcons);
            gbxIntegrationMisc.Controls.Add(cbxShellFileIcons);
            gbxIntegrationMisc.Location = new System.Drawing.Point(8, 99);
            gbxIntegrationMisc.Margin = new System.Windows.Forms.Padding(2);
            gbxIntegrationMisc.Name = "gbxIntegrationMisc";
            gbxIntegrationMisc.Padding = new System.Windows.Forms.Padding(2);
            gbxIntegrationMisc.Size = new System.Drawing.Size(552, 150);
            gbxIntegrationMisc.TabIndex = 7;
            gbxIntegrationMisc.TabStop = false;
            gbxIntegrationMisc.Text = "Other options";
            // 
            // lblSystemIcons
            // 
            lblSystemIcons.AutoSize = true;
            lblSystemIcons.Location = new System.Drawing.Point(11, 64);
            lblSystemIcons.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblSystemIcons.Name = "lblSystemIcons";
            lblSystemIcons.Size = new System.Drawing.Size(522, 60);
            lblSystemIcons.TabIndex = 6;
            lblSystemIcons.Text = "If this option is enabled, TotalImage will obtain icons and names for file types\r\nfrom Windows, which can be slower in some situations. If disabled, generic\r\nicons and names will be used instead.";
            // 
            // cbxShellFileIcons
            // 
            cbxShellFileIcons.AutoSize = true;
            cbxShellFileIcons.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxShellFileIcons.Location = new System.Drawing.Point(14, 28);
            cbxShellFileIcons.Margin = new System.Windows.Forms.Padding(4);
            cbxShellFileIcons.Name = "cbxShellFileIcons";
            cbxShellFileIcons.Size = new System.Drawing.Size(374, 25);
            cbxShellFileIcons.TabIndex = 3;
            cbxShellFileIcons.Text = "Display system icons and file type names in file list";
            cbxShellFileIcons.UseVisualStyleBackColor = true;
            // 
            // dlgSettings
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.Color.White;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(606, 688);
            Controls.Add(tabs);
            Controls.Add(pnlBottom);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Margin = new System.Windows.Forms.Padding(4);
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
        private System.Windows.Forms.CheckBox cbxShowDeletedItems;
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
