namespace TotalImage
{
    partial class dlgImageInfo
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Container information", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Partition information", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("File system information", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Miscellaneous", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "File name",
            "<filename>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "File size",
            "<filesize>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Container type",
            "<containertype>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Container subtype",
            "<containersubtype>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "Created by",
            "<createdby>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "Partitioning scheme",
            "<partitionscheme>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "No. of partitions",
            "<nopartitions>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "Selected partition",
            "<selectedpartition>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
            "Partition ID/type",
            "<selectedpartitionID>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
            "File system",
            "<filesystem>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] {
            "Volume label",
            "<volumelabel>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
            "Volume serial number",
            "<volumeserial>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem(new string[] {
            "Total storage capacity",
            "<capacity>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem(new string[] {
            "Free space",
            "<freespace>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem(new string[] {
            "Files",
            "<nofiles>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem(new string[] {
            "Subdirectories",
            "<subdirectories>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem(new string[] {
            "MD5 hash",
            "<md5hash>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem(new string[] {
            "SHA-1 hash",
            "<sha1hash>"}, -1);
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblComment = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.lstProperties = new System.Windows.Forms.ListView();
            this.clmProperty = new System.Windows.Forms.ColumnHeader();
            this.clmValue = new System.Windows.Forms.ColumnHeader();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(478, 17);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 32);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 492);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(591, 62);
            this.pnlBottom.TabIndex = 22;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSave.Location = new System.Drawing.Point(13, 17);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 32);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save...";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(15, 336);
            this.lblComment.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(77, 20);
            this.lblComment.TabIndex = 23;
            this.lblComment.Text = "Comment:";
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(15, 359);
            this.txtComment.Margin = new System.Windows.Forms.Padding(4);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.ReadOnly = true;
            this.txtComment.Size = new System.Drawing.Size(560, 113);
            this.txtComment.TabIndex = 1;
            // 
            // lstProperties
            // 
            this.lstProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmProperty,
            this.clmValue});
            this.lstProperties.FullRowSelect = true;
            listViewGroup1.Header = "Container information";
            listViewGroup1.Name = "grpContainerInfo";
            listViewGroup2.Header = "Partition information";
            listViewGroup2.Name = "grpPartitionInfo";
            listViewGroup3.Header = "File system information";
            listViewGroup3.Name = "grpFsInfo";
            listViewGroup4.Header = "Miscellaneous";
            listViewGroup4.Name = "grpMiscInfo";
            this.lstProperties.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4});
            this.lstProperties.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstProperties.HideSelection = false;
            listViewItem1.Group = listViewGroup1;
            listViewItem1.StateImageIndex = 0;
            listViewItem1.Tag = "File name";
            listViewItem2.Group = listViewGroup1;
            listViewItem2.StateImageIndex = 0;
            listViewItem2.Tag = "Filesize";
            listViewItem3.Group = listViewGroup1;
            listViewItem3.StateImageIndex = 0;
            listViewItem3.Tag = "Container";
            listViewItem4.Group = listViewGroup1;
            listViewItem5.Group = listViewGroup1;
            listViewItem6.Group = listViewGroup2;
            listViewItem6.StateImageIndex = 0;
            listViewItem6.Tag = "Partscheme";
            listViewItem7.Group = listViewGroup2;
            listViewItem7.StateImageIndex = 0;
            listViewItem7.Tag = "Partcount";
            listViewItem8.Group = listViewGroup2;
            listViewItem8.StateImageIndex = 0;
            listViewItem8.Tag = "Selpart";
            listViewItem9.Group = listViewGroup2;
            listViewItem9.Tag = "PartID";
            listViewItem10.Group = listViewGroup3;
            listViewItem10.StateImageIndex = 0;
            listViewItem10.Tag = "Fs";
            listViewItem11.Group = listViewGroup3;
            listViewItem12.Group = listViewGroup3;
            listViewItem12.Tag = "VolSerial";
            listViewItem13.Group = listViewGroup3;
            listViewItem13.StateImageIndex = 0;
            listViewItem13.Tag = "Capacity";
            listViewItem14.Group = listViewGroup3;
            listViewItem14.StateImageIndex = 0;
            listViewItem14.Tag = "Freespace";
            listViewItem15.Group = listViewGroup3;
            listViewItem15.StateImageIndex = 0;
            listViewItem15.Tag = "Filecount";
            listViewItem16.Group = listViewGroup3;
            listViewItem16.StateImageIndex = 0;
            listViewItem16.Tag = "Dircount";
            listViewItem17.Group = listViewGroup4;
            listViewItem17.Tag = "md5";
            listViewItem18.Group = listViewGroup4;
            listViewItem18.Tag = "sha1";
            this.lstProperties.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11,
            listViewItem12,
            listViewItem13,
            listViewItem14,
            listViewItem15,
            listViewItem16,
            listViewItem17,
            listViewItem18});
            this.lstProperties.LabelWrap = false;
            this.lstProperties.Location = new System.Drawing.Point(15, 15);
            this.lstProperties.Margin = new System.Windows.Forms.Padding(4);
            this.lstProperties.MultiSelect = false;
            this.lstProperties.Name = "lstProperties";
            this.lstProperties.Size = new System.Drawing.Size(560, 305);
            this.lstProperties.TabIndex = 0;
            this.lstProperties.UseCompatibleStateImageBehavior = false;
            this.lstProperties.View = System.Windows.Forms.View.Details;
            // 
            // clmProperty
            // 
            this.clmProperty.Text = "Property";
            this.clmProperty.Width = 175;
            // 
            // clmValue
            // 
            this.clmValue.Text = "Value";
            this.clmValue.Width = 250;
            // 
            // dlgImageInfo
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(591, 554);
            this.Controls.Add(this.lstProperties);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgImageInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Image information";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dlgImageInfo_FormClosing);
            this.Load += new System.EventHandler(this.dlgImageInfo_Load);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.ListView lstProperties;
        private System.Windows.Forms.ColumnHeader clmProperty;
        private System.Windows.Forms.ColumnHeader clmValue;
        private System.Windows.Forms.Button btnSave;
    }
}