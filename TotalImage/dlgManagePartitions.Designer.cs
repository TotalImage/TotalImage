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
            btnOK = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            pnlBottom = new System.Windows.Forms.Panel();
            lstPartitions = new System.Windows.Forms.ListView();
            clmNumber = new System.Windows.Forms.ColumnHeader();
            clmLabel = new System.Windows.Forms.ColumnHeader();
            clmType = new System.Windows.Forms.ColumnHeader();
            clmStart = new System.Windows.Forms.ColumnHeader();
            clmEnd = new System.Windows.Forms.ColumnHeader();
            clmSize = new System.Windows.Forms.ColumnHeader();
            clmActive = new System.Windows.Forms.ColumnHeader();
            btnCreate = new System.Windows.Forms.Button();
            btnFormat = new System.Windows.Forms.Button();
            btnResize = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            btnActive = new System.Windows.Forms.Button();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnOK.Location = new System.Drawing.Point(406, 12);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnCancel.Location = new System.Drawing.Point(492, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(80, 26);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 231);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(584, 50);
            pnlBottom.TabIndex = 2;
            // 
            // lstPartitions
            // 
            lstPartitions.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lstPartitions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { clmNumber, clmLabel, clmType, clmStart, clmEnd, clmSize, clmActive });
            lstPartitions.FullRowSelect = true;
            lstPartitions.GridLines = true;
            lstPartitions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            lstPartitions.Location = new System.Drawing.Point(12, 12);
            lstPartitions.MultiSelect = false;
            lstPartitions.Name = "lstPartitions";
            lstPartitions.ShowGroups = false;
            lstPartitions.Size = new System.Drawing.Size(560, 181);
            lstPartitions.TabIndex = 7;
            lstPartitions.UseCompatibleStateImageBehavior = false;
            lstPartitions.View = System.Windows.Forms.View.Details;
            lstPartitions.ItemSelectionChanged += lstPartitions_ItemSelectionChanged;
            // 
            // clmNumber
            // 
            clmNumber.Text = "No.";
            clmNumber.Width = 31;
            // 
            // clmLabel
            // 
            clmLabel.Text = "Volume label";
            clmLabel.Width = 88;
            // 
            // clmType
            // 
            clmType.Text = "Type";
            clmType.Width = 79;
            // 
            // clmStart
            // 
            clmStart.Text = "Start";
            clmStart.Width = 78;
            // 
            // clmEnd
            // 
            clmEnd.Text = "End";
            clmEnd.Width = 92;
            // 
            // clmSize
            // 
            clmSize.Text = "Size";
            clmSize.Width = 87;
            // 
            // clmActive
            // 
            clmActive.Text = "Active";
            clmActive.Width = 49;
            // 
            // btnCreate
            // 
            btnCreate.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnCreate.Location = new System.Drawing.Point(12, 199);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new System.Drawing.Size(80, 26);
            btnCreate.TabIndex = 2;
            btnCreate.Text = "Create...";
            btnCreate.UseVisualStyleBackColor = true;
            // 
            // btnFormat
            // 
            btnFormat.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnFormat.Enabled = false;
            btnFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnFormat.Location = new System.Drawing.Point(184, 199);
            btnFormat.Name = "btnFormat";
            btnFormat.Size = new System.Drawing.Size(80, 26);
            btnFormat.TabIndex = 8;
            btnFormat.Text = "Format...";
            btnFormat.UseVisualStyleBackColor = true;
            // 
            // btnResize
            // 
            btnResize.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnResize.Enabled = false;
            btnResize.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnResize.Location = new System.Drawing.Point(270, 199);
            btnResize.Name = "btnResize";
            btnResize.Size = new System.Drawing.Size(80, 26);
            btnResize.TabIndex = 9;
            btnResize.Text = "Resize...";
            btnResize.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnDelete.Enabled = false;
            btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnDelete.Location = new System.Drawing.Point(356, 199);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(80, 26);
            btnDelete.TabIndex = 10;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnActive
            // 
            btnActive.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnActive.Enabled = false;
            btnActive.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnActive.Location = new System.Drawing.Point(98, 199);
            btnActive.Name = "btnActive";
            btnActive.Size = new System.Drawing.Size(80, 26);
            btnActive.TabIndex = 11;
            btnActive.Text = "Active";
            btnActive.UseVisualStyleBackColor = true;
            // 
            // dlgManagePartitions
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.Color.White;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(584, 281);
            Controls.Add(btnActive);
            Controls.Add(btnDelete);
            Controls.Add(btnResize);
            Controls.Add(btnFormat);
            Controls.Add(btnCreate);
            Controls.Add(lstPartitions);
            Controls.Add(pnlBottom);
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgManagePartitions";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Manage partitions";
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
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
