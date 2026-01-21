namespace TotalImage
{
    partial class dlgExtract
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
            lblPath = new System.Windows.Forms.Label();
            txtPath = new System.Windows.Forms.TextBox();
            cbxOpenFolder = new System.Windows.Forms.CheckBox();
            rbnIgnoreFolders = new System.Windows.Forms.RadioButton();
            rbnExtractSameFolder = new System.Windows.Forms.RadioButton();
            rbnPreserveDirs = new System.Windows.Forms.RadioButton();
            btnBrowse = new System.Windows.Forms.Button();
            pnlBottom = new System.Windows.Forms.Panel();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(395, 12);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 6;
            btnOK.Text = "OK";
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(482, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(80, 26);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            // 
            // lblPath
            // 
            lblPath.AutoSize = true;
            lblPath.Location = new System.Drawing.Point(9, 17);
            lblPath.Name = "lblPath";
            lblPath.Size = new System.Drawing.Size(252, 15);
            lblPath.TabIndex = 10;
            lblPath.Text = "Extract selected item(s) to the following folder:";
            // 
            // txtPath
            // 
            txtPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtPath.Location = new System.Drawing.Point(12, 36);
            txtPath.Name = "txtPath";
            txtPath.Size = new System.Drawing.Size(464, 23);
            txtPath.TabIndex = 0;
            // 
            // cbxOpenFolder
            // 
            cbxOpenFolder.AutoSize = true;
            cbxOpenFolder.Location = new System.Drawing.Point(12, 100);
            cbxOpenFolder.Name = "cbxOpenFolder";
            cbxOpenFolder.Size = new System.Drawing.Size(233, 19);
            cbxOpenFolder.TabIndex = 5;
            cbxOpenFolder.Text = "Open destination folder after extraction";
            // 
            // rbnIgnoreFolders
            // 
            rbnIgnoreFolders.AutoSize = true;
            rbnIgnoreFolders.Checked = true;
            rbnIgnoreFolders.Location = new System.Drawing.Point(12, 74);
            rbnIgnoreFolders.Name = "rbnIgnoreFolders";
            rbnIgnoreFolders.Size = new System.Drawing.Size(98, 19);
            rbnIgnoreFolders.TabIndex = 2;
            rbnIgnoreFolders.TabStop = true;
            rbnIgnoreFolders.Text = "Ignore folders";
            // 
            // rbnExtractSameFolder
            // 
            rbnExtractSameFolder.AutoSize = true;
            rbnExtractSameFolder.Location = new System.Drawing.Point(122, 74);
            rbnExtractSameFolder.Name = "rbnExtractSameFolder";
            rbnExtractSameFolder.Size = new System.Drawing.Size(208, 19);
            rbnExtractSameFolder.TabIndex = 3;
            rbnExtractSameFolder.TabStop = true;
            rbnExtractSameFolder.Text = "Extract all files into the same folder";
            // 
            // rbnPreserveDirs
            // 
            rbnPreserveDirs.AutoSize = true;
            rbnPreserveDirs.Location = new System.Drawing.Point(343, 74);
            rbnPreserveDirs.Name = "rbnPreserveDirs";
            rbnPreserveDirs.Size = new System.Drawing.Size(212, 19);
            rbnPreserveDirs.TabIndex = 4;
            rbnPreserveDirs.TabStop = true;
            rbnPreserveDirs.Text = "Preserve original directory structure";
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new System.Drawing.Point(482, 34);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new System.Drawing.Size(80, 26);
            btnBrowse.TabIndex = 1;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 132);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(574, 50);
            pnlBottom.TabIndex = 11;
            // 
            // dlgExtract
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(574, 182);
            Controls.Add(pnlBottom);
            Controls.Add(btnBrowse);
            Controls.Add(rbnPreserveDirs);
            Controls.Add(rbnExtractSameFolder);
            Controls.Add(rbnIgnoreFolders);
            Controls.Add(cbxOpenFolder);
            Controls.Add(txtPath);
            Controls.Add(lblPath);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgExtract";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Extract item(s)";
            Load += dlgExtract_Load;
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.CheckBox cbxOpenFolder;
        private System.Windows.Forms.RadioButton rbnIgnoreFolders;
        private System.Windows.Forms.RadioButton rbnExtractSameFolder;
        private System.Windows.Forms.RadioButton rbnPreserveDirs;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Panel pnlBottom;
        internal System.Windows.Forms.Label lblPath;
    }
}
