namespace TotalImage.Dialogs
{
    partial class PropertiesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertiesDialog));
            iconPictureBox = new System.Windows.Forms.PictureBox();
            nameTextBox = new System.Windows.Forms.TextBox();
            typeLabel = new System.Windows.Forms.Label();
            locationLabel = new System.Windows.Forms.Label();
            sizeLabel = new System.Windows.Forms.Label();
            sizeOnDiskLabel = new System.Windows.Forms.Label();
            toolTip = new System.Windows.Forms.ToolTip(components);
            shortNameLabel = new System.Windows.Forms.Label();
            readOnlyCheckBox = new System.Windows.Forms.CheckBox();
            hiddenCheckBox = new System.Windows.Forms.CheckBox();
            systemCheckBox = new System.Windows.Forms.CheckBox();
            archiveCheckBox = new System.Windows.Forms.CheckBox();
            accessedDateTimePicker = new System.Windows.Forms.DateTimePicker();
            createdDateTimePicker = new System.Windows.Forms.DateTimePicker();
            modifiedDateTimePicker = new System.Windows.Forms.DateTimePicker();
            sizeTextBox = new System.Windows.Forms.TextBox();
            sizeOnDiskTextBox = new System.Windows.Forms.TextBox();
            shortNameTextBox = new System.Windows.Forms.TextBox();
            containsTextBox = new System.Windows.Forms.TextBox();
            containsLabel = new System.Windows.Forms.Label();
            createdCheckBox = new System.Windows.Forms.CheckBox();
            modifiedCheckBox = new System.Windows.Forms.CheckBox();
            accessedCheckBox = new System.Windows.Forms.CheckBox();
            firstClusterTextBox = new System.Windows.Forms.TextBox();
            firstClusterLabel = new System.Windows.Forms.Label();
            sha1TextBox = new System.Windows.Forms.TextBox();
            sha1Label = new System.Windows.Forms.Label();
            md5TextBox = new System.Windows.Forms.TextBox();
            md5Label = new System.Windows.Forms.Label();
            crc32TextBox = new System.Windows.Forms.TextBox();
            crc32Label = new System.Windows.Forms.Label();
            cancelButton = new System.Windows.Forms.Button();
            okButton = new System.Windows.Forms.Button();
            buttonPanel = new System.Windows.Forms.Panel();
            separator1Label = new System.Windows.Forms.Label();
            separator2Label = new System.Windows.Forms.Label();
            separator3Label = new System.Windows.Forms.Label();
            attributesLabel = new System.Windows.Forms.Label();
            typeTextBox = new System.Windows.Forms.TextBox();
            locationTextBox = new System.Windows.Forms.TextBox();
            selectionSummaryLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox).BeginInit();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // iconPictureBox
            // 
            iconPictureBox.Location = new System.Drawing.Point(12, 12);
            iconPictureBox.Name = "iconPictureBox";
            iconPictureBox.Size = new System.Drawing.Size(32, 32);
            iconPictureBox.TabIndex = 6;
            iconPictureBox.TabStop = false;
            // 
            // nameTextBox
            // 
            nameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            nameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            nameTextBox.Location = new System.Drawing.Point(50, 16);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new System.Drawing.Size(337, 23);
            nameTextBox.TabIndex = 0;
            nameTextBox.Text = "<filename>";
            toolTip.SetToolTip(nameTextBox, resources.GetString("nameTextBox.ToolTip"));
            // 
            // typeLabel
            // 
            typeLabel.AutoSize = true;
            typeLabel.Location = new System.Drawing.Point(12, 62);
            typeLabel.Name = "typeLabel";
            typeLabel.Size = new System.Drawing.Size(35, 15);
            typeLabel.TabIndex = 8;
            typeLabel.Text = "Type:";
            // 
            // locationLabel
            // 
            locationLabel.AutoSize = true;
            locationLabel.Location = new System.Drawing.Point(12, 86);
            locationLabel.Name = "locationLabel";
            locationLabel.Size = new System.Drawing.Size(56, 15);
            locationLabel.TabIndex = 10;
            locationLabel.Text = "Location:";
            // 
            // sizeLabel
            // 
            sizeLabel.AutoSize = true;
            sizeLabel.Location = new System.Drawing.Point(12, 110);
            sizeLabel.Name = "sizeLabel";
            sizeLabel.Size = new System.Drawing.Size(30, 15);
            sizeLabel.TabIndex = 12;
            sizeLabel.Text = "Size:";
            toolTip.SetToolTip(sizeLabel, "This is the actual size of the file or directory.");
            // 
            // sizeOnDiskLabel
            // 
            sizeOnDiskLabel.AutoSize = true;
            sizeOnDiskLabel.Location = new System.Drawing.Point(12, 134);
            sizeOnDiskLabel.Name = "sizeOnDiskLabel";
            sizeOnDiskLabel.Size = new System.Drawing.Size(71, 15);
            sizeOnDiskLabel.TabIndex = 14;
            sizeOnDiskLabel.Text = "Size on disk:";
            toolTip.SetToolTip(sizeOnDiskLabel, "This is the space occupied by the file or directory on the disk\r\ndue to cluster size.");
            // 
            // toolTip
            // 
            toolTip.AutoPopDelay = 10000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 100;
            toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            toolTip.ToolTipTitle = "The more you know...";
            // 
            // shortNameLabel
            // 
            shortNameLabel.AutoSize = true;
            shortNameLabel.Location = new System.Drawing.Point(12, 158);
            shortNameLabel.Name = "shortNameLabel";
            shortNameLabel.Size = new System.Drawing.Size(97, 15);
            shortNameLabel.TabIndex = 16;
            shortNameLabel.Text = "Short name (8.3):";
            toolTip.SetToolTip(shortNameLabel, resources.GetString("shortNameLabel.ToolTip"));
            // 
            // readOnlyCheckBox
            // 
            readOnlyCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            readOnlyCheckBox.AutoSize = true;
            readOnlyCheckBox.Location = new System.Drawing.Point(83, 415);
            readOnlyCheckBox.Name = "readOnlyCheckBox";
            readOnlyCheckBox.Size = new System.Drawing.Size(80, 19);
            readOnlyCheckBox.TabIndex = 4;
            readOnlyCheckBox.Text = "Read-only";
            toolTip.SetToolTip(readOnlyCheckBox, "Read-only files cannot be written to until this attribute\r\nis cleared.");
            // 
            // hiddenCheckBox
            // 
            hiddenCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            hiddenCheckBox.AutoSize = true;
            hiddenCheckBox.Location = new System.Drawing.Point(169, 415);
            hiddenCheckBox.Name = "hiddenCheckBox";
            hiddenCheckBox.Size = new System.Drawing.Size(65, 19);
            hiddenCheckBox.TabIndex = 5;
            hiddenCheckBox.Text = "Hidden";
            toolTip.SetToolTip(hiddenCheckBox, "Hidden files do not show up in regular directory listings and\r\ncannot be moved, deleted, etc. until this attribute is cleared.");
            // 
            // systemCheckBox
            // 
            systemCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            systemCheckBox.AutoSize = true;
            systemCheckBox.Location = new System.Drawing.Point(243, 415);
            systemCheckBox.Name = "systemCheckBox";
            systemCheckBox.Size = new System.Drawing.Size(64, 19);
            systemCheckBox.TabIndex = 6;
            systemCheckBox.Text = "System";
            toolTip.SetToolTip(systemCheckBox, "System files will not be physically relocated, for example\r\nduring disk defragmentation.");
            // 
            // archiveCheckBox
            // 
            archiveCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            archiveCheckBox.AutoSize = true;
            archiveCheckBox.Location = new System.Drawing.Point(315, 415);
            archiveCheckBox.Name = "archiveCheckBox";
            archiveCheckBox.Size = new System.Drawing.Size(66, 19);
            archiveCheckBox.TabIndex = 7;
            archiveCheckBox.Text = "Archive";
            toolTip.SetToolTip(archiveCheckBox, "Archive files are marked as ready to be backed up by a backup\r\nutility. In practice, this attribute is obsolete.");
            // 
            // accessedDateTimePicker
            // 
            accessedDateTimePicker.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            accessedDateTimePicker.CustomFormat = "yyyy-MM-dd";
            accessedDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            accessedDateTimePicker.Location = new System.Drawing.Point(97, 371);
            accessedDateTimePicker.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            accessedDateTimePicker.Name = "accessedDateTimePicker";
            accessedDateTimePicker.Size = new System.Drawing.Size(293, 23);
            accessedDateTimePicker.TabIndex = 3;
            toolTip.SetToolTip(accessedDateTimePicker, resources.GetString("accessedDateTimePicker.ToolTip"));
            // 
            // createdDateTimePicker
            // 
            createdDateTimePicker.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            createdDateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            createdDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            createdDateTimePicker.Location = new System.Drawing.Point(97, 313);
            createdDateTimePicker.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            createdDateTimePicker.Name = "createdDateTimePicker";
            createdDateTimePicker.Size = new System.Drawing.Size(293, 23);
            createdDateTimePicker.TabIndex = 1;
            toolTip.SetToolTip(createdDateTimePicker, "This is the date and time when the file or directory was originally\r\ncreated. For FAT file systems, only software supporting the VFAT\r\nextensions uses this value.");
            // 
            // modifiedDateTimePicker
            // 
            modifiedDateTimePicker.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            modifiedDateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            modifiedDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            modifiedDateTimePicker.Location = new System.Drawing.Point(97, 342);
            modifiedDateTimePicker.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            modifiedDateTimePicker.Name = "modifiedDateTimePicker";
            modifiedDateTimePicker.Size = new System.Drawing.Size(293, 23);
            modifiedDateTimePicker.TabIndex = 2;
            toolTip.SetToolTip(modifiedDateTimePicker, "This is the date and time when the file or directory was last\r\nwritten to.");
            // 
            // sizeTextBox
            // 
            sizeTextBox.BackColor = System.Drawing.SystemColors.Window;
            sizeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            sizeTextBox.Location = new System.Drawing.Point(115, 110);
            sizeTextBox.Name = "sizeTextBox";
            sizeTextBox.ReadOnly = true;
            sizeTextBox.Size = new System.Drawing.Size(255, 16);
            sizeTextBox.TabIndex = 29;
            sizeTextBox.TabStop = false;
            sizeTextBox.Text = "<size>";
            toolTip.SetToolTip(sizeTextBox, "This is the actual size of the file or directory.");
            // 
            // sizeOnDiskTextBox
            // 
            sizeOnDiskTextBox.BackColor = System.Drawing.SystemColors.Window;
            sizeOnDiskTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            sizeOnDiskTextBox.Location = new System.Drawing.Point(115, 134);
            sizeOnDiskTextBox.Name = "sizeOnDiskTextBox";
            sizeOnDiskTextBox.ReadOnly = true;
            sizeOnDiskTextBox.Size = new System.Drawing.Size(255, 16);
            sizeOnDiskTextBox.TabIndex = 30;
            sizeOnDiskTextBox.TabStop = false;
            sizeOnDiskTextBox.Text = "<sizeondisk>";
            toolTip.SetToolTip(sizeOnDiskTextBox, "This is the space occupied by the file or directory on the disk\r\ndue to cluster size.");
            // 
            // shortNameTextBox
            // 
            shortNameTextBox.BackColor = System.Drawing.SystemColors.Window;
            shortNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            shortNameTextBox.Location = new System.Drawing.Point(115, 158);
            shortNameTextBox.Name = "shortNameTextBox";
            shortNameTextBox.ReadOnly = true;
            shortNameTextBox.Size = new System.Drawing.Size(255, 16);
            shortNameTextBox.TabIndex = 31;
            shortNameTextBox.TabStop = false;
            shortNameTextBox.Text = "<shortname>";
            toolTip.SetToolTip(shortNameTextBox, resources.GetString("shortNameTextBox.ToolTip"));
            // 
            // containsTextBox
            // 
            containsTextBox.BackColor = System.Drawing.SystemColors.Window;
            containsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            containsTextBox.Location = new System.Drawing.Point(115, 182);
            containsTextBox.Name = "containsTextBox";
            containsTextBox.ReadOnly = true;
            containsTextBox.Size = new System.Drawing.Size(255, 16);
            containsTextBox.TabIndex = 33;
            containsTextBox.TabStop = false;
            containsTextBox.Text = "<contains>";
            toolTip.SetToolTip(containsTextBox, "The number of files and subdirectories contained in this directory.\r\n");
            // 
            // containsLabel
            // 
            containsLabel.AutoSize = true;
            containsLabel.Location = new System.Drawing.Point(12, 182);
            containsLabel.Name = "containsLabel";
            containsLabel.Size = new System.Drawing.Size(57, 15);
            containsLabel.TabIndex = 32;
            containsLabel.Text = "Contains:";
            toolTip.SetToolTip(containsLabel, "The number of files and subdirectories contained in this directory.\r\n");
            // 
            // createdCheckBox
            // 
            createdCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            createdCheckBox.AutoSize = true;
            createdCheckBox.Location = new System.Drawing.Point(15, 315);
            createdCheckBox.Name = "createdCheckBox";
            createdCheckBox.Size = new System.Drawing.Size(70, 19);
            createdCheckBox.TabIndex = 24;
            createdCheckBox.Text = "Created:";
            toolTip.SetToolTip(createdCheckBox, "This is the date and time when the file or directory was originally\r\ncreated. For FAT file systems, only software supporting the VFAT\r\nextensions uses this value.");
            // 
            // modifiedCheckBox
            // 
            modifiedCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            modifiedCheckBox.AutoSize = true;
            modifiedCheckBox.Location = new System.Drawing.Point(15, 344);
            modifiedCheckBox.Name = "modifiedCheckBox";
            modifiedCheckBox.Size = new System.Drawing.Size(77, 19);
            modifiedCheckBox.TabIndex = 25;
            modifiedCheckBox.Text = "Modified:";
            toolTip.SetToolTip(modifiedCheckBox, "This is the date and time when the file or directory was last\r\nwritten to.");
            // 
            // accessedCheckBox
            // 
            accessedCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            accessedCheckBox.AutoSize = true;
            accessedCheckBox.Location = new System.Drawing.Point(15, 373);
            accessedCheckBox.Name = "accessedCheckBox";
            accessedCheckBox.Size = new System.Drawing.Size(78, 19);
            accessedCheckBox.TabIndex = 26;
            accessedCheckBox.Text = "Accessed:";
            toolTip.SetToolTip(accessedCheckBox, resources.GetString("accessedCheckBox.ToolTip"));
            // 
            // firstClusterTextBox
            // 
            firstClusterTextBox.BackColor = System.Drawing.SystemColors.Window;
            firstClusterTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            firstClusterTextBox.Location = new System.Drawing.Point(115, 206);
            firstClusterTextBox.Name = "firstClusterTextBox";
            firstClusterTextBox.ReadOnly = true;
            firstClusterTextBox.Size = new System.Drawing.Size(255, 16);
            firstClusterTextBox.TabIndex = 35;
            firstClusterTextBox.TabStop = false;
            firstClusterTextBox.Text = "<firstcluster>";
            toolTip.SetToolTip(firstClusterTextBox, "The starting cluster in the cluster chain of this object.\r\n");
            // 
            // firstClusterLabel
            // 
            firstClusterLabel.AutoSize = true;
            firstClusterLabel.Location = new System.Drawing.Point(12, 206);
            firstClusterLabel.Name = "firstClusterLabel";
            firstClusterLabel.Size = new System.Drawing.Size(70, 15);
            firstClusterLabel.TabIndex = 34;
            firstClusterLabel.Text = "First cluster:";
            toolTip.SetToolTip(firstClusterLabel, "The starting cluster in the cluster chain of this object.\r\n");
            // 
            // sha1TextBox
            // 
            sha1TextBox.BackColor = System.Drawing.SystemColors.Window;
            sha1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            sha1TextBox.Location = new System.Drawing.Point(115, 278);
            sha1TextBox.Name = "sha1TextBox";
            sha1TextBox.ReadOnly = true;
            sha1TextBox.Size = new System.Drawing.Size(255, 16);
            sha1TextBox.TabIndex = 40;
            sha1TextBox.TabStop = false;
            sha1TextBox.Text = "<SHA1 hash>";
            toolTip.SetToolTip(sha1TextBox, "The SHA-1 hash of this file.\r\n");
            // 
            // sha1Label
            // 
            sha1Label.AutoSize = true;
            sha1Label.Location = new System.Drawing.Point(12, 278);
            sha1Label.Name = "sha1Label";
            sha1Label.Size = new System.Drawing.Size(72, 15);
            sha1Label.TabIndex = 39;
            sha1Label.Text = "SHA-1 hash:";
            toolTip.SetToolTip(sha1Label, "The SHA-1 hash of this file.");
            // 
            // md5TextBox
            // 
            md5TextBox.BackColor = System.Drawing.SystemColors.Window;
            md5TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            md5TextBox.Location = new System.Drawing.Point(115, 254);
            md5TextBox.Name = "md5TextBox";
            md5TextBox.ReadOnly = true;
            md5TextBox.Size = new System.Drawing.Size(255, 16);
            md5TextBox.TabIndex = 38;
            md5TextBox.TabStop = false;
            md5TextBox.Text = "<MD5 hash>";
            toolTip.SetToolTip(md5TextBox, "The MD5 hash of this file.\r\n");
            // 
            // md5Label
            // 
            md5Label.AutoSize = true;
            md5Label.Location = new System.Drawing.Point(12, 254);
            md5Label.Name = "md5Label";
            md5Label.Size = new System.Drawing.Size(63, 15);
            md5Label.TabIndex = 37;
            md5Label.Text = "MD5 hash:";
            toolTip.SetToolTip(md5Label, "The MD5 hash of this file.\r\n");
            // 
            // crc32TextBox
            // 
            crc32TextBox.BackColor = System.Drawing.SystemColors.Window;
            crc32TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            crc32TextBox.Location = new System.Drawing.Point(115, 230);
            crc32TextBox.Name = "crc32TextBox";
            crc32TextBox.ReadOnly = true;
            crc32TextBox.Size = new System.Drawing.Size(255, 16);
            crc32TextBox.TabIndex = 42;
            crc32TextBox.TabStop = false;
            crc32TextBox.Text = "<CRC32 hash>";
            toolTip.SetToolTip(crc32TextBox, "The CRC-32 hash of this file.\r\n");
            // 
            // crc32Label
            // 
            crc32Label.AutoSize = true;
            crc32Label.Location = new System.Drawing.Point(12, 230);
            crc32Label.Name = "crc32Label";
            crc32Label.Size = new System.Drawing.Size(78, 15);
            crc32Label.TabIndex = 41;
            crc32Label.Text = "CRC-32 hash:";
            toolTip.SetToolTip(crc32Label, "The CRC-32 hash of this file.\r\n");
            // 
            // cancelButton
            // 
            cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(307, 12);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(80, 26);
            cancelButton.TabIndex = 9;
            cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Location = new System.Drawing.Point(221, 12);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(80, 26);
            okButton.TabIndex = 8;
            okButton.Text = "OK";
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = System.Drawing.SystemColors.Control;
            buttonPanel.Controls.Add(cancelButton);
            buttonPanel.Controls.Add(okButton);
            buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonPanel.Location = new System.Drawing.Point(0, 441);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(399, 50);
            buttonPanel.TabIndex = 10;
            // 
            // separator1Label
            // 
            separator1Label.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            separator1Label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            separator1Label.Enabled = false;
            separator1Label.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            separator1Label.ForeColor = System.Drawing.SystemColors.ControlDark;
            separator1Label.Location = new System.Drawing.Point(12, 53);
            separator1Label.Name = "separator1Label";
            separator1Label.Size = new System.Drawing.Size(375, 1);
            separator1Label.TabIndex = 11;
            // 
            // separator2Label
            // 
            separator2Label.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            separator2Label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            separator2Label.Enabled = false;
            separator2Label.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            separator2Label.ForeColor = System.Drawing.SystemColors.ControlDark;
            separator2Label.Location = new System.Drawing.Point(15, 300);
            separator2Label.Name = "separator2Label";
            separator2Label.Size = new System.Drawing.Size(375, 1);
            separator2Label.TabIndex = 18;
            // 
            // separator3Label
            // 
            separator3Label.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            separator3Label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            separator3Label.Enabled = false;
            separator3Label.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            separator3Label.ForeColor = System.Drawing.SystemColors.ControlDark;
            separator3Label.Location = new System.Drawing.Point(15, 405);
            separator3Label.Name = "separator3Label";
            separator3Label.Size = new System.Drawing.Size(375, 1);
            separator3Label.TabIndex = 19;
            // 
            // attributesLabel
            // 
            attributesLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            attributesLabel.AutoSize = true;
            attributesLabel.Location = new System.Drawing.Point(15, 417);
            attributesLabel.Name = "attributesLabel";
            attributesLabel.Size = new System.Drawing.Size(62, 15);
            attributesLabel.TabIndex = 23;
            attributesLabel.Text = "Attributes:";
            // 
            // typeTextBox
            // 
            typeTextBox.BackColor = System.Drawing.SystemColors.Window;
            typeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            typeTextBox.Location = new System.Drawing.Point(115, 62);
            typeTextBox.Name = "typeTextBox";
            typeTextBox.ReadOnly = true;
            typeTextBox.Size = new System.Drawing.Size(255, 16);
            typeTextBox.TabIndex = 27;
            typeTextBox.TabStop = false;
            typeTextBox.Text = "<type>";
            // 
            // locationTextBox
            // 
            locationTextBox.BackColor = System.Drawing.SystemColors.Window;
            locationTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            locationTextBox.Location = new System.Drawing.Point(115, 85);
            locationTextBox.Name = "locationTextBox";
            locationTextBox.ReadOnly = true;
            locationTextBox.Size = new System.Drawing.Size(255, 16);
            locationTextBox.TabIndex = 28;
            locationTextBox.TabStop = false;
            locationTextBox.Text = "<location>";
            // 
            // selectionSummaryLabel
            // 
            selectionSummaryLabel.AutoSize = true;
            selectionSummaryLabel.Location = new System.Drawing.Point(52, 20);
            selectionSummaryLabel.Name = "selectionSummaryLabel";
            selectionSummaryLabel.Size = new System.Drawing.Size(117, 15);
            selectionSummaryLabel.TabIndex = 36;
            selectionSummaryLabel.Text = "Files: X, directories: Y";
            selectionSummaryLabel.Visible = false;
            // 
            // PropertiesDialog
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(399, 491);
            Controls.Add(crc32TextBox);
            Controls.Add(crc32Label);
            Controls.Add(sha1TextBox);
            Controls.Add(sha1Label);
            Controls.Add(md5TextBox);
            Controls.Add(md5Label);
            Controls.Add(modifiedDateTimePicker);
            Controls.Add(selectionSummaryLabel);
            Controls.Add(firstClusterTextBox);
            Controls.Add(firstClusterLabel);
            Controls.Add(containsTextBox);
            Controls.Add(containsLabel);
            Controls.Add(shortNameTextBox);
            Controls.Add(sizeOnDiskTextBox);
            Controls.Add(sizeTextBox);
            Controls.Add(locationTextBox);
            Controls.Add(typeTextBox);
            Controls.Add(archiveCheckBox);
            Controls.Add(systemCheckBox);
            Controls.Add(hiddenCheckBox);
            Controls.Add(accessedDateTimePicker);
            Controls.Add(accessedCheckBox);
            Controls.Add(modifiedCheckBox);
            Controls.Add(createdDateTimePicker);
            Controls.Add(createdCheckBox);
            Controls.Add(attributesLabel);
            Controls.Add(separator3Label);
            Controls.Add(readOnlyCheckBox);
            Controls.Add(separator2Label);
            Controls.Add(typeLabel);
            Controls.Add(separator1Label);
            Controls.Add(buttonPanel);
            Controls.Add(shortNameLabel);
            Controls.Add(locationLabel);
            Controls.Add(nameTextBox);
            Controls.Add(sizeOnDiskLabel);
            Controls.Add(iconPictureBox);
            Controls.Add(sizeLabel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PropertiesDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Properties";
            ((System.ComponentModel.ISupportInitialize)iconPictureBox).EndInit();
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.PictureBox iconPictureBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.Label sizeLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label sizeOnDiskLabel;
        private System.Windows.Forms.Label shortNameLabel;
        private System.Windows.Forms.CheckBox archiveCheckBox;
        private System.Windows.Forms.CheckBox systemCheckBox;
        private System.Windows.Forms.CheckBox hiddenCheckBox;
        private System.Windows.Forms.CheckBox readOnlyCheckBox;
        private System.Windows.Forms.DateTimePicker createdDateTimePicker;
        private System.Windows.Forms.DateTimePicker modifiedDateTimePicker;
        private System.Windows.Forms.DateTimePicker accessedDateTimePicker;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Label separator1Label;
        private System.Windows.Forms.Label separator2Label;
        private System.Windows.Forms.Label separator3Label;
        private System.Windows.Forms.Label attributesLabel;
        private System.Windows.Forms.CheckBox createdCheckBox;
        private System.Windows.Forms.CheckBox modifiedCheckBox;
        private System.Windows.Forms.CheckBox accessedCheckBox;
        private System.Windows.Forms.TextBox typeTextBox;
        private System.Windows.Forms.TextBox locationTextBox;
        private System.Windows.Forms.TextBox sizeTextBox;
        private System.Windows.Forms.TextBox sizeOnDiskTextBox;
        private System.Windows.Forms.TextBox shortNameTextBox;
        private System.Windows.Forms.TextBox containsTextBox;
        private System.Windows.Forms.Label containsLabel;
        private System.Windows.Forms.TextBox firstClusterTextBox;
        private System.Windows.Forms.Label firstClusterLabel;
        private System.Windows.Forms.Label selectionSummaryLabel;
        private System.Windows.Forms.TextBox sha1TextBox;
        private System.Windows.Forms.Label sha1Label;
        private System.Windows.Forms.TextBox md5TextBox;
        private System.Windows.Forms.Label md5Label;
        private System.Windows.Forms.TextBox crc32TextBox;
        private System.Windows.Forms.Label crc32Label;
    }
}
