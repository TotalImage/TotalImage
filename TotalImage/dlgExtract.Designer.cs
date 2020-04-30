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
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(461, 138);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(547, 138);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(9, 9);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(245, 15);
            this.lblPath.TabIndex = 10;
            this.lblPath.Text = "Extract selected file(s) to the following folder:";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(12, 36);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(529, 23);
            this.txtPath.TabIndex = 11;
            // 
            // cbxOpenFolder
            // 
            this.cbxOpenFolder.AutoSize = true;
            this.cbxOpenFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxOpenFolder.Location = new System.Drawing.Point(12, 109);
            this.cbxOpenFolder.Name = "cbxOpenFolder";
            this.cbxOpenFolder.Size = new System.Drawing.Size(240, 20);
            this.cbxOpenFolder.TabIndex = 12;
            this.cbxOpenFolder.Text = "Open destination folder after extraction";
            this.cbxOpenFolder.UseVisualStyleBackColor = true;
            // 
            // rbnIgnoreFolders
            // 
            this.rbnIgnoreFolders.AutoSize = true;
            this.rbnIgnoreFolders.Checked = true;
            this.rbnIgnoreFolders.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnIgnoreFolders.Location = new System.Drawing.Point(12, 74);
            this.rbnIgnoreFolders.Name = "rbnIgnoreFolders";
            this.rbnIgnoreFolders.Size = new System.Drawing.Size(104, 20);
            this.rbnIgnoreFolders.TabIndex = 13;
            this.rbnIgnoreFolders.TabStop = true;
            this.rbnIgnoreFolders.Text = "Ignore folders";
            this.rbnIgnoreFolders.UseVisualStyleBackColor = true;
            // 
            // rbnExtractSameFolder
            // 
            this.rbnExtractSameFolder.AutoSize = true;
            this.rbnExtractSameFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnExtractSameFolder.Location = new System.Drawing.Point(122, 74);
            this.rbnExtractSameFolder.Name = "rbnExtractSameFolder";
            this.rbnExtractSameFolder.Size = new System.Drawing.Size(215, 20);
            this.rbnExtractSameFolder.TabIndex = 14;
            this.rbnExtractSameFolder.TabStop = true;
            this.rbnExtractSameFolder.Text = "Extract all files into the same folder";
            this.rbnExtractSameFolder.UseVisualStyleBackColor = true;
            // 
            // rbnPreserveDirs
            // 
            this.rbnPreserveDirs.AutoSize = true;
            this.rbnPreserveDirs.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbnPreserveDirs.Location = new System.Drawing.Point(343, 74);
            this.rbnPreserveDirs.Name = "rbnPreserveDirs";
            this.rbnPreserveDirs.Size = new System.Drawing.Size(218, 20);
            this.rbnPreserveDirs.TabIndex = 15;
            this.rbnPreserveDirs.TabStop = true;
            this.rbnPreserveDirs.Text = "Preserve original directory structure";
            this.rbnPreserveDirs.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(547, 35);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(80, 26);
            this.btnBrowse.TabIndex = 16;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // dlgExtract
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(639, 176);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.rbnPreserveDirs);
            this.Controls.Add(this.rbnExtractSameFolder);
            this.Controls.Add(this.rbnIgnoreFolders);
            this.Controls.Add(this.cbxOpenFolder);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgExtract";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Extract file(s)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.CheckBox cbxOpenFolder;
        private System.Windows.Forms.RadioButton rbnIgnoreFolders;
        private System.Windows.Forms.RadioButton rbnExtractSameFolder;
        private System.Windows.Forms.RadioButton rbnPreserveDirs;
        private System.Windows.Forms.Button btnBrowse;
    }
}