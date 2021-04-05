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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Raw sector images (.IMG, .IMA, .VFD, .FLP)");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("WinImage compressed image (.IMZ)");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Anex86 disk images (.FDI, .FDM, .HDI, .HDM)");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("IBM SafeDskF image (.DSK)");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("DiskDupe image (.DDI)");
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.gbxMisc = new System.Windows.Forms.GroupBox();
            this.btnClearRecent = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.gbxBehavior = new System.Windows.Forms.GroupBox();
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
            this.cbxSelectAll = new System.Windows.Forms.CheckBox();
            this.lstFileTypes = new System.Windows.Forms.ListView();
            this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblFileAssociations = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.gbxMisc.SuspendLayout();
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
            this.btnCancel.Location = new System.Drawing.Point(393, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(307, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
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
            this.pnlBottom.Location = new System.Drawing.Point(0, 500);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(485, 50);
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
            this.tabs.Location = new System.Drawing.Point(12, 12);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(461, 482);
            this.tabs.TabIndex = 3;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.gbxMisc);
            this.tabGeneral.Controls.Add(this.gbxBehavior);
            this.tabGeneral.Location = new System.Drawing.Point(4, 24);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(453, 454);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // gbxMisc
            // 
            this.gbxMisc.Controls.Add(this.btnClearRecent);
            this.gbxMisc.Controls.Add(this.btnReset);
            this.gbxMisc.Location = new System.Drawing.Point(6, 199);
            this.gbxMisc.Name = "gbxMisc";
            this.gbxMisc.Size = new System.Drawing.Size(441, 59);
            this.gbxMisc.TabIndex = 1;
            this.gbxMisc.TabStop = false;
            this.gbxMisc.Text = "Miscellaneous";
            // 
            // btnClearRecent
            // 
            this.btnClearRecent.Location = new System.Drawing.Point(121, 22);
            this.btnClearRecent.Name = "btnClearRecent";
            this.btnClearRecent.Size = new System.Drawing.Size(147, 26);
            this.btnClearRecent.TabIndex = 5;
            this.btnClearRecent.Text = "Clear recent images list";
            this.btnClearRecent.UseVisualStyleBackColor = true;
            this.btnClearRecent.Click += new System.EventHandler(this.btnClearRecent_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(6, 22);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(109, 26);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Reset to defaults";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // gbxBehavior
            // 
            this.gbxBehavior.Location = new System.Drawing.Point(6, 6);
            this.gbxBehavior.Name = "gbxBehavior";
            this.gbxBehavior.Size = new System.Drawing.Size(441, 187);
            this.gbxBehavior.TabIndex = 0;
            this.gbxBehavior.TabStop = false;
            this.gbxBehavior.Text = "Behavior";
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
            this.tabView.Location = new System.Drawing.Point(4, 24);
            this.tabView.Name = "tabView";
            this.tabView.Padding = new System.Windows.Forms.Padding(3);
            this.tabView.Size = new System.Drawing.Size(453, 454);
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
            this.lstSortOrder.Location = new System.Drawing.Point(214, 84);
            this.lstSortOrder.Name = "lstSortOrder";
            this.lstSortOrder.Size = new System.Drawing.Size(170, 23);
            this.lstSortOrder.TabIndex = 19;
            // 
            // lblSortOrder
            // 
            this.lblSortOrder.AutoSize = true;
            this.lblSortOrder.Location = new System.Drawing.Point(211, 66);
            this.lblSortOrder.Name = "lblSortOrder";
            this.lblSortOrder.Size = new System.Drawing.Size(134, 15);
            this.lblSortOrder.TabIndex = 18;
            this.lblSortOrder.Text = "Sorting order for file list:";
            // 
            // cbxShowDeletedItems
            // 
            this.cbxShowDeletedItems.AutoSize = true;
            this.cbxShowDeletedItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowDeletedItems.Location = new System.Drawing.Point(214, 150);
            this.cbxShowDeletedItems.Name = "cbxShowDeletedItems";
            this.cbxShowDeletedItems.Size = new System.Drawing.Size(135, 20);
            this.cbxShowDeletedItems.TabIndex = 17;
            this.cbxShowDeletedItems.Text = "Show deleted items";
            this.cbxShowDeletedItems.UseVisualStyleBackColor = true;
            // 
            // cbxShowStatusBar
            // 
            this.cbxShowStatusBar.AutoSize = true;
            this.cbxShowStatusBar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowStatusBar.Location = new System.Drawing.Point(214, 124);
            this.cbxShowStatusBar.Name = "cbxShowStatusBar";
            this.cbxShowStatusBar.Size = new System.Drawing.Size(115, 20);
            this.cbxShowStatusBar.TabIndex = 16;
            this.cbxShowStatusBar.Text = "Show status bar";
            this.cbxShowStatusBar.UseVisualStyleBackColor = true;
            // 
            // cbxShowDirectoryTree
            // 
            this.cbxShowDirectoryTree.AutoSize = true;
            this.cbxShowDirectoryTree.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowDirectoryTree.Location = new System.Drawing.Point(18, 150);
            this.cbxShowDirectoryTree.Name = "cbxShowDirectoryTree";
            this.cbxShowDirectoryTree.Size = new System.Drawing.Size(134, 20);
            this.cbxShowDirectoryTree.TabIndex = 15;
            this.cbxShowDirectoryTree.Text = "Show directory tree";
            this.cbxShowDirectoryTree.UseVisualStyleBackColor = true;
            // 
            // cbxShowHiddenItems
            // 
            this.cbxShowHiddenItems.AutoSize = true;
            this.cbxShowHiddenItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowHiddenItems.Location = new System.Drawing.Point(18, 176);
            this.cbxShowHiddenItems.Name = "cbxShowHiddenItems";
            this.cbxShowHiddenItems.Size = new System.Drawing.Size(133, 20);
            this.cbxShowHiddenItems.TabIndex = 14;
            this.cbxShowHiddenItems.Text = "Show hidden items";
            this.cbxShowHiddenItems.UseVisualStyleBackColor = true;
            // 
            // cbxShowCommandBar
            // 
            this.cbxShowCommandBar.AutoSize = true;
            this.cbxShowCommandBar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowCommandBar.Location = new System.Drawing.Point(18, 124);
            this.cbxShowCommandBar.Name = "cbxShowCommandBar";
            this.cbxShowCommandBar.Size = new System.Drawing.Size(139, 20);
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
            "Bytes (B)",
            "Kilobytes (KB = 1000 B)",
            "Kibibytes (KiB = 1024 B)",
            "Megabytes (MB = 1000 KB)",
            "Mebibytes (MiB = 1024 KiB)"});
            this.lstSizeUnits.Location = new System.Drawing.Point(214, 33);
            this.lstSizeUnits.Name = "lstSizeUnits";
            this.lstSizeUnits.Size = new System.Drawing.Size(170, 23);
            this.lstSizeUnits.TabIndex = 11;
            // 
            // lblSizeUnits
            // 
            this.lblSizeUnits.AutoSize = true;
            this.lblSizeUnits.Location = new System.Drawing.Point(211, 15);
            this.lblSizeUnits.Name = "lblSizeUnits";
            this.lblSizeUnits.Size = new System.Drawing.Size(59, 15);
            this.lblSizeUnits.TabIndex = 10;
            this.lblSizeUnits.Text = "Size units:";
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
            this.lstSortBy.Location = new System.Drawing.Point(18, 84);
            this.lstSortBy.Name = "lstSortBy";
            this.lstSortBy.Size = new System.Drawing.Size(170, 23);
            this.lstSortBy.TabIndex = 9;
            // 
            // lblSortBy
            // 
            this.lblSortBy.AutoSize = true;
            this.lblSortBy.Location = new System.Drawing.Point(15, 66);
            this.lblSortBy.Name = "lblSortBy";
            this.lblSortBy.Size = new System.Drawing.Size(129, 15);
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
            this.lstViewType.Location = new System.Drawing.Point(18, 33);
            this.lstViewType.Name = "lstViewType";
            this.lstViewType.Size = new System.Drawing.Size(170, 23);
            this.lstViewType.TabIndex = 7;
            // 
            // lblViewType
            // 
            this.lblViewType.AutoSize = true;
            this.lblViewType.Location = new System.Drawing.Point(15, 15);
            this.lblViewType.Name = "lblViewType";
            this.lblViewType.Size = new System.Drawing.Size(144, 15);
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
            this.tabExtraction.Location = new System.Drawing.Point(4, 24);
            this.tabExtraction.Name = "tabExtraction";
            this.tabExtraction.Padding = new System.Windows.Forms.Padding(3);
            this.tabExtraction.Size = new System.Drawing.Size(453, 454);
            this.tabExtraction.TabIndex = 3;
            this.tabExtraction.Text = "Extraction";
            this.tabExtraction.UseVisualStyleBackColor = true;
            // 
            // cbxPreserveAttributes
            // 
            this.cbxPreserveAttributes.AutoSize = true;
            this.cbxPreserveAttributes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxPreserveAttributes.Location = new System.Drawing.Point(15, 240);
            this.cbxPreserveAttributes.Name = "cbxPreserveAttributes";
            this.cbxPreserveAttributes.Size = new System.Drawing.Size(236, 20);
            this.cbxPreserveAttributes.TabIndex = 9;
            this.cbxPreserveAttributes.Text = "Preserve file attributes when extracting";
            this.cbxPreserveAttributes.UseVisualStyleBackColor = true;
            // 
            // cbxPreserveDates
            // 
            this.cbxPreserveDates.AutoSize = true;
            this.cbxPreserveDates.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxPreserveDates.Location = new System.Drawing.Point(15, 214);
            this.cbxPreserveDates.Name = "cbxPreserveDates";
            this.cbxPreserveDates.Size = new System.Drawing.Size(214, 20);
            this.cbxPreserveDates.TabIndex = 8;
            this.cbxPreserveDates.Text = "Preserve file dates when extracting";
            this.cbxPreserveDates.UseVisualStyleBackColor = true;
            // 
            // cbxOpenDir
            // 
            this.cbxOpenDir.AutoSize = true;
            this.cbxOpenDir.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxOpenDir.Location = new System.Drawing.Point(15, 188);
            this.cbxOpenDir.Name = "cbxOpenDir";
            this.cbxOpenDir.Size = new System.Drawing.Size(256, 20);
            this.cbxOpenDir.TabIndex = 7;
            this.cbxOpenDir.Text = "Open destination directory after extraction";
            this.cbxOpenDir.UseVisualStyleBackColor = true;
            // 
            // rbnExtractPreserve
            // 
            this.rbnExtractPreserve.AutoSize = true;
            this.rbnExtractPreserve.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnExtractPreserve.Location = new System.Drawing.Point(15, 154);
            this.rbnExtractPreserve.Name = "rbnExtractPreserve";
            this.rbnExtractPreserve.Size = new System.Drawing.Size(218, 20);
            this.rbnExtractPreserve.TabIndex = 6;
            this.rbnExtractPreserve.TabStop = true;
            this.rbnExtractPreserve.Text = "Preserve original directory structure";
            this.rbnExtractPreserve.UseVisualStyleBackColor = true;
            // 
            // rbnExtractFlat
            // 
            this.rbnExtractFlat.AutoSize = true;
            this.rbnExtractFlat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnExtractFlat.Location = new System.Drawing.Point(15, 129);
            this.rbnExtractFlat.Name = "rbnExtractFlat";
            this.rbnExtractFlat.Size = new System.Drawing.Size(231, 20);
            this.rbnExtractFlat.TabIndex = 5;
            this.rbnExtractFlat.TabStop = true;
            this.rbnExtractFlat.Text = "Extract all files into the same directory";
            this.rbnExtractFlat.UseVisualStyleBackColor = true;
            // 
            // rbnIgnoreFolders
            // 
            this.rbnIgnoreFolders.AutoSize = true;
            this.rbnIgnoreFolders.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnIgnoreFolders.Location = new System.Drawing.Point(15, 104);
            this.rbnIgnoreFolders.Name = "rbnIgnoreFolders";
            this.rbnIgnoreFolders.Size = new System.Drawing.Size(223, 20);
            this.rbnIgnoreFolders.TabIndex = 4;
            this.rbnIgnoreFolders.TabStop = true;
            this.rbnIgnoreFolders.Text = "Ignore directories and subdirectories";
            this.rbnIgnoreFolders.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowse.Location = new System.Drawing.Point(364, 65);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 25);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtExtractPath
            // 
            this.txtExtractPath.Location = new System.Drawing.Point(15, 66);
            this.txtExtractPath.Name = "txtExtractPath";
            this.txtExtractPath.Size = new System.Drawing.Size(343, 23);
            this.txtExtractPath.TabIndex = 2;
            // 
            // lblExtractPath
            // 
            this.lblExtractPath.AutoSize = true;
            this.lblExtractPath.Location = new System.Drawing.Point(12, 48);
            this.lblExtractPath.Name = "lblExtractPath";
            this.lblExtractPath.Size = new System.Drawing.Size(269, 15);
            this.lblExtractPath.TabIndex = 1;
            this.lblExtractPath.Text = "Extract selected item(s) to the following directory:";
            // 
            // cbxExtractAsk
            // 
            this.cbxExtractAsk.AutoSize = true;
            this.cbxExtractAsk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxExtractAsk.Location = new System.Drawing.Point(15, 15);
            this.cbxExtractAsk.Name = "cbxExtractAsk";
            this.cbxExtractAsk.Size = new System.Drawing.Size(292, 20);
            this.cbxExtractAsk.TabIndex = 0;
            this.cbxExtractAsk.Text = "Always show extraction options before extracting";
            this.cbxExtractAsk.UseVisualStyleBackColor = true;
            // 
            // tabIntegration
            // 
            this.tabIntegration.Controls.Add(this.cbxSelectAll);
            this.tabIntegration.Controls.Add(this.lstFileTypes);
            this.tabIntegration.Controls.Add(this.lblFileAssociations);
            this.tabIntegration.Location = new System.Drawing.Point(4, 24);
            this.tabIntegration.Name = "tabIntegration";
            this.tabIntegration.Padding = new System.Windows.Forms.Padding(3);
            this.tabIntegration.Size = new System.Drawing.Size(453, 454);
            this.tabIntegration.TabIndex = 1;
            this.tabIntegration.Text = "Integration";
            this.tabIntegration.UseVisualStyleBackColor = true;
            // 
            // cbxSelectAll
            // 
            this.cbxSelectAll.AutoSize = true;
            this.cbxSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxSelectAll.Location = new System.Drawing.Point(18, 149);
            this.cbxSelectAll.Name = "cbxSelectAll";
            this.cbxSelectAll.Size = new System.Drawing.Size(78, 20);
            this.cbxSelectAll.TabIndex = 2;
            this.cbxSelectAll.Text = "Select all";
            this.cbxSelectAll.UseVisualStyleBackColor = true;
            this.cbxSelectAll.Click += new System.EventHandler(this.cbxSelectAll_Click);
            // 
            // lstFileTypes
            // 
            this.lstFileTypes.AutoArrange = false;
            this.lstFileTypes.CheckBoxes = true;
            this.lstFileTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnType});
            this.lstFileTypes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstFileTypes.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.StateImageIndex = 0;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.StateImageIndex = 0;
            this.lstFileTypes.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5});
            this.lstFileTypes.Location = new System.Drawing.Point(18, 33);
            this.lstFileTypes.MultiSelect = false;
            this.lstFileTypes.Name = "lstFileTypes";
            this.lstFileTypes.ShowGroups = false;
            this.lstFileTypes.Size = new System.Drawing.Size(416, 110);
            this.lstFileTypes.TabIndex = 1;
            this.lstFileTypes.UseCompatibleStateImageBehavior = false;
            this.lstFileTypes.View = System.Windows.Forms.View.Details;
            this.lstFileTypes.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lstFileTypes_ItemChecked);
            this.lstFileTypes.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstFileTypes_ItemSelectionChanged);
            // 
            // columnType
            // 
            this.columnType.Text = "";
            this.columnType.Width = 410;
            // 
            // lblFileAssociations
            // 
            this.lblFileAssociations.AutoSize = true;
            this.lblFileAssociations.Location = new System.Drawing.Point(15, 15);
            this.lblFileAssociations.Name = "lblFileAssociations";
            this.lblFileAssociations.Size = new System.Drawing.Size(270, 15);
            this.lblFileAssociations.TabIndex = 0;
            this.lblFileAssociations.Text = "Associate the following file types with TotalImage:";
            // 
            // dlgSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(485, 550);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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
        private System.Windows.Forms.CheckBox cbxSelectAll;
        private System.Windows.Forms.GroupBox gbxMisc;
        private System.Windows.Forms.Button btnClearRecent;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox cbxPreserveAttributes;
        private System.Windows.Forms.CheckBox cbxPreserveDates;
    }
}