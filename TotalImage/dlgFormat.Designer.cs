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
            pnlBottom = new System.Windows.Forms.Panel();
            btnCancel = new System.Windows.Forms.Button();
            btnOK = new System.Windows.Forms.Button();
            lblFileSystem = new System.Windows.Forms.Label();
            lstFileSystem = new System.Windows.Forms.ComboBox();
            lstClusterSize = new System.Windows.Forms.ComboBox();
            lblClusterSize = new System.Windows.Forms.Label();
            lblVolumeLabel = new System.Windows.Forms.Label();
            txtVolumeLabel = new System.Windows.Forms.TextBox();
            cbxQuickFormat = new System.Windows.Forms.CheckBox();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 211);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(284, 50);
            pnlBottom.TabIndex = 99;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(192, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(80, 26);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(106, 12);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 5;
            btnOK.Text = "OK";
            btnOK.Click += btnOK_Click;
            // 
            // lblFileSystem
            // 
            lblFileSystem.AutoSize = true;
            lblFileSystem.Location = new System.Drawing.Point(12, 9);
            lblFileSystem.Name = "lblFileSystem";
            lblFileSystem.Size = new System.Drawing.Size(68, 15);
            lblFileSystem.TabIndex = 100;
            lblFileSystem.Text = "File system:";
            // 
            // lstFileSystem
            // 
            lstFileSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstFileSystem.FormattingEnabled = true;
            lstFileSystem.Location = new System.Drawing.Point(15, 27);
            lstFileSystem.Name = "lstFileSystem";
            lstFileSystem.Size = new System.Drawing.Size(257, 23);
            lstFileSystem.TabIndex = 1;
            // 
            // lstClusterSize
            // 
            lstClusterSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            lstClusterSize.FormattingEnabled = true;
            lstClusterSize.Location = new System.Drawing.Point(15, 80);
            lstClusterSize.Name = "lstClusterSize";
            lstClusterSize.Size = new System.Drawing.Size(257, 23);
            lstClusterSize.TabIndex = 2;
            // 
            // lblClusterSize
            // 
            lblClusterSize.AutoSize = true;
            lblClusterSize.Location = new System.Drawing.Point(12, 62);
            lblClusterSize.Name = "lblClusterSize";
            lblClusterSize.Size = new System.Drawing.Size(69, 15);
            lblClusterSize.TabIndex = 102;
            lblClusterSize.Text = "Cluster size:";
            // 
            // lblVolumeLabel
            // 
            lblVolumeLabel.AutoSize = true;
            lblVolumeLabel.Location = new System.Drawing.Point(12, 115);
            lblVolumeLabel.Name = "lblVolumeLabel";
            lblVolumeLabel.Size = new System.Drawing.Size(78, 15);
            lblVolumeLabel.TabIndex = 104;
            lblVolumeLabel.Text = "Volume label:";
            // 
            // txtVolumeLabel
            // 
            txtVolumeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtVolumeLabel.Location = new System.Drawing.Point(15, 133);
            txtVolumeLabel.MaxLength = 11;
            txtVolumeLabel.Name = "txtVolumeLabel";
            txtVolumeLabel.Size = new System.Drawing.Size(257, 23);
            txtVolumeLabel.TabIndex = 3;
            // 
            // cbxQuickFormat
            // 
            cbxQuickFormat.AutoSize = true;
            cbxQuickFormat.Checked = true;
            cbxQuickFormat.CheckState = System.Windows.Forms.CheckState.Checked;
            cbxQuickFormat.Location = new System.Drawing.Point(15, 171);
            cbxQuickFormat.Name = "cbxQuickFormat";
            cbxQuickFormat.Size = new System.Drawing.Size(96, 19);
            cbxQuickFormat.TabIndex = 4;
            cbxQuickFormat.Text = "Quick format";
            // 
            // dlgFormat
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(284, 261);
            Controls.Add(cbxQuickFormat);
            Controls.Add(txtVolumeLabel);
            Controls.Add(lblVolumeLabel);
            Controls.Add(lstClusterSize);
            Controls.Add(lblClusterSize);
            Controls.Add(lstFileSystem);
            Controls.Add(lblFileSystem);
            Controls.Add(pnlBottom);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgFormat";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Format";
            Load += dlgFormat_Load;
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
