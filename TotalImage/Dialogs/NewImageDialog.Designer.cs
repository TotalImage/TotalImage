
namespace TotalImage.Dialogs
{
    partial class NewImageDialog
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
            okButton = new System.Windows.Forms.Button();
            cancelButton = new System.Windows.Forms.Button();
            mediaTypeGroupBox = new System.Windows.Forms.GroupBox();
            containerFormatLabel = new System.Windows.Forms.Label();
            containerFormatComboBox = new System.Windows.Forms.ComboBox();
            hardDiskRadioButton = new System.Windows.Forms.RadioButton();
            floppyDiskRadioButton = new System.Windows.Forms.RadioButton();
            optionsGroupBox = new System.Windows.Forms.GroupBox();
            hardDiskPanel = new System.Windows.Forms.Panel();
            partitionTableComboBox = new System.Windows.Forms.ComboBox();
            writePartitionTableCheckBox = new System.Windows.Forms.CheckBox();
            differencingRadioButton = new System.Windows.Forms.RadioButton();
            dynamicRadioButton = new System.Windows.Forms.RadioButton();
            fixedSizeRadioButton = new System.Windows.Forms.RadioButton();
            diskTypeLabel = new System.Windows.Forms.Label();
            diskSizeLabel = new System.Windows.Forms.Label();
            diskSizeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            floppyPanel = new System.Windows.Forms.Panel();
            bpbVersionComboBox = new System.Windows.Forms.ComboBox();
            writeBpbCheckBox = new System.Windows.Forms.CheckBox();
            volumeLabelTextBox = new System.Windows.Forms.TextBox();
            geometryLabel = new System.Windows.Forms.Label();
            volumeLabelLabel = new System.Windows.Forms.Label();
            advancedButton = new System.Windows.Forms.Button();
            geometryComboBox = new System.Windows.Forms.ComboBox();
            buttonPanel.SuspendLayout();
            mediaTypeGroupBox.SuspendLayout();
            optionsGroupBox.SuspendLayout();
            hardDiskPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)diskSizeNumericUpDown).BeginInit();
            floppyPanel.SuspendLayout();
            SuspendLayout();
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = System.Drawing.SystemColors.Control;
            buttonPanel.Controls.Add(okButton);
            buttonPanel.Controls.Add(cancelButton);
            buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonPanel.Location = new System.Drawing.Point(0, 272);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(399, 50);
            buttonPanel.TabIndex = 0;
            // 
            // okButton
            // 
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Location = new System.Drawing.Point(219, 12);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(80, 26);
            okButton.TabIndex = 9;
            okButton.Text = "OK";
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(305, 12);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(80, 26);
            cancelButton.TabIndex = 10;
            cancelButton.Text = "Cancel";
            // 
            // mediaTypeGroupBox
            // 
            mediaTypeGroupBox.Controls.Add(containerFormatLabel);
            mediaTypeGroupBox.Controls.Add(containerFormatComboBox);
            mediaTypeGroupBox.Controls.Add(hardDiskRadioButton);
            mediaTypeGroupBox.Controls.Add(floppyDiskRadioButton);
            mediaTypeGroupBox.Location = new System.Drawing.Point(10, 12);
            mediaTypeGroupBox.Name = "mediaTypeGroupBox";
            mediaTypeGroupBox.Size = new System.Drawing.Size(378, 80);
            mediaTypeGroupBox.TabIndex = 0;
            mediaTypeGroupBox.TabStop = false;
            mediaTypeGroupBox.Text = "Media and container type";
            // 
            // containerFormatLabel
            // 
            containerFormatLabel.AutoSize = true;
            containerFormatLabel.Location = new System.Drawing.Point(6, 51);
            containerFormatLabel.Name = "containerFormatLabel";
            containerFormatLabel.Size = new System.Drawing.Size(101, 15);
            containerFormatLabel.TabIndex = 9;
            containerFormatLabel.Text = "Container format:";
            // 
            // containerFormatComboBox
            // 
            containerFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            containerFormatComboBox.FormattingEnabled = true;
            containerFormatComboBox.Location = new System.Drawing.Point(113, 47);
            containerFormatComboBox.Name = "containerFormatComboBox";
            containerFormatComboBox.Size = new System.Drawing.Size(259, 23);
            containerFormatComboBox.TabIndex = 9;
            // 
            // hardDiskRadioButton
            // 
            hardDiskRadioButton.AutoSize = true;
            hardDiskRadioButton.Location = new System.Drawing.Point(135, 22);
            hardDiskRadioButton.Name = "hardDiskRadioButton";
            hardDiskRadioButton.Size = new System.Drawing.Size(75, 19);
            hardDiskRadioButton.TabIndex = 2;
            hardDiskRadioButton.Text = "Hard disk";
            // 
            // floppyDiskRadioButton
            // 
            floppyDiskRadioButton.AutoSize = true;
            floppyDiskRadioButton.Checked = true;
            floppyDiskRadioButton.Location = new System.Drawing.Point(8, 22);
            floppyDiskRadioButton.Name = "floppyDiskRadioButton";
            floppyDiskRadioButton.Size = new System.Drawing.Size(85, 19);
            floppyDiskRadioButton.TabIndex = 1;
            floppyDiskRadioButton.TabStop = true;
            floppyDiskRadioButton.Text = "Floppy disk";
            // 
            // optionsGroupBox
            // 
            optionsGroupBox.Controls.Add(floppyPanel);
            optionsGroupBox.Controls.Add(hardDiskPanel);
            optionsGroupBox.Location = new System.Drawing.Point(10, 98);
            optionsGroupBox.Name = "optionsGroupBox";
            optionsGroupBox.Size = new System.Drawing.Size(378, 166);
            optionsGroupBox.TabIndex = 3;
            optionsGroupBox.TabStop = false;
            optionsGroupBox.Text = "Options";
            // 
            // hardDiskPanel
            // 
            hardDiskPanel.Controls.Add(partitionTableComboBox);
            hardDiskPanel.Controls.Add(writePartitionTableCheckBox);
            hardDiskPanel.Controls.Add(differencingRadioButton);
            hardDiskPanel.Controls.Add(dynamicRadioButton);
            hardDiskPanel.Controls.Add(fixedSizeRadioButton);
            hardDiskPanel.Controls.Add(diskTypeLabel);
            hardDiskPanel.Controls.Add(diskSizeLabel);
            hardDiskPanel.Controls.Add(diskSizeNumericUpDown);
            hardDiskPanel.Location = new System.Drawing.Point(2, 17);
            hardDiskPanel.Name = "hardDiskPanel";
            hardDiskPanel.Size = new System.Drawing.Size(371, 140);
            hardDiskPanel.TabIndex = 10;
            // 
            // partitionTableComboBox
            // 
            partitionTableComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            partitionTableComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            partitionTableComboBox.FormattingEnabled = true;
            partitionTableComboBox.Location = new System.Drawing.Point(216, 37);
            partitionTableComboBox.Name = "partitionTableComboBox";
            partitionTableComboBox.Size = new System.Drawing.Size(150, 23);
            partitionTableComboBox.TabIndex = 15;
            // 
            // writePartitionTableCheckBox
            // 
            writePartitionTableCheckBox.AutoSize = true;
            writePartitionTableCheckBox.Checked = true;
            writePartitionTableCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            writePartitionTableCheckBox.Location = new System.Drawing.Point(8, 38);
            writePartitionTableCheckBox.Name = "writePartitionTableCheckBox";
            writePartitionTableCheckBox.Size = new System.Drawing.Size(201, 19);
            writePartitionTableCheckBox.TabIndex = 14;
            writePartitionTableCheckBox.Text = "Write a partition table to the disk:";
            // 
            // differencingRadioButton
            // 
            differencingRadioButton.AutoSize = true;
            differencingRadioButton.Enabled = false;
            differencingRadioButton.Location = new System.Drawing.Point(260, 89);
            differencingRadioButton.Name = "differencingRadioButton";
            differencingRadioButton.Size = new System.Drawing.Size(90, 19);
            differencingRadioButton.TabIndex = 13;
            differencingRadioButton.Text = "Differencing";
            // 
            // dynamicRadioButton
            // 
            dynamicRadioButton.AutoSize = true;
            dynamicRadioButton.Enabled = false;
            dynamicRadioButton.Location = new System.Drawing.Point(98, 89);
            dynamicRadioButton.Name = "dynamicRadioButton";
            dynamicRadioButton.Size = new System.Drawing.Size(148, 19);
            dynamicRadioButton.TabIndex = 12;
            dynamicRadioButton.Text = "Dynamically expanding";
            // 
            // fixedSizeRadioButton
            // 
            fixedSizeRadioButton.AutoSize = true;
            fixedSizeRadioButton.Checked = true;
            fixedSizeRadioButton.Location = new System.Drawing.Point(8, 89);
            fixedSizeRadioButton.Name = "fixedSizeRadioButton";
            fixedSizeRadioButton.Size = new System.Drawing.Size(76, 19);
            fixedSizeRadioButton.TabIndex = 10;
            fixedSizeRadioButton.TabStop = true;
            fixedSizeRadioButton.Text = "Fixed-size";
            // 
            // diskTypeLabel
            // 
            diskTypeLabel.AutoSize = true;
            diskTypeLabel.Location = new System.Drawing.Point(5, 70);
            diskTypeLabel.Name = "diskTypeLabel";
            diskTypeLabel.Size = new System.Drawing.Size(58, 15);
            diskTypeLabel.TabIndex = 10;
            diskTypeLabel.Text = "Disk type:";
            // 
            // diskSizeLabel
            // 
            diskSizeLabel.AutoSize = true;
            diskSizeLabel.Location = new System.Drawing.Point(5, 10);
            diskSizeLabel.Name = "diskSizeLabel";
            diskSizeLabel.Size = new System.Drawing.Size(86, 15);
            diskSizeLabel.TabIndex = 10;
            diskSizeLabel.Text = "Disk size (MiB):";
            // 
            // diskSizeNumericUpDown
            // 
            diskSizeNumericUpDown.Location = new System.Drawing.Point(101, 8);
            diskSizeNumericUpDown.Margin = new System.Windows.Forms.Padding(2);
            diskSizeNumericUpDown.Maximum = new decimal(new int[] { 2088960, 0, 0, 0 });
            diskSizeNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            diskSizeNumericUpDown.Name = "diskSizeNumericUpDown";
            diskSizeNumericUpDown.Size = new System.Drawing.Size(120, 23);
            diskSizeNumericUpDown.TabIndex = 11;
            diskSizeNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // floppyPanel
            // 
            floppyPanel.Controls.Add(bpbVersionComboBox);
            floppyPanel.Controls.Add(writeBpbCheckBox);
            floppyPanel.Controls.Add(volumeLabelTextBox);
            floppyPanel.Controls.Add(geometryLabel);
            floppyPanel.Controls.Add(volumeLabelLabel);
            floppyPanel.Controls.Add(advancedButton);
            floppyPanel.Controls.Add(geometryComboBox);
            floppyPanel.Location = new System.Drawing.Point(2, 17);
            floppyPanel.Name = "floppyPanel";
            floppyPanel.Size = new System.Drawing.Size(371, 140);
            floppyPanel.TabIndex = 9;
            // 
            // bpbVersionComboBox
            // 
            bpbVersionComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            bpbVersionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            bpbVersionComboBox.FormattingEnabled = true;
            bpbVersionComboBox.Location = new System.Drawing.Point(227, 86);
            bpbVersionComboBox.Name = "bpbVersionComboBox";
            bpbVersionComboBox.Size = new System.Drawing.Size(138, 23);
            bpbVersionComboBox.TabIndex = 8;
            // 
            // writeBpbCheckBox
            // 
            writeBpbCheckBox.AutoSize = true;
            writeBpbCheckBox.Location = new System.Drawing.Point(8, 89);
            writeBpbCheckBox.Name = "writeBpbCheckBox";
            writeBpbCheckBox.Size = new System.Drawing.Size(213, 19);
            writeBpbCheckBox.TabIndex = 7;
            writeBpbCheckBox.Text = "Write a DOS BPB to the boot sector:";
            // 
            // volumeLabelTextBox
            // 
            volumeLabelTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            volumeLabelTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            volumeLabelTextBox.Location = new System.Drawing.Point(90, 57);
            volumeLabelTextBox.MaxLength = 11;
            volumeLabelTextBox.Name = "volumeLabelTextBox";
            volumeLabelTextBox.Size = new System.Drawing.Size(275, 23);
            volumeLabelTextBox.TabIndex = 6;
            // 
            // geometryLabel
            // 
            geometryLabel.AutoSize = true;
            geometryLabel.Location = new System.Drawing.Point(6, 10);
            geometryLabel.Name = "geometryLabel";
            geometryLabel.Size = new System.Drawing.Size(62, 15);
            geometryLabel.TabIndex = 0;
            geometryLabel.Text = "Geometry:";
            // 
            // volumeLabelLabel
            // 
            volumeLabelLabel.AutoSize = true;
            volumeLabelLabel.Location = new System.Drawing.Point(6, 62);
            volumeLabelLabel.Name = "volumeLabelLabel";
            volumeLabelLabel.Size = new System.Drawing.Size(78, 15);
            volumeLabelLabel.TabIndex = 3;
            volumeLabelLabel.Text = "Volume label:";
            // 
            // advancedButton
            // 
            advancedButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            advancedButton.Location = new System.Drawing.Point(282, 27);
            advancedButton.Name = "advancedButton";
            advancedButton.Size = new System.Drawing.Size(83, 26);
            advancedButton.TabIndex = 5;
            advancedButton.Text = "Advanced...";
            advancedButton.UseVisualStyleBackColor = true;
            // 
            // geometryComboBox
            // 
            geometryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            geometryComboBox.FormattingEnabled = true;
            geometryComboBox.Location = new System.Drawing.Point(8, 28);
            geometryComboBox.Name = "geometryComboBox";
            geometryComboBox.Size = new System.Drawing.Size(270, 23);
            geometryComboBox.TabIndex = 4;
            // 
            // NewImageDialog
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(399, 322);
            Controls.Add(optionsGroupBox);
            Controls.Add(mediaTypeGroupBox);
            Controls.Add(buttonPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NewImageDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "New image";
            buttonPanel.ResumeLayout(false);
            mediaTypeGroupBox.ResumeLayout(false);
            mediaTypeGroupBox.PerformLayout();
            optionsGroupBox.ResumeLayout(false);
            hardDiskPanel.ResumeLayout(false);
            hardDiskPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)diskSizeNumericUpDown).EndInit();
            floppyPanel.ResumeLayout(false);
            floppyPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.GroupBox mediaTypeGroupBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.RadioButton floppyDiskRadioButton;
        private System.Windows.Forms.RadioButton hardDiskRadioButton;
        private System.Windows.Forms.GroupBox optionsGroupBox;
        private System.Windows.Forms.Label geometryLabel;
        private System.Windows.Forms.Button advancedButton;
        private System.Windows.Forms.Label volumeLabelLabel;
        private System.Windows.Forms.TextBox volumeLabelTextBox;
        private System.Windows.Forms.Panel floppyPanel;
        private System.Windows.Forms.Panel hardDiskPanel;
        private System.Windows.Forms.ComboBox geometryComboBox;
        private System.Windows.Forms.CheckBox writeBpbCheckBox;
        private System.Windows.Forms.ComboBox bpbVersionComboBox;
        private System.Windows.Forms.Label containerFormatLabel;
        private System.Windows.Forms.ComboBox containerFormatComboBox;
        private System.Windows.Forms.Label diskSizeLabel;
        private System.Windows.Forms.NumericUpDown diskSizeNumericUpDown;
        private System.Windows.Forms.RadioButton differencingRadioButton;
        private System.Windows.Forms.RadioButton dynamicRadioButton;
        private System.Windows.Forms.RadioButton fixedSizeRadioButton;
        private System.Windows.Forms.Label diskTypeLabel;
        private System.Windows.Forms.ComboBox partitionTableComboBox;
        private System.Windows.Forms.CheckBox writePartitionTableCheckBox;
    }
}
