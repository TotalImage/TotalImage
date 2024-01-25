namespace TotalImage
{
    partial class dlgChangeVolLabel
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
            txtRootDirLabel = new System.Windows.Forms.TextBox();
            lblRootDirLabel = new System.Windows.Forms.Label();
            txtBPBLabel = new System.Windows.Forms.TextBox();
            cbxBPBLabel = new System.Windows.Forms.CheckBox();
            pnlBottom = new System.Windows.Forms.Panel();
            cbxSync = new System.Windows.Forms.CheckBox();
            pnlBottom.SuspendLayout();
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
            // txtRootDirLabel
            // 
            txtRootDirLabel.Location = new System.Drawing.Point(12, 58);
            txtRootDirLabel.MaxLength = 11;
            txtRootDirLabel.Name = "txtRootDirLabel";
            txtRootDirLabel.Size = new System.Drawing.Size(250, 23);
            txtRootDirLabel.TabIndex = 0;
            txtRootDirLabel.TextChanged += txtRootDirLabel_TextChanged;
            // 
            // lblRootDirLabel
            // 
            lblRootDirLabel.AutoSize = true;
            lblRootDirLabel.Location = new System.Drawing.Point(9, 9);
            lblRootDirLabel.Name = "lblRootDirLabel";
            lblRootDirLabel.Size = new System.Drawing.Size(253, 45);
            lblRootDirLabel.TabIndex = 4;
            lblRootDirLabel.Text = "Enter a new volume label (up to 11 characters).\r\n\r\nRoot directory volume label:";
            // 
            // txtBPBLabel
            // 
            txtBPBLabel.Location = new System.Drawing.Point(12, 139);
            txtBPBLabel.MaxLength = 11;
            txtBPBLabel.Name = "txtBPBLabel";
            txtBPBLabel.ReadOnly = true;
            txtBPBLabel.Size = new System.Drawing.Size(250, 23);
            txtBPBLabel.TabIndex = 2;
            // 
            // cbxBPBLabel
            // 
            cbxBPBLabel.AutoSize = true;
            cbxBPBLabel.Checked = true;
            cbxBPBLabel.CheckState = System.Windows.Forms.CheckState.Checked;
            cbxBPBLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxBPBLabel.Location = new System.Drawing.Point(12, 87);
            cbxBPBLabel.Name = "cbxBPBLabel";
            cbxBPBLabel.Size = new System.Drawing.Size(233, 20);
            cbxBPBLabel.TabIndex = 1;
            cbxBPBLabel.Text = "Also write the volume label to the BPB";
            cbxBPBLabel.UseVisualStyleBackColor = true;
            cbxBPBLabel.CheckedChanged += cbxBPBLabel_CheckedChanged;
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
            // cbxSync
            // 
            cbxSync.AutoSize = true;
            cbxSync.Checked = true;
            cbxSync.CheckState = System.Windows.Forms.CheckState.Checked;
            cbxSync.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxSync.Location = new System.Drawing.Point(12, 113);
            cbxSync.Name = "cbxSync";
            cbxSync.Size = new System.Drawing.Size(241, 20);
            cbxSync.TabIndex = 6;
            cbxSync.Text = "Write the same label as in root directory";
            cbxSync.UseVisualStyleBackColor = true;
            cbxSync.CheckedChanged += cbxSync_CheckedChanged;
            // 
            // dlgChangeVolLabel
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(274, 222);
            Controls.Add(cbxSync);
            Controls.Add(pnlBottom);
            Controls.Add(cbxBPBLabel);
            Controls.Add(txtBPBLabel);
            Controls.Add(txtRootDirLabel);
            Controls.Add(lblRootDirLabel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgChangeVolLabel";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Change volume label";
            Load += dlgChangeVolLabel_Load;
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtRootDirLabel;
        private System.Windows.Forms.Label lblRootDirLabel;
        private System.Windows.Forms.TextBox txtBPBLabel;
        private System.Windows.Forms.CheckBox cbxBPBLabel;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.CheckBox cbxSync;
    }
}
