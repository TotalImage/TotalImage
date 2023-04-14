
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
            clmLabel = new System.Windows.Forms.ColumnHeader();
            clmType = new System.Windows.Forms.ColumnHeader();
            clmStart = new System.Windows.Forms.ColumnHeader();
            clmEnd = new System.Windows.Forms.ColumnHeader();
            clmSize = new System.Windows.Forms.ColumnHeader();
            clmActive = new System.Windows.Forms.ColumnHeader();
            lblPartioningScheme = new System.Windows.Forms.Label();
            lblPartitioningScheme1 = new System.Windows.Forms.Label();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // lblDesc
            // 
            lblDesc.AutoSize = true;
            lblDesc.Location = new System.Drawing.Point(12, 11);
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
            pnlBottom.Location = new System.Drawing.Point(0, 239);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(584, 50);
            pnlBottom.TabIndex = 3;
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
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Enabled = false;
            btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnOK.Location = new System.Drawing.Point(406, 12);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // cbxReadOnly
            // 
            cbxReadOnly.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            cbxReadOnly.AutoSize = true;
            cbxReadOnly.Enabled = false;
            cbxReadOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            cbxReadOnly.Location = new System.Drawing.Point(15, 213);
            cbxReadOnly.Name = "cbxReadOnly";
            cbxReadOnly.Size = new System.Drawing.Size(126, 20);
            cbxReadOnly.TabIndex = 5;
            cbxReadOnly.Text = "Load as read-only";
            cbxReadOnly.UseVisualStyleBackColor = true;
            // 
            // lstPartitions
            // 
            lstPartitions.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lstPartitions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { clmNumber, clmLabel, clmType, clmStart, clmEnd, clmSize, clmActive });
            lstPartitions.FullRowSelect = true;
            lstPartitions.GridLines = true;
            lstPartitions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            lstPartitions.Location = new System.Drawing.Point(15, 29);
            lstPartitions.MultiSelect = false;
            lstPartitions.Name = "lstPartitions";
            lstPartitions.ShowGroups = false;
            lstPartitions.Size = new System.Drawing.Size(557, 150);
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
            // clmLabel
            // 
            clmLabel.Text = "Volume label";
            clmLabel.Width = 100;
            // 
            // clmType
            // 
            clmType.Text = "Type";
            clmType.Width = 120;
            // 
            // clmStart
            // 
            clmStart.Text = "Start";
            clmStart.Width = 120;
            // 
            // clmEnd
            // 
            clmEnd.Text = "End";
            clmEnd.Width = 120;
            // 
            // clmSize
            // 
            clmSize.Text = "Size";
            clmSize.Width = 120;
            // 
            // clmActive
            // 
            clmActive.Text = "Active";
            clmActive.Width = 65;
            // 
            // lblPartioningScheme
            // 
            lblPartioningScheme.AutoSize = true;
            lblPartioningScheme.Location = new System.Drawing.Point(12, 189);
            lblPartioningScheme.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartioningScheme.Name = "lblPartioningScheme";
            lblPartioningScheme.Size = new System.Drawing.Size(116, 15);
            lblPartioningScheme.TabIndex = 7;
            lblPartioningScheme.Text = "Partitioning scheme:";
            // 
            // lblPartitioningScheme1
            // 
            lblPartitioningScheme1.AutoSize = true;
            lblPartitioningScheme1.Location = new System.Drawing.Point(130, 189);
            lblPartitioningScheme1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblPartitioningScheme1.Name = "lblPartitioningScheme1";
            lblPartitioningScheme1.Size = new System.Drawing.Size(129, 15);
            lblPartitioningScheme1.TabIndex = 8;
            lblPartitioningScheme1.Text = "<partitioning scheme>";
            // 
            // dlgSelectPartition
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.Color.White;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(584, 289);
            Controls.Add(lblPartitioningScheme1);
            Controls.Add(lblPartioningScheme);
            Controls.Add(lstPartitions);
            Controls.Add(cbxReadOnly);
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
        private System.Windows.Forms.ColumnHeader clmType;
        private System.Windows.Forms.ColumnHeader clmStart;
        private System.Windows.Forms.ColumnHeader clmEnd;
        private System.Windows.Forms.ColumnHeader clmActive;
        private System.Windows.Forms.ColumnHeader clmSize;
        private System.Windows.Forms.ColumnHeader clmLabel;
        private System.Windows.Forms.Label lblPartioningScheme;
        private System.Windows.Forms.Label lblPartitioningScheme1;
    }
}
