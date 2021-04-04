using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TotalImage.FileSystems;

namespace TotalImage
{
    class FileSystemObjectComparer : Comparer<FileSystemObject>
    {
        enum SortCriterion
        {
            Name,
            Length,
            LastWriteDate
        }

        SortCriterion Criterion { get; }
        bool DirectoriesFirst { get; }

        private FileSystemObjectComparer(SortCriterion criterion, bool prioritizeDirectories)
        {
            Criterion = criterion;
            DirectoriesFirst = prioritizeDirectories;
        }

        public override int Compare(FileSystemObject? x, FileSystemObject? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;

            if(DirectoriesFirst)
            {
                if (x is Directory && !(y is Directory))
                    return -1;
                if (y is Directory && !(x is Directory))
                    return 1;
            }

            switch (Criterion)
            {
                case SortCriterion.Name:
                    return string.Compare(x.Name, y.Name, true);
                case SortCriterion.Length:
                    return (int)(y.Length - x.Length);
                case SortCriterion.LastWriteDate:
                    return DateTime.Compare(
                        x.LastWriteTime.GetValueOrDefault(),
                        y.LastWriteTime.GetValueOrDefault()
                    );
                default:
                    return 0; // idk
            }
        }

        public static FileSystemObjectComparer Name => new FileSystemObjectComparer(SortCriterion.Name, false);
        public static FileSystemObjectComparer Length => new FileSystemObjectComparer(SortCriterion.Length, false);
        public static FileSystemObjectComparer LastWriteDate => new FileSystemObjectComparer(SortCriterion.LastWriteDate, false);

        public static FileSystemObjectComparer NameDirectoriesFirst => new FileSystemObjectComparer(SortCriterion.Name, true);
        public static FileSystemObjectComparer LengthDirectoriesFirst => new FileSystemObjectComparer(SortCriterion.Length, true);
        public static FileSystemObjectComparer LastWriteDateDirectoriesFirst => new FileSystemObjectComparer(SortCriterion.LastWriteDate, true);
    }
}