using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace TotalImage.Avalonia.Platform.Windows;

/// <summary>
/// File type names and icons from the Windows shell, looked up by extension only (the files
/// don't exist on the computer).
/// </summary>
[SupportedOSPlatform("windows")]
internal static class ShellFileInfo
{
    private static readonly Dictionary<string, string> TypeNames = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// The shell's name for the type of a file or folder, for example "Text Document".
    /// </summary>
    public static string GetTypeName(string fileName, FileAttributes attributes)
    {
        var key = attributes.HasFlag(FileAttributes.Directory) ? "folder" : Path.GetExtension(fileName);
        if (!TypeNames.TryGetValue(key, out var name))
        {
            var info = new SHFILEINFO();
            SHGetFileInfo(fileName, attributes, ref info, (uint)Marshal.SizeOf(info), SHGFI.TYPENAME | SHGFI.USEFILEATTRIBUTES);
            name = info.szTypeName ?? string.Empty;
            TypeNames[key] = name;
        }

        return name;
    }

    /// <summary>
    /// The shell's icon of a file or folder, or <c>null</c> when it has none.
    /// </summary>
    /// <param name="large">The large (32 px at 100%) icon rather than the small one.</param>
    public static Bitmap? GetIcon(string fileName, FileAttributes attributes, bool large)
    {
        var info = new SHFILEINFO();
        var flags = SHGFI.ICON | SHGFI.USEFILEATTRIBUTES | (large ? SHGFI.LARGEICON : SHGFI.SMALLICON);
        if (SHGetFileInfo(fileName, attributes, ref info, (uint)Marshal.SizeOf(info), flags) == IntPtr.Zero || info.hIcon == IntPtr.Zero)
            return null;

        try
        {
            return ToBitmap(info.hIcon);
        }
        finally
        {
            DestroyIcon(info.hIcon);
        }
    }

    // Copies the pixels of an icon into a bitmap.
    private static Bitmap? ToBitmap(IntPtr icon)
    {
        if (!GetIconInfo(icon, out var iconInfo))
            return null;

        try
        {
            if (iconInfo.hbmColor == IntPtr.Zero || GetObject(iconInfo.hbmColor, Marshal.SizeOf<BITMAP>(), out var bitmap) == 0)
                return null;

            int width = bitmap.bmWidth, height = bitmap.bmHeight;
            var pixels = ReadPixels(iconInfo.hbmColor, width, height);
            if (pixels is null)
                return null;

            // Icons without an alpha channel use their mask for transparency.
            if (!HasAlpha(pixels) && iconInfo.hbmMask != IntPtr.Zero && ReadPixels(iconInfo.hbmMask, width, height) is { } mask)
            {
                for (int i = 0; i < pixels.Length; i += 4)
                    pixels[i + 3] = mask[i] == 0 ? (byte)255 : (byte)0;
            }
            else if (!HasAlpha(pixels))
            {
                for (int i = 3; i < pixels.Length; i += 4)
                    pixels[i] = 255;
            }

            var handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            try
            {
                return new Bitmap(PixelFormat.Bgra8888, AlphaFormat.Unpremul, handle.AddrOfPinnedObject(),
                    new PixelSize(width, height), new Vector(96, 96), width * 4);
            }
            finally
            {
                handle.Free();
            }
        }
        finally
        {
            if (iconInfo.hbmColor != IntPtr.Zero)
                DeleteObject(iconInfo.hbmColor);
            if (iconInfo.hbmMask != IntPtr.Zero)
                DeleteObject(iconInfo.hbmMask);
        }
    }

    private static bool HasAlpha(byte[] pixels)
    {
        for (int i = 3; i < pixels.Length; i += 4)
        {
            if (pixels[i] != 0)
                return true;
        }

        return false;
    }

    // Reads a bitmap as 32-bit BGRA, top row first.
    private static byte[]? ReadPixels(IntPtr hBitmap, int width, int height)
    {
        var header = new BITMAPINFOHEADER
        {
            biSize = (uint)Marshal.SizeOf<BITMAPINFOHEADER>(),
            biWidth = width,
            biHeight = -height,
            biPlanes = 1,
            biBitCount = 32,
            biCompression = 0 // BI_RGB
        };

        var pixels = new byte[width * height * 4];
        var dc = GetDC(IntPtr.Zero);
        try
        {
            return GetDIBits(dc, hBitmap, 0, (uint)height, pixels, ref header, 0) == 0 ? null : pixels;
        }
        finally
        {
            ReleaseDC(IntPtr.Zero, dc);
        }
    }

    #region Native methods

    [Flags]
    private enum SHGFI : uint
    {
        ICON = 0x000000100,
        LARGEICON = 0x000000000,
        SMALLICON = 0x000000001,
        TYPENAME = 0x000000400,
        USEFILEATTRIBUTES = 0x000000010
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ICONINFO
    {
        public bool fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct BITMAP
    {
        public int bmType;
        public int bmWidth;
        public int bmHeight;
        public int bmWidthBytes;
        public ushort bmPlanes;
        public ushort bmBitsPixel;
        public IntPtr bmBits;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;
    }

    [DllImport("shell32.dll", EntryPoint = "SHGetFileInfoW", CharSet = CharSet.Unicode)]
    private static extern IntPtr SHGetFileInfo(string pszPath, FileAttributes dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool DestroyIcon(IntPtr hIcon);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

    [DllImport("gdi32.dll")]
    private static extern int GetObject(IntPtr h, int c, out BITMAP pv);

    [DllImport("gdi32.dll")]
    private static extern int GetDIBits(IntPtr hdc, IntPtr hbm, uint start, uint cLines, [Out] byte[] lpvBits, ref BITMAPINFOHEADER lpbmi, uint usage);

    [DllImport("gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool DeleteObject(IntPtr ho);

    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    #endregion
}
