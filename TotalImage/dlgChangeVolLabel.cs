using System;
using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgChangeVolLabel : Form
    {
        private string oldRDLabel;
        private string oldBPBLabel;
        public string NewRDLabel { get; private set; }
        public string NewBPBLabel { get; private set; }
        public bool WriteBPBLabel { get; private set; }

        public dlgChangeVolLabel(string rdLabel, string bpbLabel)
        {
            InitializeComponent();
            oldRDLabel = rdLabel;
            oldBPBLabel = bpbLabel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            NewRDLabel = txtRootDirLabel.Text.ToUpper().PadRight(11, ' ');
            WriteBPBLabel = cbxBPBLabel.Checked;
            NewBPBLabel = txtBPBLabel.Text;
        }

        private void cbxBPBLabel_CheckedChanged(object sender, EventArgs e)
        {
            txtBPBLabel.Enabled = cbxBPBLabel.Checked;
            cbxSync.Enabled = cbxBPBLabel.Checked;
            txtBPBLabel.Text = txtRootDirLabel.Text;
        }

        private void txtRootDirLabel_TextChanged(object sender, EventArgs e)
        {
            if (cbxBPBLabel.Checked && cbxSync.Checked)
                txtBPBLabel.Text = txtRootDirLabel.Text;
        }

        private void dlgChangeVolLabel_Load(object sender, EventArgs e)
        {
            txtBPBLabel.Text = oldBPBLabel;
            txtRootDirLabel.Text = oldRDLabel;
        }

        private void cbxSync_CheckedChanged(object sender, EventArgs e)
        {
            if(cbxSync.Checked)
                txtBPBLabel.Text = txtRootDirLabel.Text;
        }

        private void txtBPBLabel_TextChanged(object sender, EventArgs e)
        {
            if (cbxBPBLabel.Checked && cbxSync.Checked)
                txtRootDirLabel.Text = txtBPBLabel.Text;
        }
    }
}
