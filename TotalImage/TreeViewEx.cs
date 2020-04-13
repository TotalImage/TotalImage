using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Interop.UxTheme;

namespace TotalImage
{
    public class TreeViewEx : TreeView
    {
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