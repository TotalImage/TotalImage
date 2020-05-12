using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgNewFolder : Form
    {
        public string NewName { get; private set; }

        public dlgNewFolder()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            //Name needs to be validated first...
            NewName = txtName.Text; 
        }

        private void txtName_TextChanged(object sender, System.EventArgs e)
        {
            /* Convert long name to short (8.3) name here */
        }
    }
}