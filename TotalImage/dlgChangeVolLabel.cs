using System;
using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgChangeVolLabel : Form
    {
        public dlgChangeVolLabel()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            frmMain main = (frmMain)Application.OpenForms["frmMain"];
            main.image.ChangeVolumeLabel(txtLabel.Text.ToUpper().PadRight(11, ' '));
            Close();
        }
    }
}
