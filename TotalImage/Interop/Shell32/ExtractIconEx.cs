using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Shell32
    {
        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern uint ExtractIconEx(string szFileName, int nIconIndex, out SafeIconHandle phiconLarge, out SafeIconHandle phiconSmall, uint nIcons);
    }
}
