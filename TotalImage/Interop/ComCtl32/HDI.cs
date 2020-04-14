using System;

internal static partial class Interop
{
    internal static partial class ComCtl32
    {
        [Flags]
        public enum HDI : uint
        {
            WIDTH = 0x0001,
            HEIGHT = WIDTH,
            TEXT = 0x0002,
            FORMAT = 0x0004,
            LPARAM = 0x0008,
            BITMAP = 0x0010,
            IMAGE = 0x0020,
            DI_SETITEM = 0x0040,
            ORDER = 0x0080,
            FILTER = 0x0100,
            STATE = 0x0200
        }
    }
}