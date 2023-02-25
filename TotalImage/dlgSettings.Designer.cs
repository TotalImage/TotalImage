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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.gbxMisc = new System.Windows.Forms.GroupBox();
            this.lblClearTemp = new System.Windows.Forms.Label();
            this.lblClearRecent = new System.Windows.Forms.Label();
            this.lblResetSettings = new System.Windows.Forms.Label();
            this.btnClearTemp = new System.Windows.Forms.Button();
            this.btnClearRecent = new System.Windows.Forms.Button();
            this.btnResetSettings = new System.Windows.Forms.Button();
            this.gbxBehavior = new System.Windows.Forms.GroupBox();
            this.lblMemoryMapping1 = new System.Windows.Forms.Label();
            this.txtMemoryMapping = new System.Windows.Forms.NumericUpDown();
            this.lblMemoryMapping = new System.Windows.Forms.Label();
            this.cbxAutoincrementFilename = new System.Windows.Forms.CheckBox();
            this.cbxConfirmOverwriteExtract = new System.Windows.Forms.CheckBox();
            this.cbxConfirmDeletion = new System.Windows.Forms.CheckBox();
            this.cbxConfirmInjection = new System.Windows.Forms.CheckBox();
            this.tabView = new System.Windows.Forms.TabPage();
            this.gbxSizeUnits = new System.Windows.Forms.GroupBox();
            this.lblSizeUnitsTip = new System.Windows.Forms.Label();
            this.lblSizeUnitsPreview = new System.Windows.Forms.Label();
            this.lblSizeUnits = new System.Windows.Forms.Label();
            this.lstSizeUnits = new System.Windows.Forms.ComboBox();
            this.gbxFileList = new System.Windows.Forms.GroupBox();
            this.cbxShowDirSizes = new System.Windows.Forms.CheckBox();
            this.lblViewType = new System.Windows.Forms.Label();
            this.lstSortOrder = new System.Windows.Forms.ComboBox();
            this.lstViewType = new System.Windows.Forms.ComboBox();
            this.lblSortOrder = new System.Windows.Forms.Label();
            this.lblSortBy = new System.Windows.Forms.Label();
            this.lstSortBy = new System.Windows.Forms.ComboBox();
            this.gbxMainWindow = new System.Windows.Forms.GroupBox();
            this.cbxShowCommandBar = new System.Windows.Forms.CheckBox();
            this.cbxShowStatusBar = new System.Windows.Forms.CheckBox();
            this.cbxShowDirectoryTree = new System.Windows.Forms.CheckBox();
            this.cbxShowDeletedItems = new System.Windows.Forms.CheckBox();
            this.cbxShowHiddenItems = new System.Windows.Forms.CheckBox();
            this.tabExtraction = new System.Windows.Forms.TabPage();
            this.cbxPreserveAttributes = new System.Windows.Forms.CheckBox();
            this.cbxPreserveDates = new System.Windows.Forms.CheckBox();
            this.cbxOpenDir = new System.Windows.Forms.CheckBox();
            this.rbnExtractPreserve = new System.Windows.Forms.RadioButton();
            this.rbnExtractFlat = new System.Windows.Forms.RadioButton();
            this.rbnIgnoreFolders = new System.Windows.Forms.RadioButton();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtExtractPath = new System.Windows.Forms.TextBox();
            this.lblExtractPath = new System.Windows.Forms.Label();
            this.cbxExtractAsk = new System.Windows.Forms.CheckBox();
            this.tabIntegration = new System.Windows.Forms.TabPage();
            this.gbxFileAssociations = new System.Windows.Forms.GroupBox();
            this.lblFileAssoc = new System.Windows.Forms.Label();
            this.btnFileAssoc = new System.Windows.Forms.Button();
            this.gbxIntegrationMisc = new System.Windows.Forms.GroupBox();
            this.lblSystemIcons = new System.Windows.Forms.Label();
            this.cbxShellFileIcons = new System.Windows.Forms.CheckBox();
            this.gbxExtraction = new System.Windows.Forms.GroupBox();
            this.pnlBottom.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.gbxMisc.SuspendLayout();
            this.gbxBehavior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemoryMapping)).BeginInit();
            this.tabView.SuspendLayout();
            this.gbxSizeUnits.SuspendLayout();
            this.gbxFileList.SuspendLayout();
            this.gbxMainWindow.SuspendLayout();
            this.tabExtraction.SuspendLayout();
            this.tabIntegration.SuspendLayout();
            this.gbxFileAssociations.SuspendLayout();
            this.gbxIntegrationMisc.SuspendLayout();
            this.gbxExtraction.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(491, 15);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 32);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(384, 15);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 32);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 626);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(606, 62);
            this.pnlBottom.TabIndex = 2;
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabGeneral);
            this.tabs.Controls.Add(this.tabView);
            this.tabs.Controls.Add(this.tabExtraction);
            this.tabs.Controls.Add(this.tabIntegration);
            this.tabs.Location = new System.Drawing.Point(15, 16);
            this.tabs.Margin = new System.Windows.Forms.Padding(4);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(576, 602);
            this.tabs.TabIndex = 3;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.gbxMisc);
            this.tabGeneral.Controls.Add(this.gbxBehavior);
            this.tabGeneral.Location = new System.Drawing.Point(4, 29);
            this.tabGeneral.Margin = new System.Windows.Forms.Padding(4);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(4);
            this.tabGeneral.Size = new System.Drawing.Size(568, 569);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // gbxMisc
            // 
            this.gbxMisc.Controls.Add(this.lblClearTemp);
            this.gbxMisc.Controls.Add(this.lblClearRecent);
            this.gbxMisc.Controls.Add(this.lblResetSettings);
            this.gbxMisc.Controls.Add(this.btnClearTemp);
            this.gbxMisc.Controls.Add(this.btnClearRecent);
            this.gbxMisc.Controls.Add(this.btnResetSettings);
            this.gbxMisc.Location = new System.Drawing.Point(8, 216);
            this.gbxMisc.Margin = new System.Windows.Forms.Padding(4);
            this.gbxMisc.Name = "gbxMisc";
            this.gbxMisc.Padding = new System.Windows.Forms.Padding(4);
            this.gbxMisc.Size = new System.Drawing.Size(552, 162);
            this.gbxMisc.TabIndex = 1;
            this.gbxMisc.TabStop = false;
            this.gbxMisc.Text = "Advanced options";
            // 
            // lblClearTemp
            // 
            this.lblClearTemp.AutoSize = true;
            this.lblClearTemp.Location = new System.Drawing.Point(11, 120);
            this.lblClearTemp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblClearTemp.Name = "lblClearTemp";
            this.lblClearTemp.Size = new System.Drawing.Size(416, 20);
            this.lblClearTemp.TabIndex = 9;
            this.lblClearTemp.Text = "Clear TotalImage\'s temporary folder in local application data.";
            // 
            // lblClearRecent
            // 
            this.lblClearRecent.AutoSize = true;
            this.lblClearRecent.Location = new System.Drawing.Point(11, 76);
            this.lblClearRecent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblClearRecent.Name = "lblClearRecent";
            this.lblClearRecent.Size = new System.Drawing.Size(191, 20);
            this.lblClearRecent.TabIndex = 8;
            this.lblClearRecent.Text = "Clear the recent images list.";
            // 
            // lblResetSettings
            // 
            this.lblResetSettings.AutoSize = true;
            this.lblResetSettings.Location = new System.Drawing.Point(11, 30);
            this.lblResetSettings.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblResetSettings.Name = "lblResetSettings";
            this.lblResetSettings.Size = new System.Drawing.Size(350, 20);
            this.lblResetSettings.TabIndex = 7;
            this.lblResetSettings.Text = "Reset all TotalImage settings to their default values.";
            // 
            // btnClearTemp
            // 
            this.btnClearTemp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClearTemp.Location = new System.Drawing.Point(444, 114);
            this.btnClearTemp.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearTemp.Name = "btnClearTemp";
            this.btnClearTemp.Size = new System.Drawing.Size(100, 32);
            this.btnClearTemp.TabIndex = 6;
            this.btnClearTemp.Text = "Clear";
            this.btnClearTemp.UseVisualStyleBackColor = true;
            this.btnClearTemp.Click += new System.EventHandler(this.btnClearTemp_Click);
            // 
            // btnClearRecent
            // 
            this.btnClearRecent.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClearRecent.Location = new System.Drawing.Point(444, 70);
            this.btnClearRecent.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearRecent.Name = "btnClearRecent";
            this.btnClearRecent.Size = new System.Drawing.Size(100, 32);
            this.btnClearRecent.TabIndex = 5;
            this.btnClearRecent.Text = "Clear";
            this.btnClearRecent.UseVisualStyleBackColor = true;
            this.btnClearRecent.Click += new System.EventHandler(this.btnClearRecent_Click);
            // 
            // btnResetSettings
            // 
            this.btnResetSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnResetSettings.Location = new System.Drawing.Point(444, 24);
            this.btnResetSettings.Margin = new System.Windows.Forms.Padding(4);
            this.btnResetSettings.Name = "btnResetSettings";
            this.btnResetSettings.Size = new System.Drawing.Size(100, 32);
            this.btnResetSettings.TabIndex = 4;
            this.btnResetSettings.Text = "Reset";
            this.btnResetSettings.UseVisualStyleBackColor = true;
            this.btnResetSettings.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // gbxBehavior
            // 
            this.gbxBehavior.Controls.Add(this.lblMemoryMapping1);
            this.gbxBehavior.Controls.Add(this.txtMemoryMapping);
            this.gbxBehavior.Controls.Add(this.lblMemoryMapping);
            this.gbxBehavior.Controls.Add(this.cbxAutoincrementFilename);
            this.gbxBehavior.Controls.Add(this.cbxConfirmOverwriteExtract);
            this.gbxBehavior.Controls.Add(this.cbxConfirmDeletion);
            this.gbxBehavior.Controls.Add(this.cbxConfirmInjection);
            this.gbxBehavior.Location = new System.Drawing.Point(8, 8);
            this.gbxBehavior.Margin = new System.Windows.Forms.Padding(4);
            this.gbxBehavior.Name = "gbxBehavior";
            this.gbxBehavior.Padding = new System.Windows.Forms.Padding(4);
            this.gbxBehavior.Size = new System.Drawing.Size(552, 200);
            this.gbxBehavior.TabIndex = 0;
            this.gbxBehavior.TabStop = false;
            this.gbxBehavior.Text = "Behavior";
            // 
            // lblMemoryMapping1
            // 
            this.lblMemoryMapping1.AutoSize = true;
            this.lblMemoryMapping1.Enabled = false;
            this.lblMemoryMapping1.Location = new System.Drawing.Point(436, 163);
            this.lblMemoryMapping1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMemoryMapping1.Name = "lblMemoryMapping1";
            this.lblMemoryMapping1.Size = new System.Drawing.Size(35, 20);
            this.lblMemoryMapping1.TabIndex = 21;
            this.lblMemoryMapping1.Text = "MiB";
            // 
            // txtMemoryMapping
            // 
            this.txtMemoryMapping.Enabled = false;
            this.txtMemoryMapping.Location = new System.Drawing.Point(303, 161);
            this.txtMemoryMapping.Margin = new System.Windows.Forms.Padding(4);
            this.txtMemoryMapping.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.txtMemoryMapping.Name = "txtMemoryMapping";
            this.txtMemoryMapping.Size = new System.Drawing.Size(125, 27);
            this.txtMemoryMapping.TabIndex = 20;
            this.txtMemoryMapping.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblMemoryMapping
            // 
            this.lblMemoryMapping.AutoSize = true;
            this.lblMemoryMapping.Enabled = false;
            this.lblMemoryMapping.Location = new System.Drawing.Point(11, 163);
            this.lblMemoryMapping.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMemoryMapping.Name = "lblMemoryMapping";
            this.lblMemoryMapping.Size = new System.Drawing.Size(284, 20);
            this.lblMemoryMapping.TabIndex = 19;
            this.lblMemoryMapping.Text = "Threshold for mapping files into memory:";
            // 
            // cbxAutoincrementFilename
            // 
            this.cbxAutoincrementFilename.AutoSize = true;
            this.cbxAutoincrementFilename.Enabled = false;
            this.cbxAutoincrementFilename.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxAutoincrementFilename.Location = new System.Drawing.Point(14, 127);
            this.cbxAutoincrementFilename.Margin = new System.Windows.Forms.Padding(4);
            this.cbxAutoincrementFilename.Name = "cbxAutoincrementFilename";
            this.cbxAutoincrementFilename.Size = new System.Drawing.Size(420, 25);
            this.cbxAutoincrementFilename.TabIndex = 18;
            this.cbxAutoincrementFilename.Text = "When saving, auto-increment last filename when possible";
            this.cbxAutoincrementFilename.UseVisualStyleBackColor = true;
            // 
            // cbxConfirmOverwriteExtract
            // 
            this.cbxConfirmOverwriteExtract.AutoSize = true;
            this.cbxConfirmOverwriteExtract.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxConfirmOverwriteExtract.Location = new System.Drawing.Point(14, 28);
            this.cbxConfirmOverwriteExtract.Margin = new System.Windows.Forms.Padding(4);
            this.cbxConfirmOverwriteExtract.Name = "cbxConfirmOverwriteExtract";
            this.cbxConfirmOverwriteExtract.Size = new System.Drawing.Size(276, 25);
            this.cbxConfirmOverwriteExtract.TabIndex = 17;
            this.cbxConfirmOverwriteExtract.Text = "Confirm overwrite during extraction";
            this.cbxConfirmOverwriteExtract.UseVisualStyleBackColor = true;
            // 
            // cbxConfirmDeletion
            // 
            this.cbxConfirmDeletion.AutoSize = true;
            this.cbxConfirmDeletion.Enabled = false;
            this.cbxConfirmDeletion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxConfirmDeletion.Location = new System.Drawing.Point(14, 61);
            this.cbxConfirmDeletion.Margin = new System.Windows.Forms.Padding(4);
            this.cbxConfirmDeletion.Name = "cbxConfirmDeletion";
            this.cbxConfirmDeletion.Size = new System.Drawing.Size(152, 25);
            this.cbxConfirmDeletion.TabIndex = 16;
            this.cbxConfirmDeletion.Text = "Confirm deletion";
            this.cbxConfirmDeletion.UseVisualStyleBackColor = true;
            // 
            // cbxConfirmInjection
            // 
            this.cbxConfirmInjection.AutoSize = true;
            this.cbxConfirmInjection.Enabled = false;
            this.cbxConfirmInjection.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxConfirmInjection.Location = new System.Drawing.Point(14, 94);
            this.cbxConfirmInjection.Margin = new System.Windows.Forms.Padding(4);
            this.cbxConfirmInjection.Name = "cbxConfirmInjection";
            this.cbxConfirmInjection.Size = new System.Drawing.Size(154, 25);
            this.cbxConfirmInjection.TabIndex = 15;
            this.cbxConfirmInjection.Text = "Confirm injection";
            this.cbxConfirmInjection.UseVisualStyleBackColor = true;
            // 
            // tabView
            // 
            this.tabView.Controls.Add(this.gbxSizeUnits);
            this.tabView.Controls.Add(this.gbxFileList);
            this.tabView.Controls.Add(this.gbxMainWindow);
            this.tabView.Location = new System.Drawing.Point(4, 29);
            this.tabView.Margin = new System.Windows.Forms.Padding(4);
            this.tabView.Name = "tabView";
            this.tabView.Padding = new System.Windows.Forms.Padding(4);
            this.tabView.Size = new System.Drawing.Size(568, 569);
            this.tabView.TabIndex = 2;
            this.tabView.Text = "View";
            this.tabView.UseVisualStyleBackColor = true;
            // 
            // gbxSizeUnits
            // 
            this.gbxSizeUnits.Controls.Add(this.lblSizeUnitsTip);
            this.gbxSizeUnits.Controls.Add(this.lblSizeUnitsPreview);
            this.gbxSizeUnits.Controls.Add(this.lblSizeUnits);
            this.gbxSizeUnits.Controls.Add(this.lstSizeUnits);
            this.gbxSizeUnits.Location = new System.Drawing.Point(8, 314);
            this.gbxSizeUnits.Margin = new System.Windows.Forms.Padding(4);
            this.gbxSizeUnits.Name = "gbxSizeUnits";
            this.gbxSizeUnits.Padding = new System.Windows.Forms.Padding(4);
            this.gbxSizeUnits.Size = new System.Drawing.Size(552, 145);
            this.gbxSizeUnits.TabIndex = 23;
            this.gbxSizeUnits.TabStop = false;
            this.gbxSizeUnits.Text = "Size units";
            // 
            // lblSizeUnitsTip
            // 
            this.lblSizeUnitsTip.AutoSize = true;
            this.lblSizeUnitsTip.Location = new System.Drawing.Point(256, 24);
            this.lblSizeUnitsTip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSizeUnitsTip.Name = "lblSizeUnitsTip";
            this.lblSizeUnitsTip.Size = new System.Drawing.Size(283, 60);
            this.lblSizeUnitsTip.TabIndex = 22;
            this.lblSizeUnitsTip.Text = "For binary and decimal, TotalImage will\r\nautomatically determine the most\r\napprop" +
    "riate unit and convert accordingly.";
            // 
            // lblSizeUnitsPreview
            // 
            this.lblSizeUnitsPreview.AutoSize = true;
            this.lblSizeUnitsPreview.Location = new System.Drawing.Point(14, 87);
            this.lblSizeUnitsPreview.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSizeUnitsPreview.Name = "lblSizeUnitsPreview";
            this.lblSizeUnitsPreview.Size = new System.Drawing.Size(382, 40);
            this.lblSizeUnitsPreview.TabIndex = 21;
            this.lblSizeUnitsPreview.Text = "Preview:\r\n234 B, 2345 B, 234567 B, 234567890 B, 2345678901234 B";
            // 
            // lblSizeUnits
            // 
            this.lblSizeUnits.AutoSize = true;
            this.lblSizeUnits.Location = new System.Drawing.Point(11, 24);
            this.lblSizeUnits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSizeUnits.Name = "lblSizeUnits";
            this.lblSizeUnits.Size = new System.Drawing.Size(101, 20);
            this.lblSizeUnits.TabIndex = 10;
            this.lblSizeUnits.Text = "Show sizes as:";
            // 
            // lstSizeUnits
            // 
            this.lstSizeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstSizeUnits.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstSizeUnits.FormattingEnabled = true;
            this.lstSizeUnits.Items.AddRange(new object[] {
            "Bytes",
            "Decimal units (factor 1000)",
            "Binary units (factor 1024)"});
            this.lstSizeUnits.Location = new System.Drawing.Point(15, 46);
            this.lstSizeUnits.Margin = new System.Windows.Forms.Padding(4);
            this.lstSizeUnits.Name = "lstSizeUnits";
            this.lstSizeUnits.Size = new System.Drawing.Size(212, 28);
            this.lstSizeUnits.TabIndex = 11;
            this.lstSizeUnits.SelectedIndexChanged += new System.EventHandler(this.lstSizeUnits_SelectedIndexChanged);
            // 
            // gbxFileList
            // 
            this.gbxFileList.Controls.Add(this.cbxShowDirSizes);
            this.gbxFileList.Controls.Add(this.lblViewType);
            this.gbxFileList.Controls.Add(this.lstSortOrder);
            this.gbxFileList.Controls.Add(this.lstViewType);
            this.gbxFileList.Controls.Add(this.lblSortOrder);
            this.gbxFileList.Controls.Add(this.lblSortBy);
            this.gbxFileList.Controls.Add(this.lstSortBy);
            this.gbxFileList.Location = new System.Drawing.Point(8, 149);
            this.gbxFileList.Margin = new System.Windows.Forms.Padding(4);
            this.gbxFileList.Name = "gbxFileList";
            this.gbxFileList.Padding = new System.Windows.Forms.Padding(4);
            this.gbxFileList.Size = new System.Drawing.Size(552, 157);
            this.gbxFileList.TabIndex = 22;
            this.gbxFileList.TabStop = false;
            this.gbxFileList.Text = "File list";
            // 
            // cbxShowDirSizes
            // 
            this.cbxShowDirSizes.AutoSize = true;
            this.cbxShowDirSizes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowDirSizes.Location = new System.Drawing.Point(259, 110);
            this.cbxShowDirSizes.Margin = new System.Windows.Forms.Padding(4);
            this.cbxShowDirSizes.Name = "cbxShowDirSizes";
            this.cbxShowDirSizes.Size = new System.Drawing.Size(238, 25);
            this.cbxShowDirSizes.TabIndex = 20;
            this.cbxShowDirSizes.Text = "Show directory sizes in file list";
            this.cbxShowDirSizes.UseVisualStyleBackColor = true;
            // 
            // lblViewType
            // 
            this.lblViewType.AutoSize = true;
            this.lblViewType.Location = new System.Drawing.Point(11, 24);
            this.lblViewType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblViewType.Name = "lblViewType";
            this.lblViewType.Size = new System.Drawing.Size(183, 20);
            this.lblViewType.TabIndex = 6;
            this.lblViewType.Text = "Display items in file list as:";
            // 
            // lstSortOrder
            // 
            this.lstSortOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstSortOrder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstSortOrder.FormattingEnabled = true;
            this.lstSortOrder.Items.AddRange(new object[] {
            "Ascending",
            "Descending"});
            this.lstSortOrder.Location = new System.Drawing.Point(15, 110);
            this.lstSortOrder.Margin = new System.Windows.Forms.Padding(4);
            this.lstSortOrder.Name = "lstSortOrder";
            this.lstSortOrder.Size = new System.Drawing.Size(212, 28);
            this.lstSortOrder.TabIndex = 19;
            // 
            // lstViewType
            // 
            this.lstViewType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstViewType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstViewType.FormattingEnabled = true;
            this.lstViewType.Items.AddRange(new object[] {
            "Large icons",
            "Small icons",
            "List",
            "Details"});
            this.lstViewType.Location = new System.Drawing.Point(14, 46);
            this.lstViewType.Margin = new System.Windows.Forms.Padding(4);
            this.lstViewType.Name = "lstViewType";
            this.lstViewType.Size = new System.Drawing.Size(212, 28);
            this.lstViewType.TabIndex = 7;
            // 
            // lblSortOrder
            // 
            this.lblSortOrder.AutoSize = true;
            this.lblSortOrder.Location = new System.Drawing.Point(11, 87);
            this.lblSortOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSortOrder.Name = "lblSortOrder";
            this.lblSortOrder.Size = new System.Drawing.Size(171, 20);
            this.lblSortOrder.TabIndex = 18;
            this.lblSortOrder.Text = "Sorting order for file list:";
            // 
            // lblSortBy
            // 
            this.lblSortBy.AutoSize = true;
            this.lblSortBy.Location = new System.Drawing.Point(256, 24);
            this.lblSortBy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSortBy.Name = "lblSortBy";
            this.lblSortBy.Size = new System.Drawing.Size(163, 20);
            this.lblSortBy.TabIndex = 8;
            this.lblSortBy.Text = "Sort items in file list by:";
            // 
            // lstSortBy
            // 
            this.lstSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstSortBy.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstSortBy.FormattingEnabled = true;
            this.lstSortBy.Items.AddRange(new object[] {
            "Name",
            "Type",
            "Size",
            "Modified",
            "Attributes"});
            this.lstSortBy.Location = new System.Drawing.Point(259, 47);
            this.lstSortBy.Margin = new System.Windows.Forms.Padding(4);
            this.lstSortBy.Name = "lstSortBy";
            this.lstSortBy.Size = new System.Drawing.Size(212, 28);
            this.lstSortBy.TabIndex = 9;
            // 
            // gbxMainWindow
            // 
            this.gbxMainWindow.Controls.Add(this.cbxShowCommandBar);
            this.gbxMainWindow.Controls.Add(this.cbxShowStatusBar);
            this.gbxMainWindow.Controls.Add(this.cbxShowDirectoryTree);
            this.gbxMainWindow.Controls.Add(this.cbxShowDeletedItems);
            this.gbxMainWindow.Controls.Add(this.cbxShowHiddenItems);
            this.gbxMainWindow.Location = new System.Drawing.Point(8, 8);
            this.gbxMainWindow.Margin = new System.Windows.Forms.Padding(4);
            this.gbxMainWindow.Name = "gbxMainWindow";
            this.gbxMainWindow.Padding = new System.Windows.Forms.Padding(4);
            this.gbxMainWindow.Size = new System.Drawing.Size(552, 133);
            this.gbxMainWindow.TabIndex = 21;
            this.gbxMainWindow.TabStop = false;
            this.gbxMainWindow.Text = "Main window";
            // 
            // cbxShowCommandBar
            // 
            this.cbxShowCommandBar.AutoSize = true;
            this.cbxShowCommandBar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowCommandBar.Location = new System.Drawing.Point(14, 28);
            this.cbxShowCommandBar.Margin = new System.Windows.Forms.Padding(4);
            this.cbxShowCommandBar.Name = "cbxShowCommandBar";
            this.cbxShowCommandBar.Size = new System.Drawing.Size(173, 25);
            this.cbxShowCommandBar.TabIndex = 12;
            this.cbxShowCommandBar.Text = "Show command bar";
            this.cbxShowCommandBar.UseVisualStyleBackColor = true;
            // 
            // cbxShowStatusBar
            // 
            this.cbxShowStatusBar.AutoSize = true;
            this.cbxShowStatusBar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowStatusBar.Location = new System.Drawing.Point(14, 94);
            this.cbxShowStatusBar.Margin = new System.Windows.Forms.Padding(4);
            this.cbxShowStatusBar.Name = "cbxShowStatusBar";
            this.cbxShowStatusBar.Size = new System.Drawing.Size(144, 25);
            this.cbxShowStatusBar.TabIndex = 16;
            this.cbxShowStatusBar.Text = "Show status bar";
            this.cbxShowStatusBar.UseVisualStyleBackColor = true;
            // 
            // cbxShowDirectoryTree
            // 
            this.cbxShowDirectoryTree.AutoSize = true;
            this.cbxShowDirectoryTree.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowDirectoryTree.Location = new System.Drawing.Point(14, 61);
            this.cbxShowDirectoryTree.Margin = new System.Windows.Forms.Padding(4);
            this.cbxShowDirectoryTree.Name = "cbxShowDirectoryTree";
            this.cbxShowDirectoryTree.Size = new System.Drawing.Size(169, 25);
            this.cbxShowDirectoryTree.TabIndex = 15;
            this.cbxShowDirectoryTree.Text = "Show directory tree";
            this.cbxShowDirectoryTree.UseVisualStyleBackColor = true;
            // 
            // cbxShowDeletedItems
            // 
            this.cbxShowDeletedItems.AutoSize = true;
            this.cbxShowDeletedItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowDeletedItems.Location = new System.Drawing.Point(259, 28);
            this.cbxShowDeletedItems.Margin = new System.Windows.Forms.Padding(4);
            this.cbxShowDeletedItems.Name = "cbxShowDeletedItems";
            this.cbxShowDeletedItems.Size = new System.Drawing.Size(183, 25);
            this.cbxShowDeletedItems.TabIndex = 17;
            this.cbxShowDeletedItems.Text = "Show deleted objects";
            this.cbxShowDeletedItems.UseVisualStyleBackColor = true;
            // 
            // cbxShowHiddenItems
            // 
            this.cbxShowHiddenItems.AutoSize = true;
            this.cbxShowHiddenItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowHiddenItems.Location = new System.Drawing.Point(259, 61);
            this.cbxShowHiddenItems.Margin = new System.Windows.Forms.Padding(4);
            this.cbxShowHiddenItems.Name = "cbxShowHiddenItems";
            this.cbxShowHiddenItems.Size = new System.Drawing.Size(178, 25);
            this.cbxShowHiddenItems.TabIndex = 14;
            this.cbxShowHiddenItems.Text = "Show hidden objects";
            this.cbxShowHiddenItems.UseVisualStyleBackColor = true;
            // 
            // tabExtraction
            // 
            this.tabExtraction.Controls.Add(this.gbxExtraction);
            this.tabExtraction.Location = new System.Drawing.Point(4, 29);
            this.tabExtraction.Margin = new System.Windows.Forms.Padding(4);
            this.tabExtraction.Name = "tabExtraction";
            this.tabExtraction.Padding = new System.Windows.Forms.Padding(4);
            this.tabExtraction.Size = new System.Drawing.Size(568, 569);
            this.tabExtraction.TabIndex = 3;
            this.tabExtraction.Text = "Extraction";
            this.tabExtraction.UseVisualStyleBackColor = true;
            // 
            // cbxPreserveAttributes
            // 
            this.cbxPreserveAttributes.AutoSize = true;
            this.cbxPreserveAttributes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxPreserveAttributes.Location = new System.Drawing.Point(14, 299);
            this.cbxPreserveAttributes.Margin = new System.Windows.Forms.Padding(4);
            this.cbxPreserveAttributes.Name = "cbxPreserveAttributes";
            this.cbxPreserveAttributes.Size = new System.Drawing.Size(296, 25);
            this.cbxPreserveAttributes.TabIndex = 9;
            this.cbxPreserveAttributes.Text = "Preserve file attributes when extracting";
            this.cbxPreserveAttributes.UseVisualStyleBackColor = true;
            // 
            // cbxPreserveDates
            // 
            this.cbxPreserveDates.AutoSize = true;
            this.cbxPreserveDates.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxPreserveDates.Location = new System.Drawing.Point(14, 267);
            this.cbxPreserveDates.Margin = new System.Windows.Forms.Padding(4);
            this.cbxPreserveDates.Name = "cbxPreserveDates";
            this.cbxPreserveDates.Size = new System.Drawing.Size(269, 25);
            this.cbxPreserveDates.TabIndex = 8;
            this.cbxPreserveDates.Text = "Preserve file dates when extracting";
            this.cbxPreserveDates.UseVisualStyleBackColor = true;
            // 
            // cbxOpenDir
            // 
            this.cbxOpenDir.AutoSize = true;
            this.cbxOpenDir.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxOpenDir.Location = new System.Drawing.Point(14, 234);
            this.cbxOpenDir.Margin = new System.Windows.Forms.Padding(4);
            this.cbxOpenDir.Name = "cbxOpenDir";
            this.cbxOpenDir.Size = new System.Drawing.Size(322, 25);
            this.cbxOpenDir.TabIndex = 7;
            this.cbxOpenDir.Text = "Open destination directory after extraction";
            this.cbxOpenDir.UseVisualStyleBackColor = true;
            // 
            // rbnExtractPreserve
            // 
            this.rbnExtractPreserve.AutoSize = true;
            this.rbnExtractPreserve.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnExtractPreserve.Location = new System.Drawing.Point(14, 191);
            this.rbnExtractPreserve.Margin = new System.Windows.Forms.Padding(4);
            this.rbnExtractPreserve.Name = "rbnExtractPreserve";
            this.rbnExtractPreserve.Size = new System.Drawing.Size(273, 25);
            this.rbnExtractPreserve.TabIndex = 6;
            this.rbnExtractPreserve.TabStop = true;
            this.rbnExtractPreserve.Text = "Preserve original directory structure";
            this.rbnExtractPreserve.UseVisualStyleBackColor = true;
            // 
            // rbnExtractFlat
            // 
            this.rbnExtractFlat.AutoSize = true;
            this.rbnExtractFlat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnExtractFlat.Location = new System.Drawing.Point(14, 160);
            this.rbnExtractFlat.Margin = new System.Windows.Forms.Padding(4);
            this.rbnExtractFlat.Name = "rbnExtractFlat";
            this.rbnExtractFlat.Size = new System.Drawing.Size(292, 25);
            this.rbnExtractFlat.TabIndex = 5;
            this.rbnExtractFlat.TabStop = true;
            this.rbnExtractFlat.Text = "Extract all files into the same directory";
            this.rbnExtractFlat.UseVisualStyleBackColor = true;
            // 
            // rbnIgnoreFolders
            // 
            this.rbnIgnoreFolders.AutoSize = true;
            this.rbnIgnoreFolders.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnIgnoreFolders.Location = new System.Drawing.Point(14, 129);
            this.rbnIgnoreFolders.Margin = new System.Windows.Forms.Padding(4);
            this.rbnIgnoreFolders.Name = "rbnIgnoreFolders";
            this.rbnIgnoreFolders.Size = new System.Drawing.Size(282, 25);
            this.rbnIgnoreFolders.TabIndex = 4;
            this.rbnIgnoreFolders.TabStop = true;
            this.rbnIgnoreFolders.Text = "Ignore directories and subdirectories";
            this.rbnIgnoreFolders.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowse.Location = new System.Drawing.Point(450, 83);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(94, 31);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtExtractPath
            // 
            this.txtExtractPath.Location = new System.Drawing.Point(14, 85);
            this.txtExtractPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtExtractPath.Name = "txtExtractPath";
            this.txtExtractPath.Size = new System.Drawing.Size(428, 27);
            this.txtExtractPath.TabIndex = 2;
            // 
            // lblExtractPath
            // 
            this.lblExtractPath.AutoSize = true;
            this.lblExtractPath.Location = new System.Drawing.Point(10, 63);
            this.lblExtractPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExtractPath.Name = "lblExtractPath";
            this.lblExtractPath.Size = new System.Drawing.Size(339, 20);
            this.lblExtractPath.TabIndex = 1;
            this.lblExtractPath.Text = "Extract selected item(s) to the following directory:";
            // 
            // cbxExtractAsk
            // 
            this.cbxExtractAsk.AutoSize = true;
            this.cbxExtractAsk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxExtractAsk.Location = new System.Drawing.Point(14, 28);
            this.cbxExtractAsk.Margin = new System.Windows.Forms.Padding(4);
            this.cbxExtractAsk.Name = "cbxExtractAsk";
            this.cbxExtractAsk.Size = new System.Drawing.Size(376, 25);
            this.cbxExtractAsk.TabIndex = 0;
            this.cbxExtractAsk.Text = "Always ask for extraction options before extracting";
            this.cbxExtractAsk.UseVisualStyleBackColor = true;
            // 
            // tabIntegration
            // 
            this.tabIntegration.Controls.Add(this.gbxFileAssociations);
            this.tabIntegration.Controls.Add(this.gbxIntegrationMisc);
            this.tabIntegration.Location = new System.Drawing.Point(4, 29);
            this.tabIntegration.Margin = new System.Windows.Forms.Padding(4);
            this.tabIntegration.Name = "tabIntegration";
            this.tabIntegration.Padding = new System.Windows.Forms.Padding(4);
            this.tabIntegration.Size = new System.Drawing.Size(568, 569);
            this.tabIntegration.TabIndex = 1;
            this.tabIntegration.Text = "Integration";
            this.tabIntegration.UseVisualStyleBackColor = true;
            // 
            // gbxFileAssociations
            // 
            this.gbxFileAssociations.Controls.Add(this.lblFileAssoc);
            this.gbxFileAssociations.Controls.Add(this.btnFileAssoc);
            this.gbxFileAssociations.Location = new System.Drawing.Point(8, 8);
            this.gbxFileAssociations.Margin = new System.Windows.Forms.Padding(4);
            this.gbxFileAssociations.Name = "gbxFileAssociations";
            this.gbxFileAssociations.Padding = new System.Windows.Forms.Padding(4);
            this.gbxFileAssociations.Size = new System.Drawing.Size(552, 85);
            this.gbxFileAssociations.TabIndex = 8;
            this.gbxFileAssociations.TabStop = false;
            this.gbxFileAssociations.Text = "File associations";
            // 
            // lblFileAssoc
            // 
            this.lblFileAssoc.AutoSize = true;
            this.lblFileAssoc.Location = new System.Drawing.Point(11, 24);
            this.lblFileAssoc.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFileAssoc.Name = "lblFileAssoc";
            this.lblFileAssoc.Size = new System.Drawing.Size(389, 40);
            this.lblFileAssoc.TabIndex = 4;
            this.lblFileAssoc.Text = "File associations for TotalImage can be managed through\r\nControl Panel/Windows Se" +
    "ttings.";
            // 
            // btnFileAssoc
            // 
            this.btnFileAssoc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFileAssoc.Location = new System.Drawing.Point(446, 26);
            this.btnFileAssoc.Margin = new System.Windows.Forms.Padding(2);
            this.btnFileAssoc.Name = "btnFileAssoc";
            this.btnFileAssoc.Size = new System.Drawing.Size(100, 32);
            this.btnFileAssoc.TabIndex = 5;
            this.btnFileAssoc.Text = "Open";
            this.btnFileAssoc.UseVisualStyleBackColor = true;
            this.btnFileAssoc.Click += new System.EventHandler(this.btnFileAssoc_Click);
            // 
            // gbxIntegrationMisc
            // 
            this.gbxIntegrationMisc.Controls.Add(this.lblSystemIcons);
            this.gbxIntegrationMisc.Controls.Add(this.cbxShellFileIcons);
            this.gbxIntegrationMisc.Location = new System.Drawing.Point(8, 99);
            this.gbxIntegrationMisc.Margin = new System.Windows.Forms.Padding(2);
            this.gbxIntegrationMisc.Name = "gbxIntegrationMisc";
            this.gbxIntegrationMisc.Padding = new System.Windows.Forms.Padding(2);
            this.gbxIntegrationMisc.Size = new System.Drawing.Size(552, 150);
            this.gbxIntegrationMisc.TabIndex = 7;
            this.gbxIntegrationMisc.TabStop = false;
            this.gbxIntegrationMisc.Text = "Other options";
            // 
            // lblSystemIcons
            // 
            this.lblSystemIcons.AutoSize = true;
            this.lblSystemIcons.Location = new System.Drawing.Point(11, 64);
            this.lblSystemIcons.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSystemIcons.Name = "lblSystemIcons";
            this.lblSystemIcons.Size = new System.Drawing.Size(522, 60);
            this.lblSystemIcons.TabIndex = 6;
            this.lblSystemIcons.Text = "If this option is enabled, TotalImage will obtain icons and names for file types\r" +
    "\nfrom Windows, which can be slower in some situations. If disabled, generic\r\nico" +
    "ns and names will be used instead.";
            // 
            // cbxShellFileIcons
            // 
            this.cbxShellFileIcons.AutoSize = true;
            this.cbxShellFileIcons.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShellFileIcons.Location = new System.Drawing.Point(14, 28);
            this.cbxShellFileIcons.Margin = new System.Windows.Forms.Padding(4);
            this.cbxShellFileIcons.Name = "cbxShellFileIcons";
            this.cbxShellFileIcons.Size = new System.Drawing.Size(374, 25);
            this.cbxShellFileIcons.TabIndex = 3;
            this.cbxShellFileIcons.Text = "Display system icons and file type names in file list";
            this.cbxShellFileIcons.UseVisualStyleBackColor = true;
            // 
            // gbxExtraction
            // 
            this.gbxExtraction.Controls.Add(this.cbxExtractAsk);
            this.gbxExtraction.Controls.Add(this.cbxPreserveAttributes);
            this.gbxExtraction.Controls.Add(this.lblExtractPath);
            this.gbxExtraction.Controls.Add(this.cbxPreserveDates);
            this.gbxExtraction.Controls.Add(this.txtExtractPath);
            this.gbxExtraction.Controls.Add(this.cbxOpenDir);
            this.gbxExtraction.Controls.Add(this.btnBrowse);
            this.gbxExtraction.Controls.Add(this.rbnExtractPreserve);
            this.gbxExtraction.Controls.Add(this.rbnIgnoreFolders);
            this.gbxExtraction.Controls.Add(this.rbnExtractFlat);
            this.gbxExtraction.Location = new System.Drawing.Point(8, 8);
            this.gbxExtraction.Margin = new System.Windows.Forms.Padding(4);
            this.gbxExtraction.Name = "gbxExtraction";
            this.gbxExtraction.Padding = new System.Windows.Forms.Padding(4);
            this.gbxExtraction.Size = new System.Drawing.Size(552, 336);
            this.gbxExtraction.TabIndex = 22;
            this.gbxExtraction.TabStop = false;
            this.gbxExtraction.Text = "Extraction options";
            // 
            // dlgSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(606, 688);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.pnlBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.dlgSettings_Load);
            this.pnlBottom.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.gbxMisc.ResumeLayout(false);
            this.gbxMisc.PerformLayout();
            this.gbxBehavior.ResumeLayout(false);
            this.gbxBehavior.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemoryMapping)).EndInit();
            this.tabView.ResumeLayout(false);
            this.gbxSizeUnits.ResumeLayout(false);
            this.gbxSizeUnits.PerformLayout();
            this.gbxFileList.ResumeLayout(false);
            this.gbxFileList.PerformLayout();
            this.gbxMainWindow.ResumeLayout(false);
            this.gbxMainWindow.PerformLayout();
            this.tabExtraction.ResumeLayout(false);
            this.tabIntegration.ResumeLayout(false);
            this.gbxFileAssociations.ResumeLayout(false);
            this.gbxFileAssociations.PerformLayout();
            this.gbxIntegrationMisc.ResumeLayout(false);
            this.gbxIntegrationMisc.PerformLayout();
            this.gbxExtraction.ResumeLayout(false);
            this.gbxExtraction.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.GroupBox gbxExtraction;
    }
}
