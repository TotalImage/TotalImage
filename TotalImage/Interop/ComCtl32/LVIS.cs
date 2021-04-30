using System;

internal static partial class Interop
{
    internal static partial class ComCtl32
    {
        [Flags]
        public enum LVIS : uint
        {
            FOCUSED = 0x0001,
            SELECTED = 0x0002,
            CUT = 0x0004,
            DROPHILITED = 0x0008,
            GLOW = 0x0010,
            ACTIVATING = 0x0020,

            OVERLAYMASK = 0x0F00,
            STATEIMAGEMASK = 0xF000
        }
    }
}