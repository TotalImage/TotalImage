namespace TotalImage
{
    partial class dlgBootSector
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
            this.lblBytes = new System.Windows.Forms.Label();
            this.txtBytes = new System.Windows.Forms.RichTextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblOffset = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(388, 12);
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
            this.btnCancel.Location = new System.Drawing.Point(474, 12);
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
            this.pnlBottom.Location = new System.Drawing.Point(0, 669);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(566, 50);
            this.pnlBottom.TabIndex = 10;
            // 
            // lblBytes
            // 
            this.lblBytes.AutoSize = true;
            this.lblBytes.Location = new System.Drawing.Point(12, 9);
            this.lblBytes.Name = "lblBytes";
            this.lblBytes.Size = new System.Drawing.Size(98, 15);
            this.lblBytes.TabIndex = 11;
            this.lblBytes.Text = "Bootsector bytes:";
            // 
            // txtBytes
            // 
            this.txtBytes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBytes.Font = new System.Drawing.Font("Courier New", 12F);
            this.txtBytes.Location = new System.Drawing.Point(59, 71);
            this.txtBytes.Name = "txtBytes";
            this.txtBytes.ReadOnly = true;
            this.txtBytes.Size = new System.Drawing.Size(490, 580);
            this.txtBytes.TabIndex = 12;
            this.txtBytes.Text = "";
            // 
            // lblAddress
            // 
            this.lblAddress.Font = new System.Drawing.Font("Courier New", 12F);
            this.lblAddress.Location = new System.Drawing.Point(15, 71);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(38, 595);
            this.lblAddress.TabIndex = 13;
            this.lblAddress.Text = "000\r\n010\r\n020\r\n030\r\n040\r\n050\r\n060\r\n070\r\n080\r\n090\r\n0A0\r\n0B0\r\n0C0\r\n0D0\r\n0E0\r\n0F0\r\n1" +
    "00\r\n110\r\n120\r\n130\r\n140\r\n150\r\n160\r\n170\r\n180\r\n190\r\n1A0\r\n1B0\r\n1C0\r\n1D0\r\n1E0\r\n1F0";
            // 
            // lblOffset
            // 
            this.lblOffset.Font = new System.Drawing.Font("Courier New", 12F);
            this.lblOffset.Location = new System.Drawing.Point(57, 45);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(493, 23);
            this.lblOffset.TabIndex = 14;
            this.lblOffset.Text = "00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F";
            // 
            // dlgBootSector
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(566, 719);
            this.Controls.Add(this.lblOffset);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtBytes);
            this.Controls.Add(this.lblBytes);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgBootSector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Boot sector properties";
            this.Load += new System.EventHandler(this.dlgBootSector_Load);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblBytes;
        private System.Windows.Forms.RichTextBox txtBytes;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblOffset;
    }
}