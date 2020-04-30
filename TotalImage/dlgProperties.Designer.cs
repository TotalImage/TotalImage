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
            this.gbxGeneral = new System.Windows.Forms.GroupBox();
            this.gbxDates = new System.Windows.Forms.GroupBox();
            this.lblAccessed = new System.Windows.Forms.Label();
            this.dateAccessed = new System.Windows.Forms.DateTimePicker();
            this.dateCreated = new System.Windows.Forms.DateTimePicker();
            this.lblModified = new System.Windows.Forms.Label();
            this.lblCreated = new System.Windows.Forms.Label();
            this.dateModified = new System.Windows.Forms.DateTimePicker();
            this.gbxAttributes = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imgIcon)).BeginInit();
            this.gbxGeneral.SuspendLayout();
            this.gbxDates.SuspendLayout();
            this.gbxAttributes.SuspendLayout();
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
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(6, 19);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(34, 15);
            this.lblType.TabIndex = 8;
            this.lblType.Text = "Type:";
            // 
            // lblType1
            // 
            this.lblType1.AutoSize = true;
            this.lblType1.Location = new System.Drawing.Point(105, 19);
            this.lblType1.Name = "lblType1";
            this.lblType1.Size = new System.Drawing.Size(46, 15);
            this.lblType1.TabIndex = 9;
            this.lblType1.Text = "<type>";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(6, 43);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(56, 15);
            this.lblLocation.TabIndex = 10;
            this.lblLocation.Text = "Location:";
            // 
            // lblLocation1
            // 
            this.lblLocation1.AutoSize = true;
            this.lblLocation1.Location = new System.Drawing.Point(105, 43);
            this.lblLocation1.Name = "lblLocation1";
            this.lblLocation1.Size = new System.Drawing.Size(66, 15);
            this.lblLocation1.TabIndex = 11;
            this.lblLocation1.Text = "<location>";
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(6, 67);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(30, 15);
            this.lblSize.TabIndex = 12;
            this.lblSize.Text = "Size:";
            this.toolTip.SetToolTip(this.lblSize, "This is the actual size of the data inside the file.");
            // 
            // lblSize1
            // 
            this.lblSize1.AutoSize = true;
            this.lblSize1.Location = new System.Drawing.Point(105, 67);
            this.lblSize1.Name = "lblSize1";
            this.lblSize1.Size = new System.Drawing.Size(42, 15);
            this.lblSize1.TabIndex = 13;
            this.lblSize1.Text = "<size>";
            this.toolTip.SetToolTip(this.lblSize1, "This is the actual size of the data inside the file.");
            // 
            // lblSizeOnDisk
            // 
            this.lblSizeOnDisk.AutoSize = true;
            this.lblSizeOnDisk.Location = new System.Drawing.Point(6, 91);
            this.lblSizeOnDisk.Name = "lblSizeOnDisk";
            this.lblSizeOnDisk.Size = new System.Drawing.Size(93, 15);
            this.lblSizeOnDisk.TabIndex = 14;
            this.lblSizeOnDisk.Text = "Space occupied:";
            this.toolTip.SetToolTip(this.lblSizeOnDisk, "This is the disk space occupied by the file due to\r\ncluster size.");
            // 
            // lblSizeOnDisk1
            // 
            this.lblSizeOnDisk1.AutoSize = true;
            this.lblSizeOnDisk1.Location = new System.Drawing.Point(105, 91);
            this.lblSizeOnDisk1.Name = "lblSizeOnDisk1";
            this.lblSizeOnDisk1.Size = new System.Drawing.Size(102, 15);
            this.lblSizeOnDisk1.TabIndex = 15;
            this.lblSizeOnDisk1.Text = "<spaceoccupied>";
            this.toolTip.SetToolTip(this.lblSizeOnDisk1, "This is the disk space occupied by the file due to\r\ncluster size.");
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
            this.lblShortFilename.Location = new System.Drawing.Point(6, 115);
            this.lblShortFilename.Name = "lblShortFilename";
            this.lblShortFilename.Size = new System.Drawing.Size(113, 15);
            this.lblShortFilename.TabIndex = 16;
            this.lblShortFilename.Text = "Short filename (8.3):";
            this.toolTip.SetToolTip(this.lblShortFilename, "Short filenames are used for backwards compatibility with\r\nsystems not supporting" +
        " the VFAT/LFN extensions.");
            // 
            // lblShortFilename1
            // 
            this.lblShortFilename1.AutoSize = true;
            this.lblShortFilename1.Location = new System.Drawing.Point(6, 134);
            this.lblShortFilename1.Name = "lblShortFilename1";
            this.lblShortFilename1.Size = new System.Drawing.Size(96, 15);
            this.lblShortFilename1.TabIndex = 17;
            this.lblShortFilename1.Text = "<shortfilename>";
            this.toolTip.SetToolTip(this.lblShortFilename1, "Short filenames are used for backwards compatibility with\r\nsystems not supporting" +
        " the VFAT/LFN extensions.\r\n");
            // 
            // cbxReadOnly
            // 
            this.cbxReadOnly.AutoSize = true;
            this.cbxReadOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxReadOnly.Location = new System.Drawing.Point(9, 22);
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
            this.cbxHidden.Location = new System.Drawing.Point(101, 22);
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
            this.cbxSystem.Location = new System.Drawing.Point(178, 22);
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
            this.cbxArchive.Location = new System.Drawing.Point(254, 22);
            this.cbxArchive.Name = "cbxArchive";
            this.cbxArchive.Size = new System.Drawing.Size(72, 20);
            this.cbxArchive.TabIndex = 7;
            this.cbxArchive.Text = "Archive";
            this.toolTip.SetToolTip(this.cbxArchive, "Archive files are marked as ready to be backed up by a backup\r\nutility. In practi" +
        "ce, this attribute is obsolete.");
            this.cbxArchive.UseVisualStyleBackColor = true;
            // 
            // gbxGeneral
            // 
            this.gbxGeneral.Controls.Add(this.lblType);
            this.gbxGeneral.Controls.Add(this.lblShortFilename1);
            this.gbxGeneral.Controls.Add(this.lblType1);
            this.gbxGeneral.Controls.Add(this.lblShortFilename);
            this.gbxGeneral.Controls.Add(this.lblLocation);
            this.gbxGeneral.Controls.Add(this.lblSizeOnDisk1);
            this.gbxGeneral.Controls.Add(this.lblLocation1);
            this.gbxGeneral.Controls.Add(this.lblSizeOnDisk);
            this.gbxGeneral.Controls.Add(this.lblSize);
            this.gbxGeneral.Controls.Add(this.lblSize1);
            this.gbxGeneral.Location = new System.Drawing.Point(12, 50);
            this.gbxGeneral.Name = "gbxGeneral";
            this.gbxGeneral.Size = new System.Drawing.Size(360, 159);
            this.gbxGeneral.TabIndex = 1;
            this.gbxGeneral.TabStop = false;
            this.gbxGeneral.Text = "General";
            // 
            // gbxDates
            // 
            this.gbxDates.Controls.Add(this.lblAccessed);
            this.gbxDates.Controls.Add(this.dateAccessed);
            this.gbxDates.Controls.Add(this.dateCreated);
            this.gbxDates.Controls.Add(this.lblModified);
            this.gbxDates.Controls.Add(this.lblCreated);
            this.gbxDates.Controls.Add(this.dateModified);
            this.gbxDates.Location = new System.Drawing.Point(12, 215);
            this.gbxDates.Name = "gbxDates";
            this.gbxDates.Size = new System.Drawing.Size(360, 115);
            this.gbxDates.TabIndex = 1;
            this.gbxDates.TabStop = false;
            this.gbxDates.Text = "File dates";
            // 
            // lblAccessed
            // 
            this.lblAccessed.AutoSize = true;
            this.lblAccessed.Location = new System.Drawing.Point(6, 86);
            this.lblAccessed.Name = "lblAccessed";
            this.lblAccessed.Size = new System.Drawing.Size(59, 15);
            this.lblAccessed.TabIndex = 22;
            this.lblAccessed.Text = "Accessed:";
            // 
            // dateAccessed
            // 
            this.dateAccessed.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateAccessed.Location = new System.Drawing.Point(75, 80);
            this.dateAccessed.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            this.dateAccessed.Name = "dateAccessed";
            this.dateAccessed.ShowCheckBox = true;
            this.dateAccessed.ShowUpDown = true;
            this.dateAccessed.Size = new System.Drawing.Size(279, 23);
            this.dateAccessed.TabIndex = 3;
            // 
            // dateCreated
            // 
            this.dateCreated.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateCreated.Location = new System.Drawing.Point(75, 22);
            this.dateCreated.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            this.dateCreated.Name = "dateCreated";
            this.dateCreated.ShowCheckBox = true;
            this.dateCreated.ShowUpDown = true;
            this.dateCreated.Size = new System.Drawing.Size(279, 23);
            this.dateCreated.TabIndex = 1;
            // 
            // lblModified
            // 
            this.lblModified.AutoSize = true;
            this.lblModified.Location = new System.Drawing.Point(6, 57);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new System.Drawing.Size(58, 15);
            this.lblModified.TabIndex = 19;
            this.lblModified.Text = "Modified:";
            // 
            // lblCreated
            // 
            this.lblCreated.AutoSize = true;
            this.lblCreated.Location = new System.Drawing.Point(6, 28);
            this.lblCreated.Name = "lblCreated";
            this.lblCreated.Size = new System.Drawing.Size(51, 15);
            this.lblCreated.TabIndex = 18;
            this.lblCreated.Text = "Created:";
            // 
            // dateModified
            // 
            this.dateModified.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateModified.Location = new System.Drawing.Point(75, 51);
            this.dateModified.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            this.dateModified.Name = "dateModified";
            this.dateModified.ShowCheckBox = true;
            this.dateModified.ShowUpDown = true;
            this.dateModified.Size = new System.Drawing.Size(279, 23);
            this.dateModified.TabIndex = 2;
            // 
            // gbxAttributes
            // 
            this.gbxAttributes.Controls.Add(this.cbxArchive);
            this.gbxAttributes.Controls.Add(this.cbxSystem);
            this.gbxAttributes.Controls.Add(this.cbxHidden);
            this.gbxAttributes.Controls.Add(this.cbxReadOnly);
            this.gbxAttributes.Location = new System.Drawing.Point(12, 336);
            this.gbxAttributes.Name = "gbxAttributes";
            this.gbxAttributes.Size = new System.Drawing.Size(360, 54);
            this.gbxAttributes.TabIndex = 4;
            this.gbxAttributes.TabStop = false;
            this.gbxAttributes.Text = "Attributes";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(292, 403);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(206, 403);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // dlgProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 441);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbxAttributes);
            this.Controls.Add(this.gbxDates);
            this.Controls.Add(this.gbxGeneral);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.imgIcon);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgProperties";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Properties";
            ((System.ComponentModel.ISupportInitialize)(this.imgIcon)).EndInit();
            this.gbxGeneral.ResumeLayout(false);
            this.gbxGeneral.PerformLayout();
            this.gbxDates.ResumeLayout(false);
            this.gbxDates.PerformLayout();
            this.gbxAttributes.ResumeLayout(false);
            this.gbxAttributes.PerformLayout();
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
        private System.Windows.Forms.GroupBox gbxGeneral;
        private System.Windows.Forms.GroupBox gbxDates;
        private System.Windows.Forms.GroupBox gbxAttributes;
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
    }
}