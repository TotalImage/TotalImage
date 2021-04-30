using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class User32
    {
        [DllImport("user32.dll", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public static IntPtr SendMessage<T>(IntPtr hWnd, uint Msg, IntPtr wParam, ref T? lParam)
        {
            if (lParam != null)
            {
                var pTemp = Marshal.AllocHGlobal(Marshal.SizeOf(lParam));
                Marshal.StructureToPtr(lParam, pTemp, false);

                var ret = SendMessage(hWnd, Msg, wParam, pTemp);

                lParam = (T?)Marshal.PtrToStructure(pTemp, typeof(T));
                Marshal.FreeHGlobal(pTemp);

                return ret;
            }

            return SendMessage(hWnd, Msg, wParam, IntPtr.Zero);
        }
    }
}