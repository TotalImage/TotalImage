using System.Collections;

namespace TotalImage
{
    class DescendingComparer : IComparer
    {
        public IComparer Comparer { get; }

        public DescendingComparer(IComparer comparer)
        {
            Comparer = comparer;
        }

        public int Compare(object x, object y)
        {
            return -Comparer.Compare(x, y);
        }
    }
}