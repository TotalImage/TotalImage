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
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.lblPreset = new System.Windows.Forms.Label();
            this.lstPresets = new System.Windows.Forms.ComboBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.lblJumpCode = new System.Windows.Forms.Label();
            this.txtJumpCode = new System.Windows.Forms.TextBox();
            this.lblOEMID = new System.Windows.Forms.Label();
            this.txtOEMID = new System.Windows.Forms.TextBox();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(485, 15);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 32);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(592, 15);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 32);
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
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlBottom.Location = new System.Drawing.Point(0, 485);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(708, 62);
            this.pnlBottom.TabIndex = 10;
            // 
            // btnLoad
            // 
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLoad.Location = new System.Drawing.Point(12, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 32);
            this.btnLoad.TabIndex = 11;
            this.btnLoad.Text = "Load...";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnExport
            // 
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExport.Location = new System.Drawing.Point(118, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 32);
            this.btnExport.TabIndex = 12;
            this.btnExport.Text = "Export...";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblPreset
            // 
            this.lblPreset.AutoSize = true;
            this.lblPreset.Location = new System.Drawing.Point(224, 18);
            this.lblPreset.Name = "lblPreset";
            this.lblPreset.Size = new System.Drawing.Size(58, 20);
            this.lblPreset.TabIndex = 13;
            this.lblPreset.Text = "Presets:";
            // 
            // lstPresets
            // 
            this.lstPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstPresets.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lstPresets.FormattingEnabled = true;
            this.lstPresets.Location = new System.Drawing.Point(288, 14);
            this.lstPresets.Name = "lstPresets";
            this.lstPresets.Size = new System.Drawing.Size(302, 28);
            this.lstPresets.TabIndex = 14;
            // 
            // btnApply
            // 
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnApply.Location = new System.Drawing.Point(596, 12);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(100, 32);
            this.btnApply.TabIndex = 15;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // lblJumpCode
            // 
            this.lblJumpCode.AutoSize = true;
            this.lblJumpCode.Location = new System.Drawing.Point(12, 70);
            this.lblJumpCode.Name = "lblJumpCode";
            this.lblJumpCode.Size = new System.Drawing.Size(84, 20);
            this.lblJumpCode.TabIndex = 16;
            this.lblJumpCode.Text = "Jump code:";
            // 
            // txtJumpCode
            // 
            this.txtJumpCode.Location = new System.Drawing.Point(102, 67);
            this.txtJumpCode.Name = "txtJumpCode";
            this.txtJumpCode.Size = new System.Drawing.Size(180, 27);
            this.txtJumpCode.TabIndex = 17;
            // 
            // lblOEMID
            // 
            this.lblOEMID.AutoSize = true;
            this.lblOEMID.Location = new System.Drawing.Point(288, 70);
            this.lblOEMID.Name = "lblOEMID";
            this.lblOEMID.Size = new System.Drawing.Size(63, 20);
            this.lblOEMID.TabIndex = 18;
            this.lblOEMID.Text = "OEM ID:";
            // 
            // txtOEMID
            // 
            this.txtOEMID.Location = new System.Drawing.Point(357, 67);
            this.txtOEMID.Name = "txtOEMID";
            this.txtOEMID.Size = new System.Drawing.Size(180, 27);
            this.txtOEMID.TabIndex = 19;
            // 
            // dlgBootSector
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.pnlBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ClientSize = new System.Drawing.Size(708, 547);
            this.Controls.Add(this.txtOEMID);
            this.Controls.Add(this.lblOEMID);
            this.Controls.Add(this.txtJumpCode);
            this.Controls.Add(this.lblJumpCode);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lstPresets);
            this.Controls.Add(this.lblPreset);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.pnlBottom);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgBootSector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
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
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblPreset;
        private System.Windows.Forms.ComboBox lstPresets;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lblJumpCode;
        private System.Windows.Forms.TextBox txtJumpCode;
        private System.Windows.Forms.Label lblOEMID;
        private System.Windows.Forms.TextBox txtOEMID;
    }
}