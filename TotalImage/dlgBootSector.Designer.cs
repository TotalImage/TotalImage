namespace TotalImage
{
    partial class dlgBootSector
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblPreset = new System.Windows.Forms.Label();
            this.lstPresets = new System.Windows.Forms.ComboBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.txtBootSector = new System.Windows.Forms.RichTextBox();
            this.rbnMBR = new System.Windows.Forms.RadioButton();
            this.lblShow = new System.Windows.Forms.Label();
            this.rbnVBR = new System.Windows.Forms.RadioButton();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(388, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(474, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 388);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(566, 50);
            this.pnlBottom.TabIndex = 10;
            // 
            // btnLoad
            // 
            this.btnLoad.Enabled = false;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLoad.Location = new System.Drawing.Point(10, 34);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(80, 26);
            this.btnLoad.TabIndex = 11;
            this.btnLoad.Text = "Load...";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSave.Location = new System.Drawing.Point(94, 34);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 26);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save...";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblPreset
            // 
            this.lblPreset.AutoSize = true;
            this.lblPreset.Enabled = false;
            this.lblPreset.Location = new System.Drawing.Point(179, 39);
            this.lblPreset.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPreset.Name = "lblPreset";
            this.lblPreset.Size = new System.Drawing.Size(47, 15);
            this.lblPreset.TabIndex = 13;
            this.lblPreset.Text = "Presets:";
            // 
            // lstPresets
            // 
            this.lstPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstPresets.Enabled = false;
            this.lstPresets.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstPresets.FormattingEnabled = true;
            this.lstPresets.Location = new System.Drawing.Point(230, 36);
            this.lstPresets.Margin = new System.Windows.Forms.Padding(2);
            this.lstPresets.Name = "lstPresets";
            this.lstPresets.Size = new System.Drawing.Size(242, 23);
            this.lstPresets.TabIndex = 14;
            // 
            // btnApply
            // 
            this.btnApply.Enabled = false;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnApply.Location = new System.Drawing.Point(477, 34);
            this.btnApply.Margin = new System.Windows.Forms.Padding(2);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(80, 26);
            this.btnApply.TabIndex = 15;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // txtBootSector
            // 
            this.txtBootSector.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtBootSector.Location = new System.Drawing.Point(10, 75);
            this.txtBootSector.Margin = new System.Windows.Forms.Padding(2);
            this.txtBootSector.Name = "txtBootSector";
            this.txtBootSector.ReadOnly = true;
            this.txtBootSector.Size = new System.Drawing.Size(545, 300);
            this.txtBootSector.TabIndex = 16;
            this.txtBootSector.Text = "";
            // 
            // rbnMBR
            // 
            this.rbnMBR.AutoSize = true;
            this.rbnMBR.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnMBR.Location = new System.Drawing.Point(130, 8);
            this.rbnMBR.Margin = new System.Windows.Forms.Padding(2);
            this.rbnMBR.Name = "rbnMBR";
            this.rbnMBR.Size = new System.Drawing.Size(89, 20);
            this.rbnMBR.TabIndex = 17;
            this.rbnMBR.TabStop = true;
            this.rbnMBR.Text = "Disk (MBR)";
            this.rbnMBR.UseVisualStyleBackColor = true;
            // 
            // lblShow
            // 
            this.lblShow.AutoSize = true;
            this.lblShow.Location = new System.Drawing.Point(10, 10);
            this.lblShow.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblShow.Name = "lblShow";
            this.lblShow.Size = new System.Drawing.Size(116, 15);
            this.lblShow.TabIndex = 18;
            this.lblShow.Text = "Show boot sector of:";
            // 
            // rbnVBR
            // 
            this.rbnVBR.AutoSize = true;
            this.rbnVBR.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnVBR.Location = new System.Drawing.Point(230, 8);
            this.rbnVBR.Margin = new System.Windows.Forms.Padding(2);
            this.rbnVBR.Name = "rbnVBR";
            this.rbnVBR.Size = new System.Drawing.Size(155, 20);
            this.rbnVBR.TabIndex = 19;
            this.rbnVBR.TabStop = true;
            this.rbnVBR.Text = "Selected partition (VBR)";
            this.rbnVBR.UseVisualStyleBackColor = true;
            // 
            // dlgBootSector
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(566, 438);
            this.Controls.Add(this.rbnVBR);
            this.Controls.Add(this.lblShow);
            this.Controls.Add(this.rbnMBR);
            this.Controls.Add(this.txtBootSector);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lstPresets);
            this.Controls.Add(this.lblPreset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgBootSector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Boot sector properties";
            this.Load += new System.EventHandler(this.dlgBootSector_Load);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblPreset;
        private System.Windows.Forms.ComboBox lstPresets;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.RichTextBox txtBootSector;
        private System.Windows.Forms.RadioButton rbnMBR;
        private System.Windows.Forms.Label lblShow;
        private System.Windows.Forms.RadioButton rbnVBR;
    }
}
