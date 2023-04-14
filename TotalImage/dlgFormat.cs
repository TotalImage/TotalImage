using System;
using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgFormat : Form
    {
        public dlgFormat()
        {
            InitializeComponent();
        }

        //TODO: Obtain the list of available file systems and cluster sizes for this particular image and display them
        private void dlgFormat_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            TaskDialogButton result = TaskDialog.ShowDialog(this, new TaskDialogPage()
            {
                Text = "Are you sure you want to continue?",
                Heading = "WARNING: Formatting will erase all data in the current partition!",
                Caption = "Warning",
                Buttons =
                        {
                            TaskDialogButton.Yes,
                            TaskDialogButton.No
                        },
                Icon = TaskDialogIcon.Warning,
            });

            if (result == TaskDialogButton.Yes)
            {
                throw new NotImplementedException();
            }
            else
            {
                DialogResult = DialogResult.None;
            }
        }
    }
}
