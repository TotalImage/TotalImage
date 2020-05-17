using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgExtract : Form
    {
        public string TargetPath { get; private set; }
        public bool OpenFolder { get; private set; }
        public Settings.FolderExtract ExtractType { get; private set; }

        public dlgExtract()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            TargetPath = txtPath.Text;
            OpenFolder = cbxOpenFolder.Checked;
            if (rbnExtractSameFolder.Checked)
            {
                ExtractType = Settings.FolderExtract.Merge;
            }
            else if (rbnIgnoreFolders.Checked)
            {
                ExtractType = Settings.FolderExtract.Ignore;
            }
            else if (rbnPreserveDirs.Checked)
            {
                ExtractType = Settings.FolderExtract.Preserve;
            }
        }

        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select the destination folder...";
            fbd.ShowNewFolderButton = true;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = fbd.SelectedPath;
            }
            fbd.Dispose();
        }
    }
}
