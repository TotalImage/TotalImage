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

        private void dlgBootSector_Load(object sender, EventArgs e)
        {
            frmMain main = (frmMain)Application.OpenForms["frmMain"];
            byte[] bytes = main.image.GetRawBytes(0, 512);
            
            var sb = new StringBuilder();
            for (int i = 0; i < 512; i++)
            {
                sb.Append(bytes[i].ToString("X2"));
                if ((i + 1) % 16 == 0 && i < 511)
                {
                    sb.Append(Environment.NewLine);
                }
                else
                {
                    sb.Append(" ");
                }
            }

            txtBytes.Text = sb.ToString();
        }
    }
}
