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
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem(new string[] { "Filename", "<filename>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem(new string[] { "Size", "<filesize>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem(new string[] { "Created", "<created>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem(new string[] { "Modified", "<modified>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem(new string[] { "Accessed", "<accessed>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem19 = new System.Windows.Forms.ListViewItem(new string[] { "Attributes", "<attributes>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem20 = new System.Windows.Forms.ListViewItem(new string[] { "MD5 hash", "<md5hash>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem21 = new System.Windows.Forms.ListViewItem(new string[] { "SHA-1 hash", "<sha1hash>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] { "Container type", "<containertype>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] { "Container subtype", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] { "Container version", "Unknown" }, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] { "Created by", "Unknown" }, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] { "Creator version", "Unknown" }, -1);
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
            clmProperty = new System.Windows.Forms.ColumnHeader();
            clmValue = new System.Windows.Forms.ColumnHeader();
            tabContainer = new System.Windows.Forms.TabPage();
            tabPartitionTable = new System.Windows.Forms.TabPage();
            tabPartition = new System.Windows.Forms.TabPage();
            tabFileSystem = new System.Windows.Forms.TabPage();
            tabMisc = new System.Windows.Forms.TabPage();
            lstPropertiesContainer = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            pnlBottom.SuspendLayout();
            cmsCopy.SuspendLayout();
            tclImageInfo.SuspendLayout();
            tabFileInfo.SuspendLayout();
            tabContainer.SuspendLayout();
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
            tclImageInfo.Controls.Add(tabMisc);
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
            lstPropertiesFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { clmProperty, clmValue });
            lstPropertiesFile.ContextMenuStrip = cmsCopy;
            lstPropertiesFile.FullRowSelect = true;
            lstPropertiesFile.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem14.StateImageIndex = 0;
            listViewItem14.Tag = "Filename";
            listViewItem15.StateImageIndex = 0;
            listViewItem15.Tag = "Filesize";
            listViewItem16.StateImageIndex = 0;
            listViewItem16.Tag = "Filecreated";
            listViewItem17.StateImageIndex = 0;
            listViewItem17.Tag = "Filemodified";
            listViewItem18.Tag = "Fileaccessed";
            listViewItem19.Tag = "Fileattrib";
            listViewItem20.Tag = "md5";
            listViewItem21.Tag = "sha1";
            lstPropertiesFile.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem14, listViewItem15, listViewItem16, listViewItem17, listViewItem18, listViewItem19, listViewItem20, listViewItem21 });
            lstPropertiesFile.LabelWrap = false;
            lstPropertiesFile.Location = new System.Drawing.Point(6, 6);
            lstPropertiesFile.MultiSelect = false;
            lstPropertiesFile.Name = "lstPropertiesFile";
            lstPropertiesFile.ShowItemToolTips = true;
            lstPropertiesFile.Size = new System.Drawing.Size(429, 335);
            lstPropertiesFile.TabIndex = 1;
            lstPropertiesFile.UseCompatibleStateImageBehavior = false;
            lstPropertiesFile.View = System.Windows.Forms.View.Details;
            // 
            // clmProperty
            // 
            clmProperty.Text = "Property";
            clmProperty.Width = 170;
            // 
            // clmValue
            // 
            clmValue.Text = "Value";
            clmValue.Width = 250;
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
            // tabPartitionTable
            // 
            tabPartitionTable.Location = new System.Drawing.Point(4, 24);
            tabPartitionTable.Name = "tabPartitionTable";
            tabPartitionTable.Padding = new System.Windows.Forms.Padding(3);
            tabPartitionTable.Size = new System.Drawing.Size(441, 347);
            tabPartitionTable.TabIndex = 2;
            tabPartitionTable.Text = "Partition table";
            tabPartitionTable.UseVisualStyleBackColor = true;
            // 
            // tabPartition
            // 
            tabPartition.Location = new System.Drawing.Point(4, 24);
            tabPartition.Name = "tabPartition";
            tabPartition.Padding = new System.Windows.Forms.Padding(3);
            tabPartition.Size = new System.Drawing.Size(441, 347);
            tabPartition.TabIndex = 3;
            tabPartition.Text = "Partition";
            tabPartition.UseVisualStyleBackColor = true;
            // 
            // tabFileSystem
            // 
            tabFileSystem.Location = new System.Drawing.Point(4, 24);
            tabFileSystem.Name = "tabFileSystem";
            tabFileSystem.Padding = new System.Windows.Forms.Padding(3);
            tabFileSystem.Size = new System.Drawing.Size(441, 347);
            tabFileSystem.TabIndex = 4;
            tabFileSystem.Text = "File system";
            tabFileSystem.UseVisualStyleBackColor = true;
            // 
            // tabMisc
            // 
            tabMisc.Location = new System.Drawing.Point(4, 24);
            tabMisc.Name = "tabMisc";
            tabMisc.Padding = new System.Windows.Forms.Padding(3);
            tabMisc.Size = new System.Drawing.Size(441, 347);
            tabMisc.TabIndex = 5;
            tabMisc.Text = "Miscellaneous";
            tabMisc.UseVisualStyleBackColor = true;
            // 
            // lstPropertiesContainer
            // 
            lstPropertiesContainer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2 });
            lstPropertiesContainer.ContextMenuStrip = cmsCopy;
            lstPropertiesContainer.FullRowSelect = true;
            lstPropertiesContainer.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem1.StateImageIndex = 0;
            listViewItem1.Tag = "Container";
            listViewItem2.Tag = "Containersubtype";
            listViewItem3.Tag = "Containerver";
            listViewItem4.Tag = "Creator";
            listViewItem5.Tag = "Creatorver";
            lstPropertiesContainer.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4, listViewItem5 });
            lstPropertiesContainer.LabelWrap = false;
            lstPropertiesContainer.Location = new System.Drawing.Point(6, 6);
            lstPropertiesContainer.MultiSelect = false;
            lstPropertiesContainer.Name = "lstPropertiesContainer";
            lstPropertiesContainer.ShowItemToolTips = true;
            lstPropertiesContainer.Size = new System.Drawing.Size(429, 213);
            lstPropertiesContainer.TabIndex = 24;
            lstPropertiesContainer.UseCompatibleStateImageBehavior = false;
            lstPropertiesContainer.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Property";
            columnHeader1.Width = 170;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Value";
            columnHeader2.Width = 250;
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
            Text = "Image information new";
            FormClosing += dlgImageInfo_FormClosing;
            Load += dlgImageInfo_Load;
            Shown += dlgImageInfo_Shown;
            pnlBottom.ResumeLayout(false);
            cmsCopy.ResumeLayout(false);
            tclImageInfo.ResumeLayout(false);
            tabFileInfo.ResumeLayout(false);
            tabContainer.ResumeLayout(false);
            tabContainer.PerformLayout();
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
        private System.Windows.Forms.TabPage tabMisc;
        private System.Windows.Forms.ListView lstPropertiesFile;
        private System.Windows.Forms.ColumnHeader clmProperty;
        private System.Windows.Forms.ColumnHeader clmValue;
        private System.Windows.Forms.ListView lstPropertiesContainer;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}
