using System;

internal static partial class Interop
{
    internal static partial class ComCtl32
    {
        [Flags]
        public enum HDF : uint
        {
            LEFT = 0x0000,
            RIGHT = 0x0001,
            CENTER = 0x0002,
            JUSTIFYMASK = 0x0003,
            RTLREADING = 0x0004,
            BITMAP = 0x2000,
            STRING = 0x4000,
            OWNERDRAW = 0x8000,
            IMAGE = 0x0800,
            BITMAP_ON_RIGHT = 0x1000,
            SORTUP = 0x0400,
            SORTDOWN = 0x0200,
            CHECKBOX = 0x0040,
            CHECKED = 0x0080,
            FIXEDWIDTH = 0x0100,
            SPLITBUTTON = 0x1000000
        }
    }
}