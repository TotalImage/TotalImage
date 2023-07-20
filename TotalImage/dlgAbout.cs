using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
            var asm = Assembly.GetEntryAssembly();
            var attrs = asm.GetCustomAttributes<AssemblyMetadataAttribute>();
            var hash = attrs.FirstOrDefault(a => a.Key == "GitHash");
            lblVer.Text = $"Version: {asm.GetName().Version} ({hash.Value})";
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
