namespace TotalImage
{
    partial class dlgNewImage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgNewImage));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblFloppyType = new System.Windows.Forms.Label();
            this.lstFloppyCapacity = new System.Windows.Forms.ComboBox();
            this.cbxFloppyBPB = new System.Windows.Forms.CheckBox();
            this.lstFloppyBPB = new System.Windows.Forms.ComboBox();
            this.lblFloppyOEMID = new System.Windows.Forms.Label();
            this.txtFloppyOEMID = new System.Windows.Forms.TextBox();
            this.lblFloppyBPS = new System.Windows.Forms.Label();
            this.txtFloppyBPS = new System.Windows.Forms.NumericUpDown();
            this.lblFloppySPC = new System.Windows.Forms.Label();
            this.txtFloppySPC = new System.Windows.Forms.NumericUpDown();
            this.lblFloppyReserved = new System.Windows.Forms.Label();
            this.lblFloppyNumFATs = new System.Windows.Forms.Label();
            this.lblFloppyRootDirEntries = new System.Windows.Forms.Label();
            this.txtFloppyRootDirEntries = new System.Windows.Forms.NumericUpDown();
            this.lblFloppyTotalSect = new System.Windows.Forms.Label();
            this.txtFloppyTotalSect = new System.Windows.Forms.NumericUpDown();
            this.lblFloppyMediaDesc = new System.Windows.Forms.Label();
            this.txtFloppyMediaDesc = new System.Windows.Forms.NumericUpDown();
            this.lblFloppySPF = new System.Windows.Forms.Label();
            this.txtFloppySPF = new System.Windows.Forms.NumericUpDown();
            this.lblFloppySPT = new System.Windows.Forms.Label();
            this.txtFloppySPT = new System.Windows.Forms.NumericUpDown();
            this.lblFloppySides = new System.Windows.Forms.Label();
            this.lstFloppySides = new System.Windows.Forms.ComboBox();
            this.lblFloppySerial = new System.Windows.Forms.Label();
            this.txtFloppySerial = new System.Windows.Forms.TextBox();
            this.lblFloppyLabel = new System.Windows.Forms.Label();
            this.txtFloppyLabel = new System.Windows.Forms.TextBox();
            this.lblFloppyFSType = new System.Windows.Forms.Label();
            this.txtFloppyFSType = new System.Windows.Forms.TextBox();
            this.txtFloppyNumFATs = new System.Windows.Forms.NumericUpDown();
            this.txtFloppyReserved = new System.Windows.Forms.NumericUpDown();
            this.lblFloppyTracks = new System.Windows.Forms.Label();
            this.txtFloppyTracks = new System.Windows.Forms.NumericUpDown();
            this.lstType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.NumericUpDown();
            this.lblSize = new System.Windows.Forms.Label();
            this.txtSectors = new System.Windows.Forms.NumericUpDown();
            this.lblSectors = new System.Windows.Forms.Label();
            this.txtHeads = new System.Windows.Forms.NumericUpDown();
            this.lblHeads = new System.Windows.Forms.Label();
            this.txtCylinders = new System.Windows.Forms.NumericUpDown();
            this.lblCylinders = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabFloppy = new System.Windows.Forms.TabPage();
            this.tabHDD = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyBPS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppySPC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyRootDirEntries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyTotalSect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyMediaDesc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppySPF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppySPT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyNumFATs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyReserved)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyTracks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCylinders)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabFloppy.SuspendLayout();
            this.tabHDD.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(637, 248);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(551, 248);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
            this.btnOK.TabIndex = 16;
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
            // lblFloppyType
            // 
            this.lblFloppyType.AutoSize = true;
            this.lblFloppyType.Location = new System.Drawing.Point(16, 17);
            this.lblFloppyType.Name = "lblFloppyType";
            this.lblFloppyType.Size = new System.Drawing.Size(112, 15);
            this.lblFloppyType.TabIndex = 0;
            this.lblFloppyType.Text = "Formatted capacity:";
            this.toolTip.SetToolTip(this.lblFloppyType, "This will be the total storage capacity of your floppy disk image\r\nin kibibytes (" +
        "1 kibibyte =1024 bytes).\r\n");
            // 
            // lstFloppyCapacity
            // 
            this.lstFloppyCapacity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstFloppyCapacity.FormattingEnabled = true;
            this.lstFloppyCapacity.Items.AddRange(new object[] {
            "160 KiB",
            "180 KiB",
            "320 KiB (5.25\")",
            "320 KiB (3.5\")",
            "360 KiB (5.25\")",
            "360 KiB (3.5\")",
            "400 KiB (DEC RX50)",
            "640 KiB",
            "720 KiB (Tandy 2000)",
            "720 KiB",
            "800 KiB",
            "1200 KiB",
            "1232 KiB (NEC PC-98)",
            "1440 KiB",
            "1520 KiB (IBM XDF)",
            "1680 KiB (Microsoft DMF 1024 BPC)",
            "1680 KiB (Microsoft DMF 2048 BPC)",
            "1722 KiB",
            "1840 KiB (IBM XDF)",
            "2880 KiB",
            "3680 KiB (IBM XDF)",
            "Custom"});
            this.lstFloppyCapacity.Location = new System.Drawing.Point(147, 13);
            this.lstFloppyCapacity.Name = "lstFloppyCapacity";
            this.lstFloppyCapacity.Size = new System.Drawing.Size(225, 23);
            this.lstFloppyCapacity.TabIndex = 1;
            this.toolTip.SetToolTip(this.lstFloppyCapacity, "This will be the total storage capacity of your floppy disk image\r\nin kibibytes (" +
        "1 kibibyte =1024 bytes).");
            this.lstFloppyCapacity.SelectedIndexChanged += new System.EventHandler(this.lstFloppyType_SelectedIndexChanged);
            // 
            // cbxFloppyBPB
            // 
            this.cbxFloppyBPB.AutoSize = true;
            this.cbxFloppyBPB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxFloppyBPB.Location = new System.Drawing.Point(387, 15);
            this.cbxFloppyBPB.Name = "cbxFloppyBPB";
            this.cbxFloppyBPB.Size = new System.Drawing.Size(219, 20);
            this.cbxFloppyBPB.TabIndex = 2;
            this.cbxFloppyBPB.Text = "Write a DOS BPB to the boot sector:";
            this.toolTip.SetToolTip(this.cbxFloppyBPB, "DOS 1.0 and 1.1 did not use a BPB, but most programs and operating\r\nsystems made " +
        "since then will expect one. Newer versions added\r\nadditional fields to better de" +
        "scribe the physical media.\r\n");
            this.cbxFloppyBPB.UseVisualStyleBackColor = true;
            this.cbxFloppyBPB.CheckedChanged += new System.EventHandler(this.cbxFloppyBPB_CheckedChanged);
            // 
            // lstFloppyBPB
            // 
            this.lstFloppyBPB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstFloppyBPB.FormattingEnabled = true;
            this.lstFloppyBPB.Items.AddRange(new object[] {
            "DOS 2.0",
            "DOS 3.0",
            "DOS 3.2",
            "DOS 3.31",
            "DOS 3.4",
            "DOS 4.0+"});
            this.lstFloppyBPB.Location = new System.Drawing.Point(612, 13);
            this.lstFloppyBPB.Name = "lstFloppyBPB";
            this.lstFloppyBPB.Size = new System.Drawing.Size(72, 23);
            this.lstFloppyBPB.TabIndex = 3;
            this.toolTip.SetToolTip(this.lstFloppyBPB, "DOS 1.0 and 1.1 did not use a BPB, but most programs and operating\r\nsystems made " +
        "since then will expect one. Newer versions added\r\nadditional fields to better de" +
        "scribe the physical media.\r\n");
            this.lstFloppyBPB.SelectedIndexChanged += new System.EventHandler(this.lstFloppyBPB_SelectedIndexChanged);
            // 
            // lblFloppyOEMID
            // 
            this.lblFloppyOEMID.AutoSize = true;
            this.lblFloppyOEMID.Location = new System.Drawing.Point(16, 46);
            this.lblFloppyOEMID.Name = "lblFloppyOEMID";
            this.lblFloppyOEMID.Size = new System.Drawing.Size(50, 15);
            this.lblFloppyOEMID.TabIndex = 4;
            this.lblFloppyOEMID.Text = "OEM ID:";
            this.toolTip.SetToolTip(this.lblFloppyOEMID, "The OEM ID field is often used to identify the system that formatted\r\nthe disk. U" +
        "sing non-standard values could result in the operating\r\nsystem or program not re" +
        "cognizing the disk.\r\n");
            // 
            // txtFloppyOEMID
            // 
            this.txtFloppyOEMID.Location = new System.Drawing.Point(147, 42);
            this.txtFloppyOEMID.MaxLength = 8;
            this.txtFloppyOEMID.Name = "txtFloppyOEMID";
            this.txtFloppyOEMID.Size = new System.Drawing.Size(113, 23);
            this.txtFloppyOEMID.TabIndex = 4;
            this.txtFloppyOEMID.Text = "MSDOS5.0";
            this.toolTip.SetToolTip(this.txtFloppyOEMID, "The OEM ID field is often used to identify the system that formatted\r\nthe disk. U" +
        "sing non-standard values could result in the operating\r\nsystem or program not re" +
        "cognizing the disk.");
            // 
            // lblFloppyBPS
            // 
            this.lblFloppyBPS.AutoSize = true;
            this.lblFloppyBPS.Location = new System.Drawing.Point(275, 46);
            this.lblFloppyBPS.Name = "lblFloppyBPS";
            this.lblFloppyBPS.Size = new System.Drawing.Size(93, 15);
            this.lblFloppyBPS.TabIndex = 6;
            this.lblFloppyBPS.Text = "Bytes per sector:";
            this.toolTip.SetToolTip(this.lblFloppyBPS, "This field determines the size of a sector in bytes.");
            // 
            // txtFloppyBPS
            // 
            this.txtFloppyBPS.Increment = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.txtFloppyBPS.Location = new System.Drawing.Point(387, 42);
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
            this.txtFloppyBPS.Size = new System.Drawing.Size(70, 23);
            this.txtFloppyBPS.TabIndex = 5;
            this.toolTip.SetToolTip(this.txtFloppyBPS, "This field determines the size of a sector in bytes.");
            this.txtFloppyBPS.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // lblFloppySPC
            // 
            this.lblFloppySPC.AutoSize = true;
            this.lblFloppySPC.Location = new System.Drawing.Point(275, 75);
            this.lblFloppySPC.Name = "lblFloppySPC";
            this.lblFloppySPC.Size = new System.Drawing.Size(106, 15);
            this.lblFloppySPC.TabIndex = 8;
            this.lblFloppySPC.Text = "Sectors per cluster:";
            this.toolTip.SetToolTip(this.lblFloppySPC, "This field determines the number of sectors that make up one cluster.");
            // 
            // txtFloppySPC
            // 
            this.txtFloppySPC.Location = new System.Drawing.Point(387, 71);
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
            this.txtFloppySPC.Size = new System.Drawing.Size(70, 23);
            this.txtFloppySPC.TabIndex = 8;
            this.toolTip.SetToolTip(this.txtFloppySPC, "This field determines the number of sectors that make up one cluster.\r\n");
            this.txtFloppySPC.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblFloppyReserved
            // 
            this.lblFloppyReserved.AutoSize = true;
            this.lblFloppyReserved.Location = new System.Drawing.Point(275, 163);
            this.lblFloppyReserved.Name = "lblFloppyReserved";
            this.lblFloppyReserved.Size = new System.Drawing.Size(97, 15);
            this.lblFloppyReserved.TabIndex = 10;
            this.lblFloppyReserved.Text = "Reserved sectors:";
            this.toolTip.SetToolTip(this.lblFloppyReserved, "The number of sectors before the first file allocation table (FAT).\r\nThe only pos" +
        "sible value for FAT12-formatted floppy disks is 1.");
            // 
            // lblFloppyNumFATs
            // 
            this.lblFloppyNumFATs.AutoSize = true;
            this.lblFloppyNumFATs.Location = new System.Drawing.Point(16, 163);
            this.lblFloppyNumFATs.Name = "lblFloppyNumFATs";
            this.lblFloppyNumFATs.Size = new System.Drawing.Size(93, 15);
            this.lblFloppyNumFATs.TabIndex = 11;
            this.lblFloppyNumFATs.Text = "Number of FATs:";
            this.toolTip.SetToolTip(this.lblFloppyNumFATs, "The number of file allocation tables (FAT). On FAT12-formatted\r\nfloppy disks, thi" +
        "s value is always 2.");
            // 
            // lblFloppyRootDirEntries
            // 
            this.lblFloppyRootDirEntries.AutoSize = true;
            this.lblFloppyRootDirEntries.Location = new System.Drawing.Point(472, 74);
            this.lblFloppyRootDirEntries.Name = "lblFloppyRootDirEntries";
            this.lblFloppyRootDirEntries.Size = new System.Drawing.Size(123, 15);
            this.lblFloppyRootDirEntries.TabIndex = 12;
            this.lblFloppyRootDirEntries.Text = "Root directory entries:";
            this.toolTip.SetToolTip(this.lblFloppyRootDirEntries, "This is the maximum number of files that can be located in the\r\nroot directory of" +
        " the disk.");
            // 
            // txtFloppyRootDirEntries
            // 
            this.txtFloppyRootDirEntries.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.txtFloppyRootDirEntries.Location = new System.Drawing.Point(612, 71);
            this.txtFloppyRootDirEntries.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.txtFloppyRootDirEntries.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.txtFloppyRootDirEntries.Name = "txtFloppyRootDirEntries";
            this.txtFloppyRootDirEntries.Size = new System.Drawing.Size(72, 23);
            this.txtFloppyRootDirEntries.TabIndex = 9;
            this.toolTip.SetToolTip(this.txtFloppyRootDirEntries, "This is the maximum number of files that can be located in the\r\nroot directory of" +
        " the disk. ");
            this.txtFloppyRootDirEntries.Value = new decimal(new int[] {
            224,
            0,
            0,
            0});
            // 
            // lblFloppyTotalSect
            // 
            this.lblFloppyTotalSect.AutoSize = true;
            this.lblFloppyTotalSect.Location = new System.Drawing.Point(472, 46);
            this.lblFloppyTotalSect.Name = "lblFloppyTotalSect";
            this.lblFloppyTotalSect.Size = new System.Drawing.Size(134, 15);
            this.lblFloppyTotalSect.TabIndex = 14;
            this.lblFloppyTotalSect.Text = "Total number of sectors:";
            this.toolTip.SetToolTip(this.lblFloppyTotalSect, "This field specifies the total number of sectors on a disk. The number\r\nis calcul" +
        "ated by multiplying the number of tracks, sectors per track and\r\nnumber of sides" +
        " (tracks * SPT * sides).");
            // 
            // txtFloppyTotalSect
            // 
            this.txtFloppyTotalSect.Location = new System.Drawing.Point(612, 42);
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
            this.txtFloppyTotalSect.Size = new System.Drawing.Size(72, 23);
            this.txtFloppyTotalSect.TabIndex = 6;
            this.toolTip.SetToolTip(this.txtFloppyTotalSect, "This field specifies the total number of sectors on a disk. The number\r\nis calcul" +
        "ated by multiplying the number of tracks, sectors per track and\r\nnumber of sides" +
        " (tracks * SPT * sides).\r\n");
            this.txtFloppyTotalSect.Value = new decimal(new int[] {
            2880,
            0,
            0,
            0});
            // 
            // lblFloppyMediaDesc
            // 
            this.lblFloppyMediaDesc.AutoSize = true;
            this.lblFloppyMediaDesc.Location = new System.Drawing.Point(472, 133);
            this.lblFloppyMediaDesc.Name = "lblFloppyMediaDesc";
            this.lblFloppyMediaDesc.Size = new System.Drawing.Size(99, 15);
            this.lblFloppyMediaDesc.TabIndex = 16;
            this.lblFloppyMediaDesc.Text = "Media descriptor:";
            this.toolTip.SetToolTip(this.lblFloppyMediaDesc, resources.GetString("lblFloppyMediaDesc.ToolTip"));
            // 
            // txtFloppyMediaDesc
            // 
            this.txtFloppyMediaDesc.Hexadecimal = true;
            this.txtFloppyMediaDesc.Location = new System.Drawing.Point(612, 129);
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
            this.txtFloppyMediaDesc.Size = new System.Drawing.Size(72, 23);
            this.txtFloppyMediaDesc.TabIndex = 12;
            this.toolTip.SetToolTip(this.txtFloppyMediaDesc, resources.GetString("txtFloppyMediaDesc.ToolTip"));
            this.txtFloppyMediaDesc.Value = new decimal(new int[] {
            240,
            0,
            0,
            0});
            // 
            // lblFloppySPF
            // 
            this.lblFloppySPF.AutoSize = true;
            this.lblFloppySPF.Location = new System.Drawing.Point(275, 133);
            this.lblFloppySPF.Name = "lblFloppySPF";
            this.lblFloppySPF.Size = new System.Drawing.Size(89, 15);
            this.lblFloppySPF.TabIndex = 18;
            this.lblFloppySPF.Text = "Sectors per FAT:";
            this.toolTip.SetToolTip(this.lblFloppySPF, "Defines the number of sectors occupied by each file\r\nallocation table (FAT).");
            // 
            // txtFloppySPF
            // 
            this.txtFloppySPF.Location = new System.Drawing.Point(387, 129);
            this.txtFloppySPF.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtFloppySPF.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtFloppySPF.Name = "txtFloppySPF";
            this.txtFloppySPF.Size = new System.Drawing.Size(70, 23);
            this.txtFloppySPF.TabIndex = 14;
            this.toolTip.SetToolTip(this.txtFloppySPF, "Defines the number of sectors occupied by each file\r\nallocation table (FAT).");
            this.txtFloppySPF.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // lblFloppySPT
            // 
            this.lblFloppySPT.AutoSize = true;
            this.lblFloppySPT.Location = new System.Drawing.Point(275, 104);
            this.lblFloppySPT.Name = "lblFloppySPT";
            this.lblFloppySPT.Size = new System.Drawing.Size(97, 15);
            this.lblFloppySPT.TabIndex = 20;
            this.lblFloppySPT.Text = "Sectors per track:";
            this.toolTip.SetToolTip(this.lblFloppySPT, "The number of sectors in one track.");
            // 
            // txtFloppySPT
            // 
            this.txtFloppySPT.Location = new System.Drawing.Point(387, 100);
            this.txtFloppySPT.Maximum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.txtFloppySPT.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.txtFloppySPT.Name = "txtFloppySPT";
            this.txtFloppySPT.Size = new System.Drawing.Size(70, 23);
            this.txtFloppySPT.TabIndex = 11;
            this.toolTip.SetToolTip(this.txtFloppySPT, "The number of sectors in one track.");
            this.txtFloppySPT.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
            // 
            // lblFloppySides
            // 
            this.lblFloppySides.AutoSize = true;
            this.lblFloppySides.Location = new System.Drawing.Point(472, 163);
            this.lblFloppySides.Name = "lblFloppySides";
            this.lblFloppySides.Size = new System.Drawing.Size(37, 15);
            this.lblFloppySides.TabIndex = 22;
            this.lblFloppySides.Text = "Sides:";
            this.toolTip.SetToolTip(this.lblFloppySides, "Determines whether a disk is single-sided (SS) or double-sided (DS).");
            // 
            // lstFloppySides
            // 
            this.lstFloppySides.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstFloppySides.FormattingEnabled = true;
            this.lstFloppySides.Items.AddRange(new object[] {
            "1",
            "2"});
            this.lstFloppySides.Location = new System.Drawing.Point(612, 158);
            this.lstFloppySides.Name = "lstFloppySides";
            this.lstFloppySides.Size = new System.Drawing.Size(72, 23);
            this.lstFloppySides.TabIndex = 15;
            this.toolTip.SetToolTip(this.lstFloppySides, "Determines whether a disk is single-sided (SS) or double-sided (DS).");
            // 
            // lblFloppySerial
            // 
            this.lblFloppySerial.AutoSize = true;
            this.lblFloppySerial.Location = new System.Drawing.Point(16, 104);
            this.lblFloppySerial.Name = "lblFloppySerial";
            this.lblFloppySerial.Size = new System.Drawing.Size(125, 15);
            this.lblFloppySerial.TabIndex = 24;
            this.lblFloppySerial.Text = "Volume serial number:\r\n";
            this.toolTip.SetToolTip(this.lblFloppySerial, "This field can be used to detect when the disk was ejected\r\nand a different disk " +
        "was inserted.\r\n");
            // 
            // txtFloppySerial
            // 
            this.txtFloppySerial.Location = new System.Drawing.Point(147, 100);
            this.txtFloppySerial.MaxLength = 8;
            this.txtFloppySerial.Name = "txtFloppySerial";
            this.txtFloppySerial.Size = new System.Drawing.Size(113, 23);
            this.txtFloppySerial.TabIndex = 10;
            this.toolTip.SetToolTip(this.txtFloppySerial, "This field can be used to detect when the disk was ejected\r\nand a different disk " +
        "was inserted.");
            // 
            // lblFloppyLabel
            // 
            this.lblFloppyLabel.AutoSize = true;
            this.lblFloppyLabel.Location = new System.Drawing.Point(16, 75);
            this.lblFloppyLabel.Name = "lblFloppyLabel";
            this.lblFloppyLabel.Size = new System.Drawing.Size(78, 15);
            this.lblFloppyLabel.TabIndex = 26;
            this.lblFloppyLabel.Text = "Volume label:\r\n";
            this.toolTip.SetToolTip(this.lblFloppyLabel, "Volume label can be used to describe the contents or purpose of\r\nthe disk.");
            // 
            // txtFloppyLabel
            // 
            this.txtFloppyLabel.Location = new System.Drawing.Point(147, 71);
            this.txtFloppyLabel.MaxLength = 11;
            this.txtFloppyLabel.Name = "txtFloppyLabel";
            this.txtFloppyLabel.Size = new System.Drawing.Size(113, 23);
            this.txtFloppyLabel.TabIndex = 7;
            this.toolTip.SetToolTip(this.txtFloppyLabel, "Volume label can be used to describe the contents or purpose of\r\nthe disk. Some o" +
        "perating systems or programs may not recognize\r\nthe disk if this value is not wh" +
        "at they expect.");
            // 
            // lblFloppyFSType
            // 
            this.lblFloppyFSType.AutoSize = true;
            this.lblFloppyFSType.Location = new System.Drawing.Point(16, 133);
            this.lblFloppyFSType.Name = "lblFloppyFSType";
            this.lblFloppyFSType.Size = new System.Drawing.Size(94, 15);
            this.lblFloppyFSType.TabIndex = 28;
            this.lblFloppyFSType.Text = "File system type:";
            this.toolTip.SetToolTip(this.lblFloppyFSType, resources.GetString("lblFloppyFSType.ToolTip"));
            // 
            // txtFloppyFSType
            // 
            this.txtFloppyFSType.Location = new System.Drawing.Point(147, 129);
            this.txtFloppyFSType.MaxLength = 8;
            this.txtFloppyFSType.Name = "txtFloppyFSType";
            this.txtFloppyFSType.Size = new System.Drawing.Size(113, 23);
            this.txtFloppyFSType.TabIndex = 13;
            this.txtFloppyFSType.Text = "FAT12";
            this.toolTip.SetToolTip(this.txtFloppyFSType, resources.GetString("txtFloppyFSType.ToolTip"));
            // 
            // txtFloppyNumFATs
            // 
            this.txtFloppyNumFATs.Location = new System.Drawing.Point(147, 158);
            this.txtFloppyNumFATs.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.txtFloppyNumFATs.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.txtFloppyNumFATs.Name = "txtFloppyNumFATs";
            this.txtFloppyNumFATs.ReadOnly = true;
            this.txtFloppyNumFATs.Size = new System.Drawing.Size(113, 23);
            this.txtFloppyNumFATs.TabIndex = 30;
            this.toolTip.SetToolTip(this.txtFloppyNumFATs, "The number of file allocation tables (FAT). On FAT12-formatted\r\nfloppy disks, thi" +
        "s value is always 2.\r\n");
            this.txtFloppyNumFATs.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // txtFloppyReserved
            // 
            this.txtFloppyReserved.Location = new System.Drawing.Point(387, 158);
            this.txtFloppyReserved.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtFloppyReserved.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtFloppyReserved.Name = "txtFloppyReserved";
            this.txtFloppyReserved.ReadOnly = true;
            this.txtFloppyReserved.Size = new System.Drawing.Size(70, 23);
            this.txtFloppyReserved.TabIndex = 31;
            this.toolTip.SetToolTip(this.txtFloppyReserved, "The number of sectors before the first file allocation table (FAT).\r\nThe only pos" +
        "sible value for FAT12-formatted floppy disks is 1.\r\n");
            this.txtFloppyReserved.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblFloppyTracks
            // 
            this.lblFloppyTracks.AutoSize = true;
            this.lblFloppyTracks.Location = new System.Drawing.Point(472, 104);
            this.lblFloppyTracks.Name = "lblFloppyTracks";
            this.lblFloppyTracks.Size = new System.Drawing.Size(86, 15);
            this.lblFloppyTracks.TabIndex = 32;
            this.lblFloppyTracks.Text = "Tracks per side:";
            this.toolTip.SetToolTip(this.lblFloppyTracks, "The number of tracks on one side of the disk.");
            // 
            // txtFloppyTracks
            // 
            this.txtFloppyTracks.Location = new System.Drawing.Point(612, 100);
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
            this.txtFloppyTracks.Size = new System.Drawing.Size(72, 23);
            this.txtFloppyTracks.TabIndex = 33;
            this.toolTip.SetToolTip(this.txtFloppyTracks, "The number of tracks on one side of the disk.");
            this.txtFloppyTracks.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // lstType
            // 
            this.lstType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstType.FormattingEnabled = true;
            this.lstType.Location = new System.Drawing.Point(219, 48);
            this.lstType.MaxDropDownItems = 99;
            this.lstType.Name = "lstType";
            this.lstType.Size = new System.Drawing.Size(223, 23);
            this.lstType.TabIndex = 9;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(170, 53);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(34, 15);
            this.lblType.TabIndex = 8;
            this.lblType.Text = "Type:";
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(79, 48);
            this.txtSize.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(75, 23);
            this.txtSize.TabIndex = 7;
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(14, 53);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(59, 15);
            this.lblSize.TabIndex = 6;
            this.lblSize.Text = "Size (MB):";
            // 
            // txtSectors
            // 
            this.txtSectors.Location = new System.Drawing.Point(367, 12);
            this.txtSectors.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.txtSectors.Name = "txtSectors";
            this.txtSectors.Size = new System.Drawing.Size(75, 23);
            this.txtSectors.TabIndex = 5;
            // 
            // lblSectors
            // 
            this.lblSectors.AutoSize = true;
            this.lblSectors.Location = new System.Drawing.Point(313, 16);
            this.lblSectors.Name = "lblSectors";
            this.lblSectors.Size = new System.Drawing.Size(48, 15);
            this.lblSectors.TabIndex = 4;
            this.lblSectors.Text = "Sectors:";
            // 
            // txtHeads
            // 
            this.txtHeads.Location = new System.Drawing.Point(219, 12);
            this.txtHeads.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.txtHeads.Name = "txtHeads";
            this.txtHeads.Size = new System.Drawing.Size(75, 23);
            this.txtHeads.TabIndex = 3;
            // 
            // lblHeads
            // 
            this.lblHeads.AutoSize = true;
            this.lblHeads.Location = new System.Drawing.Point(170, 16);
            this.lblHeads.Name = "lblHeads";
            this.lblHeads.Size = new System.Drawing.Size(43, 15);
            this.lblHeads.TabIndex = 2;
            this.lblHeads.Text = "Heads:";
            // 
            // txtCylinders
            // 
            this.txtCylinders.Location = new System.Drawing.Point(79, 12);
            this.txtCylinders.Maximum = new decimal(new int[] {
            266305,
            0,
            0,
            0});
            this.txtCylinders.Name = "txtCylinders";
            this.txtCylinders.Size = new System.Drawing.Size(75, 23);
            this.txtCylinders.TabIndex = 1;
            // 
            // lblCylinders
            // 
            this.lblCylinders.AutoSize = true;
            this.lblCylinders.Location = new System.Drawing.Point(14, 16);
            this.lblCylinders.Name = "lblCylinders";
            this.lblCylinders.Size = new System.Drawing.Size(59, 15);
            this.lblCylinders.TabIndex = 0;
            this.lblCylinders.Text = "Cylinders:";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabFloppy);
            this.tabControl.Controls.Add(this.tabHDD);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(706, 225);
            this.tabControl.TabIndex = 0;
            // 
            // tabFloppy
            // 
            this.tabFloppy.Controls.Add(this.txtFloppyTracks);
            this.tabFloppy.Controls.Add(this.lblFloppyTracks);
            this.tabFloppy.Controls.Add(this.txtFloppyReserved);
            this.tabFloppy.Controls.Add(this.txtFloppyNumFATs);
            this.tabFloppy.Controls.Add(this.txtFloppyFSType);
            this.tabFloppy.Controls.Add(this.lblFloppyFSType);
            this.tabFloppy.Controls.Add(this.txtFloppyLabel);
            this.tabFloppy.Controls.Add(this.lblFloppyLabel);
            this.tabFloppy.Controls.Add(this.txtFloppySerial);
            this.tabFloppy.Controls.Add(this.lblFloppySerial);
            this.tabFloppy.Controls.Add(this.lstFloppySides);
            this.tabFloppy.Controls.Add(this.lblFloppySides);
            this.tabFloppy.Controls.Add(this.txtFloppySPT);
            this.tabFloppy.Controls.Add(this.lblFloppySPT);
            this.tabFloppy.Controls.Add(this.txtFloppySPF);
            this.tabFloppy.Controls.Add(this.lblFloppySPF);
            this.tabFloppy.Controls.Add(this.txtFloppyMediaDesc);
            this.tabFloppy.Controls.Add(this.lblFloppyMediaDesc);
            this.tabFloppy.Controls.Add(this.txtFloppyTotalSect);
            this.tabFloppy.Controls.Add(this.lblFloppyTotalSect);
            this.tabFloppy.Controls.Add(this.txtFloppyRootDirEntries);
            this.tabFloppy.Controls.Add(this.lblFloppyRootDirEntries);
            this.tabFloppy.Controls.Add(this.lblFloppyNumFATs);
            this.tabFloppy.Controls.Add(this.lblFloppyReserved);
            this.tabFloppy.Controls.Add(this.txtFloppySPC);
            this.tabFloppy.Controls.Add(this.lblFloppySPC);
            this.tabFloppy.Controls.Add(this.txtFloppyBPS);
            this.tabFloppy.Controls.Add(this.lblFloppyBPS);
            this.tabFloppy.Controls.Add(this.txtFloppyOEMID);
            this.tabFloppy.Controls.Add(this.lblFloppyOEMID);
            this.tabFloppy.Controls.Add(this.lstFloppyBPB);
            this.tabFloppy.Controls.Add(this.cbxFloppyBPB);
            this.tabFloppy.Controls.Add(this.lstFloppyCapacity);
            this.tabFloppy.Controls.Add(this.lblFloppyType);
            this.tabFloppy.Location = new System.Drawing.Point(4, 24);
            this.tabFloppy.Name = "tabFloppy";
            this.tabFloppy.Padding = new System.Windows.Forms.Padding(3);
            this.tabFloppy.Size = new System.Drawing.Size(698, 197);
            this.tabFloppy.TabIndex = 0;
            this.tabFloppy.Text = "Floppy disk";
            this.tabFloppy.UseVisualStyleBackColor = true;
            // 
            // tabHDD
            // 
            this.tabHDD.Controls.Add(this.lstType);
            this.tabHDD.Controls.Add(this.lblCylinders);
            this.tabHDD.Controls.Add(this.lblType);
            this.tabHDD.Controls.Add(this.txtCylinders);
            this.tabHDD.Controls.Add(this.txtSize);
            this.tabHDD.Controls.Add(this.lblHeads);
            this.tabHDD.Controls.Add(this.lblSize);
            this.tabHDD.Controls.Add(this.txtHeads);
            this.tabHDD.Controls.Add(this.txtSectors);
            this.tabHDD.Controls.Add(this.lblSectors);
            this.tabHDD.Location = new System.Drawing.Point(4, 24);
            this.tabHDD.Name = "tabHDD";
            this.tabHDD.Padding = new System.Windows.Forms.Padding(3);
            this.tabHDD.Size = new System.Drawing.Size(698, 197);
            this.tabHDD.TabIndex = 1;
            this.tabHDD.Text = "Hard disk";
            this.tabHDD.UseVisualStyleBackColor = true;
            // 
            // dlgNewImage
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(729, 286);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgNewImage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New image";
            this.Load += new System.EventHandler(this.dlgNewSectorImage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyBPS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppySPC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyRootDirEntries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyTotalSect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyMediaDesc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppySPF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppySPT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyNumFATs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyReserved)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloppyTracks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCylinders)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabFloppy.ResumeLayout(false);
            this.tabFloppy.PerformLayout();
            this.tabHDD.ResumeLayout(false);
            this.tabHDD.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label lblCylinders;
        private System.Windows.Forms.NumericUpDown txtCylinders;
        private System.Windows.Forms.NumericUpDown txtHeads;
        private System.Windows.Forms.Label lblHeads;
        private System.Windows.Forms.NumericUpDown txtSectors;
        private System.Windows.Forms.Label lblSectors;
        private System.Windows.Forms.NumericUpDown txtSize;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox lstType;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabFloppy;
        private System.Windows.Forms.TabPage tabHDD;
        private System.Windows.Forms.ComboBox lstFloppyCapacity;
        private System.Windows.Forms.Label lblFloppyType;
        private System.Windows.Forms.ComboBox lstFloppyBPB;
        private System.Windows.Forms.CheckBox cbxFloppyBPB;
        private System.Windows.Forms.Label lblFloppyOEMID;
        private System.Windows.Forms.TextBox txtFloppyOEMID;
        private System.Windows.Forms.Label lblFloppyBPS;
        private System.Windows.Forms.NumericUpDown txtFloppyBPS;
        private System.Windows.Forms.Label lblFloppySPC;
        private System.Windows.Forms.NumericUpDown txtFloppySPC;
        private System.Windows.Forms.Label lblFloppyReserved;
        private System.Windows.Forms.Label lblFloppyNumFATs;
        private System.Windows.Forms.Label lblFloppyRootDirEntries;
        private System.Windows.Forms.NumericUpDown txtFloppyRootDirEntries;
        private System.Windows.Forms.NumericUpDown txtFloppyTotalSect;
        private System.Windows.Forms.Label lblFloppyTotalSect;
        private System.Windows.Forms.NumericUpDown txtFloppyMediaDesc;
        private System.Windows.Forms.Label lblFloppyMediaDesc;
        private System.Windows.Forms.NumericUpDown txtFloppySPF;
        private System.Windows.Forms.Label lblFloppySPF;
        private System.Windows.Forms.Label lblFloppySPT;
        private System.Windows.Forms.NumericUpDown txtFloppySPT;
        private System.Windows.Forms.Label lblFloppySides;
        private System.Windows.Forms.ComboBox lstFloppySides;
        private System.Windows.Forms.Label lblFloppySerial;
        private System.Windows.Forms.TextBox txtFloppySerial;
        private System.Windows.Forms.TextBox txtFloppyLabel;
        private System.Windows.Forms.Label lblFloppyLabel;
        private System.Windows.Forms.Label lblFloppyFSType;
        private System.Windows.Forms.TextBox txtFloppyFSType;
        private System.Windows.Forms.NumericUpDown txtFloppyReserved;
        private System.Windows.Forms.NumericUpDown txtFloppyNumFATs;
        private System.Windows.Forms.Label lblFloppyTracks;
        private System.Windows.Forms.NumericUpDown txtFloppyTracks;
    }
}