namespace TotalImage.Dialogs
{
    partial class ImageInfoDialog
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
            components = new System.ComponentModel.Container();
            okButton = new System.Windows.Forms.Button();
            buttonPanel = new System.Windows.Forms.Panel();
            exportButton = new System.Windows.Forms.Button();
            commentLabel = new System.Windows.Forms.Label();
            commentTextBox = new System.Windows.Forms.TextBox();
            copyContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            copyValueMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tabControl = new System.Windows.Forms.TabControl();
            fileTabPage = new System.Windows.Forms.TabPage();
            fileListView = new System.Windows.Forms.ListView();
            fileNameColumn = new System.Windows.Forms.ColumnHeader();
            fileValueColumn = new System.Windows.Forms.ColumnHeader();
            containerTabPage = new System.Windows.Forms.TabPage();
            containerListView = new System.Windows.Forms.ListView();
            containerNameColumn = new System.Windows.Forms.ColumnHeader();
            containerValueColumn = new System.Windows.Forms.ColumnHeader();
            partitionTableTabPage = new System.Windows.Forms.TabPage();
            partitionTableListView = new System.Windows.Forms.ListView();
            partitionTableNameColumn = new System.Windows.Forms.ColumnHeader();
            partitionTableValueColumn = new System.Windows.Forms.ColumnHeader();
            partitionTabPage = new System.Windows.Forms.TabPage();
            partitionListView = new System.Windows.Forms.ListView();
            partitionNameColumn = new System.Windows.Forms.ColumnHeader();
            partitionValueColumn = new System.Windows.Forms.ColumnHeader();
            fileSystemTabPage = new System.Windows.Forms.TabPage();
            fileSystemListView = new System.Windows.Forms.ListView();
            fileSystemNameColumn = new System.Windows.Forms.ColumnHeader();
            fileSystemValueColumn = new System.Windows.Forms.ColumnHeader();
            buttonPanel.SuspendLayout();
            copyContextMenu.SuspendLayout();
            tabControl.SuspendLayout();
            fileTabPage.SuspendLayout();
            containerTabPage.SuspendLayout();
            partitionTableTabPage.SuspendLayout();
            partitionTabPage.SuspendLayout();
            fileSystemTabPage.SuspendLayout();
            SuspendLayout();
            // 
            // okButton
            // 
            okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Location = new System.Drawing.Point(381, 12);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(80, 26);
            okButton.TabIndex = 1;
            okButton.Text = "OK";
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = System.Drawing.SystemColors.Control;
            buttonPanel.Controls.Add(exportButton);
            buttonPanel.Controls.Add(okButton);
            buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonPanel.Location = new System.Drawing.Point(0, 393);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(473, 50);
            buttonPanel.TabIndex = 22;
            // 
            // exportButton
            // 
            exportButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            exportButton.Location = new System.Drawing.Point(12, 12);
            exportButton.Name = "exportButton";
            exportButton.Size = new System.Drawing.Size(80, 26);
            exportButton.TabIndex = 2;
            exportButton.Text = "Save...";
            // 
            // commentLabel
            // 
            commentLabel.AutoSize = true;
            commentLabel.Location = new System.Drawing.Point(6, 232);
            commentLabel.Name = "commentLabel";
            commentLabel.Size = new System.Drawing.Size(64, 15);
            commentLabel.TabIndex = 23;
            commentLabel.Text = "Comment:";
            // 
            // commentTextBox
            // 
            commentTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            commentTextBox.Location = new System.Drawing.Point(6, 250);
            commentTextBox.Multiline = true;
            commentTextBox.Name = "commentTextBox";
            commentTextBox.ReadOnly = true;
            commentTextBox.Size = new System.Drawing.Size(429, 91);
            commentTextBox.TabIndex = 1;
            commentTextBox.Text = "This container type does not support comments.";
            // 
            // copyContextMenu
            // 
            copyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { copyValueMenuItem });
            copyContextMenu.Name = "copyContextMenu";
            copyContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            copyContextMenu.Size = new System.Drawing.Size(134, 26);
            copyContextMenu.Opening += CopyContextMenu_Opening;
            // 
            // copyValueMenuItem
            // 
            copyValueMenuItem.Image = Properties.Resources.page_white_copy;
            copyValueMenuItem.Name = "copyValueMenuItem";
            copyValueMenuItem.Size = new System.Drawing.Size(133, 22);
            copyValueMenuItem.Text = "Copy value";
            copyValueMenuItem.Click += CopyValueMenuItem_Click;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(fileTabPage);
            tabControl.Controls.Add(containerTabPage);
            tabControl.Controls.Add(partitionTableTabPage);
            tabControl.Controls.Add(partitionTabPage);
            tabControl.Controls.Add(fileSystemTabPage);
            tabControl.Location = new System.Drawing.Point(12, 12);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(449, 375);
            tabControl.TabIndex = 24;
            // 
            // fileTabPage
            // 
            fileTabPage.BackColor = System.Drawing.SystemColors.Window;
            fileTabPage.Controls.Add(fileListView);
            fileTabPage.Location = new System.Drawing.Point(4, 24);
            fileTabPage.Name = "fileTabPage";
            fileTabPage.Padding = new System.Windows.Forms.Padding(3);
            fileTabPage.Size = new System.Drawing.Size(441, 347);
            fileTabPage.TabIndex = 0;
            fileTabPage.Text = "File";
            // 
            // fileListView
            // 
            fileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { fileNameColumn, fileValueColumn });
            fileListView.ContextMenuStrip = copyContextMenu;
            fileListView.FullRowSelect = true;
            fileListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            fileListView.LabelWrap = false;
            fileListView.Location = new System.Drawing.Point(6, 6);
            fileListView.MultiSelect = false;
            fileListView.Name = "fileListView";
            fileListView.ShowGroups = false;
            fileListView.ShowItemToolTips = true;
            fileListView.Size = new System.Drawing.Size(429, 335);
            fileListView.TabIndex = 1;
            fileListView.UseCompatibleStateImageBehavior = false;
            fileListView.View = System.Windows.Forms.View.Details;
            // 
            // fileNameColumn
            // 
            fileNameColumn.Text = "Property";
            fileNameColumn.Width = 170;
            // 
            // fileValueColumn
            // 
            fileValueColumn.Text = "Value";
            fileValueColumn.Width = 250;
            // 
            // containerTabPage
            // 
            containerTabPage.BackColor = System.Drawing.SystemColors.Window;
            containerTabPage.Controls.Add(containerListView);
            containerTabPage.Controls.Add(commentLabel);
            containerTabPage.Controls.Add(commentTextBox);
            containerTabPage.Location = new System.Drawing.Point(4, 24);
            containerTabPage.Name = "containerTabPage";
            containerTabPage.Padding = new System.Windows.Forms.Padding(3);
            containerTabPage.Size = new System.Drawing.Size(441, 347);
            containerTabPage.TabIndex = 1;
            containerTabPage.Text = "Container";
            // 
            // containerListView
            // 
            containerListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { containerNameColumn, containerValueColumn });
            containerListView.ContextMenuStrip = copyContextMenu;
            containerListView.FullRowSelect = true;
            containerListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            containerListView.LabelWrap = false;
            containerListView.Location = new System.Drawing.Point(6, 6);
            containerListView.MultiSelect = false;
            containerListView.Name = "containerListView";
            containerListView.ShowGroups = false;
            containerListView.ShowItemToolTips = true;
            containerListView.Size = new System.Drawing.Size(429, 213);
            containerListView.TabIndex = 24;
            containerListView.UseCompatibleStateImageBehavior = false;
            containerListView.View = System.Windows.Forms.View.Details;
            // 
            // containerNameColumn
            // 
            containerNameColumn.Text = "Property";
            containerNameColumn.Width = 170;
            // 
            // containerValueColumn
            // 
            containerValueColumn.Text = "Value";
            containerValueColumn.Width = 250;
            // 
            // partitionTableTabPage
            // 
            partitionTableTabPage.BackColor = System.Drawing.SystemColors.Window;
            partitionTableTabPage.Controls.Add(partitionTableListView);
            partitionTableTabPage.Location = new System.Drawing.Point(4, 24);
            partitionTableTabPage.Name = "partitionTableTabPage";
            partitionTableTabPage.Padding = new System.Windows.Forms.Padding(3);
            partitionTableTabPage.Size = new System.Drawing.Size(441, 347);
            partitionTableTabPage.TabIndex = 2;
            partitionTableTabPage.Text = "Partition table";
            // 
            // partitionTableListView
            // 
            partitionTableListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { partitionTableNameColumn, partitionTableValueColumn });
            partitionTableListView.ContextMenuStrip = copyContextMenu;
            partitionTableListView.FullRowSelect = true;
            partitionTableListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            partitionTableListView.LabelWrap = false;
            partitionTableListView.Location = new System.Drawing.Point(6, 6);
            partitionTableListView.MultiSelect = false;
            partitionTableListView.Name = "partitionTableListView";
            partitionTableListView.ShowGroups = false;
            partitionTableListView.ShowItemToolTips = true;
            partitionTableListView.Size = new System.Drawing.Size(429, 335);
            partitionTableListView.TabIndex = 1;
            partitionTableListView.UseCompatibleStateImageBehavior = false;
            partitionTableListView.View = System.Windows.Forms.View.Details;
            // 
            // partitionTableNameColumn
            // 
            partitionTableNameColumn.Text = "Property";
            partitionTableNameColumn.Width = 170;
            // 
            // partitionTableValueColumn
            // 
            partitionTableValueColumn.Text = "Value";
            partitionTableValueColumn.Width = 250;
            // 
            // partitionTabPage
            // 
            partitionTabPage.BackColor = System.Drawing.SystemColors.Window;
            partitionTabPage.Controls.Add(partitionListView);
            partitionTabPage.Location = new System.Drawing.Point(4, 24);
            partitionTabPage.Name = "partitionTabPage";
            partitionTabPage.Padding = new System.Windows.Forms.Padding(3);
            partitionTabPage.Size = new System.Drawing.Size(441, 347);
            partitionTabPage.TabIndex = 3;
            partitionTabPage.Text = "Selected partition";
            // 
            // partitionListView
            // 
            partitionListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { partitionNameColumn, partitionValueColumn });
            partitionListView.ContextMenuStrip = copyContextMenu;
            partitionListView.FullRowSelect = true;
            partitionListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            partitionListView.LabelWrap = false;
            partitionListView.Location = new System.Drawing.Point(6, 6);
            partitionListView.MultiSelect = false;
            partitionListView.Name = "partitionListView";
            partitionListView.ShowGroups = false;
            partitionListView.ShowItemToolTips = true;
            partitionListView.Size = new System.Drawing.Size(429, 335);
            partitionListView.TabIndex = 1;
            partitionListView.UseCompatibleStateImageBehavior = false;
            partitionListView.View = System.Windows.Forms.View.Details;
            // 
            // partitionNameColumn
            // 
            partitionNameColumn.Text = "Property";
            partitionNameColumn.Width = 170;
            // 
            // partitionValueColumn
            // 
            partitionValueColumn.Text = "Value";
            partitionValueColumn.Width = 250;
            // 
            // fileSystemTabPage
            // 
            fileSystemTabPage.BackColor = System.Drawing.SystemColors.Window;
            fileSystemTabPage.Controls.Add(fileSystemListView);
            fileSystemTabPage.Location = new System.Drawing.Point(4, 24);
            fileSystemTabPage.Name = "fileSystemTabPage";
            fileSystemTabPage.Padding = new System.Windows.Forms.Padding(3);
            fileSystemTabPage.Size = new System.Drawing.Size(441, 347);
            fileSystemTabPage.TabIndex = 4;
            fileSystemTabPage.Text = "File system";
            // 
            // fileSystemListView
            // 
            fileSystemListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { fileSystemNameColumn, fileSystemValueColumn });
            fileSystemListView.ContextMenuStrip = copyContextMenu;
            fileSystemListView.FullRowSelect = true;
            fileSystemListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            fileSystemListView.LabelWrap = false;
            fileSystemListView.Location = new System.Drawing.Point(6, 6);
            fileSystemListView.MultiSelect = false;
            fileSystemListView.Name = "fileSystemListView";
            fileSystemListView.ShowGroups = false;
            fileSystemListView.ShowItemToolTips = true;
            fileSystemListView.Size = new System.Drawing.Size(429, 335);
            fileSystemListView.TabIndex = 24;
            fileSystemListView.UseCompatibleStateImageBehavior = false;
            fileSystemListView.View = System.Windows.Forms.View.Details;
            // 
            // fileSystemNameColumn
            // 
            fileSystemNameColumn.Text = "Property";
            fileSystemNameColumn.Width = 170;
            // 
            // fileSystemValueColumn
            // 
            fileSystemValueColumn.Text = "Value";
            fileSystemValueColumn.Width = 250;
            // 
            // ImageInfoDialog
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = okButton;
            ClientSize = new System.Drawing.Size(473, 443);
            Controls.Add(tabControl);
            Controls.Add(buttonPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ImageInfoDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Image information";
            buttonPanel.ResumeLayout(false);
            copyContextMenu.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            fileTabPage.ResumeLayout(false);
            containerTabPage.ResumeLayout(false);
            containerTabPage.PerformLayout();
            partitionTableTabPage.ResumeLayout(false);
            partitionTabPage.ResumeLayout(false);
            fileSystemTabPage.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Label commentLabel;
        private System.Windows.Forms.TextBox commentTextBox;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.ContextMenuStrip copyContextMenu;
        private System.Windows.Forms.ToolStripMenuItem copyValueMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage fileTabPage;
        private System.Windows.Forms.TabPage containerTabPage;
        private System.Windows.Forms.TabPage partitionTableTabPage;
        private System.Windows.Forms.TabPage partitionTabPage;
        private System.Windows.Forms.TabPage fileSystemTabPage;
        private System.Windows.Forms.ListView fileListView;
        private System.Windows.Forms.ColumnHeader fileNameColumn;
        private System.Windows.Forms.ColumnHeader fileValueColumn;
        private System.Windows.Forms.ListView containerListView;
        private System.Windows.Forms.ColumnHeader containerNameColumn;
        private System.Windows.Forms.ColumnHeader containerValueColumn;
        private System.Windows.Forms.ListView fileSystemListView;
        private System.Windows.Forms.ListView partitionTableListView;
        private System.Windows.Forms.ColumnHeader partitionTableNameColumn;
        private System.Windows.Forms.ColumnHeader partitionTableValueColumn;
        private System.Windows.Forms.ListView partitionListView;
        private System.Windows.Forms.ColumnHeader partitionNameColumn;
        private System.Windows.Forms.ColumnHeader partitionValueColumn;
        private System.Windows.Forms.ColumnHeader fileSystemNameColumn;
        private System.Windows.Forms.ColumnHeader fileSystemValueColumn;
    }
}
