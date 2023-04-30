using System;
using System.Drawing;
using Microsoft.Win32.SafeHandles;
using static Interop.User32;

internal static partial class Interop
{
    public class SafeIconHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        SafeIconHandle() : base(ownsHandle: true) { }

        public SafeIconHandle(nint preexistingHandle, bool ownsHandle) : base(ownsHandle)
        {
            SetHandle(preexistingHandle);
        }

        protected override bool ReleaseHandle() =>
            DestroyIcon(handle);

        public Icon ToIcon() =>
            (Icon)Icon.FromHandle(handle).Clone();
    }
}
