using System;
using System.Runtime.InteropServices;
using static Interop.ComCtl32;

internal static partial class Interop
{
    internal static partial class Shell32
    {
        public enum SHIL
        {
            LARGE,
            SMALL,
            EXTRALARGE,
            SYSSMALL,
            JUMBO
        }

        [DllImport("shell32.dll", EntryPoint = "#727")]
        public extern static int SHGetImageList(SHIL iImageList, ref Guid riid, out IImageList ppv);
    }
}