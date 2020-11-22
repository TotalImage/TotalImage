using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class UxTheme
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string? pszSubIdList);
    }
}