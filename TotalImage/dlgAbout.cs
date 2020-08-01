using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgAbout : Form
    {
        public dlgAbout()
        {
            InitializeComponent();
        }

        private void dlgAbout_Load(object sender, EventArgs e)
        {
            lblVer.Text = "Version: " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
        }

        private void lnkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lnkGitHub.LinkVisited = true;
            //Process.Start(lnkGitHub.Text);
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = lnkGitHub.Text,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}