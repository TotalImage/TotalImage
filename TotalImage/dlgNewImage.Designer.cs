
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
            pnlBottom = new System.Windows.Forms.Panel();
            btnOK = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            gbxMediaType = new System.Windows.Forms.GroupBox();
            lblContainerFormat = new System.Windows.Forms.Label();
            lstContainerFormat = new System.Windows.Forms.ComboBox();
            rbnHardDisk = new System.Windows.Forms.RadioButton();
            rbnFloppyDisk = new System.Windows.Forms.RadioButton();
            gbxOptions = new System.Windows.Forms.GroupBox();
            pnlFloppy = new System.Windows.Forms.Panel();
            lstFloppyBPB = new System.Windows.Forms.ComboBox();
            cbxFloppyBPB = new System.Windows.Forms.CheckBox();
            txtFloppyLabel = new System.Windows.Forms.TextBox();
            lblFloppyGeometry = new System.Windows.Forms.Label();
            lblFloppyLabel = new System.Windows.Forms.Label();
            btnAdvanced = new System.Windows.Forms.Button();
            lstFloppyGeometries = new System.Windows.Forms.ComboBox();
            pnlHardDisk = new System.Windows.Forms.Panel();
            lstPartitionTable = new System.Windows.Forms.ComboBox();
            cbxWritePartTable = new System.Windows.Forms.CheckBox();
            rbnDifferencing = new System.Windows.Forms.RadioButton();
            rbnDynamic = new System.Windows.Forms.RadioButton();
            rbnFixed = new System.Windows.Forms.RadioButton();
            lblDiskType = new System.Windows.Forms.Label();
            lblDiskSize = new System.Windows.Forms.Label();
            txtDiskSize = new System.Windows.Forms.NumericUpDown();
            pnlBottom.SuspendLayout();
            gbxMediaType.SuspendLayout();
            gbxOptions.SuspendLayout();
            pnlFloppy.SuspendLayout();
            pnlHardDisk.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtDiskSize).BeginInit();
            SuspendLayout();
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 272);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(399, 50);
            pnlBottom.TabIndex = 0;
            // 
            // btnOK
            // 
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnOK.Location = new System.Drawing.Point(219, 12);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 9;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnCancel.Location = new System.Drawing.Point(305, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(80, 26);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbxMediaType
            // 
            gbxMediaType.Controls.Add(lblContainerFormat);
            gbxMediaType.Controls.Add(lstContainerFormat);
            gbxMediaType.Controls.Add(rbnHardDisk);
            gbxMediaType.Controls.Add(rbnFloppyDisk);
            gbxMediaType.Location = new System.Drawing.Point(10, 12);
            gbxMediaType.Name = "gbxMediaType";
            gbxMediaType.Size = new System.Drawing.Size(378, 80);
            gbxMediaType.TabIndex = 0;
            gbxMediaType.TabStop = false;
            gbxMediaType.Text = "Media and container type";
            // 
            // lblContainerFormat
            // 
            lblContainerFormat.AutoSize = true;
            lblContainerFormat.Location = new System.Drawing.Point(6, 51);
            lblContainerFormat.Name = "lblContainerFormat";
            lblContainerFormat.Size = new System.Drawing.Size(101, 15);
            lblContainerFormat.TabIndex = 9;
            lblContainerFormat.Text = "Container format:";
            // 
            // lstContainerFormat
            // 
            lstContainerFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstContainerFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            lstContainerFormat.FormattingEnabled = true;
            lstContainerFormat.Items.AddRange(new object[] { "Plain sector image" });
            lstContainerFormat.Location = new System.Drawing.Point(113, 49);
            lstContainerFormat.Name = "lstContainerFormat";
            lstContainerFormat.Size = new System.Drawing.Size(254, 23);
            lstContainerFormat.TabIndex = 9;
            // 
            // rbnHardDisk
            // 
            rbnHardDisk.AutoSize = true;
            rbnHardDisk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            rbnHardDisk.Location = new System.Drawing.Point(135, 22);
            rbnHardDisk.Name = "rbnHardDisk";
            rbnHardDisk.Size = new System.Drawing.Size(81, 20);
            rbnHardDisk.TabIndex = 2;
            rbnHardDisk.Text = "Hard disk";
            rbnHardDisk.UseVisualStyleBackColor = true;
            // 
            // rbnFloppyDisk
            // 
            rbnFloppyDisk.AutoSize = true;
            rbnFloppyDisk.Checked = true;
            rbnFloppyDisk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            rbnFloppyDisk.Location = new System.Drawing.Point(8, 22);
            rbnFloppyDisk.Name = "rbnFloppyDisk";
            rbnFloppyDisk.Size = new System.Drawing.Size(91, 20);
            rbnFloppyDisk.TabIndex = 1;
            rbnFloppyDisk.TabStop = true;
            rbnFloppyDisk.Text = "Floppy disk";
            rbnFloppyDisk.UseVisualStyleBackColor = true;
            rbnFloppyDisk.CheckedChanged += rbnFloppyDisk_CheckedChanged;
            // 
            // gbxOptions
            // 
            gbxOptions.Controls.Add(pnlFloppy);
            gbxOptions.Controls.Add(pnlHardDisk);
            gbxOptions.Location = new System.Drawing.Point(10, 98);
            gbxOptions.Name = "gbxOptions";
            gbxOptions.Size = new System.Drawing.Size(378, 166);
            gbxOptions.TabIndex = 3;
            gbxOptions.TabStop = false;
            gbxOptions.Text = "Options";
            // 
            // pnlFloppy
            // 
            pnlFloppy.Controls.Add(lstFloppyBPB);
            pnlFloppy.Controls.Add(cbxFloppyBPB);
            pnlFloppy.Controls.Add(txtFloppyLabel);
            pnlFloppy.Controls.Add(lblFloppyGeometry);
            pnlFloppy.Controls.Add(lblFloppyLabel);
            pnlFloppy.Controls.Add(btnAdvanced);
            pnlFloppy.Controls.Add(lstFloppyGeometries);
            pnlFloppy.Location = new System.Drawing.Point(2, 17);
            pnlFloppy.Name = "pnlFloppy";
            pnlFloppy.Size = new System.Drawing.Size(371, 140);
            pnlFloppy.TabIndex = 9;
            // 
            // lstFloppyBPB
            // 
            lstFloppyBPB.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lstFloppyBPB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstFloppyBPB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            lstFloppyBPB.FormattingEnabled = true;
            lstFloppyBPB.Items.AddRange(new object[] { "DOS 2.0", "DOS 3.4", "DOS 4.0+" });
            lstFloppyBPB.Location = new System.Drawing.Point(224, 86);
            lstFloppyBPB.Name = "lstFloppyBPB";
            lstFloppyBPB.Size = new System.Drawing.Size(141, 23);
            lstFloppyBPB.TabIndex = 8;
            lstFloppyBPB.SelectedIndexChanged += lstFloppyBPB_SelectedIndexChanged;
            // 
            // cbxFloppyBPB
            // 
            cbxFloppyBPB.AutoSize = true;
            cbxFloppyBPB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxFloppyBPB.Location = new System.Drawing.Point(8, 89);
            cbxFloppyBPB.Name = "cbxFloppyBPB";
            cbxFloppyBPB.Size = new System.Drawing.Size(219, 20);
            cbxFloppyBPB.TabIndex = 7;
            cbxFloppyBPB.Text = "Write a DOS BPB to the boot sector:";
            cbxFloppyBPB.UseVisualStyleBackColor = true;
            cbxFloppyBPB.CheckedChanged += cbxFloppyBPB_CheckedChanged;
            // 
            // txtFloppyLabel
            // 
            txtFloppyLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            txtFloppyLabel.Location = new System.Drawing.Point(90, 59);
            txtFloppyLabel.MaxLength = 11;
            txtFloppyLabel.Name = "txtFloppyLabel";
            txtFloppyLabel.Size = new System.Drawing.Size(275, 23);
            txtFloppyLabel.TabIndex = 6;
            txtFloppyLabel.TextChanged += txtFloppyLabel_TextChanged;
            // 
            // lblFloppyGeometry
            // 
            lblFloppyGeometry.AutoSize = true;
            lblFloppyGeometry.Location = new System.Drawing.Point(6, 10);
            lblFloppyGeometry.Name = "lblFloppyGeometry";
            lblFloppyGeometry.Size = new System.Drawing.Size(62, 15);
            lblFloppyGeometry.TabIndex = 0;
            lblFloppyGeometry.Text = "Geometry:";
            // 
            // lblFloppyLabel
            // 
            lblFloppyLabel.AutoSize = true;
            lblFloppyLabel.Location = new System.Drawing.Point(6, 62);
            lblFloppyLabel.Name = "lblFloppyLabel";
            lblFloppyLabel.Size = new System.Drawing.Size(78, 15);
            lblFloppyLabel.TabIndex = 3;
            lblFloppyLabel.Text = "Volume label:";
            // 
            // btnAdvanced
            // 
            btnAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAdvanced.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnAdvanced.Location = new System.Drawing.Point(282, 27);
            btnAdvanced.Name = "btnAdvanced";
            btnAdvanced.Size = new System.Drawing.Size(83, 26);
            btnAdvanced.TabIndex = 5;
            btnAdvanced.Text = "Advanced...";
            btnAdvanced.UseVisualStyleBackColor = true;
            btnAdvanced.Click += btnAdvanced_Click;
            // 
            // lstFloppyGeometries
            // 
            lstFloppyGeometries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstFloppyGeometries.FlatStyle = System.Windows.Forms.FlatStyle.System;
            lstFloppyGeometries.FormattingEnabled = true;
            lstFloppyGeometries.Location = new System.Drawing.Point(8, 28);
            lstFloppyGeometries.Name = "lstFloppyGeometries";
            lstFloppyGeometries.Size = new System.Drawing.Size(270, 23);
            lstFloppyGeometries.TabIndex = 4;
            lstFloppyGeometries.SelectedIndexChanged += lstFloppyGeometries_SelectedIndexChanged;
            // 
            // pnlHardDisk
            // 
            pnlHardDisk.Controls.Add(lstPartitionTable);
            pnlHardDisk.Controls.Add(cbxWritePartTable);
            pnlHardDisk.Controls.Add(rbnDifferencing);
            pnlHardDisk.Controls.Add(rbnDynamic);
            pnlHardDisk.Controls.Add(rbnFixed);
            pnlHardDisk.Controls.Add(lblDiskType);
            pnlHardDisk.Controls.Add(lblDiskSize);
            pnlHardDisk.Controls.Add(txtDiskSize);
            pnlHardDisk.Location = new System.Drawing.Point(2, 17);
            pnlHardDisk.Name = "pnlHardDisk";
            pnlHardDisk.Size = new System.Drawing.Size(371, 140);
            pnlHardDisk.TabIndex = 10;
            // 
            // lstPartitionTable
            // 
            lstPartitionTable.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lstPartitionTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstPartitionTable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            lstPartitionTable.FormattingEnabled = true;
            lstPartitionTable.Items.AddRange(new object[] { "Master Boot Record", "GUID Partition Table" });
            lstPartitionTable.Location = new System.Drawing.Point(216, 37);
            lstPartitionTable.Name = "lstPartitionTable";
            lstPartitionTable.Size = new System.Drawing.Size(150, 23);
            lstPartitionTable.TabIndex = 15;
            // 
            // cbxWritePartTable
            // 
            cbxWritePartTable.AutoSize = true;
            cbxWritePartTable.Checked = true;
            cbxWritePartTable.CheckState = System.Windows.Forms.CheckState.Checked;
            cbxWritePartTable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxWritePartTable.Location = new System.Drawing.Point(8, 38);
            cbxWritePartTable.Name = "cbxWritePartTable";
            cbxWritePartTable.Size = new System.Drawing.Size(207, 20);
            cbxWritePartTable.TabIndex = 14;
            cbxWritePartTable.Text = "Write a partition table to the disk:";
            cbxWritePartTable.UseVisualStyleBackColor = true;
            cbxWritePartTable.CheckedChanged += cbxWritePartTable_CheckedChanged;
            // 
            // rbnDifferencing
            // 
            rbnDifferencing.AutoSize = true;
            rbnDifferencing.Enabled = false;
            rbnDifferencing.FlatStyle = System.Windows.Forms.FlatStyle.System;
            rbnDifferencing.Location = new System.Drawing.Point(260, 89);
            rbnDifferencing.Name = "rbnDifferencing";
            rbnDifferencing.Size = new System.Drawing.Size(96, 20);
            rbnDifferencing.TabIndex = 13;
            rbnDifferencing.Text = "Differencing";
            rbnDifferencing.UseVisualStyleBackColor = true;
            // 
            // rbnDynamic
            // 
            rbnDynamic.AutoSize = true;
            rbnDynamic.Enabled = false;
            rbnDynamic.FlatStyle = System.Windows.Forms.FlatStyle.System;
            rbnDynamic.Location = new System.Drawing.Point(98, 89);
            rbnDynamic.Name = "rbnDynamic";
            rbnDynamic.Size = new System.Drawing.Size(155, 20);
            rbnDynamic.TabIndex = 12;
            rbnDynamic.Text = "Dynamically expanding";
            rbnDynamic.UseVisualStyleBackColor = true;
            // 
            // rbnFixed
            // 
            rbnFixed.AutoSize = true;
            rbnFixed.Checked = true;
            rbnFixed.FlatStyle = System.Windows.Forms.FlatStyle.System;
            rbnFixed.Location = new System.Drawing.Point(8, 89);
            rbnFixed.Name = "rbnFixed";
            rbnFixed.Size = new System.Drawing.Size(83, 20);
            rbnFixed.TabIndex = 10;
            rbnFixed.TabStop = true;
            rbnFixed.Text = "Fixed-size";
            rbnFixed.UseVisualStyleBackColor = true;
            // 
            // lblDiskType
            // 
            lblDiskType.AutoSize = true;
            lblDiskType.Location = new System.Drawing.Point(5, 70);
            lblDiskType.Name = "lblDiskType";
            lblDiskType.Size = new System.Drawing.Size(58, 15);
            lblDiskType.TabIndex = 10;
            lblDiskType.Text = "Disk type:";
            // 
            // lblDiskSize
            // 
            lblDiskSize.AutoSize = true;
            lblDiskSize.Location = new System.Drawing.Point(5, 10);
            lblDiskSize.Name = "lblDiskSize";
            lblDiskSize.Size = new System.Drawing.Size(86, 15);
            lblDiskSize.TabIndex = 10;
            lblDiskSize.Text = "Disk size (MiB):";
            // 
            // txtDiskSize
            // 
            txtDiskSize.Location = new System.Drawing.Point(101, 8);
            txtDiskSize.Margin = new System.Windows.Forms.Padding(2);
            txtDiskSize.Maximum = new decimal(new int[] { 2088960, 0, 0, 0 });
            txtDiskSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            txtDiskSize.Name = "txtDiskSize";
            txtDiskSize.Size = new System.Drawing.Size(120, 23);
            txtDiskSize.TabIndex = 11;
            txtDiskSize.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // dlgNewImage
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(399, 322);
            Controls.Add(gbxOptions);
            Controls.Add(gbxMediaType);
            Controls.Add(pnlBottom);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgNewImage";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "New image";
            Load += dlgNewImage_Load;
            pnlBottom.ResumeLayout(false);
            gbxMediaType.ResumeLayout(false);
            gbxMediaType.PerformLayout();
            gbxOptions.ResumeLayout(false);
            pnlFloppy.ResumeLayout(false);
            pnlFloppy.PerformLayout();
            pnlHardDisk.ResumeLayout(false);
            pnlHardDisk.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtDiskSize).EndInit();
            ResumeLayout(false);
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
