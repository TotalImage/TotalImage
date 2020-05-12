using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgExtract : Form
    {
        public string TargetPath { get; private set; }
        public bool OpenFolder { get; private set; }
        public FolderBehaviour ExtractType { get; private set; }
        public enum FolderBehaviour
        { 
            Ignore,
            Merge,
            Preserve
        }

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
                ExtractType = FolderBehaviour.Merge;
            }
            else if (rbnIgnoreFolders.Checked)
            {
                ExtractType = FolderBehaviour.Ignore;
            }
            else if (rbnPreserveDirs.Checked)
            {
                ExtractType = FolderBehaviour.Preserve;
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
