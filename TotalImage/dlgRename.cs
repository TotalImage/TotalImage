using System;
using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgRename : Form
    {
        public string NewName { get; private set; }
        private string oldname;

        public dlgRename(string oldname)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(oldname))
                throw new ArgumentNullException(nameof(oldname), "oldname cannot be null!");
            this.oldname = oldname;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            NewName = txtName.Text; //Name needs to be validated first...
        }

        private void dlgRename_Load(object sender, EventArgs e)
        {
            lblDesc.Text = "Enter a new name for " + oldname + ":";
        }
    }
}
