using System;
using System.Windows.Forms;

namespace TotalImage.Dialogs
{
    public partial class NewFolderDialog : Form
    {
        public string NewName => txtName.Text;

        public NewFolderDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            // TODO: Proper validation and handling of invalid states should be done here...
            if (string.IsNullOrEmpty(NewName))
            {
                throw new Exception("The new name must not be empty");
            }
        }

        private void txtName_TextChanged(object sender, System.EventArgs e)
        {
            // TODO: Convert long name to short (8.3) name here
        }
    }
}
