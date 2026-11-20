using System.Windows.Forms;

namespace TotalImage.Dialogs
{
    public partial class ManagePartitionsDialog : Form
    {
        public ManagePartitionsDialog()
        {
            InitializeComponent();
        }

        private void lstPartitions_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lstPartitions.SelectedItems.Count >= 1)
            {
                btnActive.Enabled = true;
                btnDelete.Enabled = true;
                btnFormat.Enabled = true;
                btnResize.Enabled = true;
            }
            else
            {
                btnActive.Enabled = false;
                btnDelete.Enabled = false;
                btnFormat.Enabled = false;
                btnResize.Enabled = false;
            }
        }
    }
}
