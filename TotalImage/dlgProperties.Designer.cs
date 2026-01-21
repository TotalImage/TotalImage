namespace TotalImage
{
    partial class dlgProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgProperties));
            imgIcon = new System.Windows.Forms.PictureBox();
            txtFilename = new System.Windows.Forms.TextBox();
            lblType = new System.Windows.Forms.Label();
            lblLocation = new System.Windows.Forms.Label();
            lblSize = new System.Windows.Forms.Label();
            lblSizeOnDisk = new System.Windows.Forms.Label();
            toolTip = new System.Windows.Forms.ToolTip(components);
            lblShortFilename = new System.Windows.Forms.Label();
            cbxReadOnly = new System.Windows.Forms.CheckBox();
            cbxHidden = new System.Windows.Forms.CheckBox();
            cbxSystem = new System.Windows.Forms.CheckBox();
            cbxArchive = new System.Windows.Forms.CheckBox();
            dtpAccessed = new System.Windows.Forms.DateTimePicker();
            dtpCreated = new System.Windows.Forms.DateTimePicker();
            dtpModified = new System.Windows.Forms.DateTimePicker();
            txtSize1 = new System.Windows.Forms.TextBox();
            txtSizeOnDisk1 = new System.Windows.Forms.TextBox();
            txtShortFilename1 = new System.Windows.Forms.TextBox();
            txtContains1 = new System.Windows.Forms.TextBox();
            lblContains = new System.Windows.Forms.Label();
            cbxDateCreated = new System.Windows.Forms.CheckBox();
            cbxDateModified = new System.Windows.Forms.CheckBox();
            cbxDateAccessed = new System.Windows.Forms.CheckBox();
            txtFirstCluster1 = new System.Windows.Forms.TextBox();
            lblFirstCluster = new System.Windows.Forms.Label();
            btnCancel = new System.Windows.Forms.Button();
            btnOK = new System.Windows.Forms.Button();
            pnlBottom = new System.Windows.Forms.Panel();
            lblSeparator1 = new System.Windows.Forms.Label();
            lblSeparator2 = new System.Windows.Forms.Label();
            lblSeparator3 = new System.Windows.Forms.Label();
            lblAttributes = new System.Windows.Forms.Label();
            txtType1 = new System.Windows.Forms.TextBox();
            txtLocation1 = new System.Windows.Forms.TextBox();
            lblMultipleObjectsCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)imgIcon).BeginInit();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // imgIcon
            // 
            imgIcon.Location = new System.Drawing.Point(12, 12);
            imgIcon.Name = "imgIcon";
            imgIcon.Size = new System.Drawing.Size(32, 32);
            imgIcon.TabIndex = 6;
            imgIcon.TabStop = false;
            // 
            // txtFilename
            // 
            txtFilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtFilename.Location = new System.Drawing.Point(50, 16);
            txtFilename.Name = "txtFilename";
            txtFilename.Size = new System.Drawing.Size(322, 23);
            txtFilename.TabIndex = 0;
            txtFilename.Text = "<filename>";
            toolTip.SetToolTip(txtFilename, resources.GetString("txtFilename.ToolTip"));
            txtFilename.TextChanged += txtFilename_TextChanged;
            // 
            // lblType
            // 
            lblType.AutoSize = true;
            lblType.Location = new System.Drawing.Point(12, 62);
            lblType.Name = "lblType";
            lblType.Size = new System.Drawing.Size(35, 15);
            lblType.TabIndex = 8;
            lblType.Text = "Type:";
            // 
            // lblLocation
            // 
            lblLocation.AutoSize = true;
            lblLocation.Location = new System.Drawing.Point(12, 86);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new System.Drawing.Size(56, 15);
            lblLocation.TabIndex = 10;
            lblLocation.Text = "Location:";
            // 
            // lblSize
            // 
            lblSize.AutoSize = true;
            lblSize.Location = new System.Drawing.Point(12, 110);
            lblSize.Name = "lblSize";
            lblSize.Size = new System.Drawing.Size(30, 15);
            lblSize.TabIndex = 12;
            lblSize.Text = "Size:";
            toolTip.SetToolTip(lblSize, "This is the actual size of the file or directory.");
            // 
            // lblSizeOnDisk
            // 
            lblSizeOnDisk.AutoSize = true;
            lblSizeOnDisk.Location = new System.Drawing.Point(12, 134);
            lblSizeOnDisk.Name = "lblSizeOnDisk";
            lblSizeOnDisk.Size = new System.Drawing.Size(71, 15);
            lblSizeOnDisk.TabIndex = 14;
            lblSizeOnDisk.Text = "Size on disk:";
            toolTip.SetToolTip(lblSizeOnDisk, "This is the space occupied by the file or directory on the disk\r\ndue to cluster size.");
            // 
            // toolTip
            // 
            toolTip.AutoPopDelay = 10000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 100;
            toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            toolTip.ToolTipTitle = "The more you know...";
            // 
            // lblShortFilename
            // 
            lblShortFilename.AutoSize = true;
            lblShortFilename.Location = new System.Drawing.Point(12, 158);
            lblShortFilename.Name = "lblShortFilename";
            lblShortFilename.Size = new System.Drawing.Size(97, 15);
            lblShortFilename.TabIndex = 16;
            lblShortFilename.Text = "Short name (8.3):";
            toolTip.SetToolTip(lblShortFilename, resources.GetString("lblShortFilename.ToolTip"));
            // 
            // cbxReadOnly
            // 
            cbxReadOnly.AutoSize = true;
            cbxReadOnly.Location = new System.Drawing.Point(80, 351);
            cbxReadOnly.Name = "cbxReadOnly";
            cbxReadOnly.Size = new System.Drawing.Size(80, 19);
            cbxReadOnly.TabIndex = 4;
            cbxReadOnly.Text = "Read-only";
            toolTip.SetToolTip(cbxReadOnly, "Read-only files cannot be written to until this attribute\r\nis cleared.");
            // 
            // cbxHidden
            // 
            cbxHidden.AutoSize = true;
            cbxHidden.Location = new System.Drawing.Point(166, 351);
            cbxHidden.Name = "cbxHidden";
            cbxHidden.Size = new System.Drawing.Size(65, 19);
            cbxHidden.TabIndex = 5;
            cbxHidden.Text = "Hidden";
            toolTip.SetToolTip(cbxHidden, "Hidden files do not show up in regular directory listings and\r\ncannot be moved, deleted, etc. until this attribute is cleared.");
            // 
            // cbxSystem
            // 
            cbxSystem.AutoSize = true;
            cbxSystem.Location = new System.Drawing.Point(240, 351);
            cbxSystem.Name = "cbxSystem";
            cbxSystem.Size = new System.Drawing.Size(64, 19);
            cbxSystem.TabIndex = 6;
            cbxSystem.Text = "System";
            toolTip.SetToolTip(cbxSystem, "System files will not be physically relocated, for example\r\nduring disk defragmentation.");
            // 
            // cbxArchive
            // 
            cbxArchive.AutoSize = true;
            cbxArchive.Location = new System.Drawing.Point(312, 351);
            cbxArchive.Name = "cbxArchive";
            cbxArchive.Size = new System.Drawing.Size(66, 19);
            cbxArchive.TabIndex = 7;
            cbxArchive.Text = "Archive";
            toolTip.SetToolTip(cbxArchive, "Archive files are marked as ready to be backed up by a backup\r\nutility. In practice, this attribute is obsolete.");
            // 
            // dtpAccessed
            // 
            dtpAccessed.CustomFormat = "yyyy-MM-dd";
            dtpAccessed.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpAccessed.Location = new System.Drawing.Point(94, 307);
            dtpAccessed.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            dtpAccessed.Name = "dtpAccessed";
            dtpAccessed.Size = new System.Drawing.Size(278, 23);
            dtpAccessed.TabIndex = 3;
            toolTip.SetToolTip(dtpAccessed, resources.GetString("dtpAccessed.ToolTip"));
            // 
            // dtpCreated
            // 
            dtpCreated.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dtpCreated.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpCreated.Location = new System.Drawing.Point(94, 249);
            dtpCreated.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            dtpCreated.Name = "dtpCreated";
            dtpCreated.Size = new System.Drawing.Size(278, 23);
            dtpCreated.TabIndex = 1;
            toolTip.SetToolTip(dtpCreated, "This is the date and time when the file or directory was originally\r\ncreated. For FAT file systems, only software supporting the VFAT\r\nextensions uses this value.");
            // 
            // dtpModified
            // 
            dtpModified.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dtpModified.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpModified.Location = new System.Drawing.Point(94, 278);
            dtpModified.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            dtpModified.Name = "dtpModified";
            dtpModified.Size = new System.Drawing.Size(278, 23);
            dtpModified.TabIndex = 2;
            toolTip.SetToolTip(dtpModified, "This is the date and time when the file or directory was last\r\nwritten to.");
            // 
            // txtSize1
            // 
            txtSize1.BackColor = System.Drawing.SystemColors.Window;
            txtSize1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtSize1.Location = new System.Drawing.Point(115, 110);
            txtSize1.Name = "txtSize1";
            txtSize1.ReadOnly = true;
            txtSize1.Size = new System.Drawing.Size(255, 16);
            txtSize1.TabIndex = 29;
            txtSize1.TabStop = false;
            txtSize1.Text = "<size>";
            toolTip.SetToolTip(txtSize1, "This is the actual size of the file or directory.");
            // 
            // txtSizeOnDisk1
            // 
            txtSizeOnDisk1.BackColor = System.Drawing.SystemColors.Window;
            txtSizeOnDisk1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtSizeOnDisk1.Location = new System.Drawing.Point(115, 134);
            txtSizeOnDisk1.Name = "txtSizeOnDisk1";
            txtSizeOnDisk1.ReadOnly = true;
            txtSizeOnDisk1.Size = new System.Drawing.Size(255, 16);
            txtSizeOnDisk1.TabIndex = 30;
            txtSizeOnDisk1.TabStop = false;
            txtSizeOnDisk1.Text = "<sizeondisk>";
            toolTip.SetToolTip(txtSizeOnDisk1, "This is the space occupied by the file or directory on the disk\r\ndue to cluster size.");
            // 
            // txtShortFilename1
            // 
            txtShortFilename1.BackColor = System.Drawing.SystemColors.Window;
            txtShortFilename1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtShortFilename1.Location = new System.Drawing.Point(115, 158);
            txtShortFilename1.Name = "txtShortFilename1";
            txtShortFilename1.ReadOnly = true;
            txtShortFilename1.Size = new System.Drawing.Size(255, 16);
            txtShortFilename1.TabIndex = 31;
            txtShortFilename1.TabStop = false;
            txtShortFilename1.Text = "<shortname>";
            toolTip.SetToolTip(txtShortFilename1, resources.GetString("txtShortFilename1.ToolTip"));
            // 
            // txtContains1
            // 
            txtContains1.BackColor = System.Drawing.SystemColors.Window;
            txtContains1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtContains1.Location = new System.Drawing.Point(115, 182);
            txtContains1.Name = "txtContains1";
            txtContains1.ReadOnly = true;
            txtContains1.Size = new System.Drawing.Size(255, 16);
            txtContains1.TabIndex = 33;
            txtContains1.TabStop = false;
            txtContains1.Text = "<contains>";
            toolTip.SetToolTip(txtContains1, "The number of files and subdirectories contained in this directory.\r\n");
            // 
            // lblContains
            // 
            lblContains.AutoSize = true;
            lblContains.Location = new System.Drawing.Point(12, 182);
            lblContains.Name = "lblContains";
            lblContains.Size = new System.Drawing.Size(57, 15);
            lblContains.TabIndex = 32;
            lblContains.Text = "Contains:";
            toolTip.SetToolTip(lblContains, "The number of files and subdirectories contained in this directory.\r\n");
            // 
            // cbxDateCreated
            // 
            cbxDateCreated.AutoSize = true;
            cbxDateCreated.Location = new System.Drawing.Point(12, 251);
            cbxDateCreated.Name = "cbxDateCreated";
            cbxDateCreated.Size = new System.Drawing.Size(70, 19);
            cbxDateCreated.TabIndex = 24;
            cbxDateCreated.Text = "Created:";
            toolTip.SetToolTip(cbxDateCreated, "This is the date and time when the file or directory was originally\r\ncreated. For FAT file systems, only software supporting the VFAT\r\nextensions uses this value.");
            cbxDateCreated.CheckedChanged += cbxDateCreated_CheckedChanged;
            // 
            // cbxDateModified
            // 
            cbxDateModified.AutoSize = true;
            cbxDateModified.Location = new System.Drawing.Point(12, 280);
            cbxDateModified.Name = "cbxDateModified";
            cbxDateModified.Size = new System.Drawing.Size(77, 19);
            cbxDateModified.TabIndex = 25;
            cbxDateModified.Text = "Modified:";
            toolTip.SetToolTip(cbxDateModified, "This is the date and time when the file or directory was last\r\nwritten to.");
            cbxDateModified.CheckedChanged += cbxDateModified_CheckedChanged;
            // 
            // cbxDateAccessed
            // 
            cbxDateAccessed.AutoSize = true;
            cbxDateAccessed.Location = new System.Drawing.Point(12, 309);
            cbxDateAccessed.Name = "cbxDateAccessed";
            cbxDateAccessed.Size = new System.Drawing.Size(78, 19);
            cbxDateAccessed.TabIndex = 26;
            cbxDateAccessed.Text = "Accessed:";
            toolTip.SetToolTip(cbxDateAccessed, resources.GetString("cbxDateAccessed.ToolTip"));
            cbxDateAccessed.CheckedChanged += cbxDateAccessed_CheckedChanged;
            // 
            // txtFirstCluster1
            // 
            txtFirstCluster1.BackColor = System.Drawing.SystemColors.Window;
            txtFirstCluster1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtFirstCluster1.Location = new System.Drawing.Point(115, 206);
            txtFirstCluster1.Name = "txtFirstCluster1";
            txtFirstCluster1.ReadOnly = true;
            txtFirstCluster1.Size = new System.Drawing.Size(255, 16);
            txtFirstCluster1.TabIndex = 35;
            txtFirstCluster1.TabStop = false;
            txtFirstCluster1.Text = "<firstcluster>";
            toolTip.SetToolTip(txtFirstCluster1, "The starting cluster in the cluster chain of this object.\r\n");
            // 
            // lblFirstCluster
            // 
            lblFirstCluster.AutoSize = true;
            lblFirstCluster.Location = new System.Drawing.Point(12, 206);
            lblFirstCluster.Name = "lblFirstCluster";
            lblFirstCluster.Size = new System.Drawing.Size(70, 15);
            lblFirstCluster.TabIndex = 34;
            lblFirstCluster.Text = "First cluster:";
            toolTip.SetToolTip(lblFirstCluster, "The starting cluster in the cluster chain of this object.\r\n");
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(292, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(80, 26);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(206, 12);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 8;
            btnOK.Text = "OK";
            btnOK.Click += btnOK_Click;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 380);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(384, 50);
            pnlBottom.TabIndex = 10;
            // 
            // lblSeparator1
            // 
            lblSeparator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            lblSeparator1.Enabled = false;
            lblSeparator1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblSeparator1.ForeColor = System.Drawing.SystemColors.ControlDark;
            lblSeparator1.Location = new System.Drawing.Point(12, 53);
            lblSeparator1.Name = "lblSeparator1";
            lblSeparator1.Size = new System.Drawing.Size(360, 1);
            lblSeparator1.TabIndex = 11;
            // 
            // lblSeparator2
            // 
            lblSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            lblSeparator2.Enabled = false;
            lblSeparator2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblSeparator2.ForeColor = System.Drawing.SystemColors.ControlDark;
            lblSeparator2.Location = new System.Drawing.Point(12, 236);
            lblSeparator2.Name = "lblSeparator2";
            lblSeparator2.Size = new System.Drawing.Size(360, 1);
            lblSeparator2.TabIndex = 18;
            // 
            // lblSeparator3
            // 
            lblSeparator3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            lblSeparator3.Enabled = false;
            lblSeparator3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblSeparator3.ForeColor = System.Drawing.SystemColors.ControlDark;
            lblSeparator3.Location = new System.Drawing.Point(12, 341);
            lblSeparator3.Name = "lblSeparator3";
            lblSeparator3.Size = new System.Drawing.Size(360, 1);
            lblSeparator3.TabIndex = 19;
            // 
            // lblAttributes
            // 
            lblAttributes.AutoSize = true;
            lblAttributes.Location = new System.Drawing.Point(12, 353);
            lblAttributes.Name = "lblAttributes";
            lblAttributes.Size = new System.Drawing.Size(62, 15);
            lblAttributes.TabIndex = 23;
            lblAttributes.Text = "Attributes:";
            // 
            // txtType1
            // 
            txtType1.BackColor = System.Drawing.SystemColors.Window;
            txtType1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtType1.Location = new System.Drawing.Point(115, 62);
            txtType1.Name = "txtType1";
            txtType1.ReadOnly = true;
            txtType1.Size = new System.Drawing.Size(255, 16);
            txtType1.TabIndex = 27;
            txtType1.TabStop = false;
            txtType1.Text = "<type>";
            // 
            // txtLocation1
            // 
            txtLocation1.BackColor = System.Drawing.SystemColors.Window;
            txtLocation1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtLocation1.Location = new System.Drawing.Point(115, 85);
            txtLocation1.Name = "txtLocation1";
            txtLocation1.ReadOnly = true;
            txtLocation1.Size = new System.Drawing.Size(255, 16);
            txtLocation1.TabIndex = 28;
            txtLocation1.TabStop = false;
            txtLocation1.Text = "<location>";
            // 
            // lblMultipleObjectsCount
            // 
            lblMultipleObjectsCount.AutoSize = true;
            lblMultipleObjectsCount.Location = new System.Drawing.Point(52, 20);
            lblMultipleObjectsCount.Name = "lblMultipleObjectsCount";
            lblMultipleObjectsCount.Size = new System.Drawing.Size(117, 15);
            lblMultipleObjectsCount.TabIndex = 36;
            lblMultipleObjectsCount.Text = "Files: X, directories: Y";
            lblMultipleObjectsCount.Visible = false;
            // 
            // dlgProperties
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(384, 430);
            Controls.Add(dtpModified);
            Controls.Add(lblMultipleObjectsCount);
            Controls.Add(txtFirstCluster1);
            Controls.Add(lblFirstCluster);
            Controls.Add(txtContains1);
            Controls.Add(lblContains);
            Controls.Add(txtShortFilename1);
            Controls.Add(txtSizeOnDisk1);
            Controls.Add(txtSize1);
            Controls.Add(txtLocation1);
            Controls.Add(txtType1);
            Controls.Add(cbxArchive);
            Controls.Add(cbxSystem);
            Controls.Add(cbxHidden);
            Controls.Add(dtpAccessed);
            Controls.Add(cbxDateAccessed);
            Controls.Add(cbxDateModified);
            Controls.Add(dtpCreated);
            Controls.Add(cbxDateCreated);
            Controls.Add(lblAttributes);
            Controls.Add(lblSeparator3);
            Controls.Add(cbxReadOnly);
            Controls.Add(lblSeparator2);
            Controls.Add(lblType);
            Controls.Add(lblSeparator1);
            Controls.Add(pnlBottom);
            Controls.Add(lblShortFilename);
            Controls.Add(lblLocation);
            Controls.Add(txtFilename);
            Controls.Add(lblSizeOnDisk);
            Controls.Add(imgIcon);
            Controls.Add(lblSize);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgProperties";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Properties";
            Load += dlgProperties_Load;
            ((System.ComponentModel.ISupportInitialize)imgIcon).EndInit();
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.PictureBox imgIcon;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label lblSizeOnDisk;
        private System.Windows.Forms.Label lblShortFilename;
        private System.Windows.Forms.CheckBox cbxArchive;
        private System.Windows.Forms.CheckBox cbxSystem;
        private System.Windows.Forms.CheckBox cbxHidden;
        private System.Windows.Forms.CheckBox cbxReadOnly;
        private System.Windows.Forms.DateTimePicker dtpCreated;
        private System.Windows.Forms.DateTimePicker dtpModified;
        private System.Windows.Forms.DateTimePicker dtpAccessed;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblSeparator1;
        private System.Windows.Forms.Label lblSeparator2;
        private System.Windows.Forms.Label lblSeparator3;
        private System.Windows.Forms.Label lblAttributes;
        private System.Windows.Forms.CheckBox cbxDateCreated;
        private System.Windows.Forms.CheckBox cbxDateModified;
        private System.Windows.Forms.CheckBox cbxDateAccessed;
        private System.Windows.Forms.TextBox txtType1;
        private System.Windows.Forms.TextBox txtLocation1;
        private System.Windows.Forms.TextBox txtSize1;
        private System.Windows.Forms.TextBox txtSizeOnDisk1;
        private System.Windows.Forms.TextBox txtShortFilename1;
        private System.Windows.Forms.TextBox txtContains1;
        private System.Windows.Forms.Label lblContains;
        private System.Windows.Forms.TextBox txtFirstCluster1;
        private System.Windows.Forms.Label lblFirstCluster;
        private System.Windows.Forms.Label lblMultipleObjectsCount;
    }
}
