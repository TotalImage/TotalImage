using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TotalImage.FileSystems;

namespace TotalImage
{
    class FileListViewItemComparer : Comparer<ListViewItem>
    {
        enum FileListViewColumn
        {
            Name = 0,
            Type,
            Size,
            Modified,
            Attributes
        }

        FileListViewColumn SortColumn { get; }

        private FileListViewItemComparer(FileListViewColumn sortColumn)
        {
            SortColumn = sortColumn;
        }

        public override int Compare(ListViewItem? x, ListViewItem? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;

            if (x.Text == "..") return -1;
            if (y.Text == "..") return 1;

            switch (SortColumn)
            {
                case FileListViewColumn.Name:
                    return FileSystemObjectComparer.NameDirectoriesFirst.Compare(x.Tag as FileSystemObject, y.Tag as FileSystemObject);
                case FileListViewColumn.Type:
                case FileListViewColumn.Attributes:
                    if (x.Tag is Directory && !(y.Tag is Directory))
                        return -1;
                    if (y.Tag is Directory && !(x.Tag is Directory))
                        return 1;
                    return string.Compare(x.SubItems[(int)SortColumn].Text, y.SubItems[(int)SortColumn].Text);
                case FileListViewColumn.Size:
                    return FileSystemObjectComparer.LengthDirectoriesFirst.Compare(x.Tag as FileSystemObject, y.Tag as FileSystemObject);
                case FileListViewColumn.Modified:
                    return FileSystemObjectComparer.LastWriteDateDirectoriesFirst.Compare(x.Tag as FileSystemObject, y.Tag as FileSystemObject);
                default:
                    return 0; // idk
            }
        }

        public static IComparer GetColumnSorter(int column)
        {
            switch((FileListViewColumn) column)
            {
                case FileListViewColumn.Name:
                    return FileListViewItemComparer.Name;
                case FileListViewColumn.Type:
                    return FileListViewItemComparer.Type;
                case FileListViewColumn.Size:
                    return FileListViewItemComparer.Size;
                case FileListViewColumn.Modified:
                    return FileListViewItemComparer.Modified;
                case FileListViewColumn.Attributes:
                    return FileListViewItemComparer.Attributes;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static FileListViewItemComparer Name => new FileListViewItemComparer(FileListViewColumn.Name);
        public static FileListViewItemComparer Type => new FileListViewItemComparer(FileListViewColumn.Type);
        public static FileListViewItemComparer Size => new FileListViewItemComparer(FileListViewColumn.Size);
        public static FileListViewItemComparer Modified => new FileListViewItemComparer(FileListViewColumn.Modified);
        public static FileListViewItemComparer Attributes => new FileListViewItemComparer(FileListViewColumn.Attributes);
    }
}