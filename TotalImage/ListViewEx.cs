using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TotalImage
{
    internal static class ListViewExtensions
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct LVCOLUMN
        {
            public int mask;
            public int cx;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszText;
            public IntPtr hbm;
            public int cchTextMax;
            public int fmt;
            public int iSubItem;
            public int iImage;
            public int iOrder;
        }

        const int HDI_FORMAT = 0x0004;
        const int HDF_LEFT = 0x0000;
        const int HDF_BITMAP_ON_RIGHT = 0x1000;
        const int HDF_SORTUP = 0x0400;
        const int HDF_SORTDOWN = 0x0200;

        const int LVM_FIRST = 0x1000;
        const int LVM_GETHEADER = LVM_FIRST + 31;
        const int HDM_FIRST = 0x1200;
        const int HDM_GETITEM = HDM_FIRST + 11;
        const int HDM_SETITEM = HDM_FIRST + 12;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessageLVCOLUMN(IntPtr hWnd, Int32 Msg, IntPtr wParam, ref LVCOLUMN lPLVCOLUMN);

        public static void SetSortIcon(this ListView listView, int columnIndex, SortOrder order)
        {
            IntPtr columnHeader = SendMessage(listView.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);

            for (int columnNumber = 0; columnNumber <= listView.Columns.Count - 1; columnNumber++)
            {
                IntPtr columnPtr = new IntPtr(columnNumber);
                LVCOLUMN lvColumn = new LVCOLUMN();
                lvColumn.mask = HDI_FORMAT;

                SendMessageLVCOLUMN(columnHeader, HDM_GETITEM, columnPtr, ref lvColumn);

                if (!(order == SortOrder.None) && columnNumber == columnIndex)
                {
                    switch (order)
                    {
                        case SortOrder.Ascending:
                            lvColumn.fmt &= ~HDF_SORTDOWN;
                            lvColumn.fmt |= HDF_SORTUP;
                            break;
                        case SortOrder.Descending:
                            lvColumn.fmt &= ~HDF_SORTUP;
                            lvColumn.fmt |= HDF_SORTDOWN;
                            break;
                    }
                    lvColumn.fmt |= (HDF_LEFT | HDF_BITMAP_ON_RIGHT);
                }
                else
                {
                    lvColumn.fmt &= ~HDF_SORTDOWN & ~HDF_SORTUP & ~HDF_BITMAP_ON_RIGHT;
                }

                SendMessageLVCOLUMN(columnHeader, HDM_SETITEM, columnPtr, ref lvColumn);
            }
        }
    }

    public class ListViewEx : ListView
    {

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(Handle, "explorer", null); //Enables Explorer-like appearance
            SendMessage(Handle, 0x1000 + 54, 0x00010000, 0x00010000); //Enables semi-transparent selection rectangle
        }
    }
}
