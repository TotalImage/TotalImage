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

        private void btnClearRecent_Click(object sender, System.EventArgs e)
        {
            Settings.ClearRecentImages();

#if NET5_0
            TaskDialog.ShowDialog(this, new TaskDialogPage()
            {
                Text = "List of recently opened images has been successfully cleared.",
                Heading = "Recent images cleared",
                Caption = "Success",
                Buttons =
                {
                    TaskDialogButton.OK
                },
                Icon = TaskDialogIcon.Information,
                DefaultButton = TaskDialogButton.OK
            });
#endif
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
#if NET5_0
            TaskDialogButton result = TaskDialog.ShowDialog(this, new TaskDialogPage()
            {
                Text = "All settings will be reset to their default values.",
                Heading = "Are you sure you want to continue?",
                Caption = "Warning",
                Buttons =
                {
                    TaskDialogButton.Yes,
                    TaskDialogButton.No
                },
                Icon = TaskDialogIcon.Warning,
                DefaultButton = TaskDialogButton.No
            });

            if(result == TaskDialogButton.Yes)
            {
                Settings.LoadDefaults();
                Settings.Save();

                TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = "All settings were successfully reset to their default values.",
                    Heading = "Settings reset",
                    Caption = "Success",
                    Buttons =
                {
                    TaskDialogButton.OK
                },
                    Icon = TaskDialogIcon.Information,
                    DefaultButton = TaskDialogButton.OK
                });
            }
#endif
        }
    }
}