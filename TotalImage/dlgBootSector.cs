using System;
using System.Text;
using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgBootSector : Form
    {
        public dlgBootSector()
        {
            InitializeComponent();
        }

        //TODO: Get the relevant data from the main form etc. and display it in a text and hex view
        private void dlgBootSector_Load(object sender, EventArgs e)
        {
            frmMain main = (frmMain)Application.OpenForms["frmMain"];
            if (main.image == null)
            {
                return;
            }

            byte[] bytes = main.image.GetRawBytes(0, 512);
        }
    }
}
