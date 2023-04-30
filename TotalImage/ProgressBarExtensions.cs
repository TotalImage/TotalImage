using System;
using System.Windows.Forms;
using Windows.Win32.Foundation;

using static Windows.Win32.PInvoke;

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
            SendMessage((HWND)bar.Handle, PBM_SETSTATE, (nuint)state, 0);
        }

        public static ProgressBarState GetState(this ProgressBar bar)
        {
            return (ProgressBarState)(nint)SendMessage((HWND)bar.Handle, PBM_GETSTATE, 0, 0);
        }
    }
}
