using System;
using System.Collections.Generic;

namespace TotalImage.Changes
{
    /// <summary>
    /// An ordered collection of pending mutations against a disk image. Changes are accumulated here
    /// and applied atomically to a temporary copy of the image when <see cref="Container.CommitChanges"/> is called.
    /// </summary>
    public sealed class ChangeSet : IDisposable
    {
        private readonly List<PendingChange> _changes = new();
        private bool _disposed;

        /// <summary>
        /// The ordered list of pending changes accumulated since the last commit or discard.
        /// </summary>
        public IReadOnlyList<PendingChange> Changes => _changes;

        /// <summary>
        /// <see langword="true"/> if there is at least one pending change that has not been committed.
        /// </summary>
        public bool IsDirty => _changes.Count > 0;

        /// <summary>
        /// Raised whenever the set of pending changes is modified (a change is added or the set is cleared).
        /// </summary>
        public event EventHandler? Changed;

        /// <summary>
        /// Appends a pending change to the end of the change list and raises <see cref="Changed"/>.
        /// </summary>
        /// <param name="change">The change to add.</param>
        public void Add(PendingChange change)
        {
            if (change is null) throw new ArgumentNullException(nameof(change));
            _changes.Add(change);
            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Discards all pending changes, disposing any associated resources (such as temporary files
        /// created by <see cref="TempFileDataSource"/>), and raises <see cref="Changed"/>.
        /// </summary>
        public void Clear()
        {
            foreach (var change in _changes)
            {
                if (change is IDisposable disposable)
                    disposable.Dispose();
            }
            _changes.Clear();
            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_disposed)
            {
                Clear();
                _disposed = true;
            }
        }
    }
}
