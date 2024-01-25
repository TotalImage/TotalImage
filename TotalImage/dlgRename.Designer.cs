namespace TotalImage
{
    partial class dlgRename
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
            pnlBottom = new System.Windows.Forms.Panel();
            btnCancel = new System.Windows.Forms.Button();
            btnOK = new System.Windows.Forms.Button();
            lblDesc = new System.Windows.Forms.Label();
            txtName = new System.Windows.Forms.TextBox();
            lblShortname = new System.Windows.Forms.Label();
            lblShortname1 = new System.Windows.Forms.Label();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 101);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(484, 50);
            pnlBottom.TabIndex = 3;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnCancel.Location = new System.Drawing.Point(392, 12);
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
            btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnOK.Location = new System.Drawing.Point(306, 12);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(80, 26);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // lblDesc
            // 
            lblDesc.AutoSize = true;
            lblDesc.Location = new System.Drawing.Point(9, 9);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new System.Drawing.Size(214, 15);
            lblDesc.TabIndex = 4;
            lblDesc.Text = "Enter a new name for <oldname here>:";
            // 
            // txtName
            // 
            txtName.Location = new System.Drawing.Point(12, 36);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(460, 23);
            txtName.TabIndex = 5;
            // 
            // lblShortname
            // 
            lblShortname.AutoSize = true;
            lblShortname.Location = new System.Drawing.Point(9, 71);
            lblShortname.Name = "lblShortname";
            lblShortname.Size = new System.Drawing.Size(97, 15);
            lblShortname.TabIndex = 6;
            lblShortname.Text = "Short name (8.3):";
            // 
            // lblShortname1
            // 
            lblShortname1.AutoSize = true;
            lblShortname1.Location = new System.Drawing.Point(109, 71);
            lblShortname1.Name = "lblShortname1";
            lblShortname1.Size = new System.Drawing.Size(80, 15);
            lblShortname1.TabIndex = 7;
            lblShortname1.Text = "<shortname>";
            // 
            // dlgRename
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(484, 151);
            Controls.Add(lblShortname1);
            Controls.Add(lblShortname);
            Controls.Add(txtName);
            Controls.Add(lblDesc);
            Controls.Add(pnlBottom);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgRename";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Rename item";
            Load += dlgRename_Load;
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblShortname;
        private System.Windows.Forms.Label lblShortname1;
    }
}
