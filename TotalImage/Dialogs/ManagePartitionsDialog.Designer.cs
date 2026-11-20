using TotalImage.Controls;

namespace TotalImage.Dialogs
{
    partial class ManagePartitionsDialog
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
            partitionsListView = new ListViewEx();
            numberColumnHeader = new System.Windows.Forms.ColumnHeader();
            labelColumnHeader = new System.Windows.Forms.ColumnHeader();
            typeColumnHeader = new System.Windows.Forms.ColumnHeader();
            startColumnHeader = new System.Windows.Forms.ColumnHeader();
            endColumnHeader = new System.Windows.Forms.ColumnHeader();
            sizeColumnHeader = new System.Windows.Forms.ColumnHeader();
            activeColumnHeader = new System.Windows.Forms.ColumnHeader();
            createButton = new System.Windows.Forms.Button();
            formatButton = new System.Windows.Forms.Button();
            resizeButton = new System.Windows.Forms.Button();
            deleteButton = new System.Windows.Forms.Button();
            activeButton = new System.Windows.Forms.Button();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // okButton
            // 
            okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Location = new System.Drawing.Point(406, 12);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(80, 26);
            okButton.TabIndex = 0;
            okButton.Text = "OK";
            // 
            // cancelButton
            // 
            cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(492, 12);
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
            buttonPanel.Location = new System.Drawing.Point(0, 231);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(584, 50);
            buttonPanel.TabIndex = 2;
            // 
            // partitionsListView
            // 
            partitionsListView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            partitionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { numberColumnHeader, labelColumnHeader, typeColumnHeader, startColumnHeader, endColumnHeader, sizeColumnHeader, activeColumnHeader });
            partitionsListView.FullRowSelect = true;
            partitionsListView.GridLines = true;
            partitionsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            partitionsListView.Location = new System.Drawing.Point(12, 12);
            partitionsListView.MultiSelect = false;
            partitionsListView.Name = "partitionsListView";
            partitionsListView.ShowGroups = false;
            partitionsListView.Size = new System.Drawing.Size(560, 181);
            partitionsListView.TabIndex = 7;
            partitionsListView.UseCompatibleStateImageBehavior = false;
            partitionsListView.View = System.Windows.Forms.View.Details;
            // 
            // numberColumnHeader
            // 
            numberColumnHeader.Text = "No.";
            numberColumnHeader.Width = 35;
            // 
            // labelColumnHeader
            // 
            labelColumnHeader.Text = "Volume label";
            labelColumnHeader.Width = 100;
            // 
            // typeColumnHeader
            // 
            typeColumnHeader.Text = "Type";
            typeColumnHeader.Width = 90;
            // 
            // startColumnHeader
            // 
            startColumnHeader.Text = "Start";
            startColumnHeader.Width = 80;
            // 
            // endColumnHeader
            // 
            endColumnHeader.Text = "End";
            endColumnHeader.Width = 95;
            // 
            // sizeColumnHeader
            // 
            sizeColumnHeader.Text = "Size";
            sizeColumnHeader.Width = 90;
            // 
            // activeColumnHeader
            // 
            activeColumnHeader.Text = "Active";
            activeColumnHeader.Width = 55;
            // 
            // createButton
            // 
            createButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            createButton.Location = new System.Drawing.Point(12, 199);
            createButton.Name = "createButton";
            createButton.Size = new System.Drawing.Size(80, 26);
            createButton.TabIndex = 2;
            createButton.Text = "Create...";
            createButton.UseVisualStyleBackColor = true;
            // 
            // formatButton
            // 
            formatButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            formatButton.Enabled = false;
            formatButton.Location = new System.Drawing.Point(184, 199);
            formatButton.Name = "formatButton";
            formatButton.Size = new System.Drawing.Size(80, 26);
            formatButton.TabIndex = 8;
            formatButton.Text = "Format...";
            formatButton.UseVisualStyleBackColor = true;
            // 
            // resizeButton
            // 
            resizeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            resizeButton.Enabled = false;
            resizeButton.Location = new System.Drawing.Point(270, 199);
            resizeButton.Name = "resizeButton";
            resizeButton.Size = new System.Drawing.Size(80, 26);
            resizeButton.TabIndex = 9;
            resizeButton.Text = "Resize...";
            resizeButton.UseVisualStyleBackColor = true;
            // 
            // deleteButton
            // 
            deleteButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            deleteButton.Enabled = false;
            deleteButton.Location = new System.Drawing.Point(356, 199);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new System.Drawing.Size(80, 26);
            deleteButton.TabIndex = 10;
            deleteButton.Text = "Delete";
            deleteButton.UseVisualStyleBackColor = true;
            // 
            // activeButton
            // 
            activeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            activeButton.Enabled = false;
            activeButton.Location = new System.Drawing.Point(98, 199);
            activeButton.Name = "activeButton";
            activeButton.Size = new System.Drawing.Size(80, 26);
            activeButton.TabIndex = 11;
            activeButton.Text = "Active";
            activeButton.UseVisualStyleBackColor = true;
            // 
            // ManagePartitionsDialog
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(584, 281);
            Controls.Add(activeButton);
            Controls.Add(deleteButton);
            Controls.Add(resizeButton);
            Controls.Add(formatButton);
            Controls.Add(createButton);
            Controls.Add(partitionsListView);
            Controls.Add(buttonPanel);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ManagePartitionsDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Manage partitions";
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel buttonPanel;
        private ListViewEx partitionsListView;
        private System.Windows.Forms.ColumnHeader numberColumnHeader;
        private System.Windows.Forms.ColumnHeader labelColumnHeader;
        private System.Windows.Forms.ColumnHeader typeColumnHeader;
        private System.Windows.Forms.ColumnHeader startColumnHeader;
        private System.Windows.Forms.ColumnHeader endColumnHeader;
        private System.Windows.Forms.ColumnHeader sizeColumnHeader;
        private System.Windows.Forms.ColumnHeader activeColumnHeader;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button formatButton;
        private System.Windows.Forms.Button resizeButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button activeButton;
    }
}
