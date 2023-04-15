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
            lblVer.Text = $"Version: {System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version} Alpha 1";
        }

        private void lnkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lnkGitHub.LinkVisited = true;
            ProcessStartInfo psi = new()
            {
                FileName = lnkGitHub.Text,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
