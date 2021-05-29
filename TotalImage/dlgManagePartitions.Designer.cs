namespace TotalImage
{
    partial class dlgManagePartitions
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lstPartitions = new System.Windows.Forms.ListView();
            this.clmNumber = new System.Windows.Forms.ColumnHeader();
            this.clmLabel = new System.Windows.Forms.ColumnHeader();
            this.clmType = new System.Windows.Forms.ColumnHeader();
            this.clmStart = new System.Windows.Forms.ColumnHeader();
            this.clmEnd = new System.Windows.Forms.ColumnHeader();
            this.clmSize = new System.Windows.Forms.ColumnHeader();
            this.clmActive = new System.Windows.Forms.ColumnHeader();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnFormat = new System.Windows.Forms.Button();
            this.btnResize = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnActive = new System.Windows.Forms.Button();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(406, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(492, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 231);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(584, 50);
            this.pnlBottom.TabIndex = 2;
            // 
            // lstPartitions
            // 
            this.lstPartitions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPartitions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmNumber,
            this.clmLabel,
            this.clmType,
            this.clmStart,
            this.clmEnd,
            this.clmSize,
            this.clmActive});
            this.lstPartitions.FullRowSelect = true;
            this.lstPartitions.GridLines = true;
            this.lstPartitions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstPartitions.HideSelection = false;
            this.lstPartitions.Location = new System.Drawing.Point(12, 12);
            this.lstPartitions.MultiSelect = false;
            this.lstPartitions.Name = "lstPartitions";
            this.lstPartitions.ShowGroups = false;
            this.lstPartitions.Size = new System.Drawing.Size(560, 181);
            this.lstPartitions.TabIndex = 7;
            this.lstPartitions.UseCompatibleStateImageBehavior = false;
            this.lstPartitions.View = System.Windows.Forms.View.Details;
            this.lstPartitions.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstPartitions_ItemSelectionChanged);
            // 
            // clmNumber
            // 
            this.clmNumber.Text = "No.";
            this.clmNumber.Width = 31;
            // 
            // clmLabel
            // 
            this.clmLabel.Text = "Volume label";
            this.clmLabel.Width = 88;
            // 
            // clmType
            // 
            this.clmType.Text = "Type";
            this.clmType.Width = 79;
            // 
            // clmStart
            // 
            this.clmStart.Text = "Start";
            this.clmStart.Width = 78;
            // 
            // clmEnd
            // 
            this.clmEnd.Text = "End";
            this.clmEnd.Width = 92;
            // 
            // clmSize
            // 
            this.clmSize.Text = "Size";
            this.clmSize.Width = 87;
            // 
            // clmActive
            // 
            this.clmActive.Text = "Active";
            this.clmActive.Width = 49;
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCreate.Location = new System.Drawing.Point(12, 199);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(80, 26);
            this.btnCreate.TabIndex = 2;
            this.btnCreate.Text = "Create...";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // btnFormat
            // 
            this.btnFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFormat.Enabled = false;
            this.btnFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFormat.Location = new System.Drawing.Point(184, 199);
            this.btnFormat.Name = "btnFormat";
            this.btnFormat.Size = new System.Drawing.Size(80, 26);
            this.btnFormat.TabIndex = 8;
            this.btnFormat.Text = "Format...";
            this.btnFormat.UseVisualStyleBackColor = true;
            // 
            // btnResize
            // 
            this.btnResize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResize.Enabled = false;
            this.btnResize.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnResize.Location = new System.Drawing.Point(270, 199);
            this.btnResize.Name = "btnResize";
            this.btnResize.Size = new System.Drawing.Size(80, 26);
            this.btnResize.TabIndex = 9;
            this.btnResize.Text = "Resize...";
            this.btnResize.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDelete.Location = new System.Drawing.Point(356, 199);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 26);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnActive
            // 
            this.btnActive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnActive.Enabled = false;
            this.btnActive.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnActive.Location = new System.Drawing.Point(98, 199);
            this.btnActive.Name = "btnActive";
            this.btnActive.Size = new System.Drawing.Size(80, 26);
            this.btnActive.TabIndex = 11;
            this.btnActive.Text = "Active";
            this.btnActive.UseVisualStyleBackColor = true;
            // 
            // dlgManagePartitions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 281);
            this.Controls.Add(this.btnActive);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnResize);
            this.Controls.Add(this.btnFormat);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.lstPartitions);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgManagePartitions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage partitions";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.ListView lstPartitions;
        private System.Windows.Forms.ColumnHeader clmNumber;
        private System.Windows.Forms.ColumnHeader clmLabel;
        private System.Windows.Forms.ColumnHeader clmType;
        private System.Windows.Forms.ColumnHeader clmStart;
        private System.Windows.Forms.ColumnHeader clmEnd;
        private System.Windows.Forms.ColumnHeader clmSize;
        private System.Windows.Forms.ColumnHeader clmActive;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnFormat;
        private System.Windows.Forms.Button btnResize;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnActive;
    }
}