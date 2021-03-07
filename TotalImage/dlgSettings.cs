using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgSettings : Form
    {
        public dlgSettings()
        {
            InitializeComponent();
        }

        private void cbxExtractAsk_CheckedChanged(object sender, System.EventArgs e)
        {
            txtExtractPath.Enabled = !cbxExtractAsk.Checked;
            btnBrowse.Enabled = !cbxExtractAsk.Checked;
            rbnExtractFlat.Enabled = !cbxExtractAsk.Checked;
            rbnIgnoreFolders.Enabled = !cbxExtractAsk.Checked;
            rbnExtractPreserve.Enabled = !cbxExtractAsk.Checked;
            cbxOpenDir.Enabled = !cbxExtractAsk.Checked;
        }
    }
}