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
            btnOK = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            pnlBottom = new System.Windows.Forms.Panel();
            btnLoad = new System.Windows.Forms.Button();
            btnSave = new System.Windows.Forms.Button();
            lblPreset = new System.Windows.Forms.Label();
            lstPresets = new System.Windows.Forms.ComboBox();
            btnApply = new System.Windows.Forms.Button();
            txtBootSector = new System.Windows.Forms.RichTextBox();
            rbnMBR = new System.Windows.Forms.RadioButton();
            lblShow = new System.Windows.Forms.Label();
            rbnVBR = new System.Windows.Forms.RadioButton();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(388, 12);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(474, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(80, 26);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 388);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(566, 50);
            pnlBottom.TabIndex = 10;
            // 
            // btnLoad
            // 
            btnLoad.Enabled = false;
            btnLoad.Location = new System.Drawing.Point(10, 34);
            btnLoad.Margin = new System.Windows.Forms.Padding(2);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new System.Drawing.Size(80, 26);
            btnLoad.TabIndex = 11;
            btnLoad.Text = "Load...";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(94, 34);
            btnSave.Margin = new System.Windows.Forms.Padding(2);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(80, 26);
            btnSave.TabIndex = 12;
            btnSave.Text = "Save...";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // lblPreset
            // 
            lblPreset.AutoSize = true;
            lblPreset.Enabled = false;
            lblPreset.Location = new System.Drawing.Point(179, 39);
            lblPreset.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPreset.Name = "lblPreset";
            lblPreset.Size = new System.Drawing.Size(47, 15);
            lblPreset.TabIndex = 13;
            lblPreset.Text = "Presets:";
            // 
            // lstPresets
            // 
            lstPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstPresets.Enabled = false;
            lstPresets.FormattingEnabled = true;
            lstPresets.Location = new System.Drawing.Point(230, 36);
            lstPresets.Margin = new System.Windows.Forms.Padding(2);
            lstPresets.Name = "lstPresets";
            lstPresets.Size = new System.Drawing.Size(242, 23);
            lstPresets.TabIndex = 14;
            // 
            // btnApply
            // 
            btnApply.Enabled = false;
            btnApply.Location = new System.Drawing.Point(477, 34);
            btnApply.Margin = new System.Windows.Forms.Padding(2);
            btnApply.Name = "btnApply";
            btnApply.Size = new System.Drawing.Size(80, 26);
            btnApply.TabIndex = 15;
            btnApply.Text = "Apply";
            btnApply.UseVisualStyleBackColor = true;
            // 
            // txtBootSector
            // 
            txtBootSector.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtBootSector.Font = new System.Drawing.Font("Consolas", 9.75F);
            txtBootSector.Location = new System.Drawing.Point(10, 75);
            txtBootSector.Margin = new System.Windows.Forms.Padding(2);
            txtBootSector.Name = "txtBootSector";
            txtBootSector.ReadOnly = true;
            txtBootSector.Size = new System.Drawing.Size(545, 300);
            txtBootSector.TabIndex = 16;
            txtBootSector.Text = "";
            // 
            // rbnMBR
            // 
            rbnMBR.AutoSize = true;
            rbnMBR.Location = new System.Drawing.Point(130, 8);
            rbnMBR.Margin = new System.Windows.Forms.Padding(2);
            rbnMBR.Name = "rbnMBR";
            rbnMBR.Size = new System.Drawing.Size(83, 19);
            rbnMBR.TabIndex = 17;
            rbnMBR.TabStop = true;
            rbnMBR.Text = "Disk (MBR)";
            rbnMBR.CheckedChanged += rbnMBR_CheckedChanged;
            // 
            // lblShow
            // 
            lblShow.AutoSize = true;
            lblShow.Location = new System.Drawing.Point(10, 10);
            lblShow.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblShow.Name = "lblShow";
            lblShow.Size = new System.Drawing.Size(116, 15);
            lblShow.TabIndex = 18;
            lblShow.Text = "Show boot sector of:";
            // 
            // rbnVBR
            // 
            rbnVBR.AutoSize = true;
            rbnVBR.Location = new System.Drawing.Point(230, 8);
            rbnVBR.Margin = new System.Windows.Forms.Padding(2);
            rbnVBR.Name = "rbnVBR";
            rbnVBR.Size = new System.Drawing.Size(149, 19);
            rbnVBR.TabIndex = 19;
            rbnVBR.TabStop = true;
            rbnVBR.Text = "Selected partition (VBR)";
            rbnVBR.CheckedChanged += rbnVBR_CheckedChanged;
            // 
            // dlgBootSector
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(566, 438);
            Controls.Add(rbnVBR);
            Controls.Add(lblShow);
            Controls.Add(rbnMBR);
            Controls.Add(txtBootSector);
            Controls.Add(btnApply);
            Controls.Add(lstPresets);
            Controls.Add(lblPreset);
            Controls.Add(btnSave);
            Controls.Add(btnLoad);
            Controls.Add(pnlBottom);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgBootSector";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Boot sector properties";
            Load += dlgBootSector_Load;
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
