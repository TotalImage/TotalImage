using System;
using System.Windows.Forms;

using static Interop.User32;
using static Interop.ComCtl32;

namespace TotalImage
{
    internal enum ProgressBarState
    {
        Normal = 1,
        Error = 2,
        Paused = 3
    }

    internal static class ProgressBarExtensions
    {
        public static void SetState(this ProgressBar bar, ProgressBarState state)
        {
            SendMessage(bar.Handle, (uint)PBM.SETSTATE, (nint)state, (nint)0);
        }

        public static ProgressBarState GetState(this ProgressBar bar)
        {
            return (ProgressBarState)SendMessage(bar.Handle, (uint)PBM.GETSTATE, (nint)0, (nint)0);
        }
    }
}