using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static Interop.ComCtl32;
using static Interop.Shell32;
using static Interop.User32;

namespace TotalImage;

internal static class ShellInterop
{
    static Dictionary<string, (string Name, int IconIndex)> _fileTypes = new(StringComparer.InvariantCultureIgnoreCase);

    static (string Name, int IconIndex) GetFileTypeInfo(string fileName, FileAttributes attributes)
    {
        var extension = attributes.HasFlag(FileAttributes.Directory) ? "folder" : Path.GetExtension(fileName);

        if (!_fileTypes.ContainsKey(extension))
        {
            var shinfo = new SHFILEINFO();
            var flags = SHGFI.TYPENAME | SHGFI.SYSICONINDEX | SHGFI.USEFILEATTRIBUTES;

            SHGetFileInfo(fileName, attributes, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);

            _fileTypes.Add(extension, (shinfo.szTypeName.ToString(), shinfo.iIcon));
        }

        return _fileTypes[extension];
    }

    public static string GetFileTypeName(string fileName, FileAttributes attributes) => GetFileTypeInfo(fileName, attributes).Name;
    public static int GetFileTypeIconIndex(string fileName, FileAttributes attributes) => GetFileTypeInfo(fileName, attributes).IconIndex;

    public static Icon GetFileTypeIcon(int index, bool large)
    {
        var iid = new Guid(IID_IImageList);
        _ = SHGetImageList(large ? SHIL.LARGE : SHIL.SMALL, ref iid, out var list);

        list.GetIcon(index, ILD.TRANSPARENT, out var iconHandle);

        using (iconHandle)
        {
            return iconHandle.ToIcon();
        }
    }

    public static Icon GetFileTypeIcon(string fileName, FileAttributes attributes, bool large = true)
        => GetFileTypeIcon(GetFileTypeIconIndex(fileName, attributes), large);

    public static Icon LargeFolderIcon => GetFileTypeIcon(GetFileTypeIconIndex("folder", FileAttributes.Directory), true);
    public static Icon SmallFolderIcon => GetFileTypeIcon(GetFileTypeIconIndex("folder", FileAttributes.Directory), false);
    public static Icon LargeFileIcon => GetFileTypeIcon(GetFileTypeIconIndex("file", 0), true);
    public static Icon SmallFileIcon => GetFileTypeIcon(GetFileTypeIconIndex("file", 0), false);

    public static (Icon Small, Icon Large) GetGoUpIcons()
    {
        if (ExtractIconEx("shell32.dll", 45, out var largeIconHandle, out var smallIconHandle, 1) != uint.MaxValue)
        {
            using (largeIconHandle)
            using (smallIconHandle)
            {
                return (smallIconHandle.ToIcon(), largeIconHandle.ToIcon());
            }
        }

        throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error())!;
    }
}
