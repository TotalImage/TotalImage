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
            txtSize = new System.Windows.Forms.TextBox();
            txtSizeOnDisk = new System.Windows.Forms.TextBox();
            txtShortFilename = new System.Windows.Forms.TextBox();
            txtContains = new System.Windows.Forms.TextBox();
            lblContains = new System.Windows.Forms.Label();
            cbxDateCreated = new System.Windows.Forms.CheckBox();
            cbxDateModified = new System.Windows.Forms.CheckBox();
            cbxDateAccessed = new System.Windows.Forms.CheckBox();
            txtFirstCluster = new System.Windows.Forms.TextBox();
            lblFirstCluster = new System.Windows.Forms.Label();
            txtHashSHA1 = new System.Windows.Forms.TextBox();
            lblHashSHA1 = new System.Windows.Forms.Label();
            txtHashMD5 = new System.Windows.Forms.TextBox();
            lblHashMD5 = new System.Windows.Forms.Label();
            btnCancel = new System.Windows.Forms.Button();
            btnOK = new System.Windows.Forms.Button();
            pnlBottom = new System.Windows.Forms.Panel();
            lblSeparator1 = new System.Windows.Forms.Label();
            lblSeparator2 = new System.Windows.Forms.Label();
            lblSeparator3 = new System.Windows.Forms.Label();
            lblAttributes = new System.Windows.Forms.Label();
            txtType = new System.Windows.Forms.TextBox();
            txtLocation = new System.Windows.Forms.TextBox();
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
            lblType.Size = new System.Drawing.Size(34, 15);
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
            cbxReadOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxReadOnly.Location = new System.Drawing.Point(83, 399);
            cbxReadOnly.Name = "cbxReadOnly";
            cbxReadOnly.Size = new System.Drawing.Size(86, 20);
            cbxReadOnly.TabIndex = 4;
            cbxReadOnly.Text = "Read-only";
            toolTip.SetToolTip(cbxReadOnly, "Read-only files cannot be written to until this attribute\r\nis cleared.");
            cbxReadOnly.UseVisualStyleBackColor = true;
            // 
            // cbxHidden
            // 
            cbxHidden.AutoSize = true;
            cbxHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxHidden.Location = new System.Drawing.Point(169, 399);
            cbxHidden.Name = "cbxHidden";
            cbxHidden.Size = new System.Drawing.Size(71, 20);
            cbxHidden.TabIndex = 5;
            cbxHidden.Text = "Hidden";
            toolTip.SetToolTip(cbxHidden, "Hidden files do not show up in regular directory listings and\r\ncannot be moved, deleted, etc. until this attribute is cleared.");
            cbxHidden.UseVisualStyleBackColor = true;
            // 
            // cbxSystem
            // 
            cbxSystem.AutoSize = true;
            cbxSystem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxSystem.Location = new System.Drawing.Point(243, 399);
            cbxSystem.Name = "cbxSystem";
            cbxSystem.Size = new System.Drawing.Size(70, 20);
            cbxSystem.TabIndex = 6;
            cbxSystem.Text = "System";
            toolTip.SetToolTip(cbxSystem, "System files will not be physically relocated, for example\r\nduring disk defragmentation.");
            cbxSystem.UseVisualStyleBackColor = true;
            // 
            // cbxArchive
            // 
            cbxArchive.AutoSize = true;
            cbxArchive.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxArchive.Location = new System.Drawing.Point(315, 399);
            cbxArchive.Name = "cbxArchive";
            cbxArchive.Size = new System.Drawing.Size(72, 20);
            cbxArchive.TabIndex = 7;
            cbxArchive.Text = "Archive";
            toolTip.SetToolTip(cbxArchive, "Archive files are marked as ready to be backed up by a backup\r\nutility. In practice, this attribute is obsolete.");
            cbxArchive.UseVisualStyleBackColor = true;
            // 
            // dtpAccessed
            // 
            dtpAccessed.CustomFormat = "yyyy-MM-dd";
            dtpAccessed.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpAccessed.Location = new System.Drawing.Point(97, 355);
            dtpAccessed.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            dtpAccessed.Name = "dtpAccessed";
            dtpAccessed.ShowUpDown = true;
            dtpAccessed.Size = new System.Drawing.Size(278, 23);
            dtpAccessed.TabIndex = 3;
            toolTip.SetToolTip(dtpAccessed, resources.GetString("dtpAccessed.ToolTip"));
            // 
            // dtpCreated
            // 
            dtpCreated.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dtpCreated.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpCreated.Location = new System.Drawing.Point(97, 297);
            dtpCreated.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            dtpCreated.Name = "dtpCreated";
            dtpCreated.ShowUpDown = true;
            dtpCreated.Size = new System.Drawing.Size(278, 23);
            dtpCreated.TabIndex = 1;
            toolTip.SetToolTip(dtpCreated, "This is the date and time when the file or directory was originally\r\ncreated. For FAT file systems, only software supporting the VFAT\r\nextensions uses this value.");
            // 
            // dtpModified
            // 
            dtpModified.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dtpModified.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpModified.Location = new System.Drawing.Point(97, 326);
            dtpModified.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            dtpModified.Name = "dtpModified";
            dtpModified.ShowUpDown = true;
            dtpModified.Size = new System.Drawing.Size(278, 23);
            dtpModified.TabIndex = 2;
            toolTip.SetToolTip(dtpModified, "This is the date and time when the file or directory was last\r\nwritten to.");
            // 
            // txtSize
            // 
            txtSize.BackColor = System.Drawing.Color.White;
            txtSize.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtSize.Location = new System.Drawing.Point(115, 110);
            txtSize.Name = "txtSize";
            txtSize.ReadOnly = true;
            txtSize.Size = new System.Drawing.Size(255, 16);
            txtSize.TabIndex = 29;
            txtSize.TabStop = false;
            txtSize.Text = "<size>";
            toolTip.SetToolTip(txtSize, "This is the actual size of the file or directory.");
            // 
            // txtSizeOnDisk
            // 
            txtSizeOnDisk.BackColor = System.Drawing.Color.White;
            txtSizeOnDisk.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtSizeOnDisk.Location = new System.Drawing.Point(115, 134);
            txtSizeOnDisk.Name = "txtSizeOnDisk";
            txtSizeOnDisk.ReadOnly = true;
            txtSizeOnDisk.Size = new System.Drawing.Size(255, 16);
            txtSizeOnDisk.TabIndex = 30;
            txtSizeOnDisk.TabStop = false;
            txtSizeOnDisk.Text = "<sizeondisk>";
            toolTip.SetToolTip(txtSizeOnDisk, "This is the space occupied by the file or directory on the disk\r\ndue to cluster size.");
            // 
            // txtShortFilename
            // 
            txtShortFilename.BackColor = System.Drawing.Color.White;
            txtShortFilename.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtShortFilename.Location = new System.Drawing.Point(115, 158);
            txtShortFilename.Name = "txtShortFilename";
            txtShortFilename.ReadOnly = true;
            txtShortFilename.Size = new System.Drawing.Size(255, 16);
            txtShortFilename.TabIndex = 31;
            txtShortFilename.TabStop = false;
            txtShortFilename.Text = "<shortname>";
            toolTip.SetToolTip(txtShortFilename, resources.GetString("txtShortFilename.ToolTip"));
            // 
            // txtContains
            // 
            txtContains.BackColor = System.Drawing.Color.White;
            txtContains.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtContains.Location = new System.Drawing.Point(115, 182);
            txtContains.Name = "txtContains";
            txtContains.ReadOnly = true;
            txtContains.Size = new System.Drawing.Size(255, 16);
            txtContains.TabIndex = 33;
            txtContains.TabStop = false;
            txtContains.Text = "<contains>";
            toolTip.SetToolTip(txtContains, "The number of files and subdirectories contained in this directory.\r\n");
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
            cbxDateCreated.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxDateCreated.Location = new System.Drawing.Point(15, 299);
            cbxDateCreated.Name = "cbxDateCreated";
            cbxDateCreated.Size = new System.Drawing.Size(76, 20);
            cbxDateCreated.TabIndex = 24;
            cbxDateCreated.Text = "Created:";
            toolTip.SetToolTip(cbxDateCreated, "This is the date and time when the file or directory was originally\r\ncreated. For FAT file systems, only software supporting the VFAT\r\nextensions uses this value.");
            cbxDateCreated.UseVisualStyleBackColor = true;
            cbxDateCreated.CheckedChanged += cbxDateCreated_CheckedChanged;
            // 
            // cbxDateModified
            // 
            cbxDateModified.AutoSize = true;
            cbxDateModified.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxDateModified.Location = new System.Drawing.Point(15, 328);
            cbxDateModified.Name = "cbxDateModified";
            cbxDateModified.Size = new System.Drawing.Size(83, 20);
            cbxDateModified.TabIndex = 25;
            cbxDateModified.Text = "Modified:";
            toolTip.SetToolTip(cbxDateModified, "This is the date and time when the file or directory was last\r\nwritten to.");
            cbxDateModified.UseVisualStyleBackColor = true;
            cbxDateModified.CheckedChanged += cbxDateModified_CheckedChanged;
            // 
            // cbxDateAccessed
            // 
            cbxDateAccessed.AutoSize = true;
            cbxDateAccessed.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxDateAccessed.Location = new System.Drawing.Point(15, 357);
            cbxDateAccessed.Name = "cbxDateAccessed";
            cbxDateAccessed.Size = new System.Drawing.Size(84, 20);
            cbxDateAccessed.TabIndex = 26;
            cbxDateAccessed.Text = "Accessed:";
            toolTip.SetToolTip(cbxDateAccessed, resources.GetString("cbxDateAccessed.ToolTip"));
            cbxDateAccessed.UseVisualStyleBackColor = true;
            cbxDateAccessed.CheckedChanged += cbxDateAccessed_CheckedChanged;
            // 
            // txtFirstCluster
            // 
            txtFirstCluster.BackColor = System.Drawing.Color.White;
            txtFirstCluster.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtFirstCluster.Location = new System.Drawing.Point(115, 206);
            txtFirstCluster.Name = "txtFirstCluster";
            txtFirstCluster.ReadOnly = true;
            txtFirstCluster.Size = new System.Drawing.Size(255, 16);
            txtFirstCluster.TabIndex = 35;
            txtFirstCluster.TabStop = false;
            txtFirstCluster.Text = "<firstcluster>";
            toolTip.SetToolTip(txtFirstCluster, "The starting cluster in the cluster chain of this object.\r\n");
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
            // txtHashSHA1
            // 
            txtHashSHA1.BackColor = System.Drawing.Color.White;
            txtHashSHA1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtHashSHA1.Location = new System.Drawing.Point(115, 254);
            txtHashSHA1.Name = "txtHashSHA1";
            txtHashSHA1.ReadOnly = true;
            txtHashSHA1.Size = new System.Drawing.Size(255, 16);
            txtHashSHA1.TabIndex = 40;
            txtHashSHA1.TabStop = false;
            txtHashSHA1.Text = "<SHA1 hash>";
            toolTip.SetToolTip(txtHashSHA1, "The SHA-1 hash of this file.\r\n");
            txtHashSHA1.Click += txtHash_Click;
            // 
            // lblHashSHA1
            // 
            lblHashSHA1.AutoSize = true;
            lblHashSHA1.Location = new System.Drawing.Point(12, 254);
            lblHashSHA1.Name = "lblHashSHA1";
            lblHashSHA1.Size = new System.Drawing.Size(72, 15);
            lblHashSHA1.TabIndex = 39;
            lblHashSHA1.Text = "SHA-1 hash:";
            toolTip.SetToolTip(lblHashSHA1, "The SHA-1 hash of this file.");
            // 
            // txtHashMD5
            // 
            txtHashMD5.BackColor = System.Drawing.Color.White;
            txtHashMD5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtHashMD5.Location = new System.Drawing.Point(115, 230);
            txtHashMD5.Name = "txtHashMD5";
            txtHashMD5.ReadOnly = true;
            txtHashMD5.Size = new System.Drawing.Size(255, 16);
            txtHashMD5.TabIndex = 38;
            txtHashMD5.TabStop = false;
            txtHashMD5.Text = "<MD5 hash>";
            toolTip.SetToolTip(txtHashMD5, "The MD5 hash of this file.\r\n");
            txtHashMD5.Click += txtHash_Click;
            // 
            // lblHashMD5
            // 
            lblHashMD5.AutoSize = true;
            lblHashMD5.Location = new System.Drawing.Point(12, 230);
            lblHashMD5.Name = "lblHashMD5";
            lblHashMD5.Size = new System.Drawing.Size(63, 15);
            lblHashMD5.TabIndex = 37;
            lblHashMD5.Text = "MD5 hash:";
            toolTip.SetToolTip(lblHashMD5, "The MD5 hash of this file.\r\n");
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnCancel.Location = new System.Drawing.Point(292, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(80, 26);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnOK.Location = new System.Drawing.Point(206, 12);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 8;
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
            pnlBottom.Location = new System.Drawing.Point(0, 425);
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
            lblSeparator2.Location = new System.Drawing.Point(15, 284);
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
            lblSeparator3.Location = new System.Drawing.Point(15, 389);
            lblSeparator3.Name = "lblSeparator3";
            lblSeparator3.Size = new System.Drawing.Size(360, 1);
            lblSeparator3.TabIndex = 19;
            // 
            // lblAttributes
            // 
            lblAttributes.AutoSize = true;
            lblAttributes.Location = new System.Drawing.Point(15, 401);
            lblAttributes.Name = "lblAttributes";
            lblAttributes.Size = new System.Drawing.Size(62, 15);
            lblAttributes.TabIndex = 23;
            lblAttributes.Text = "Attributes:";
            // 
            // txtType
            // 
            txtType.BackColor = System.Drawing.Color.White;
            txtType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtType.Location = new System.Drawing.Point(115, 62);
            txtType.Name = "txtType";
            txtType.ReadOnly = true;
            txtType.Size = new System.Drawing.Size(255, 16);
            txtType.TabIndex = 27;
            txtType.TabStop = false;
            txtType.Text = "<type>";
            // 
            // txtLocation
            // 
            txtLocation.BackColor = System.Drawing.Color.White;
            txtLocation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtLocation.Location = new System.Drawing.Point(115, 85);
            txtLocation.Name = "txtLocation";
            txtLocation.ReadOnly = true;
            txtLocation.Size = new System.Drawing.Size(255, 16);
            txtLocation.TabIndex = 28;
            txtLocation.TabStop = false;
            txtLocation.Text = "<location>";
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
            ClientSize = new System.Drawing.Size(384, 475);
            Controls.Add(txtHashSHA1);
            Controls.Add(lblHashSHA1);
            Controls.Add(txtHashMD5);
            Controls.Add(lblHashMD5);
            Controls.Add(dtpModified);
            Controls.Add(lblMultipleObjectsCount);
            Controls.Add(txtFirstCluster);
            Controls.Add(lblFirstCluster);
            Controls.Add(txtContains);
            Controls.Add(lblContains);
            Controls.Add(txtShortFilename);
            Controls.Add(txtSizeOnDisk);
            Controls.Add(txtSize);
            Controls.Add(txtLocation);
            Controls.Add(txtType);
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
            FormClosing += dlgProperties_FormClosing;
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
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.TextBox txtSizeOnDisk;
        private System.Windows.Forms.TextBox txtShortFilename;
        private System.Windows.Forms.TextBox txtContains;
        private System.Windows.Forms.Label lblContains;
        private System.Windows.Forms.TextBox txtFirstCluster;
        private System.Windows.Forms.Label lblFirstCluster;
        private System.Windows.Forms.Label lblMultipleObjectsCount;
        private System.Windows.Forms.TextBox txtHashSHA1;
        private System.Windows.Forms.Label lblHashSHA1;
        private System.Windows.Forms.TextBox txtHashMD5;
        private System.Windows.Forms.Label lblHashMD5;
    }
}
