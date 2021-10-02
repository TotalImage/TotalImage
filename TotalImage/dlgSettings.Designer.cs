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
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("Raw sector images (.IMG, .IMA, .VFD, .FLP)");
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("WinImage compressed image (.IMZ)");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("Anex86 disk images (.FDI, .FDM, .HDI, .HDM)");
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("IBM SafeDskF image (.DSK)");
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem("DiskDupe image (.DDI)");
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.gbxMisc = new System.Windows.Forms.GroupBox();
            this.btnClearTemp = new System.Windows.Forms.Button();
            this.btnClearRecent = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.gbxBehavior = new System.Windows.Forms.GroupBox();
            this.lblMemoryMapping1 = new System.Windows.Forms.Label();
            this.txtMemoryMapping = new System.Windows.Forms.NumericUpDown();
            this.lblMemoryMapping = new System.Windows.Forms.Label();
            this.cbxAutoincrementFilename = new System.Windows.Forms.CheckBox();
            this.cbxConfirmOverwriteExtract = new System.Windows.Forms.CheckBox();
            this.cbxConfirmDeletion = new System.Windows.Forms.CheckBox();
            this.cbxConfirmInjection = new System.Windows.Forms.CheckBox();
            this.tabView = new System.Windows.Forms.TabPage();
            this.lstSortOrder = new System.Windows.Forms.ComboBox();
            this.lblSortOrder = new System.Windows.Forms.Label();
            this.cbxShowDeletedItems = new System.Windows.Forms.CheckBox();
            this.cbxShowStatusBar = new System.Windows.Forms.CheckBox();
            this.cbxShowDirectoryTree = new System.Windows.Forms.CheckBox();
            this.cbxShowHiddenItems = new System.Windows.Forms.CheckBox();
            this.cbxShowCommandBar = new System.Windows.Forms.CheckBox();
            this.lstSizeUnits = new System.Windows.Forms.ComboBox();
            this.lblSizeUnits = new System.Windows.Forms.Label();
            this.lstSortBy = new System.Windows.Forms.ComboBox();
            this.lblSortBy = new System.Windows.Forms.Label();
            this.lstViewType = new System.Windows.Forms.ComboBox();
            this.lblViewType = new System.Windows.Forms.Label();
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
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.cbxShellFileIcons = new System.Windows.Forms.CheckBox();
            this.lstFileTypes = new System.Windows.Forms.ListView();
            this.columnType = new System.Windows.Forms.ColumnHeader();
            this.lblFileAssociations = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.gbxMisc.SuspendLayout();
            this.gbxBehavior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemoryMapping)).BeginInit();
            this.tabView.SuspendLayout();
            this.tabExtraction.SuspendLayout();
            this.tabIntegration.SuspendLayout();
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
            this.tabs.Location = new System.Drawing.Point(15, 15);
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
            this.gbxMisc.Controls.Add(this.btnClearTemp);
            this.gbxMisc.Controls.Add(this.btnClearRecent);
            this.gbxMisc.Controls.Add(this.btnReset);
            this.gbxMisc.Location = new System.Drawing.Point(8, 249);
            this.gbxMisc.Margin = new System.Windows.Forms.Padding(4);
            this.gbxMisc.Name = "gbxMisc";
            this.gbxMisc.Padding = new System.Windows.Forms.Padding(4);
            this.gbxMisc.Size = new System.Drawing.Size(551, 74);
            this.gbxMisc.TabIndex = 1;
            this.gbxMisc.TabStop = false;
            this.gbxMisc.Text = "Miscellaneous";
            // 
            // btnClearTemp
            // 
            this.btnClearTemp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClearTemp.Location = new System.Drawing.Point(334, 28);
            this.btnClearTemp.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearTemp.Name = "btnClearTemp";
            this.btnClearTemp.Size = new System.Drawing.Size(140, 32);
            this.btnClearTemp.TabIndex = 6;
            this.btnClearTemp.Text = "Clear temp folder";
            this.btnClearTemp.UseVisualStyleBackColor = true;
            this.btnClearTemp.Click += new System.EventHandler(this.btnClearTemp_Click);
            // 
            // btnClearRecent
            // 
            this.btnClearRecent.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClearRecent.Location = new System.Drawing.Point(151, 28);
            this.btnClearRecent.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearRecent.Name = "btnClearRecent";
            this.btnClearRecent.Size = new System.Drawing.Size(175, 32);
            this.btnClearRecent.TabIndex = 5;
            this.btnClearRecent.Text = "Clear recent images list";
            this.btnClearRecent.UseVisualStyleBackColor = true;
            this.btnClearRecent.Click += new System.EventHandler(this.btnClearRecent_Click);
            // 
            // btnReset
            // 
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnReset.Location = new System.Drawing.Point(8, 28);
            this.btnReset.Margin = new System.Windows.Forms.Padding(4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(136, 32);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Reset to defaults";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
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
            this.gbxBehavior.Size = new System.Drawing.Size(551, 234);
            this.gbxBehavior.TabIndex = 0;
            this.gbxBehavior.TabStop = false;
            this.gbxBehavior.Text = "Behavior";
            // 
            // lblMemoryMapping1
            // 
            this.lblMemoryMapping1.AutoSize = true;
            this.lblMemoryMapping1.Location = new System.Drawing.Point(431, 174);
            this.lblMemoryMapping1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMemoryMapping1.Name = "lblMemoryMapping1";
            this.lblMemoryMapping1.Size = new System.Drawing.Size(35, 20);
            this.lblMemoryMapping1.TabIndex = 21;
            this.lblMemoryMapping1.Text = "MiB";
            // 
            // txtMemoryMapping
            // 
            this.txtMemoryMapping.Location = new System.Drawing.Point(299, 169);
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
            this.lblMemoryMapping.Location = new System.Drawing.Point(8, 174);
            this.lblMemoryMapping.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMemoryMapping.Name = "lblMemoryMapping";
            this.lblMemoryMapping.Size = new System.Drawing.Size(284, 20);
            this.lblMemoryMapping.TabIndex = 19;
            this.lblMemoryMapping.Text = "Threshold for mapping files into memory:";
            // 
            // cbxAutoincrementFilename
            // 
            this.cbxAutoincrementFilename.AutoSize = true;
            this.cbxAutoincrementFilename.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxAutoincrementFilename.Location = new System.Drawing.Point(8, 125);
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
            this.cbxConfirmOverwriteExtract.Location = new System.Drawing.Point(8, 28);
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
            this.cbxConfirmDeletion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxConfirmDeletion.Location = new System.Drawing.Point(8, 60);
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
            this.cbxConfirmInjection.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxConfirmInjection.Location = new System.Drawing.Point(8, 92);
            this.cbxConfirmInjection.Margin = new System.Windows.Forms.Padding(4);
            this.cbxConfirmInjection.Name = "cbxConfirmInjection";
            this.cbxConfirmInjection.Size = new System.Drawing.Size(154, 25);
            this.cbxConfirmInjection.TabIndex = 15;
            this.cbxConfirmInjection.Text = "Confirm injection";
            this.cbxConfirmInjection.UseVisualStyleBackColor = true;
            // 
            // tabView
            // 
            this.tabView.Controls.Add(this.lstSortOrder);
            this.tabView.Controls.Add(this.lblSortOrder);
            this.tabView.Controls.Add(this.cbxShowDeletedItems);
            this.tabView.Controls.Add(this.cbxShowStatusBar);
            this.tabView.Controls.Add(this.cbxShowDirectoryTree);
            this.tabView.Controls.Add(this.cbxShowHiddenItems);
            this.tabView.Controls.Add(this.cbxShowCommandBar);
            this.tabView.Controls.Add(this.lstSizeUnits);
            this.tabView.Controls.Add(this.lblSizeUnits);
            this.tabView.Controls.Add(this.lstSortBy);
            this.tabView.Controls.Add(this.lblSortBy);
            this.tabView.Controls.Add(this.lstViewType);
            this.tabView.Controls.Add(this.lblViewType);
            this.tabView.Location = new System.Drawing.Point(4, 29);
            this.tabView.Margin = new System.Windows.Forms.Padding(4);
            this.tabView.Name = "tabView";
            this.tabView.Padding = new System.Windows.Forms.Padding(4);
            this.tabView.Size = new System.Drawing.Size(568, 569);
            this.tabView.TabIndex = 2;
            this.tabView.Text = "View";
            this.tabView.UseVisualStyleBackColor = true;
            // 
            // lstSortOrder
            // 
            this.lstSortOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstSortOrder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstSortOrder.FormattingEnabled = true;
            this.lstSortOrder.Items.AddRange(new object[] {
            "Ascending",
            "Descending"});
            this.lstSortOrder.Location = new System.Drawing.Point(268, 105);
            this.lstSortOrder.Margin = new System.Windows.Forms.Padding(4);
            this.lstSortOrder.Name = "lstSortOrder";
            this.lstSortOrder.Size = new System.Drawing.Size(212, 28);
            this.lstSortOrder.TabIndex = 19;
            // 
            // lblSortOrder
            // 
            this.lblSortOrder.AutoSize = true;
            this.lblSortOrder.Location = new System.Drawing.Point(264, 82);
            this.lblSortOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSortOrder.Name = "lblSortOrder";
            this.lblSortOrder.Size = new System.Drawing.Size(171, 20);
            this.lblSortOrder.TabIndex = 18;
            this.lblSortOrder.Text = "Sorting order for file list:";
            // 
            // cbxShowDeletedItems
            // 
            this.cbxShowDeletedItems.AutoSize = true;
            this.cbxShowDeletedItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowDeletedItems.Location = new System.Drawing.Point(268, 188);
            this.cbxShowDeletedItems.Margin = new System.Windows.Forms.Padding(4);
            this.cbxShowDeletedItems.Name = "cbxShowDeletedItems";
            this.cbxShowDeletedItems.Size = new System.Drawing.Size(183, 25);
            this.cbxShowDeletedItems.TabIndex = 17;
            this.cbxShowDeletedItems.Text = "Show deleted objects";
            this.cbxShowDeletedItems.UseVisualStyleBackColor = true;
            // 
            // cbxShowStatusBar
            // 
            this.cbxShowStatusBar.AutoSize = true;
            this.cbxShowStatusBar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowStatusBar.Location = new System.Drawing.Point(268, 155);
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
            this.cbxShowDirectoryTree.Location = new System.Drawing.Point(22, 188);
            this.cbxShowDirectoryTree.Margin = new System.Windows.Forms.Padding(4);
            this.cbxShowDirectoryTree.Name = "cbxShowDirectoryTree";
            this.cbxShowDirectoryTree.Size = new System.Drawing.Size(169, 25);
            this.cbxShowDirectoryTree.TabIndex = 15;
            this.cbxShowDirectoryTree.Text = "Show directory tree";
            this.cbxShowDirectoryTree.UseVisualStyleBackColor = true;
            // 
            // cbxShowHiddenItems
            // 
            this.cbxShowHiddenItems.AutoSize = true;
            this.cbxShowHiddenItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowHiddenItems.Location = new System.Drawing.Point(22, 220);
            this.cbxShowHiddenItems.Margin = new System.Windows.Forms.Padding(4);
            this.cbxShowHiddenItems.Name = "cbxShowHiddenItems";
            this.cbxShowHiddenItems.Size = new System.Drawing.Size(178, 25);
            this.cbxShowHiddenItems.TabIndex = 14;
            this.cbxShowHiddenItems.Text = "Show hidden objects";
            this.cbxShowHiddenItems.UseVisualStyleBackColor = true;
            // 
            // cbxShowCommandBar
            // 
            this.cbxShowCommandBar.AutoSize = true;
            this.cbxShowCommandBar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowCommandBar.Location = new System.Drawing.Point(22, 155);
            this.cbxShowCommandBar.Margin = new System.Windows.Forms.Padding(4);
            this.cbxShowCommandBar.Name = "cbxShowCommandBar";
            this.cbxShowCommandBar.Size = new System.Drawing.Size(173, 25);
            this.cbxShowCommandBar.TabIndex = 12;
            this.cbxShowCommandBar.Text = "Show command bar";
            this.cbxShowCommandBar.UseVisualStyleBackColor = true;
            // 
            // lstSizeUnits
            // 
            this.lstSizeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstSizeUnits.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstSizeUnits.FormattingEnabled = true;
            this.lstSizeUnits.Items.AddRange(new object[] {
            "Bytes",
            "Decimal units (1000)",
            "Binary units (1024)"});
            this.lstSizeUnits.Location = new System.Drawing.Point(268, 41);
            this.lstSizeUnits.Margin = new System.Windows.Forms.Padding(4);
            this.lstSizeUnits.Name = "lstSizeUnits";
            this.lstSizeUnits.Size = new System.Drawing.Size(212, 28);
            this.lstSizeUnits.TabIndex = 11;
            // 
            // lblSizeUnits
            // 
            this.lblSizeUnits.AutoSize = true;
            this.lblSizeUnits.Location = new System.Drawing.Point(264, 19);
            this.lblSizeUnits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSizeUnits.Name = "lblSizeUnits";
            this.lblSizeUnits.Size = new System.Drawing.Size(68, 20);
            this.lblSizeUnits.TabIndex = 10;
            this.lblSizeUnits.Text = "Size unit:";
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
            "Modified"});
            this.lstSortBy.Location = new System.Drawing.Point(22, 105);
            this.lstSortBy.Margin = new System.Windows.Forms.Padding(4);
            this.lstSortBy.Name = "lstSortBy";
            this.lstSortBy.Size = new System.Drawing.Size(212, 28);
            this.lstSortBy.TabIndex = 9;
            // 
            // lblSortBy
            // 
            this.lblSortBy.AutoSize = true;
            this.lblSortBy.Location = new System.Drawing.Point(19, 82);
            this.lblSortBy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSortBy.Name = "lblSortBy";
            this.lblSortBy.Size = new System.Drawing.Size(163, 20);
            this.lblSortBy.TabIndex = 8;
            this.lblSortBy.Text = "Sort items in file list by:";
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
            "Details",
            "Tiles"});
            this.lstViewType.Location = new System.Drawing.Point(22, 41);
            this.lstViewType.Margin = new System.Windows.Forms.Padding(4);
            this.lstViewType.Name = "lstViewType";
            this.lstViewType.Size = new System.Drawing.Size(212, 28);
            this.lstViewType.TabIndex = 7;
            // 
            // lblViewType
            // 
            this.lblViewType.AutoSize = true;
            this.lblViewType.Location = new System.Drawing.Point(19, 19);
            this.lblViewType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblViewType.Name = "lblViewType";
            this.lblViewType.Size = new System.Drawing.Size(183, 20);
            this.lblViewType.TabIndex = 6;
            this.lblViewType.Text = "Display items in file list as:";
            // 
            // tabExtraction
            // 
            this.tabExtraction.Controls.Add(this.cbxPreserveAttributes);
            this.tabExtraction.Controls.Add(this.cbxPreserveDates);
            this.tabExtraction.Controls.Add(this.cbxOpenDir);
            this.tabExtraction.Controls.Add(this.rbnExtractPreserve);
            this.tabExtraction.Controls.Add(this.rbnExtractFlat);
            this.tabExtraction.Controls.Add(this.rbnIgnoreFolders);
            this.tabExtraction.Controls.Add(this.btnBrowse);
            this.tabExtraction.Controls.Add(this.txtExtractPath);
            this.tabExtraction.Controls.Add(this.lblExtractPath);
            this.tabExtraction.Controls.Add(this.cbxExtractAsk);
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
            this.cbxPreserveAttributes.Location = new System.Drawing.Point(19, 300);
            this.cbxPreserveAttributes.Margin = new System.Windows.Forms.Padding(4);
            this.cbxPreserveAttributes.Name = "cbxPreserveAttributes";
            this.cbxPreserveAttributes.Size = new System.Drawing.Size(271, 25);
            this.cbxPreserveAttributes.TabIndex = 9;
            this.cbxPreserveAttributes.Text = "Preserve attributes when extracting";
            this.cbxPreserveAttributes.UseVisualStyleBackColor = true;
            // 
            // cbxPreserveDates
            // 
            this.cbxPreserveDates.AutoSize = true;
            this.cbxPreserveDates.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxPreserveDates.Location = new System.Drawing.Point(19, 268);
            this.cbxPreserveDates.Margin = new System.Windows.Forms.Padding(4);
            this.cbxPreserveDates.Name = "cbxPreserveDates";
            this.cbxPreserveDates.Size = new System.Drawing.Size(244, 25);
            this.cbxPreserveDates.TabIndex = 8;
            this.cbxPreserveDates.Text = "Preserve dates when extracting";
            this.cbxPreserveDates.UseVisualStyleBackColor = true;
            // 
            // cbxOpenDir
            // 
            this.cbxOpenDir.AutoSize = true;
            this.cbxOpenDir.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxOpenDir.Location = new System.Drawing.Point(19, 235);
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
            this.rbnExtractPreserve.Location = new System.Drawing.Point(19, 192);
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
            this.rbnExtractFlat.Location = new System.Drawing.Point(19, 161);
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
            this.rbnIgnoreFolders.Location = new System.Drawing.Point(19, 130);
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
            this.btnBrowse.Location = new System.Drawing.Point(455, 81);
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
            this.txtExtractPath.Location = new System.Drawing.Point(19, 82);
            this.txtExtractPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtExtractPath.Name = "txtExtractPath";
            this.txtExtractPath.Size = new System.Drawing.Size(428, 27);
            this.txtExtractPath.TabIndex = 2;
            // 
            // lblExtractPath
            // 
            this.lblExtractPath.AutoSize = true;
            this.lblExtractPath.Location = new System.Drawing.Point(15, 60);
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
            this.cbxExtractAsk.Location = new System.Drawing.Point(19, 19);
            this.cbxExtractAsk.Margin = new System.Windows.Forms.Padding(4);
            this.cbxExtractAsk.Name = "cbxExtractAsk";
            this.cbxExtractAsk.Size = new System.Drawing.Size(366, 25);
            this.cbxExtractAsk.TabIndex = 0;
            this.cbxExtractAsk.Text = "Always show extraction options before extracting";
            this.cbxExtractAsk.UseVisualStyleBackColor = true;
            // 
            // tabIntegration
            // 
            this.tabIntegration.Controls.Add(this.btnClearAll);
            this.tabIntegration.Controls.Add(this.btnSelectAll);
            this.tabIntegration.Controls.Add(this.cbxShellFileIcons);
            this.tabIntegration.Controls.Add(this.lstFileTypes);
            this.tabIntegration.Controls.Add(this.lblFileAssociations);
            this.tabIntegration.Location = new System.Drawing.Point(4, 29);
            this.tabIntegration.Margin = new System.Windows.Forms.Padding(4);
            this.tabIntegration.Name = "tabIntegration";
            this.tabIntegration.Padding = new System.Windows.Forms.Padding(4);
            this.tabIntegration.Size = new System.Drawing.Size(568, 569);
            this.tabIntegration.TabIndex = 1;
            this.tabIntegration.Text = "Integration";
            this.tabIntegration.UseVisualStyleBackColor = true;
            // 
            // btnClearAll
            // 
            this.btnClearAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClearAll.Location = new System.Drawing.Point(130, 186);
            this.btnClearAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(100, 32);
            this.btnClearAll.TabIndex = 4;
            this.btnClearAll.Text = "Clear all";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Enabled = false;
            this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectAll.Location = new System.Drawing.Point(22, 186);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(100, 32);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "Select all";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // cbxShellFileIcons
            // 
            this.cbxShellFileIcons.AutoSize = true;
            this.cbxShellFileIcons.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShellFileIcons.Location = new System.Drawing.Point(22, 250);
            this.cbxShellFileIcons.Margin = new System.Windows.Forms.Padding(4);
            this.cbxShellFileIcons.Name = "cbxShellFileIcons";
            this.cbxShellFileIcons.Size = new System.Drawing.Size(399, 25);
            this.cbxShellFileIcons.TabIndex = 3;
            this.cbxShellFileIcons.Text = "Display system icons and file type names in the file list";
            this.cbxShellFileIcons.UseVisualStyleBackColor = true;
            // 
            // lstFileTypes
            // 
            this.lstFileTypes.AutoArrange = false;
            this.lstFileTypes.CheckBoxes = true;
            this.lstFileTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnType});
            this.lstFileTypes.Enabled = false;
            this.lstFileTypes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstFileTypes.HideSelection = false;
            listViewItem6.StateImageIndex = 0;
            listViewItem7.StateImageIndex = 0;
            listViewItem8.StateImageIndex = 0;
            listViewItem9.StateImageIndex = 0;
            listViewItem10.StateImageIndex = 0;
            this.lstFileTypes.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10});
            this.lstFileTypes.Location = new System.Drawing.Point(22, 41);
            this.lstFileTypes.Margin = new System.Windows.Forms.Padding(4);
            this.lstFileTypes.MultiSelect = false;
            this.lstFileTypes.Name = "lstFileTypes";
            this.lstFileTypes.ShowGroups = false;
            this.lstFileTypes.Size = new System.Drawing.Size(519, 136);
            this.lstFileTypes.TabIndex = 1;
            this.lstFileTypes.UseCompatibleStateImageBehavior = false;
            this.lstFileTypes.View = System.Windows.Forms.View.Details;
            // 
            // columnType
            // 
            this.columnType.Text = "";
            this.columnType.Width = 410;
            // 
            // lblFileAssociations
            // 
            this.lblFileAssociations.AutoSize = true;
            this.lblFileAssociations.Location = new System.Drawing.Point(19, 19);
            this.lblFileAssociations.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFileAssociations.Name = "lblFileAssociations";
            this.lblFileAssociations.Size = new System.Drawing.Size(342, 20);
            this.lblFileAssociations.TabIndex = 0;
            this.lblFileAssociations.Text = "Associate the following file types with TotalImage:";
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
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.dlgSettings_Load);
            this.pnlBottom.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.gbxMisc.ResumeLayout(false);
            this.gbxBehavior.ResumeLayout(false);
            this.gbxBehavior.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemoryMapping)).EndInit();
            this.tabView.ResumeLayout(false);
            this.tabView.PerformLayout();
            this.tabExtraction.ResumeLayout(false);
            this.tabExtraction.PerformLayout();
            this.tabIntegration.ResumeLayout(false);
            this.tabIntegration.PerformLayout();
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
        private System.Windows.Forms.Label lblFileAssociations;
        private System.Windows.Forms.ListView lstFileTypes;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.GroupBox gbxMisc;
        private System.Windows.Forms.Button btnClearRecent;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox cbxPreserveAttributes;
        private System.Windows.Forms.CheckBox cbxPreserveDates;
        private System.Windows.Forms.CheckBox cbxShellFileIcons;
        private System.Windows.Forms.CheckBox cbxConfirmInjection;
        private System.Windows.Forms.CheckBox cbxConfirmDeletion;
        private System.Windows.Forms.CheckBox cbxConfirmOverwriteExtract;
        private System.Windows.Forms.CheckBox cbxAutoincrementFilename;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Label lblMemoryMapping1;
        private System.Windows.Forms.Label lblMemoryMapping;
        private System.Windows.Forms.NumericUpDown txtMemoryMapping;
        private System.Windows.Forms.Button btnClearTemp;
    }
}