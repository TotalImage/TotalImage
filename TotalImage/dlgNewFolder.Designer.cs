namespace TotalImage
{
    partial class dlgNewFolder
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
            lblName = new System.Windows.Forms.Label();
            txtName = new System.Windows.Forms.TextBox();
            btnCancel = new System.Windows.Forms.Button();
            btnOK = new System.Windows.Forms.Button();
            lblShortName = new System.Windows.Forms.Label();
            lblShortName1 = new System.Windows.Forms.Label();
            pnlBottom = new System.Windows.Forms.Panel();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(9, 9);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(176, 15);
            lblName.TabIndex = 0;
            lblName.Text = "Enter a name for the new folder:";
            // 
            // txtName
            // 
            txtName.Location = new System.Drawing.Point(12, 36);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(460, 23);
            txtName.TabIndex = 0;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnCancel.Location = new System.Drawing.Point(392, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(80, 26);
            btnCancel.TabIndex = 2;
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
            btnOK.TabIndex = 1;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // lblShortName
            // 
            lblShortName.AutoSize = true;
            lblShortName.Location = new System.Drawing.Point(9, 71);
            lblShortName.Name = "lblShortName";
            lblShortName.Size = new System.Drawing.Size(97, 15);
            lblShortName.TabIndex = 4;
            lblShortName.Text = "Short name (8.3):";
            // 
            // lblShortName1
            // 
            lblShortName1.AutoSize = true;
            lblShortName1.Location = new System.Drawing.Point(107, 71);
            lblShortName1.Name = "lblShortName1";
            lblShortName1.Size = new System.Drawing.Size(80, 15);
            lblShortName1.TabIndex = 5;
            lblShortName1.Text = "<shortname>";
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
            pnlBottom.TabIndex = 6;
            // 
            // dlgNewFolder
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.Color.White;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(484, 151);
            Controls.Add(pnlBottom);
            Controls.Add(lblShortName1);
            Controls.Add(lblShortName);
            Controls.Add(txtName);
            Controls.Add(lblName);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgNewFolder";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "New folder";
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblShortName;
        private System.Windows.Forms.Label lblShortName1;
        private System.Windows.Forms.Panel pnlBottom;
    }
}
