namespace TotalImage.Dialogs
{
    partial class FormatDialog
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
            fileSystemLabel = new System.Windows.Forms.Label();
            fileSystemComboBox = new System.Windows.Forms.ComboBox();
            clusterSizeComboBox = new System.Windows.Forms.ComboBox();
            clusterSizeLabel = new System.Windows.Forms.Label();
            volumeLabelLabel = new System.Windows.Forms.Label();
            volumeLabelTextBox = new System.Windows.Forms.TextBox();
            quickFormatCheckBox = new System.Windows.Forms.CheckBox();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = System.Drawing.SystemColors.Control;
            buttonPanel.Controls.Add(cancelButton);
            buttonPanel.Controls.Add(okButton);
            buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonPanel.Location = new System.Drawing.Point(0, 211);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(284, 50);
            buttonPanel.TabIndex = 99;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(192, 12);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(80, 26);
            cancelButton.TabIndex = 6;
            cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Location = new System.Drawing.Point(106, 12);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(80, 26);
            okButton.TabIndex = 5;
            okButton.Text = "OK";
            // 
            // fileSystemLabel
            // 
            fileSystemLabel.AutoSize = true;
            fileSystemLabel.Location = new System.Drawing.Point(12, 9);
            fileSystemLabel.Name = "fileSystemLabel";
            fileSystemLabel.Size = new System.Drawing.Size(68, 15);
            fileSystemLabel.TabIndex = 100;
            fileSystemLabel.Text = "File system:";
            // 
            // fileSystemComboBox
            // 
            fileSystemComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            fileSystemComboBox.FormattingEnabled = true;
            fileSystemComboBox.Location = new System.Drawing.Point(15, 27);
            fileSystemComboBox.Name = "fileSystemComboBox";
            fileSystemComboBox.Size = new System.Drawing.Size(257, 23);
            fileSystemComboBox.TabIndex = 1;
            // 
            // clusterSizeComboBox
            // 
            clusterSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            clusterSizeComboBox.FormattingEnabled = true;
            clusterSizeComboBox.Location = new System.Drawing.Point(15, 80);
            clusterSizeComboBox.Name = "clusterSizeComboBox";
            clusterSizeComboBox.Size = new System.Drawing.Size(257, 23);
            clusterSizeComboBox.TabIndex = 2;
            // 
            // clusterSizeLabel
            // 
            clusterSizeLabel.AutoSize = true;
            clusterSizeLabel.Location = new System.Drawing.Point(12, 62);
            clusterSizeLabel.Name = "clusterSizeLabel";
            clusterSizeLabel.Size = new System.Drawing.Size(69, 15);
            clusterSizeLabel.TabIndex = 102;
            clusterSizeLabel.Text = "Cluster size:";
            // 
            // volumeLabelLabel
            // 
            volumeLabelLabel.AutoSize = true;
            volumeLabelLabel.Location = new System.Drawing.Point(12, 115);
            volumeLabelLabel.Name = "volumeLabelLabel";
            volumeLabelLabel.Size = new System.Drawing.Size(78, 15);
            volumeLabelLabel.TabIndex = 104;
            volumeLabelLabel.Text = "Volume label:";
            // 
            // volumeLabelTextBox
            // 
            volumeLabelTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            volumeLabelTextBox.Location = new System.Drawing.Point(15, 133);
            volumeLabelTextBox.MaxLength = 11;
            volumeLabelTextBox.Name = "volumeLabelTextBox";
            volumeLabelTextBox.Size = new System.Drawing.Size(257, 23);
            volumeLabelTextBox.TabIndex = 3;
            // 
            // quickFormatCheckBox
            // 
            quickFormatCheckBox.AutoSize = true;
            quickFormatCheckBox.Checked = true;
            quickFormatCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            quickFormatCheckBox.Location = new System.Drawing.Point(15, 171);
            quickFormatCheckBox.Name = "quickFormatCheckBox";
            quickFormatCheckBox.Size = new System.Drawing.Size(96, 19);
            quickFormatCheckBox.TabIndex = 4;
            quickFormatCheckBox.Text = "Quick format";
            // 
            // FormatDialog
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(284, 261);
            Controls.Add(quickFormatCheckBox);
            Controls.Add(volumeLabelTextBox);
            Controls.Add(volumeLabelLabel);
            Controls.Add(clusterSizeComboBox);
            Controls.Add(clusterSizeLabel);
            Controls.Add(fileSystemComboBox);
            Controls.Add(fileSystemLabel);
            Controls.Add(buttonPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormatDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Format";
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label fileSystemLabel;
        private System.Windows.Forms.ComboBox fileSystemComboBox;
        private System.Windows.Forms.ComboBox clusterSizeComboBox;
        private System.Windows.Forms.Label clusterSizeLabel;
        private System.Windows.Forms.Label volumeLabelLabel;
        private System.Windows.Forms.TextBox volumeLabelTextBox;
        private System.Windows.Forms.CheckBox quickFormatCheckBox;
    }
}
