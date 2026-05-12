namespace TotalImage
{
    partial class dlgFilePreview
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
            txtContents = new System.Windows.Forms.RichTextBox();
            SuspendLayout();
            // 
            // txtContents
            // 
            txtContents.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtContents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtContents.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            txtContents.Location = new System.Drawing.Point(12, 12);
            txtContents.Name = "txtContents";
            txtContents.ReadOnly = true;
            txtContents.Size = new System.Drawing.Size(960, 737);
            txtContents.TabIndex = 0;
            txtContents.Text = "";
            // 
            // dlgFilePreview
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            ClientSize = new System.Drawing.Size(984, 761);
            Controls.Add(txtContents);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dlgFilePreview";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "File preview";
            ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.RichTextBox txtContents;
    }
}