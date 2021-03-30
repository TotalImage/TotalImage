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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Filename",
            "<filename>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "File size",
            "<filesize>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Container type",
            "<containertype>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Partitioning scheme",
            "<partitionscheme>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "No. of partitions",
            "<nopartitions>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "Selected partition",
            "<selectedpartition>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "File system",
            "<filesystem>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "Total storage capacity",
            "<capacity>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
            "Free space",
            "<freespace>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
            "No. of files",
            "<nofiles>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] {
            "No. of subdirectories",
            "<subdirectories>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
            "MD5 hash",
            "<md5hash>"}, -1);
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem(new string[] {
            "SHA-1 hash",
            "<sha1hash>"}, -1);
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblComment = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.lstProperties = new System.Windows.Forms.ListView();
            this.clmProperty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(379, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 393);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(471, 50);
            this.pnlBottom.TabIndex = 22;
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(12, 269);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(64, 15);
            this.lblComment.TabIndex = 23;
            this.lblComment.Text = "Comment:";
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(12, 287);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.ReadOnly = true;
            this.txtComment.Size = new System.Drawing.Size(447, 91);
            this.txtComment.TabIndex = 24;
            // 
            // lstProperties
            // 
            this.lstProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmProperty,
            this.clmValue});
            this.lstProperties.FullRowSelect = true;
            this.lstProperties.GridLines = true;
            this.lstProperties.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem1.StateImageIndex = 0;
            listViewItem1.Tag = "Filename";
            listViewItem2.StateImageIndex = 0;
            listViewItem2.Tag = "Filesize";
            listViewItem3.StateImageIndex = 0;
            listViewItem3.Tag = "Container";
            listViewItem4.StateImageIndex = 0;
            listViewItem4.Tag = "Partscheme";
            listViewItem5.StateImageIndex = 0;
            listViewItem5.Tag = "Partcount";
            listViewItem6.StateImageIndex = 0;
            listViewItem6.Tag = "Selpart";
            listViewItem7.StateImageIndex = 0;
            listViewItem7.Tag = "Fs";
            listViewItem8.StateImageIndex = 0;
            listViewItem8.Tag = "Capacity";
            listViewItem9.StateImageIndex = 0;
            listViewItem9.Tag = "Freespace";
            listViewItem10.StateImageIndex = 0;
            listViewItem10.Tag = "Filecount";
            listViewItem11.StateImageIndex = 0;
            listViewItem11.Tag = "Dircount";
            listViewItem12.Tag = "md5";
            listViewItem13.Tag = "sha1";
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
            listViewItem13});
            this.lstProperties.Location = new System.Drawing.Point(12, 12);
            this.lstProperties.MultiSelect = false;
            this.lstProperties.Name = "lstProperties";
            this.lstProperties.ShowGroups = false;
            this.lstProperties.Size = new System.Drawing.Size(447, 245);
            this.lstProperties.TabIndex = 26;
            this.lstProperties.UseCompatibleStateImageBehavior = false;
            this.lstProperties.View = System.Windows.Forms.View.Details;
            // 
            // clmProperty
            // 
            this.clmProperty.Text = "Property";
            this.clmProperty.Width = 210;
            // 
            // clmValue
            // 
            this.clmValue.Text = "Value";
            this.clmValue.Width = 215;
            // 
            // dlgImageInfo
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(471, 443);
            this.Controls.Add(this.lstProperties);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgImageInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image information";
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
    }
}