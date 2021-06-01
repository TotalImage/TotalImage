
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbxMediaType = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.rbnFloppyDisk = new System.Windows.Forms.RadioButton();
            this.rbnHardDisk = new System.Windows.Forms.RadioButton();
            this.gbxOptions = new System.Windows.Forms.GroupBox();
            this.lblFloppyGeometry = new System.Windows.Forms.Label();
            this.lstFloppyGeometries = new System.Windows.Forms.ComboBox();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.lblFloppyLabel = new System.Windows.Forms.Label();
            this.txtFloppyLabel = new System.Windows.Forms.TextBox();
            this.lstFloppyBPB = new System.Windows.Forms.ComboBox();
            this.cbxFloppyBPB = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.gbxMediaType.SuspendLayout();
            this.gbxOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 251);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(399, 50);
            this.panel1.TabIndex = 0;
            // 
            // gbxMediaType
            // 
            this.gbxMediaType.Controls.Add(this.rbnHardDisk);
            this.gbxMediaType.Controls.Add(this.rbnFloppyDisk);
            this.gbxMediaType.Location = new System.Drawing.Point(12, 12);
            this.gbxMediaType.Name = "gbxMediaType";
            this.gbxMediaType.Size = new System.Drawing.Size(375, 54);
            this.gbxMediaType.TabIndex = 1;
            this.gbxMediaType.TabStop = false;
            this.gbxMediaType.Text = "Media type";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(305, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(219, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // rbnFloppyDisk
            // 
            this.rbnFloppyDisk.AutoSize = true;
            this.rbnFloppyDisk.Checked = true;
            this.rbnFloppyDisk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnFloppyDisk.Location = new System.Drawing.Point(10, 22);
            this.rbnFloppyDisk.Name = "rbnFloppyDisk";
            this.rbnFloppyDisk.Size = new System.Drawing.Size(91, 20);
            this.rbnFloppyDisk.TabIndex = 0;
            this.rbnFloppyDisk.TabStop = true;
            this.rbnFloppyDisk.Text = "Floppy disk";
            this.rbnFloppyDisk.UseVisualStyleBackColor = true;
            // 
            // rbnHardDisk
            // 
            this.rbnHardDisk.AutoSize = true;
            this.rbnHardDisk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnHardDisk.Location = new System.Drawing.Point(135, 22);
            this.rbnHardDisk.Name = "rbnHardDisk";
            this.rbnHardDisk.Size = new System.Drawing.Size(81, 20);
            this.rbnHardDisk.TabIndex = 1;
            this.rbnHardDisk.Text = "Hard disk";
            this.rbnHardDisk.UseVisualStyleBackColor = true;
            // 
            // gbxOptions
            // 
            this.gbxOptions.Controls.Add(this.lstFloppyBPB);
            this.gbxOptions.Controls.Add(this.cbxFloppyBPB);
            this.gbxOptions.Controls.Add(this.txtFloppyLabel);
            this.gbxOptions.Controls.Add(this.lblFloppyLabel);
            this.gbxOptions.Controls.Add(this.btnAdvanced);
            this.gbxOptions.Controls.Add(this.lstFloppyGeometries);
            this.gbxOptions.Controls.Add(this.lblFloppyGeometry);
            this.gbxOptions.Location = new System.Drawing.Point(12, 72);
            this.gbxOptions.Name = "gbxOptions";
            this.gbxOptions.Size = new System.Drawing.Size(375, 166);
            this.gbxOptions.TabIndex = 2;
            this.gbxOptions.TabStop = false;
            this.gbxOptions.Text = "Options";
            // 
            // lblFloppyGeometry
            // 
            this.lblFloppyGeometry.AutoSize = true;
            this.lblFloppyGeometry.Location = new System.Drawing.Point(8, 22);
            this.lblFloppyGeometry.Name = "lblFloppyGeometry";
            this.lblFloppyGeometry.Size = new System.Drawing.Size(62, 15);
            this.lblFloppyGeometry.TabIndex = 0;
            this.lblFloppyGeometry.Text = "Geometry:";
            // 
            // lstFloppyGeometries
            // 
            this.lstFloppyGeometries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstFloppyGeometries.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstFloppyGeometries.FormattingEnabled = true;
            this.lstFloppyGeometries.Location = new System.Drawing.Point(10, 40);
            this.lstFloppyGeometries.Name = "lstFloppyGeometries";
            this.lstFloppyGeometries.Size = new System.Drawing.Size(268, 23);
            this.lstFloppyGeometries.TabIndex = 2;
            this.lstFloppyGeometries.SelectedIndexChanged += new System.EventHandler(this.lstFloppyGeometries_SelectedIndexChanged);
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdvanced.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdvanced.Location = new System.Drawing.Point(286, 38);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(83, 26);
            this.btnAdvanced.TabIndex = 3;
            this.btnAdvanced.Text = "Advanced...";
            this.btnAdvanced.UseVisualStyleBackColor = true;
            // 
            // lblFloppyLabel
            // 
            this.lblFloppyLabel.AutoSize = true;
            this.lblFloppyLabel.Location = new System.Drawing.Point(8, 75);
            this.lblFloppyLabel.Name = "lblFloppyLabel";
            this.lblFloppyLabel.Size = new System.Drawing.Size(78, 15);
            this.lblFloppyLabel.TabIndex = 3;
            this.lblFloppyLabel.Text = "Volume label:";
            // 
            // txtFloppyLabel
            // 
            this.txtFloppyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFloppyLabel.Location = new System.Drawing.Point(92, 71);
            this.txtFloppyLabel.MaxLength = 11;
            this.txtFloppyLabel.Name = "txtFloppyLabel";
            this.txtFloppyLabel.Size = new System.Drawing.Size(277, 23);
            this.txtFloppyLabel.TabIndex = 4;
            this.txtFloppyLabel.TextChanged += new System.EventHandler(this.txtFloppyLabel_TextChanged);
            // 
            // lstFloppyBPB
            // 
            this.lstFloppyBPB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFloppyBPB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstFloppyBPB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstFloppyBPB.FormattingEnabled = true;
            this.lstFloppyBPB.Items.AddRange(new object[] {
            "DOS 2.0",
            "DOS 3.31",
            "DOS 3.4",
            "DOS 4.0+"});
            this.lstFloppyBPB.Location = new System.Drawing.Point(226, 102);
            this.lstFloppyBPB.Name = "lstFloppyBPB";
            this.lstFloppyBPB.Size = new System.Drawing.Size(143, 23);
            this.lstFloppyBPB.TabIndex = 6;
            this.lstFloppyBPB.SelectedIndexChanged += new System.EventHandler(this.lstFloppyBPB_SelectedIndexChanged);
            // 
            // cbxFloppyBPB
            // 
            this.cbxFloppyBPB.AutoSize = true;
            this.cbxFloppyBPB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxFloppyBPB.Location = new System.Drawing.Point(10, 104);
            this.cbxFloppyBPB.Name = "cbxFloppyBPB";
            this.cbxFloppyBPB.Size = new System.Drawing.Size(219, 20);
            this.cbxFloppyBPB.TabIndex = 5;
            this.cbxFloppyBPB.Text = "Write a DOS BPB to the boot sector:";
            this.cbxFloppyBPB.UseVisualStyleBackColor = true;
            this.cbxFloppyBPB.CheckedChanged += new System.EventHandler(this.cbxFloppyBPB_CheckedChanged);
            // 
            // dlgNewImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(399, 301);
            this.Controls.Add(this.gbxOptions);
            this.Controls.Add(this.gbxMediaType);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgNewImage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New image";
            this.Load += new System.EventHandler(this.dlgNewImage_Load);
            this.panel1.ResumeLayout(false);
            this.gbxMediaType.ResumeLayout(false);
            this.gbxMediaType.PerformLayout();
            this.gbxOptions.ResumeLayout(false);
            this.gbxOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbxMediaType;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton rbnFloppyDisk;
        private System.Windows.Forms.RadioButton rbnHardDisk;
        private System.Windows.Forms.GroupBox gbxOptions;
        private System.Windows.Forms.Label lblFloppyGeometry;
        private System.Windows.Forms.ComboBox lstFloppyGeometries;
        private System.Windows.Forms.Button btnAdvanced;
        private System.Windows.Forms.Label lblFloppyLabel;
        private System.Windows.Forms.TextBox txtFloppyLabel;
        private System.Windows.Forms.ComboBox lstFloppyBPB;
        private System.Windows.Forms.CheckBox cbxFloppyBPB;
    }
}