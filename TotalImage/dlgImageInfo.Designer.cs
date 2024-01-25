namespace TotalImage
{
    partial class dlgImageInfo
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("File information", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Container information", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Partition information", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("File system information", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Miscellaneous", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] { "Filename", "<filename>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] { "Size", "<filesize>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] { "Created", "<created>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] { "Modified", "<modified>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] { "Accessed", "<accessed>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] { "Attributes", "<attributes>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] { "MD5 hash", "<md5hash>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] { "SHA-1 hash", "<sha1hash>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] { "Container type", "<containertype>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] { "Container subtype", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] { "Container version", "Unknown" }, -1);
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] { "Created by", "Unknown" }, -1);
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem(new string[] { "Creator version", "Unknown" }, -1);
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem(new string[] { "Partitioning scheme", "<partitionscheme>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem(new string[] { "No. of partitions", "<nopartitions>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem(new string[] { "Selected partition", "<selectedpartition>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem(new string[] { "Partition ID/type", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem(new string[] { "File system", "<filesystem>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem19 = new System.Windows.Forms.ListViewItem(new string[] { "Volume label", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem20 = new System.Windows.Forms.ListViewItem(new string[] { "Volume serial number", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem21 = new System.Windows.Forms.ListViewItem(new string[] { "Total storage capacity", "<capacity>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem22 = new System.Windows.Forms.ListViewItem(new string[] { "Free space", "<freespace>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem23 = new System.Windows.Forms.ListViewItem(new string[] { "Files", "<nofiles>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem24 = new System.Windows.Forms.ListViewItem(new string[] { "Subdirectories", "<subdirectories>" }, -1);
            btnOK = new System.Windows.Forms.Button();
            pnlBottom = new System.Windows.Forms.Panel();
            btnSave = new System.Windows.Forms.Button();
            lblComment = new System.Windows.Forms.Label();
            txtComment = new System.Windows.Forms.TextBox();
            lstProperties = new System.Windows.Forms.ListView();
            clmProperty = new System.Windows.Forms.ColumnHeader();
            clmValue = new System.Windows.Forms.ColumnHeader();
            cmsCopy = new System.Windows.Forms.ContextMenuStrip(components);
            copyValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pnlBottom.SuspendLayout();
            cmsCopy.SuspendLayout();
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
            lblComment.Location = new System.Drawing.Point(12, 269);
            lblComment.Name = "lblComment";
            lblComment.Size = new System.Drawing.Size(64, 15);
            lblComment.TabIndex = 23;
            lblComment.Text = "Comment:";
            // 
            // txtComment
            // 
            txtComment.Location = new System.Drawing.Point(12, 287);
            txtComment.Multiline = true;
            txtComment.Name = "txtComment";
            txtComment.ReadOnly = true;
            txtComment.Size = new System.Drawing.Size(449, 91);
            txtComment.TabIndex = 1;
            txtComment.Text = "This container type does not support comments.";
            // 
            // lstProperties
            // 
            lstProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { clmProperty, clmValue });
            lstProperties.ContextMenuStrip = cmsCopy;
            lstProperties.FullRowSelect = true;
            listViewGroup1.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup1.Header = "File information";
            listViewGroup1.Name = "grpFileInfo";
            listViewGroup2.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup2.Header = "Container information";
            listViewGroup2.Name = "grpContainerInfo";
            listViewGroup3.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup3.Header = "Partition information";
            listViewGroup3.Name = "grpPartitionInfo";
            listViewGroup4.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup4.Header = "File system information";
            listViewGroup4.Name = "grpFsInfo";
            listViewGroup5.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup5.Header = "Miscellaneous";
            listViewGroup5.Name = "grpMiscInfo";
            lstProperties.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] { listViewGroup1, listViewGroup2, listViewGroup3, listViewGroup4, listViewGroup5 });
            lstProperties.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem1.Group = listViewGroup1;
            listViewItem1.StateImageIndex = 0;
            listViewItem1.Tag = "Filename";
            listViewItem2.Group = listViewGroup1;
            listViewItem2.StateImageIndex = 0;
            listViewItem2.Tag = "Filesize";
            listViewItem3.Group = listViewGroup1;
            listViewItem3.StateImageIndex = 0;
            listViewItem3.Tag = "Filecreated";
            listViewItem4.Group = listViewGroup1;
            listViewItem4.StateImageIndex = 0;
            listViewItem4.Tag = "Filemodified";
            listViewItem5.Group = listViewGroup1;
            listViewItem5.Tag = "Fileaccessed";
            listViewItem6.Group = listViewGroup1;
            listViewItem6.Tag = "Fileattrib";
            listViewItem7.Group = listViewGroup1;
            listViewItem7.Tag = "md5";
            listViewItem8.Group = listViewGroup1;
            listViewItem8.Tag = "sha1";
            listViewItem9.Group = listViewGroup2;
            listViewItem9.StateImageIndex = 0;
            listViewItem9.Tag = "Container";
            listViewItem10.Group = listViewGroup2;
            listViewItem10.Tag = "Containersubtype";
            listViewItem11.Group = listViewGroup2;
            listViewItem11.Tag = "Containerver";
            listViewItem12.Group = listViewGroup2;
            listViewItem12.Tag = "Creator";
            listViewItem13.Group = listViewGroup2;
            listViewItem13.Tag = "Creatorver";
            listViewItem14.Group = listViewGroup3;
            listViewItem14.StateImageIndex = 0;
            listViewItem14.Tag = "Partscheme";
            listViewItem15.Group = listViewGroup3;
            listViewItem15.StateImageIndex = 0;
            listViewItem15.Tag = "Partcount";
            listViewItem16.Group = listViewGroup3;
            listViewItem16.StateImageIndex = 0;
            listViewItem16.Tag = "Selpart";
            listViewItem17.Group = listViewGroup3;
            listViewItem17.Tag = "PartID";
            listViewItem18.Group = listViewGroup4;
            listViewItem18.StateImageIndex = 0;
            listViewItem18.Tag = "Fs";
            listViewItem19.Group = listViewGroup4;
            listViewItem20.Group = listViewGroup4;
            listViewItem20.Tag = "VolSerial";
            listViewItem21.Group = listViewGroup4;
            listViewItem21.StateImageIndex = 0;
            listViewItem21.Tag = "Capacity";
            listViewItem22.Group = listViewGroup4;
            listViewItem22.StateImageIndex = 0;
            listViewItem22.Tag = "Freespace";
            listViewItem23.Group = listViewGroup4;
            listViewItem23.StateImageIndex = 0;
            listViewItem23.Tag = "Filecount";
            listViewItem24.Group = listViewGroup4;
            listViewItem24.StateImageIndex = 0;
            listViewItem24.Tag = "Dircount";
            lstProperties.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4, listViewItem5, listViewItem6, listViewItem7, listViewItem8, listViewItem9, listViewItem10, listViewItem11, listViewItem12, listViewItem13, listViewItem14, listViewItem15, listViewItem16, listViewItem17, listViewItem18, listViewItem19, listViewItem20, listViewItem21, listViewItem22, listViewItem23, listViewItem24 });
            lstProperties.LabelWrap = false;
            lstProperties.Location = new System.Drawing.Point(12, 12);
            lstProperties.MultiSelect = false;
            lstProperties.Name = "lstProperties";
            lstProperties.ShowItemToolTips = true;
            lstProperties.Size = new System.Drawing.Size(449, 245);
            lstProperties.TabIndex = 0;
            lstProperties.UseCompatibleStateImageBehavior = false;
            lstProperties.View = System.Windows.Forms.View.Details;
            // 
            // clmProperty
            // 
            clmProperty.Text = "Property";
            clmProperty.Width = 175;
            // 
            // clmValue
            // 
            clmValue.Text = "Value";
            clmValue.Width = 250;
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
            // dlgImageInfo
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            ClientSize = new System.Drawing.Size(473, 443);
            Controls.Add(lstProperties);
            Controls.Add(txtComment);
            Controls.Add(lblComment);
            Controls.Add(pnlBottom);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgImageInfo";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Image information";
            FormClosing += dlgImageInfo_FormClosing;
            Load += dlgImageInfo_Load;
            Shown += dlgImageInfo_Shown;
            pnlBottom.ResumeLayout(false);
            cmsCopy.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.ListView lstProperties;
        private System.Windows.Forms.ColumnHeader clmProperty;
        private System.Windows.Forms.ColumnHeader clmValue;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ContextMenuStrip cmsCopy;
        private System.Windows.Forms.ToolStripMenuItem copyValueToolStripMenuItem;
    }
}
