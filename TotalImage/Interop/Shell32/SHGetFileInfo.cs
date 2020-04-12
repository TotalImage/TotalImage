using System;
using System.IO;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Shell32
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [Flags]
        public enum SHGFI : uint
        {
            ADDOVERLAYS = 0x000000020,
            ATTR_SPECIFIED = 0x000020000,
            ATTRIBUTES = 0x000000800,
            DISPLAYNAME = 0x000000200,
            EXETYPE = 0x000002000,
            ICON = 0x000000100,
            ICONLOCATION = 0x000001000,
            LARGEICON = 0x000000000,
            LINKOVERLAY = 0x000008000,
            OPENICON = 0x000000002,
            OVERLAYINDEX = 0x000000040,
            PIDL = 0x000000008,
            SELECTED = 0x000010000,
            SHELLICONSIZE = 0x000000004,
            SMALLICON = 0x000000001,
            SYSICONINDEX = 0x000004000,
            TYPENAME = 0x000000400,
            USEFILEATTRIBUTES = 0x000000010
        }

        [DllImport("shell32.dll", EntryPoint = "SHGetFileInfoW", CharSet = CharSet.Unicode)]
        public static extern IntPtr SHGetFileInfo(
            [MarshalAs(UnmanagedType.LPTStr)] [In] string pszPath,
            [MarshalAs(UnmanagedType.U4)] FileAttributes dwFileAttributes,
            ref SHFILEINFO shinfo,
            uint cbfileInfo,
            SHGFI uFlags);
    }
}
