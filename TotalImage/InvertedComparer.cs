using System.Collections.Generic;

namespace TotalImage
{
    /// <summary>
    /// A comparer that inverts the output of another comparer in order to
    /// achieve descending sort order
    /// </summary>
    class InvertedComparer<T> : IComparer<T>
    {
        private IComparer<T> Comparer { get; }

        /// <summary>
        /// Creates an inverted comparer.
        /// </summary>
        /// <param name="comparer">The comparer whose output should be inverted</param>
        public InvertedComparer(IComparer<T> comparer)
        {
            Comparer = comparer;
        }

        /// <inheritdoc/>
        public int Compare(T x, T y)
        {
            return -Comparer.Compare(x, y);
        }
    }
}