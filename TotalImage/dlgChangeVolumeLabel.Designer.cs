namespace TotalImage
{
    partial class dlgChangeVolumeLabel
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
            txtNewLabel = new System.Windows.Forms.TextBox();
            lblNewLabel = new System.Windows.Forms.Label();
            pnlBottom = new System.Windows.Forms.Panel();
            gbxFatOptions = new System.Windows.Forms.GroupBox();
            cbxSync = new System.Windows.Forms.CheckBox();
            cbxBPBLabel = new System.Windows.Forms.CheckBox();
            txtBPBLabel = new System.Windows.Forms.TextBox();
            pnlBottom.SuspendLayout();
            gbxFatOptions.SuspendLayout();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnOK.Location = new System.Drawing.Point(96, 12);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 3;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnCancel.Location = new System.Drawing.Point(182, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(80, 26);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtNewLabel
            // 
            txtNewLabel.Location = new System.Drawing.Point(12, 27);
            txtNewLabel.MaxLength = 11;
            txtNewLabel.Name = "txtNewLabel";
            txtNewLabel.Size = new System.Drawing.Size(250, 23);
            txtNewLabel.TabIndex = 0;
            txtNewLabel.TextChanged += txtRootDirLabel_TextChanged;
            // 
            // lblNewLabel
            // 
            lblNewLabel.AutoSize = true;
            lblNewLabel.Location = new System.Drawing.Point(9, 9);
            lblNewLabel.Name = "lblNewLabel";
            lblNewLabel.Size = new System.Drawing.Size(142, 15);
            lblNewLabel.TabIndex = 4;
            lblNewLabel.Text = "Enter a new volume label:";
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 172);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(274, 50);
            pnlBottom.TabIndex = 5;
            // 
            // gbxFatOptions
            // 
            gbxFatOptions.Controls.Add(cbxSync);
            gbxFatOptions.Controls.Add(cbxBPBLabel);
            gbxFatOptions.Controls.Add(txtBPBLabel);
            gbxFatOptions.Location = new System.Drawing.Point(12, 56);
            gbxFatOptions.Name = "gbxFatOptions";
            gbxFatOptions.Size = new System.Drawing.Size(250, 110);
            gbxFatOptions.TabIndex = 7;
            gbxFatOptions.TabStop = false;
            gbxFatOptions.Text = "FAT options";
            // 
            // cbxSync
            // 
            cbxSync.AutoSize = true;
            cbxSync.Checked = true;
            cbxSync.CheckState = System.Windows.Forms.CheckState.Checked;
            cbxSync.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxSync.Location = new System.Drawing.Point(11, 48);
            cbxSync.Name = "cbxSync";
            cbxSync.Size = new System.Drawing.Size(241, 20);
            cbxSync.TabIndex = 9;
            cbxSync.Text = "Write the same label as in root directory";
            cbxSync.UseVisualStyleBackColor = true;
            cbxSync.CheckedChanged += cbxSync_CheckedChanged;
            // 
            // cbxBPBLabel
            // 
            cbxBPBLabel.AutoSize = true;
            cbxBPBLabel.Checked = true;
            cbxBPBLabel.CheckState = System.Windows.Forms.CheckState.Checked;
            cbxBPBLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxBPBLabel.Location = new System.Drawing.Point(11, 22);
            cbxBPBLabel.Name = "cbxBPBLabel";
            cbxBPBLabel.Size = new System.Drawing.Size(233, 20);
            cbxBPBLabel.TabIndex = 7;
            cbxBPBLabel.Text = "Also write the volume label to the BPB";
            cbxBPBLabel.UseVisualStyleBackColor = true;
            cbxBPBLabel.CheckedChanged += cbxBPBLabel_CheckedChanged;
            // 
            // txtBPBLabel
            // 
            txtBPBLabel.Location = new System.Drawing.Point(11, 74);
            txtBPBLabel.MaxLength = 11;
            txtBPBLabel.Name = "txtBPBLabel";
            txtBPBLabel.ReadOnly = true;
            txtBPBLabel.Size = new System.Drawing.Size(233, 23);
            txtBPBLabel.TabIndex = 8;
            // 
            // dlgChangeVolumeLabel
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(274, 222);
            Controls.Add(pnlBottom);
            Controls.Add(txtNewLabel);
            Controls.Add(lblNewLabel);
            Controls.Add(gbxFatOptions);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgChangeVolumeLabel";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Change volume label";
            Load += dlgChangeVolLabel_Load;
            pnlBottom.ResumeLayout(false);
            gbxFatOptions.ResumeLayout(false);
            gbxFatOptions.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtNewLabel;
        private System.Windows.Forms.Label lblNewLabel;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.GroupBox gbxFatOptions;
        private System.Windows.Forms.CheckBox cbxSync;
        private System.Windows.Forms.CheckBox cbxBPBLabel;
        private System.Windows.Forms.TextBox txtBPBLabel;
    }
}
