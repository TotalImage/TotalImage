namespace TotalImage
{
    partial class dlgNewImageAdvanced
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgNewImageAdvanced));
            btnOK = new System.Windows.Forms.Button();
            toolTip = new System.Windows.Forms.ToolTip(components);
            txtFloppyTracks = new System.Windows.Forms.NumericUpDown();
            lblFloppyTracks = new System.Windows.Forms.Label();
            txtFloppyReservedSect = new System.Windows.Forms.NumericUpDown();
            txtFloppyNumFATs = new System.Windows.Forms.NumericUpDown();
            txtFloppyFSType = new System.Windows.Forms.TextBox();
            lblFloppyFSType = new System.Windows.Forms.Label();
            txtFloppySerial = new System.Windows.Forms.TextBox();
            lblFloppySerial = new System.Windows.Forms.Label();
            lstFloppySides = new System.Windows.Forms.ComboBox();
            lblFloppySides = new System.Windows.Forms.Label();
            txtFloppySPT = new System.Windows.Forms.NumericUpDown();
            lblFloppySPT = new System.Windows.Forms.Label();
            txtFloppySPF = new System.Windows.Forms.NumericUpDown();
            lblFloppySPF = new System.Windows.Forms.Label();
            txtFloppyMediaDesc = new System.Windows.Forms.NumericUpDown();
            lblFloppyMediaDesc = new System.Windows.Forms.Label();
            txtFloppyTotalSect = new System.Windows.Forms.NumericUpDown();
            lblFloppyTotalSect = new System.Windows.Forms.Label();
            txtFloppyRootDir = new System.Windows.Forms.NumericUpDown();
            lblFloppyRootDir = new System.Windows.Forms.Label();
            lblFloppyNumFATs = new System.Windows.Forms.Label();
            lblFloppyReservedSect = new System.Windows.Forms.Label();
            txtFloppySPC = new System.Windows.Forms.NumericUpDown();
            lblFloppySPC = new System.Windows.Forms.Label();
            txtFloppyBPS = new System.Windows.Forms.NumericUpDown();
            lblFloppyBPS = new System.Windows.Forms.Label();
            txtFloppyOEMID = new System.Windows.Forms.TextBox();
            lblFloppyOEMID = new System.Windows.Forms.Label();
            pnlBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)txtFloppyTracks).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppyReservedSect).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppyNumFATs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppySPT).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppySPF).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppyMediaDesc).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppyTotalSect).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppyRootDir).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppySPC).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppyBPS).BeginInit();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(422, 12);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 14;
            btnOK.Text = "OK";
            btnOK.Click += btnOK_Click;
            // 
            // toolTip
            // 
            toolTip.AutoPopDelay = 10000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 100;
            toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            toolTip.ToolTipTitle = "The more you know...";
            // 
            // txtFloppyTracks
            // 
            txtFloppyTracks.Location = new System.Drawing.Point(411, 131);
            txtFloppyTracks.Maximum = new decimal(new int[] { 82, 0, 0, 0 });
            txtFloppyTracks.Minimum = new decimal(new int[] { 40, 0, 0, 0 });
            txtFloppyTracks.Name = "txtFloppyTracks";
            txtFloppyTracks.Size = new System.Drawing.Size(84, 23);
            txtFloppyTracks.TabIndex = 9;
            toolTip.SetToolTip(txtFloppyTracks, "The number of tracks on one side of the disk.");
            txtFloppyTracks.Value = new decimal(new int[] { 80, 0, 0, 0 });
            // 
            // lblFloppyTracks
            // 
            lblFloppyTracks.AutoSize = true;
            lblFloppyTracks.Location = new System.Drawing.Point(269, 133);
            lblFloppyTracks.Name = "lblFloppyTracks";
            lblFloppyTracks.Size = new System.Drawing.Size(87, 15);
            lblFloppyTracks.TabIndex = 60;
            lblFloppyTracks.Text = "Tracks per side:";
            toolTip.SetToolTip(lblFloppyTracks, "The number of tracks on one side of the disk.");
            // 
            // txtFloppyReservedSect
            // 
            txtFloppyReservedSect.Location = new System.Drawing.Point(411, 44);
            txtFloppyReservedSect.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            txtFloppyReservedSect.Name = "txtFloppyReservedSect";
            txtFloppyReservedSect.Size = new System.Drawing.Size(84, 23);
            txtFloppyReservedSect.TabIndex = 3;
            toolTip.SetToolTip(txtFloppyReservedSect, "The number of sectors before the first file allocation table (FAT).\r\nFor most FAT12-formatted floppies, there is only one reserved\r\nsector - the boot sector.\r\n");
            txtFloppyReservedSect.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // txtFloppyNumFATs
            // 
            txtFloppyNumFATs.Location = new System.Drawing.Point(143, 102);
            txtFloppyNumFATs.Maximum = new decimal(new int[] { 9, 0, 0, 0 });
            txtFloppyNumFATs.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            txtFloppyNumFATs.Name = "txtFloppyNumFATs";
            txtFloppyNumFATs.Size = new System.Drawing.Size(113, 23);
            txtFloppyNumFATs.TabIndex = 6;
            toolTip.SetToolTip(txtFloppyNumFATs, "The number of file allocation tables (FAT). On FAT12-formatted\r\nfloppy disks, this value is basically always 2.\r\n");
            txtFloppyNumFATs.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // txtFloppyFSType
            // 
            txtFloppyFSType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtFloppyFSType.Location = new System.Drawing.Point(143, 73);
            txtFloppyFSType.MaxLength = 8;
            txtFloppyFSType.Name = "txtFloppyFSType";
            txtFloppyFSType.Size = new System.Drawing.Size(113, 23);
            txtFloppyFSType.TabIndex = 4;
            txtFloppyFSType.Text = "FAT12";
            toolTip.SetToolTip(txtFloppyFSType, resources.GetString("txtFloppyFSType.ToolTip"));
            // 
            // lblFloppyFSType
            // 
            lblFloppyFSType.AutoSize = true;
            lblFloppyFSType.Location = new System.Drawing.Point(12, 75);
            lblFloppyFSType.Name = "lblFloppyFSType";
            lblFloppyFSType.Size = new System.Drawing.Size(94, 15);
            lblFloppyFSType.TabIndex = 57;
            lblFloppyFSType.Text = "File system type:";
            toolTip.SetToolTip(lblFloppyFSType, resources.GetString("lblFloppyFSType.ToolTip"));
            // 
            // txtFloppySerial
            // 
            txtFloppySerial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtFloppySerial.Location = new System.Drawing.Point(143, 44);
            txtFloppySerial.MaxLength = 8;
            txtFloppySerial.Name = "txtFloppySerial";
            txtFloppySerial.Size = new System.Drawing.Size(113, 23);
            txtFloppySerial.TabIndex = 2;
            toolTip.SetToolTip(txtFloppySerial, "This field can be used to detect when the disk was ejected\r\nand a different disk was inserted.");
            // 
            // lblFloppySerial
            // 
            lblFloppySerial.AutoSize = true;
            lblFloppySerial.Location = new System.Drawing.Point(12, 47);
            lblFloppySerial.Name = "lblFloppySerial";
            lblFloppySerial.Size = new System.Drawing.Size(125, 15);
            lblFloppySerial.TabIndex = 56;
            lblFloppySerial.Text = "Volume serial number:\r\n";
            toolTip.SetToolTip(lblFloppySerial, "This field can be used to detect when the disk was ejected\r\nand a different disk was inserted.\r\n");
            // 
            // lstFloppySides
            // 
            lstFloppySides.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstFloppySides.FormattingEnabled = true;
            lstFloppySides.Items.AddRange(new object[] { "1", "2" });
            lstFloppySides.Location = new System.Drawing.Point(411, 189);
            lstFloppySides.Name = "lstFloppySides";
            lstFloppySides.Size = new System.Drawing.Size(84, 23);
            lstFloppySides.TabIndex = 13;
            toolTip.SetToolTip(lstFloppySides, "Determines whether a disk is single-sided (SS) or double-sided (DS).");
            // 
            // lblFloppySides
            // 
            lblFloppySides.AutoSize = true;
            lblFloppySides.Location = new System.Drawing.Point(269, 190);
            lblFloppySides.Name = "lblFloppySides";
            lblFloppySides.Size = new System.Drawing.Size(37, 15);
            lblFloppySides.TabIndex = 55;
            lblFloppySides.Text = "Sides:";
            toolTip.SetToolTip(lblFloppySides, "Determines whether a disk is single-sided (SS) or double-sided (DS).");
            // 
            // txtFloppySPT
            // 
            txtFloppySPT.Location = new System.Drawing.Point(143, 188);
            txtFloppySPT.Maximum = new decimal(new int[] { 36, 0, 0, 0 });
            txtFloppySPT.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            txtFloppySPT.Name = "txtFloppySPT";
            txtFloppySPT.Size = new System.Drawing.Size(113, 23);
            txtFloppySPT.TabIndex = 12;
            toolTip.SetToolTip(txtFloppySPT, "The number of sectors in one track.");
            txtFloppySPT.Value = new decimal(new int[] { 18, 0, 0, 0 });
            // 
            // lblFloppySPT
            // 
            lblFloppySPT.AutoSize = true;
            lblFloppySPT.Location = new System.Drawing.Point(12, 190);
            lblFloppySPT.Name = "lblFloppySPT";
            lblFloppySPT.Size = new System.Drawing.Size(97, 15);
            lblFloppySPT.TabIndex = 54;
            lblFloppySPT.Text = "Sectors per track:";
            toolTip.SetToolTip(lblFloppySPT, "The number of sectors in one track.");
            // 
            // txtFloppySPF
            // 
            txtFloppySPF.Location = new System.Drawing.Point(411, 14);
            txtFloppySPF.Maximum = new decimal(new int[] { 11, 0, 0, 0 });
            txtFloppySPF.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            txtFloppySPF.Name = "txtFloppySPF";
            txtFloppySPF.Size = new System.Drawing.Size(84, 23);
            txtFloppySPF.TabIndex = 1;
            toolTip.SetToolTip(txtFloppySPF, "Defines the number of sectors occupied by each file\r\nallocation table (FAT).");
            txtFloppySPF.Value = new decimal(new int[] { 9, 0, 0, 0 });
            // 
            // lblFloppySPF
            // 
            lblFloppySPF.AutoSize = true;
            lblFloppySPF.Location = new System.Drawing.Point(269, 17);
            lblFloppySPF.Name = "lblFloppySPF";
            lblFloppySPF.Size = new System.Drawing.Size(90, 15);
            lblFloppySPF.TabIndex = 53;
            lblFloppySPF.Text = "Sectors per FAT:";
            toolTip.SetToolTip(lblFloppySPF, "Defines the number of sectors occupied by each file\r\nallocation table (FAT).");
            // 
            // txtFloppyMediaDesc
            // 
            txtFloppyMediaDesc.Hexadecimal = true;
            txtFloppyMediaDesc.Location = new System.Drawing.Point(411, 160);
            txtFloppyMediaDesc.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            txtFloppyMediaDesc.Minimum = new decimal(new int[] { 237, 0, 0, 0 });
            txtFloppyMediaDesc.Name = "txtFloppyMediaDesc";
            txtFloppyMediaDesc.Size = new System.Drawing.Size(84, 23);
            txtFloppyMediaDesc.TabIndex = 11;
            toolTip.SetToolTip(txtFloppyMediaDesc, resources.GetString("txtFloppyMediaDesc.ToolTip"));
            txtFloppyMediaDesc.Value = new decimal(new int[] { 240, 0, 0, 0 });
            // 
            // lblFloppyMediaDesc
            // 
            lblFloppyMediaDesc.AutoSize = true;
            lblFloppyMediaDesc.Location = new System.Drawing.Point(269, 162);
            lblFloppyMediaDesc.Name = "lblFloppyMediaDesc";
            lblFloppyMediaDesc.Size = new System.Drawing.Size(99, 15);
            lblFloppyMediaDesc.TabIndex = 52;
            lblFloppyMediaDesc.Text = "Media descriptor:";
            toolTip.SetToolTip(lblFloppyMediaDesc, resources.GetString("lblFloppyMediaDesc.ToolTip"));
            // 
            // txtFloppyTotalSect
            // 
            txtFloppyTotalSect.Location = new System.Drawing.Point(411, 73);
            txtFloppyTotalSect.Maximum = new decimal(new int[] { 7360, 0, 0, 0 });
            txtFloppyTotalSect.Minimum = new decimal(new int[] { 320, 0, 0, 0 });
            txtFloppyTotalSect.Name = "txtFloppyTotalSect";
            txtFloppyTotalSect.Size = new System.Drawing.Size(84, 23);
            txtFloppyTotalSect.TabIndex = 5;
            toolTip.SetToolTip(txtFloppyTotalSect, "This field specifies the total number of sectors on a disk. The number\r\nis calculated by multiplying the number of tracks, sectors per track and\r\nnumber of sides (tracks * SPT * sides).\r\n");
            txtFloppyTotalSect.Value = new decimal(new int[] { 2880, 0, 0, 0 });
            // 
            // lblFloppyTotalSect
            // 
            lblFloppyTotalSect.AutoSize = true;
            lblFloppyTotalSect.Location = new System.Drawing.Point(269, 75);
            lblFloppyTotalSect.Name = "lblFloppyTotalSect";
            lblFloppyTotalSect.Size = new System.Drawing.Size(135, 15);
            lblFloppyTotalSect.TabIndex = 49;
            lblFloppyTotalSect.Text = "Total number of sectors:";
            toolTip.SetToolTip(lblFloppyTotalSect, "This field specifies the total number of sectors on a disk. The number\r\nis calculated by multiplying the number of tracks, sectors per track and\r\nnumber of sides (tracks * SPT * sides).");
            // 
            // txtFloppyRootDir
            // 
            txtFloppyRootDir.Increment = new decimal(new int[] { 16, 0, 0, 0 });
            txtFloppyRootDir.Location = new System.Drawing.Point(411, 102);
            txtFloppyRootDir.Maximum = new decimal(new int[] { 320, 0, 0, 0 });
            txtFloppyRootDir.Minimum = new decimal(new int[] { 16, 0, 0, 0 });
            txtFloppyRootDir.Name = "txtFloppyRootDir";
            txtFloppyRootDir.Size = new System.Drawing.Size(84, 23);
            txtFloppyRootDir.TabIndex = 7;
            toolTip.SetToolTip(txtFloppyRootDir, "This is the maximum number of files that can be located in the\r\nroot directory of the disk. ");
            txtFloppyRootDir.Value = new decimal(new int[] { 224, 0, 0, 0 });
            // 
            // lblFloppyRootDir
            // 
            lblFloppyRootDir.AutoSize = true;
            lblFloppyRootDir.Location = new System.Drawing.Point(269, 104);
            lblFloppyRootDir.Name = "lblFloppyRootDir";
            lblFloppyRootDir.Size = new System.Drawing.Size(123, 15);
            lblFloppyRootDir.TabIndex = 46;
            lblFloppyRootDir.Text = "Root directory entries:";
            toolTip.SetToolTip(lblFloppyRootDir, "This is the maximum number of files that can be located in the\r\nroot directory of the disk.");
            // 
            // lblFloppyNumFATs
            // 
            lblFloppyNumFATs.AutoSize = true;
            lblFloppyNumFATs.Location = new System.Drawing.Point(12, 104);
            lblFloppyNumFATs.Name = "lblFloppyNumFATs";
            lblFloppyNumFATs.Size = new System.Drawing.Size(94, 15);
            lblFloppyNumFATs.TabIndex = 45;
            lblFloppyNumFATs.Text = "Number of FATs:";
            toolTip.SetToolTip(lblFloppyNumFATs, "The number of file allocation tables (FAT). On FAT12-formatted\r\nfloppy disks, this value is basically always 2.");
            // 
            // lblFloppyReservedSect
            // 
            lblFloppyReservedSect.AutoSize = true;
            lblFloppyReservedSect.Location = new System.Drawing.Point(269, 47);
            lblFloppyReservedSect.Name = "lblFloppyReservedSect";
            lblFloppyReservedSect.Size = new System.Drawing.Size(97, 15);
            lblFloppyReservedSect.TabIndex = 43;
            lblFloppyReservedSect.Text = "Reserved sectors:";
            toolTip.SetToolTip(lblFloppyReservedSect, "The number of sectors before the first file allocation table (FAT).\r\nFor most FAT12-formatted floppies, there is only one reserved\r\nsector - the boot sector.\r\n");
            // 
            // txtFloppySPC
            // 
            txtFloppySPC.Location = new System.Drawing.Point(143, 159);
            txtFloppySPC.Maximum = new decimal(new int[] { 128, 0, 0, 0 });
            txtFloppySPC.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            txtFloppySPC.Name = "txtFloppySPC";
            txtFloppySPC.Size = new System.Drawing.Size(113, 23);
            txtFloppySPC.TabIndex = 10;
            toolTip.SetToolTip(txtFloppySPC, "This field determines the number of sectors that make up one cluster.\r\n");
            txtFloppySPC.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblFloppySPC
            // 
            lblFloppySPC.AutoSize = true;
            lblFloppySPC.Location = new System.Drawing.Point(12, 162);
            lblFloppySPC.Name = "lblFloppySPC";
            lblFloppySPC.Size = new System.Drawing.Size(106, 15);
            lblFloppySPC.TabIndex = 39;
            lblFloppySPC.Text = "Sectors per cluster:";
            toolTip.SetToolTip(lblFloppySPC, "This field determines the number of sectors that make up one cluster.");
            // 
            // txtFloppyBPS
            // 
            txtFloppyBPS.Increment = new decimal(new int[] { 128, 0, 0, 0 });
            txtFloppyBPS.Location = new System.Drawing.Point(143, 130);
            txtFloppyBPS.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            txtFloppyBPS.Minimum = new decimal(new int[] { 128, 0, 0, 0 });
            txtFloppyBPS.Name = "txtFloppyBPS";
            txtFloppyBPS.Size = new System.Drawing.Size(113, 23);
            txtFloppyBPS.TabIndex = 8;
            toolTip.SetToolTip(txtFloppyBPS, "This field determines the size of a sector in bytes.");
            txtFloppyBPS.Value = new decimal(new int[] { 512, 0, 0, 0 });
            // 
            // lblFloppyBPS
            // 
            lblFloppyBPS.AutoSize = true;
            lblFloppyBPS.Location = new System.Drawing.Point(12, 133);
            lblFloppyBPS.Name = "lblFloppyBPS";
            lblFloppyBPS.Size = new System.Drawing.Size(93, 15);
            lblFloppyBPS.TabIndex = 37;
            lblFloppyBPS.Text = "Bytes per sector:";
            toolTip.SetToolTip(lblFloppyBPS, "This field determines the size of a sector in bytes.");
            // 
            // txtFloppyOEMID
            // 
            txtFloppyOEMID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtFloppyOEMID.Location = new System.Drawing.Point(143, 14);
            txtFloppyOEMID.MaxLength = 8;
            txtFloppyOEMID.Name = "txtFloppyOEMID";
            txtFloppyOEMID.Size = new System.Drawing.Size(113, 23);
            txtFloppyOEMID.TabIndex = 0;
            txtFloppyOEMID.Text = "MSDOS5.0";
            toolTip.SetToolTip(txtFloppyOEMID, "The OEM ID field is often used to identify the system that formatted\r\nthe disk. Using non-standard values could result in the operating\r\nsystem or program not recognizing the disk.");
            // 
            // lblFloppyOEMID
            // 
            lblFloppyOEMID.AutoSize = true;
            lblFloppyOEMID.Location = new System.Drawing.Point(12, 17);
            lblFloppyOEMID.Name = "lblFloppyOEMID";
            lblFloppyOEMID.Size = new System.Drawing.Size(50, 15);
            lblFloppyOEMID.TabIndex = 34;
            lblFloppyOEMID.Text = "OEM ID:";
            toolTip.SetToolTip(lblFloppyOEMID, "The OEM ID field is often used to identify the system that formatted\r\nthe disk. Using non-standard values could result in the operating\r\nsystem or program not recognizing the disk.\r\n");
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 226);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(514, 50);
            pnlBottom.TabIndex = 18;
            // 
            // dlgNewImageAdvanced
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            ClientSize = new System.Drawing.Size(514, 276);
            Controls.Add(txtFloppyTracks);
            Controls.Add(lblFloppyTracks);
            Controls.Add(txtFloppyReservedSect);
            Controls.Add(txtFloppyNumFATs);
            Controls.Add(txtFloppyFSType);
            Controls.Add(lblFloppyFSType);
            Controls.Add(txtFloppySerial);
            Controls.Add(lblFloppySerial);
            Controls.Add(lstFloppySides);
            Controls.Add(lblFloppySides);
            Controls.Add(txtFloppySPT);
            Controls.Add(lblFloppySPT);
            Controls.Add(txtFloppySPF);
            Controls.Add(lblFloppySPF);
            Controls.Add(txtFloppyMediaDesc);
            Controls.Add(lblFloppyMediaDesc);
            Controls.Add(txtFloppyTotalSect);
            Controls.Add(lblFloppyTotalSect);
            Controls.Add(txtFloppyRootDir);
            Controls.Add(lblFloppyRootDir);
            Controls.Add(lblFloppyNumFATs);
            Controls.Add(lblFloppyReservedSect);
            Controls.Add(txtFloppySPC);
            Controls.Add(lblFloppySPC);
            Controls.Add(txtFloppyBPS);
            Controls.Add(lblFloppyBPS);
            Controls.Add(txtFloppyOEMID);
            Controls.Add(lblFloppyOEMID);
            Controls.Add(pnlBottom);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgNewImageAdvanced";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Advanced options";
            Load += dlgNewImageAdvanced_Load;
            ((System.ComponentModel.ISupportInitialize)txtFloppyTracks).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppyReservedSect).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppyNumFATs).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppySPT).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppySPF).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppyMediaDesc).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppyTotalSect).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppyRootDir).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppySPC).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtFloppyBPS).EndInit();
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblFloppyTracks;
        private System.Windows.Forms.Label lblFloppyFSType;
        private System.Windows.Forms.Label lblFloppySerial;
        private System.Windows.Forms.Label lblFloppySides;
        private System.Windows.Forms.Label lblFloppySPT;
        private System.Windows.Forms.Label lblFloppySPF;
        private System.Windows.Forms.Label lblFloppyMediaDesc;
        private System.Windows.Forms.Label lblFloppyTotalSect;
        private System.Windows.Forms.Label lblFloppyRootDir;
        private System.Windows.Forms.Label lblFloppyNumFATs;
        private System.Windows.Forms.Label lblFloppyReservedSect;
        private System.Windows.Forms.Label lblFloppySPC;
        private System.Windows.Forms.Label lblFloppyBPS;
        private System.Windows.Forms.Label lblFloppyOEMID;
        internal System.Windows.Forms.NumericUpDown txtFloppyTracks;
        internal System.Windows.Forms.NumericUpDown txtFloppyReservedSect;
        internal System.Windows.Forms.ComboBox lstFloppySides;
        internal System.Windows.Forms.NumericUpDown txtFloppySPF;
        internal System.Windows.Forms.NumericUpDown txtFloppyMediaDesc;
        internal System.Windows.Forms.NumericUpDown txtFloppyTotalSect;
        internal System.Windows.Forms.NumericUpDown txtFloppyRootDir;
        internal System.Windows.Forms.NumericUpDown txtFloppyNumFATs;
        internal System.Windows.Forms.TextBox txtFloppyFSType;
        internal System.Windows.Forms.TextBox txtFloppySerial;
        internal System.Windows.Forms.NumericUpDown txtFloppySPT;
        internal System.Windows.Forms.NumericUpDown txtFloppySPC;
        internal System.Windows.Forms.NumericUpDown txtFloppyBPS;
        internal System.Windows.Forms.TextBox txtFloppyOEMID;
    }
}
