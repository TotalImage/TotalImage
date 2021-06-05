
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
            this.lblTest = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.gbxMediaType.SuspendLayout();
            this.gbxOptions.SuspendLayout();
            this.pnlFloppy.SuspendLayout();
            this.pnlHardDisk.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 251);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(399, 50);
            this.pnlBottom.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(219, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(305, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbxMediaType
            // 
            this.gbxMediaType.Controls.Add(this.rbnHardDisk);
            this.gbxMediaType.Controls.Add(this.rbnFloppyDisk);
            this.gbxMediaType.Location = new System.Drawing.Point(12, 12);
            this.gbxMediaType.Name = "gbxMediaType";
            this.gbxMediaType.Size = new System.Drawing.Size(375, 54);
            this.gbxMediaType.TabIndex = 0;
            this.gbxMediaType.TabStop = false;
            this.gbxMediaType.Text = "Media type";
            // 
            // rbnHardDisk
            // 
            this.rbnHardDisk.AutoSize = true;
            this.rbnHardDisk.Enabled = false;
            this.rbnHardDisk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnHardDisk.Location = new System.Drawing.Point(135, 22);
            this.rbnHardDisk.Name = "rbnHardDisk";
            this.rbnHardDisk.Size = new System.Drawing.Size(81, 20);
            this.rbnHardDisk.TabIndex = 2;
            this.rbnHardDisk.Text = "Hard disk";
            this.rbnHardDisk.UseVisualStyleBackColor = true;
            // 
            // rbnFloppyDisk
            // 
            this.rbnFloppyDisk.AutoSize = true;
            this.rbnFloppyDisk.Checked = true;
            this.rbnFloppyDisk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnFloppyDisk.Location = new System.Drawing.Point(10, 22);
            this.rbnFloppyDisk.Name = "rbnFloppyDisk";
            this.rbnFloppyDisk.Size = new System.Drawing.Size(91, 20);
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
            this.gbxOptions.Location = new System.Drawing.Point(12, 72);
            this.gbxOptions.Name = "gbxOptions";
            this.gbxOptions.Size = new System.Drawing.Size(375, 166);
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
            this.pnlFloppy.Location = new System.Drawing.Point(2, 17);
            this.pnlFloppy.Name = "pnlFloppy";
            this.pnlFloppy.Size = new System.Drawing.Size(371, 140);
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
            this.lstFloppyBPB.Location = new System.Drawing.Point(222, 88);
            this.lstFloppyBPB.Name = "lstFloppyBPB";
            this.lstFloppyBPB.Size = new System.Drawing.Size(143, 23);
            this.lstFloppyBPB.TabIndex = 8;
            this.lstFloppyBPB.SelectedIndexChanged += new System.EventHandler(this.lstFloppyBPB_SelectedIndexChanged);
            // 
            // cbxFloppyBPB
            // 
            this.cbxFloppyBPB.AutoSize = true;
            this.cbxFloppyBPB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxFloppyBPB.Location = new System.Drawing.Point(8, 89);
            this.cbxFloppyBPB.Name = "cbxFloppyBPB";
            this.cbxFloppyBPB.Size = new System.Drawing.Size(219, 20);
            this.cbxFloppyBPB.TabIndex = 7;
            this.cbxFloppyBPB.Text = "Write a DOS BPB to the boot sector:";
            this.cbxFloppyBPB.UseVisualStyleBackColor = true;
            this.cbxFloppyBPB.CheckedChanged += new System.EventHandler(this.cbxFloppyBPB_CheckedChanged);
            // 
            // txtFloppyLabel
            // 
            this.txtFloppyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFloppyLabel.Location = new System.Drawing.Point(90, 59);
            this.txtFloppyLabel.MaxLength = 11;
            this.txtFloppyLabel.Name = "txtFloppyLabel";
            this.txtFloppyLabel.Size = new System.Drawing.Size(275, 23);
            this.txtFloppyLabel.TabIndex = 6;
            this.txtFloppyLabel.TextChanged += new System.EventHandler(this.txtFloppyLabel_TextChanged);
            // 
            // lblFloppyGeometry
            // 
            this.lblFloppyGeometry.AutoSize = true;
            this.lblFloppyGeometry.Location = new System.Drawing.Point(6, 10);
            this.lblFloppyGeometry.Name = "lblFloppyGeometry";
            this.lblFloppyGeometry.Size = new System.Drawing.Size(62, 15);
            this.lblFloppyGeometry.TabIndex = 0;
            this.lblFloppyGeometry.Text = "Geometry:";
            // 
            // lblFloppyLabel
            // 
            this.lblFloppyLabel.AutoSize = true;
            this.lblFloppyLabel.Location = new System.Drawing.Point(6, 62);
            this.lblFloppyLabel.Name = "lblFloppyLabel";
            this.lblFloppyLabel.Size = new System.Drawing.Size(78, 15);
            this.lblFloppyLabel.TabIndex = 3;
            this.lblFloppyLabel.Text = "Volume label:";
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdvanced.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdvanced.Location = new System.Drawing.Point(282, 27);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(83, 26);
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
            this.lstFloppyGeometries.Location = new System.Drawing.Point(8, 28);
            this.lstFloppyGeometries.Name = "lstFloppyGeometries";
            this.lstFloppyGeometries.Size = new System.Drawing.Size(270, 23);
            this.lstFloppyGeometries.TabIndex = 4;
            this.lstFloppyGeometries.SelectedIndexChanged += new System.EventHandler(this.lstFloppyGeometries_SelectedIndexChanged);
            // 
            // pnlHardDisk
            // 
            this.pnlHardDisk.Controls.Add(this.lblTest);
            this.pnlHardDisk.Location = new System.Drawing.Point(2, 17);
            this.pnlHardDisk.Name = "pnlHardDisk";
            this.pnlHardDisk.Size = new System.Drawing.Size(371, 140);
            this.pnlHardDisk.TabIndex = 10;
            // 
            // lblTest
            // 
            this.lblTest.AutoSize = true;
            this.lblTest.Location = new System.Drawing.Point(64, 59);
            this.lblTest.Name = "lblTest";
            this.lblTest.Size = new System.Drawing.Size(243, 15);
            this.lblTest.TabIndex = 0;
            this.lblTest.Text = "This is a super secret hard disk options panel!";
            // 
            // dlgNewImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(399, 301);
            this.Controls.Add(this.gbxOptions);
            this.Controls.Add(this.gbxMediaType);
            this.Controls.Add(this.pnlBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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
        private System.Windows.Forms.Label lblTest;
        private System.Windows.Forms.ComboBox lstFloppyGeometries;
        internal System.Windows.Forms.CheckBox cbxFloppyBPB;
        internal System.Windows.Forms.ComboBox lstFloppyBPB;
    }
}