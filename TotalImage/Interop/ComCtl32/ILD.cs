using System;

internal static partial class Interop
{
    internal static partial class ComCtl32
    {
        [Flags]
        public enum ILD
        {
            NORMAL = 0x00000000,
            TRANSPARENT = 0x00000001,
            BLEND25 = 0x00000002,
            FOCUS = 0x00000002,
            BLEND50 = 0x00000004,
            SELECTED = 0x00000004,
            BLEND = 0x00000004,
            MASK = 0x00000010,
            IMAGE = 0x00000020,
            ROP = 0x00000040,
            OVERLAYMASK = 0x00000F00,
            PRESERVEALPHA = 0x00001000,
            SCALE = 0x00002000,
            DPISCALE = 0x00004000,
            ASYNC = 0x00008000
        }
    }
}