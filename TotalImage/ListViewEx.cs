using System.Windows.Forms;
using static Interop.ComCtl32;
using static Interop.User32;
using static Interop.UxTheme;

namespace TotalImage
{
    internal static class ListViewExtensions
    {
        public static void SelectAllItems(this ListView listView)
        {
            var lvItem = new LVITEM()
            {
                state = LVIS.SELECTED,
                stateMask = LVIS.SELECTED
            };

            SendMessage(listView.Handle, (uint)LVM.SETITEMSTATE, (nint)(-1), ref lvItem);
        }

        public static void SetSortIcon(this ListView listView, int columnIndex, SortOrder order)
        {
            var headerHwnd = SendMessage(listView.Handle, (uint)LVM.GETHEADER, (nint)0, (nint)0);

            for (int columnNumber = 0; columnNumber <= listView.Columns.Count - 1; columnNumber++)
            {
                var hdItem = new HDITEM()
                {
                    mask = HDI.FORMAT
                };

                SendMessage(headerHwnd, (uint)HDM.GETITEMW, (nint)columnNumber, ref hdItem);

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

                SendMessage(headerHwnd, (uint)HDM.SETITEMW, (nint)columnNumber, ref hdItem);
            }
        }
    }

    public class ListViewEx : ListView
    {
        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(Handle, "explorer", null); //Enables Explorer-like appearance
            SendMessage(Handle, (uint)LVM.SETEXTENDEDLISTVIEWSTYLE, (nint)LVS_EX.DOUBLEBUFFER, (nint)LVS_EX.DOUBLEBUFFER); //Enables semi-transparent selection rectangle
        }
    }
}
