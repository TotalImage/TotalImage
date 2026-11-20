namespace TotalImage.Dialogs
{
    partial class ExtractDialog
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
            cancelButton = new System.Windows.Forms.Button();
            promptLabel = new System.Windows.Forms.Label();
            targetPathTextBox = new System.Windows.Forms.TextBox();
            openFolderCheckBox = new System.Windows.Forms.CheckBox();
            skipDirectoriesRadioButton = new System.Windows.Forms.RadioButton();
            mergeDirectoriesRadioButton = new System.Windows.Forms.RadioButton();
            preserveDirectoriesRadioButton = new System.Windows.Forms.RadioButton();
            browseButton = new System.Windows.Forms.Button();
            buttonPanel = new System.Windows.Forms.Panel();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // okButton
            // 
            okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            okButton.Location = new System.Drawing.Point(395, 12);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(80, 26);
            okButton.TabIndex = 6;
            okButton.Text = "OK";
            // 
            // cancelButton
            // 
            cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(482, 12);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(80, 26);
            cancelButton.TabIndex = 7;
            cancelButton.Text = "Cancel";
            // 
            // promptLabel
            // 
            promptLabel.AutoSize = true;
            promptLabel.Location = new System.Drawing.Point(9, 17);
            promptLabel.Name = "promptLabel";
            promptLabel.Size = new System.Drawing.Size(252, 15);
            promptLabel.TabIndex = 10;
            promptLabel.Text = "Extract selected item(s) to the following folder:";
            // 
            // targetPathTextBox
            // 
            targetPathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            targetPathTextBox.Location = new System.Drawing.Point(12, 36);
            targetPathTextBox.Name = "targetPathTextBox";
            targetPathTextBox.Size = new System.Drawing.Size(464, 23);
            targetPathTextBox.TabIndex = 0;
            // 
            // openFolderCheckBox
            // 
            openFolderCheckBox.AutoSize = true;
            openFolderCheckBox.Location = new System.Drawing.Point(12, 100);
            openFolderCheckBox.Name = "openFolderCheckBox";
            openFolderCheckBox.Size = new System.Drawing.Size(233, 19);
            openFolderCheckBox.TabIndex = 5;
            openFolderCheckBox.Text = "Open destination folder after extraction";
            // 
            // skipDirectoriesRadioButton
            // 
            skipDirectoriesRadioButton.AutoSize = true;
            skipDirectoriesRadioButton.Location = new System.Drawing.Point(12, 74);
            skipDirectoriesRadioButton.Name = "skipDirectoriesRadioButton";
            skipDirectoriesRadioButton.Size = new System.Drawing.Size(98, 19);
            skipDirectoriesRadioButton.TabIndex = 2;
            skipDirectoriesRadioButton.TabStop = true;
            skipDirectoriesRadioButton.Text = "Ignore folders";
            // 
            // mergeDirectoriesRadioButton
            // 
            mergeDirectoriesRadioButton.AutoSize = true;
            mergeDirectoriesRadioButton.Location = new System.Drawing.Point(122, 74);
            mergeDirectoriesRadioButton.Name = "mergeDirectoriesRadioButton";
            mergeDirectoriesRadioButton.Size = new System.Drawing.Size(208, 19);
            mergeDirectoriesRadioButton.TabIndex = 3;
            mergeDirectoriesRadioButton.TabStop = true;
            mergeDirectoriesRadioButton.Text = "Extract all files into the same folder";
            // 
            // preserveDirectoriesRadioButton
            // 
            preserveDirectoriesRadioButton.AutoSize = true;
            preserveDirectoriesRadioButton.Location = new System.Drawing.Point(343, 74);
            preserveDirectoriesRadioButton.Name = "preserveDirectoriesRadioButton";
            preserveDirectoriesRadioButton.Size = new System.Drawing.Size(212, 19);
            preserveDirectoriesRadioButton.TabIndex = 4;
            preserveDirectoriesRadioButton.TabStop = true;
            preserveDirectoriesRadioButton.Text = "Preserve original directory structure";
            // 
            // browseButton
            // 
            browseButton.Location = new System.Drawing.Point(482, 34);
            browseButton.Name = "browseButton";
            browseButton.Size = new System.Drawing.Size(80, 26);
            browseButton.TabIndex = 1;
            browseButton.Text = "Browse...";
            browseButton.UseVisualStyleBackColor = true;
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = System.Drawing.SystemColors.Control;
            buttonPanel.Controls.Add(cancelButton);
            buttonPanel.Controls.Add(okButton);
            buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonPanel.Location = new System.Drawing.Point(0, 132);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(574, 50);
            buttonPanel.TabIndex = 11;
            // 
            // ExtractDialog
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(574, 182);
            Controls.Add(buttonPanel);
            Controls.Add(browseButton);
            Controls.Add(preserveDirectoriesRadioButton);
            Controls.Add(mergeDirectoriesRadioButton);
            Controls.Add(skipDirectoriesRadioButton);
            Controls.Add(openFolderCheckBox);
            Controls.Add(targetPathTextBox);
            Controls.Add(promptLabel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ExtractDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Extract item(s)";
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox targetPathTextBox;
        private System.Windows.Forms.CheckBox openFolderCheckBox;
        private System.Windows.Forms.RadioButton skipDirectoriesRadioButton;
        private System.Windows.Forms.RadioButton mergeDirectoriesRadioButton;
        private System.Windows.Forms.RadioButton preserveDirectoriesRadioButton;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Label promptLabel;
    }
}
