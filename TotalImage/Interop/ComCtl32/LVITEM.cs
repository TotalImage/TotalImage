using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class ComCtl32
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct LVITEM
        {
            public LVIF mask;

            public int iItem;

            public int iSubItem;

            public LVIS state;

            public LVIS stateMask;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszText;

            public int cchTextMax;

            public int iImage;

            public IntPtr lParam;

            public int iIndent;

            public int iGroupId;

            public uint cColumns;

            public IntPtr puColumns;
        };
    }
}