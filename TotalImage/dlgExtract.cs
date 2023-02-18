using System;
using System.Drawing;
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

            //If directory doesn't exist, ask the user to create it 
            if(!System.IO.Directory.Exists(TargetPath))
            {
                TaskDialogPage page = new TaskDialogPage()
                {
                    Text = $"The target directory you selected does not exist.{Environment.NewLine}Would you like to create it and continue the extraction?",
                    Heading = $"Directory does not exist",
                    Caption = "Extraction",
                    Buttons =
                        {
                            TaskDialogButton.Yes,
                            TaskDialogButton.No
                        },
                    Icon = TaskDialogIcon.Warning,
                    SizeToContent = true
                };
                TaskDialogButton result = TaskDialog.ShowDialog(this, page);

                if(result == TaskDialogButton.Yes)
                {
                    System.IO.Directory.CreateDirectory(TargetPath);
                }
                else
                {
                    DialogResult = DialogResult.None;
                    return;
                }
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
