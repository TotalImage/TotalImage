using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace TotalImage.Controls
{
    /// <summary>
    /// Binding helpers for list views showing view-model rows.
    /// </summary>
    internal static class ListViewBindingExtensions
    {
        /// <summary>
        /// Syncs the selected item of a single-selection list view with an <see cref="int"/>
        /// view-model property in both directions; -1 means nothing is selected.
        /// </summary>
        public static void BindSelectedIndex(this ListView listView, INotifyPropertyChanged source, string sourceProperty)
        {
            var property = source.GetType().GetProperty(sourceProperty)
                ?? throw new ArgumentException($"{source.GetType().Name} has no property {sourceProperty}.", nameof(sourceProperty));

            void UpdateControl()
            {
                var index = (int)property.GetValue(source)!;
                var current = listView.SelectedIndices.Count > 0 ? listView.SelectedIndices[0] : -1;
                if (index == current)
                    return;

                listView.SelectedIndices.Clear();
                if (index >= 0 && index < listView.Items.Count)
                    listView.SelectedIndices.Add(index);
            }

            UpdateControl();

            listView.ItemSelectionChanged += (_, _) =>
            {
                var index = listView.SelectedIndices.Count > 0 ? listView.SelectedIndices[0] : -1;
                if ((int)property.GetValue(source)! != index)
                    property.SetValue(source, index);
            };
            source.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == sourceProperty)
                    UpdateControl();
            };
        }
    }
}
