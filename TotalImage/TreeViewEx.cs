using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Windows.Win32.Foundation;

using static Windows.Win32.PInvoke;

namespace TotalImage
{
    public class TreeViewEx : TreeView
    {
        protected override void CreateHandle()
        {
            base.CreateHandle();
            _ = SetWindowTheme((HWND)this.Handle, "explorer", null); //Enable Explorer-like appearance
            SendMessage((HWND)this.Handle, TVM_SETEXTENDEDSTYLE, (nuint)TVS_EX_AUTOHSCROLL, (nint)TVS_EX_AUTOHSCROLL); //Enable auto-scroll extended style
        }
    }
}
