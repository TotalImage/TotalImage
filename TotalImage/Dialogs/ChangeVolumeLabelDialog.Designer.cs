namespace TotalImage.Dialogs
{
    partial class ChangeVolumeLabelDialog
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
            rootDirectoryLabelTextBox = new System.Windows.Forms.TextBox();
            rootDirectoryLabelLabel = new System.Windows.Forms.Label();
            buttonPanel = new System.Windows.Forms.Panel();
            fatOptionsGroupBox = new System.Windows.Forms.GroupBox();
            syncLabelsCheckBox = new System.Windows.Forms.CheckBox();
            writeBpbLabelCheckBox = new System.Windows.Forms.CheckBox();
            bpbLabelTextBox = new System.Windows.Forms.TextBox();
            buttonPanel.SuspendLayout();
            fatOptionsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // okButton
            // 
            okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Location = new System.Drawing.Point(106, 12);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(80, 26);
            okButton.TabIndex = 3;
            okButton.Text = "OK";
            // 
            // cancelButton
            // 
            cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(192, 12);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(80, 26);
            cancelButton.TabIndex = 4;
            cancelButton.Text = "Cancel";
            // 
            // rootDirectoryLabelTextBox
            // 
            rootDirectoryLabelTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            rootDirectoryLabelTextBox.Location = new System.Drawing.Point(12, 27);
            rootDirectoryLabelTextBox.MaxLength = 11;
            rootDirectoryLabelTextBox.Name = "rootDirectoryLabelTextBox";
            rootDirectoryLabelTextBox.Size = new System.Drawing.Size(260, 23);
            rootDirectoryLabelTextBox.TabIndex = 0;
            // 
            // rootDirectoryLabelLabel
            // 
            rootDirectoryLabelLabel.AutoSize = true;
            rootDirectoryLabelLabel.Location = new System.Drawing.Point(9, 9);
            rootDirectoryLabelLabel.Name = "rootDirectoryLabelLabel";
            rootDirectoryLabelLabel.Size = new System.Drawing.Size(142, 15);
            rootDirectoryLabelLabel.TabIndex = 4;
            rootDirectoryLabelLabel.Text = "Enter a new volume label:";
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = System.Drawing.SystemColors.Control;
            buttonPanel.Controls.Add(okButton);
            buttonPanel.Controls.Add(cancelButton);
            buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonPanel.Location = new System.Drawing.Point(0, 171);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(284, 50);
            buttonPanel.TabIndex = 5;
            // 
            // fatOptionsGroupBox
            // 
            fatOptionsGroupBox.Controls.Add(syncLabelsCheckBox);
            fatOptionsGroupBox.Controls.Add(writeBpbLabelCheckBox);
            fatOptionsGroupBox.Controls.Add(bpbLabelTextBox);
            fatOptionsGroupBox.Location = new System.Drawing.Point(12, 56);
            fatOptionsGroupBox.Name = "fatOptionsGroupBox";
            fatOptionsGroupBox.Size = new System.Drawing.Size(260, 109);
            fatOptionsGroupBox.TabIndex = 7;
            fatOptionsGroupBox.TabStop = false;
            fatOptionsGroupBox.Text = "FAT options";
            // 
            // syncLabelsCheckBox
            // 
            syncLabelsCheckBox.AutoSize = true;
            syncLabelsCheckBox.Checked = true;
            syncLabelsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            syncLabelsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            syncLabelsCheckBox.Location = new System.Drawing.Point(6, 48);
            syncLabelsCheckBox.Name = "syncLabelsCheckBox";
            syncLabelsCheckBox.Size = new System.Drawing.Size(241, 20);
            syncLabelsCheckBox.TabIndex = 9;
            syncLabelsCheckBox.Text = "Write the same label as in root directory";
            // 
            // writeBpbLabelCheckBox
            // 
            writeBpbLabelCheckBox.AutoSize = true;
            writeBpbLabelCheckBox.Checked = true;
            writeBpbLabelCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            writeBpbLabelCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            writeBpbLabelCheckBox.Location = new System.Drawing.Point(6, 22);
            writeBpbLabelCheckBox.Name = "writeBpbLabelCheckBox";
            writeBpbLabelCheckBox.Size = new System.Drawing.Size(233, 20);
            writeBpbLabelCheckBox.TabIndex = 7;
            writeBpbLabelCheckBox.Text = "Also write the volume label to the BPB";
            // 
            // bpbLabelTextBox
            // 
            bpbLabelTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            bpbLabelTextBox.Location = new System.Drawing.Point(6, 74);
            bpbLabelTextBox.MaxLength = 11;
            bpbLabelTextBox.Name = "bpbLabelTextBox";
            bpbLabelTextBox.ReadOnly = true;
            bpbLabelTextBox.Size = new System.Drawing.Size(248, 23);
            bpbLabelTextBox.TabIndex = 8;
            // 
            // ChangeVolumeLabelDialog
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(284, 221);
            Controls.Add(buttonPanel);
            Controls.Add(rootDirectoryLabelTextBox);
            Controls.Add(rootDirectoryLabelLabel);
            Controls.Add(fatOptionsGroupBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ChangeVolumeLabelDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Change volume label";
            buttonPanel.ResumeLayout(false);
            fatOptionsGroupBox.ResumeLayout(false);
            fatOptionsGroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox rootDirectoryLabelTextBox;
        private System.Windows.Forms.Label rootDirectoryLabelLabel;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.GroupBox fatOptionsGroupBox;
        private System.Windows.Forms.CheckBox syncLabelsCheckBox;
        private System.Windows.Forms.CheckBox writeBpbLabelCheckBox;
        private System.Windows.Forms.TextBox bpbLabelTextBox;
    }
}
