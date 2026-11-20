using TotalImage.Controls;

namespace TotalImage.Dialogs
{
    partial class SelectPartitionDialog
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
            descriptionLabel = new System.Windows.Forms.Label();
            buttonPanel = new System.Windows.Forms.Panel();
            cancelButton = new System.Windows.Forms.Button();
            okButton = new System.Windows.Forms.Button();
            readOnlyCheckBox = new System.Windows.Forms.CheckBox();
            partitionsListView = new ListViewEx();
            numberColumnHeader = new System.Windows.Forms.ColumnHeader();
            fileSystemColumnHeader = new System.Windows.Forms.ColumnHeader();
            volumeLabelColumnHeader = new System.Windows.Forms.ColumnHeader();
            sizeColumnHeader = new System.Windows.Forms.ColumnHeader();
            flagColumnHeader = new System.Windows.Forms.ColumnHeader();
            partitionTableLabel = new System.Windows.Forms.Label();
            diskTotalSizeLabel = new System.Windows.Forms.Label();
            diskTotalSizeValueLabel = new System.Windows.Forms.Label();
            diskInfoGroupBox = new System.Windows.Forms.GroupBox();
            diskGuidValueLabel = new System.Windows.Forms.Label();
            diskTotalSectorsLabel = new System.Windows.Forms.Label();
            diskGuidLabel = new System.Windows.Forms.Label();
            diskSerialValueLabel = new System.Windows.Forms.Label();
            diskSerialLabel = new System.Windows.Forms.Label();
            diskTimestampValueLabel = new System.Windows.Forms.Label();
            diskTimestampLabel = new System.Windows.Forms.Label();
            partitionTableValueLabel = new System.Windows.Forms.Label();
            diskUsableSizeValueLabel = new System.Windows.Forms.Label();
            diskUsableSizeLabel = new System.Windows.Forms.Label();
            diskTotalSectorsValueLabel = new System.Windows.Forms.Label();
            diskTypeValueLabel = new System.Windows.Forms.Label();
            diskTypeLabel = new System.Windows.Forms.Label();
            partitionInfoGroupBox = new System.Windows.Forms.GroupBox();
            partitionEndOffsetValueLabel = new System.Windows.Forms.Label();
            partitionEndOffsetLabel = new System.Windows.Forms.Label();
            partitionStartOffsetValueLabel = new System.Windows.Forms.Label();
            partitionStartOffsetLabel = new System.Windows.Forms.Label();
            partitionGuidValueLabel = new System.Windows.Forms.Label();
            partitionGuidLabel = new System.Windows.Forms.Label();
            partitionTotalSectorsValueLabel = new System.Windows.Forms.Label();
            partitionTotalSectorsLabel = new System.Windows.Forms.Label();
            partitionSerialValueLabel = new System.Windows.Forms.Label();
            partitionSerialLabel = new System.Windows.Forms.Label();
            partitionLastLbaValueLabel = new System.Windows.Forms.Label();
            partitionLastLbaLabel = new System.Windows.Forms.Label();
            partitionFirstLbaValueLabel = new System.Windows.Forms.Label();
            partitionFirstLbaLabel = new System.Windows.Forms.Label();
            partitionTypeLabel = new System.Windows.Forms.Label();
            partitionTypeValueLabel = new System.Windows.Forms.Label();
            loadOptionsGroupBox = new System.Windows.Forms.GroupBox();
            buttonPanel.SuspendLayout();
            diskInfoGroupBox.SuspendLayout();
            partitionInfoGroupBox.SuspendLayout();
            loadOptionsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new System.Drawing.Point(10, 7);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new System.Drawing.Size(138, 15);
            descriptionLabel.TabIndex = 0;
            descriptionLabel.Text = "Select a partition to load:";
            // 
            // buttonPanel
            // 
            buttonPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            buttonPanel.BackColor = System.Drawing.SystemColors.Control;
            buttonPanel.Controls.Add(cancelButton);
            buttonPanel.Controls.Add(okButton);
            buttonPanel.Location = new System.Drawing.Point(0, 421);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(486, 50);
            buttonPanel.TabIndex = 3;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(395, 14);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(80, 26);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Enabled = false;
            okButton.Location = new System.Drawing.Point(309, 14);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(80, 26);
            okButton.TabIndex = 0;
            okButton.Text = "OK";
            // 
            // readOnlyCheckBox
            // 
            readOnlyCheckBox.AutoSize = true;
            readOnlyCheckBox.Enabled = false;
            readOnlyCheckBox.Location = new System.Drawing.Point(9, 23);
            readOnlyCheckBox.Name = "readOnlyCheckBox";
            readOnlyCheckBox.Size = new System.Drawing.Size(120, 19);
            readOnlyCheckBox.TabIndex = 5;
            readOnlyCheckBox.Text = "Load as read-only";
            // 
            // partitionsListView
            // 
            partitionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { numberColumnHeader, fileSystemColumnHeader, volumeLabelColumnHeader, sizeColumnHeader, flagColumnHeader });
            partitionsListView.FullRowSelect = true;
            partitionsListView.GridLines = true;
            partitionsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            partitionsListView.Location = new System.Drawing.Point(10, 30);
            partitionsListView.MultiSelect = false;
            partitionsListView.Name = "partitionsListView";
            partitionsListView.ShowGroups = false;
            partitionsListView.Size = new System.Drawing.Size(467, 141);
            partitionsListView.TabIndex = 6;
            partitionsListView.UseCompatibleStateImageBehavior = false;
            partitionsListView.View = System.Windows.Forms.View.Details;
            partitionsListView.MouseDoubleClick += partitionsListView_MouseDoubleClick;
            // 
            // numberColumnHeader
            // 
            numberColumnHeader.Text = "No.";
            numberColumnHeader.Width = 35;
            // 
            // fileSystemColumnHeader
            // 
            fileSystemColumnHeader.Text = "File system";
            fileSystemColumnHeader.Width = 110;
            // 
            // volumeLabelColumnHeader
            // 
            volumeLabelColumnHeader.Text = "Volume label";
            volumeLabelColumnHeader.Width = 140;
            // 
            // sizeColumnHeader
            // 
            sizeColumnHeader.Text = "Size";
            sizeColumnHeader.Width = 110;
            // 
            // flagColumnHeader
            // 
            flagColumnHeader.Text = "Active";
            flagColumnHeader.Width = 65;
            // 
            // partitionTableLabel
            // 
            partitionTableLabel.AutoSize = true;
            partitionTableLabel.Location = new System.Drawing.Point(5, 37);
            partitionTableLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionTableLabel.Name = "partitionTableLabel";
            partitionTableLabel.Size = new System.Drawing.Size(84, 15);
            partitionTableLabel.TabIndex = 7;
            partitionTableLabel.Text = "Partition table:";
            // 
            // diskTotalSizeLabel
            // 
            diskTotalSizeLabel.AutoSize = true;
            diskTotalSizeLabel.Location = new System.Drawing.Point(5, 129);
            diskTotalSizeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            diskTotalSizeLabel.Name = "diskTotalSizeLabel";
            diskTotalSizeLabel.Size = new System.Drawing.Size(58, 15);
            diskTotalSizeLabel.TabIndex = 9;
            diskTotalSizeLabel.Text = "Total size:";
            // 
            // diskTotalSizeValueLabel
            // 
            diskTotalSizeValueLabel.AutoEllipsis = true;
            diskTotalSizeValueLabel.Location = new System.Drawing.Point(94, 129);
            diskTotalSizeValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            diskTotalSizeValueLabel.Name = "diskTotalSizeValueLabel";
            diskTotalSizeValueLabel.Size = new System.Drawing.Size(132, 16);
            diskTotalSizeValueLabel.TabIndex = 10;
            diskTotalSizeValueLabel.Text = "<total size>";
            // 
            // diskInfoGroupBox
            // 
            diskInfoGroupBox.Controls.Add(diskGuidValueLabel);
            diskInfoGroupBox.Controls.Add(diskTotalSectorsLabel);
            diskInfoGroupBox.Controls.Add(diskGuidLabel);
            diskInfoGroupBox.Controls.Add(diskSerialValueLabel);
            diskInfoGroupBox.Controls.Add(diskSerialLabel);
            diskInfoGroupBox.Controls.Add(diskTimestampValueLabel);
            diskInfoGroupBox.Controls.Add(diskTimestampLabel);
            diskInfoGroupBox.Controls.Add(partitionTableValueLabel);
            diskInfoGroupBox.Controls.Add(diskUsableSizeValueLabel);
            diskInfoGroupBox.Controls.Add(diskUsableSizeLabel);
            diskInfoGroupBox.Controls.Add(diskTotalSectorsValueLabel);
            diskInfoGroupBox.Controls.Add(diskTypeValueLabel);
            diskInfoGroupBox.Controls.Add(diskTypeLabel);
            diskInfoGroupBox.Controls.Add(partitionTableLabel);
            diskInfoGroupBox.Controls.Add(diskTotalSizeValueLabel);
            diskInfoGroupBox.Controls.Add(diskTotalSizeLabel);
            diskInfoGroupBox.Location = new System.Drawing.Point(10, 176);
            diskInfoGroupBox.Margin = new System.Windows.Forms.Padding(2);
            diskInfoGroupBox.Name = "diskInfoGroupBox";
            diskInfoGroupBox.Padding = new System.Windows.Forms.Padding(2);
            diskInfoGroupBox.Size = new System.Drawing.Size(230, 172);
            diskInfoGroupBox.TabIndex = 11;
            diskInfoGroupBox.TabStop = false;
            diskInfoGroupBox.Text = "Disk information";
            // 
            // diskGuidValueLabel
            // 
            diskGuidValueLabel.AutoEllipsis = true;
            diskGuidValueLabel.Location = new System.Drawing.Point(94, 92);
            diskGuidValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            diskGuidValueLabel.Name = "diskGuidValueLabel";
            diskGuidValueLabel.Size = new System.Drawing.Size(132, 16);
            diskGuidValueLabel.TabIndex = 26;
            diskGuidValueLabel.Text = "<gpt guid>";
            // 
            // diskTotalSectorsLabel
            // 
            diskTotalSectorsLabel.AutoSize = true;
            diskTotalSectorsLabel.Location = new System.Drawing.Point(5, 148);
            diskTotalSectorsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            diskTotalSectorsLabel.Name = "diskTotalSectorsLabel";
            diskTotalSectorsLabel.Size = new System.Drawing.Size(76, 15);
            diskTotalSectorsLabel.TabIndex = 17;
            diskTotalSectorsLabel.Text = "Total sectors:";
            // 
            // diskGuidLabel
            // 
            diskGuidLabel.AutoSize = true;
            diskGuidLabel.Location = new System.Drawing.Point(5, 92);
            diskGuidLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            diskGuidLabel.Name = "diskGuidLabel";
            diskGuidLabel.Size = new System.Drawing.Size(62, 15);
            diskGuidLabel.TabIndex = 25;
            diskGuidLabel.Text = "GPT GUID:";
            // 
            // diskSerialValueLabel
            // 
            diskSerialValueLabel.AutoEllipsis = true;
            diskSerialValueLabel.Location = new System.Drawing.Point(94, 74);
            diskSerialValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            diskSerialValueLabel.Name = "diskSerialValueLabel";
            diskSerialValueLabel.Size = new System.Drawing.Size(132, 16);
            diskSerialValueLabel.TabIndex = 24;
            diskSerialValueLabel.Text = "<serial number>";
            // 
            // diskSerialLabel
            // 
            diskSerialLabel.AutoSize = true;
            diskSerialLabel.Location = new System.Drawing.Point(5, 74);
            diskSerialLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            diskSerialLabel.Name = "diskSerialLabel";
            diskSerialLabel.Size = new System.Drawing.Size(83, 15);
            diskSerialLabel.TabIndex = 23;
            diskSerialLabel.Text = "Serial number:";
            // 
            // diskTimestampValueLabel
            // 
            diskTimestampValueLabel.AutoEllipsis = true;
            diskTimestampValueLabel.Location = new System.Drawing.Point(94, 55);
            diskTimestampValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            diskTimestampValueLabel.Name = "diskTimestampValueLabel";
            diskTimestampValueLabel.Size = new System.Drawing.Size(132, 16);
            diskTimestampValueLabel.TabIndex = 22;
            diskTimestampValueLabel.Text = "<timestamp>";
            // 
            // diskTimestampLabel
            // 
            diskTimestampLabel.AutoSize = true;
            diskTimestampLabel.Location = new System.Drawing.Point(5, 55);
            diskTimestampLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            diskTimestampLabel.Name = "diskTimestampLabel";
            diskTimestampLabel.Size = new System.Drawing.Size(70, 15);
            diskTimestampLabel.TabIndex = 21;
            diskTimestampLabel.Text = "Timestamp:";
            // 
            // partitionTableValueLabel
            // 
            partitionTableValueLabel.AutoEllipsis = true;
            partitionTableValueLabel.Location = new System.Drawing.Point(94, 37);
            partitionTableValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionTableValueLabel.Name = "partitionTableValueLabel";
            partitionTableValueLabel.Size = new System.Drawing.Size(132, 16);
            partitionTableValueLabel.TabIndex = 20;
            partitionTableValueLabel.Text = "<partition table>";
            // 
            // diskUsableSizeValueLabel
            // 
            diskUsableSizeValueLabel.AutoEllipsis = true;
            diskUsableSizeValueLabel.Location = new System.Drawing.Point(94, 110);
            diskUsableSizeValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            diskUsableSizeValueLabel.Name = "diskUsableSizeValueLabel";
            diskUsableSizeValueLabel.Size = new System.Drawing.Size(132, 16);
            diskUsableSizeValueLabel.TabIndex = 19;
            diskUsableSizeValueLabel.Text = "<usable size>";
            // 
            // diskUsableSizeLabel
            // 
            diskUsableSizeLabel.AutoSize = true;
            diskUsableSizeLabel.Location = new System.Drawing.Point(5, 110);
            diskUsableSizeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            diskUsableSizeLabel.Name = "diskUsableSizeLabel";
            diskUsableSizeLabel.Size = new System.Drawing.Size(67, 15);
            diskUsableSizeLabel.TabIndex = 18;
            diskUsableSizeLabel.Text = "Usable size:";
            // 
            // diskTotalSectorsValueLabel
            // 
            diskTotalSectorsValueLabel.AutoEllipsis = true;
            diskTotalSectorsValueLabel.Location = new System.Drawing.Point(94, 148);
            diskTotalSectorsValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            diskTotalSectorsValueLabel.Name = "diskTotalSectorsValueLabel";
            diskTotalSectorsValueLabel.Size = new System.Drawing.Size(132, 16);
            diskTotalSectorsValueLabel.TabIndex = 16;
            diskTotalSectorsValueLabel.Text = "<total sectors>";
            // 
            // diskTypeValueLabel
            // 
            diskTypeValueLabel.AutoEllipsis = true;
            diskTypeValueLabel.Location = new System.Drawing.Point(94, 18);
            diskTypeValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            diskTypeValueLabel.Name = "diskTypeValueLabel";
            diskTypeValueLabel.Size = new System.Drawing.Size(132, 16);
            diskTypeValueLabel.TabIndex = 12;
            diskTypeValueLabel.Text = "<disk type>";
            // 
            // diskTypeLabel
            // 
            diskTypeLabel.AutoSize = true;
            diskTypeLabel.Location = new System.Drawing.Point(5, 18);
            diskTypeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            diskTypeLabel.Name = "diskTypeLabel";
            diskTypeLabel.Size = new System.Drawing.Size(58, 15);
            diskTypeLabel.TabIndex = 11;
            diskTypeLabel.Text = "Disk type:";
            // 
            // partitionInfoGroupBox
            // 
            partitionInfoGroupBox.Controls.Add(partitionEndOffsetValueLabel);
            partitionInfoGroupBox.Controls.Add(partitionEndOffsetLabel);
            partitionInfoGroupBox.Controls.Add(partitionStartOffsetValueLabel);
            partitionInfoGroupBox.Controls.Add(partitionStartOffsetLabel);
            partitionInfoGroupBox.Controls.Add(partitionGuidValueLabel);
            partitionInfoGroupBox.Controls.Add(partitionGuidLabel);
            partitionInfoGroupBox.Controls.Add(partitionTotalSectorsValueLabel);
            partitionInfoGroupBox.Controls.Add(partitionTotalSectorsLabel);
            partitionInfoGroupBox.Controls.Add(partitionSerialValueLabel);
            partitionInfoGroupBox.Controls.Add(partitionSerialLabel);
            partitionInfoGroupBox.Controls.Add(partitionLastLbaValueLabel);
            partitionInfoGroupBox.Controls.Add(partitionLastLbaLabel);
            partitionInfoGroupBox.Controls.Add(partitionFirstLbaValueLabel);
            partitionInfoGroupBox.Controls.Add(partitionFirstLbaLabel);
            partitionInfoGroupBox.Controls.Add(partitionTypeLabel);
            partitionInfoGroupBox.Controls.Add(partitionTypeValueLabel);
            partitionInfoGroupBox.Location = new System.Drawing.Point(245, 176);
            partitionInfoGroupBox.Margin = new System.Windows.Forms.Padding(2);
            partitionInfoGroupBox.Name = "partitionInfoGroupBox";
            partitionInfoGroupBox.Padding = new System.Windows.Forms.Padding(2);
            partitionInfoGroupBox.Size = new System.Drawing.Size(231, 172);
            partitionInfoGroupBox.TabIndex = 12;
            partitionInfoGroupBox.TabStop = false;
            partitionInfoGroupBox.Text = "Partition information";
            // 
            // partitionEndOffsetValueLabel
            // 
            partitionEndOffsetValueLabel.AutoEllipsis = true;
            partitionEndOffsetValueLabel.Location = new System.Drawing.Point(93, 129);
            partitionEndOffsetValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionEndOffsetValueLabel.Name = "partitionEndOffsetValueLabel";
            partitionEndOffsetValueLabel.Size = new System.Drawing.Size(134, 16);
            partitionEndOffsetValueLabel.TabIndex = 26;
            partitionEndOffsetValueLabel.Text = "<end offset>";
            // 
            // partitionEndOffsetLabel
            // 
            partitionEndOffsetLabel.AutoSize = true;
            partitionEndOffsetLabel.Location = new System.Drawing.Point(5, 129);
            partitionEndOffsetLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionEndOffsetLabel.Name = "partitionEndOffsetLabel";
            partitionEndOffsetLabel.Size = new System.Drawing.Size(63, 15);
            partitionEndOffsetLabel.TabIndex = 27;
            partitionEndOffsetLabel.Text = "End offset:";
            // 
            // partitionStartOffsetValueLabel
            // 
            partitionStartOffsetValueLabel.AutoEllipsis = true;
            partitionStartOffsetValueLabel.Location = new System.Drawing.Point(93, 92);
            partitionStartOffsetValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionStartOffsetValueLabel.Name = "partitionStartOffsetValueLabel";
            partitionStartOffsetValueLabel.Size = new System.Drawing.Size(134, 16);
            partitionStartOffsetValueLabel.TabIndex = 24;
            partitionStartOffsetValueLabel.Text = "<start offset>";
            // 
            // partitionStartOffsetLabel
            // 
            partitionStartOffsetLabel.AutoSize = true;
            partitionStartOffsetLabel.Location = new System.Drawing.Point(5, 92);
            partitionStartOffsetLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionStartOffsetLabel.Name = "partitionStartOffsetLabel";
            partitionStartOffsetLabel.Size = new System.Drawing.Size(67, 15);
            partitionStartOffsetLabel.TabIndex = 25;
            partitionStartOffsetLabel.Text = "Start offset:";
            // 
            // partitionGuidValueLabel
            // 
            partitionGuidValueLabel.AutoEllipsis = true;
            partitionGuidValueLabel.Location = new System.Drawing.Point(93, 55);
            partitionGuidValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionGuidValueLabel.Name = "partitionGuidValueLabel";
            partitionGuidValueLabel.Size = new System.Drawing.Size(134, 16);
            partitionGuidValueLabel.TabIndex = 22;
            partitionGuidValueLabel.Text = "<gpt guid>";
            // 
            // partitionGuidLabel
            // 
            partitionGuidLabel.AutoSize = true;
            partitionGuidLabel.Location = new System.Drawing.Point(5, 55);
            partitionGuidLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionGuidLabel.Name = "partitionGuidLabel";
            partitionGuidLabel.Size = new System.Drawing.Size(62, 15);
            partitionGuidLabel.TabIndex = 23;
            partitionGuidLabel.Text = "GPT GUID:";
            // 
            // partitionTotalSectorsValueLabel
            // 
            partitionTotalSectorsValueLabel.AutoEllipsis = true;
            partitionTotalSectorsValueLabel.Location = new System.Drawing.Point(93, 148);
            partitionTotalSectorsValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionTotalSectorsValueLabel.Name = "partitionTotalSectorsValueLabel";
            partitionTotalSectorsValueLabel.Size = new System.Drawing.Size(134, 16);
            partitionTotalSectorsValueLabel.TabIndex = 20;
            partitionTotalSectorsValueLabel.Text = "<total sectors>";
            // 
            // partitionTotalSectorsLabel
            // 
            partitionTotalSectorsLabel.AutoSize = true;
            partitionTotalSectorsLabel.Location = new System.Drawing.Point(5, 148);
            partitionTotalSectorsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionTotalSectorsLabel.Name = "partitionTotalSectorsLabel";
            partitionTotalSectorsLabel.Size = new System.Drawing.Size(76, 15);
            partitionTotalSectorsLabel.TabIndex = 21;
            partitionTotalSectorsLabel.Text = "Total sectors:";
            // 
            // partitionSerialValueLabel
            // 
            partitionSerialValueLabel.AutoEllipsis = true;
            partitionSerialValueLabel.Location = new System.Drawing.Point(93, 37);
            partitionSerialValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionSerialValueLabel.Name = "partitionSerialValueLabel";
            partitionSerialValueLabel.Size = new System.Drawing.Size(134, 16);
            partitionSerialValueLabel.TabIndex = 18;
            partitionSerialValueLabel.Text = "<serial number>";
            // 
            // partitionSerialLabel
            // 
            partitionSerialLabel.AutoSize = true;
            partitionSerialLabel.Location = new System.Drawing.Point(5, 37);
            partitionSerialLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionSerialLabel.Name = "partitionSerialLabel";
            partitionSerialLabel.Size = new System.Drawing.Size(83, 15);
            partitionSerialLabel.TabIndex = 19;
            partitionSerialLabel.Text = "Serial number:";
            // 
            // partitionLastLbaValueLabel
            // 
            partitionLastLbaValueLabel.AutoEllipsis = true;
            partitionLastLbaValueLabel.Location = new System.Drawing.Point(93, 110);
            partitionLastLbaValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionLastLbaValueLabel.Name = "partitionLastLbaValueLabel";
            partitionLastLbaValueLabel.Size = new System.Drawing.Size(134, 16);
            partitionLastLbaValueLabel.TabIndex = 16;
            partitionLastLbaValueLabel.Text = "<last lba>";
            // 
            // partitionLastLbaLabel
            // 
            partitionLastLbaLabel.AutoSize = true;
            partitionLastLbaLabel.Location = new System.Drawing.Point(5, 110);
            partitionLastLbaLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionLastLbaLabel.Name = "partitionLastLbaLabel";
            partitionLastLbaLabel.Size = new System.Drawing.Size(55, 15);
            partitionLastLbaLabel.TabIndex = 17;
            partitionLastLbaLabel.Text = "Last LBA:";
            // 
            // partitionFirstLbaValueLabel
            // 
            partitionFirstLbaValueLabel.AutoEllipsis = true;
            partitionFirstLbaValueLabel.Location = new System.Drawing.Point(93, 74);
            partitionFirstLbaValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionFirstLbaValueLabel.Name = "partitionFirstLbaValueLabel";
            partitionFirstLbaValueLabel.Size = new System.Drawing.Size(134, 16);
            partitionFirstLbaValueLabel.TabIndex = 13;
            partitionFirstLbaValueLabel.Text = "<first lba>";
            // 
            // partitionFirstLbaLabel
            // 
            partitionFirstLbaLabel.AutoSize = true;
            partitionFirstLbaLabel.Location = new System.Drawing.Point(5, 74);
            partitionFirstLbaLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionFirstLbaLabel.Name = "partitionFirstLbaLabel";
            partitionFirstLbaLabel.Size = new System.Drawing.Size(56, 15);
            partitionFirstLbaLabel.TabIndex = 15;
            partitionFirstLbaLabel.Text = "First LBA:";
            // 
            // partitionTypeLabel
            // 
            partitionTypeLabel.AutoSize = true;
            partitionTypeLabel.Location = new System.Drawing.Point(5, 18);
            partitionTypeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionTypeLabel.Name = "partitionTypeLabel";
            partitionTypeLabel.Size = new System.Drawing.Size(81, 15);
            partitionTypeLabel.TabIndex = 13;
            partitionTypeLabel.Text = "Partition type:";
            // 
            // partitionTypeValueLabel
            // 
            partitionTypeValueLabel.AutoEllipsis = true;
            partitionTypeValueLabel.Location = new System.Drawing.Point(93, 18);
            partitionTypeValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            partitionTypeValueLabel.Name = "partitionTypeValueLabel";
            partitionTypeValueLabel.Size = new System.Drawing.Size(134, 16);
            partitionTypeValueLabel.TabIndex = 14;
            partitionTypeValueLabel.Text = "<partition type>";
            // 
            // loadOptionsGroupBox
            // 
            loadOptionsGroupBox.Controls.Add(readOnlyCheckBox);
            loadOptionsGroupBox.Location = new System.Drawing.Point(10, 353);
            loadOptionsGroupBox.Margin = new System.Windows.Forms.Padding(2);
            loadOptionsGroupBox.Name = "loadOptionsGroupBox";
            loadOptionsGroupBox.Padding = new System.Windows.Forms.Padding(2);
            loadOptionsGroupBox.Size = new System.Drawing.Size(466, 54);
            loadOptionsGroupBox.TabIndex = 13;
            loadOptionsGroupBox.TabStop = false;
            loadOptionsGroupBox.Text = "Options";
            // 
            // SelectPartitionDialog
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(486, 470);
            Controls.Add(loadOptionsGroupBox);
            Controls.Add(partitionInfoGroupBox);
            Controls.Add(diskInfoGroupBox);
            Controls.Add(partitionsListView);
            Controls.Add(buttonPanel);
            Controls.Add(descriptionLabel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SelectPartitionDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Select partition";
            buttonPanel.ResumeLayout(false);
            diskInfoGroupBox.ResumeLayout(false);
            diskInfoGroupBox.PerformLayout();
            partitionInfoGroupBox.ResumeLayout(false);
            partitionInfoGroupBox.PerformLayout();
            loadOptionsGroupBox.ResumeLayout(false);
            loadOptionsGroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.CheckBox readOnlyCheckBox;
        private ListViewEx partitionsListView;
        private System.Windows.Forms.ColumnHeader numberColumnHeader;
        private System.Windows.Forms.ColumnHeader flagColumnHeader;
        private System.Windows.Forms.ColumnHeader sizeColumnHeader;
        private System.Windows.Forms.ColumnHeader volumeLabelColumnHeader;
        private System.Windows.Forms.Label partitionTableLabel;
        private System.Windows.Forms.Label diskTotalSizeLabel;
        private System.Windows.Forms.Label diskTotalSizeValueLabel;
        private System.Windows.Forms.ColumnHeader fileSystemColumnHeader;
        private System.Windows.Forms.GroupBox diskInfoGroupBox;
        private System.Windows.Forms.GroupBox partitionInfoGroupBox;
        private System.Windows.Forms.GroupBox loadOptionsGroupBox;
        private System.Windows.Forms.Label diskTypeValueLabel;
        private System.Windows.Forms.Label diskTypeLabel;
        private System.Windows.Forms.Label partitionTypeLabel;
        private System.Windows.Forms.Label partitionTypeValueLabel;
        private System.Windows.Forms.Label partitionFirstLbaValueLabel;
        private System.Windows.Forms.Label partitionFirstLbaLabel;
        private System.Windows.Forms.Label partitionLastLbaValueLabel;
        private System.Windows.Forms.Label partitionLastLbaLabel;
        private System.Windows.Forms.Label partitionSerialValueLabel;
        private System.Windows.Forms.Label partitionSerialLabel;
        private System.Windows.Forms.Label partitionTotalSectorsValueLabel;
        private System.Windows.Forms.Label partitionTotalSectorsLabel;
        private System.Windows.Forms.Label diskTotalSectorsValueLabel;
        private System.Windows.Forms.Label diskTotalSectorsLabel;
        private System.Windows.Forms.Label partitionEndOffsetValueLabel;
        private System.Windows.Forms.Label partitionEndOffsetLabel;
        private System.Windows.Forms.Label partitionStartOffsetValueLabel;
        private System.Windows.Forms.Label partitionStartOffsetLabel;
        private System.Windows.Forms.Label partitionGuidValueLabel;
        private System.Windows.Forms.Label partitionGuidLabel;
        private System.Windows.Forms.Label diskUsableSizeValueLabel;
        private System.Windows.Forms.Label diskUsableSizeLabel;
        private System.Windows.Forms.Label partitionTableValueLabel;
        private System.Windows.Forms.Label diskGuidValueLabel;
        private System.Windows.Forms.Label diskGuidLabel;
        private System.Windows.Forms.Label diskSerialValueLabel;
        private System.Windows.Forms.Label diskSerialLabel;
        private System.Windows.Forms.Label diskTimestampValueLabel;
        private System.Windows.Forms.Label diskTimestampLabel;
    }
}
