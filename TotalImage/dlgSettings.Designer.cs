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
            this.tabView = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxShowDeletedItems = new System.Windows.Forms.CheckBox();
            this.cbxShowStatusBar = new System.Windows.Forms.CheckBox();
            this.cbxShowDirectoryTree = new System.Windows.Forms.CheckBox();
            this.cbxShowHiddenItems = new System.Windows.Forms.CheckBox();
            this.cbxShowFileList = new System.Windows.Forms.CheckBox();
            this.cbxShowCommandBar = new System.Windows.Forms.CheckBox();
            this.lstSizeUnits = new System.Windows.Forms.ComboBox();
            this.lblSizeUnits = new System.Windows.Forms.Label();
            this.lstSortBy = new System.Windows.Forms.ComboBox();
            this.lblSortBy = new System.Windows.Forms.Label();
            this.lstViewType = new System.Windows.Forms.ComboBox();
            this.lblViewType = new System.Windows.Forms.Label();
            this.tabExtraction = new System.Windows.Forms.TabPage();
            this.cbxOpenDir = new System.Windows.Forms.CheckBox();
            this.rbnExtractPreserve = new System.Windows.Forms.RadioButton();
            this.rbnExtractFlat = new System.Windows.Forms.RadioButton();
            this.rbnIgnoreFolders = new System.Windows.Forms.RadioButton();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtExtractPath = new System.Windows.Forms.TextBox();
            this.lblExtractPath = new System.Windows.Forms.Label();
            this.cbxExtractAsk = new System.Windows.Forms.CheckBox();
            this.tabIntegration = new System.Windows.Forms.TabPage();
            this.cbxShellIntegration = new System.Windows.Forms.CheckBox();
            this.cbxFileAssociations = new System.Windows.Forms.CheckBox();
            this.tabMisc = new System.Windows.Forms.TabPage();
            this.btnClearRecent = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lstSortOrder = new System.Windows.Forms.ComboBox();
            this.pnlBottom.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabView.SuspendLayout();
            this.tabExtraction.SuspendLayout();
            this.tabIntegration.SuspendLayout();
            this.tabMisc.SuspendLayout();
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
            this.tabs.Controls.Add(this.tabMisc);
            this.tabs.Location = new System.Drawing.Point(12, 12);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(461, 482);
            this.tabs.TabIndex = 3;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Location = new System.Drawing.Point(4, 24);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(453, 454);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // tabView
            // 
            this.tabView.Controls.Add(this.lstSortOrder);
            this.tabView.Controls.Add(this.label1);
            this.tabView.Controls.Add(this.cbxShowDeletedItems);
            this.tabView.Controls.Add(this.cbxShowStatusBar);
            this.tabView.Controls.Add(this.cbxShowDirectoryTree);
            this.tabView.Controls.Add(this.cbxShowHiddenItems);
            this.tabView.Controls.Add(this.cbxShowFileList);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(211, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 15);
            this.label1.TabIndex = 18;
            this.label1.Text = "Sorting order for file list:";
            // 
            // cbxShowDeletedItems
            // 
            this.cbxShowDeletedItems.AutoSize = true;
            this.cbxShowDeletedItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowDeletedItems.Location = new System.Drawing.Point(284, 150);
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
            this.cbxShowStatusBar.Location = new System.Drawing.Point(163, 150);
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
            this.cbxShowHiddenItems.Location = new System.Drawing.Point(284, 124);
            this.cbxShowHiddenItems.Name = "cbxShowHiddenItems";
            this.cbxShowHiddenItems.Size = new System.Drawing.Size(133, 20);
            this.cbxShowHiddenItems.TabIndex = 14;
            this.cbxShowHiddenItems.Text = "Show hidden items";
            this.cbxShowHiddenItems.UseVisualStyleBackColor = true;
            // 
            // cbxShowFileList
            // 
            this.cbxShowFileList.AutoSize = true;
            this.cbxShowFileList.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowFileList.Location = new System.Drawing.Point(163, 124);
            this.cbxShowFileList.Name = "cbxShowFileList";
            this.cbxShowFileList.Size = new System.Drawing.Size(98, 20);
            this.cbxShowFileList.TabIndex = 13;
            this.cbxShowFileList.Text = "Show file list";
            this.cbxShowFileList.UseVisualStyleBackColor = true;
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
            this.lstSizeUnits.Location = new System.Drawing.Point(214, 31);
            this.lstSizeUnits.Name = "lstSizeUnits";
            this.lstSizeUnits.Size = new System.Drawing.Size(170, 23);
            this.lstSizeUnits.TabIndex = 11;
            // 
            // lblSizeUnits
            // 
            this.lblSizeUnits.AutoSize = true;
            this.lblSizeUnits.Location = new System.Drawing.Point(211, 13);
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
            this.lstViewType.Location = new System.Drawing.Point(18, 31);
            this.lstViewType.Name = "lstViewType";
            this.lstViewType.Size = new System.Drawing.Size(170, 23);
            this.lstViewType.TabIndex = 7;
            // 
            // lblViewType
            // 
            this.lblViewType.AutoSize = true;
            this.lblViewType.Location = new System.Drawing.Point(15, 13);
            this.lblViewType.Name = "lblViewType";
            this.lblViewType.Size = new System.Drawing.Size(144, 15);
            this.lblViewType.TabIndex = 6;
            this.lblViewType.Text = "Display items in file list as:";
            // 
            // tabExtraction
            // 
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
            this.btnBrowse.Location = new System.Drawing.Point(372, 65);
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
            this.txtExtractPath.Size = new System.Drawing.Size(351, 23);
            this.txtExtractPath.TabIndex = 2;
            // 
            // lblExtractPath
            // 
            this.lblExtractPath.AutoSize = true;
            this.lblExtractPath.Location = new System.Drawing.Point(12, 47);
            this.lblExtractPath.Name = "lblExtractPath";
            this.lblExtractPath.Size = new System.Drawing.Size(269, 15);
            this.lblExtractPath.TabIndex = 1;
            this.lblExtractPath.Text = "Extract selected item(s) to the following directory:";
            // 
            // cbxExtractAsk
            // 
            this.cbxExtractAsk.AutoSize = true;
            this.cbxExtractAsk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxExtractAsk.Location = new System.Drawing.Point(15, 16);
            this.cbxExtractAsk.Name = "cbxExtractAsk";
            this.cbxExtractAsk.Size = new System.Drawing.Size(206, 20);
            this.cbxExtractAsk.TabIndex = 0;
            this.cbxExtractAsk.Text = "Always ask for extraction options";
            this.cbxExtractAsk.UseVisualStyleBackColor = true;
            this.cbxExtractAsk.CheckedChanged += new System.EventHandler(this.cbxExtractAsk_CheckedChanged);
            // 
            // tabIntegration
            // 
            this.tabIntegration.Controls.Add(this.cbxShellIntegration);
            this.tabIntegration.Controls.Add(this.cbxFileAssociations);
            this.tabIntegration.Location = new System.Drawing.Point(4, 24);
            this.tabIntegration.Name = "tabIntegration";
            this.tabIntegration.Padding = new System.Windows.Forms.Padding(3);
            this.tabIntegration.Size = new System.Drawing.Size(453, 454);
            this.tabIntegration.TabIndex = 1;
            this.tabIntegration.Text = "Integration";
            this.tabIntegration.UseVisualStyleBackColor = true;
            // 
            // cbxShellIntegration
            // 
            this.cbxShellIntegration.AutoSize = true;
            this.cbxShellIntegration.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShellIntegration.Location = new System.Drawing.Point(15, 41);
            this.cbxShellIntegration.Name = "cbxShellIntegration";
            this.cbxShellIntegration.Size = new System.Drawing.Size(351, 20);
            this.cbxShellIntegration.TabIndex = 1;
            this.cbxShellIntegration.Text = "Add TotalImage to the right-click menu in Windows Explorer";
            this.cbxShellIntegration.UseVisualStyleBackColor = true;
            // 
            // cbxFileAssociations
            // 
            this.cbxFileAssociations.AutoSize = true;
            this.cbxFileAssociations.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxFileAssociations.Location = new System.Drawing.Point(15, 15);
            this.cbxFileAssociations.Name = "cbxFileAssociations";
            this.cbxFileAssociations.Size = new System.Drawing.Size(291, 20);
            this.cbxFileAssociations.TabIndex = 0;
            this.cbxFileAssociations.Text = "Associate TotalImage with all supported file types";
            this.cbxFileAssociations.UseVisualStyleBackColor = true;
            // 
            // tabMisc
            // 
            this.tabMisc.Controls.Add(this.btnClearRecent);
            this.tabMisc.Controls.Add(this.btnReset);
            this.tabMisc.Location = new System.Drawing.Point(4, 24);
            this.tabMisc.Name = "tabMisc";
            this.tabMisc.Padding = new System.Windows.Forms.Padding(3);
            this.tabMisc.Size = new System.Drawing.Size(453, 454);
            this.tabMisc.TabIndex = 4;
            this.tabMisc.Text = "Miscellaneous";
            this.tabMisc.UseVisualStyleBackColor = true;
            // 
            // btnClearRecent
            // 
            this.btnClearRecent.Location = new System.Drawing.Point(141, 16);
            this.btnClearRecent.Name = "btnClearRecent";
            this.btnClearRecent.Size = new System.Drawing.Size(147, 26);
            this.btnClearRecent.TabIndex = 3;
            this.btnClearRecent.Text = "Clear recent images list";
            this.btnClearRecent.UseVisualStyleBackColor = true;
            this.btnClearRecent.Click += new System.EventHandler(this.btnClearRecent_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(16, 16);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(109, 26);
            this.btnReset.TabIndex = 0;
            this.btnReset.Text = "Reset to defaults";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
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
            this.tabView.ResumeLayout(false);
            this.tabView.PerformLayout();
            this.tabExtraction.ResumeLayout(false);
            this.tabExtraction.PerformLayout();
            this.tabIntegration.ResumeLayout(false);
            this.tabIntegration.PerformLayout();
            this.tabMisc.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tabMisc;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnClearRecent;
        private System.Windows.Forms.CheckBox cbxExtractAsk;
        private System.Windows.Forms.Label lblExtractPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtExtractPath;
        private System.Windows.Forms.RadioButton rbnIgnoreFolders;
        private System.Windows.Forms.RadioButton rbnExtractPreserve;
        private System.Windows.Forms.RadioButton rbnExtractFlat;
        private System.Windows.Forms.CheckBox cbxOpenDir;
        private System.Windows.Forms.CheckBox cbxFileAssociations;
        private System.Windows.Forms.CheckBox cbxShellIntegration;
        private System.Windows.Forms.ComboBox lstSizeUnits;
        private System.Windows.Forms.Label lblSizeUnits;
        private System.Windows.Forms.ComboBox lstSortBy;
        private System.Windows.Forms.Label lblSortBy;
        private System.Windows.Forms.ComboBox lstViewType;
        private System.Windows.Forms.Label lblViewType;
        private System.Windows.Forms.CheckBox cbxShowCommandBar;
        private System.Windows.Forms.CheckBox cbxShowFileList;
        private System.Windows.Forms.CheckBox cbxShowHiddenItems;
        private System.Windows.Forms.CheckBox cbxShowDirectoryTree;
        private System.Windows.Forms.CheckBox cbxShowStatusBar;
        private System.Windows.Forms.CheckBox cbxShowDeletedItems;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox lstSortOrder;
    }
}