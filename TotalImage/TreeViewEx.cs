using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Interop.ComCtl32;
using static Interop.User32;
using static Interop.UxTheme;

namespace TotalImage
{
    public class TreeViewEx : TreeView
    {
        protected override void CreateHandle()
        {
            base.CreateHandle();
            _ = SetWindowTheme(this.Handle, "explorer", null); //Enable Explorer-like appearance
            SendMessage(this.Handle, (uint)TVM.SETEXTENDEDSTYLE, (IntPtr)TVS_EX.AUTOHSCROLL, (IntPtr)TVS_EX.AUTOHSCROLL); //Enable auto-scroll extended style
        }
    }
}