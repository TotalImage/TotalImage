using System.Collections;

namespace TotalImage
{
    /// <summary>
    /// A comparer that inverts the output of another comparer in order to
    /// achieve descending sort order
    /// </summary>
    class InvertedComparer : IComparer
    {
        private IComparer Comparer { get; }

        /// <summary>
        /// Creates an inverted comparer.
        /// </summary>
        /// <param name="comparer">The comparer whose output should be inverted</param>
        public InvertedComparer(IComparer comparer)
        {
            Comparer = comparer;
        }

        /// <inheritdoc/>
        public int Compare(object? x, object? y)
        {
            return -Comparer.Compare(x, y);
        }
    }
}