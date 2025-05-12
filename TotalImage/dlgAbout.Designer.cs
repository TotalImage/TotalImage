namespace TotalImage
{
    partial class dlgAbout
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
            lblTitle = new System.Windows.Forms.Label();
            lblDesc = new System.Windows.Forms.Label();
            lblVer = new System.Windows.Forms.Label();
            lblCopyright = new System.Windows.Forms.Label();
            pnlBottom = new System.Windows.Forms.Panel();
            imgLogo = new System.Windows.Forms.PictureBox();
            lblLicense = new System.Windows.Forms.Label();
            lnkGitHub = new System.Windows.Forms.LinkLabel();
            pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imgLogo).BeginInit();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnOK.Location = new System.Drawing.Point(177, 12);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblTitle.Location = new System.Drawing.Point(66, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(96, 21);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "TotalImage";
            // 
            // lblDesc
            // 
            lblDesc.AutoSize = true;
            lblDesc.Location = new System.Drawing.Point(67, 43);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new System.Drawing.Size(143, 15);
            lblDesc.TabIndex = 2;
            lblDesc.Text = "A better disk image editor";
            // 
            // lblVer
            // 
            lblVer.AutoSize = true;
            lblVer.Location = new System.Drawing.Point(9, 73);
            lblVer.Name = "lblVer";
            lblVer.Size = new System.Drawing.Size(229, 15);
            lblVer.TabIndex = 3;
            lblVer.Text = "Version: <version> <release> (<commit>)";
            // 
            // lblCopyright
            // 
            lblCopyright.AutoSize = true;
            lblCopyright.Location = new System.Drawing.Point(9, 97);
            lblCopyright.Name = "lblCopyright";
            lblCopyright.Size = new System.Drawing.Size(244, 15);
            lblCopyright.TabIndex = 4;
            lblCopyright.Text = "Copyright © 2020-2025 The TotalImage Team";
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 206);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(269, 50);
            pnlBottom.TabIndex = 5;
            // 
            // imgLogo
            // 
            imgLogo.Image = Properties.Resources.logo_48;
            imgLogo.Location = new System.Drawing.Point(12, 12);
            imgLogo.Name = "imgLogo";
            imgLogo.Size = new System.Drawing.Size(48, 48);
            imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            imgLogo.TabIndex = 6;
            imgLogo.TabStop = false;
            // 
            // lblLicense
            // 
            lblLicense.AutoSize = true;
            lblLicense.Location = new System.Drawing.Point(9, 121);
            lblLicense.Name = "lblLicense";
            lblLicense.Size = new System.Drawing.Size(249, 45);
            lblLicense.TabIndex = 7;
            lblLicense.Text = "TotalImage is licensed under the MIT license. \r\nSee the LICENSE file for more information and\r\nAUTHORS for a list of contributors.";
            // 
            // lnkGitHub
            // 
            lnkGitHub.AutoSize = true;
            lnkGitHub.Location = new System.Drawing.Point(9, 175);
            lnkGitHub.Name = "lnkGitHub";
            lnkGitHub.Size = new System.Drawing.Size(235, 15);
            lnkGitHub.TabIndex = 8;
            lnkGitHub.TabStop = true;
            lnkGitHub.Text = "https://github.com/TotalImage/TotalImage";
            lnkGitHub.LinkClicked += lnkGitHub_LinkClicked;
            // 
            // dlgAbout
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            ClientSize = new System.Drawing.Size(269, 256);
            Controls.Add(lnkGitHub);
            Controls.Add(lblLicense);
            Controls.Add(imgLogo);
            Controls.Add(pnlBottom);
            Controls.Add(lblCopyright);
            Controls.Add(lblVer);
            Controls.Add(lblDesc);
            Controls.Add(lblTitle);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgAbout";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "About TotalImage";
            Load += dlgAbout_Load;
            pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)imgLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Label lblVer;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.PictureBox imgLogo;
        private System.Windows.Forms.Label lblLicense;
        private System.Windows.Forms.LinkLabel lnkGitHub;
    }
}
