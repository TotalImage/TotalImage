namespace TotalImage.Dialogs
{
    partial class BootSectorDialog
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
            buttonPanel = new System.Windows.Forms.Panel();
            loadButton = new System.Windows.Forms.Button();
            exportButton = new System.Windows.Forms.Button();
            presetLabel = new System.Windows.Forms.Label();
            presetComboBox = new System.Windows.Forms.ComboBox();
            applyButton = new System.Windows.Forms.Button();
            bootSectorTextBox = new System.Windows.Forms.RichTextBox();
            diskRadioButton = new System.Windows.Forms.RadioButton();
            showLabel = new System.Windows.Forms.Label();
            partitionRadioButton = new System.Windows.Forms.RadioButton();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // okButton
            // 
            okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Location = new System.Drawing.Point(388, 12);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(80, 26);
            okButton.TabIndex = 0;
            okButton.Text = "OK";
            // 
            // cancelButton
            // 
            cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(474, 12);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(80, 26);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Cancel";
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = System.Drawing.SystemColors.Control;
            buttonPanel.Controls.Add(cancelButton);
            buttonPanel.Controls.Add(okButton);
            buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonPanel.Location = new System.Drawing.Point(0, 388);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(566, 50);
            buttonPanel.TabIndex = 10;
            // 
            // loadButton
            // 
            loadButton.Enabled = false;
            loadButton.Location = new System.Drawing.Point(10, 34);
            loadButton.Margin = new System.Windows.Forms.Padding(2);
            loadButton.Name = "loadButton";
            loadButton.Size = new System.Drawing.Size(80, 26);
            loadButton.TabIndex = 11;
            loadButton.Text = "Load...";
            loadButton.UseVisualStyleBackColor = true;
            // 
            // exportButton
            // 
            exportButton.Location = new System.Drawing.Point(94, 34);
            exportButton.Margin = new System.Windows.Forms.Padding(2);
            exportButton.Name = "exportButton";
            exportButton.Size = new System.Drawing.Size(80, 26);
            exportButton.TabIndex = 12;
            exportButton.Text = "Save...";
            exportButton.UseVisualStyleBackColor = true;
            // 
            // presetLabel
            // 
            presetLabel.AutoSize = true;
            presetLabel.Enabled = false;
            presetLabel.Location = new System.Drawing.Point(179, 39);
            presetLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            presetLabel.Name = "presetLabel";
            presetLabel.Size = new System.Drawing.Size(47, 15);
            presetLabel.TabIndex = 13;
            presetLabel.Text = "Presets:";
            // 
            // presetComboBox
            // 
            presetComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            presetComboBox.Enabled = false;
            presetComboBox.FormattingEnabled = true;
            presetComboBox.Location = new System.Drawing.Point(230, 36);
            presetComboBox.Margin = new System.Windows.Forms.Padding(2);
            presetComboBox.Name = "presetComboBox";
            presetComboBox.Size = new System.Drawing.Size(242, 23);
            presetComboBox.TabIndex = 14;
            // 
            // applyButton
            // 
            applyButton.Enabled = false;
            applyButton.Location = new System.Drawing.Point(477, 34);
            applyButton.Margin = new System.Windows.Forms.Padding(2);
            applyButton.Name = "applyButton";
            applyButton.Size = new System.Drawing.Size(80, 26);
            applyButton.TabIndex = 15;
            applyButton.Text = "Apply";
            applyButton.UseVisualStyleBackColor = true;
            // 
            // bootSectorTextBox
            // 
            bootSectorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            bootSectorTextBox.Font = new System.Drawing.Font("Consolas", 9.75F);
            bootSectorTextBox.Location = new System.Drawing.Point(10, 75);
            bootSectorTextBox.Margin = new System.Windows.Forms.Padding(2);
            bootSectorTextBox.Name = "bootSectorTextBox";
            bootSectorTextBox.ReadOnly = true;
            bootSectorTextBox.Size = new System.Drawing.Size(545, 300);
            bootSectorTextBox.TabIndex = 16;
            bootSectorTextBox.Text = "";
            // 
            // diskRadioButton
            // 
            diskRadioButton.AutoSize = true;
            diskRadioButton.Location = new System.Drawing.Point(130, 8);
            diskRadioButton.Margin = new System.Windows.Forms.Padding(2);
            diskRadioButton.Name = "diskRadioButton";
            diskRadioButton.Size = new System.Drawing.Size(83, 19);
            diskRadioButton.TabIndex = 17;
            diskRadioButton.TabStop = true;
            diskRadioButton.Text = "Disk (MBR)";
            // 
            // showLabel
            // 
            showLabel.AutoSize = true;
            showLabel.Location = new System.Drawing.Point(10, 10);
            showLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            showLabel.Name = "showLabel";
            showLabel.Size = new System.Drawing.Size(116, 15);
            showLabel.TabIndex = 18;
            showLabel.Text = "Show boot sector of:";
            // 
            // partitionRadioButton
            // 
            partitionRadioButton.AutoSize = true;
            partitionRadioButton.Location = new System.Drawing.Point(230, 8);
            partitionRadioButton.Margin = new System.Windows.Forms.Padding(2);
            partitionRadioButton.Name = "partitionRadioButton";
            partitionRadioButton.Size = new System.Drawing.Size(149, 19);
            partitionRadioButton.TabIndex = 19;
            partitionRadioButton.TabStop = true;
            partitionRadioButton.Text = "Selected partition (VBR)";
            // 
            // BootSectorDialog
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(566, 438);
            Controls.Add(partitionRadioButton);
            Controls.Add(showLabel);
            Controls.Add(diskRadioButton);
            Controls.Add(bootSectorTextBox);
            Controls.Add(applyButton);
            Controls.Add(presetComboBox);
            Controls.Add(presetLabel);
            Controls.Add(exportButton);
            Controls.Add(loadButton);
            Controls.Add(buttonPanel);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BootSectorDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Boot sector properties";
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Label presetLabel;
        private System.Windows.Forms.ComboBox presetComboBox;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.RichTextBox bootSectorTextBox;
        private System.Windows.Forms.RadioButton diskRadioButton;
        private System.Windows.Forms.Label showLabel;
        private System.Windows.Forms.RadioButton partitionRadioButton;
    }
}
