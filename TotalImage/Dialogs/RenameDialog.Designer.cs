namespace TotalImage.Dialogs
{
    partial class RenameDialog
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
            buttonPanel = new System.Windows.Forms.Panel();
            cancelButton = new System.Windows.Forms.Button();
            okButton = new System.Windows.Forms.Button();
            promptLabel = new System.Windows.Forms.Label();
            nameTextBox = new System.Windows.Forms.TextBox();
            shortNameLabel = new System.Windows.Forms.Label();
            shortNameValueLabel = new System.Windows.Forms.Label();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = System.Drawing.SystemColors.Control;
            buttonPanel.Controls.Add(cancelButton);
            buttonPanel.Controls.Add(okButton);
            buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonPanel.Location = new System.Drawing.Point(0, 101);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(484, 50);
            buttonPanel.TabIndex = 3;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(392, 12);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(80, 26);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Location = new System.Drawing.Point(306, 12);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(80, 26);
            okButton.TabIndex = 0;
            okButton.Text = "OK";
            // 
            // promptLabel
            // 
            promptLabel.AutoSize = true;
            promptLabel.Location = new System.Drawing.Point(9, 9);
            promptLabel.Name = "promptLabel";
            promptLabel.Size = new System.Drawing.Size(214, 15);
            promptLabel.TabIndex = 4;
            promptLabel.Text = "Enter a new name for <oldname here>:";
            // 
            // nameTextBox
            // 
            nameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            nameTextBox.Location = new System.Drawing.Point(12, 36);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new System.Drawing.Size(460, 23);
            nameTextBox.TabIndex = 5;
            // 
            // shortNameLabel
            // 
            shortNameLabel.AutoSize = true;
            shortNameLabel.Location = new System.Drawing.Point(9, 71);
            shortNameLabel.Name = "shortNameLabel";
            shortNameLabel.Size = new System.Drawing.Size(97, 15);
            shortNameLabel.TabIndex = 6;
            shortNameLabel.Text = "Short name (8.3):";
            // 
            // shortNameValueLabel
            // 
            shortNameValueLabel.AutoSize = true;
            shortNameValueLabel.Location = new System.Drawing.Point(109, 71);
            shortNameValueLabel.Name = "shortNameValueLabel";
            shortNameValueLabel.Size = new System.Drawing.Size(80, 15);
            shortNameValueLabel.TabIndex = 7;
            shortNameValueLabel.Text = "<shortname>";
            // 
            // RenameDialog
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(484, 151);
            Controls.Add(shortNameValueLabel);
            Controls.Add(shortNameLabel);
            Controls.Add(nameTextBox);
            Controls.Add(promptLabel);
            Controls.Add(buttonPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RenameDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Rename item";
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label promptLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label shortNameLabel;
        private System.Windows.Forms.Label shortNameValueLabel;
    }
}
