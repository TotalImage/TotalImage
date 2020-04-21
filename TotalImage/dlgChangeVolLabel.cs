using System;
using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgChangeVolLabel : Form
    {
        private string rdLabel;
        private string bpbLabel;

        public dlgChangeVolLabel(string rdLabel, string bpbLabel)
        {
            InitializeComponent();
            this.rdLabel = rdLabel;
            this.bpbLabel = bpbLabel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            frmMain main = (frmMain)Application.OpenForms["frmMain"];
            main.image.ChangeVolumeLabel(txtRootDirLabel.Text.ToUpper().PadRight(11, ' '));
            Close();
        }

        private void cbxBPBLabel_CheckedChanged(object sender, EventArgs e)
        {
            txtBPBLabel.ReadOnly = cbxBPBLabel.Checked;
            txtBPBLabel.Text = txtRootDirLabel.Text;
        }

        private void txtRootDirLabel_TextChanged(object sender, EventArgs e)
        {
            if (cbxBPBLabel.Checked)
            {
                txtBPBLabel.Text = txtRootDirLabel.Text;
            }
        }

        private void dlgChangeVolLabel_Load(object sender, EventArgs e)
        {
            txtBPBLabel.Text = bpbLabel;
            txtRootDirLabel.Text = rdLabel;
        }
    }
}
