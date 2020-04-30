using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgExtract : Form
    {
        public dlgExtract()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
