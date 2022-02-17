
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
            this.clmLabel = new System.Windows.Forms.ColumnHeader();
            this.clmType = new System.Windows.Forms.ColumnHeader();
            this.clmStart = new System.Windows.Forms.ColumnHeader();
            this.clmEnd = new System.Windows.Forms.ColumnHeader();
            this.clmSize = new System.Windows.Forms.ColumnHeader();
            this.clmActive = new System.Windows.Forms.ColumnHeader();
            this.lblPartioningScheme = new System.Windows.Forms.Label();
            this.lblPartitioningScheme1 = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(15, 14);
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
            this.pnlBottom.Location = new System.Drawing.Point(0, 299);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(730, 62);
            this.pnlBottom.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(615, 15);
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
            this.btnOK.Location = new System.Drawing.Point(508, 15);
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
            this.cbxReadOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxReadOnly.AutoSize = true;
            this.cbxReadOnly.Enabled = false;
            this.cbxReadOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxReadOnly.Location = new System.Drawing.Point(19, 266);
            this.cbxReadOnly.Margin = new System.Windows.Forms.Padding(4);
            this.cbxReadOnly.Name = "cbxReadOnly";
            this.cbxReadOnly.Size = new System.Drawing.Size(159, 25);
            this.cbxReadOnly.TabIndex = 5;
            this.cbxReadOnly.Text = "Load as read-only";
            this.cbxReadOnly.UseVisualStyleBackColor = true;
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
            this.lstPartitions.Location = new System.Drawing.Point(19, 36);
            this.lstPartitions.Margin = new System.Windows.Forms.Padding(4);
            this.lstPartitions.MultiSelect = false;
            this.lstPartitions.Name = "lstPartitions";
            this.lstPartitions.ShowGroups = false;
            this.lstPartitions.Size = new System.Drawing.Size(695, 187);
            this.lstPartitions.TabIndex = 6;
            this.lstPartitions.UseCompatibleStateImageBehavior = false;
            this.lstPartitions.View = System.Windows.Forms.View.Details;
            this.lstPartitions.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstPartitions_ItemSelectionChanged);
            this.lstPartitions.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstPartitions_MouseDoubleClick);
            // 
            // clmNumber
            // 
            this.clmNumber.Text = "No.";
            this.clmNumber.Width = 45;
            // 
            // clmLabel
            // 
            this.clmLabel.Text = "Volume label";
            this.clmLabel.Width = 100;
            // 
            // clmType
            // 
            this.clmType.Text = "Type";
            this.clmType.Width = 120;
            // 
            // clmStart
            // 
            this.clmStart.Text = "Start";
            this.clmStart.Width = 120;
            // 
            // clmEnd
            // 
            this.clmEnd.Text = "End";
            this.clmEnd.Width = 120;
            // 
            // clmSize
            // 
            this.clmSize.Text = "Size";
            this.clmSize.Width = 120;
            // 
            // clmActive
            // 
            this.clmActive.Text = "Active";
            this.clmActive.Width = 65;
            // 
            // lblPartioningScheme
            // 
            this.lblPartioningScheme.AutoSize = true;
            this.lblPartioningScheme.Location = new System.Drawing.Point(15, 236);
            this.lblPartioningScheme.Name = "lblPartioningScheme";
            this.lblPartioningScheme.Size = new System.Drawing.Size(142, 20);
            this.lblPartioningScheme.TabIndex = 7;
            this.lblPartioningScheme.Text = "Partitioning scheme:";
            // 
            // lblPartitioningScheme1
            // 
            this.lblPartitioningScheme1.AutoSize = true;
            this.lblPartitioningScheme1.Location = new System.Drawing.Point(163, 236);
            this.lblPartitioningScheme1.Name = "lblPartitioningScheme1";
            this.lblPartitioningScheme1.Size = new System.Drawing.Size(161, 20);
            this.lblPartitioningScheme1.TabIndex = 8;
            this.lblPartitioningScheme1.Text = "<partitioning scheme>";
            // 
            // dlgSelectPartition
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(730, 361);
            this.Controls.Add(this.lblPartitioningScheme1);
            this.Controls.Add(this.lblPartioningScheme);
            this.Controls.Add(this.lstPartitions);
            this.Controls.Add(this.cbxReadOnly);
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
