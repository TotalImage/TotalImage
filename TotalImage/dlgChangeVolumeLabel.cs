using System;
using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgChangeVolumeLabel : Form
    {
        private readonly string oldLabel;
        private readonly string oldBPBLabel;

        public string NewLabel { get; private set; } = "";
        public string NewBPBLabel { get; private set; } = "";
        public bool WriteBPBLabel { get; private set; }

        public dlgChangeVolumeLabel(string rdLabel, string bpbLabel)
        {
            InitializeComponent();
            oldLabel = rdLabel;
            oldBPBLabel = bpbLabel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            NewLabel = txtNewLabel.Text.ToUpper().PadRight(11, ' ');
            WriteBPBLabel = cbxBPBLabel.Checked;
            NewBPBLabel = txtBPBLabel.Text.ToUpper().PadRight(11, ' ');
        }

        private void cbxBPBLabel_CheckedChanged(object sender, EventArgs e)
        {
            txtBPBLabel.Enabled = cbxBPBLabel.Checked;
            cbxSync.Enabled = cbxBPBLabel.Checked;
            if (!cbxBPBLabel.Checked)
            {
                txtBPBLabel.Text = "";
                cbxSync.Checked = false;
            }
            else
            {
                txtBPBLabel.Text = txtNewLabel.Text;
            }
        }

        private void txtRootDirLabel_TextChanged(object sender, EventArgs e)
        {
            if (cbxBPBLabel.Checked && cbxSync.Checked)
                txtBPBLabel.Text = txtNewLabel.Text;
        }

        private void dlgChangeVolLabel_Load(object sender, EventArgs e)
        {
            txtBPBLabel.Text = oldBPBLabel;
            txtNewLabel.Text = oldLabel;
        }

        private void cbxSync_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxSync.Checked)
            {
                txtBPBLabel.Text = txtNewLabel.Text;
                txtBPBLabel.ReadOnly = true;
            }
            else
            {
                txtBPBLabel.ReadOnly = false;
            }
        }
    }
}
