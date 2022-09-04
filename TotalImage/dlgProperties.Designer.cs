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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgProperties));
            this.imgIcon = new System.Windows.Forms.PictureBox();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblSizeOnDisk = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblShortFilename = new System.Windows.Forms.Label();
            this.cbxReadOnly = new System.Windows.Forms.CheckBox();
            this.cbxHidden = new System.Windows.Forms.CheckBox();
            this.cbxSystem = new System.Windows.Forms.CheckBox();
            this.cbxArchive = new System.Windows.Forms.CheckBox();
            this.dtpAccessed = new System.Windows.Forms.DateTimePicker();
            this.dtpCreated = new System.Windows.Forms.DateTimePicker();
            this.dtpModified = new System.Windows.Forms.DateTimePicker();
            this.txtSize1 = new System.Windows.Forms.TextBox();
            this.txtSizeOnDisk1 = new System.Windows.Forms.TextBox();
            this.txtShortFilename1 = new System.Windows.Forms.TextBox();
            this.txtContains1 = new System.Windows.Forms.TextBox();
            this.lblContains = new System.Windows.Forms.Label();
            this.cbxDateCreated = new System.Windows.Forms.CheckBox();
            this.cbxDateModified = new System.Windows.Forms.CheckBox();
            this.cbxDateAccessed = new System.Windows.Forms.CheckBox();
            this.txtFirstCluster1 = new System.Windows.Forms.TextBox();
            this.lblFirstCluster = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblSeparator1 = new System.Windows.Forms.Label();
            this.lblSeparator2 = new System.Windows.Forms.Label();
            this.lblSeparator3 = new System.Windows.Forms.Label();
            this.lblAttributes = new System.Windows.Forms.Label();
            this.txtType1 = new System.Windows.Forms.TextBox();
            this.txtLocation1 = new System.Windows.Forms.TextBox();
            this.lblMultipleObjectsCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imgIcon)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgIcon
            // 
            this.imgIcon.Location = new System.Drawing.Point(15, 15);
            this.imgIcon.Margin = new System.Windows.Forms.Padding(4);
            this.imgIcon.Name = "imgIcon";
            this.imgIcon.Size = new System.Drawing.Size(40, 40);
            this.imgIcon.TabIndex = 6;
            this.imgIcon.TabStop = false;
            // 
            // txtFilename
            // 
            this.txtFilename.Location = new System.Drawing.Point(62, 20);
            this.txtFilename.Margin = new System.Windows.Forms.Padding(4);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(402, 27);
            this.txtFilename.TabIndex = 0;
            this.txtFilename.Text = "<filename>";
            this.toolTip.SetToolTip(this.txtFilename, resources.GetString("txtFilename.ToolTip"));
            this.txtFilename.TextChanged += new System.EventHandler(this.txtFilename_TextChanged);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(15, 78);
            this.lblType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(43, 20);
            this.lblType.TabIndex = 8;
            this.lblType.Text = "Type:";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(15, 108);
            this.lblLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(69, 20);
            this.lblLocation.TabIndex = 10;
            this.lblLocation.Text = "Location:";
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(15, 138);
            this.lblSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(39, 20);
            this.lblSize.TabIndex = 12;
            this.lblSize.Text = "Size:";
            this.toolTip.SetToolTip(this.lblSize, "This is the actual size of the file or directory.");
            // 
            // lblSizeOnDisk
            // 
            this.lblSizeOnDisk.AutoSize = true;
            this.lblSizeOnDisk.Location = new System.Drawing.Point(15, 168);
            this.lblSizeOnDisk.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSizeOnDisk.Name = "lblSizeOnDisk";
            this.lblSizeOnDisk.Size = new System.Drawing.Size(90, 20);
            this.lblSizeOnDisk.TabIndex = 14;
            this.lblSizeOnDisk.Text = "Size on disk:";
            this.toolTip.SetToolTip(this.lblSizeOnDisk, "This is the space occupied by the file or directory on the disk\r\ndue to cluster s" +
        "ize.");
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 10000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "The more you know...";
            // 
            // lblShortFilename
            // 
            this.lblShortFilename.AutoSize = true;
            this.lblShortFilename.Location = new System.Drawing.Point(15, 198);
            this.lblShortFilename.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblShortFilename.Name = "lblShortFilename";
            this.lblShortFilename.Size = new System.Drawing.Size(121, 20);
            this.lblShortFilename.TabIndex = 16;
            this.lblShortFilename.Text = "Short name (8.3):";
            this.toolTip.SetToolTip(this.lblShortFilename, resources.GetString("lblShortFilename.ToolTip"));
            // 
            // cbxReadOnly
            // 
            this.cbxReadOnly.AutoSize = true;
            this.cbxReadOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxReadOnly.Location = new System.Drawing.Point(100, 439);
            this.cbxReadOnly.Margin = new System.Windows.Forms.Padding(4);
            this.cbxReadOnly.Name = "cbxReadOnly";
            this.cbxReadOnly.Size = new System.Drawing.Size(108, 25);
            this.cbxReadOnly.TabIndex = 4;
            this.cbxReadOnly.Text = "Read-only";
            this.toolTip.SetToolTip(this.cbxReadOnly, "Read-only files cannot be written to until this attribute\r\nis cleared.");
            this.cbxReadOnly.UseVisualStyleBackColor = true;
            // 
            // cbxHidden
            // 
            this.cbxHidden.AutoSize = true;
            this.cbxHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxHidden.Location = new System.Drawing.Point(208, 439);
            this.cbxHidden.Margin = new System.Windows.Forms.Padding(4);
            this.cbxHidden.Name = "cbxHidden";
            this.cbxHidden.Size = new System.Drawing.Size(89, 25);
            this.cbxHidden.TabIndex = 5;
            this.cbxHidden.Text = "Hidden";
            this.toolTip.SetToolTip(this.cbxHidden, "Hidden files do not show up in regular directory listings and\r\ncannot be moved, d" +
        "eleted, etc. until this attribute is cleared.");
            this.cbxHidden.UseVisualStyleBackColor = true;
            // 
            // cbxSystem
            // 
            this.cbxSystem.AutoSize = true;
            this.cbxSystem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxSystem.Location = new System.Drawing.Point(300, 439);
            this.cbxSystem.Margin = new System.Windows.Forms.Padding(4);
            this.cbxSystem.Name = "cbxSystem";
            this.cbxSystem.Size = new System.Drawing.Size(87, 25);
            this.cbxSystem.TabIndex = 6;
            this.cbxSystem.Text = "System";
            this.toolTip.SetToolTip(this.cbxSystem, "System files will not be physically relocated, for example\r\nduring disk defragmen" +
        "tation.");
            this.cbxSystem.UseVisualStyleBackColor = true;
            // 
            // cbxArchive
            // 
            this.cbxArchive.AutoSize = true;
            this.cbxArchive.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxArchive.Location = new System.Drawing.Point(390, 439);
            this.cbxArchive.Margin = new System.Windows.Forms.Padding(4);
            this.cbxArchive.Name = "cbxArchive";
            this.cbxArchive.Size = new System.Drawing.Size(89, 25);
            this.cbxArchive.TabIndex = 7;
            this.cbxArchive.Text = "Archive";
            this.toolTip.SetToolTip(this.cbxArchive, "Archive files are marked as ready to be backed up by a backup\r\nutility. In practi" +
        "ce, this attribute is obsolete.");
            this.cbxArchive.UseVisualStyleBackColor = true;
            // 
            // dtpAccessed
            // 
            this.dtpAccessed.CustomFormat = "yyyy-MM-dd";
            this.dtpAccessed.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpAccessed.Location = new System.Drawing.Point(118, 384);
            this.dtpAccessed.Margin = new System.Windows.Forms.Padding(4);
            this.dtpAccessed.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            this.dtpAccessed.Name = "dtpAccessed";
            this.dtpAccessed.ShowUpDown = true;
            this.dtpAccessed.Size = new System.Drawing.Size(346, 27);
            this.dtpAccessed.TabIndex = 3;
            this.toolTip.SetToolTip(this.dtpAccessed, resources.GetString("dtpAccessed.ToolTip"));
            // 
            // dtpCreated
            // 
            this.dtpCreated.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpCreated.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCreated.Location = new System.Drawing.Point(118, 311);
            this.dtpCreated.Margin = new System.Windows.Forms.Padding(4);
            this.dtpCreated.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            this.dtpCreated.Name = "dtpCreated";
            this.dtpCreated.ShowUpDown = true;
            this.dtpCreated.Size = new System.Drawing.Size(346, 27);
            this.dtpCreated.TabIndex = 1;
            this.toolTip.SetToolTip(this.dtpCreated, "This is the date and time when the file or directory was originally\r\ncreated. For" +
        " FAT file systems, only software supporting the VFAT\r\nextensions uses this value" +
        ".");
            // 
            // dtpModified
            // 
            this.dtpModified.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpModified.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpModified.Location = new System.Drawing.Point(118, 347);
            this.dtpModified.Margin = new System.Windows.Forms.Padding(4);
            this.dtpModified.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            this.dtpModified.Name = "dtpModified";
            this.dtpModified.ShowUpDown = true;
            this.dtpModified.Size = new System.Drawing.Size(346, 27);
            this.dtpModified.TabIndex = 2;
            this.toolTip.SetToolTip(this.dtpModified, "This is the date and time when the file or directory was last\r\nwritten to.");
            // 
            // txtSize1
            // 
            this.txtSize1.BackColor = System.Drawing.Color.White;
            this.txtSize1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSize1.Location = new System.Drawing.Point(144, 138);
            this.txtSize1.Margin = new System.Windows.Forms.Padding(4);
            this.txtSize1.Name = "txtSize1";
            this.txtSize1.ReadOnly = true;
            this.txtSize1.Size = new System.Drawing.Size(319, 20);
            this.txtSize1.TabIndex = 29;
            this.txtSize1.TabStop = false;
            this.txtSize1.Text = "<size>";
            this.toolTip.SetToolTip(this.txtSize1, "This is the actual size of the file or directory.");
            // 
            // txtSizeOnDisk1
            // 
            this.txtSizeOnDisk1.BackColor = System.Drawing.Color.White;
            this.txtSizeOnDisk1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSizeOnDisk1.Location = new System.Drawing.Point(144, 168);
            this.txtSizeOnDisk1.Margin = new System.Windows.Forms.Padding(4);
            this.txtSizeOnDisk1.Name = "txtSizeOnDisk1";
            this.txtSizeOnDisk1.ReadOnly = true;
            this.txtSizeOnDisk1.Size = new System.Drawing.Size(319, 20);
            this.txtSizeOnDisk1.TabIndex = 30;
            this.txtSizeOnDisk1.TabStop = false;
            this.txtSizeOnDisk1.Text = "<sizeondisk>";
            this.toolTip.SetToolTip(this.txtSizeOnDisk1, "This is the space occupied by the file or directory on the disk\r\ndue to cluster s" +
        "ize.");
            // 
            // txtShortFilename1
            // 
            this.txtShortFilename1.BackColor = System.Drawing.Color.White;
            this.txtShortFilename1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtShortFilename1.Location = new System.Drawing.Point(144, 198);
            this.txtShortFilename1.Margin = new System.Windows.Forms.Padding(4);
            this.txtShortFilename1.Name = "txtShortFilename1";
            this.txtShortFilename1.ReadOnly = true;
            this.txtShortFilename1.Size = new System.Drawing.Size(319, 20);
            this.txtShortFilename1.TabIndex = 31;
            this.txtShortFilename1.TabStop = false;
            this.txtShortFilename1.Text = "<shortname>";
            this.toolTip.SetToolTip(this.txtShortFilename1, resources.GetString("txtShortFilename1.ToolTip"));
            // 
            // txtContains1
            // 
            this.txtContains1.BackColor = System.Drawing.Color.White;
            this.txtContains1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtContains1.Location = new System.Drawing.Point(144, 228);
            this.txtContains1.Margin = new System.Windows.Forms.Padding(4);
            this.txtContains1.Name = "txtContains1";
            this.txtContains1.ReadOnly = true;
            this.txtContains1.Size = new System.Drawing.Size(319, 20);
            this.txtContains1.TabIndex = 33;
            this.txtContains1.TabStop = false;
            this.txtContains1.Text = "<contains>";
            this.toolTip.SetToolTip(this.txtContains1, "The number of files and subdirectories contained in this directory.\r\n");
            // 
            // lblContains
            // 
            this.lblContains.AutoSize = true;
            this.lblContains.Location = new System.Drawing.Point(15, 228);
            this.lblContains.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblContains.Name = "lblContains";
            this.lblContains.Size = new System.Drawing.Size(69, 20);
            this.lblContains.TabIndex = 32;
            this.lblContains.Text = "Contains:";
            this.toolTip.SetToolTip(this.lblContains, "The number of files and subdirectories contained in this directory.\r\n");
            // 
            // cbxDateCreated
            // 
            this.cbxDateCreated.AutoSize = true;
            this.cbxDateCreated.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxDateCreated.Location = new System.Drawing.Point(15, 314);
            this.cbxDateCreated.Margin = new System.Windows.Forms.Padding(4);
            this.cbxDateCreated.Name = "cbxDateCreated";
            this.cbxDateCreated.Size = new System.Drawing.Size(95, 25);
            this.cbxDateCreated.TabIndex = 24;
            this.cbxDateCreated.Text = "Created:";
            this.toolTip.SetToolTip(this.cbxDateCreated, "This is the date and time when the file or directory was originally\r\ncreated. For" +
        " FAT file systems, only software supporting the VFAT\r\nextensions uses this value" +
        ".");
            this.cbxDateCreated.UseVisualStyleBackColor = true;
            this.cbxDateCreated.CheckedChanged += new System.EventHandler(this.cbxDateCreated_CheckedChanged);
            // 
            // cbxDateModified
            // 
            this.cbxDateModified.AutoSize = true;
            this.cbxDateModified.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxDateModified.Location = new System.Drawing.Point(15, 350);
            this.cbxDateModified.Margin = new System.Windows.Forms.Padding(4);
            this.cbxDateModified.Name = "cbxDateModified";
            this.cbxDateModified.Size = new System.Drawing.Size(101, 25);
            this.cbxDateModified.TabIndex = 25;
            this.cbxDateModified.Text = "Modified";
            this.toolTip.SetToolTip(this.cbxDateModified, "This is the date and time when the file or directory was last\r\nwritten to.");
            this.cbxDateModified.UseVisualStyleBackColor = true;
            this.cbxDateModified.CheckedChanged += new System.EventHandler(this.cbxDateModified_CheckedChanged);
            // 
            // cbxDateAccessed
            // 
            this.cbxDateAccessed.AutoSize = true;
            this.cbxDateAccessed.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxDateAccessed.Location = new System.Drawing.Point(15, 386);
            this.cbxDateAccessed.Margin = new System.Windows.Forms.Padding(4);
            this.cbxDateAccessed.Name = "cbxDateAccessed";
            this.cbxDateAccessed.Size = new System.Drawing.Size(104, 25);
            this.cbxDateAccessed.TabIndex = 26;
            this.cbxDateAccessed.Text = "Accessed:";
            this.toolTip.SetToolTip(this.cbxDateAccessed, resources.GetString("cbxDateAccessed.ToolTip"));
            this.cbxDateAccessed.UseVisualStyleBackColor = true;
            this.cbxDateAccessed.CheckedChanged += new System.EventHandler(this.cbxDateAccessed_CheckedChanged);
            // 
            // txtFirstCluster1
            // 
            this.txtFirstCluster1.BackColor = System.Drawing.Color.White;
            this.txtFirstCluster1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFirstCluster1.Location = new System.Drawing.Point(144, 258);
            this.txtFirstCluster1.Margin = new System.Windows.Forms.Padding(4);
            this.txtFirstCluster1.Name = "txtFirstCluster1";
            this.txtFirstCluster1.ReadOnly = true;
            this.txtFirstCluster1.Size = new System.Drawing.Size(319, 20);
            this.txtFirstCluster1.TabIndex = 35;
            this.txtFirstCluster1.TabStop = false;
            this.txtFirstCluster1.Text = "<firstcluster>";
            this.toolTip.SetToolTip(this.txtFirstCluster1, "The starting cluster in the cluster chain of this object.\r\n");
            // 
            // lblFirstCluster
            // 
            this.lblFirstCluster.AutoSize = true;
            this.lblFirstCluster.Location = new System.Drawing.Point(15, 258);
            this.lblFirstCluster.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFirstCluster.Name = "lblFirstCluster";
            this.lblFirstCluster.Size = new System.Drawing.Size(86, 20);
            this.lblFirstCluster.TabIndex = 34;
            this.lblFirstCluster.Text = "First cluster:";
            this.toolTip.SetToolTip(this.lblFirstCluster, "The starting cluster in the cluster chain of this object.\r\n");
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(365, 15);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 32);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(258, 15);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 32);
            this.btnOK.TabIndex = 8;
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
            this.pnlBottom.Location = new System.Drawing.Point(0, 476);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(480, 62);
            this.pnlBottom.TabIndex = 10;
            // 
            // lblSeparator1
            // 
            this.lblSeparator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator1.Enabled = false;
            this.lblSeparator1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSeparator1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblSeparator1.Location = new System.Drawing.Point(15, 66);
            this.lblSeparator1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSeparator1.Name = "lblSeparator1";
            this.lblSeparator1.Size = new System.Drawing.Size(450, 1);
            this.lblSeparator1.TabIndex = 11;
            // 
            // lblSeparator2
            // 
            this.lblSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator2.Enabled = false;
            this.lblSeparator2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSeparator2.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblSeparator2.Location = new System.Drawing.Point(15, 295);
            this.lblSeparator2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSeparator2.Name = "lblSeparator2";
            this.lblSeparator2.Size = new System.Drawing.Size(450, 1);
            this.lblSeparator2.TabIndex = 18;
            // 
            // lblSeparator3
            // 
            this.lblSeparator3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator3.Enabled = false;
            this.lblSeparator3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSeparator3.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblSeparator3.Location = new System.Drawing.Point(15, 426);
            this.lblSeparator3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSeparator3.Name = "lblSeparator3";
            this.lblSeparator3.Size = new System.Drawing.Size(450, 1);
            this.lblSeparator3.TabIndex = 19;
            // 
            // lblAttributes
            // 
            this.lblAttributes.AutoSize = true;
            this.lblAttributes.Location = new System.Drawing.Point(15, 441);
            this.lblAttributes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAttributes.Name = "lblAttributes";
            this.lblAttributes.Size = new System.Drawing.Size(77, 20);
            this.lblAttributes.TabIndex = 23;
            this.lblAttributes.Text = "Attributes:";
            // 
            // txtType1
            // 
            this.txtType1.BackColor = System.Drawing.Color.White;
            this.txtType1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtType1.Location = new System.Drawing.Point(144, 78);
            this.txtType1.Margin = new System.Windows.Forms.Padding(4);
            this.txtType1.Name = "txtType1";
            this.txtType1.ReadOnly = true;
            this.txtType1.Size = new System.Drawing.Size(319, 20);
            this.txtType1.TabIndex = 27;
            this.txtType1.TabStop = false;
            this.txtType1.Text = "<type>";
            // 
            // txtLocation1
            // 
            this.txtLocation1.BackColor = System.Drawing.Color.White;
            this.txtLocation1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLocation1.Location = new System.Drawing.Point(144, 106);
            this.txtLocation1.Margin = new System.Windows.Forms.Padding(4);
            this.txtLocation1.Name = "txtLocation1";
            this.txtLocation1.ReadOnly = true;
            this.txtLocation1.Size = new System.Drawing.Size(319, 20);
            this.txtLocation1.TabIndex = 28;
            this.txtLocation1.TabStop = false;
            this.txtLocation1.Text = "<location>";
            // 
            // lblMultipleObjectsCount
            // 
            this.lblMultipleObjectsCount.AutoSize = true;
            this.lblMultipleObjectsCount.Location = new System.Drawing.Point(63, 23);
            this.lblMultipleObjectsCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMultipleObjectsCount.Name = "lblMultipleObjectsCount";
            this.lblMultipleObjectsCount.Size = new System.Drawing.Size(146, 20);
            this.lblMultipleObjectsCount.TabIndex = 36;
            this.lblMultipleObjectsCount.Text = "Files: X, directories: Y";
            this.lblMultipleObjectsCount.Visible = false;
            // 
            // dlgProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(480, 538);
            this.Controls.Add(this.lblMultipleObjectsCount);
            this.Controls.Add(this.txtFirstCluster1);
            this.Controls.Add(this.lblFirstCluster);
            this.Controls.Add(this.txtContains1);
            this.Controls.Add(this.lblContains);
            this.Controls.Add(this.txtShortFilename1);
            this.Controls.Add(this.txtSizeOnDisk1);
            this.Controls.Add(this.txtSize1);
            this.Controls.Add(this.txtLocation1);
            this.Controls.Add(this.txtType1);
            this.Controls.Add(this.cbxArchive);
            this.Controls.Add(this.cbxSystem);
            this.Controls.Add(this.cbxHidden);
            this.Controls.Add(this.dtpAccessed);
            this.Controls.Add(this.cbxDateAccessed);
            this.Controls.Add(this.cbxDateModified);
            this.Controls.Add(this.dtpCreated);
            this.Controls.Add(this.cbxDateCreated);
            this.Controls.Add(this.lblAttributes);
            this.Controls.Add(this.lblSeparator3);
            this.Controls.Add(this.cbxReadOnly);
            this.Controls.Add(this.lblSeparator2);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.dtpModified);
            this.Controls.Add(this.lblSeparator1);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.lblShortFilename);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.lblSizeOnDisk);
            this.Controls.Add(this.imgIcon);
            this.Controls.Add(this.lblSize);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgProperties";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Properties";
            this.Load += new System.EventHandler(this.dlgProperties_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imgIcon)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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