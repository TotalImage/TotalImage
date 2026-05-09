using System;
using System.Windows.Forms;

namespace TotalImage
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.SetColorMode(Settings.CurrentSettings.ColorMode);
            Application.Run(new frmMain());
        }
    }
}
