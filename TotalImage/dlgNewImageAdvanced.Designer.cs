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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgNewImageAdvanced));
            this.btnOK = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.txtFloppyTracks = new System.Windows.Forms.NumericUpDown();
            this.lblFloppyTracks = new System.Windows.Forms.Label();
            this.txtFloppyReservedSect = new System.Windows.Forms.NumericUpDown();
            this.txtFloppyNumFATs = new System.Windows.Forms.NumericUpDown();
            this.txtFloppyFSType = new System.Windows.Forms.TextBox();
            this.lblFloppyFSType = new System.Windows.Forms.Label();
            this.txtFloppySerial = new System.Windows.Forms.TextBox();
            this.lblFloppySerial = new System.Windows.Forms.Label();
            this.lstFloppySides = new System.Windows.Forms.ComboBox();
            this.lblFloppySides = new System.Windows.Forms.Label();
            this.txtFloppySPT = new System.Windows.Forms.NumericUpDown();
            this.lblFloppySPT = new System.Windows.Forms.Label();
            this.txtFloppySPF = new System.Windows.Forms.NumericUpDown();
            this.lblFloppySPF = new System.Windows.Forms.Label();
            this.txtFloppyMediaDesc = new System.Windows.Forms.NumericUpDown();
            this.lblFloppyMediaDesc = new System.Windows.Forms.Label();
            this.txtFloppyTotalSect = new System.Windows.Forms.NumericUpDown();
            this.lblFloppyTotalSect = new System.Windows.Forms.Label();
            this.txtFloppyRootDir = new System.Windows.Forms.NumericUpDown();
            this.lblFloppyRootDir = new System.Windows.Forms.Label();
            this.lblFloppyNumFATs = new System.Windows.Forms.Label();
            this.lblFloppyReservedSect = new System.Windows.Forms.Label();
            this.txtFloppySPC = new System.Windows.Forms.NumericUpDown();
            this.lblFloppySPC = new System.Windows.Forms.Label();
            this.txtFloppyBPS = new System.Windows.Forms.NumericUpDown();
            this.lblFloppyBPS = new System.Windows.Forms.Label();
            this.txtFloppyOEMID = new System.Windows.Forms.TextBox();
            this.lblFloppyOEMID = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyTracks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyReservedSect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyNumFATs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppySPT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppySPF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyMediaDesc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyTotalSect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyRootDir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppySPC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyBPS)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(422, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 10000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "The more you know...";
            // 
            // txtFloppyTracks
            // 
            this.txtFloppyTracks.Location = new System.Drawing.Point(411, 131);
            this.txtFloppyTracks.Maximum = new decimal(new int[] {
            82,
            0,
            0,
            0});
            this.txtFloppyTracks.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.txtFloppyTracks.Name = "txtFloppyTracks";
            this.txtFloppyTracks.Size = new System.Drawing.Size(84, 23);
            this.txtFloppyTracks.TabIndex = 9;
            this.txtFloppyTracks.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.txtFloppyTracks, "The number of tracks on one side of the disk.");
            this.txtFloppyTracks.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // lblFloppyTracks
            // 
            this.lblFloppyTracks.AutoSize = true;
            this.lblFloppyTracks.Location = new System.Drawing.Point(269, 133);
            this.lblFloppyTracks.Name = "lblFloppyTracks";
            this.lblFloppyTracks.Size = new System.Drawing.Size(86, 15);
            this.lblFloppyTracks.TabIndex = 60;
            this.lblFloppyTracks.Text = "Tracks per side:";
            this.toolTip.SetToolTip(this.lblFloppyTracks, "The number of tracks on one side of the disk.");
            // 
            // txtFloppyReservedSect
            // 
            this.txtFloppyReservedSect.Location = new System.Drawing.Point(411, 44);
            this.txtFloppyReservedSect.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.txtFloppyReservedSect.Name = "txtFloppyReservedSect";
            this.txtFloppyReservedSect.Size = new System.Drawing.Size(84, 23);
            this.txtFloppyReservedSect.TabIndex = 3;
            this.txtFloppyReservedSect.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.txtFloppyReservedSect, "The number of sectors before the first file allocation table (FAT).\r\nFor most FAT" +
        "12-formatted floppies, there is only one reserved\r\nsector - the boot sector.\r\n");
            this.txtFloppyReservedSect.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtFloppyNumFATs
            // 
            this.txtFloppyNumFATs.Location = new System.Drawing.Point(143, 102);
            this.txtFloppyNumFATs.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.txtFloppyNumFATs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtFloppyNumFATs.Name = "txtFloppyNumFATs";
            this.txtFloppyNumFATs.Size = new System.Drawing.Size(113, 23);
            this.txtFloppyNumFATs.TabIndex = 6;
            this.txtFloppyNumFATs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.txtFloppyNumFATs, "The number of file allocation tables (FAT). On FAT12-formatted\r\nfloppy disks, thi" +
        "s value is basically always 2.\r\n");
            this.txtFloppyNumFATs.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // txtFloppyFSType
            // 
            this.txtFloppyFSType.Location = new System.Drawing.Point(143, 73);
            this.txtFloppyFSType.MaxLength = 8;
            this.txtFloppyFSType.Name = "txtFloppyFSType";
            this.txtFloppyFSType.Size = new System.Drawing.Size(113, 23);
            this.txtFloppyFSType.TabIndex = 4;
            this.txtFloppyFSType.Text = "FAT12";
            this.toolTip.SetToolTip(this.txtFloppyFSType, resources.GetString("txtFloppyFSType.ToolTip"));
            // 
            // lblFloppyFSType
            // 
            this.lblFloppyFSType.AutoSize = true;
            this.lblFloppyFSType.Location = new System.Drawing.Point(12, 75);
            this.lblFloppyFSType.Name = "lblFloppyFSType";
            this.lblFloppyFSType.Size = new System.Drawing.Size(94, 15);
            this.lblFloppyFSType.TabIndex = 57;
            this.lblFloppyFSType.Text = "File system type:";
            this.toolTip.SetToolTip(this.lblFloppyFSType, resources.GetString("lblFloppyFSType.ToolTip"));
            // 
            // txtFloppySerial
            // 
            this.txtFloppySerial.Location = new System.Drawing.Point(143, 44);
            this.txtFloppySerial.MaxLength = 8;
            this.txtFloppySerial.Name = "txtFloppySerial";
            this.txtFloppySerial.Size = new System.Drawing.Size(113, 23);
            this.txtFloppySerial.TabIndex = 2;
            this.toolTip.SetToolTip(this.txtFloppySerial, "This field can be used to detect when the disk was ejected\r\nand a different disk " +
        "was inserted.");
            // 
            // lblFloppySerial
            // 
            this.lblFloppySerial.AutoSize = true;
            this.lblFloppySerial.Location = new System.Drawing.Point(12, 47);
            this.lblFloppySerial.Name = "lblFloppySerial";
            this.lblFloppySerial.Size = new System.Drawing.Size(125, 15);
            this.lblFloppySerial.TabIndex = 56;
            this.lblFloppySerial.Text = "Volume serial number:\r\n";
            this.toolTip.SetToolTip(this.lblFloppySerial, "This field can be used to detect when the disk was ejected\r\nand a different disk " +
        "was inserted.\r\n");
            // 
            // lstFloppySides
            // 
            this.lstFloppySides.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstFloppySides.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstFloppySides.FormattingEnabled = true;
            this.lstFloppySides.Items.AddRange(new object[] {
            "1",
            "2"});
            this.lstFloppySides.Location = new System.Drawing.Point(411, 189);
            this.lstFloppySides.Name = "lstFloppySides";
            this.lstFloppySides.Size = new System.Drawing.Size(84, 23);
            this.lstFloppySides.TabIndex = 13;
            this.toolTip.SetToolTip(this.lstFloppySides, "Determines whether a disk is single-sided (SS) or double-sided (DS).");
            // 
            // lblFloppySides
            // 
            this.lblFloppySides.AutoSize = true;
            this.lblFloppySides.Location = new System.Drawing.Point(269, 190);
            this.lblFloppySides.Name = "lblFloppySides";
            this.lblFloppySides.Size = new System.Drawing.Size(37, 15);
            this.lblFloppySides.TabIndex = 55;
            this.lblFloppySides.Text = "Sides:";
            this.toolTip.SetToolTip(this.lblFloppySides, "Determines whether a disk is single-sided (SS) or double-sided (DS).");
            // 
            // txtFloppySPT
            // 
            this.txtFloppySPT.Location = new System.Drawing.Point(143, 188);
            this.txtFloppySPT.Maximum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.txtFloppySPT.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.txtFloppySPT.Name = "txtFloppySPT";
            this.txtFloppySPT.Size = new System.Drawing.Size(113, 23);
            this.txtFloppySPT.TabIndex = 12;
            this.txtFloppySPT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.txtFloppySPT, "The number of sectors in one track.");
            this.txtFloppySPT.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
            // 
            // lblFloppySPT
            // 
            this.lblFloppySPT.AutoSize = true;
            this.lblFloppySPT.Location = new System.Drawing.Point(12, 190);
            this.lblFloppySPT.Name = "lblFloppySPT";
            this.lblFloppySPT.Size = new System.Drawing.Size(97, 15);
            this.lblFloppySPT.TabIndex = 54;
            this.lblFloppySPT.Text = "Sectors per track:";
            this.toolTip.SetToolTip(this.lblFloppySPT, "The number of sectors in one track.");
            // 
            // txtFloppySPF
            // 
            this.txtFloppySPF.Location = new System.Drawing.Point(411, 14);
            this.txtFloppySPF.Maximum = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.txtFloppySPF.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtFloppySPF.Name = "txtFloppySPF";
            this.txtFloppySPF.Size = new System.Drawing.Size(84, 23);
            this.txtFloppySPF.TabIndex = 1;
            this.txtFloppySPF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.txtFloppySPF, "Defines the number of sectors occupied by each file\r\nallocation table (FAT).");
            this.txtFloppySPF.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // lblFloppySPF
            // 
            this.lblFloppySPF.AutoSize = true;
            this.lblFloppySPF.Location = new System.Drawing.Point(269, 17);
            this.lblFloppySPF.Name = "lblFloppySPF";
            this.lblFloppySPF.Size = new System.Drawing.Size(89, 15);
            this.lblFloppySPF.TabIndex = 53;
            this.lblFloppySPF.Text = "Sectors per FAT:";
            this.toolTip.SetToolTip(this.lblFloppySPF, "Defines the number of sectors occupied by each file\r\nallocation table (FAT).");
            // 
            // txtFloppyMediaDesc
            // 
            this.txtFloppyMediaDesc.Hexadecimal = true;
            this.txtFloppyMediaDesc.Location = new System.Drawing.Point(411, 160);
            this.txtFloppyMediaDesc.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.txtFloppyMediaDesc.Minimum = new decimal(new int[] {
            237,
            0,
            0,
            0});
            this.txtFloppyMediaDesc.Name = "txtFloppyMediaDesc";
            this.txtFloppyMediaDesc.Size = new System.Drawing.Size(84, 23);
            this.txtFloppyMediaDesc.TabIndex = 11;
            this.txtFloppyMediaDesc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.txtFloppyMediaDesc, resources.GetString("txtFloppyMediaDesc.ToolTip"));
            this.txtFloppyMediaDesc.Value = new decimal(new int[] {
            240,
            0,
            0,
            0});
            // 
            // lblFloppyMediaDesc
            // 
            this.lblFloppyMediaDesc.AutoSize = true;
            this.lblFloppyMediaDesc.Location = new System.Drawing.Point(269, 162);
            this.lblFloppyMediaDesc.Name = "lblFloppyMediaDesc";
            this.lblFloppyMediaDesc.Size = new System.Drawing.Size(99, 15);
            this.lblFloppyMediaDesc.TabIndex = 52;
            this.lblFloppyMediaDesc.Text = "Media descriptor:";
            this.toolTip.SetToolTip(this.lblFloppyMediaDesc, resources.GetString("lblFloppyMediaDesc.ToolTip"));
            // 
            // txtFloppyTotalSect
            // 
            this.txtFloppyTotalSect.Location = new System.Drawing.Point(411, 73);
            this.txtFloppyTotalSect.Maximum = new decimal(new int[] {
            7360,
            0,
            0,
            0});
            this.txtFloppyTotalSect.Minimum = new decimal(new int[] {
            320,
            0,
            0,
            0});
            this.txtFloppyTotalSect.Name = "txtFloppyTotalSect";
            this.txtFloppyTotalSect.Size = new System.Drawing.Size(84, 23);
            this.txtFloppyTotalSect.TabIndex = 5;
            this.txtFloppyTotalSect.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.txtFloppyTotalSect, "This field specifies the total number of sectors on a disk. The number\r\nis calcul" +
        "ated by multiplying the number of tracks, sectors per track and\r\nnumber of sides" +
        " (tracks * SPT * sides).\r\n");
            this.txtFloppyTotalSect.Value = new decimal(new int[] {
            2880,
            0,
            0,
            0});
            // 
            // lblFloppyTotalSect
            // 
            this.lblFloppyTotalSect.AutoSize = true;
            this.lblFloppyTotalSect.Location = new System.Drawing.Point(269, 75);
            this.lblFloppyTotalSect.Name = "lblFloppyTotalSect";
            this.lblFloppyTotalSect.Size = new System.Drawing.Size(134, 15);
            this.lblFloppyTotalSect.TabIndex = 49;
            this.lblFloppyTotalSect.Text = "Total number of sectors:";
            this.toolTip.SetToolTip(this.lblFloppyTotalSect, "This field specifies the total number of sectors on a disk. The number\r\nis calcul" +
        "ated by multiplying the number of tracks, sectors per track and\r\nnumber of sides" +
        " (tracks * SPT * sides).");
            // 
            // txtFloppyRootDir
            // 
            this.txtFloppyRootDir.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.txtFloppyRootDir.Location = new System.Drawing.Point(411, 102);
            this.txtFloppyRootDir.Maximum = new decimal(new int[] {
            320,
            0,
            0,
            0});
            this.txtFloppyRootDir.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.txtFloppyRootDir.Name = "txtFloppyRootDir";
            this.txtFloppyRootDir.Size = new System.Drawing.Size(84, 23);
            this.txtFloppyRootDir.TabIndex = 7;
            this.txtFloppyRootDir.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.txtFloppyRootDir, "This is the maximum number of files that can be located in the\r\nroot directory of" +
        " the disk. ");
            this.txtFloppyRootDir.Value = new decimal(new int[] {
            224,
            0,
            0,
            0});
            // 
            // lblFloppyRootDir
            // 
            this.lblFloppyRootDir.AutoSize = true;
            this.lblFloppyRootDir.Location = new System.Drawing.Point(269, 104);
            this.lblFloppyRootDir.Name = "lblFloppyRootDir";
            this.lblFloppyRootDir.Size = new System.Drawing.Size(123, 15);
            this.lblFloppyRootDir.TabIndex = 46;
            this.lblFloppyRootDir.Text = "Root directory entries:";
            this.toolTip.SetToolTip(this.lblFloppyRootDir, "This is the maximum number of files that can be located in the\r\nroot directory of" +
        " the disk.");
            // 
            // lblFloppyNumFATs
            // 
            this.lblFloppyNumFATs.AutoSize = true;
            this.lblFloppyNumFATs.Location = new System.Drawing.Point(12, 104);
            this.lblFloppyNumFATs.Name = "lblFloppyNumFATs";
            this.lblFloppyNumFATs.Size = new System.Drawing.Size(93, 15);
            this.lblFloppyNumFATs.TabIndex = 45;
            this.lblFloppyNumFATs.Text = "Number of FATs:";
            this.toolTip.SetToolTip(this.lblFloppyNumFATs, "The number of file allocation tables (FAT). On FAT12-formatted\r\nfloppy disks, thi" +
        "s value is basically always 2.");
            // 
            // lblFloppyReservedSect
            // 
            this.lblFloppyReservedSect.AutoSize = true;
            this.lblFloppyReservedSect.Location = new System.Drawing.Point(269, 47);
            this.lblFloppyReservedSect.Name = "lblFloppyReservedSect";
            this.lblFloppyReservedSect.Size = new System.Drawing.Size(97, 15);
            this.lblFloppyReservedSect.TabIndex = 43;
            this.lblFloppyReservedSect.Text = "Reserved sectors:";
            this.toolTip.SetToolTip(this.lblFloppyReservedSect, "The number of sectors before the first file allocation table (FAT).\r\nFor most FAT" +
        "12-formatted floppies, there is only one reserved\r\nsector - the boot sector.\r\n");
            // 
            // txtFloppySPC
            // 
            this.txtFloppySPC.Location = new System.Drawing.Point(143, 159);
            this.txtFloppySPC.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.txtFloppySPC.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtFloppySPC.Name = "txtFloppySPC";
            this.txtFloppySPC.Size = new System.Drawing.Size(113, 23);
            this.txtFloppySPC.TabIndex = 10;
            this.txtFloppySPC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.txtFloppySPC, "This field determines the number of sectors that make up one cluster.\r\n");
            this.txtFloppySPC.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblFloppySPC
            // 
            this.lblFloppySPC.AutoSize = true;
            this.lblFloppySPC.Location = new System.Drawing.Point(12, 162);
            this.lblFloppySPC.Name = "lblFloppySPC";
            this.lblFloppySPC.Size = new System.Drawing.Size(106, 15);
            this.lblFloppySPC.TabIndex = 39;
            this.lblFloppySPC.Text = "Sectors per cluster:";
            this.toolTip.SetToolTip(this.lblFloppySPC, "This field determines the number of sectors that make up one cluster.");
            // 
            // txtFloppyBPS
            // 
            this.txtFloppyBPS.Increment = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.txtFloppyBPS.Location = new System.Drawing.Point(143, 130);
            this.txtFloppyBPS.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.txtFloppyBPS.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.txtFloppyBPS.Name = "txtFloppyBPS";
            this.txtFloppyBPS.Size = new System.Drawing.Size(113, 23);
            this.txtFloppyBPS.TabIndex = 8;
            this.txtFloppyBPS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.txtFloppyBPS, "This field determines the size of a sector in bytes.");
            this.txtFloppyBPS.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // lblFloppyBPS
            // 
            this.lblFloppyBPS.AutoSize = true;
            this.lblFloppyBPS.Location = new System.Drawing.Point(12, 133);
            this.lblFloppyBPS.Name = "lblFloppyBPS";
            this.lblFloppyBPS.Size = new System.Drawing.Size(93, 15);
            this.lblFloppyBPS.TabIndex = 37;
            this.lblFloppyBPS.Text = "Bytes per sector:";
            this.toolTip.SetToolTip(this.lblFloppyBPS, "This field determines the size of a sector in bytes.");
            // 
            // txtFloppyOEMID
            // 
            this.txtFloppyOEMID.Location = new System.Drawing.Point(143, 14);
            this.txtFloppyOEMID.MaxLength = 8;
            this.txtFloppyOEMID.Name = "txtFloppyOEMID";
            this.txtFloppyOEMID.Size = new System.Drawing.Size(113, 23);
            this.txtFloppyOEMID.TabIndex = 0;
            this.txtFloppyOEMID.Text = "MSDOS5.0";
            this.toolTip.SetToolTip(this.txtFloppyOEMID, "The OEM ID field is often used to identify the system that formatted\r\nthe disk. U" +
        "sing non-standard values could result in the operating\r\nsystem or program not re" +
        "cognizing the disk.");
            // 
            // lblFloppyOEMID
            // 
            this.lblFloppyOEMID.AutoSize = true;
            this.lblFloppyOEMID.Location = new System.Drawing.Point(12, 17);
            this.lblFloppyOEMID.Name = "lblFloppyOEMID";
            this.lblFloppyOEMID.Size = new System.Drawing.Size(50, 15);
            this.lblFloppyOEMID.TabIndex = 34;
            this.lblFloppyOEMID.Text = "OEM ID:";
            this.toolTip.SetToolTip(this.lblFloppyOEMID, "The OEM ID field is often used to identify the system that formatted\r\nthe disk. U" +
        "sing non-standard values could result in the operating\r\nsystem or program not re" +
        "cognizing the disk.\r\n");
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 226);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(514, 50);
            this.pnlBottom.TabIndex = 18;
            // 
            // dlgNewImageAdvanced
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(514, 276);
            this.Controls.Add(this.txtFloppyTracks);
            this.Controls.Add(this.lblFloppyTracks);
            this.Controls.Add(this.txtFloppyReservedSect);
            this.Controls.Add(this.txtFloppyNumFATs);
            this.Controls.Add(this.txtFloppyFSType);
            this.Controls.Add(this.lblFloppyFSType);
            this.Controls.Add(this.txtFloppySerial);
            this.Controls.Add(this.lblFloppySerial);
            this.Controls.Add(this.lstFloppySides);
            this.Controls.Add(this.lblFloppySides);
            this.Controls.Add(this.txtFloppySPT);
            this.Controls.Add(this.lblFloppySPT);
            this.Controls.Add(this.txtFloppySPF);
            this.Controls.Add(this.lblFloppySPF);
            this.Controls.Add(this.txtFloppyMediaDesc);
            this.Controls.Add(this.lblFloppyMediaDesc);
            this.Controls.Add(this.txtFloppyTotalSect);
            this.Controls.Add(this.lblFloppyTotalSect);
            this.Controls.Add(this.txtFloppyRootDir);
            this.Controls.Add(this.lblFloppyRootDir);
            this.Controls.Add(this.lblFloppyNumFATs);
            this.Controls.Add(this.lblFloppyReservedSect);
            this.Controls.Add(this.txtFloppySPC);
            this.Controls.Add(this.lblFloppySPC);
            this.Controls.Add(this.txtFloppyBPS);
            this.Controls.Add(this.lblFloppyBPS);
            this.Controls.Add(this.txtFloppyOEMID);
            this.Controls.Add(this.lblFloppyOEMID);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgNewImageAdvanced";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced options";
            this.Load += new System.EventHandler(this.dlgNewImageAdvanced_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyTracks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyReservedSect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyNumFATs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppySPT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppySPF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyMediaDesc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyTotalSect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyRootDir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppySPC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyBPS)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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