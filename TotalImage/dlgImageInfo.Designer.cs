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
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("File information", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup("Container information", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup("Partition information", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup9 = new System.Windows.Forms.ListViewGroup("File system information", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup10 = new System.Windows.Forms.ListViewGroup("Miscellaneous", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem25 = new System.Windows.Forms.ListViewItem(new string[] { "Filename", "<filename>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem26 = new System.Windows.Forms.ListViewItem(new string[] { "Size", "<filesize>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem27 = new System.Windows.Forms.ListViewItem(new string[] { "Created", "<created>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem28 = new System.Windows.Forms.ListViewItem(new string[] { "Modified", "<modified>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem29 = new System.Windows.Forms.ListViewItem(new string[] { "Accessed", "<accessed>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem30 = new System.Windows.Forms.ListViewItem(new string[] { "Attributes", "<attributes>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem31 = new System.Windows.Forms.ListViewItem(new string[] { "MD5 hash", "<md5hash>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem32 = new System.Windows.Forms.ListViewItem(new string[] { "SHA-1 hash", "<sha1hash>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem33 = new System.Windows.Forms.ListViewItem(new string[] { "Container type", "<containertype>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem34 = new System.Windows.Forms.ListViewItem(new string[] { "Container subtype", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem35 = new System.Windows.Forms.ListViewItem(new string[] { "Container version", "Unknown" }, -1);
            System.Windows.Forms.ListViewItem listViewItem36 = new System.Windows.Forms.ListViewItem(new string[] { "Created by", "Unknown" }, -1);
            System.Windows.Forms.ListViewItem listViewItem37 = new System.Windows.Forms.ListViewItem(new string[] { "Creator version", "Unknown" }, -1);
            System.Windows.Forms.ListViewItem listViewItem38 = new System.Windows.Forms.ListViewItem(new string[] { "Partitioning scheme", "<partitionscheme>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem39 = new System.Windows.Forms.ListViewItem(new string[] { "No. of partitions", "<nopartitions>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem40 = new System.Windows.Forms.ListViewItem(new string[] { "Selected partition", "<selectedpartition>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem41 = new System.Windows.Forms.ListViewItem(new string[] { "Partition ID/type", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem42 = new System.Windows.Forms.ListViewItem(new string[] { "File system", "<filesystem>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem43 = new System.Windows.Forms.ListViewItem(new string[] { "Volume label", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem44 = new System.Windows.Forms.ListViewItem(new string[] { "Volume serial number", "N/A" }, -1);
            System.Windows.Forms.ListViewItem listViewItem45 = new System.Windows.Forms.ListViewItem(new string[] { "Total storage capacity", "<capacity>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem46 = new System.Windows.Forms.ListViewItem(new string[] { "Free space", "<freespace>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem47 = new System.Windows.Forms.ListViewItem(new string[] { "Files", "<nofiles>" }, -1);
            System.Windows.Forms.ListViewItem listViewItem48 = new System.Windows.Forms.ListViewItem(new string[] { "Subdirectories", "<subdirectories>" }, -1);
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
            clmProperty5 = new System.Windows.Forms.ColumnHeader();
            clmValue5 = new System.Windows.Forms.ColumnHeader();
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
            listViewGroup6.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup6.Header = "File information";
            listViewGroup6.Name = "grpFileInfo";
            listViewGroup7.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup7.Header = "Container information";
            listViewGroup7.Name = "grpContainerInfo";
            listViewGroup8.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup8.Header = "Partition information";
            listViewGroup8.Name = "grpPartitionInfo";
            listViewGroup9.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup9.Header = "File system information";
            listViewGroup9.Name = "grpFsInfo";
            listViewGroup10.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup10.Header = "Miscellaneous";
            listViewGroup10.Name = "grpMiscInfo";
            lstProperties.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] { listViewGroup6, listViewGroup7, listViewGroup8, listViewGroup9, listViewGroup10 });
            lstProperties.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem25.Group = listViewGroup6;
            listViewItem25.StateImageIndex = 0;
            listViewItem25.Tag = "Filename";
            listViewItem26.Group = listViewGroup6;
            listViewItem26.StateImageIndex = 0;
            listViewItem26.Tag = "Filesize";
            listViewItem27.Group = listViewGroup6;
            listViewItem27.StateImageIndex = 0;
            listViewItem27.Tag = "Filecreated";
            listViewItem28.Group = listViewGroup6;
            listViewItem28.StateImageIndex = 0;
            listViewItem28.Tag = "Filemodified";
            listViewItem29.Group = listViewGroup6;
            listViewItem29.Tag = "Fileaccessed";
            listViewItem30.Group = listViewGroup6;
            listViewItem30.Tag = "Fileattrib";
            listViewItem31.Group = listViewGroup6;
            listViewItem31.Tag = "md5";
            listViewItem32.Group = listViewGroup6;
            listViewItem32.Tag = "sha1";
            listViewItem33.Group = listViewGroup7;
            listViewItem33.StateImageIndex = 0;
            listViewItem33.Tag = "Container";
            listViewItem34.Group = listViewGroup7;
            listViewItem34.Tag = "Containersubtype";
            listViewItem35.Group = listViewGroup7;
            listViewItem35.Tag = "Containerver";
            listViewItem36.Group = listViewGroup7;
            listViewItem36.Tag = "Creator";
            listViewItem37.Group = listViewGroup7;
            listViewItem37.Tag = "Creatorver";
            listViewItem38.Group = listViewGroup8;
            listViewItem38.StateImageIndex = 0;
            listViewItem38.Tag = "Partscheme";
            listViewItem39.Group = listViewGroup8;
            listViewItem39.StateImageIndex = 0;
            listViewItem39.Tag = "Partcount";
            listViewItem40.Group = listViewGroup8;
            listViewItem40.StateImageIndex = 0;
            listViewItem40.Tag = "Selpart";
            listViewItem41.Group = listViewGroup8;
            listViewItem41.Tag = "PartID";
            listViewItem42.Group = listViewGroup9;
            listViewItem42.StateImageIndex = 0;
            listViewItem42.Tag = "Fs";
            listViewItem43.Group = listViewGroup9;
            listViewItem44.Group = listViewGroup9;
            listViewItem44.Tag = "VolSerial";
            listViewItem45.Group = listViewGroup9;
            listViewItem45.StateImageIndex = 0;
            listViewItem45.Tag = "Capacity";
            listViewItem46.Group = listViewGroup9;
            listViewItem46.StateImageIndex = 0;
            listViewItem46.Tag = "Freespace";
            listViewItem47.Group = listViewGroup9;
            listViewItem47.StateImageIndex = 0;
            listViewItem47.Tag = "Filecount";
            listViewItem48.Group = listViewGroup9;
            listViewItem48.StateImageIndex = 0;
            listViewItem48.Tag = "Dircount";
            lstProperties.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem25, listViewItem26, listViewItem27, listViewItem28, listViewItem29, listViewItem30, listViewItem31, listViewItem32, listViewItem33, listViewItem34, listViewItem35, listViewItem36, listViewItem37, listViewItem38, listViewItem39, listViewItem40, listViewItem41, listViewItem42, listViewItem43, listViewItem44, listViewItem45, listViewItem46, listViewItem47, listViewItem48 });
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
        private System.Windows.Forms.ColumnHeader clmProperty5;
        private System.Windows.Forms.ColumnHeader clmValue5;
    }
}
