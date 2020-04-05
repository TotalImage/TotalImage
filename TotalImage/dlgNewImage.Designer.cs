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
            this.gbxMediaType = new System.Windows.Forms.GroupBox();
            this.rbnHardDisk = new System.Windows.Forms.RadioButton();
            this.rbnFloppy = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbxFloppyCapacity = new System.Windows.Forms.GroupBox();
            this.rbn172m = new System.Windows.Forms.RadioButton();
            this.rbn800k = new System.Windows.Forms.RadioButton();
            this.rbn35_XDF2 = new System.Windows.Forms.RadioButton();
            this.rbn125m = new System.Windows.Forms.RadioButton();
            this.rbn35_320k = new System.Windows.Forms.RadioButton();
            this.rbn35_360k = new System.Windows.Forms.RadioButton();
            this.rbn640k = new System.Windows.Forms.RadioButton();
            this.rbn400k = new System.Windows.Forms.RadioButton();
            this.rbn288m = new System.Windows.Forms.RadioButton();
            this.rbnCustom = new System.Windows.Forms.RadioButton();
            this.rbn35_XDF1 = new System.Windows.Forms.RadioButton();
            this.rbn525_XDF = new System.Windows.Forms.RadioButton();
            this.rbnDMF = new System.Windows.Forms.RadioButton();
            this.rbn144m = new System.Windows.Forms.RadioButton();
            this.rbn12m = new System.Windows.Forms.RadioButton();
            this.rbn720k = new System.Windows.Forms.RadioButton();
            this.rbn525_360k = new System.Windows.Forms.RadioButton();
            this.rbn525_320k = new System.Windows.Forms.RadioButton();
            this.rbn180k = new System.Windows.Forms.RadioButton();
            this.rbn160k = new System.Windows.Forms.RadioButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbxHardDiskCapacity = new System.Windows.Forms.GroupBox();
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
            this.gbxMediaType.SuspendLayout();
            this.gbxFloppyCapacity.SuspendLayout();
            this.gbxHardDiskCapacity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCylinders)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxMediaType
            // 
            this.gbxMediaType.Controls.Add(this.rbnHardDisk);
            this.gbxMediaType.Controls.Add(this.rbnFloppy);
            this.gbxMediaType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbxMediaType.Location = new System.Drawing.Point(12, 12);
            this.gbxMediaType.Name = "gbxMediaType";
            this.gbxMediaType.Size = new System.Drawing.Size(465, 58);
            this.gbxMediaType.TabIndex = 1;
            this.gbxMediaType.TabStop = false;
            this.gbxMediaType.Text = "Media type";
            // 
            // rbnHardDisk
            // 
            this.rbnHardDisk.AutoSize = true;
            this.rbnHardDisk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnHardDisk.Location = new System.Drawing.Point(127, 22);
            this.rbnHardDisk.Name = "rbnHardDisk";
            this.rbnHardDisk.Size = new System.Drawing.Size(81, 20);
            this.rbnHardDisk.TabIndex = 3;
            this.rbnHardDisk.TabStop = true;
            this.rbnHardDisk.Text = "Hard disk";
            this.rbnHardDisk.UseVisualStyleBackColor = true;
            this.rbnHardDisk.CheckedChanged += new System.EventHandler(this.rbnHardDisk_CheckedChanged);
            // 
            // rbnFloppy
            // 
            this.rbnFloppy.AutoSize = true;
            this.rbnFloppy.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnFloppy.Location = new System.Drawing.Point(15, 22);
            this.rbnFloppy.Name = "rbnFloppy";
            this.rbnFloppy.Size = new System.Drawing.Size(91, 20);
            this.rbnFloppy.TabIndex = 2;
            this.rbnFloppy.TabStop = true;
            this.rbnFloppy.Text = "Floppy disk";
            this.rbnFloppy.UseVisualStyleBackColor = true;
            this.rbnFloppy.CheckedChanged += new System.EventHandler(this.rbnFloppy_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(397, 353);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(311, 353);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbxFloppyCapacity
            // 
            this.gbxFloppyCapacity.Controls.Add(this.rbn172m);
            this.gbxFloppyCapacity.Controls.Add(this.rbn800k);
            this.gbxFloppyCapacity.Controls.Add(this.rbn35_XDF2);
            this.gbxFloppyCapacity.Controls.Add(this.rbn125m);
            this.gbxFloppyCapacity.Controls.Add(this.rbn35_320k);
            this.gbxFloppyCapacity.Controls.Add(this.rbn35_360k);
            this.gbxFloppyCapacity.Controls.Add(this.rbn640k);
            this.gbxFloppyCapacity.Controls.Add(this.rbn400k);
            this.gbxFloppyCapacity.Controls.Add(this.rbn288m);
            this.gbxFloppyCapacity.Controls.Add(this.rbnCustom);
            this.gbxFloppyCapacity.Controls.Add(this.rbn35_XDF1);
            this.gbxFloppyCapacity.Controls.Add(this.rbn525_XDF);
            this.gbxFloppyCapacity.Controls.Add(this.rbnDMF);
            this.gbxFloppyCapacity.Controls.Add(this.rbn144m);
            this.gbxFloppyCapacity.Controls.Add(this.rbn12m);
            this.gbxFloppyCapacity.Controls.Add(this.rbn720k);
            this.gbxFloppyCapacity.Controls.Add(this.rbn525_360k);
            this.gbxFloppyCapacity.Controls.Add(this.rbn525_320k);
            this.gbxFloppyCapacity.Controls.Add(this.rbn180k);
            this.gbxFloppyCapacity.Controls.Add(this.rbn160k);
            this.gbxFloppyCapacity.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbxFloppyCapacity.Location = new System.Drawing.Point(12, 76);
            this.gbxFloppyCapacity.Name = "gbxFloppyCapacity";
            this.gbxFloppyCapacity.Size = new System.Drawing.Size(465, 159);
            this.gbxFloppyCapacity.TabIndex = 6;
            this.gbxFloppyCapacity.TabStop = false;
            this.gbxFloppyCapacity.Text = "Floppy disk capacity";
            // 
            // rbn172m
            // 
            this.rbn172m.AutoSize = true;
            this.rbn172m.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn172m.Location = new System.Drawing.Point(362, 22);
            this.rbn172m.Name = "rbn172m";
            this.rbn172m.Size = new System.Drawing.Size(75, 20);
            this.rbn172m.TabIndex = 35;
            this.rbn172m.TabStop = true;
            this.rbn172m.Tag = "15";
            this.rbn172m.Text = "1722 KiB";
            this.toolTip.SetToolTip(this.rbn172m, "Physical size: 3.5\"\r\nDensity: high (HD)\r\nSides: 2 (DS)\r\nBytes per sector: 512\r\nSe" +
        "ctors per track: 21\r\nTracks: 82\r\n\r\nRare format.");
            this.rbn172m.UseVisualStyleBackColor = true;
            // 
            // rbn800k
            // 
            this.rbn800k.AutoSize = true;
            this.rbn800k.Enabled = false;
            this.rbn800k.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn800k.Location = new System.Drawing.Point(139, 126);
            this.rbn800k.Name = "rbn800k";
            this.rbn800k.Size = new System.Drawing.Size(69, 20);
            this.rbn800k.TabIndex = 34;
            this.rbn800k.TabStop = true;
            this.rbn800k.Tag = "9";
            this.rbn800k.Text = "800 KiB";
            this.toolTip.SetToolTip(this.rbn800k, "Physical size: 3.5\"\r\nDensity: double (DD)\r\nSides: 2 (DS)\r\nBytes per sector: 512\r\n" +
        "Sectors per track: 10\r\nTracks: 80\r\n\r\nRare format.");
            this.rbn800k.UseVisualStyleBackColor = true;
            // 
            // rbn35_XDF2
            // 
            this.rbn35_XDF2.AutoSize = true;
            this.rbn35_XDF2.Enabled = false;
            this.rbn35_XDF2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn35_XDF2.Location = new System.Drawing.Point(362, 100);
            this.rbn35_XDF2.Name = "rbn35_XDF2";
            this.rbn35_XDF2.Size = new System.Drawing.Size(75, 20);
            this.rbn35_XDF2.TabIndex = 33;
            this.rbn35_XDF2.TabStop = true;
            this.rbn35_XDF2.Tag = "18";
            this.rbn35_XDF2.Text = "3680 KiB";
            this.toolTip.SetToolTip(this.rbn35_XDF2, "Physical size: 3.5\"\r\nDensity: extended (ED)\r\nSides: 2 (DS)\r\nBytes per sector: var" +
        "iable\r\nSectors per track: ???\r\nTracks: 80\r\n\r\nIBM XDF proprietary format.\r\n");
            this.rbn35_XDF2.UseVisualStyleBackColor = true;
            // 
            // rbn125m
            // 
            this.rbn125m.AutoSize = true;
            this.rbn125m.Enabled = false;
            this.rbn125m.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn125m.Location = new System.Drawing.Point(261, 48);
            this.rbn125m.Name = "rbn125m";
            this.rbn125m.Size = new System.Drawing.Size(75, 20);
            this.rbn125m.TabIndex = 32;
            this.rbn125m.TabStop = true;
            this.rbn125m.Tag = "11";
            this.rbn125m.Text = "1232 KiB";
            this.toolTip.SetToolTip(this.rbn125m, "Physical size: 5.25\" or 3.5\"\r\nDensity: high (HD)\r\nSides: 2 (DS)\r\nBytes per sector" +
        ": 1024\r\nSectors per track: 8\r\nTracks: 77\r\n\r\nNEC PC-98 proprietary format.");
            this.rbn125m.UseVisualStyleBackColor = true;
            // 
            // rbn35_320k
            // 
            this.rbn35_320k.AutoSize = true;
            this.rbn35_320k.Enabled = false;
            this.rbn35_320k.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn35_320k.Location = new System.Drawing.Point(12, 100);
            this.rbn35_320k.Name = "rbn35_320k";
            this.rbn35_320k.Size = new System.Drawing.Size(100, 20);
            this.rbn35_320k.TabIndex = 30;
            this.rbn35_320k.TabStop = true;
            this.rbn35_320k.Tag = "3";
            this.rbn35_320k.Text = "320 KiB (3.5\")";
            this.toolTip.SetToolTip(this.rbn35_320k, "Physical size: 3.5\"\r\nDensity: double (DD)\r\nSides: 1 (SS)\r\nBytes per sector: 512\r\n" +
        "Sectors per track: 8\r\nTracks: 80\r\n\r\nRare format.");
            this.rbn35_320k.UseVisualStyleBackColor = true;
            // 
            // rbn35_360k
            // 
            this.rbn35_360k.AutoSize = true;
            this.rbn35_360k.Enabled = false;
            this.rbn35_360k.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn35_360k.Location = new System.Drawing.Point(139, 22);
            this.rbn35_360k.Name = "rbn35_360k";
            this.rbn35_360k.Size = new System.Drawing.Size(100, 20);
            this.rbn35_360k.TabIndex = 29;
            this.rbn35_360k.TabStop = true;
            this.rbn35_360k.Tag = "5";
            this.rbn35_360k.Text = "360 KiB (3.5\")";
            this.toolTip.SetToolTip(this.rbn35_360k, "Physical size: 3.5\"\r\nDensity: double (DD)\r\nSides: 1 (SS)\r\nBytes per sector: 512\r\n" +
        "Sectors per track: 9\r\nTracks: 80\r\n\r\nRare format.");
            this.rbn35_360k.UseVisualStyleBackColor = true;
            // 
            // rbn640k
            // 
            this.rbn640k.AutoSize = true;
            this.rbn640k.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn640k.Location = new System.Drawing.Point(139, 74);
            this.rbn640k.Name = "rbn640k";
            this.rbn640k.Size = new System.Drawing.Size(69, 20);
            this.rbn640k.TabIndex = 28;
            this.rbn640k.TabStop = true;
            this.rbn640k.Tag = "7";
            this.rbn640k.Text = "640 KiB";
            this.toolTip.SetToolTip(this.rbn640k, "Physical size: 5.25\" or 3.5\"\r\nDensity: double (DD) or quad (QD)\r\nSides: 2 (DS)\r\nB" +
        "ytes per sector: 512\r\nSectors per track: 8\r\nTracks: 80\r\n\r\nRare format.");
            this.rbn640k.UseVisualStyleBackColor = true;
            // 
            // rbn400k
            // 
            this.rbn400k.AutoSize = true;
            this.rbn400k.Enabled = false;
            this.rbn400k.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn400k.Location = new System.Drawing.Point(139, 48);
            this.rbn400k.Name = "rbn400k";
            this.rbn400k.Size = new System.Drawing.Size(69, 20);
            this.rbn400k.TabIndex = 27;
            this.rbn400k.TabStop = true;
            this.rbn400k.Tag = "6";
            this.rbn400k.Text = "400 KiB";
            this.toolTip.SetToolTip(this.rbn400k, "Physical size: 5.25\"\r\nDensity: double (DD)\r\nSides: 1 (SS)\r\nBytes per sector: 512\r" +
        "\nSectors per track: 10\r\nTracks: 80\r\n\r\nDEC RX50 proprietary format.");
            this.rbn400k.UseVisualStyleBackColor = true;
            // 
            // rbn288m
            // 
            this.rbn288m.AutoSize = true;
            this.rbn288m.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn288m.Location = new System.Drawing.Point(362, 74);
            this.rbn288m.Name = "rbn288m";
            this.rbn288m.Size = new System.Drawing.Size(75, 20);
            this.rbn288m.TabIndex = 26;
            this.rbn288m.TabStop = true;
            this.rbn288m.Tag = "17";
            this.rbn288m.Text = "2880 KiB";
            this.toolTip.SetToolTip(this.rbn288m, "Physical size: 3.5\"\r\nDensity: high (HD)\r\nSides: 2 (DS)\r\nBytes per sector: 512\r\nSe" +
        "ctors per track: 36\r\nTracks: 80\r\n\r\nRare format.");
            this.rbn288m.UseVisualStyleBackColor = true;
            // 
            // rbnCustom
            // 
            this.rbnCustom.AutoSize = true;
            this.rbnCustom.Enabled = false;
            this.rbnCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnCustom.Location = new System.Drawing.Point(362, 126);
            this.rbnCustom.Name = "rbnCustom";
            this.rbnCustom.Size = new System.Drawing.Size(82, 20);
            this.rbnCustom.TabIndex = 25;
            this.rbnCustom.TabStop = true;
            this.rbnCustom.Text = "Custom...";
            this.toolTip.SetToolTip(this.rbnCustom, "Define custom parameters.");
            this.rbnCustom.UseVisualStyleBackColor = true;
            // 
            // rbn35_XDF1
            // 
            this.rbn35_XDF1.AutoSize = true;
            this.rbn35_XDF1.Enabled = false;
            this.rbn35_XDF1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn35_XDF1.Location = new System.Drawing.Point(362, 48);
            this.rbn35_XDF1.Name = "rbn35_XDF1";
            this.rbn35_XDF1.Size = new System.Drawing.Size(75, 20);
            this.rbn35_XDF1.TabIndex = 24;
            this.rbn35_XDF1.TabStop = true;
            this.rbn35_XDF1.Tag = "16";
            this.rbn35_XDF1.Text = "1840 KiB";
            this.toolTip.SetToolTip(this.rbn35_XDF1, "Physical size: 3.5\"\r\nDensity: high (HD)\r\nSides: 2 (DS)\r\nBytes per sector: variabl" +
        "e\r\nSectors per track: ???\r\nTracks: 80\r\n\r\nIBM XDF proprietary format.\r\n");
            this.rbn35_XDF1.UseVisualStyleBackColor = true;
            // 
            // rbn525_XDF
            // 
            this.rbn525_XDF.AutoSize = true;
            this.rbn525_XDF.Enabled = false;
            this.rbn525_XDF.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn525_XDF.Location = new System.Drawing.Point(261, 100);
            this.rbn525_XDF.Name = "rbn525_XDF";
            this.rbn525_XDF.Size = new System.Drawing.Size(75, 20);
            this.rbn525_XDF.TabIndex = 23;
            this.rbn525_XDF.TabStop = true;
            this.rbn525_XDF.Tag = "13";
            this.rbn525_XDF.Text = "1520 KiB";
            this.toolTip.SetToolTip(this.rbn525_XDF, "Physical size: 5.25\"\r\nDensity: high (HD)\r\nSides: 2 (DS)\r\nBytes per sector: variab" +
        "le\r\nSectors per track: ???\r\nTracks: 80\r\n\r\nIBM XDF proprietary format.");
            this.rbn525_XDF.UseVisualStyleBackColor = true;
            // 
            // rbnDMF
            // 
            this.rbnDMF.AutoSize = true;
            this.rbnDMF.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnDMF.Location = new System.Drawing.Point(261, 126);
            this.rbnDMF.Name = "rbnDMF";
            this.rbnDMF.Size = new System.Drawing.Size(75, 20);
            this.rbnDMF.TabIndex = 21;
            this.rbnDMF.TabStop = true;
            this.rbnDMF.Tag = "14";
            this.rbnDMF.Text = "1680 KiB";
            this.toolTip.SetToolTip(this.rbnDMF, "Physical size: 3.5\"\r\nDensity: high (HD)\r\nSides: 2 (DS)\r\nBytes per sector: 512\r\nSe" +
        "ctors per track: 21\r\nTracks: 80\r\n\r\nMicrosoft DMF proprietary format.\r\n");
            this.rbnDMF.UseVisualStyleBackColor = true;
            // 
            // rbn144m
            // 
            this.rbn144m.AutoSize = true;
            this.rbn144m.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn144m.Location = new System.Drawing.Point(261, 74);
            this.rbn144m.Name = "rbn144m";
            this.rbn144m.Size = new System.Drawing.Size(75, 20);
            this.rbn144m.TabIndex = 20;
            this.rbn144m.TabStop = true;
            this.rbn144m.Tag = "12";
            this.rbn144m.Text = "1440 KiB";
            this.toolTip.SetToolTip(this.rbn144m, "Physical size: 3.5\"\r\nDensity: high (HD)\r\nSides: 2 (DS)\r\nBytes per sector: 512\r\nSe" +
        "ctors per track: 18\r\nTracks: 80\r\n\r\nStandard PC-compatible format.\r\n");
            this.rbn144m.UseVisualStyleBackColor = true;
            // 
            // rbn12m
            // 
            this.rbn12m.AutoSize = true;
            this.rbn12m.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn12m.Location = new System.Drawing.Point(261, 22);
            this.rbn12m.Name = "rbn12m";
            this.rbn12m.Size = new System.Drawing.Size(75, 20);
            this.rbn12m.TabIndex = 18;
            this.rbn12m.TabStop = true;
            this.rbn12m.Tag = "10";
            this.rbn12m.Text = "1200 KiB";
            this.toolTip.SetToolTip(this.rbn12m, resources.GetString("rbn12m.ToolTip"));
            this.rbn12m.UseVisualStyleBackColor = true;
            // 
            // rbn720k
            // 
            this.rbn720k.AutoSize = true;
            this.rbn720k.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn720k.Location = new System.Drawing.Point(139, 100);
            this.rbn720k.Name = "rbn720k";
            this.rbn720k.Size = new System.Drawing.Size(69, 20);
            this.rbn720k.TabIndex = 17;
            this.rbn720k.TabStop = true;
            this.rbn720k.Tag = "8";
            this.rbn720k.Text = "720 KiB";
            this.toolTip.SetToolTip(this.rbn720k, resources.GetString("rbn720k.ToolTip"));
            this.rbn720k.UseVisualStyleBackColor = true;
            // 
            // rbn525_360k
            // 
            this.rbn525_360k.AutoSize = true;
            this.rbn525_360k.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn525_360k.Location = new System.Drawing.Point(12, 126);
            this.rbn525_360k.Name = "rbn525_360k";
            this.rbn525_360k.Size = new System.Drawing.Size(106, 20);
            this.rbn525_360k.TabIndex = 16;
            this.rbn525_360k.TabStop = true;
            this.rbn525_360k.Tag = "4";
            this.rbn525_360k.Text = "360 KiB (5.25\")";
            this.toolTip.SetToolTip(this.rbn525_360k, "Physical size: 5.25\"\r\nDensity: double (DD)\r\nSides: 2 (DS)\r\nBytes per sector: 512\r" +
        "\nSectors per track: 9\r\nTracks: 40\r\n\r\nStandard IBM PC-compatible format.");
            this.rbn525_360k.UseVisualStyleBackColor = true;
            // 
            // rbn525_320k
            // 
            this.rbn525_320k.AutoSize = true;
            this.rbn525_320k.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn525_320k.Location = new System.Drawing.Point(12, 74);
            this.rbn525_320k.Name = "rbn525_320k";
            this.rbn525_320k.Size = new System.Drawing.Size(106, 20);
            this.rbn525_320k.TabIndex = 15;
            this.rbn525_320k.TabStop = true;
            this.rbn525_320k.Tag = "2";
            this.rbn525_320k.Text = "320 KiB (5.25\")";
            this.toolTip.SetToolTip(this.rbn525_320k, "Physical size: 5.25\"\r\nDensity: double (DD)\r\nSides: 2 (DS)\r\nBytes per sector: 512\r" +
        "\nSectors per track: 8\r\nTracks: 40\r\n\r\nStandard IBM PC-compatible format.");
            this.rbn525_320k.UseVisualStyleBackColor = true;
            // 
            // rbn180k
            // 
            this.rbn180k.AutoSize = true;
            this.rbn180k.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn180k.Location = new System.Drawing.Point(12, 48);
            this.rbn180k.Name = "rbn180k";
            this.rbn180k.Size = new System.Drawing.Size(69, 20);
            this.rbn180k.TabIndex = 14;
            this.rbn180k.TabStop = true;
            this.rbn180k.Tag = "1";
            this.rbn180k.Text = "180 KiB";
            this.toolTip.SetToolTip(this.rbn180k, "Physical size: 5.25\"\r\nDensity: double (DD)\r\nSides: 1 (SS)\r\nBytes per sector: 512\r" +
        "\nSectors per track: 9\r\nTracks: 40\r\n\r\nStandard IBM PC-compatible format.");
            this.rbn180k.UseVisualStyleBackColor = true;
            // 
            // rbn160k
            // 
            this.rbn160k.AutoSize = true;
            this.rbn160k.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbn160k.Location = new System.Drawing.Point(12, 22);
            this.rbn160k.Name = "rbn160k";
            this.rbn160k.Size = new System.Drawing.Size(69, 20);
            this.rbn160k.TabIndex = 13;
            this.rbn160k.TabStop = true;
            this.rbn160k.Tag = "0";
            this.rbn160k.Text = "160 KiB";
            this.toolTip.SetToolTip(this.rbn160k, "Physical size: 5.25\"\r\nDensity: double (DD)\r\nSides: 1 (SS)\r\nBytes per sector: 512\r" +
        "\nSectors per track: 8\r\nTracks: 40\r\n\r\nStandard IBM PC-compatible format.");
            this.rbn160k.UseVisualStyleBackColor = true;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 10000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "Format information";
            // 
            // gbxHardDiskCapacity
            // 
            this.gbxHardDiskCapacity.Controls.Add(this.lstType);
            this.gbxHardDiskCapacity.Controls.Add(this.lblType);
            this.gbxHardDiskCapacity.Controls.Add(this.txtSize);
            this.gbxHardDiskCapacity.Controls.Add(this.lblSize);
            this.gbxHardDiskCapacity.Controls.Add(this.txtSectors);
            this.gbxHardDiskCapacity.Controls.Add(this.lblSectors);
            this.gbxHardDiskCapacity.Controls.Add(this.txtHeads);
            this.gbxHardDiskCapacity.Controls.Add(this.lblHeads);
            this.gbxHardDiskCapacity.Controls.Add(this.txtCylinders);
            this.gbxHardDiskCapacity.Controls.Add(this.lblCylinders);
            this.gbxHardDiskCapacity.Enabled = false;
            this.gbxHardDiskCapacity.Location = new System.Drawing.Point(12, 241);
            this.gbxHardDiskCapacity.Name = "gbxHardDiskCapacity";
            this.gbxHardDiskCapacity.Size = new System.Drawing.Size(465, 96);
            this.gbxHardDiskCapacity.TabIndex = 7;
            this.gbxHardDiskCapacity.TabStop = false;
            this.gbxHardDiskCapacity.Text = "Hard disk capacity";
            // 
            // lstType
            // 
            this.lstType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstType.FormattingEnabled = true;
            this.lstType.Location = new System.Drawing.Point(214, 58);
            this.lstType.MaxDropDownItems = 99;
            this.lstType.Name = "lstType";
            this.lstType.Size = new System.Drawing.Size(223, 23);
            this.lstType.TabIndex = 9;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(165, 63);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(34, 15);
            this.lblType.TabIndex = 8;
            this.lblType.Text = "Type:";
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(74, 58);
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
            this.lblSize.Location = new System.Drawing.Point(9, 63);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(59, 15);
            this.lblSize.TabIndex = 6;
            this.lblSize.Text = "Size (MB):";
            // 
            // txtSectors
            // 
            this.txtSectors.Location = new System.Drawing.Point(362, 22);
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
            this.lblSectors.Location = new System.Drawing.Point(308, 26);
            this.lblSectors.Name = "lblSectors";
            this.lblSectors.Size = new System.Drawing.Size(48, 15);
            this.lblSectors.TabIndex = 4;
            this.lblSectors.Text = "Sectors:";
            // 
            // txtHeads
            // 
            this.txtHeads.Location = new System.Drawing.Point(214, 22);
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
            this.lblHeads.Location = new System.Drawing.Point(165, 26);
            this.lblHeads.Name = "lblHeads";
            this.lblHeads.Size = new System.Drawing.Size(43, 15);
            this.lblHeads.TabIndex = 2;
            this.lblHeads.Text = "Heads:";
            // 
            // txtCylinders
            // 
            this.txtCylinders.Location = new System.Drawing.Point(74, 22);
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
            this.lblCylinders.Location = new System.Drawing.Point(9, 26);
            this.lblCylinders.Name = "lblCylinders";
            this.lblCylinders.Size = new System.Drawing.Size(59, 15);
            this.lblCylinders.TabIndex = 0;
            this.lblCylinders.Text = "Cylinders:";
            // 
            // dlgNewImage
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(489, 391);
            this.Controls.Add(this.gbxHardDiskCapacity);
            this.Controls.Add(this.gbxFloppyCapacity);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbxMediaType);
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
            this.gbxMediaType.ResumeLayout(false);
            this.gbxMediaType.PerformLayout();
            this.gbxFloppyCapacity.ResumeLayout(false);
            this.gbxFloppyCapacity.PerformLayout();
            this.gbxHardDiskCapacity.ResumeLayout(false);
            this.gbxHardDiskCapacity.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCylinders)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxMediaType;
        private System.Windows.Forms.RadioButton rbnHardDisk;
        private System.Windows.Forms.RadioButton rbnFloppy;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbxFloppyCapacity;
        private System.Windows.Forms.RadioButton rbn525_360k;
        private System.Windows.Forms.RadioButton rbn525_320k;
        private System.Windows.Forms.RadioButton rbn180k;
        private System.Windows.Forms.RadioButton rbn160k;
        private System.Windows.Forms.RadioButton rbn144m;
        private System.Windows.Forms.RadioButton rbn12m;
        private System.Windows.Forms.RadioButton rbn720k;
        private System.Windows.Forms.RadioButton rbnDMF;
        private System.Windows.Forms.RadioButton rbn35_XDF1;
        private System.Windows.Forms.RadioButton rbn525_XDF;
        private System.Windows.Forms.RadioButton rbnCustom;
        private System.Windows.Forms.RadioButton rbn400k;
        private System.Windows.Forms.RadioButton rbn288m;
        private System.Windows.Forms.RadioButton rbn35_320k;
        private System.Windows.Forms.RadioButton rbn35_360k;
        private System.Windows.Forms.RadioButton rbn640k;
        private System.Windows.Forms.RadioButton rbn125m;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.RadioButton rbn35_XDF2;
        private System.Windows.Forms.RadioButton rbn800k;
        private System.Windows.Forms.RadioButton rbn172m;
        private System.Windows.Forms.GroupBox gbxHardDiskCapacity;
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
    }
}