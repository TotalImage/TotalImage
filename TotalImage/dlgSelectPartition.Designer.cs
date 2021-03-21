
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
            this.clmNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmStart = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmEnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmActive = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(12, 11);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(138, 15);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "Select a partition to load:";
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 185);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(537, 50);
            this.pnlBottom.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(445, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
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
            this.btnOK.Location = new System.Drawing.Point(359, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbxReadOnly
            // 
            this.cbxReadOnly.AutoSize = true;
            this.cbxReadOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxReadOnly.Location = new System.Drawing.Point(15, 159);
            this.cbxReadOnly.Name = "cbxReadOnly";
            this.cbxReadOnly.Size = new System.Drawing.Size(86, 20);
            this.cbxReadOnly.TabIndex = 5;
            this.cbxReadOnly.Text = "Read-only";
            this.cbxReadOnly.UseVisualStyleBackColor = true;
            // 
            // lstPartitions
            // 
            this.lstPartitions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmNumber,
            this.clmType,
            this.clmStart,
            this.clmEnd,
            this.clmSize,
            this.clmActive});
            this.lstPartitions.FullRowSelect = true;
            this.lstPartitions.GridLines = true;
            this.lstPartitions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstPartitions.HideSelection = false;
            this.lstPartitions.Location = new System.Drawing.Point(15, 29);
            this.lstPartitions.MultiSelect = false;
            this.lstPartitions.Name = "lstPartitions";
            this.lstPartitions.ShowGroups = false;
            this.lstPartitions.Size = new System.Drawing.Size(510, 124);
            this.lstPartitions.TabIndex = 6;
            this.lstPartitions.UseCompatibleStateImageBehavior = false;
            this.lstPartitions.View = System.Windows.Forms.View.Details;
            this.lstPartitions.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstPartitions_ItemSelectionChanged);
            // 
            // clmNumber
            // 
            this.clmNumber.Text = "No.";
            this.clmNumber.Width = 41;
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
            this.clmSize.Width = 85;
            // 
            // clmActive
            // 
            this.clmActive.Text = "Active";
            this.clmActive.Width = 59;
            // 
            // dlgSelectPartition
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(537, 235);
            this.Controls.Add(this.lstPartitions);
            this.Controls.Add(this.cbxReadOnly);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.lblDesc);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgSelectPartition";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select partition";
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
    }
}