using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgSelectPartition : Form
    {
        public bool ReadOnly { get; private set; }
        public int SelectedEntry { get; private set; }

        public dlgSelectPartition()
        {
            InitializeComponent();
        }

        private void lstPartitions_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lstPartitions.SelectedItems.Count >= 1)
            {
                btnOK.Enabled = true;
            }
            else
            {
                btnOK.Enabled = false;
            }
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            ReadOnly = cbxReadOnly.Checked;
            SelectedEntry = int.Parse(lstPartitions.SelectedItems[0].Text);
        }
    }
}