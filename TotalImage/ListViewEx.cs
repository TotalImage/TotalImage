using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Interop.ComCtl32;
using static Interop.User32;
using static Interop.UxTheme;

namespace TotalImage
{
    internal static class ListViewExtensions
    {

        public static void SetSortIcon(this ListView listView, int columnIndex, SortOrder order)
        {
            IntPtr columnHeader = SendMessage(listView.Handle, (uint)LVM.GETHEADER, IntPtr.Zero, IntPtr.Zero);

            for (int columnNumber = 0; columnNumber <= listView.Columns.Count - 1; columnNumber++)
            {
                IntPtr columnPtr = new IntPtr(columnNumber);
                HDITEM hdItem = new HDITEM();
                hdItem.mask = HDI.FORMAT;

                var phdItem = Marshal.AllocHGlobal(Marshal.SizeOf(hdItem));
                Marshal.StructureToPtr(hdItem, phdItem, false);

                SendMessage(columnHeader, (uint)HDM.GETITEMW, columnPtr, phdItem);

                hdItem = (HDITEM)Marshal.PtrToStructure(phdItem, typeof(HDITEM));

                if (!(order == SortOrder.None) && columnNumber == columnIndex)
                {
                    switch (order)
                    {
                        case SortOrder.Ascending:
                            hdItem.fmt &= ~HDF.SORTDOWN;
                            hdItem.fmt |= HDF.SORTUP;
                            break;
                        case SortOrder.Descending:
                            hdItem.fmt &= ~HDF.SORTUP;
                            hdItem.fmt |= HDF.SORTDOWN;
                            break;
                    }
                    hdItem.fmt |= (HDF.LEFT | HDF.BITMAP_ON_RIGHT);
                }
                else
                {
                    hdItem.fmt &= ~HDF.SORTDOWN & ~HDF.SORTUP & ~HDF.BITMAP_ON_RIGHT;
                }

                Marshal.StructureToPtr(hdItem, phdItem, true);

                SendMessage(columnHeader, (uint)HDM.SETITEMW, columnPtr, phdItem);

                Marshal.FreeHGlobal(phdItem);
            }
        }
    }

    public class ListViewEx : ListView
    {
        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(Handle, "explorer", null); //Enables Explorer-like appearance
            SendMessage(Handle, (uint)LVM.SETEXTENDEDLISTVIEWSTYLE, (IntPtr)LVS_EX.DOUBLEBUFFER, (IntPtr)LVS_EX.DOUBLEBUFFER); //Enables semi-transparent selection rectangle
        }
    }
}
