using System;
using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgRename : Form
    {
        public string NewName => txtName.Text;
        private readonly string oldname;

        public dlgRename(string oldname)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(oldname))
                throw new ArgumentNullException(nameof(oldname), "oldname cannot be null!");
            this.oldname = oldname;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // TODO: Proper validation and handling of invalid states should be done here...
            if (string.IsNullOrEmpty(NewName))
            {
                throw new Exception("The new name must not be empty");
            }
        }

        private void dlgRename_Load(object sender, EventArgs e)
        {
            lblDesc.Text = $"Enter a new name for {oldname}:";
        }
    }
}
