namespace TotalImage.Dialogs
{
    partial class AboutDialog
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
            okButton = new System.Windows.Forms.Button();
            titleLabel = new System.Windows.Forms.Label();
            descriptionLabel = new System.Windows.Forms.Label();
            versionLabel = new System.Windows.Forms.Label();
            copyrightLabel = new System.Windows.Forms.Label();
            buttonPanel = new System.Windows.Forms.Panel();
            logoPictureBox = new System.Windows.Forms.PictureBox();
            licenseLabel = new System.Windows.Forms.Label();
            projectLinkLabel = new System.Windows.Forms.LinkLabel();
            buttonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
            SuspendLayout();
            // 
            // okButton
            // 
            okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Location = new System.Drawing.Point(177, 12);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(80, 26);
            okButton.TabIndex = 0;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            titleLabel.Location = new System.Drawing.Point(66, 12);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new System.Drawing.Size(96, 21);
            titleLabel.TabIndex = 1;
            titleLabel.Text = "TotalImage";
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new System.Drawing.Point(67, 43);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new System.Drawing.Size(143, 15);
            descriptionLabel.TabIndex = 2;
            descriptionLabel.Text = "A better disk image editor";
            // 
            // versionLabel
            // 
            versionLabel.AutoSize = true;
            versionLabel.Location = new System.Drawing.Point(9, 73);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new System.Drawing.Size(229, 15);
            versionLabel.TabIndex = 3;
            versionLabel.Text = "Version: <version> <release> (<commit>)";
            // 
            // copyrightLabel
            // 
            copyrightLabel.AutoSize = true;
            copyrightLabel.Location = new System.Drawing.Point(9, 97);
            copyrightLabel.Name = "copyrightLabel";
            copyrightLabel.Size = new System.Drawing.Size(247, 15);
            copyrightLabel.TabIndex = 4;
            copyrightLabel.Text = "Copyright © 2020-2026 The TotalImage Team";
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = System.Drawing.SystemColors.Control;
            buttonPanel.Controls.Add(okButton);
            buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonPanel.Location = new System.Drawing.Point(0, 206);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(269, 50);
            buttonPanel.TabIndex = 5;
            // 
            // logoPictureBox
            // 
            logoPictureBox.Image = Properties.Resources.logo_48;
            logoPictureBox.Location = new System.Drawing.Point(12, 12);
            logoPictureBox.Name = "logoPictureBox";
            logoPictureBox.Size = new System.Drawing.Size(48, 48);
            logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            logoPictureBox.TabIndex = 6;
            logoPictureBox.TabStop = false;
            // 
            // licenseLabel
            // 
            licenseLabel.AutoSize = true;
            licenseLabel.Location = new System.Drawing.Point(9, 121);
            licenseLabel.Name = "licenseLabel";
            licenseLabel.Size = new System.Drawing.Size(249, 45);
            licenseLabel.TabIndex = 7;
            licenseLabel.Text = "TotalImage is licensed under the MIT license. \r\nSee the LICENSE file for more information and\r\nAUTHORS for a list of contributors.";
            // 
            // projectLinkLabel
            // 
            projectLinkLabel.AutoSize = true;
            projectLinkLabel.LinkColor = System.Drawing.SystemColors.HotTrack;
            projectLinkLabel.Location = new System.Drawing.Point(9, 175);
            projectLinkLabel.Name = "projectLinkLabel";
            projectLinkLabel.Size = new System.Drawing.Size(237, 15);
            projectLinkLabel.TabIndex = 8;
            projectLinkLabel.TabStop = true;
            projectLinkLabel.Text = "https://github.com/TotalImage/TotalImage";
            projectLinkLabel.LinkClicked += ProjectLinkLabel_LinkClicked;
            // 
            // AboutDialog
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = okButton;
            ClientSize = new System.Drawing.Size(269, 256);
            Controls.Add(projectLinkLabel);
            Controls.Add(licenseLabel);
            Controls.Add(logoPictureBox);
            Controls.Add(buttonPanel);
            Controls.Add(copyrightLabel);
            Controls.Add(versionLabel);
            Controls.Add(descriptionLabel);
            Controls.Add(titleLabel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "About TotalImage";
            buttonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label licenseLabel;
        private System.Windows.Forms.LinkLabel projectLinkLabel;
    }
}
