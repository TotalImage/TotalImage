using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace TotalImage.Controls
{
    /// <summary>
    /// Shorthands for binding control properties to view-model properties.
    /// </summary>
    internal static class BindingExtensions
    {
        /// <summary>
        /// Binds a control property to a view-model property in both directions, writing back
        /// to the view model as soon as the control's value changes.
        /// </summary>
        public static void BindTwoWay(this Control control, string controlProperty, INotifyPropertyChanged source, string sourceProperty)
            => control.DataBindings.Add(controlProperty, source, sourceProperty, false, DataSourceUpdateMode.OnPropertyChanged);

        /// <summary>
        /// Binds a control property to a view-model property, updating the control only.
        /// </summary>
        public static void BindOneWay(this Control control, string controlProperty, INotifyPropertyChanged source, string sourceProperty)
            => control.DataBindings.Add(controlProperty, source, sourceProperty, false, DataSourceUpdateMode.Never);

        /// <summary>
        /// Fills a combo box with items and binds its selected item to a view-model property.
        /// A <c>SelectedItem</c> data binding fails validation while nothing is selected (and blocks
        /// the dialog's buttons), so the selection is synced through events instead.
        /// </summary>
        public static void BindItems<T>(this ComboBox comboBox, IReadOnlyList<T> items, INotifyPropertyChanged source, string sourceProperty)
        {
            var property = source.GetType().GetProperty(sourceProperty)
                ?? throw new ArgumentException($"{source.GetType().Name} has no property {sourceProperty}.", nameof(sourceProperty));

            comboBox.DataSource = items;
            void UpdateControl() => comboBox.SelectedItem = property.GetValue(source);
            UpdateControl();

            comboBox.SelectedIndexChanged += (_, _) => property.SetValue(source, comboBox.SelectedItem);
            source.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == sourceProperty)
                    UpdateControl();
            };
        }

        /// <summary>
        /// Binds a control property to a property of an object that never changes.
        /// </summary>
        public static void BindOneTime(this Control control, string controlProperty, object source, string sourceProperty)
            => control.DataBindings.Add(controlProperty, source, sourceProperty, false, DataSourceUpdateMode.Never);
    }
}
