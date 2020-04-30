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
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(96, 153);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
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
            this.btnCancel.Location = new System.Drawing.Point(182, 153);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtRootDirLabel
            // 
            this.txtRootDirLabel.Location = new System.Drawing.Point(15, 57);
            this.txtRootDirLabel.MaxLength = 11;
            this.txtRootDirLabel.Name = "txtRootDirLabel";
            this.txtRootDirLabel.Size = new System.Drawing.Size(245, 23);
            this.txtRootDirLabel.TabIndex = 0;
            this.txtRootDirLabel.TextChanged += new System.EventHandler(this.txtRootDirLabel_TextChanged);
            // 
            // lblRootDirLabel
            // 
            this.lblRootDirLabel.AutoSize = true;
            this.lblRootDirLabel.Location = new System.Drawing.Point(12, 9);
            this.lblRootDirLabel.Name = "lblRootDirLabel";
            this.lblRootDirLabel.Size = new System.Drawing.Size(253, 45);
            this.lblRootDirLabel.TabIndex = 4;
            this.lblRootDirLabel.Text = "Enter a new volume label (up to 11 characters).\r\n\r\nRoot directory volume label:";
            // 
            // txtBPBLabel
            // 
            this.txtBPBLabel.Location = new System.Drawing.Point(15, 112);
            this.txtBPBLabel.MaxLength = 11;
            this.txtBPBLabel.Name = "txtBPBLabel";
            this.txtBPBLabel.ReadOnly = true;
            this.txtBPBLabel.Size = new System.Drawing.Size(245, 23);
            this.txtBPBLabel.TabIndex = 2;
            // 
            // cbxBPBLabel
            // 
            this.cbxBPBLabel.AutoSize = true;
            this.cbxBPBLabel.Checked = true;
            this.cbxBPBLabel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxBPBLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxBPBLabel.Location = new System.Drawing.Point(15, 86);
            this.cbxBPBLabel.Name = "cbxBPBLabel";
            this.cbxBPBLabel.Size = new System.Drawing.Size(225, 20);
            this.cbxBPBLabel.TabIndex = 1;
            this.cbxBPBLabel.Text = "Set the same label in the BPB as well:";
            this.cbxBPBLabel.UseVisualStyleBackColor = true;
            this.cbxBPBLabel.CheckedChanged += new System.EventHandler(this.cbxBPBLabel_CheckedChanged);
            // 
            // dlgChangeVolLabel
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(274, 191);
            this.Controls.Add(this.cbxBPBLabel);
            this.Controls.Add(this.txtBPBLabel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtRootDirLabel);
            this.Controls.Add(this.lblRootDirLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgChangeVolLabel";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change volume label";
            this.Load += new System.EventHandler(this.dlgChangeVolLabel_Load);
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
    }
}