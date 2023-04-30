using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Controls;

using static Windows.Win32.PInvoke;

namespace TotalImage
{
    internal static class ListViewExtensions
    {
        public static void SelectAllItems(this ListView listView)
        {
            var lvItem = new LVITEMW()
            {
                state = LIST_VIEW_ITEM_STATE_FLAGS.LVIS_SELECTED,
                stateMask = LIST_VIEW_ITEM_STATE_FLAGS.LVIS_SELECTED
            };

            var plvItem = Marshal.AllocHGlobal(Marshal.SizeOf(lvItem));
            Marshal.StructureToPtr(lvItem, plvItem, false);

            try
            {
                SendMessage((HWND)listView.Handle, LVM_SETITEMSTATE, unchecked((nuint)(-1)), plvItem);
            }
            finally
            {
                Marshal.FreeHGlobal(plvItem);
            }
        }

        public static void SetSortIcon(this ListView listView, int columnIndex, SortOrder order)
        {
            var headerHwnd = (HWND)SendMessage((HWND)listView.Handle, LVM_GETHEADER, 0, 0).Value;

            var phdItem = Marshal.AllocHGlobal(Marshal.SizeOf<HDITEMW>());

            try
            {
                for (int columnNumber = 0; columnNumber <= listView.Columns.Count - 1; columnNumber++)
                {
                    var hdItem = new HDITEMW()
                    {
                        mask = HDI_MASK.HDI_FORMAT
                    };

                    Marshal.StructureToPtr(hdItem, phdItem, false);

                    SendMessage(headerHwnd, HDM_GETITEMW, (nuint)columnNumber, phdItem);

                    hdItem = Marshal.PtrToStructure<HDITEMW>(phdItem);

                    if (!(order == SortOrder.None) && columnNumber == columnIndex)
                    {
                        switch (order)
                        {
                            case SortOrder.Ascending:
                                hdItem.fmt &= ~HEADER_CONTROL_FORMAT_FLAGS.HDF_SORTDOWN;
                                hdItem.fmt |= HEADER_CONTROL_FORMAT_FLAGS.HDF_SORTUP;
                                break;
                            case SortOrder.Descending:
                                hdItem.fmt &= ~HEADER_CONTROL_FORMAT_FLAGS.HDF_SORTUP;
                                hdItem.fmt |= HEADER_CONTROL_FORMAT_FLAGS.HDF_SORTDOWN;
                                break;
                        }
                        hdItem.fmt |= HEADER_CONTROL_FORMAT_FLAGS.HDF_LEFT | HEADER_CONTROL_FORMAT_FLAGS.HDF_BITMAP_ON_RIGHT;
                    }
                    else
                    {
                        hdItem.fmt &= ~HEADER_CONTROL_FORMAT_FLAGS.HDF_SORTDOWN & ~HEADER_CONTROL_FORMAT_FLAGS.HDF_SORTUP & ~HEADER_CONTROL_FORMAT_FLAGS.HDF_BITMAP_ON_RIGHT;
                    }

                    Marshal.StructureToPtr(hdItem, phdItem, true);

                    SendMessage(headerHwnd, HDM_SETITEMW, (nuint)columnNumber, phdItem);

                    Marshal.DestroyStructure<HDITEMW>(phdItem);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(phdItem);
            }
        }
    }

    public class ListViewEx : ListView
    {
        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme((HWND)Handle, "explorer", null); //Enables Explorer-like appearance
            SendMessage((HWND)Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, (nuint)LVS_EX_DOUBLEBUFFER, (nint)LVS_EX_DOUBLEBUFFER); //Enables semi-transparent selection rectangle
        }
    }
}
