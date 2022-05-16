using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgExtract : Form
    {
        public string TargetPath { get; private set; } = Settings.CurrentSettings.DefaultExtractPath;
        public bool OpenFolder { get; private set; } = Settings.CurrentSettings.OpenFolderAfterExtract;
        public DirectoryExtractionMode DirectoryExtractionMode { get; private set; }

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
                DirectoryExtractionMode = DirectoryExtractionMode.Merge;
            }
            else if (rbnIgnoreFolders.Checked)
            {
                DirectoryExtractionMode = DirectoryExtractionMode.Skip;
            }
            else if (rbnPreserveDirs.Checked)
            {
                DirectoryExtractionMode = DirectoryExtractionMode.Preserve;
            }
        }

        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            using FolderBrowserDialog fbd = new FolderBrowserDialog
            {
                ShowNewFolderButton = true
            };

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = fbd.SelectedPath;
            }
        }

        private void dlgExtract_Load(object sender, System.EventArgs e)
        {
            txtPath.Text = TargetPath;
            cbxOpenFolder.Checked = OpenFolder;
            switch (Settings.CurrentSettings.DefaultDirectoryExtractionMode)
            {
                case DirectoryExtractionMode.Skip: rbnIgnoreFolders.Checked = true; break;
                case DirectoryExtractionMode.Merge: rbnExtractSameFolder.Checked = true; break;
                case DirectoryExtractionMode.Preserve: rbnPreserveDirs.Checked = true; break;
            }
        }
    }
}
