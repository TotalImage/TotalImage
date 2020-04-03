using System;
using System.IO;
using System.Windows.Forms;
using TotalImage.ImageFormats;

namespace TotalImage
{
    public partial class frmMain : Form
    {
        public static bool surfaceImage;
        public IMG imgMan;

        public frmMain()
        {
            InitializeComponent();
            imgMan = new IMG();
        }

        private void newToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            dlgNewSectorImage dlg = new dlgNewSectorImage();
            dlg.ShowDialog();
        }

        private void newToolStripButton_Click(object sender, System.EventArgs e)
        {
            dlgNewSectorImage dlg = new dlgNewSectorImage();
            dlg.ShowDialog();
        }

        private void saveAsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AutoUpgradeEnabled = true;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            sfd.OverwritePrompt = true;

            if (surfaceImage)
            {
                sfd.Title = "Save surface image...";
                sfd.DefaultExt = "86f";
                sfd.Filter = "86F surface image (*.86f)|*.86f|All files (*.*)|*.*";
            }
            else
            {
                sfd.Title = "Save sector image...";
                sfd.DefaultExt = "img";
                sfd.Filter = "Basic sector image (*.img, *.ima, *.vfd, *.flp, *.dsk)|*.img;*.ima;*.vfd;*.flp;*.dsk|WinImage Compressed Image (*.imz)|*.imz|All files (*.*)|*.*";
            }

            if(sfd.ShowDialog() == DialogResult.OK)
            {
                if (surfaceImage)
                {
                    /* Surface image stuff will be done here */
                }
                else
                {
                    //IMG
                    if (sfd.FilterIndex == 0 || sfd.FileName.EndsWith(".img") || sfd.FileName.EndsWith(".ima") || sfd.FileName.EndsWith(".vfd") || sfd.FileName.EndsWith(".flp") || sfd.FileName.EndsWith(".dsk"))
                    {
                        byte[] imageBytes = imgMan.GetImageBytes();
                        File.WriteAllBytes(sfd.FileName, imageBytes);
                    }
                    //IMZ
                    else if (sfd.FilterIndex == 1 || sfd.FileName.EndsWith(".imz"))
                    {
                        /* IMZ stuff will be done here */
                    }
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AutoUpgradeEnabled = true;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            sfd.OverwritePrompt = true;

            if (surfaceImage)
            {
                sfd.Title = "Save surface image...";
                sfd.DefaultExt = "86f";
                sfd.Filter = "86F surface image (*.86f)|*.86f|All files (*.*)|*.*";
            }
            else
            {
                sfd.Title = "Save sector image...";
                sfd.DefaultExt = "img";
                sfd.Filter = "Basic sector image (*.img, *.ima, *.vfd, *.flp, *.dsk)|*.img;*.ima;*.vfd;*.flp;*.dsk|WinImage Compressed Image (*.imz)|*.imz|All files (*.*)|*.*";
            }

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (surfaceImage)
                {
                    /* Surface image stuff will be done here */
                }
                else
                {
                    //IMG
                    if (sfd.FilterIndex == 0 || sfd.FileName.EndsWith(".img") || sfd.FileName.EndsWith(".ima") || sfd.FileName.EndsWith(".vfd") || sfd.FileName.EndsWith(".flp") || sfd.FileName.EndsWith(".dsk"))
                    {
                        byte[] imageBytes = imgMan.GetImageBytes();
                        File.WriteAllBytes(sfd.FileName, imageBytes);
                    }
                    //IMZ
                    else if (sfd.FilterIndex == 1 || sfd.FileName.EndsWith(".imz"))
                    {
                        /* IMZ stuff will be done here */
                    }
                }
            }
        }
    }
}