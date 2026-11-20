namespace TotalImage.Dialogs
{
    partial class NewImageAdvancedDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewImageAdvancedDialog));
            okButton = new System.Windows.Forms.Button();
            toolTip = new System.Windows.Forms.ToolTip(components);
            tracksNumericUpDown = new System.Windows.Forms.NumericUpDown();
            tracksLabel = new System.Windows.Forms.Label();
            reservedSectorsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            fatCountNumericUpDown = new System.Windows.Forms.NumericUpDown();
            fileSystemTypeTextBox = new System.Windows.Forms.TextBox();
            fileSystemTypeLabel = new System.Windows.Forms.Label();
            serialNumberTextBox = new System.Windows.Forms.TextBox();
            serialNumberLabel = new System.Windows.Forms.Label();
            sidesComboBox = new System.Windows.Forms.ComboBox();
            sidesLabel = new System.Windows.Forms.Label();
            sectorsPerTrackNumericUpDown = new System.Windows.Forms.NumericUpDown();
            sectorsPerTrackLabel = new System.Windows.Forms.Label();
            sectorsPerFatNumericUpDown = new System.Windows.Forms.NumericUpDown();
            sectorsPerFatLabel = new System.Windows.Forms.Label();
            mediaDescriptorNumericUpDown = new System.Windows.Forms.NumericUpDown();
            mediaDescriptorLabel = new System.Windows.Forms.Label();
            totalSectorsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            totalSectorsLabel = new System.Windows.Forms.Label();
            rootDirectoryEntriesNumericUpDown = new System.Windows.Forms.NumericUpDown();
            rootDirectoryEntriesLabel = new System.Windows.Forms.Label();
            fatCountLabel = new System.Windows.Forms.Label();
            reservedSectorsLabel = new System.Windows.Forms.Label();
            sectorsPerClusterNumericUpDown = new System.Windows.Forms.NumericUpDown();
            sectorsPerClusterLabel = new System.Windows.Forms.Label();
            bytesPerSectorNumericUpDown = new System.Windows.Forms.NumericUpDown();
            bytesPerSectorLabel = new System.Windows.Forms.Label();
            oemIdTextBox = new System.Windows.Forms.TextBox();
            oemIdLabel = new System.Windows.Forms.Label();
            buttonPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)tracksNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)reservedSectorsNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fatCountNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sectorsPerTrackNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sectorsPerFatNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mediaDescriptorNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)totalSectorsNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)rootDirectoryEntriesNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sectorsPerClusterNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bytesPerSectorNumericUpDown).BeginInit();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // okButton
            // 
            okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Location = new System.Drawing.Point(422, 12);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(80, 26);
            okButton.TabIndex = 14;
            okButton.Text = "OK";
            // 
            // toolTip
            // 
            toolTip.AutoPopDelay = 10000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 100;
            toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            toolTip.ToolTipTitle = "The more you know...";
            // 
            // tracksNumericUpDown
            // 
            tracksNumericUpDown.Location = new System.Drawing.Point(411, 131);
            tracksNumericUpDown.Maximum = new decimal(new int[] { 82, 0, 0, 0 });
            tracksNumericUpDown.Minimum = new decimal(new int[] { 40, 0, 0, 0 });
            tracksNumericUpDown.Name = "tracksNumericUpDown";
            tracksNumericUpDown.Size = new System.Drawing.Size(84, 23);
            tracksNumericUpDown.TabIndex = 9;
            toolTip.SetToolTip(tracksNumericUpDown, "The number of tracks on one side of the disk.");
            tracksNumericUpDown.Value = new decimal(new int[] { 80, 0, 0, 0 });
            // 
            // tracksLabel
            // 
            tracksLabel.AutoSize = true;
            tracksLabel.Location = new System.Drawing.Point(269, 133);
            tracksLabel.Name = "tracksLabel";
            tracksLabel.Size = new System.Drawing.Size(87, 15);
            tracksLabel.TabIndex = 60;
            tracksLabel.Text = "Tracks per side:";
            toolTip.SetToolTip(tracksLabel, "The number of tracks on one side of the disk.");
            // 
            // reservedSectorsNumericUpDown
            // 
            reservedSectorsNumericUpDown.Location = new System.Drawing.Point(411, 44);
            reservedSectorsNumericUpDown.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            reservedSectorsNumericUpDown.Name = "reservedSectorsNumericUpDown";
            reservedSectorsNumericUpDown.Size = new System.Drawing.Size(84, 23);
            reservedSectorsNumericUpDown.TabIndex = 3;
            toolTip.SetToolTip(reservedSectorsNumericUpDown, "The number of sectors before the first file allocation table (FAT).\r\nFor most FAT12-formatted floppies, there is only one reserved\r\nsector - the boot sector.\r\n");
            reservedSectorsNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // fatCountNumericUpDown
            // 
            fatCountNumericUpDown.Location = new System.Drawing.Point(143, 102);
            fatCountNumericUpDown.Maximum = new decimal(new int[] { 9, 0, 0, 0 });
            fatCountNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            fatCountNumericUpDown.Name = "fatCountNumericUpDown";
            fatCountNumericUpDown.Size = new System.Drawing.Size(113, 23);
            fatCountNumericUpDown.TabIndex = 6;
            toolTip.SetToolTip(fatCountNumericUpDown, "The number of file allocation tables (FAT). On FAT12-formatted\r\nfloppy disks, this value is basically always 2.\r\n");
            fatCountNumericUpDown.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // fileSystemTypeTextBox
            // 
            fileSystemTypeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            fileSystemTypeTextBox.Location = new System.Drawing.Point(143, 73);
            fileSystemTypeTextBox.MaxLength = 8;
            fileSystemTypeTextBox.Name = "fileSystemTypeTextBox";
            fileSystemTypeTextBox.Size = new System.Drawing.Size(113, 23);
            fileSystemTypeTextBox.TabIndex = 4;
            fileSystemTypeTextBox.Text = "FAT12";
            toolTip.SetToolTip(fileSystemTypeTextBox, resources.GetString("fileSystemTypeTextBox.ToolTip"));
            // 
            // fileSystemTypeLabel
            // 
            fileSystemTypeLabel.AutoSize = true;
            fileSystemTypeLabel.Location = new System.Drawing.Point(12, 75);
            fileSystemTypeLabel.Name = "fileSystemTypeLabel";
            fileSystemTypeLabel.Size = new System.Drawing.Size(94, 15);
            fileSystemTypeLabel.TabIndex = 57;
            fileSystemTypeLabel.Text = "File system type:";
            toolTip.SetToolTip(fileSystemTypeLabel, resources.GetString("fileSystemTypeLabel.ToolTip"));
            // 
            // serialNumberTextBox
            // 
            serialNumberTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            serialNumberTextBox.Location = new System.Drawing.Point(143, 44);
            serialNumberTextBox.MaxLength = 8;
            serialNumberTextBox.Name = "serialNumberTextBox";
            serialNumberTextBox.Size = new System.Drawing.Size(113, 23);
            serialNumberTextBox.TabIndex = 2;
            toolTip.SetToolTip(serialNumberTextBox, "This field can be used to detect when the disk was ejected\r\nand a different disk was inserted.");
            // 
            // serialNumberLabel
            // 
            serialNumberLabel.AutoSize = true;
            serialNumberLabel.Location = new System.Drawing.Point(12, 47);
            serialNumberLabel.Name = "serialNumberLabel";
            serialNumberLabel.Size = new System.Drawing.Size(125, 15);
            serialNumberLabel.TabIndex = 56;
            serialNumberLabel.Text = "Volume serial number:\r\n";
            toolTip.SetToolTip(serialNumberLabel, "This field can be used to detect when the disk was ejected\r\nand a different disk was inserted.\r\n");
            // 
            // sidesComboBox
            // 
            sidesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            sidesComboBox.FormattingEnabled = true;
            sidesComboBox.Location = new System.Drawing.Point(411, 189);
            sidesComboBox.Name = "sidesComboBox";
            sidesComboBox.Size = new System.Drawing.Size(84, 23);
            sidesComboBox.TabIndex = 13;
            toolTip.SetToolTip(sidesComboBox, "Determines whether a disk is single-sided (SS) or double-sided (DS).");
            // 
            // sidesLabel
            // 
            sidesLabel.AutoSize = true;
            sidesLabel.Location = new System.Drawing.Point(269, 190);
            sidesLabel.Name = "sidesLabel";
            sidesLabel.Size = new System.Drawing.Size(37, 15);
            sidesLabel.TabIndex = 55;
            sidesLabel.Text = "Sides:";
            toolTip.SetToolTip(sidesLabel, "Determines whether a disk is single-sided (SS) or double-sided (DS).");
            // 
            // sectorsPerTrackNumericUpDown
            // 
            sectorsPerTrackNumericUpDown.Location = new System.Drawing.Point(143, 188);
            sectorsPerTrackNumericUpDown.Maximum = new decimal(new int[] { 36, 0, 0, 0 });
            sectorsPerTrackNumericUpDown.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            sectorsPerTrackNumericUpDown.Name = "sectorsPerTrackNumericUpDown";
            sectorsPerTrackNumericUpDown.Size = new System.Drawing.Size(113, 23);
            sectorsPerTrackNumericUpDown.TabIndex = 12;
            toolTip.SetToolTip(sectorsPerTrackNumericUpDown, "The number of sectors in one track.");
            sectorsPerTrackNumericUpDown.Value = new decimal(new int[] { 18, 0, 0, 0 });
            // 
            // sectorsPerTrackLabel
            // 
            sectorsPerTrackLabel.AutoSize = true;
            sectorsPerTrackLabel.Location = new System.Drawing.Point(12, 190);
            sectorsPerTrackLabel.Name = "sectorsPerTrackLabel";
            sectorsPerTrackLabel.Size = new System.Drawing.Size(97, 15);
            sectorsPerTrackLabel.TabIndex = 54;
            sectorsPerTrackLabel.Text = "Sectors per track:";
            toolTip.SetToolTip(sectorsPerTrackLabel, "The number of sectors in one track.");
            // 
            // sectorsPerFatNumericUpDown
            // 
            sectorsPerFatNumericUpDown.Location = new System.Drawing.Point(411, 14);
            sectorsPerFatNumericUpDown.Maximum = new decimal(new int[] { 11, 0, 0, 0 });
            sectorsPerFatNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            sectorsPerFatNumericUpDown.Name = "sectorsPerFatNumericUpDown";
            sectorsPerFatNumericUpDown.Size = new System.Drawing.Size(84, 23);
            sectorsPerFatNumericUpDown.TabIndex = 1;
            toolTip.SetToolTip(sectorsPerFatNumericUpDown, "Defines the number of sectors occupied by each file\r\nallocation table (FAT).");
            sectorsPerFatNumericUpDown.Value = new decimal(new int[] { 9, 0, 0, 0 });
            // 
            // sectorsPerFatLabel
            // 
            sectorsPerFatLabel.AutoSize = true;
            sectorsPerFatLabel.Location = new System.Drawing.Point(269, 17);
            sectorsPerFatLabel.Name = "sectorsPerFatLabel";
            sectorsPerFatLabel.Size = new System.Drawing.Size(90, 15);
            sectorsPerFatLabel.TabIndex = 53;
            sectorsPerFatLabel.Text = "Sectors per FAT:";
            toolTip.SetToolTip(sectorsPerFatLabel, "Defines the number of sectors occupied by each file\r\nallocation table (FAT).");
            // 
            // mediaDescriptorNumericUpDown
            // 
            mediaDescriptorNumericUpDown.Hexadecimal = true;
            mediaDescriptorNumericUpDown.Location = new System.Drawing.Point(411, 160);
            mediaDescriptorNumericUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            mediaDescriptorNumericUpDown.Minimum = new decimal(new int[] { 237, 0, 0, 0 });
            mediaDescriptorNumericUpDown.Name = "mediaDescriptorNumericUpDown";
            mediaDescriptorNumericUpDown.Size = new System.Drawing.Size(84, 23);
            mediaDescriptorNumericUpDown.TabIndex = 11;
            toolTip.SetToolTip(mediaDescriptorNumericUpDown, resources.GetString("mediaDescriptorNumericUpDown.ToolTip"));
            mediaDescriptorNumericUpDown.Value = new decimal(new int[] { 240, 0, 0, 0 });
            // 
            // mediaDescriptorLabel
            // 
            mediaDescriptorLabel.AutoSize = true;
            mediaDescriptorLabel.Location = new System.Drawing.Point(269, 162);
            mediaDescriptorLabel.Name = "mediaDescriptorLabel";
            mediaDescriptorLabel.Size = new System.Drawing.Size(99, 15);
            mediaDescriptorLabel.TabIndex = 52;
            mediaDescriptorLabel.Text = "Media descriptor:";
            toolTip.SetToolTip(mediaDescriptorLabel, resources.GetString("mediaDescriptorLabel.ToolTip"));
            // 
            // totalSectorsNumericUpDown
            // 
            totalSectorsNumericUpDown.Location = new System.Drawing.Point(411, 73);
            totalSectorsNumericUpDown.Maximum = new decimal(new int[] { 7360, 0, 0, 0 });
            totalSectorsNumericUpDown.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            totalSectorsNumericUpDown.Increment = new decimal(new int[] { 0, 0, 0, 0 });
            totalSectorsNumericUpDown.ReadOnly = true;
            totalSectorsNumericUpDown.Name = "totalSectorsNumericUpDown";
            totalSectorsNumericUpDown.Size = new System.Drawing.Size(84, 23);
            totalSectorsNumericUpDown.TabIndex = 5;
            toolTip.SetToolTip(totalSectorsNumericUpDown, "This field specifies the total number of sectors on a disk. The number\r\nis calculated by multiplying the number of tracks, sectors per track and\r\nnumber of sides (tracks * SPT * sides).\r\n");
            totalSectorsNumericUpDown.Value = new decimal(new int[] { 2880, 0, 0, 0 });
            // 
            // totalSectorsLabel
            // 
            totalSectorsLabel.AutoSize = true;
            totalSectorsLabel.Location = new System.Drawing.Point(269, 75);
            totalSectorsLabel.Name = "totalSectorsLabel";
            totalSectorsLabel.Size = new System.Drawing.Size(135, 15);
            totalSectorsLabel.TabIndex = 49;
            totalSectorsLabel.Text = "Total number of sectors:";
            toolTip.SetToolTip(totalSectorsLabel, "This field specifies the total number of sectors on a disk. The number\r\nis calculated by multiplying the number of tracks, sectors per track and\r\nnumber of sides (tracks * SPT * sides).");
            // 
            // rootDirectoryEntriesNumericUpDown
            // 
            rootDirectoryEntriesNumericUpDown.Increment = new decimal(new int[] { 16, 0, 0, 0 });
            rootDirectoryEntriesNumericUpDown.Location = new System.Drawing.Point(411, 102);
            rootDirectoryEntriesNumericUpDown.Maximum = new decimal(new int[] { 320, 0, 0, 0 });
            rootDirectoryEntriesNumericUpDown.Minimum = new decimal(new int[] { 16, 0, 0, 0 });
            rootDirectoryEntriesNumericUpDown.Name = "rootDirectoryEntriesNumericUpDown";
            rootDirectoryEntriesNumericUpDown.Size = new System.Drawing.Size(84, 23);
            rootDirectoryEntriesNumericUpDown.TabIndex = 7;
            toolTip.SetToolTip(rootDirectoryEntriesNumericUpDown, "This is the maximum number of files that can be located in the\r\nroot directory of the disk. ");
            rootDirectoryEntriesNumericUpDown.Value = new decimal(new int[] { 224, 0, 0, 0 });
            // 
            // rootDirectoryEntriesLabel
            // 
            rootDirectoryEntriesLabel.AutoSize = true;
            rootDirectoryEntriesLabel.Location = new System.Drawing.Point(269, 104);
            rootDirectoryEntriesLabel.Name = "rootDirectoryEntriesLabel";
            rootDirectoryEntriesLabel.Size = new System.Drawing.Size(123, 15);
            rootDirectoryEntriesLabel.TabIndex = 46;
            rootDirectoryEntriesLabel.Text = "Root directory entries:";
            toolTip.SetToolTip(rootDirectoryEntriesLabel, "This is the maximum number of files that can be located in the\r\nroot directory of the disk.");
            // 
            // fatCountLabel
            // 
            fatCountLabel.AutoSize = true;
            fatCountLabel.Location = new System.Drawing.Point(12, 104);
            fatCountLabel.Name = "fatCountLabel";
            fatCountLabel.Size = new System.Drawing.Size(94, 15);
            fatCountLabel.TabIndex = 45;
            fatCountLabel.Text = "Number of FATs:";
            toolTip.SetToolTip(fatCountLabel, "The number of file allocation tables (FAT). On FAT12-formatted\r\nfloppy disks, this value is basically always 2.");
            // 
            // reservedSectorsLabel
            // 
            reservedSectorsLabel.AutoSize = true;
            reservedSectorsLabel.Location = new System.Drawing.Point(269, 47);
            reservedSectorsLabel.Name = "reservedSectorsLabel";
            reservedSectorsLabel.Size = new System.Drawing.Size(97, 15);
            reservedSectorsLabel.TabIndex = 43;
            reservedSectorsLabel.Text = "Reserved sectors:";
            toolTip.SetToolTip(reservedSectorsLabel, "The number of sectors before the first file allocation table (FAT).\r\nFor most FAT12-formatted floppies, there is only one reserved\r\nsector - the boot sector.\r\n");
            // 
            // sectorsPerClusterNumericUpDown
            // 
            sectorsPerClusterNumericUpDown.Location = new System.Drawing.Point(143, 159);
            sectorsPerClusterNumericUpDown.Maximum = new decimal(new int[] { 128, 0, 0, 0 });
            sectorsPerClusterNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            sectorsPerClusterNumericUpDown.Name = "sectorsPerClusterNumericUpDown";
            sectorsPerClusterNumericUpDown.Size = new System.Drawing.Size(113, 23);
            sectorsPerClusterNumericUpDown.TabIndex = 10;
            toolTip.SetToolTip(sectorsPerClusterNumericUpDown, "This field determines the number of sectors that make up one cluster.\r\n");
            sectorsPerClusterNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // sectorsPerClusterLabel
            // 
            sectorsPerClusterLabel.AutoSize = true;
            sectorsPerClusterLabel.Location = new System.Drawing.Point(12, 162);
            sectorsPerClusterLabel.Name = "sectorsPerClusterLabel";
            sectorsPerClusterLabel.Size = new System.Drawing.Size(106, 15);
            sectorsPerClusterLabel.TabIndex = 39;
            sectorsPerClusterLabel.Text = "Sectors per cluster:";
            toolTip.SetToolTip(sectorsPerClusterLabel, "This field determines the number of sectors that make up one cluster.");
            // 
            // bytesPerSectorNumericUpDown
            // 
            bytesPerSectorNumericUpDown.Increment = new decimal(new int[] { 128, 0, 0, 0 });
            bytesPerSectorNumericUpDown.Location = new System.Drawing.Point(143, 130);
            bytesPerSectorNumericUpDown.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            bytesPerSectorNumericUpDown.Minimum = new decimal(new int[] { 128, 0, 0, 0 });
            bytesPerSectorNumericUpDown.Name = "bytesPerSectorNumericUpDown";
            bytesPerSectorNumericUpDown.Size = new System.Drawing.Size(113, 23);
            bytesPerSectorNumericUpDown.TabIndex = 8;
            toolTip.SetToolTip(bytesPerSectorNumericUpDown, "This field determines the size of a sector in bytes.");
            bytesPerSectorNumericUpDown.Value = new decimal(new int[] { 512, 0, 0, 0 });
            // 
            // bytesPerSectorLabel
            // 
            bytesPerSectorLabel.AutoSize = true;
            bytesPerSectorLabel.Location = new System.Drawing.Point(12, 133);
            bytesPerSectorLabel.Name = "bytesPerSectorLabel";
            bytesPerSectorLabel.Size = new System.Drawing.Size(93, 15);
            bytesPerSectorLabel.TabIndex = 37;
            bytesPerSectorLabel.Text = "Bytes per sector:";
            toolTip.SetToolTip(bytesPerSectorLabel, "This field determines the size of a sector in bytes.");
            // 
            // oemIdTextBox
            // 
            oemIdTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            oemIdTextBox.Location = new System.Drawing.Point(143, 14);
            oemIdTextBox.MaxLength = 8;
            oemIdTextBox.Name = "oemIdTextBox";
            oemIdTextBox.Size = new System.Drawing.Size(113, 23);
            oemIdTextBox.TabIndex = 0;
            oemIdTextBox.Text = "MSDOS5.0";
            toolTip.SetToolTip(oemIdTextBox, "The OEM ID field is often used to identify the system that formatted\r\nthe disk. Using non-standard values could result in the operating\r\nsystem or program not recognizing the disk.");
            // 
            // oemIdLabel
            // 
            oemIdLabel.AutoSize = true;
            oemIdLabel.Location = new System.Drawing.Point(12, 17);
            oemIdLabel.Name = "oemIdLabel";
            oemIdLabel.Size = new System.Drawing.Size(50, 15);
            oemIdLabel.TabIndex = 34;
            oemIdLabel.Text = "OEM ID:";
            toolTip.SetToolTip(oemIdLabel, "The OEM ID field is often used to identify the system that formatted\r\nthe disk. Using non-standard values could result in the operating\r\nsystem or program not recognizing the disk.\r\n");
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = System.Drawing.SystemColors.Control;
            buttonPanel.Controls.Add(okButton);
            buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonPanel.Location = new System.Drawing.Point(0, 226);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(514, 50);
            buttonPanel.TabIndex = 18;
            // 
            // NewImageAdvancedDialog
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = okButton;
            ClientSize = new System.Drawing.Size(514, 276);
            Controls.Add(tracksNumericUpDown);
            Controls.Add(tracksLabel);
            Controls.Add(reservedSectorsNumericUpDown);
            Controls.Add(fatCountNumericUpDown);
            Controls.Add(fileSystemTypeTextBox);
            Controls.Add(fileSystemTypeLabel);
            Controls.Add(serialNumberTextBox);
            Controls.Add(serialNumberLabel);
            Controls.Add(sidesComboBox);
            Controls.Add(sidesLabel);
            Controls.Add(sectorsPerTrackNumericUpDown);
            Controls.Add(sectorsPerTrackLabel);
            Controls.Add(sectorsPerFatNumericUpDown);
            Controls.Add(sectorsPerFatLabel);
            Controls.Add(mediaDescriptorNumericUpDown);
            Controls.Add(mediaDescriptorLabel);
            Controls.Add(totalSectorsNumericUpDown);
            Controls.Add(totalSectorsLabel);
            Controls.Add(rootDirectoryEntriesNumericUpDown);
            Controls.Add(rootDirectoryEntriesLabel);
            Controls.Add(fatCountLabel);
            Controls.Add(reservedSectorsLabel);
            Controls.Add(sectorsPerClusterNumericUpDown);
            Controls.Add(sectorsPerClusterLabel);
            Controls.Add(bytesPerSectorNumericUpDown);
            Controls.Add(bytesPerSectorLabel);
            Controls.Add(oemIdTextBox);
            Controls.Add(oemIdLabel);
            Controls.Add(buttonPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NewImageAdvancedDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Advanced options";
            ((System.ComponentModel.ISupportInitialize)tracksNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)reservedSectorsNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)fatCountNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)sectorsPerTrackNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)sectorsPerFatNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)mediaDescriptorNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)totalSectorsNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)rootDirectoryEntriesNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)sectorsPerClusterNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)bytesPerSectorNumericUpDown).EndInit();
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Label tracksLabel;
        private System.Windows.Forms.Label fileSystemTypeLabel;
        private System.Windows.Forms.Label serialNumberLabel;
        private System.Windows.Forms.Label sidesLabel;
        private System.Windows.Forms.Label sectorsPerTrackLabel;
        private System.Windows.Forms.Label sectorsPerFatLabel;
        private System.Windows.Forms.Label mediaDescriptorLabel;
        private System.Windows.Forms.Label totalSectorsLabel;
        private System.Windows.Forms.Label rootDirectoryEntriesLabel;
        private System.Windows.Forms.Label fatCountLabel;
        private System.Windows.Forms.Label reservedSectorsLabel;
        private System.Windows.Forms.Label sectorsPerClusterLabel;
        private System.Windows.Forms.Label bytesPerSectorLabel;
        private System.Windows.Forms.Label oemIdLabel;
        private System.Windows.Forms.NumericUpDown tracksNumericUpDown;
        private System.Windows.Forms.NumericUpDown reservedSectorsNumericUpDown;
        private System.Windows.Forms.ComboBox sidesComboBox;
        private System.Windows.Forms.NumericUpDown sectorsPerFatNumericUpDown;
        private System.Windows.Forms.NumericUpDown mediaDescriptorNumericUpDown;
        private System.Windows.Forms.NumericUpDown totalSectorsNumericUpDown;
        private System.Windows.Forms.NumericUpDown rootDirectoryEntriesNumericUpDown;
        private System.Windows.Forms.NumericUpDown fatCountNumericUpDown;
        private System.Windows.Forms.TextBox fileSystemTypeTextBox;
        private System.Windows.Forms.TextBox serialNumberTextBox;
        private System.Windows.Forms.NumericUpDown sectorsPerTrackNumericUpDown;
        private System.Windows.Forms.NumericUpDown sectorsPerClusterNumericUpDown;
        private System.Windows.Forms.NumericUpDown bytesPerSectorNumericUpDown;
        private System.Windows.Forms.TextBox oemIdTextBox;
    }
}
