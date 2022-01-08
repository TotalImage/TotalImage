
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
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbxMediaType = new System.Windows.Forms.GroupBox();
            this.lblContainerFormat = new System.Windows.Forms.Label();
            this.lstContainerFormat = new System.Windows.Forms.ComboBox();
            this.rbnHardDisk = new System.Windows.Forms.RadioButton();
            this.rbnFloppyDisk = new System.Windows.Forms.RadioButton();
            this.gbxOptions = new System.Windows.Forms.GroupBox();
            this.pnlFloppy = new System.Windows.Forms.Panel();
            this.lstFloppyBPB = new System.Windows.Forms.ComboBox();
            this.cbxFloppyBPB = new System.Windows.Forms.CheckBox();
            this.txtFloppyLabel = new System.Windows.Forms.TextBox();
            this.lblFloppyGeometry = new System.Windows.Forms.Label();
            this.lblFloppyLabel = new System.Windows.Forms.Label();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.lstFloppyGeometries = new System.Windows.Forms.ComboBox();
            this.pnlHardDisk = new System.Windows.Forms.Panel();
            this.lstPartitionTable = new System.Windows.Forms.ComboBox();
            this.cbxWritePartTable = new System.Windows.Forms.CheckBox();
            this.rbnDifferencing = new System.Windows.Forms.RadioButton();
            this.rbnDynamic = new System.Windows.Forms.RadioButton();
            this.rbnFixed = new System.Windows.Forms.RadioButton();
            this.lblDiskType = new System.Windows.Forms.Label();
            this.lblDiskSize = new System.Windows.Forms.Label();
            this.txtDiskSize = new System.Windows.Forms.NumericUpDown();
            this.pnlBottom.SuspendLayout();
            this.gbxMediaType.SuspendLayout();
            this.gbxOptions.SuspendLayout();
            this.pnlFloppy.SuspendLayout();
            this.pnlHardDisk.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiskSize)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 341);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(499, 62);
            this.pnlBottom.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(274, 15);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 32);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(381, 15);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 32);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbxMediaType
            // 
            this.gbxMediaType.Controls.Add(this.lblContainerFormat);
            this.gbxMediaType.Controls.Add(this.lstContainerFormat);
            this.gbxMediaType.Controls.Add(this.rbnHardDisk);
            this.gbxMediaType.Controls.Add(this.rbnFloppyDisk);
            this.gbxMediaType.Location = new System.Drawing.Point(13, 15);
            this.gbxMediaType.Margin = new System.Windows.Forms.Padding(4);
            this.gbxMediaType.Name = "gbxMediaType";
            this.gbxMediaType.Padding = new System.Windows.Forms.Padding(4);
            this.gbxMediaType.Size = new System.Drawing.Size(473, 100);
            this.gbxMediaType.TabIndex = 0;
            this.gbxMediaType.TabStop = false;
            this.gbxMediaType.Text = "Media and container type";
            // 
            // lblContainerFormat
            // 
            this.lblContainerFormat.AutoSize = true;
            this.lblContainerFormat.Location = new System.Drawing.Point(8, 64);
            this.lblContainerFormat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblContainerFormat.Name = "lblContainerFormat";
            this.lblContainerFormat.Size = new System.Drawing.Size(125, 20);
            this.lblContainerFormat.TabIndex = 9;
            this.lblContainerFormat.Text = "Container format:";
            // 
            // lstContainerFormat
            // 
            this.lstContainerFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstContainerFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstContainerFormat.FormattingEnabled = true;
            this.lstContainerFormat.Items.AddRange(new object[] {
            "Plain sector image"});
            this.lstContainerFormat.Location = new System.Drawing.Point(141, 61);
            this.lstContainerFormat.Margin = new System.Windows.Forms.Padding(4);
            this.lstContainerFormat.Name = "lstContainerFormat";
            this.lstContainerFormat.Size = new System.Drawing.Size(317, 28);
            this.lstContainerFormat.TabIndex = 9;
            // 
            // rbnHardDisk
            // 
            this.rbnHardDisk.AutoSize = true;
            this.rbnHardDisk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnHardDisk.Location = new System.Drawing.Point(169, 28);
            this.rbnHardDisk.Margin = new System.Windows.Forms.Padding(4);
            this.rbnHardDisk.Name = "rbnHardDisk";
            this.rbnHardDisk.Size = new System.Drawing.Size(102, 25);
            this.rbnHardDisk.TabIndex = 2;
            this.rbnHardDisk.Text = "Hard disk";
            this.rbnHardDisk.UseVisualStyleBackColor = true;
            // 
            // rbnFloppyDisk
            // 
            this.rbnFloppyDisk.AutoSize = true;
            this.rbnFloppyDisk.Checked = true;
            this.rbnFloppyDisk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnFloppyDisk.Location = new System.Drawing.Point(10, 28);
            this.rbnFloppyDisk.Margin = new System.Windows.Forms.Padding(4);
            this.rbnFloppyDisk.Name = "rbnFloppyDisk";
            this.rbnFloppyDisk.Size = new System.Drawing.Size(114, 25);
            this.rbnFloppyDisk.TabIndex = 1;
            this.rbnFloppyDisk.TabStop = true;
            this.rbnFloppyDisk.Text = "Floppy disk";
            this.rbnFloppyDisk.UseVisualStyleBackColor = true;
            this.rbnFloppyDisk.CheckedChanged += new System.EventHandler(this.rbnFloppyDisk_CheckedChanged);
            // 
            // gbxOptions
            // 
            this.gbxOptions.Controls.Add(this.pnlFloppy);
            this.gbxOptions.Controls.Add(this.pnlHardDisk);
            this.gbxOptions.Location = new System.Drawing.Point(13, 123);
            this.gbxOptions.Margin = new System.Windows.Forms.Padding(4);
            this.gbxOptions.Name = "gbxOptions";
            this.gbxOptions.Padding = new System.Windows.Forms.Padding(4);
            this.gbxOptions.Size = new System.Drawing.Size(473, 208);
            this.gbxOptions.TabIndex = 3;
            this.gbxOptions.TabStop = false;
            this.gbxOptions.Text = "Options";
            // 
            // pnlFloppy
            // 
            this.pnlFloppy.Controls.Add(this.lstFloppyBPB);
            this.pnlFloppy.Controls.Add(this.cbxFloppyBPB);
            this.pnlFloppy.Controls.Add(this.txtFloppyLabel);
            this.pnlFloppy.Controls.Add(this.lblFloppyGeometry);
            this.pnlFloppy.Controls.Add(this.lblFloppyLabel);
            this.pnlFloppy.Controls.Add(this.btnAdvanced);
            this.pnlFloppy.Controls.Add(this.lstFloppyGeometries);
            this.pnlFloppy.Location = new System.Drawing.Point(2, 21);
            this.pnlFloppy.Margin = new System.Windows.Forms.Padding(4);
            this.pnlFloppy.Name = "pnlFloppy";
            this.pnlFloppy.Size = new System.Drawing.Size(464, 175);
            this.pnlFloppy.TabIndex = 9;
            // 
            // lstFloppyBPB
            // 
            this.lstFloppyBPB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFloppyBPB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstFloppyBPB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstFloppyBPB.FormattingEnabled = true;
            this.lstFloppyBPB.Items.AddRange(new object[] {
            "DOS 2.0",
            "DOS 3.4",
            "DOS 4.0+"});
            this.lstFloppyBPB.Location = new System.Drawing.Point(280, 108);
            this.lstFloppyBPB.Margin = new System.Windows.Forms.Padding(4);
            this.lstFloppyBPB.Name = "lstFloppyBPB";
            this.lstFloppyBPB.Size = new System.Drawing.Size(175, 28);
            this.lstFloppyBPB.TabIndex = 8;
            this.lstFloppyBPB.SelectedIndexChanged += new System.EventHandler(this.lstFloppyBPB_SelectedIndexChanged);
            // 
            // cbxFloppyBPB
            // 
            this.cbxFloppyBPB.AutoSize = true;
            this.cbxFloppyBPB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxFloppyBPB.Location = new System.Drawing.Point(10, 111);
            this.cbxFloppyBPB.Margin = new System.Windows.Forms.Padding(4);
            this.cbxFloppyBPB.Name = "cbxFloppyBPB";
            this.cbxFloppyBPB.Size = new System.Drawing.Size(278, 25);
            this.cbxFloppyBPB.TabIndex = 7;
            this.cbxFloppyBPB.Text = "Write a DOS BPB to the boot sector:";
            this.cbxFloppyBPB.UseVisualStyleBackColor = true;
            this.cbxFloppyBPB.CheckedChanged += new System.EventHandler(this.cbxFloppyBPB_CheckedChanged);
            // 
            // txtFloppyLabel
            // 
            this.txtFloppyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFloppyLabel.Location = new System.Drawing.Point(112, 74);
            this.txtFloppyLabel.Margin = new System.Windows.Forms.Padding(4);
            this.txtFloppyLabel.MaxLength = 11;
            this.txtFloppyLabel.Name = "txtFloppyLabel";
            this.txtFloppyLabel.Size = new System.Drawing.Size(343, 27);
            this.txtFloppyLabel.TabIndex = 6;
            this.txtFloppyLabel.TextChanged += new System.EventHandler(this.txtFloppyLabel_TextChanged);
            // 
            // lblFloppyGeometry
            // 
            this.lblFloppyGeometry.AutoSize = true;
            this.lblFloppyGeometry.Location = new System.Drawing.Point(8, 12);
            this.lblFloppyGeometry.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFloppyGeometry.Name = "lblFloppyGeometry";
            this.lblFloppyGeometry.Size = new System.Drawing.Size(77, 20);
            this.lblFloppyGeometry.TabIndex = 0;
            this.lblFloppyGeometry.Text = "Geometry:";
            // 
            // lblFloppyLabel
            // 
            this.lblFloppyLabel.AutoSize = true;
            this.lblFloppyLabel.Location = new System.Drawing.Point(8, 78);
            this.lblFloppyLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFloppyLabel.Name = "lblFloppyLabel";
            this.lblFloppyLabel.Size = new System.Drawing.Size(99, 20);
            this.lblFloppyLabel.TabIndex = 3;
            this.lblFloppyLabel.Text = "Volume label:";
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdvanced.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdvanced.Location = new System.Drawing.Point(352, 34);
            this.btnAdvanced.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(104, 32);
            this.btnAdvanced.TabIndex = 5;
            this.btnAdvanced.Text = "Advanced...";
            this.btnAdvanced.UseVisualStyleBackColor = true;
            this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
            // 
            // lstFloppyGeometries
            // 
            this.lstFloppyGeometries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstFloppyGeometries.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstFloppyGeometries.FormattingEnabled = true;
            this.lstFloppyGeometries.Location = new System.Drawing.Point(10, 35);
            this.lstFloppyGeometries.Margin = new System.Windows.Forms.Padding(4);
            this.lstFloppyGeometries.Name = "lstFloppyGeometries";
            this.lstFloppyGeometries.Size = new System.Drawing.Size(336, 28);
            this.lstFloppyGeometries.TabIndex = 4;
            this.lstFloppyGeometries.SelectedIndexChanged += new System.EventHandler(this.lstFloppyGeometries_SelectedIndexChanged);
            // 
            // pnlHardDisk
            // 
            this.pnlHardDisk.Controls.Add(this.lstPartitionTable);
            this.pnlHardDisk.Controls.Add(this.cbxWritePartTable);
            this.pnlHardDisk.Controls.Add(this.rbnDifferencing);
            this.pnlHardDisk.Controls.Add(this.rbnDynamic);
            this.pnlHardDisk.Controls.Add(this.rbnFixed);
            this.pnlHardDisk.Controls.Add(this.lblDiskType);
            this.pnlHardDisk.Controls.Add(this.lblDiskSize);
            this.pnlHardDisk.Controls.Add(this.txtDiskSize);
            this.pnlHardDisk.Location = new System.Drawing.Point(2, 21);
            this.pnlHardDisk.Margin = new System.Windows.Forms.Padding(4);
            this.pnlHardDisk.Name = "pnlHardDisk";
            this.pnlHardDisk.Size = new System.Drawing.Size(464, 175);
            this.pnlHardDisk.TabIndex = 10;
            // 
            // lstPartitionTable
            // 
            this.lstPartitionTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPartitionTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstPartitionTable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstPartitionTable.FormattingEnabled = true;
            this.lstPartitionTable.Items.AddRange(new object[] {
            "Master Boot Record",
            "GUID Partition Table"});
            this.lstPartitionTable.Location = new System.Drawing.Point(270, 46);
            this.lstPartitionTable.Margin = new System.Windows.Forms.Padding(4);
            this.lstPartitionTable.Name = "lstPartitionTable";
            this.lstPartitionTable.Size = new System.Drawing.Size(186, 28);
            this.lstPartitionTable.TabIndex = 15;
            // 
            // cbxWritePartTable
            // 
            this.cbxWritePartTable.AutoSize = true;
            this.cbxWritePartTable.Checked = true;
            this.cbxWritePartTable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxWritePartTable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxWritePartTable.Location = new System.Drawing.Point(10, 48);
            this.cbxWritePartTable.Margin = new System.Windows.Forms.Padding(4);
            this.cbxWritePartTable.Name = "cbxWritePartTable";
            this.cbxWritePartTable.Size = new System.Drawing.Size(263, 25);
            this.cbxWritePartTable.TabIndex = 14;
            this.cbxWritePartTable.Text = "Write a partition table to the disk:";
            this.cbxWritePartTable.UseVisualStyleBackColor = true;
            this.cbxWritePartTable.CheckedChanged += new System.EventHandler(this.cbxWritePartTable_CheckedChanged);
            // 
            // rbnDifferencing
            // 
            this.rbnDifferencing.AutoSize = true;
            this.rbnDifferencing.Enabled = false;
            this.rbnDifferencing.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnDifferencing.Location = new System.Drawing.Point(325, 111);
            this.rbnDifferencing.Margin = new System.Windows.Forms.Padding(4);
            this.rbnDifferencing.Name = "rbnDifferencing";
            this.rbnDifferencing.Size = new System.Drawing.Size(121, 25);
            this.rbnDifferencing.TabIndex = 13;
            this.rbnDifferencing.Text = "Differencing";
            this.rbnDifferencing.UseVisualStyleBackColor = true;
            // 
            // rbnDynamic
            // 
            this.rbnDynamic.AutoSize = true;
            this.rbnDynamic.Enabled = false;
            this.rbnDynamic.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnDynamic.Location = new System.Drawing.Point(123, 111);
            this.rbnDynamic.Margin = new System.Windows.Forms.Padding(4);
            this.rbnDynamic.Name = "rbnDynamic";
            this.rbnDynamic.Size = new System.Drawing.Size(194, 25);
            this.rbnDynamic.TabIndex = 12;
            this.rbnDynamic.Text = "Dynamically expanding";
            this.rbnDynamic.UseVisualStyleBackColor = true;
            // 
            // rbnFixed
            // 
            this.rbnFixed.AutoSize = true;
            this.rbnFixed.Checked = true;
            this.rbnFixed.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnFixed.Location = new System.Drawing.Point(10, 111);
            this.rbnFixed.Margin = new System.Windows.Forms.Padding(4);
            this.rbnFixed.Name = "rbnFixed";
            this.rbnFixed.Size = new System.Drawing.Size(105, 25);
            this.rbnFixed.TabIndex = 10;
            this.rbnFixed.TabStop = true;
            this.rbnFixed.Text = "Fixed-size";
            this.rbnFixed.UseVisualStyleBackColor = true;
            // 
            // lblDiskType
            // 
            this.lblDiskType.AutoSize = true;
            this.lblDiskType.Location = new System.Drawing.Point(6, 87);
            this.lblDiskType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDiskType.Name = "lblDiskType";
            this.lblDiskType.Size = new System.Drawing.Size(73, 20);
            this.lblDiskType.TabIndex = 10;
            this.lblDiskType.Text = "Disk type:";
            // 
            // lblDiskSize
            // 
            this.lblDiskSize.AutoSize = true;
            this.lblDiskSize.Location = new System.Drawing.Point(6, 13);
            this.lblDiskSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDiskSize.Name = "lblDiskSize";
            this.lblDiskSize.Size = new System.Drawing.Size(109, 20);
            this.lblDiskSize.TabIndex = 10;
            this.lblDiskSize.Text = "Disk size (MiB):";
            // 
            // txtDiskSize
            // 
            this.txtDiskSize.Location = new System.Drawing.Point(126, 10);
            this.txtDiskSize.Maximum = new decimal(new int[] {
            2088960,
            0,
            0,
            0});
            this.txtDiskSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtDiskSize.Name = "txtDiskSize";
            this.txtDiskSize.Size = new System.Drawing.Size(150, 27);
            this.txtDiskSize.TabIndex = 11;
            this.txtDiskSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // dlgNewImage
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(499, 403);
            this.Controls.Add(this.gbxOptions);
            this.Controls.Add(this.gbxMediaType);
            this.Controls.Add(this.pnlBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgNewImage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New image";
            this.Load += new System.EventHandler(this.dlgNewImage_Load);
            this.pnlBottom.ResumeLayout(false);
            this.gbxMediaType.ResumeLayout(false);
            this.gbxMediaType.PerformLayout();
            this.gbxOptions.ResumeLayout(false);
            this.pnlFloppy.ResumeLayout(false);
            this.pnlFloppy.PerformLayout();
            this.pnlHardDisk.ResumeLayout(false);
            this.pnlHardDisk.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiskSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.GroupBox gbxMediaType;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton rbnFloppyDisk;
        private System.Windows.Forms.RadioButton rbnHardDisk;
        private System.Windows.Forms.GroupBox gbxOptions;
        private System.Windows.Forms.Label lblFloppyGeometry;
        private System.Windows.Forms.Button btnAdvanced;
        private System.Windows.Forms.Label lblFloppyLabel;
        private System.Windows.Forms.TextBox txtFloppyLabel;
        private System.Windows.Forms.Panel pnlFloppy;
        private System.Windows.Forms.Panel pnlHardDisk;
        private System.Windows.Forms.ComboBox lstFloppyGeometries;
        internal System.Windows.Forms.CheckBox cbxFloppyBPB;
        internal System.Windows.Forms.ComboBox lstFloppyBPB;
        private System.Windows.Forms.Label lblContainerFormat;
        private System.Windows.Forms.ComboBox lstContainerFormat;
        private System.Windows.Forms.Label lblDiskSize;
        private System.Windows.Forms.NumericUpDown txtDiskSize;
        private System.Windows.Forms.RadioButton rbnDifferencing;
        private System.Windows.Forms.RadioButton rbnDynamic;
        private System.Windows.Forms.RadioButton rbnFixed;
        private System.Windows.Forms.Label lblDiskType;
        internal System.Windows.Forms.ComboBox lstPartitionTable;
        internal System.Windows.Forms.CheckBox cbxWritePartTable;
    }
}