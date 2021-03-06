﻿namespace TotalImage
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
            this.lblType1 = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblLocation1 = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblSize1 = new System.Windows.Forms.Label();
            this.lblSizeOnDisk = new System.Windows.Forms.Label();
            this.lblSizeOnDisk1 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblShortFilename = new System.Windows.Forms.Label();
            this.lblShortFilename1 = new System.Windows.Forms.Label();
            this.cbxReadOnly = new System.Windows.Forms.CheckBox();
            this.cbxHidden = new System.Windows.Forms.CheckBox();
            this.cbxSystem = new System.Windows.Forms.CheckBox();
            this.cbxArchive = new System.Windows.Forms.CheckBox();
            this.lblAccessed = new System.Windows.Forms.Label();
            this.dateAccessed = new System.Windows.Forms.DateTimePicker();
            this.dateCreated = new System.Windows.Forms.DateTimePicker();
            this.lblModified = new System.Windows.Forms.Label();
            this.lblCreated = new System.Windows.Forms.Label();
            this.dateModified = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblSeparator1 = new System.Windows.Forms.Label();
            this.lblSeparator2 = new System.Windows.Forms.Label();
            this.lblSeparator3 = new System.Windows.Forms.Label();
            this.lblAttributes = new System.Windows.Forms.Label();
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
            // lblType1
            // 
            this.lblType1.AutoSize = true;
            this.lblType1.Location = new System.Drawing.Point(115, 62);
            this.lblType1.Name = "lblType1";
            this.lblType1.Size = new System.Drawing.Size(46, 15);
            this.lblType1.TabIndex = 9;
            this.lblType1.Text = "<type>";
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
            // lblLocation1
            // 
            this.lblLocation1.AutoSize = true;
            this.lblLocation1.Location = new System.Drawing.Point(115, 86);
            this.lblLocation1.Name = "lblLocation1";
            this.lblLocation1.Size = new System.Drawing.Size(66, 15);
            this.lblLocation1.TabIndex = 11;
            this.lblLocation1.Text = "<location>";
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
            // lblSize1
            // 
            this.lblSize1.AutoSize = true;
            this.lblSize1.Location = new System.Drawing.Point(115, 110);
            this.lblSize1.Name = "lblSize1";
            this.lblSize1.Size = new System.Drawing.Size(42, 15);
            this.lblSize1.TabIndex = 13;
            this.lblSize1.Text = "<size>";
            this.toolTip.SetToolTip(this.lblSize1, "This is the actual size of the file or directory.");
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
            // lblSizeOnDisk1
            // 
            this.lblSizeOnDisk1.AutoSize = true;
            this.lblSizeOnDisk1.Location = new System.Drawing.Point(115, 134);
            this.lblSizeOnDisk1.Name = "lblSizeOnDisk1";
            this.lblSizeOnDisk1.Size = new System.Drawing.Size(102, 15);
            this.lblSizeOnDisk1.TabIndex = 15;
            this.lblSizeOnDisk1.Text = "<spaceoccupied>";
            this.toolTip.SetToolTip(this.lblSizeOnDisk1, "This is the space occupied by the file or directory on the disk\r\ndue to cluster s" +
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
            // lblShortFilename1
            // 
            this.lblShortFilename1.AutoSize = true;
            this.lblShortFilename1.Location = new System.Drawing.Point(115, 158);
            this.lblShortFilename1.Name = "lblShortFilename1";
            this.lblShortFilename1.Size = new System.Drawing.Size(96, 15);
            this.lblShortFilename1.TabIndex = 17;
            this.lblShortFilename1.Text = "<shortfilename>";
            this.toolTip.SetToolTip(this.lblShortFilename1, "Short names are used for backwards compatibility with systems that \r\ndon\'t suppor" +
        "t the VFAT/LFN extensions. It is automatically generated\r\nbased on the long name" +
        " (see above).\r\n");
            // 
            // cbxReadOnly
            // 
            this.cbxReadOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxReadOnly.Location = new System.Drawing.Point(80, 298);
            this.cbxReadOnly.Name = "cbxReadOnly";
            this.cbxReadOnly.Size = new System.Drawing.Size(75, 20);
            this.cbxReadOnly.TabIndex = 4;
            this.cbxReadOnly.Text = "Read-only";
            this.toolTip.SetToolTip(this.cbxReadOnly, "Read-only files cannot be written to until this attribute\r\nis cleared.");
            this.cbxReadOnly.UseVisualStyleBackColor = true;
            // 
            // cbxHidden
            // 
            this.cbxHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxHidden.Location = new System.Drawing.Point(166, 298);
            this.cbxHidden.Name = "cbxHidden";
            this.cbxHidden.Size = new System.Drawing.Size(60, 20);
            this.cbxHidden.TabIndex = 5;
            this.cbxHidden.Text = "Hidden";
            this.toolTip.SetToolTip(this.cbxHidden, "Hidden files do not show up in regular directory listings and\r\ncannot be moved, d" +
        "eleted, etc. until this attribute is cleared.");
            this.cbxHidden.UseVisualStyleBackColor = true;
            // 
            // cbxSystem
            // 
            this.cbxSystem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxSystem.Location = new System.Drawing.Point(240, 298);
            this.cbxSystem.Name = "cbxSystem";
            this.cbxSystem.Size = new System.Drawing.Size(60, 20);
            this.cbxSystem.TabIndex = 6;
            this.cbxSystem.Text = "System";
            this.toolTip.SetToolTip(this.cbxSystem, "System files will not be physically relocated, for example\r\nduring disk defragmen" +
        "tation.");
            this.cbxSystem.UseVisualStyleBackColor = true;
            // 
            // cbxArchive
            // 
            this.cbxArchive.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxArchive.Location = new System.Drawing.Point(312, 298);
            this.cbxArchive.Name = "cbxArchive";
            this.cbxArchive.Size = new System.Drawing.Size(60, 20);
            this.cbxArchive.TabIndex = 7;
            this.cbxArchive.Text = "Archive";
            this.toolTip.SetToolTip(this.cbxArchive, "Archive files are marked as ready to be backed up by a backup\r\nutility. In practi" +
        "ce, this attribute is obsolete.");
            this.cbxArchive.UseVisualStyleBackColor = true;
            // 
            // lblAccessed
            // 
            this.lblAccessed.AutoSize = true;
            this.lblAccessed.Location = new System.Drawing.Point(12, 259);
            this.lblAccessed.Name = "lblAccessed";
            this.lblAccessed.Size = new System.Drawing.Size(59, 15);
            this.lblAccessed.TabIndex = 22;
            this.lblAccessed.Text = "Accessed:";
            this.toolTip.SetToolTip(this.lblAccessed, "This is the date when the file or directory was last accessed. The definition \r\no" +
        "f this can vary significantly from system to system. Only systems \r\nsupporting t" +
        "he VFAT extensions use this value.");
            // 
            // dateAccessed
            // 
            this.dateAccessed.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateAccessed.Location = new System.Drawing.Point(77, 254);
            this.dateAccessed.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            this.dateAccessed.Name = "dateAccessed";
            this.dateAccessed.ShowCheckBox = true;
            this.dateAccessed.ShowUpDown = true;
            this.dateAccessed.Size = new System.Drawing.Size(295, 23);
            this.dateAccessed.TabIndex = 3;
            this.toolTip.SetToolTip(this.dateAccessed, "This is the date when the file or directory was last accessed. The definition \r\no" +
        "f this can vary significantly from system to system. Only systems \r\nsupporting t" +
        "he VFAT extensions use this value.");
            // 
            // dateCreated
            // 
            this.dateCreated.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateCreated.Location = new System.Drawing.Point(77, 196);
            this.dateCreated.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            this.dateCreated.Name = "dateCreated";
            this.dateCreated.ShowCheckBox = true;
            this.dateCreated.ShowUpDown = true;
            this.dateCreated.Size = new System.Drawing.Size(295, 23);
            this.dateCreated.TabIndex = 1;
            this.toolTip.SetToolTip(this.dateCreated, "This is the date and time when the file or directory was originally\r\ncreated. Onl" +
        "y systems supporting the VFAT extensions use this value.");
            // 
            // lblModified
            // 
            this.lblModified.AutoSize = true;
            this.lblModified.Location = new System.Drawing.Point(12, 230);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new System.Drawing.Size(58, 15);
            this.lblModified.TabIndex = 19;
            this.lblModified.Text = "Modified:";
            this.toolTip.SetToolTip(this.lblModified, "This is the date and time when the file or directory was last\r\nwritten to.");
            // 
            // lblCreated
            // 
            this.lblCreated.AutoSize = true;
            this.lblCreated.Location = new System.Drawing.Point(12, 201);
            this.lblCreated.Name = "lblCreated";
            this.lblCreated.Size = new System.Drawing.Size(51, 15);
            this.lblCreated.TabIndex = 18;
            this.lblCreated.Text = "Created:";
            this.toolTip.SetToolTip(this.lblCreated, "This is the date and time when the file or directory was originally\r\ncreated. Onl" +
        "y systems supporting the VFAT extensions use this value.\r\n");
            // 
            // dateModified
            // 
            this.dateModified.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateModified.Location = new System.Drawing.Point(77, 225);
            this.dateModified.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            this.dateModified.Name = "dateModified";
            this.dateModified.ShowCheckBox = true;
            this.dateModified.ShowUpDown = true;
            this.dateModified.Size = new System.Drawing.Size(295, 23);
            this.dateModified.TabIndex = 2;
            this.toolTip.SetToolTip(this.dateModified, "This is the date and time when the file or directory was last\r\nwritten to.");
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
            // dlgProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 381);
            this.Controls.Add(this.cbxArchive);
            this.Controls.Add(this.lblAttributes);
            this.Controls.Add(this.cbxSystem);
            this.Controls.Add(this.lblAccessed);
            this.Controls.Add(this.cbxHidden);
            this.Controls.Add(this.lblSeparator3);
            this.Controls.Add(this.cbxReadOnly);
            this.Controls.Add(this.dateAccessed);
            this.Controls.Add(this.lblSeparator2);
            this.Controls.Add(this.dateCreated);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblModified);
            this.Controls.Add(this.lblShortFilename1);
            this.Controls.Add(this.lblCreated);
            this.Controls.Add(this.dateModified);
            this.Controls.Add(this.lblSeparator1);
            this.Controls.Add(this.lblType1);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.lblShortFilename);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.lblSizeOnDisk1);
            this.Controls.Add(this.lblLocation1);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.lblSizeOnDisk);
            this.Controls.Add(this.imgIcon);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.lblSize1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
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
        private System.Windows.Forms.Label lblType1;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblLocation1;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label lblSize1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label lblSizeOnDisk;
        private System.Windows.Forms.Label lblSizeOnDisk1;
        private System.Windows.Forms.Label lblShortFilename;
        private System.Windows.Forms.Label lblShortFilename1;
        private System.Windows.Forms.CheckBox cbxArchive;
        private System.Windows.Forms.CheckBox cbxSystem;
        private System.Windows.Forms.CheckBox cbxHidden;
        private System.Windows.Forms.CheckBox cbxReadOnly;
        private System.Windows.Forms.DateTimePicker dateCreated;
        private System.Windows.Forms.Label lblModified;
        private System.Windows.Forms.Label lblCreated;
        private System.Windows.Forms.DateTimePicker dateModified;
        private System.Windows.Forms.Label lblAccessed;
        private System.Windows.Forms.DateTimePicker dateAccessed;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblSeparator1;
        private System.Windows.Forms.Label lblSeparator2;
        private System.Windows.Forms.Label lblSeparator3;
        private System.Windows.Forms.Label lblAttributes;
    }
}