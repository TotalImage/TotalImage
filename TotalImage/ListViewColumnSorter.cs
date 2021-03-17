using System;
using System.Collections;
using System.Windows.Forms;
using TotalImage.FileSystems;
using TotalImage.FileSystems.FAT;

namespace TotalImage
{
    public class ListViewColumnSorter : IComparer
    {
        private CaseInsensitiveComparer ObjectCompare;

        public ListViewColumnSorter()
        {
            SortColumn = Settings.CurrentSettings.FilesSortingColumn;
            Order = Settings.CurrentSettings.FilesSortOrder;
            ObjectCompare = new CaseInsensitiveComparer();
        }

        public int Compare(object? x, object? y)
        {
            if (x == null)
                throw new ArgumentNullException(nameof(x), "x cannot be null!");
            if (y == null)
                throw new ArgumentNullException(nameof(y), "y cannot be null!");

            frmMain main = (frmMain)Application.OpenForms["frmMain"];

            if (SortColumn < 0 || SortColumn > main.lstFiles.Columns.Count)
                throw new IndexOutOfRangeException("SortColumn is out of range!");

            int compareResult;
            ListViewItem listviewX, listviewY;

            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            FileSystems.FileSystemObject entryX = (FileSystems.FileSystemObject)listviewX.Tag;
            FileSystems.FileSystemObject entryY = (FileSystems.FileSystemObject)listviewY.Tag;
            if (entryX is FileSystems.Directory && !(entryY is FileSystems.Directory))
            {
                return -1;
            }
            else if (!(entryX is FileSystems.Directory) && entryY is FileSystems.Directory)
            {
                return 1;
            }

            //This if-else block is needed so each column is handled properly
            if (main.lstFiles.Columns[SortColumn].Text == "Modified")
            {
                if(listviewX.Text == "..") //This keeps the virtual .. directory always a the top
                {
                    return -1;
                }
                else if(listviewY.Text == "..")
                {
                    return 1;
                }

                compareResult = ObjectCompare.Compare(entryX.LastWriteTime, entryY.LastWriteTime);
            }
            else if (main.lstFiles.Columns[SortColumn].Text == "Size")
            {
                if(entryX is FileSystems.Directory && entryY is FileSystems.Directory)
                {
                    return 0;
                }

                compareResult = ObjectCompare.Compare(entryX.Length, entryY.Length);
            }
            else if (main.lstFiles.Columns[SortColumn].Text == "Name")
            {
                if (listviewX.Text == "..")
                {
                    return -1;
                }
                else if (listviewY.Text == "..")
                {
                    return 1;
                }

                /* Sure would be nice to implement Explorer-like numeric string sorting here... */

                compareResult = ObjectCompare.Compare(entryX.Name, entryY.Name);
            }
            else
            {
                if (listviewX.Text == "..")
                {
                    return -1;
                }
                else if (listviewY.Text == "..")
                {
                    return 1;
                }

                compareResult = ObjectCompare.Compare(listviewX.SubItems[SortColumn].Text, listviewY.SubItems[SortColumn].Text);
            }

            if (Order == SortOrder.Ascending)
            {
                return compareResult;
            }
            else if (Order == SortOrder.Descending)
            {
                return -compareResult;
            }
            else
            {
                return 0;
            }
        }

        public int SortColumn { set; get; }
        public SortOrder Order { set; get; }
    }
}