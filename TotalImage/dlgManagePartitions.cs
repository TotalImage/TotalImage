using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgManagePartitions : Form
    {
        public dlgManagePartitions()
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
