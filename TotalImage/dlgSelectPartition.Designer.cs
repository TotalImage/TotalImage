
namespace TotalImage
{
    partial class dlgSelectPartition
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
            lblDesc = new System.Windows.Forms.Label();
            pnlBottom = new System.Windows.Forms.Panel();
            btnCancel = new System.Windows.Forms.Button();
            btnOK = new System.Windows.Forms.Button();
            cbxReadOnly = new System.Windows.Forms.CheckBox();
            lstPartitions = new System.Windows.Forms.ListView();
            clmNumber = new System.Windows.Forms.ColumnHeader();
            clmFileSystem = new System.Windows.Forms.ColumnHeader();
            clmVolumeLabel = new System.Windows.Forms.ColumnHeader();
            clmSize = new System.Windows.Forms.ColumnHeader();
            clmActive = new System.Windows.Forms.ColumnHeader();
            lblPartionTable = new System.Windows.Forms.Label();
            lblDiskTotalSize = new System.Windows.Forms.Label();
            lblDiskTotalSize1 = new System.Windows.Forms.Label();
            gbxDiskInfo = new System.Windows.Forms.GroupBox();
            lblDiskGuid1 = new System.Windows.Forms.Label();
            lblDiskGuid = new System.Windows.Forms.Label();
            lblDiskSerial1 = new System.Windows.Forms.Label();
            lblDiskSerial = new System.Windows.Forms.Label();
            lblDiskTimestamp1 = new System.Windows.Forms.Label();
            lblDiskTimestamp = new System.Windows.Forms.Label();
            lblPartitionTable1 = new System.Windows.Forms.Label();
            lblDiskUsableSize1 = new System.Windows.Forms.Label();
            lblDiskUsableSize = new System.Windows.Forms.Label();
            lblDiskTotalSectors1 = new System.Windows.Forms.Label();
            lblDiskTotalSectors = new System.Windows.Forms.Label();
            lblDiskType1 = new System.Windows.Forms.Label();
            lblDiskType = new System.Windows.Forms.Label();
            gbxPartitionInfo = new System.Windows.Forms.GroupBox();
            lblPartitionEndOffset1 = new System.Windows.Forms.Label();
            lblPartitionEndOffset = new System.Windows.Forms.Label();
            lblPartitionStartOffset1 = new System.Windows.Forms.Label();
            lblPartitionStartOffset = new System.Windows.Forms.Label();
            lblPartitionGuid1 = new System.Windows.Forms.Label();
            lblPartitionGuid = new System.Windows.Forms.Label();
            lblPartitionTotalSectors1 = new System.Windows.Forms.Label();
            lblPartitionTotalSectors = new System.Windows.Forms.Label();
            lblPartitionSerial1 = new System.Windows.Forms.Label();
            lblPartitionSerial = new System.Windows.Forms.Label();
            lblPartitionLastLba1 = new System.Windows.Forms.Label();
            lblPartitionLastLba = new System.Windows.Forms.Label();
            lblPartitionFirstLba1 = new System.Windows.Forms.Label();
            lblPartitionFirstLba = new System.Windows.Forms.Label();
            lblPartitionType = new System.Windows.Forms.Label();
            lblPartitionType1 = new System.Windows.Forms.Label();
            gbxLoadOptions = new System.Windows.Forms.GroupBox();
            pnlBottom.SuspendLayout();
            gbxDiskInfo.SuspendLayout();
            gbxPartitionInfo.SuspendLayout();
            gbxLoadOptions.SuspendLayout();
            SuspendLayout();
            // 
            // lblDesc
            // 
            lblDesc.AutoSize = true;
            lblDesc.Location = new System.Drawing.Point(10, 7);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new System.Drawing.Size(138, 15);
            lblDesc.TabIndex = 0;
            lblDesc.Text = "Select a partition to load:";
            // 
            // pnlBottom
            // 
            pnlBottom.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Location = new System.Drawing.Point(0, 421);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(486, 50);
            pnlBottom.TabIndex = 3;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnCancel.Location = new System.Drawing.Point(395, 14);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(80, 26);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Enabled = false;
            btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnOK.Location = new System.Drawing.Point(309, 14);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // cbxReadOnly
            // 
            cbxReadOnly.AutoSize = true;
            cbxReadOnly.Enabled = false;
            cbxReadOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxReadOnly.Location = new System.Drawing.Point(9, 23);
            cbxReadOnly.Name = "cbxReadOnly";
            cbxReadOnly.Size = new System.Drawing.Size(126, 20);
            cbxReadOnly.TabIndex = 5;
            cbxReadOnly.Text = "Load as read-only";
            cbxReadOnly.UseVisualStyleBackColor = true;
            // 
            // lstPartitions
            // 
            lstPartitions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { clmNumber, clmFileSystem, clmVolumeLabel, clmSize, clmActive });
            lstPartitions.FullRowSelect = true;
            lstPartitions.GridLines = true;
            lstPartitions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            lstPartitions.Location = new System.Drawing.Point(10, 30);
            lstPartitions.MultiSelect = false;
            lstPartitions.Name = "lstPartitions";
            lstPartitions.ShowGroups = false;
            lstPartitions.Size = new System.Drawing.Size(467, 141);
            lstPartitions.TabIndex = 6;
            lstPartitions.UseCompatibleStateImageBehavior = false;
            lstPartitions.View = System.Windows.Forms.View.Details;
            lstPartitions.ItemSelectionChanged += lstPartitions_ItemSelectionChanged;
            lstPartitions.MouseDoubleClick += lstPartitions_MouseDoubleClick;
            // 
            // clmNumber
            // 
            clmNumber.Text = "No.";
            clmNumber.Width = 45;
            // 
            // clmFileSystem
            // 
            clmFileSystem.Text = "File system";
            clmFileSystem.Width = 130;
            // 
            // clmVolumeLabel
            // 
            clmVolumeLabel.Text = "Volume label";
            clmVolumeLabel.Width = 150;
            // 
            // clmSize
            // 
            clmSize.Text = "Size";
            clmSize.Width = 130;
            // 
            // clmActive
            // 
            clmActive.Text = "Active";
            clmActive.Width = 120;
            // 
            // lblPartionTable
            // 
            lblPartionTable.AutoSize = true;
            lblPartionTable.Location = new System.Drawing.Point(5, 37);
            lblPartionTable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartionTable.Name = "lblPartionTable";
            lblPartionTable.Size = new System.Drawing.Size(84, 15);
            lblPartionTable.TabIndex = 7;
            lblPartionTable.Text = "Partition table:";
            // 
            // lblDiskTotalSize
            // 
            lblDiskTotalSize.AutoSize = true;
            lblDiskTotalSize.Location = new System.Drawing.Point(5, 129);
            lblDiskTotalSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDiskTotalSize.Name = "lblDiskTotalSize";
            lblDiskTotalSize.Size = new System.Drawing.Size(57, 15);
            lblDiskTotalSize.TabIndex = 9;
            lblDiskTotalSize.Text = "Total size:";
            // 
            // lblDiskTotalSize1
            // 
            lblDiskTotalSize1.AutoEllipsis = true;
            lblDiskTotalSize1.Location = new System.Drawing.Point(94, 129);
            lblDiskTotalSize1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDiskTotalSize1.Name = "lblDiskTotalSize1";
            lblDiskTotalSize1.Size = new System.Drawing.Size(132, 16);
            lblDiskTotalSize1.TabIndex = 10;
            lblDiskTotalSize1.Text = "<total size>";
            // 
            // gbxDiskInfo
            // 
            gbxDiskInfo.Controls.Add(lblDiskGuid1);
            gbxDiskInfo.Controls.Add(lblDiskGuid);
            gbxDiskInfo.Controls.Add(lblDiskSerial1);
            gbxDiskInfo.Controls.Add(lblDiskSerial);
            gbxDiskInfo.Controls.Add(lblDiskTimestamp1);
            gbxDiskInfo.Controls.Add(lblDiskTimestamp);
            gbxDiskInfo.Controls.Add(lblPartitionTable1);
            gbxDiskInfo.Controls.Add(lblDiskUsableSize1);
            gbxDiskInfo.Controls.Add(lblDiskUsableSize);
            gbxDiskInfo.Controls.Add(lblDiskTotalSectors1);
            gbxDiskInfo.Controls.Add(lblDiskTotalSectors);
            gbxDiskInfo.Controls.Add(lblDiskType1);
            gbxDiskInfo.Controls.Add(lblDiskType);
            gbxDiskInfo.Controls.Add(lblPartionTable);
            gbxDiskInfo.Controls.Add(lblDiskTotalSize1);
            gbxDiskInfo.Controls.Add(lblDiskTotalSize);
            gbxDiskInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            gbxDiskInfo.Location = new System.Drawing.Point(10, 176);
            gbxDiskInfo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            gbxDiskInfo.Name = "gbxDiskInfo";
            gbxDiskInfo.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            gbxDiskInfo.Size = new System.Drawing.Size(230, 172);
            gbxDiskInfo.TabIndex = 11;
            gbxDiskInfo.TabStop = false;
            gbxDiskInfo.Text = "Disk information";
            // 
            // lblDiskGuid1
            // 
            lblDiskGuid1.AutoEllipsis = true;
            lblDiskGuid1.Location = new System.Drawing.Point(94, 92);
            lblDiskGuid1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDiskGuid1.Name = "lblDiskGuid1";
            lblDiskGuid1.Size = new System.Drawing.Size(132, 16);
            lblDiskGuid1.TabIndex = 26;
            lblDiskGuid1.Text = "<gpt guid>";
            // 
            // lblDiskGuid
            // 
            lblDiskGuid.AutoSize = true;
            lblDiskGuid.Location = new System.Drawing.Point(5, 92);
            lblDiskGuid.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDiskGuid.Name = "lblDiskGuid";
            lblDiskGuid.Size = new System.Drawing.Size(61, 15);
            lblDiskGuid.TabIndex = 25;
            lblDiskGuid.Text = "GPT GUID:";
            // 
            // lblDiskSerial1
            // 
            lblDiskSerial1.AutoEllipsis = true;
            lblDiskSerial1.Location = new System.Drawing.Point(94, 74);
            lblDiskSerial1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDiskSerial1.Name = "lblDiskSerial1";
            lblDiskSerial1.Size = new System.Drawing.Size(132, 16);
            lblDiskSerial1.TabIndex = 24;
            lblDiskSerial1.Text = "<serial number>";
            // 
            // lblDiskSerial
            // 
            lblDiskSerial.AutoSize = true;
            lblDiskSerial.Location = new System.Drawing.Point(5, 74);
            lblDiskSerial.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDiskSerial.Name = "lblDiskSerial";
            lblDiskSerial.Size = new System.Drawing.Size(83, 15);
            lblDiskSerial.TabIndex = 23;
            lblDiskSerial.Text = "Serial number:";
            // 
            // lblDiskTimestamp1
            // 
            lblDiskTimestamp1.AutoEllipsis = true;
            lblDiskTimestamp1.Location = new System.Drawing.Point(94, 55);
            lblDiskTimestamp1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDiskTimestamp1.Name = "lblDiskTimestamp1";
            lblDiskTimestamp1.Size = new System.Drawing.Size(132, 16);
            lblDiskTimestamp1.TabIndex = 22;
            lblDiskTimestamp1.Text = "<timestamp>";
            // 
            // lblDiskTimestamp
            // 
            lblDiskTimestamp.AutoSize = true;
            lblDiskTimestamp.Location = new System.Drawing.Point(5, 55);
            lblDiskTimestamp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDiskTimestamp.Name = "lblDiskTimestamp";
            lblDiskTimestamp.Size = new System.Drawing.Size(69, 15);
            lblDiskTimestamp.TabIndex = 21;
            lblDiskTimestamp.Text = "Timestamp:";
            // 
            // lblPartitionTable1
            // 
            lblPartitionTable1.AutoEllipsis = true;
            lblPartitionTable1.Location = new System.Drawing.Point(94, 37);
            lblPartitionTable1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionTable1.Name = "lblPartitionTable1";
            lblPartitionTable1.Size = new System.Drawing.Size(132, 16);
            lblPartitionTable1.TabIndex = 20;
            lblPartitionTable1.Text = "<partition table>";
            // 
            // lblDiskUsableSize1
            // 
            lblDiskUsableSize1.AutoEllipsis = true;
            lblDiskUsableSize1.Location = new System.Drawing.Point(94, 110);
            lblDiskUsableSize1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDiskUsableSize1.Name = "lblDiskUsableSize1";
            lblDiskUsableSize1.Size = new System.Drawing.Size(132, 16);
            lblDiskUsableSize1.TabIndex = 19;
            lblDiskUsableSize1.Text = "<usable size>";
            // 
            // lblDiskUsableSize
            // 
            lblDiskUsableSize.AutoSize = true;
            lblDiskUsableSize.Location = new System.Drawing.Point(5, 110);
            lblDiskUsableSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDiskUsableSize.Name = "lblDiskUsableSize";
            lblDiskUsableSize.Size = new System.Drawing.Size(67, 15);
            lblDiskUsableSize.TabIndex = 18;
            lblDiskUsableSize.Text = "Usable size:";
            // 
            // lblDiskTotalSectors1
            // 
            lblDiskTotalSectors1.AutoEllipsis = true;
            lblDiskTotalSectors1.Location = new System.Drawing.Point(94, 147);
            lblDiskTotalSectors1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDiskTotalSectors1.Name = "lblDiskTotalSectors1";
            lblDiskTotalSectors1.Size = new System.Drawing.Size(132, 16);
            lblDiskTotalSectors1.TabIndex = 16;
            lblDiskTotalSectors1.Text = "<total sectors>";
            // 
            // lblDiskTotalSectors
            // 
            lblDiskTotalSectors.AutoSize = true;
            lblDiskTotalSectors.Location = new System.Drawing.Point(5, 147);
            lblDiskTotalSectors.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDiskTotalSectors.Name = "lblDiskTotalSectors";
            lblDiskTotalSectors.Size = new System.Drawing.Size(75, 15);
            lblDiskTotalSectors.TabIndex = 17;
            lblDiskTotalSectors.Text = "Total sectors:";
            // 
            // lblDiskType1
            // 
            lblDiskType1.AutoEllipsis = true;
            lblDiskType1.Location = new System.Drawing.Point(94, 18);
            lblDiskType1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDiskType1.Name = "lblDiskType1";
            lblDiskType1.Size = new System.Drawing.Size(132, 16);
            lblDiskType1.TabIndex = 12;
            lblDiskType1.Text = "<disk type>";
            // 
            // lblDiskType
            // 
            lblDiskType.AutoSize = true;
            lblDiskType.Location = new System.Drawing.Point(5, 18);
            lblDiskType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDiskType.Name = "lblDiskType";
            lblDiskType.Size = new System.Drawing.Size(58, 15);
            lblDiskType.TabIndex = 11;
            lblDiskType.Text = "Disk type:";
            // 
            // gbxPartitionInfo
            // 
            gbxPartitionInfo.Controls.Add(lblPartitionEndOffset1);
            gbxPartitionInfo.Controls.Add(lblPartitionEndOffset);
            gbxPartitionInfo.Controls.Add(lblPartitionStartOffset1);
            gbxPartitionInfo.Controls.Add(lblPartitionStartOffset);
            gbxPartitionInfo.Controls.Add(lblPartitionGuid1);
            gbxPartitionInfo.Controls.Add(lblPartitionGuid);
            gbxPartitionInfo.Controls.Add(lblPartitionTotalSectors1);
            gbxPartitionInfo.Controls.Add(lblPartitionTotalSectors);
            gbxPartitionInfo.Controls.Add(lblPartitionSerial1);
            gbxPartitionInfo.Controls.Add(lblPartitionSerial);
            gbxPartitionInfo.Controls.Add(lblPartitionLastLba1);
            gbxPartitionInfo.Controls.Add(lblPartitionLastLba);
            gbxPartitionInfo.Controls.Add(lblPartitionFirstLba1);
            gbxPartitionInfo.Controls.Add(lblPartitionFirstLba);
            gbxPartitionInfo.Controls.Add(lblPartitionType);
            gbxPartitionInfo.Controls.Add(lblPartitionType1);
            gbxPartitionInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            gbxPartitionInfo.Location = new System.Drawing.Point(245, 176);
            gbxPartitionInfo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            gbxPartitionInfo.Name = "gbxPartitionInfo";
            gbxPartitionInfo.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            gbxPartitionInfo.Size = new System.Drawing.Size(231, 172);
            gbxPartitionInfo.TabIndex = 12;
            gbxPartitionInfo.TabStop = false;
            gbxPartitionInfo.Text = "Partition information";
            // 
            // lblPartitionEndOffset1
            // 
            lblPartitionEndOffset1.AutoEllipsis = true;
            lblPartitionEndOffset1.Location = new System.Drawing.Point(93, 129);
            lblPartitionEndOffset1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionEndOffset1.Name = "lblPartitionEndOffset1";
            lblPartitionEndOffset1.Size = new System.Drawing.Size(134, 16);
            lblPartitionEndOffset1.TabIndex = 26;
            lblPartitionEndOffset1.Text = "<end offset>";
            // 
            // lblPartitionEndOffset
            // 
            lblPartitionEndOffset.AutoSize = true;
            lblPartitionEndOffset.Location = new System.Drawing.Point(5, 129);
            lblPartitionEndOffset.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionEndOffset.Name = "lblPartitionEndOffset";
            lblPartitionEndOffset.Size = new System.Drawing.Size(63, 15);
            lblPartitionEndOffset.TabIndex = 27;
            lblPartitionEndOffset.Text = "End offset:";
            // 
            // lblPartitionStartOffset1
            // 
            lblPartitionStartOffset1.AutoEllipsis = true;
            lblPartitionStartOffset1.Location = new System.Drawing.Point(93, 92);
            lblPartitionStartOffset1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionStartOffset1.Name = "lblPartitionStartOffset1";
            lblPartitionStartOffset1.Size = new System.Drawing.Size(134, 16);
            lblPartitionStartOffset1.TabIndex = 24;
            lblPartitionStartOffset1.Text = "<start offset>";
            // 
            // lblPartitionStartOffset
            // 
            lblPartitionStartOffset.AutoSize = true;
            lblPartitionStartOffset.Location = new System.Drawing.Point(5, 92);
            lblPartitionStartOffset.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionStartOffset.Name = "lblPartitionStartOffset";
            lblPartitionStartOffset.Size = new System.Drawing.Size(67, 15);
            lblPartitionStartOffset.TabIndex = 25;
            lblPartitionStartOffset.Text = "Start offset:";
            // 
            // lblPartitionGuid1
            // 
            lblPartitionGuid1.AutoEllipsis = true;
            lblPartitionGuid1.Location = new System.Drawing.Point(93, 55);
            lblPartitionGuid1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionGuid1.Name = "lblPartitionGuid1";
            lblPartitionGuid1.Size = new System.Drawing.Size(134, 16);
            lblPartitionGuid1.TabIndex = 22;
            lblPartitionGuid1.Text = "<gpt guid>";
            // 
            // lblPartitionGuid
            // 
            lblPartitionGuid.AutoSize = true;
            lblPartitionGuid.Location = new System.Drawing.Point(5, 55);
            lblPartitionGuid.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionGuid.Name = "lblPartitionGuid";
            lblPartitionGuid.Size = new System.Drawing.Size(61, 15);
            lblPartitionGuid.TabIndex = 23;
            lblPartitionGuid.Text = "GPT GUID:";
            // 
            // lblPartitionTotalSectors1
            // 
            lblPartitionTotalSectors1.AutoEllipsis = true;
            lblPartitionTotalSectors1.Location = new System.Drawing.Point(93, 147);
            lblPartitionTotalSectors1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionTotalSectors1.Name = "lblPartitionTotalSectors1";
            lblPartitionTotalSectors1.Size = new System.Drawing.Size(134, 16);
            lblPartitionTotalSectors1.TabIndex = 20;
            lblPartitionTotalSectors1.Text = "<total sectors>";
            // 
            // lblPartitionTotalSectors
            // 
            lblPartitionTotalSectors.AutoSize = true;
            lblPartitionTotalSectors.Location = new System.Drawing.Point(5, 147);
            lblPartitionTotalSectors.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionTotalSectors.Name = "lblPartitionTotalSectors";
            lblPartitionTotalSectors.Size = new System.Drawing.Size(75, 15);
            lblPartitionTotalSectors.TabIndex = 21;
            lblPartitionTotalSectors.Text = "Total sectors:";
            // 
            // lblPartitionSerial1
            // 
            lblPartitionSerial1.AutoEllipsis = true;
            lblPartitionSerial1.Location = new System.Drawing.Point(93, 37);
            lblPartitionSerial1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionSerial1.Name = "lblPartitionSerial1";
            lblPartitionSerial1.Size = new System.Drawing.Size(134, 16);
            lblPartitionSerial1.TabIndex = 18;
            lblPartitionSerial1.Text = "<serial number>";
            // 
            // lblPartitionSerial
            // 
            lblPartitionSerial.AutoSize = true;
            lblPartitionSerial.Location = new System.Drawing.Point(5, 37);
            lblPartitionSerial.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionSerial.Name = "lblPartitionSerial";
            lblPartitionSerial.Size = new System.Drawing.Size(83, 15);
            lblPartitionSerial.TabIndex = 19;
            lblPartitionSerial.Text = "Serial number:";
            // 
            // lblPartitionLastLba1
            // 
            lblPartitionLastLba1.AutoEllipsis = true;
            lblPartitionLastLba1.Location = new System.Drawing.Point(93, 110);
            lblPartitionLastLba1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionLastLba1.Name = "lblPartitionLastLba1";
            lblPartitionLastLba1.Size = new System.Drawing.Size(134, 16);
            lblPartitionLastLba1.TabIndex = 16;
            lblPartitionLastLba1.Text = "<last lba>";
            // 
            // lblPartitionLastLba
            // 
            lblPartitionLastLba.AutoSize = true;
            lblPartitionLastLba.Location = new System.Drawing.Point(5, 110);
            lblPartitionLastLba.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionLastLba.Name = "lblPartitionLastLba";
            lblPartitionLastLba.Size = new System.Drawing.Size(55, 15);
            lblPartitionLastLba.TabIndex = 17;
            lblPartitionLastLba.Text = "Last LBA:";
            // 
            // lblPartitionFirstLba1
            // 
            lblPartitionFirstLba1.AutoEllipsis = true;
            lblPartitionFirstLba1.Location = new System.Drawing.Point(93, 74);
            lblPartitionFirstLba1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionFirstLba1.Name = "lblPartitionFirstLba1";
            lblPartitionFirstLba1.Size = new System.Drawing.Size(134, 16);
            lblPartitionFirstLba1.TabIndex = 13;
            lblPartitionFirstLba1.Text = "<first lba>";
            // 
            // lblPartitionFirstLba
            // 
            lblPartitionFirstLba.AutoSize = true;
            lblPartitionFirstLba.Location = new System.Drawing.Point(5, 74);
            lblPartitionFirstLba.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionFirstLba.Name = "lblPartitionFirstLba";
            lblPartitionFirstLba.Size = new System.Drawing.Size(56, 15);
            lblPartitionFirstLba.TabIndex = 15;
            lblPartitionFirstLba.Text = "First LBA:";
            // 
            // lblPartitionType
            // 
            lblPartitionType.AutoSize = true;
            lblPartitionType.Location = new System.Drawing.Point(5, 18);
            lblPartitionType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionType.Name = "lblPartitionType";
            lblPartitionType.Size = new System.Drawing.Size(81, 15);
            lblPartitionType.TabIndex = 13;
            lblPartitionType.Text = "Partition type:";
            // 
            // lblPartitionType1
            // 
            lblPartitionType1.AutoEllipsis = true;
            lblPartitionType1.Location = new System.Drawing.Point(93, 18);
            lblPartitionType1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitionType1.Name = "lblPartitionType1";
            lblPartitionType1.Size = new System.Drawing.Size(134, 16);
            lblPartitionType1.TabIndex = 14;
            lblPartitionType1.Text = "<partition type>";
            // 
            // gbxLoadOptions
            // 
            gbxLoadOptions.Controls.Add(cbxReadOnly);
            gbxLoadOptions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            gbxLoadOptions.Location = new System.Drawing.Point(10, 353);
            gbxLoadOptions.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            gbxLoadOptions.Name = "gbxLoadOptions";
            gbxLoadOptions.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            gbxLoadOptions.Size = new System.Drawing.Size(466, 54);
            gbxLoadOptions.TabIndex = 13;
            gbxLoadOptions.TabStop = false;
            gbxLoadOptions.Text = "Load options";
            // 
            // dlgSelectPartition
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.Color.White;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(486, 470);
            Controls.Add(gbxLoadOptions);
            Controls.Add(gbxPartitionInfo);
            Controls.Add(gbxDiskInfo);
            Controls.Add(lstPartitions);
            Controls.Add(pnlBottom);
            Controls.Add(lblDesc);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgSelectPartition";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Select partition";
            Load += dlgSelectPartition_Load;
            pnlBottom.ResumeLayout(false);
            gbxDiskInfo.ResumeLayout(false);
            gbxDiskInfo.PerformLayout();
            gbxPartitionInfo.ResumeLayout(false);
            gbxPartitionInfo.PerformLayout();
            gbxLoadOptions.ResumeLayout(false);
            gbxLoadOptions.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox cbxReadOnly;
        private System.Windows.Forms.ListView lstPartitions;
        private System.Windows.Forms.ColumnHeader clmNumber;
        private System.Windows.Forms.ColumnHeader clmActive;
        private System.Windows.Forms.ColumnHeader clmSize;
        private System.Windows.Forms.ColumnHeader clmVolumeLabel;
        private System.Windows.Forms.Label lblPartionTable;
        private System.Windows.Forms.Label lblDiskTotalSize;
        private System.Windows.Forms.Label lblDiskTotalSize1;
        private System.Windows.Forms.ColumnHeader clmFileSystem;
        private System.Windows.Forms.GroupBox gbxDiskInfo;
        private System.Windows.Forms.GroupBox gbxPartitionInfo;
        private System.Windows.Forms.GroupBox gbxLoadOptions;
        private System.Windows.Forms.Label lblDiskType1;
        private System.Windows.Forms.Label lblDiskType;
        private System.Windows.Forms.Label lblPartitionType;
        private System.Windows.Forms.Label lblPartitionType1;
        private System.Windows.Forms.Label lblPartitionFirstLba1;
        private System.Windows.Forms.Label lblPartitionFirstLba;
        private System.Windows.Forms.Label lblPartitionLastLba1;
        private System.Windows.Forms.Label lblPartitionLastLba;
        private System.Windows.Forms.Label lblPartitionSerial1;
        private System.Windows.Forms.Label lblPartitionSerial;
        private System.Windows.Forms.Label lblPartitionTotalSectors1;
        private System.Windows.Forms.Label lblPartitionTotalSectors;
        private System.Windows.Forms.Label lblDiskTotalSectors1;
        private System.Windows.Forms.Label lblDiskTotalSectors;
        private System.Windows.Forms.Label lblPartitionEndOffset1;
        private System.Windows.Forms.Label lblPartitionEndOffset;
        private System.Windows.Forms.Label lblPartitionStartOffset1;
        private System.Windows.Forms.Label lblPartitionStartOffset;
        private System.Windows.Forms.Label lblPartitionGuid1;
        private System.Windows.Forms.Label lblPartitionGuid;
        private System.Windows.Forms.Label lblDiskUsableSize1;
        private System.Windows.Forms.Label lblDiskUsableSize;
        private System.Windows.Forms.Label lblPartitionTable1;
        private System.Windows.Forms.Label lblDiskGuid1;
        private System.Windows.Forms.Label lblDiskGuid;
        private System.Windows.Forms.Label lblDiskSerial1;
        private System.Windows.Forms.Label lblDiskSerial;
        private System.Windows.Forms.Label lblDiskTimestamp1;
        private System.Windows.Forms.Label lblDiskTimestamp;
    }
}
