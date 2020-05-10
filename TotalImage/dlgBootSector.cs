using System;
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
            byte[] bytes = new byte[512];
            Array.Copy(main.image.GetImageBytes(), 0, bytes, 0, 512);
            //txtBytes.Text = "    00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F" + Environment.NewLine;
            for(int i = 0; i < 512; i++)
            {
                /*if(i % 16 == 0)
                {
                    txtBytes.Text += (i).ToString("X3") + " ";
                }*/
                txtBytes.Text += bytes[i].ToString("X2");
                if ((i + 1) % 16 == 0 && i < 511)
                {
                    txtBytes.Text += Environment.NewLine;
                }
                else
                {
                    txtBytes.Text += " ";
                }
            }
        }
    }
}
