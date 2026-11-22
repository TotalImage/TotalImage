using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TotalImage.Controls;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Main;

namespace TotalImage.Adapters
{
    /// <summary>
    /// Shows <see cref="MainViewModel.Entries"/> in a virtual list view, reading the entries by
    /// index, and keeps the list view's selection and <see cref="MainViewModel.SelectedEntries"/>
    /// in sync. Sorting is the view model's; the list view never sorts.
    /// </summary>
    internal sealed class FileListAdapter : IDisposable
    {
        private static readonly Color AddedBackColor = Color.FromArgb(198, 239, 206);
        private static readonly Color AddedForeColor = Color.FromArgb(0, 97, 0);
        private static readonly Color DeletedBackColor = Color.FromArgb(255, 199, 206);
        private static readonly Color DeletedForeColor = Color.FromArgb(156, 0, 6);
        private static readonly Color RenamedBackColor = Color.FromArgb(255, 235, 156);
        private static readonly Color RenamedForeColor = Color.FromArgb(156, 87, 0);

        private readonly ListView list;
        private readonly MainViewModel viewModel;
        private readonly FileIcons icons;

        // Cached to avoid allocating a GDI font per item.
        private readonly Font monoFont = new(FontFamily.GenericMonospace, 9);

        private IReadOnlyList<EntryViewModel> entries = [];
        private Dictionary<EntryViewModel, int> indices = [];
        private ListViewItem?[] items = [];

        // Set while the list view's selection is changed from the view model.
        private bool updatingSelection;

        public FileListAdapter(ListView list, MainViewModel viewModel, FileIcons icons)
        {
            this.list = list;
            this.viewModel = viewModel;
            this.icons = icons;

            list.RetrieveVirtualItem += (_, e) => e.Item = GetItem(e.ItemIndex);
            list.SelectedIndexChanged += (_, _) => OnListSelectionChanged();
            list.ColumnClick += (_, e) => viewModel.SortByCommand.Execute(e.Column);
            icons.Changed += (_, _) => ClearItems();

            viewModel.PropertyChanged += (_, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(MainViewModel.Entries):
                        ShowEntries();
                        break;
                    case nameof(MainViewModel.SelectedEntries):
                        if (!updatingSelection)
                            ApplySelection();
                        break;
                }
            };

            ShowEntries();
        }

        /// <summary>
        /// The entry shown at an index of the list view.
        /// </summary>
        public EntryViewModel EntryAt(int index) => entries[index];

        /// <summary>
        /// The index of an entry in the list view, or -1.
        /// </summary>
        public int IndexOf(EntryViewModel entry) => indices.TryGetValue(entry, out var index) ? index : -1;

        private void ShowEntries()
        {
            entries = viewModel.Entries;
            indices = new Dictionary<EntryViewModel, int>(entries.Count);
            for (var i = 0; i < entries.Count; i++)
                indices[entries[i]] = i;

            items = new ListViewItem?[entries.Count];

            list.BeginUpdate();
            updatingSelection = true;
            try
            {
                list.SelectedIndices.Clear();
                list.VirtualListSize = entries.Count;
                ApplySelection();
            }
            finally
            {
                updatingSelection = false;
                list.EndUpdate();
            }

            list.Invalidate();
            list.SetSortIcon(viewModel.SortColumn, viewModel.SortDirection.ToWinForms());
        }

        private void ClearItems()
        {
            Array.Clear(items);
            list.Invalidate();
        }

        private void OnListSelectionChanged()
        {
            if (updatingSelection)
                return;

            var selected = list.SelectedIndices.Cast<int>().Where(i => i < entries.Count).Select(i => entries[i]).ToList();
            updatingSelection = true;
            try
            {
                viewModel.SelectEntries(selected);
            }
            finally
            {
                updatingSelection = false;
            }

            // The view model leaves out ".." when other entries are selected too.
            if (viewModel.SelectedEntries.Count != list.SelectedIndices.Count)
                ApplySelection();
        }

        // Selects the view model's selected entries in the list view.
        private void ApplySelection()
        {
            var wanted = viewModel.SelectedEntries.Select(IndexOf).Where(i => i >= 0).ToHashSet();
            var current = list.SelectedIndices.Cast<int>().ToHashSet();
            if (wanted.SetEquals(current))
                return;

            var wasUpdating = updatingSelection;
            updatingSelection = true;
            try
            {
                var parentIndex = entries.Count > 0 && entries[0].IsParentEntry ? 0 : -1;
                if (wanted.Count > 1 && wanted.Count == entries.Count - (parentIndex + 1))
                {
                    // Everything but "..": quicker than selecting the items one by one.
                    list.SelectAllItems();
                    if (parentIndex == 0)
                        list.SelectedIndices.Remove(0);
                }
                else
                {
                    list.SelectedIndices.Clear();
                    foreach (var index in wanted.Order())
                        list.SelectedIndices.Add(index);
                }
            }
            finally
            {
                updatingSelection = wasUpdating;
            }
        }

        public void Dispose() => monoFont.Dispose();

        private ListViewItem GetItem(int index) => items[index] ??= CreateItem(entries[index]);

        private ListViewItem CreateItem(EntryViewModel entry)
        {
            var item = new ListViewItem(entry.Name)
            {
                ImageIndex = icons.GetIndex(entry.IconKey, entry.IsHidden),
                ToolTipText = entry.ToolTip,
                Tag = entry
            };

            if (entry.IsParentEntry)
            {
                for (var i = 1; i < list.Columns.Count; i++)
                    item.SubItems.Add(string.Empty);
                return item;
            }

            item.SubItems.Add(entry.TypeName);
            item.SubItems.Add(entry.SizeText);
            item.SubItems.Add(entry.ModifiedText);
            item.SubItems.Add(entry.AttributesText);
            item.UseItemStyleForSubItems = false;
            item.SubItems[4].Font = monoFont;

            //Do some simple styling for hidden items
            if (entry.IsHidden)
            {
                foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                    subItem.ForeColor = Color.Gray;
            }

            if (GetPendingColors(entry.PendingState) is (var back, var fore))
            {
                item.BackColor = back;
                item.ForeColor = fore;
            }

            return item;
        }

        /// <summary>
        /// The colors that highlight a pending change: green = added, red = pending delete, orange = renamed.
        /// </summary>
        public static (Color Back, Color Fore)? GetPendingColors(PendingState state) => state switch
        {
            PendingState.Added => (AddedBackColor, AddedForeColor),
            PendingState.Deleted => (DeletedBackColor, DeletedForeColor),
            PendingState.Renamed => (RenamedBackColor, RenamedForeColor),
            _ => null
        };
    }
}
