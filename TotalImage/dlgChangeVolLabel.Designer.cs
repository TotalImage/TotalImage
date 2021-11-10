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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtRootDirLabel = new System.Windows.Forms.TextBox();
            this.lblRootDirLabel = new System.Windows.Forms.Label();
            this.txtBPBLabel = new System.Windows.Forms.TextBox();
            this.cbxBPBLabel = new System.Windows.Forms.CheckBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.cbxSync = new System.Windows.Forms.CheckBox();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(120, 15);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 32);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(228, 15);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 32);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtRootDirLabel
            // 
            this.txtRootDirLabel.Location = new System.Drawing.Point(15, 72);
            this.txtRootDirLabel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRootDirLabel.MaxLength = 11;
            this.txtRootDirLabel.Name = "txtRootDirLabel";
            this.txtRootDirLabel.Size = new System.Drawing.Size(312, 27);
            this.txtRootDirLabel.TabIndex = 0;
            this.txtRootDirLabel.TextChanged += new System.EventHandler(this.txtRootDirLabel_TextChanged);
            // 
            // lblRootDirLabel
            // 
            this.lblRootDirLabel.AutoSize = true;
            this.lblRootDirLabel.Location = new System.Drawing.Point(11, 11);
            this.lblRootDirLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRootDirLabel.Name = "lblRootDirLabel";
            this.lblRootDirLabel.Size = new System.Drawing.Size(319, 60);
            this.lblRootDirLabel.TabIndex = 4;
            this.lblRootDirLabel.Text = "Enter a new volume label (up to 11 characters).\r\n\r\nRoot directory volume label:";
            // 
            // txtBPBLabel
            // 
            this.txtBPBLabel.Location = new System.Drawing.Point(15, 174);
            this.txtBPBLabel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBPBLabel.MaxLength = 11;
            this.txtBPBLabel.Name = "txtBPBLabel";
            this.txtBPBLabel.ReadOnly = true;
            this.txtBPBLabel.Size = new System.Drawing.Size(312, 27);
            this.txtBPBLabel.TabIndex = 2;
            // 
            // cbxBPBLabel
            // 
            this.cbxBPBLabel.AutoSize = true;
            this.cbxBPBLabel.Checked = true;
            this.cbxBPBLabel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxBPBLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxBPBLabel.Location = new System.Drawing.Point(15, 109);
            this.cbxBPBLabel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxBPBLabel.Name = "cbxBPBLabel";
            this.cbxBPBLabel.Size = new System.Drawing.Size(294, 25);
            this.cbxBPBLabel.TabIndex = 1;
            this.cbxBPBLabel.Text = "Also write the volume label to the BPB";
            this.cbxBPBLabel.UseVisualStyleBackColor = true;
            this.cbxBPBLabel.CheckedChanged += new System.EventHandler(this.cbxBPBLabel_CheckedChanged);
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 216);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(342, 62);
            this.pnlBottom.TabIndex = 5;
            // 
            // cbxSync
            // 
            this.cbxSync.AutoSize = true;
            this.cbxSync.Checked = true;
            this.cbxSync.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxSync.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxSync.Location = new System.Drawing.Point(15, 141);
            this.cbxSync.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxSync.Name = "cbxSync";
            this.cbxSync.Size = new System.Drawing.Size(306, 25);
            this.cbxSync.TabIndex = 6;
            this.cbxSync.Text = "Write the same label as in root directory";
            this.cbxSync.UseVisualStyleBackColor = true;
            this.cbxSync.CheckedChanged += new System.EventHandler(this.cbxSync_CheckedChanged);
            // 
            // dlgChangeVolLabel
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(342, 278);
            this.Controls.Add(this.cbxSync);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.cbxBPBLabel);
            this.Controls.Add(this.txtBPBLabel);
            this.Controls.Add(this.txtRootDirLabel);
            this.Controls.Add(this.lblRootDirLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgChangeVolLabel";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change volume label";
            this.Load += new System.EventHandler(this.dlgChangeVolLabel_Load);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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