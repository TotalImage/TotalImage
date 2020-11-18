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

        public override int Compare(FileSystemObject x, FileSystemObject y)
        {
            if(DirectoriesFirst)
            {
                if(x is Directory && !(y is Directory))
                    return -1;
                if(y is Directory && !(x is Directory))
                    return 1;
            }

            switch(Criterion)
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

        public static FileSystemObjectComparer Name { get => new FileSystemObjectComparer(SortCriterion.Name, false); }
        public static FileSystemObjectComparer Length { get => new FileSystemObjectComparer(SortCriterion.Length, false); }
        public static FileSystemObjectComparer LastWriteDate { get => new FileSystemObjectComparer(SortCriterion.LastWriteDate, false); }

        public static FileSystemObjectComparer NameDirectoriesFirst { get => new FileSystemObjectComparer(SortCriterion.Name, true); }
        public static FileSystemObjectComparer LengthDirectoriesFirst { get => new FileSystemObjectComparer(SortCriterion.Length, true); }
        public static FileSystemObjectComparer LastWriteDateDirectoriesFirst { get => new FileSystemObjectComparer(SortCriterion.LastWriteDate, true); }
    }
}