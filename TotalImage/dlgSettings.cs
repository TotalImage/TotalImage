using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgSettings : Form
    {
        public dlgSettings()
        {
            InitializeComponent();
            settingsFormViewModelBindingSource.DataSource = new ViewModels.SettingsFormViewModel();
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            Settings.LoadDefaults();
            Settings.Save();
            settingsFormViewModelBindingSource.DataSource = new ViewModels.SettingsFormViewModel();
        }

        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            using FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtExtractPath.Text = fbd.SelectedPath;
            }
        }
    }
}
