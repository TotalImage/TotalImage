
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
            this.lblDesc = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbxReadOnly = new System.Windows.Forms.CheckBox();
            this.lstPartitions = new System.Windows.Forms.ListView();
            this.clmNumber = new System.Windows.Forms.ColumnHeader();
            this.clmFileSystem = new System.Windows.Forms.ColumnHeader();
            this.clmVolumeLabel = new System.Windows.Forms.ColumnHeader();
            this.clmSize = new System.Windows.Forms.ColumnHeader();
            this.clmActive = new System.Windows.Forms.ColumnHeader();
            this.lblPartionTable = new System.Windows.Forms.Label();
            this.lblDiskTotalSize = new System.Windows.Forms.Label();
            this.lblDiskTotalSize1 = new System.Windows.Forms.Label();
            this.gbxDiskInfo = new System.Windows.Forms.GroupBox();
            this.lblDiskGuid1 = new System.Windows.Forms.Label();
            this.lblDiskGuid = new System.Windows.Forms.Label();
            this.lblDiskSerial1 = new System.Windows.Forms.Label();
            this.lblDiskSerial = new System.Windows.Forms.Label();
            this.lblDiskTimestamp1 = new System.Windows.Forms.Label();
            this.lblDiskTimestamp = new System.Windows.Forms.Label();
            this.lblPartitionTable1 = new System.Windows.Forms.Label();
            this.lblDiskUsableSize1 = new System.Windows.Forms.Label();
            this.lblDiskUsableSize = new System.Windows.Forms.Label();
            this.lblDiskTotalSectors1 = new System.Windows.Forms.Label();
            this.lblDiskTotalSectors = new System.Windows.Forms.Label();
            this.lblDiskType1 = new System.Windows.Forms.Label();
            this.lblDiskType = new System.Windows.Forms.Label();
            this.gbxPartitionInfo = new System.Windows.Forms.GroupBox();
            this.lblPartitionEndOffset1 = new System.Windows.Forms.Label();
            this.lblPartitionEndOffset = new System.Windows.Forms.Label();
            this.lblPartitionStartOffset1 = new System.Windows.Forms.Label();
            this.lblPartitionStartOffset = new System.Windows.Forms.Label();
            this.lblPartitionGuid1 = new System.Windows.Forms.Label();
            this.lblPartitionGuid = new System.Windows.Forms.Label();
            this.lblPartitionTotalSectors1 = new System.Windows.Forms.Label();
            this.lblPartitionTotalSectors = new System.Windows.Forms.Label();
            this.lblPartitionSerial1 = new System.Windows.Forms.Label();
            this.lblPartitionSerial = new System.Windows.Forms.Label();
            this.lblPartitionLastLba1 = new System.Windows.Forms.Label();
            this.lblPartitionLastLba = new System.Windows.Forms.Label();
            this.lblPartitionFirstLba1 = new System.Windows.Forms.Label();
            this.lblPartitionFirstLba = new System.Windows.Forms.Label();
            this.lblPartitionType = new System.Windows.Forms.Label();
            this.lblPartitionType1 = new System.Windows.Forms.Label();
            this.gbxLoadOptions = new System.Windows.Forms.GroupBox();
            this.pnlBottom.SuspendLayout();
            this.gbxDiskInfo.SuspendLayout();
            this.gbxPartitionInfo.SuspendLayout();
            this.gbxLoadOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(13, 9);
            this.lblDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(177, 20);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "Select a partition to load:";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Location = new System.Drawing.Point(0, 526);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(607, 62);
            this.pnlBottom.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(494, 17);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 32);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(386, 17);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 32);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbxReadOnly
            // 
            this.cbxReadOnly.AutoSize = true;
            this.cbxReadOnly.Enabled = false;
            this.cbxReadOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxReadOnly.Location = new System.Drawing.Point(11, 29);
            this.cbxReadOnly.Margin = new System.Windows.Forms.Padding(4);
            this.cbxReadOnly.Name = "cbxReadOnly";
            this.cbxReadOnly.Size = new System.Drawing.Size(159, 25);
            this.cbxReadOnly.TabIndex = 5;
            this.cbxReadOnly.Text = "Load as read-only";
            this.cbxReadOnly.UseVisualStyleBackColor = true;
            // 
            // lstPartitions
            // 
            this.lstPartitions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmNumber,
            this.clmFileSystem,
            this.clmVolumeLabel,
            this.clmSize,
            this.clmActive});
            this.lstPartitions.FullRowSelect = true;
            this.lstPartitions.GridLines = true;
            this.lstPartitions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstPartitions.Location = new System.Drawing.Point(12, 38);
            this.lstPartitions.Margin = new System.Windows.Forms.Padding(4);
            this.lstPartitions.MultiSelect = false;
            this.lstPartitions.Name = "lstPartitions";
            this.lstPartitions.ShowGroups = false;
            this.lstPartitions.Size = new System.Drawing.Size(583, 175);
            this.lstPartitions.TabIndex = 6;
            this.lstPartitions.UseCompatibleStateImageBehavior = false;
            this.lstPartitions.View = System.Windows.Forms.View.Details;
            this.lstPartitions.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstPartitions_ItemSelectionChanged);
            this.lstPartitions.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstPartitions_MouseDoubleClick);
            // 
            // clmNumber
            // 
            clmNumber.Text = "No.";
            clmNumber.Width = 45;
            // 
            // clmFileSystem
            // 
            this.clmFileSystem.Text = "File system";
            this.clmFileSystem.Width = 130;
            // 
            // clmVolumeLabel
            // 
            this.clmVolumeLabel.Text = "Volume label";
            this.clmVolumeLabel.Width = 150;
            // 
            // clmSize
            // 
            this.clmSize.Text = "Size";
            this.clmSize.Width = 130;
            // 
            // clmActive
            // 
            this.clmActive.Text = "Active";
            this.clmActive.Width = 120;
            // 
            // lblPartionTable
            // 
            this.lblPartionTable.AutoSize = true;
            this.lblPartionTable.Location = new System.Drawing.Point(6, 46);
            this.lblPartionTable.Name = "lblPartionTable";
            this.lblPartionTable.Size = new System.Drawing.Size(105, 20);
            this.lblPartionTable.TabIndex = 7;
            this.lblPartionTable.Text = "Partition table:";
            // 
            // lblDiskTotalSize
            // 
            this.lblDiskTotalSize.AutoSize = true;
            this.lblDiskTotalSize.Location = new System.Drawing.Point(6, 161);
            this.lblDiskTotalSize.Name = "lblDiskTotalSize";
            this.lblDiskTotalSize.Size = new System.Drawing.Size(74, 20);
            this.lblDiskTotalSize.TabIndex = 9;
            this.lblDiskTotalSize.Text = "Total size:";
            // 
            // lblDiskTotalSize1
            // 
            this.lblDiskTotalSize1.AutoEllipsis = true;
            this.lblDiskTotalSize1.Location = new System.Drawing.Point(117, 161);
            this.lblDiskTotalSize1.Name = "lblDiskTotalSize1";
            this.lblDiskTotalSize1.Size = new System.Drawing.Size(165, 20);
            this.lblDiskTotalSize1.TabIndex = 10;
            this.lblDiskTotalSize1.Text = "<total size>";
            // 
            // gbxDiskInfo
            // 
            this.gbxDiskInfo.Controls.Add(this.lblDiskGuid1);
            this.gbxDiskInfo.Controls.Add(this.lblDiskGuid);
            this.gbxDiskInfo.Controls.Add(this.lblDiskSerial1);
            this.gbxDiskInfo.Controls.Add(this.lblDiskSerial);
            this.gbxDiskInfo.Controls.Add(this.lblDiskTimestamp1);
            this.gbxDiskInfo.Controls.Add(this.lblDiskTimestamp);
            this.gbxDiskInfo.Controls.Add(this.lblPartitionTable1);
            this.gbxDiskInfo.Controls.Add(this.lblDiskUsableSize1);
            this.gbxDiskInfo.Controls.Add(this.lblDiskUsableSize);
            this.gbxDiskInfo.Controls.Add(this.lblDiskTotalSectors1);
            this.gbxDiskInfo.Controls.Add(this.lblDiskTotalSectors);
            this.gbxDiskInfo.Controls.Add(this.lblDiskType1);
            this.gbxDiskInfo.Controls.Add(this.lblDiskType);
            this.gbxDiskInfo.Controls.Add(this.lblPartionTable);
            this.gbxDiskInfo.Controls.Add(this.lblDiskTotalSize1);
            this.gbxDiskInfo.Controls.Add(this.lblDiskTotalSize);
            this.gbxDiskInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbxDiskInfo.Location = new System.Drawing.Point(12, 220);
            this.gbxDiskInfo.Name = "gbxDiskInfo";
            this.gbxDiskInfo.Size = new System.Drawing.Size(288, 215);
            this.gbxDiskInfo.TabIndex = 11;
            this.gbxDiskInfo.TabStop = false;
            this.gbxDiskInfo.Text = "Disk information";
            // 
            // lblDiskGuid1
            // 
            this.lblDiskGuid1.AutoEllipsis = true;
            this.lblDiskGuid1.Location = new System.Drawing.Point(117, 115);
            this.lblDiskGuid1.Name = "lblDiskGuid1";
            this.lblDiskGuid1.Size = new System.Drawing.Size(165, 20);
            this.lblDiskGuid1.TabIndex = 26;
            this.lblDiskGuid1.Text = "<gpt guid>";
            // 
            // lblDiskGuid
            // 
            this.lblDiskGuid.AutoSize = true;
            this.lblDiskGuid.Location = new System.Drawing.Point(6, 115);
            this.lblDiskGuid.Name = "lblDiskGuid";
            this.lblDiskGuid.Size = new System.Drawing.Size(77, 20);
            this.lblDiskGuid.TabIndex = 25;
            this.lblDiskGuid.Text = "GPT GUID:";
            // 
            // lblDiskSerial1
            // 
            this.lblDiskSerial1.AutoEllipsis = true;
            this.lblDiskSerial1.Location = new System.Drawing.Point(117, 92);
            this.lblDiskSerial1.Name = "lblDiskSerial1";
            this.lblDiskSerial1.Size = new System.Drawing.Size(165, 20);
            this.lblDiskSerial1.TabIndex = 24;
            this.lblDiskSerial1.Text = "<serial number>";
            // 
            // lblDiskSerial
            // 
            this.lblDiskSerial.AutoSize = true;
            this.lblDiskSerial.Location = new System.Drawing.Point(6, 92);
            this.lblDiskSerial.Name = "lblDiskSerial";
            this.lblDiskSerial.Size = new System.Drawing.Size(104, 20);
            this.lblDiskSerial.TabIndex = 23;
            this.lblDiskSerial.Text = "Serial number:";
            // 
            // lblDiskTimestamp1
            // 
            this.lblDiskTimestamp1.AutoEllipsis = true;
            this.lblDiskTimestamp1.Location = new System.Drawing.Point(117, 69);
            this.lblDiskTimestamp1.Name = "lblDiskTimestamp1";
            this.lblDiskTimestamp1.Size = new System.Drawing.Size(165, 20);
            this.lblDiskTimestamp1.TabIndex = 22;
            this.lblDiskTimestamp1.Text = "<timestamp>";
            // 
            // lblDiskTimestamp
            // 
            this.lblDiskTimestamp.AutoSize = true;
            this.lblDiskTimestamp.Location = new System.Drawing.Point(6, 69);
            this.lblDiskTimestamp.Name = "lblDiskTimestamp";
            this.lblDiskTimestamp.Size = new System.Drawing.Size(86, 20);
            this.lblDiskTimestamp.TabIndex = 21;
            this.lblDiskTimestamp.Text = "Timestamp:";
            // 
            // lblPartitionTable1
            // 
            this.lblPartitionTable1.AutoEllipsis = true;
            this.lblPartitionTable1.Location = new System.Drawing.Point(117, 46);
            this.lblPartitionTable1.Name = "lblPartitionTable1";
            this.lblPartitionTable1.Size = new System.Drawing.Size(165, 20);
            this.lblPartitionTable1.TabIndex = 20;
            this.lblPartitionTable1.Text = "<partition table>";
            // 
            // lblDiskUsableSize1
            // 
            this.lblDiskUsableSize1.AutoEllipsis = true;
            this.lblDiskUsableSize1.Location = new System.Drawing.Point(117, 138);
            this.lblDiskUsableSize1.Name = "lblDiskUsableSize1";
            this.lblDiskUsableSize1.Size = new System.Drawing.Size(165, 20);
            this.lblDiskUsableSize1.TabIndex = 19;
            this.lblDiskUsableSize1.Text = "<usable size>";
            // 
            // lblDiskUsableSize
            // 
            this.lblDiskUsableSize.AutoSize = true;
            this.lblDiskUsableSize.Location = new System.Drawing.Point(6, 138);
            this.lblDiskUsableSize.Name = "lblDiskUsableSize";
            this.lblDiskUsableSize.Size = new System.Drawing.Size(86, 20);
            this.lblDiskUsableSize.TabIndex = 18;
            this.lblDiskUsableSize.Text = "Usable size:";
            // 
            // lblDiskTotalSectors1
            // 
            this.lblDiskTotalSectors1.AutoEllipsis = true;
            this.lblDiskTotalSectors1.Location = new System.Drawing.Point(117, 184);
            this.lblDiskTotalSectors1.Name = "lblDiskTotalSectors1";
            this.lblDiskTotalSectors1.Size = new System.Drawing.Size(165, 20);
            this.lblDiskTotalSectors1.TabIndex = 16;
            this.lblDiskTotalSectors1.Text = "<total sectors>";
            // 
            // lblDiskTotalSectors
            // 
            this.lblDiskTotalSectors.AutoSize = true;
            this.lblDiskTotalSectors.Location = new System.Drawing.Point(6, 184);
            this.lblDiskTotalSectors.Name = "lblDiskTotalSectors";
            this.lblDiskTotalSectors.Size = new System.Drawing.Size(95, 20);
            this.lblDiskTotalSectors.TabIndex = 17;
            this.lblDiskTotalSectors.Text = "Total sectors:";
            // 
            // lblDiskType1
            // 
            this.lblDiskType1.AutoEllipsis = true;
            this.lblDiskType1.Location = new System.Drawing.Point(117, 23);
            this.lblDiskType1.Name = "lblDiskType1";
            this.lblDiskType1.Size = new System.Drawing.Size(165, 20);
            this.lblDiskType1.TabIndex = 12;
            this.lblDiskType1.Text = "<disk type>";
            // 
            // lblDiskType
            // 
            this.lblDiskType.AutoSize = true;
            this.lblDiskType.Location = new System.Drawing.Point(6, 23);
            this.lblDiskType.Name = "lblDiskType";
            this.lblDiskType.Size = new System.Drawing.Size(73, 20);
            this.lblDiskType.TabIndex = 11;
            this.lblDiskType.Text = "Disk type:";
            // 
            // gbxPartitionInfo
            // 
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionEndOffset1);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionEndOffset);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionStartOffset1);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionStartOffset);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionGuid1);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionGuid);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionTotalSectors1);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionTotalSectors);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionSerial1);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionSerial);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionLastLba1);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionLastLba);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionFirstLba1);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionFirstLba);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionType);
            this.gbxPartitionInfo.Controls.Add(this.lblPartitionType1);
            this.gbxPartitionInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbxPartitionInfo.Location = new System.Drawing.Point(306, 220);
            this.gbxPartitionInfo.Name = "gbxPartitionInfo";
            this.gbxPartitionInfo.Size = new System.Drawing.Size(289, 215);
            this.gbxPartitionInfo.TabIndex = 12;
            this.gbxPartitionInfo.TabStop = false;
            this.gbxPartitionInfo.Text = "Partition information";
            // 
            // lblPartitionEndOffset1
            // 
            this.lblPartitionEndOffset1.AutoEllipsis = true;
            this.lblPartitionEndOffset1.Location = new System.Drawing.Point(116, 161);
            this.lblPartitionEndOffset1.Name = "lblPartitionEndOffset1";
            this.lblPartitionEndOffset1.Size = new System.Drawing.Size(167, 20);
            this.lblPartitionEndOffset1.TabIndex = 26;
            this.lblPartitionEndOffset1.Text = "<end offset>";
            // 
            // lblPartitionEndOffset
            // 
            this.lblPartitionEndOffset.AutoSize = true;
            this.lblPartitionEndOffset.Location = new System.Drawing.Point(6, 161);
            this.lblPartitionEndOffset.Name = "lblPartitionEndOffset";
            this.lblPartitionEndOffset.Size = new System.Drawing.Size(79, 20);
            this.lblPartitionEndOffset.TabIndex = 27;
            this.lblPartitionEndOffset.Text = "End offset:";
            // 
            // lblPartitionStartOffset1
            // 
            this.lblPartitionStartOffset1.AutoEllipsis = true;
            this.lblPartitionStartOffset1.Location = new System.Drawing.Point(116, 115);
            this.lblPartitionStartOffset1.Name = "lblPartitionStartOffset1";
            this.lblPartitionStartOffset1.Size = new System.Drawing.Size(167, 20);
            this.lblPartitionStartOffset1.TabIndex = 24;
            this.lblPartitionStartOffset1.Text = "<start offset>";
            // 
            // lblPartitionStartOffset
            // 
            this.lblPartitionStartOffset.AutoSize = true;
            this.lblPartitionStartOffset.Location = new System.Drawing.Point(6, 115);
            this.lblPartitionStartOffset.Name = "lblPartitionStartOffset";
            this.lblPartitionStartOffset.Size = new System.Drawing.Size(85, 20);
            this.lblPartitionStartOffset.TabIndex = 25;
            this.lblPartitionStartOffset.Text = "Start offset:";
            // 
            // lblPartitionGuid1
            // 
            this.lblPartitionGuid1.AutoEllipsis = true;
            this.lblPartitionGuid1.Location = new System.Drawing.Point(116, 69);
            this.lblPartitionGuid1.Name = "lblPartitionGuid1";
            this.lblPartitionGuid1.Size = new System.Drawing.Size(167, 20);
            this.lblPartitionGuid1.TabIndex = 22;
            this.lblPartitionGuid1.Text = "<gpt guid>";
            // 
            // lblPartitionGuid
            // 
            this.lblPartitionGuid.AutoSize = true;
            this.lblPartitionGuid.Location = new System.Drawing.Point(6, 69);
            this.lblPartitionGuid.Name = "lblPartitionGuid";
            this.lblPartitionGuid.Size = new System.Drawing.Size(77, 20);
            this.lblPartitionGuid.TabIndex = 23;
            this.lblPartitionGuid.Text = "GPT GUID:";
            // 
            // lblPartitionTotalSectors1
            // 
            this.lblPartitionTotalSectors1.AutoEllipsis = true;
            this.lblPartitionTotalSectors1.Location = new System.Drawing.Point(116, 184);
            this.lblPartitionTotalSectors1.Name = "lblPartitionTotalSectors1";
            this.lblPartitionTotalSectors1.Size = new System.Drawing.Size(167, 20);
            this.lblPartitionTotalSectors1.TabIndex = 20;
            this.lblPartitionTotalSectors1.Text = "<total sectors>";
            // 
            // lblPartitionTotalSectors
            // 
            this.lblPartitionTotalSectors.AutoSize = true;
            this.lblPartitionTotalSectors.Location = new System.Drawing.Point(6, 184);
            this.lblPartitionTotalSectors.Name = "lblPartitionTotalSectors";
            this.lblPartitionTotalSectors.Size = new System.Drawing.Size(95, 20);
            this.lblPartitionTotalSectors.TabIndex = 21;
            this.lblPartitionTotalSectors.Text = "Total sectors:";
            // 
            // lblPartitionSerial1
            // 
            this.lblPartitionSerial1.AutoEllipsis = true;
            this.lblPartitionSerial1.Location = new System.Drawing.Point(116, 46);
            this.lblPartitionSerial1.Name = "lblPartitionSerial1";
            this.lblPartitionSerial1.Size = new System.Drawing.Size(167, 20);
            this.lblPartitionSerial1.TabIndex = 18;
            this.lblPartitionSerial1.Text = "<serial number>";
            // 
            // lblPartitionSerial
            // 
            this.lblPartitionSerial.AutoSize = true;
            this.lblPartitionSerial.Location = new System.Drawing.Point(6, 46);
            this.lblPartitionSerial.Name = "lblPartitionSerial";
            this.lblPartitionSerial.Size = new System.Drawing.Size(104, 20);
            this.lblPartitionSerial.TabIndex = 19;
            this.lblPartitionSerial.Text = "Serial number:";
            // 
            // lblPartitionLastLba1
            // 
            this.lblPartitionLastLba1.AutoEllipsis = true;
            this.lblPartitionLastLba1.Location = new System.Drawing.Point(116, 138);
            this.lblPartitionLastLba1.Name = "lblPartitionLastLba1";
            this.lblPartitionLastLba1.Size = new System.Drawing.Size(167, 20);
            this.lblPartitionLastLba1.TabIndex = 16;
            this.lblPartitionLastLba1.Text = "<last lba>";
            // 
            // lblPartitionLastLba
            // 
            this.lblPartitionLastLba.AutoSize = true;
            this.lblPartitionLastLba.Location = new System.Drawing.Point(6, 138);
            this.lblPartitionLastLba.Name = "lblPartitionLastLba";
            this.lblPartitionLastLba.Size = new System.Drawing.Size(68, 20);
            this.lblPartitionLastLba.TabIndex = 17;
            this.lblPartitionLastLba.Text = "Last LBA:";
            // 
            // lblPartitionFirstLba1
            // 
            this.lblPartitionFirstLba1.AutoEllipsis = true;
            this.lblPartitionFirstLba1.Location = new System.Drawing.Point(116, 92);
            this.lblPartitionFirstLba1.Name = "lblPartitionFirstLba1";
            this.lblPartitionFirstLba1.Size = new System.Drawing.Size(167, 20);
            this.lblPartitionFirstLba1.TabIndex = 13;
            this.lblPartitionFirstLba1.Text = "<first lba>";
            // 
            // lblPartitionFirstLba
            // 
            this.lblPartitionFirstLba.AutoSize = true;
            this.lblPartitionFirstLba.Location = new System.Drawing.Point(6, 92);
            this.lblPartitionFirstLba.Name = "lblPartitionFirstLba";
            this.lblPartitionFirstLba.Size = new System.Drawing.Size(69, 20);
            this.lblPartitionFirstLba.TabIndex = 15;
            this.lblPartitionFirstLba.Text = "First LBA:";
            // 
            // lblPartitionType
            // 
            this.lblPartitionType.AutoSize = true;
            this.lblPartitionType.Location = new System.Drawing.Point(6, 23);
            this.lblPartitionType.Name = "lblPartitionType";
            this.lblPartitionType.Size = new System.Drawing.Size(100, 20);
            this.lblPartitionType.TabIndex = 13;
            this.lblPartitionType.Text = "Partition type:";
            // 
            // lblPartitionType1
            // 
            this.lblPartitionType1.AutoEllipsis = true;
            this.lblPartitionType1.Location = new System.Drawing.Point(116, 23);
            this.lblPartitionType1.Name = "lblPartitionType1";
            this.lblPartitionType1.Size = new System.Drawing.Size(167, 20);
            this.lblPartitionType1.TabIndex = 14;
            this.lblPartitionType1.Text = "<partition type>";
            // 
            // gbxLoadOptions
            // 
            this.gbxLoadOptions.Controls.Add(this.cbxReadOnly);
            this.gbxLoadOptions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbxLoadOptions.Location = new System.Drawing.Point(12, 441);
            this.gbxLoadOptions.Name = "gbxLoadOptions";
            this.gbxLoadOptions.Size = new System.Drawing.Size(583, 68);
            this.gbxLoadOptions.TabIndex = 13;
            this.gbxLoadOptions.TabStop = false;
            this.gbxLoadOptions.Text = "Load options";
            // 
            // dlgSelectPartition
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(607, 588);
            this.Controls.Add(this.gbxLoadOptions);
            this.Controls.Add(this.gbxPartitionInfo);
            this.Controls.Add(this.gbxDiskInfo);
            this.Controls.Add(this.lstPartitions);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.lblDesc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgSelectPartition";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select partition";
            this.Load += new System.EventHandler(this.dlgSelectPartition_Load);
            this.pnlBottom.ResumeLayout(false);
            this.gbxDiskInfo.ResumeLayout(false);
            this.gbxDiskInfo.PerformLayout();
            this.gbxPartitionInfo.ResumeLayout(false);
            this.gbxPartitionInfo.PerformLayout();
            this.gbxLoadOptions.ResumeLayout(false);
            this.gbxLoadOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
