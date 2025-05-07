namespace TotalImage
{
    partial class dlgImageInfoNew
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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] { "Filename", "<filename>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] { "Path", "<path>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] { "Size", "<filesize>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] { "Created", "<created>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] { "Modified", "<modified>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] { "Accessed", "<accessed>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] { "Attributes", "<attributes>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] { "MD5 hash", "<md5hash>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] { "SHA-1 hash", "<sha1hash>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] { "Container type", "<containertype>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] { "Container subtype", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] { "Container version", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem(new string[] { "Created by", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem(new string[] { "Creator version", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem(new string[] { "Partitioning scheme", "<partitionscheme>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem(new string[] { "No. of partitions", "<nopartitions>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem(new string[] { "Selected partition", "<selectedpartition>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem(new string[] { "Partition ID/type", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem19 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] { new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "File system", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Segoe UI", 9F)), new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "<filesystem>", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Segoe UI", 9F)) }, -1);
            System.Windows.Forms.ListViewItem listViewItem20 = new System.Windows.Forms.ListViewItem(new string[] { "Volume label", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem21 = new System.Windows.Forms.ListViewItem(new string[] { "Volume serial number", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem22 = new System.Windows.Forms.ListViewItem(new string[] { "Total storage capacity", "<capacity>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem23 = new System.Windows.Forms.ListViewItem(new string[] { "Free space", "<freespace>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem24 = new System.Windows.Forms.ListViewItem(new string[] { "Files", "<nofiles>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem25 = new System.Windows.Forms.ListViewItem(new string[] { "Subdirectories", "<subdirectories>" }, -1);
            btnOK = new System.Windows.Forms.Button();
            pnlBottom = new System.Windows.Forms.Panel();
            btnSave = new System.Windows.Forms.Button();
            lblComment = new System.Windows.Forms.Label();
            txtComment = new System.Windows.Forms.TextBox();
            cmsCopy = new System.Windows.Forms.ContextMenuStrip(components);
            copyValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tclImageInfo = new System.Windows.Forms.TabControl();
            tabFileInfo = new System.Windows.Forms.TabPage();
            lstPropertiesFile = new System.Windows.Forms.ListView();
            clmProperty1 = new System.Windows.Forms.ColumnHeader();
            clmValue1 = new System.Windows.Forms.ColumnHeader();
            tabContainer = new System.Windows.Forms.TabPage();
            lstPropertiesContainer = new System.Windows.Forms.ListView();
            clmProperty2 = new System.Windows.Forms.ColumnHeader();
            clmValue2 = new System.Windows.Forms.ColumnHeader();
            tabPartitionTable = new System.Windows.Forms.TabPage();
            lstPropertiesPT = new System.Windows.Forms.ListView();
            clmPropery3 = new System.Windows.Forms.ColumnHeader();
            clmValue3 = new System.Windows.Forms.ColumnHeader();
            tabPartition = new System.Windows.Forms.TabPage();
            lstPropertiesPartition = new System.Windows.Forms.ListView();
            clmProperty4 = new System.Windows.Forms.ColumnHeader();
            clmValue4 = new System.Windows.Forms.ColumnHeader();
            tabFileSystem = new System.Windows.Forms.TabPage();
            lstPropertiesFS = new System.Windows.Forms.ListView();
            clmProperty5 = new System.Windows.Forms.ColumnHeader();
            clmValue5 = new System.Windows.Forms.ColumnHeader();
            pnlBottom.SuspendLayout();
            cmsCopy.SuspendLayout();
            tclImageInfo.SuspendLayout();
            tabFileInfo.SuspendLayout();
            tabContainer.SuspendLayout();
            tabPartitionTable.SuspendLayout();
            tabPartition.SuspendLayout();
            tabFileSystem.SuspendLayout();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnOK.Location = new System.Drawing.Point(382, 14);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 3;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(btnSave);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 393);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(473, 50);
            pnlBottom.TabIndex = 22;
            // 
            // btnSave
            // 
            btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnSave.Location = new System.Drawing.Point(10, 14);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(80, 26);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save...";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // lblComment
            // 
            lblComment.AutoSize = true;
            lblComment.Location = new System.Drawing.Point(6, 232);
            lblComment.Name = "lblComment";
            lblComment.Size = new System.Drawing.Size(64, 15);
            lblComment.TabIndex = 23;
            lblComment.Text = "Comment:";
            // 
            // txtComment
            // 
            txtComment.Location = new System.Drawing.Point(6, 250);
            txtComment.Multiline = true;
            txtComment.Name = "txtComment";
            txtComment.ReadOnly = true;
            txtComment.Size = new System.Drawing.Size(429, 91);
            txtComment.TabIndex = 1;
            txtComment.Text = "This container type does not support comments.";
            // 
            // cmsCopy
            // 
            cmsCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { copyValueToolStripMenuItem });
            cmsCopy.Name = "cmsCopy";
            cmsCopy.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            cmsCopy.Size = new System.Drawing.Size(134, 26);
            cmsCopy.Opening += cmsCopy_Opening;
            // 
            // copyValueToolStripMenuItem
            // 
            copyValueToolStripMenuItem.Image = Properties.Resources.page_white_copy;
            copyValueToolStripMenuItem.Name = "copyValueToolStripMenuItem";
            copyValueToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            copyValueToolStripMenuItem.Text = "Copy value";
            copyValueToolStripMenuItem.Click += copyValueToolStripMenuItem_Click;
            // 
            // tclImageInfo
            // 
            tclImageInfo.Controls.Add(tabFileInfo);
            tclImageInfo.Controls.Add(tabContainer);
            tclImageInfo.Controls.Add(tabPartitionTable);
            tclImageInfo.Controls.Add(tabPartition);
            tclImageInfo.Controls.Add(tabFileSystem);
            tclImageInfo.Location = new System.Drawing.Point(12, 12);
            tclImageInfo.Name = "tclImageInfo";
            tclImageInfo.SelectedIndex = 0;
            tclImageInfo.Size = new System.Drawing.Size(449, 375);
            tclImageInfo.TabIndex = 24;
            // 
            // tabFileInfo
            // 
            tabFileInfo.Controls.Add(lstPropertiesFile);
            tabFileInfo.Location = new System.Drawing.Point(4, 24);
            tabFileInfo.Name = "tabFileInfo";
            tabFileInfo.Padding = new System.Windows.Forms.Padding(3);
            tabFileInfo.Size = new System.Drawing.Size(441, 347);
            tabFileInfo.TabIndex = 0;
            tabFileInfo.Text = "File";
            tabFileInfo.UseVisualStyleBackColor = true;
            // 
            // lstPropertiesFile
            // 
            lstPropertiesFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { clmProperty1, clmValue1 });
            lstPropertiesFile.ContextMenuStrip = cmsCopy;
            lstPropertiesFile.FullRowSelect = true;
            lstPropertiesFile.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem1.StateImageIndex = 0;
            listViewItem1.Tag = "Filename";
            listViewItem1.UseItemStyleForSubItems = false;
            listViewItem2.StateImageIndex = 0;
            listViewItem2.Tag = "Path";
            listViewItem2.UseItemStyleForSubItems = false;
            listViewItem3.StateImageIndex = 0;
            listViewItem3.Tag = "Filesize";
            listViewItem3.UseItemStyleForSubItems = false;
            listViewItem4.StateImageIndex = 0;
            listViewItem4.Tag = "Filecreated";
            listViewItem4.UseItemStyleForSubItems = false;
            listViewItem5.StateImageIndex = 0;
            listViewItem5.Tag = "Filemodified";
            listViewItem5.UseItemStyleForSubItems = false;
            listViewItem6.Tag = "Fileaccessed";
            listViewItem6.UseItemStyleForSubItems = false;
            listViewItem7.Tag = "Fileattrib";
            listViewItem7.UseItemStyleForSubItems = false;
            listViewItem8.Tag = "md5";
            listViewItem8.UseItemStyleForSubItems = false;
            listViewItem9.Tag = "sha1";
            listViewItem9.UseItemStyleForSubItems = false;
            lstPropertiesFile.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4, listViewItem5, listViewItem6, listViewItem7, listViewItem8, listViewItem9 });
            lstPropertiesFile.LabelWrap = false;
            lstPropertiesFile.Location = new System.Drawing.Point(6, 6);
            lstPropertiesFile.MultiSelect = false;
            lstPropertiesFile.Name = "lstPropertiesFile";
            lstPropertiesFile.ShowGroups = false;
            lstPropertiesFile.ShowItemToolTips = true;
            lstPropertiesFile.Size = new System.Drawing.Size(429, 335);
            lstPropertiesFile.TabIndex = 1;
            lstPropertiesFile.UseCompatibleStateImageBehavior = false;
            lstPropertiesFile.View = System.Windows.Forms.View.Details;
            // 
            // clmProperty1
            // 
            clmProperty1.Text = "Property";
            clmProperty1.Width = 170;
            // 
            // clmValue1
            // 
            clmValue1.Text = "Value";
            clmValue1.Width = 250;
            // 
            // tabContainer
            // 
            tabContainer.Controls.Add(lstPropertiesContainer);
            tabContainer.Controls.Add(lblComment);
            tabContainer.Controls.Add(txtComment);
            tabContainer.Location = new System.Drawing.Point(4, 24);
            tabContainer.Name = "tabContainer";
            tabContainer.Padding = new System.Windows.Forms.Padding(3);
            tabContainer.Size = new System.Drawing.Size(441, 347);
            tabContainer.TabIndex = 1;
            tabContainer.Text = "Container";
            tabContainer.UseVisualStyleBackColor = true;
            // 
            // lstPropertiesContainer
            // 
            lstPropertiesContainer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { clmProperty2, clmValue2 });
            lstPropertiesContainer.ContextMenuStrip = cmsCopy;
            lstPropertiesContainer.FullRowSelect = true;
            lstPropertiesContainer.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem10.StateImageIndex = 0;
            listViewItem10.Tag = "Container";
            listViewItem10.UseItemStyleForSubItems = false;
            listViewItem11.StateImageIndex = 0;
            listViewItem11.Tag = "Containersubtype";
            listViewItem11.UseItemStyleForSubItems = false;
            listViewItem12.StateImageIndex = 0;
            listViewItem12.Tag = "Containerver";
            listViewItem12.UseItemStyleForSubItems = false;
            listViewItem13.StateImageIndex = 0;
            listViewItem13.Tag = "Creator";
            listViewItem13.UseItemStyleForSubItems = false;
            listViewItem14.StateImageIndex = 0;
            listViewItem14.Tag = "Creatorver";
            listViewItem14.UseItemStyleForSubItems = false;
            lstPropertiesContainer.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem10, listViewItem11, listViewItem12, listViewItem13, listViewItem14 });
            lstPropertiesContainer.LabelWrap = false;
            lstPropertiesContainer.Location = new System.Drawing.Point(6, 6);
            lstPropertiesContainer.MultiSelect = false;
            lstPropertiesContainer.Name = "lstPropertiesContainer";
            lstPropertiesContainer.ShowGroups = false;
            lstPropertiesContainer.ShowItemToolTips = true;
            lstPropertiesContainer.Size = new System.Drawing.Size(429, 213);
            lstPropertiesContainer.TabIndex = 24;
            lstPropertiesContainer.UseCompatibleStateImageBehavior = false;
            lstPropertiesContainer.View = System.Windows.Forms.View.Details;
            // 
            // clmProperty2
            // 
            clmProperty2.Text = "Property";
            clmProperty2.Width = 170;
            // 
            // clmValue2
            // 
            clmValue2.Text = "Value";
            clmValue2.Width = 250;
            // 
            // tabPartitionTable
            // 
            tabPartitionTable.Controls.Add(lstPropertiesPT);
            tabPartitionTable.Location = new System.Drawing.Point(4, 24);
            tabPartitionTable.Name = "tabPartitionTable";
            tabPartitionTable.Padding = new System.Windows.Forms.Padding(3);
            tabPartitionTable.Size = new System.Drawing.Size(441, 347);
            tabPartitionTable.TabIndex = 2;
            tabPartitionTable.Text = "Partition table";
            tabPartitionTable.UseVisualStyleBackColor = true;
            // 
            // lstPropertiesPT
            // 
            lstPropertiesPT.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { clmPropery3, clmValue3 });
            lstPropertiesPT.ContextMenuStrip = cmsCopy;
            lstPropertiesPT.FullRowSelect = true;
            lstPropertiesPT.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem15.StateImageIndex = 0;
            listViewItem15.Tag = "Partscheme";
            listViewItem15.UseItemStyleForSubItems = false;
            listViewItem16.StateImageIndex = 0;
            listViewItem16.Tag = "Partcount";
            listViewItem16.UseItemStyleForSubItems = false;
            listViewItem17.StateImageIndex = 0;
            listViewItem17.Tag = "Selpart";
            listViewItem17.UseItemStyleForSubItems = false;
            lstPropertiesPT.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem15, listViewItem16, listViewItem17 });
            lstPropertiesPT.LabelWrap = false;
            lstPropertiesPT.Location = new System.Drawing.Point(6, 6);
            lstPropertiesPT.MultiSelect = false;
            lstPropertiesPT.Name = "lstPropertiesPT";
            lstPropertiesPT.ShowGroups = false;
            lstPropertiesPT.ShowItemToolTips = true;
            lstPropertiesPT.Size = new System.Drawing.Size(429, 335);
            lstPropertiesPT.TabIndex = 1;
            lstPropertiesPT.UseCompatibleStateImageBehavior = false;
            lstPropertiesPT.View = System.Windows.Forms.View.Details;
            // 
            // clmPropery3
            // 
            clmPropery3.Text = "Property";
            clmPropery3.Width = 170;
            // 
            // clmValue3
            // 
            clmValue3.Text = "Value";
            clmValue3.Width = 250;
            // 
            // tabPartition
            // 
            tabPartition.Controls.Add(lstPropertiesPartition);
            tabPartition.Location = new System.Drawing.Point(4, 24);
            tabPartition.Name = "tabPartition";
            tabPartition.Padding = new System.Windows.Forms.Padding(3);
            tabPartition.Size = new System.Drawing.Size(441, 347);
            tabPartition.TabIndex = 3;
            tabPartition.Text = "Selected partition";
            tabPartition.UseVisualStyleBackColor = true;
            // 
            // lstPropertiesPartition
            // 
            lstPropertiesPartition.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { clmProperty4, clmValue4 });
            lstPropertiesPartition.ContextMenuStrip = cmsCopy;
            lstPropertiesPartition.FullRowSelect = true;
            lstPropertiesPartition.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem18.StateImageIndex = 0;
            listViewItem18.Tag = "PartID";
            listViewItem18.UseItemStyleForSubItems = false;
            lstPropertiesPartition.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem18 });
            lstPropertiesPartition.LabelWrap = false;
            lstPropertiesPartition.Location = new System.Drawing.Point(6, 6);
            lstPropertiesPartition.MultiSelect = false;
            lstPropertiesPartition.Name = "lstPropertiesPartition";
            lstPropertiesPartition.ShowGroups = false;
            lstPropertiesPartition.ShowItemToolTips = true;
            lstPropertiesPartition.Size = new System.Drawing.Size(429, 335);
            lstPropertiesPartition.TabIndex = 1;
            lstPropertiesPartition.UseCompatibleStateImageBehavior = false;
            lstPropertiesPartition.View = System.Windows.Forms.View.Details;
            // 
            // clmProperty4
            // 
            clmProperty4.Text = "Property";
            clmProperty4.Width = 170;
            // 
            // clmValue4
            // 
            clmValue4.Text = "Value";
            clmValue4.Width = 250;
            // 
            // tabFileSystem
            // 
            tabFileSystem.Controls.Add(lstPropertiesFS);
            tabFileSystem.Location = new System.Drawing.Point(4, 24);
            tabFileSystem.Name = "tabFileSystem";
            tabFileSystem.Padding = new System.Windows.Forms.Padding(3);
            tabFileSystem.Size = new System.Drawing.Size(441, 347);
            tabFileSystem.TabIndex = 4;
            tabFileSystem.Text = "File system";
            tabFileSystem.UseVisualStyleBackColor = true;
            // 
            // lstPropertiesFS
            // 
            lstPropertiesFS.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { clmProperty5, clmValue5 });
            lstPropertiesFS.ContextMenuStrip = cmsCopy;
            lstPropertiesFS.FullRowSelect = true;
            lstPropertiesFS.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem19.StateImageIndex = 0;
            listViewItem19.Tag = "Fs";
            listViewItem19.UseItemStyleForSubItems = false;
            listViewItem20.StateImageIndex = 0;
            listViewItem20.Tag = "VolLabel";
            listViewItem20.UseItemStyleForSubItems = false;
            listViewItem21.StateImageIndex = 0;
            listViewItem21.Tag = "VolSerial";
            listViewItem21.UseItemStyleForSubItems = false;
            listViewItem22.StateImageIndex = 0;
            listViewItem22.Tag = "Capacity";
            listViewItem22.UseItemStyleForSubItems = false;
            listViewItem23.StateImageIndex = 0;
            listViewItem23.Tag = "Freespace";
            listViewItem23.UseItemStyleForSubItems = false;
            listViewItem24.StateImageIndex = 0;
            listViewItem24.Tag = "Filecount";
            listViewItem24.UseItemStyleForSubItems = false;
            listViewItem25.StateImageIndex = 0;
            listViewItem25.Tag = "Dircount";
            listViewItem25.UseItemStyleForSubItems = false;
            lstPropertiesFS.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem19, listViewItem20, listViewItem21, listViewItem22, listViewItem23, listViewItem24, listViewItem25 });
            lstPropertiesFS.LabelWrap = false;
            lstPropertiesFS.Location = new System.Drawing.Point(6, 6);
            lstPropertiesFS.MultiSelect = false;
            lstPropertiesFS.Name = "lstPropertiesFS";
            lstPropertiesFS.ShowGroups = false;
            lstPropertiesFS.ShowItemToolTips = true;
            lstPropertiesFS.Size = new System.Drawing.Size(429, 335);
            lstPropertiesFS.TabIndex = 24;
            lstPropertiesFS.UseCompatibleStateImageBehavior = false;
            lstPropertiesFS.View = System.Windows.Forms.View.Details;
            // 
            // clmProperty5
            // 
            clmProperty5.Text = "Property";
            clmProperty5.Width = 170;
            // 
            // clmValue5
            // 
            clmValue5.Text = "Value";
            clmValue5.Width = 250;
            // 
            // dlgImageInfoNew
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            ClientSize = new System.Drawing.Size(473, 443);
            Controls.Add(tclImageInfo);
            Controls.Add(pnlBottom);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgImageInfoNew";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Image information";
            FormClosing += dlgImageInfo_FormClosing;
            Load += dlgImageInfo_Load;
            Shown += dlgImageInfo_Shown;
            pnlBottom.ResumeLayout(false);
            cmsCopy.ResumeLayout(false);
            tclImageInfo.ResumeLayout(false);
            tabFileInfo.ResumeLayout(false);
            tabContainer.ResumeLayout(false);
            tabContainer.PerformLayout();
            tabPartitionTable.ResumeLayout(false);
            tabPartition.ResumeLayout(false);
            tabFileSystem.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ContextMenuStrip cmsCopy;
        private System.Windows.Forms.ToolStripMenuItem copyValueToolStripMenuItem;
        private System.Windows.Forms.TabControl tclImageInfo;
        private System.Windows.Forms.TabPage tabFileInfo;
        private System.Windows.Forms.TabPage tabContainer;
        private System.Windows.Forms.TabPage tabPartitionTable;
        private System.Windows.Forms.TabPage tabPartition;
        private System.Windows.Forms.TabPage tabFileSystem;
        private System.Windows.Forms.ListView lstPropertiesFile;
        private System.Windows.Forms.ColumnHeader clmProperty1;
        private System.Windows.Forms.ColumnHeader clmValue1;
        private System.Windows.Forms.ListView lstPropertiesContainer;
        private System.Windows.Forms.ColumnHeader clmProperty2;
        private System.Windows.Forms.ColumnHeader clmValue2;
        private System.Windows.Forms.ListView lstPropertiesFS;
        private System.Windows.Forms.ListView lstPropertiesPT;
        private System.Windows.Forms.ColumnHeader clmPropery3;
        private System.Windows.Forms.ColumnHeader clmValue3;
        private System.Windows.Forms.ListView lstPropertiesPartition;
        private System.Windows.Forms.ColumnHeader clmProperty4;
        private System.Windows.Forms.ColumnHeader clmValue4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader clmProperty5;
        private System.Windows.Forms.ColumnHeader clmValue5;
    }
}
