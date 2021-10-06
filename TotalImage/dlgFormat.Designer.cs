namespace TotalImage
{
    partial class dlgFormat
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblFileSystem = new System.Windows.Forms.Label();
            this.lstFileSystem = new System.Windows.Forms.ComboBox();
            this.lstClusterSize = new System.Windows.Forms.ComboBox();
            this.lblClusterSize = new System.Windows.Forms.Label();
            this.lblVolumeLabel = new System.Windows.Forms.Label();
            this.txtVolumeLabel = new System.Windows.Forms.TextBox();
            this.cbxQuickFormat = new System.Windows.Forms.CheckBox();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 264);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(355, 62);
            this.pnlBottom.TabIndex = 99;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(240, 15);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 32);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(132, 15);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 32);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblFileSystem
            // 
            this.lblFileSystem.AutoSize = true;
            this.lblFileSystem.Location = new System.Drawing.Point(15, 11);
            this.lblFileSystem.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFileSystem.Name = "lblFileSystem";
            this.lblFileSystem.Size = new System.Drawing.Size(84, 20);
            this.lblFileSystem.TabIndex = 100;
            this.lblFileSystem.Text = "File system:";
            // 
            // lstFileSystem
            // 
            this.lstFileSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstFileSystem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstFileSystem.FormattingEnabled = true;
            this.lstFileSystem.Location = new System.Drawing.Point(19, 34);
            this.lstFileSystem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstFileSystem.Name = "lstFileSystem";
            this.lstFileSystem.Size = new System.Drawing.Size(320, 28);
            this.lstFileSystem.TabIndex = 1;
            // 
            // lstClusterSize
            // 
            this.lstClusterSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstClusterSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstClusterSize.FormattingEnabled = true;
            this.lstClusterSize.Location = new System.Drawing.Point(19, 100);
            this.lstClusterSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstClusterSize.Name = "lstClusterSize";
            this.lstClusterSize.Size = new System.Drawing.Size(320, 28);
            this.lstClusterSize.TabIndex = 2;
            // 
            // lblClusterSize
            // 
            this.lblClusterSize.AutoSize = true;
            this.lblClusterSize.Location = new System.Drawing.Point(15, 78);
            this.lblClusterSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClusterSize.Name = "lblClusterSize";
            this.lblClusterSize.Size = new System.Drawing.Size(86, 20);
            this.lblClusterSize.TabIndex = 102;
            this.lblClusterSize.Text = "Cluster size:";
            // 
            // lblVolumeLabel
            // 
            this.lblVolumeLabel.AutoSize = true;
            this.lblVolumeLabel.Location = new System.Drawing.Point(15, 144);
            this.lblVolumeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVolumeLabel.Name = "lblVolumeLabel";
            this.lblVolumeLabel.Size = new System.Drawing.Size(99, 20);
            this.lblVolumeLabel.TabIndex = 104;
            this.lblVolumeLabel.Text = "Volume label:";
            // 
            // txtVolumeLabel
            // 
            this.txtVolumeLabel.Location = new System.Drawing.Point(19, 166);
            this.txtVolumeLabel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtVolumeLabel.MaxLength = 11;
            this.txtVolumeLabel.Name = "txtVolumeLabel";
            this.txtVolumeLabel.Size = new System.Drawing.Size(320, 27);
            this.txtVolumeLabel.TabIndex = 3;
            // 
            // cbxQuickFormat
            // 
            this.cbxQuickFormat.AutoSize = true;
            this.cbxQuickFormat.Checked = true;
            this.cbxQuickFormat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxQuickFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxQuickFormat.Location = new System.Drawing.Point(19, 214);
            this.cbxQuickFormat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxQuickFormat.Name = "cbxQuickFormat";
            this.cbxQuickFormat.Size = new System.Drawing.Size(126, 25);
            this.cbxQuickFormat.TabIndex = 4;
            this.cbxQuickFormat.Text = "Quick format";
            this.cbxQuickFormat.UseVisualStyleBackColor = true;
            // 
            // dlgFormat
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(355, 326);
            this.Controls.Add(this.cbxQuickFormat);
            this.Controls.Add(this.txtVolumeLabel);
            this.Controls.Add(this.lblVolumeLabel);
            this.Controls.Add(this.lstClusterSize);
            this.Controls.Add(this.lblClusterSize);
            this.Controls.Add(this.lstFileSystem);
            this.Controls.Add(this.lblFileSystem);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgFormat";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Format";
            this.Load += new System.EventHandler(this.dlgFormat_Load);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblFileSystem;
        private System.Windows.Forms.ComboBox lstFileSystem;
        private System.Windows.Forms.ComboBox lstClusterSize;
        private System.Windows.Forms.Label lblClusterSize;
        private System.Windows.Forms.Label lblVolumeLabel;
        private System.Windows.Forms.TextBox txtVolumeLabel;
        private System.Windows.Forms.CheckBox cbxQuickFormat;
    }
}