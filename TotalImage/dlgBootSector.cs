using System;
using System.IO;
using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgBootSector : Form
    {
        internal string OEMID;
        internal byte[] JumpCode;

        public dlgBootSector()
        {
            InitializeComponent();
        }

        private void dlgBootSector_Load(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.AutoUpgradeEnabled = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;
            ofd.Filter = "All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(ofd.FileName);
                /*if(fi.Length > 512)
                {
                    //Warn the user
                }*/

                //Apply the bootsector
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using SaveFileDialog sfd = new SaveFileDialog();
            sfd.AutoUpgradeEnabled = true;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            sfd.OverwritePrompt = true;
            sfd.DefaultExt = "img";
            sfd.Filter = "All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                //Write all bytes to the file
            }
        }
    }
}