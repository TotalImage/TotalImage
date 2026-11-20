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

        /// <summary>
        /// Binds the selection of a combo box whose items are already filled in (for example by
        /// the designer) to a view-model property: item <c>i</c> stands for <c>values[i]</c>.
        /// Nothing is selected while the property holds a value that isn't in the list.
        /// </summary>
        public static void BindChoices<T>(this ComboBox comboBox, IReadOnlyList<T> values, INotifyPropertyChanged source, string sourceProperty)
        {
            var property = source.GetType().GetProperty(sourceProperty)
                ?? throw new ArgumentException($"{source.GetType().Name} has no property {sourceProperty}.", nameof(sourceProperty));

            void UpdateControl()
            {
                var value = (T)property.GetValue(source)!;
                var index = -1;
                for (var i = 0; i < values.Count; i++)
                {
                    if (EqualityComparer<T>.Default.Equals(values[i], value))
                    {
                        index = i;
                        break;
                    }
                }

                comboBox.SelectedIndex = index;
            }

            UpdateControl();

            comboBox.SelectedIndexChanged += (_, _) =>
            {
                if (comboBox.SelectedIndex >= 0 && comboBox.SelectedIndex < values.Count)
                    property.SetValue(source, values[comboBox.SelectedIndex]);
            };
            source.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == sourceProperty)
                    UpdateControl();
            };
        }

        /// <summary>
        /// Binds a radio button to one value of a view-model property: the button is checked
        /// while the property holds that value, and checking it sets the property to it.
        /// </summary>
        public static void BindRadioButton<T>(this RadioButton radioButton, INotifyPropertyChanged source, string sourceProperty, T value)
        {
            var property = source.GetType().GetProperty(sourceProperty)
                ?? throw new ArgumentException($"{source.GetType().Name} has no property {sourceProperty}.", nameof(sourceProperty));

            void UpdateControl() => radioButton.Checked = EqualityComparer<T>.Default.Equals((T)property.GetValue(source)!, value);
            UpdateControl();

            radioButton.CheckedChanged += (_, _) =>
            {
                if (radioButton.Checked)
                    property.SetValue(source, value);
            };
            source.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == sourceProperty)
                    UpdateControl();
            };
        }
    }
}
