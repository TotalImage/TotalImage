using System;
using System.Runtime.InteropServices;

namespace TotalImage
{
    public class TreeViewEx : System.Windows.Forms.TreeView
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(this.Handle, "explorer", null); //Enable Explorer-like appearance
            SendMessage(this.Handle, 0x1100 + 44, 0x0020, 0x0020); //Enable auto-scroll extended style
        }
    }
}