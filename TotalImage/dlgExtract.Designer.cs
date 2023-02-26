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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblPath = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.cbxOpenFolder = new System.Windows.Forms.CheckBox();
            this.rbnIgnoreFolders = new System.Windows.Forms.RadioButton();
            this.rbnExtractSameFolder = new System.Windows.Forms.RadioButton();
            this.rbnPreserveDirs = new System.Windows.Forms.RadioButton();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(494, 15);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 32);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(602, 15);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 32);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(11, 21);
            this.lblPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(320, 20);
            this.lblPath.TabIndex = 10;
            this.lblPath.Text = "Extract selected item(s) to the following folder:";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(15, 45);
            this.txtPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(579, 27);
            this.txtPath.TabIndex = 0;
            // 
            // cbxOpenFolder
            // 
            this.cbxOpenFolder.AutoSize = true;
            this.cbxOpenFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxOpenFolder.Location = new System.Drawing.Point(15, 125);
            this.cbxOpenFolder.Margin = new System.Windows.Forms.Padding(4);
            this.cbxOpenFolder.Name = "cbxOpenFolder";
            this.cbxOpenFolder.Size = new System.Drawing.Size(303, 25);
            this.cbxOpenFolder.TabIndex = 5;
            this.cbxOpenFolder.Text = "Open destination folder after extraction";
            this.cbxOpenFolder.UseVisualStyleBackColor = true;
            // 
            // rbnIgnoreFolders
            // 
            this.rbnIgnoreFolders.AutoSize = true;
            this.rbnIgnoreFolders.Checked = true;
            this.rbnIgnoreFolders.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnIgnoreFolders.Location = new System.Drawing.Point(15, 92);
            this.rbnIgnoreFolders.Margin = new System.Windows.Forms.Padding(4);
            this.rbnIgnoreFolders.Name = "rbnIgnoreFolders";
            this.rbnIgnoreFolders.Size = new System.Drawing.Size(132, 25);
            this.rbnIgnoreFolders.TabIndex = 2;
            this.rbnIgnoreFolders.TabStop = true;
            this.rbnIgnoreFolders.Text = "Ignore folders";
            this.rbnIgnoreFolders.UseVisualStyleBackColor = true;
            // 
            // rbnExtractSameFolder
            // 
            this.rbnExtractSameFolder.AutoSize = true;
            this.rbnExtractSameFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnExtractSameFolder.Location = new System.Drawing.Point(152, 92);
            this.rbnExtractSameFolder.Margin = new System.Windows.Forms.Padding(4);
            this.rbnExtractSameFolder.Name = "rbnExtractSameFolder";
            this.rbnExtractSameFolder.Size = new System.Drawing.Size(273, 25);
            this.rbnExtractSameFolder.TabIndex = 3;
            this.rbnExtractSameFolder.TabStop = true;
            this.rbnExtractSameFolder.Text = "Extract all files into the same folder";
            this.rbnExtractSameFolder.UseVisualStyleBackColor = true;
            // 
            // rbnPreserveDirs
            // 
            this.rbnPreserveDirs.AutoSize = true;
            this.rbnPreserveDirs.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnPreserveDirs.Location = new System.Drawing.Point(429, 92);
            this.rbnPreserveDirs.Margin = new System.Windows.Forms.Padding(4);
            this.rbnPreserveDirs.Name = "rbnPreserveDirs";
            this.rbnPreserveDirs.Size = new System.Drawing.Size(273, 25);
            this.rbnPreserveDirs.TabIndex = 4;
            this.rbnPreserveDirs.TabStop = true;
            this.rbnPreserveDirs.Text = "Preserve original directory structure";
            this.rbnPreserveDirs.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(602, 42);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 32);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 166);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(717, 62);
            this.pnlBottom.TabIndex = 11;
            // 
            // dlgExtract
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(717, 228);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.rbnPreserveDirs);
            this.Controls.Add(this.rbnExtractSameFolder);
            this.Controls.Add(this.rbnIgnoreFolders);
            this.Controls.Add(this.cbxOpenFolder);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lblPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgExtract";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Extract item(s)";
            this.Load += new System.EventHandler(this.dlgExtract_Load);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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