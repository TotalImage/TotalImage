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
            this.dateAccessed = new System.Windows.Forms.DateTimePicker();
            this.dateCreated = new System.Windows.Forms.DateTimePicker();
            this.dateModified = new System.Windows.Forms.DateTimePicker();
            this.txtSize1 = new System.Windows.Forms.TextBox();
            this.txtSizeOnDisk1 = new System.Windows.Forms.TextBox();
            this.txtShortFilename1 = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblSeparator1 = new System.Windows.Forms.Label();
            this.lblSeparator2 = new System.Windows.Forms.Label();
            this.lblSeparator3 = new System.Windows.Forms.Label();
            this.lblAttributes = new System.Windows.Forms.Label();
            this.cbxDateCreated = new System.Windows.Forms.CheckBox();
            this.cbxDateModified = new System.Windows.Forms.CheckBox();
            this.cbxDateAccessed = new System.Windows.Forms.CheckBox();
            this.txtType1 = new System.Windows.Forms.TextBox();
            this.txtLocation1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgIcon)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgIcon
            // 
            this.imgIcon.Location = new System.Drawing.Point(12, 12);
            this.imgIcon.Name = "imgIcon";
            this.imgIcon.Size = new System.Drawing.Size(32, 32);
            this.imgIcon.TabIndex = 6;
            this.imgIcon.TabStop = false;
            // 
            // txtFilename
            // 
            this.txtFilename.Location = new System.Drawing.Point(50, 16);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(322, 23);
            this.txtFilename.TabIndex = 0;
            this.txtFilename.Text = "<filename>";
            this.toolTip.SetToolTip(this.txtFilename, resources.GetString("txtFilename.ToolTip"));
            this.txtFilename.TextChanged += new System.EventHandler(this.txtFilename_TextChanged);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(12, 62);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(34, 15);
            this.lblType.TabIndex = 8;
            this.lblType.Text = "Type:";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(12, 86);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(56, 15);
            this.lblLocation.TabIndex = 10;
            this.lblLocation.Text = "Location:";
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(12, 110);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(30, 15);
            this.lblSize.TabIndex = 12;
            this.lblSize.Text = "Size:";
            this.toolTip.SetToolTip(this.lblSize, "This is the actual size of the file or directory.");
            // 
            // lblSizeOnDisk
            // 
            this.lblSizeOnDisk.AutoSize = true;
            this.lblSizeOnDisk.Location = new System.Drawing.Point(12, 134);
            this.lblSizeOnDisk.Name = "lblSizeOnDisk";
            this.lblSizeOnDisk.Size = new System.Drawing.Size(93, 15);
            this.lblSizeOnDisk.TabIndex = 14;
            this.lblSizeOnDisk.Text = "Space occupied:";
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
            this.lblShortFilename.Location = new System.Drawing.Point(12, 158);
            this.lblShortFilename.Name = "lblShortFilename";
            this.lblShortFilename.Size = new System.Drawing.Size(97, 15);
            this.lblShortFilename.TabIndex = 16;
            this.lblShortFilename.Text = "Short name (8.3):";
            this.toolTip.SetToolTip(this.lblShortFilename, "Short names are used for backwards compatibility with systems that \r\ndon\'t suppor" +
        "t the VFAT/LFN extensions. It is automatically generated\r\nbased on the long name" +
        " (see above).\r\n");
            // 
            // cbxReadOnly
            // 
            this.cbxReadOnly.AutoSize = true;
            this.cbxReadOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxReadOnly.Location = new System.Drawing.Point(80, 298);
            this.cbxReadOnly.Name = "cbxReadOnly";
            this.cbxReadOnly.Size = new System.Drawing.Size(86, 20);
            this.cbxReadOnly.TabIndex = 4;
            this.cbxReadOnly.Text = "Read-only";
            this.toolTip.SetToolTip(this.cbxReadOnly, "Read-only files cannot be written to until this attribute\r\nis cleared.");
            this.cbxReadOnly.UseVisualStyleBackColor = true;
            // 
            // cbxHidden
            // 
            this.cbxHidden.AutoSize = true;
            this.cbxHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxHidden.Location = new System.Drawing.Point(166, 298);
            this.cbxHidden.Name = "cbxHidden";
            this.cbxHidden.Size = new System.Drawing.Size(71, 20);
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
            this.cbxSystem.Location = new System.Drawing.Point(240, 298);
            this.cbxSystem.Name = "cbxSystem";
            this.cbxSystem.Size = new System.Drawing.Size(70, 20);
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
            this.cbxArchive.Location = new System.Drawing.Point(312, 298);
            this.cbxArchive.Name = "cbxArchive";
            this.cbxArchive.Size = new System.Drawing.Size(72, 20);
            this.cbxArchive.TabIndex = 7;
            this.cbxArchive.Text = "Archive";
            this.toolTip.SetToolTip(this.cbxArchive, "Archive files are marked as ready to be backed up by a backup\r\nutility. In practi" +
        "ce, this attribute is obsolete.");
            this.cbxArchive.UseVisualStyleBackColor = true;
            // 
            // dateAccessed
            // 
            this.dateAccessed.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateAccessed.Location = new System.Drawing.Point(94, 254);
            this.dateAccessed.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            this.dateAccessed.Name = "dateAccessed";
            this.dateAccessed.ShowUpDown = true;
            this.dateAccessed.Size = new System.Drawing.Size(278, 23);
            this.dateAccessed.TabIndex = 3;
            this.toolTip.SetToolTip(this.dateAccessed, "This is the date when the file or directory was last accessed. The definition \r\no" +
        "f this can vary significantly from system to system. Only systems \r\nsupporting t" +
        "he VFAT extensions use this value.");
            // 
            // dateCreated
            // 
            this.dateCreated.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateCreated.Location = new System.Drawing.Point(94, 196);
            this.dateCreated.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            this.dateCreated.Name = "dateCreated";
            this.dateCreated.ShowUpDown = true;
            this.dateCreated.Size = new System.Drawing.Size(278, 23);
            this.dateCreated.TabIndex = 1;
            this.toolTip.SetToolTip(this.dateCreated, "This is the date and time when the file or directory was originally\r\ncreated. Onl" +
        "y systems supporting the VFAT extensions use this value.");
            // 
            // dateModified
            // 
            this.dateModified.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateModified.Location = new System.Drawing.Point(94, 225);
            this.dateModified.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            this.dateModified.Name = "dateModified";
            this.dateModified.ShowUpDown = true;
            this.dateModified.Size = new System.Drawing.Size(278, 23);
            this.dateModified.TabIndex = 2;
            this.toolTip.SetToolTip(this.dateModified, "This is the date and time when the file or directory was last\r\nwritten to.");
            // 
            // txtSize1
            // 
            this.txtSize1.BackColor = System.Drawing.Color.White;
            this.txtSize1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSize1.Location = new System.Drawing.Point(115, 110);
            this.txtSize1.Name = "txtSize1";
            this.txtSize1.ReadOnly = true;
            this.txtSize1.Size = new System.Drawing.Size(255, 16);
            this.txtSize1.TabIndex = 29;
            this.txtSize1.TabStop = false;
            this.txtSize1.Text = "<size>";
            this.toolTip.SetToolTip(this.txtSize1, "This is the actual size of the file or directory.");
            // 
            // txtSizeOnDisk1
            // 
            this.txtSizeOnDisk1.BackColor = System.Drawing.Color.White;
            this.txtSizeOnDisk1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSizeOnDisk1.Location = new System.Drawing.Point(115, 134);
            this.txtSizeOnDisk1.Name = "txtSizeOnDisk1";
            this.txtSizeOnDisk1.ReadOnly = true;
            this.txtSizeOnDisk1.Size = new System.Drawing.Size(255, 16);
            this.txtSizeOnDisk1.TabIndex = 30;
            this.txtSizeOnDisk1.TabStop = false;
            this.txtSizeOnDisk1.Text = "<spaceoccupied>";
            this.toolTip.SetToolTip(this.txtSizeOnDisk1, "This is the space occupied by the file or directory on the disk\r\ndue to cluster s" +
        "ize.");
            // 
            // txtShortFilename1
            // 
            this.txtShortFilename1.BackColor = System.Drawing.Color.White;
            this.txtShortFilename1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtShortFilename1.Location = new System.Drawing.Point(115, 158);
            this.txtShortFilename1.Name = "txtShortFilename1";
            this.txtShortFilename1.ReadOnly = true;
            this.txtShortFilename1.Size = new System.Drawing.Size(255, 16);
            this.txtShortFilename1.TabIndex = 31;
            this.txtShortFilename1.TabStop = false;
            this.txtShortFilename1.Text = "<shortname>";
            this.toolTip.SetToolTip(this.txtShortFilename1, "Short names are used for backwards compatibility with systems that \r\ndon\'t suppor" +
        "t the VFAT/LFN extensions. It is automatically generated\r\nbased on the long name" +
        " (see above).");
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(292, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(206, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
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
            this.pnlBottom.Location = new System.Drawing.Point(0, 331);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(384, 50);
            this.pnlBottom.TabIndex = 10;
            // 
            // lblSeparator1
            // 
            this.lblSeparator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator1.Enabled = false;
            this.lblSeparator1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSeparator1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblSeparator1.Location = new System.Drawing.Point(12, 53);
            this.lblSeparator1.Name = "lblSeparator1";
            this.lblSeparator1.Size = new System.Drawing.Size(360, 1);
            this.lblSeparator1.TabIndex = 11;
            // 
            // lblSeparator2
            // 
            this.lblSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator2.Enabled = false;
            this.lblSeparator2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSeparator2.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblSeparator2.Location = new System.Drawing.Point(12, 183);
            this.lblSeparator2.Name = "lblSeparator2";
            this.lblSeparator2.Size = new System.Drawing.Size(360, 1);
            this.lblSeparator2.TabIndex = 18;
            // 
            // lblSeparator3
            // 
            this.lblSeparator3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator3.Enabled = false;
            this.lblSeparator3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSeparator3.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblSeparator3.Location = new System.Drawing.Point(12, 288);
            this.lblSeparator3.Name = "lblSeparator3";
            this.lblSeparator3.Size = new System.Drawing.Size(360, 1);
            this.lblSeparator3.TabIndex = 19;
            // 
            // lblAttributes
            // 
            this.lblAttributes.AutoSize = true;
            this.lblAttributes.Location = new System.Drawing.Point(12, 300);
            this.lblAttributes.Name = "lblAttributes";
            this.lblAttributes.Size = new System.Drawing.Size(62, 15);
            this.lblAttributes.TabIndex = 23;
            this.lblAttributes.Text = "Attributes:";
            // 
            // cbxDateCreated
            // 
            this.cbxDateCreated.AutoSize = true;
            this.cbxDateCreated.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxDateCreated.Location = new System.Drawing.Point(12, 198);
            this.cbxDateCreated.Name = "cbxDateCreated";
            this.cbxDateCreated.Size = new System.Drawing.Size(76, 20);
            this.cbxDateCreated.TabIndex = 24;
            this.cbxDateCreated.Text = "Created:";
            this.cbxDateCreated.UseVisualStyleBackColor = true;
            this.cbxDateCreated.CheckedChanged += new System.EventHandler(this.cbxDateCreated_CheckedChanged);
            // 
            // cbxDateModified
            // 
            this.cbxDateModified.AutoSize = true;
            this.cbxDateModified.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxDateModified.Location = new System.Drawing.Point(12, 227);
            this.cbxDateModified.Name = "cbxDateModified";
            this.cbxDateModified.Size = new System.Drawing.Size(80, 20);
            this.cbxDateModified.TabIndex = 25;
            this.cbxDateModified.Text = "Modified";
            this.cbxDateModified.UseVisualStyleBackColor = true;
            this.cbxDateModified.CheckedChanged += new System.EventHandler(this.cbxDateModified_CheckedChanged);
            // 
            // cbxDateAccessed
            // 
            this.cbxDateAccessed.AutoSize = true;
            this.cbxDateAccessed.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxDateAccessed.Location = new System.Drawing.Point(12, 256);
            this.cbxDateAccessed.Name = "cbxDateAccessed";
            this.cbxDateAccessed.Size = new System.Drawing.Size(84, 20);
            this.cbxDateAccessed.TabIndex = 26;
            this.cbxDateAccessed.Text = "Accessed:";
            this.cbxDateAccessed.UseVisualStyleBackColor = true;
            this.cbxDateAccessed.CheckedChanged += new System.EventHandler(this.cbxDateAccessed_CheckedChanged);
            // 
            // txtType1
            // 
            this.txtType1.BackColor = System.Drawing.Color.White;
            this.txtType1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtType1.Location = new System.Drawing.Point(115, 62);
            this.txtType1.Name = "txtType1";
            this.txtType1.ReadOnly = true;
            this.txtType1.Size = new System.Drawing.Size(255, 16);
            this.txtType1.TabIndex = 27;
            this.txtType1.TabStop = false;
            this.txtType1.Text = "<type>";
            // 
            // txtLocation1
            // 
            this.txtLocation1.BackColor = System.Drawing.Color.White;
            this.txtLocation1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLocation1.Location = new System.Drawing.Point(115, 85);
            this.txtLocation1.Name = "txtLocation1";
            this.txtLocation1.ReadOnly = true;
            this.txtLocation1.Size = new System.Drawing.Size(255, 16);
            this.txtLocation1.TabIndex = 28;
            this.txtLocation1.TabStop = false;
            this.txtLocation1.Text = "<location>";
            // 
            // dlgProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 381);
            this.Controls.Add(this.txtShortFilename1);
            this.Controls.Add(this.txtSizeOnDisk1);
            this.Controls.Add(this.txtSize1);
            this.Controls.Add(this.txtLocation1);
            this.Controls.Add(this.txtType1);
            this.Controls.Add(this.cbxArchive);
            this.Controls.Add(this.cbxSystem);
            this.Controls.Add(this.cbxHidden);
            this.Controls.Add(this.dateAccessed);
            this.Controls.Add(this.cbxDateAccessed);
            this.Controls.Add(this.cbxDateModified);
            this.Controls.Add(this.dateCreated);
            this.Controls.Add(this.cbxDateCreated);
            this.Controls.Add(this.lblAttributes);
            this.Controls.Add(this.lblSeparator3);
            this.Controls.Add(this.cbxReadOnly);
            this.Controls.Add(this.lblSeparator2);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.dateModified);
            this.Controls.Add(this.lblSeparator1);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.lblShortFilename);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.lblSizeOnDisk);
            this.Controls.Add(this.imgIcon);
            this.Controls.Add(this.lblSize);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgProperties";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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
        private System.Windows.Forms.DateTimePicker dateCreated;
        private System.Windows.Forms.DateTimePicker dateModified;
        private System.Windows.Forms.DateTimePicker dateAccessed;
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
    }
}