namespace TotalImage
{
    partial class dlgDeletedObjects
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
            lstDeletedObjects = new System.Windows.Forms.ListView();
            clmName = new System.Windows.Forms.ColumnHeader();
            clmLocation = new System.Windows.Forms.ColumnHeader();
            clmType = new System.Windows.Forms.ColumnHeader();
            clmSize = new System.Windows.Forms.ColumnHeader();
            clmAttributes = new System.Windows.Forms.ColumnHeader();
            btnUndelete = new System.Windows.Forms.Button();
            btnWipe = new System.Windows.Forms.Button();
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
            btnOK.TabIndex = 3;
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
            btnCancel.TabIndex = 4;
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
            // lstDeletedObjects
            // 
            lstDeletedObjects.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lstDeletedObjects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { clmName, clmLocation, clmType, clmSize, clmAttributes });
            lstDeletedObjects.FullRowSelect = true;
            lstDeletedObjects.GridLines = true;
            lstDeletedObjects.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            lstDeletedObjects.Location = new System.Drawing.Point(12, 12);
            lstDeletedObjects.MultiSelect = false;
            lstDeletedObjects.Name = "lstDeletedObjects";
            lstDeletedObjects.ShowGroups = false;
            lstDeletedObjects.Size = new System.Drawing.Size(560, 181);
            lstDeletedObjects.TabIndex = 0;
            lstDeletedObjects.UseCompatibleStateImageBehavior = false;
            lstDeletedObjects.View = System.Windows.Forms.View.Details;
            // 
            // clmName
            // 
            clmName.Text = "Name";
            clmName.Width = 100;
            // 
            // clmLocation
            // 
            clmLocation.Text = "Location";
            clmLocation.Width = 190;
            // 
            // clmType
            // 
            clmType.Text = "Type";
            clmType.Width = 90;
            // 
            // clmSize
            // 
            clmSize.Text = "Size";
            clmSize.Width = 90;
            // 
            // clmAttributes
            // 
            clmAttributes.Text = "Attributes";
            clmAttributes.Width = 80;
            // 
            // btnUndelete
            // 
            btnUndelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnUndelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnUndelete.Location = new System.Drawing.Point(12, 199);
            btnUndelete.Name = "btnUndelete";
            btnUndelete.Size = new System.Drawing.Size(80, 26);
            btnUndelete.TabIndex = 1;
            btnUndelete.Text = "Undelete";
            btnUndelete.UseVisualStyleBackColor = true;
            // 
            // btnWipe
            // 
            btnWipe.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnWipe.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnWipe.Location = new System.Drawing.Point(98, 199);
            btnWipe.Name = "btnWipe";
            btnWipe.Size = new System.Drawing.Size(80, 26);
            btnWipe.TabIndex = 2;
            btnWipe.Text = "Wipe";
            btnWipe.UseVisualStyleBackColor = true;
            // 
            // dlgDeletedObjects
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(584, 281);
            Controls.Add(btnWipe);
            Controls.Add(btnUndelete);
            Controls.Add(lstDeletedObjects);
            Controls.Add(pnlBottom);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgDeletedObjects";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Deleted objects";
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.ListView lstDeletedObjects;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ColumnHeader clmAttributes;
        private System.Windows.Forms.ColumnHeader clmLocation;
        private System.Windows.Forms.ColumnHeader clmType;
        private System.Windows.Forms.ColumnHeader clmSize;
        private System.Windows.Forms.Button btnUndelete;
        private System.Windows.Forms.Button btnWipe;
        private System.Windows.Forms.ColumnHeader clmNumber;
    }
}
