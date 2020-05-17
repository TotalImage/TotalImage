namespace TotalImage
{
    using System;
    using System.Collections;
    using System.Windows.Forms;
    using TotalImage.FileSystems;
    using TotalImage.FileSystems.FAT;

    public class ListViewColumnSorter : IComparer
    {
        private CaseInsensitiveComparer ObjectCompare;

        public ListViewColumnSorter()
        {
            SortColumn = 0;
            Order = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer();
        }

        public int Compare(object x, object y)
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

            DirectoryEntry entryX = (DirectoryEntry)listviewX.Tag;
            DirectoryEntry entryY = (DirectoryEntry)listviewY.Tag;
            if (Convert.ToBoolean(entryX.attr & 0x10) && !Convert.ToBoolean(entryY.attr & 0x10))
            {
                return -1;
            }
            else if (!Convert.ToBoolean(entryX.attr & 0x10) && Convert.ToBoolean(entryY.attr & 0x10))
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

                DateTime parsedDateX = DateTime.Parse(listviewX.SubItems[SortColumn].Text);
                DateTime parsedDateY = DateTime.Parse(listviewY.SubItems[SortColumn].Text);

                compareResult = ObjectCompare.Compare(parsedDateX, parsedDateY);
            }
            else if (main.lstFiles.Columns[SortColumn].Text == "Size")
            {  
                if(Convert.ToBoolean(entryX.attr & 0x10) && Convert.ToBoolean(entryY.attr & 0x10))
                {
                    return 0;
                }

                int sizeX = int.Parse(listviewX.SubItems[SortColumn].Text.TrimEnd(' ', 'B').Replace(".", "").Replace(",", ""));
                int sizeY = int.Parse(listviewY.SubItems[SortColumn].Text.TrimEnd(' ', 'B').Replace(".", "").Replace(",", ""));

                compareResult = ObjectCompare.Compare(sizeX, sizeY);
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

                compareResult = ObjectCompare.Compare(listviewX.SubItems[SortColumn].Text, listviewY.SubItems[SortColumn].Text);
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