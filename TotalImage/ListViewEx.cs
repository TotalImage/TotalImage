using System;
using System.Runtime.InteropServices;

namespace TotalImage
{
    public class ListViewEx : System.Windows.Forms.ListView
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(this.Handle, "explorer", null); //Enables Explorer-like appearance
            SendMessage(this.Handle, 0x1000 + 54, 0x00010000, 0x00010000); //Enables semi-transparent selection rectangle
        }
    }
}
