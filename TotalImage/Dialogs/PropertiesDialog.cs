using System.Drawing;
using System.Windows.Forms;
using TotalImage.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Dialogs
{
    public partial class PropertiesDialog : Form
    {
        /// <param name="icon">The icon of the entry (or entries).</param>
        /// <param name="stretchIcon">Whether the icon must be scaled to fit.</param>
        public PropertiesDialog(PropertiesViewModel viewModel, Image icon, bool stretchIcon)
        {
            InitializeComponent();

            iconPictureBox.Image = icon;
            iconPictureBox.SizeMode = stretchIcon ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.Normal;

            nameTextBox.BindTwoWay(nameof(TextBox.Text), viewModel, nameof(PropertiesViewModel.Name));
            nameTextBox.BindOneTime(nameof(TextBox.Enabled), viewModel, nameof(PropertiesViewModel.CanEditName));
            // Bindings on a control that starts hidden are not applied, so these fixed values are set directly.
            nameTextBox.Visible = viewModel.IsSingle;
            selectionSummaryLabel.Visible = viewModel.IsMultiple;
            selectionSummaryLabel.Text = viewModel.SelectionSummary;

            typeTextBox.BindOneTime(nameof(TextBox.Text), viewModel, nameof(PropertiesViewModel.TypeName));
            locationTextBox.BindOneTime(nameof(TextBox.Text), viewModel, nameof(PropertiesViewModel.Location));
            sizeTextBox.BindOneWay(nameof(TextBox.Text), viewModel, nameof(PropertiesViewModel.SizeText));
            sizeOnDiskTextBox.BindOneWay(nameof(TextBox.Text), viewModel, nameof(PropertiesViewModel.SizeOnDiskText));
            containsTextBox.BindOneWay(nameof(TextBox.Text), viewModel, nameof(PropertiesViewModel.ContainsText));
            containsTextBox.BindOneTime(nameof(TextBox.Enabled), viewModel, nameof(PropertiesViewModel.IsContainsAvailable));
            shortNameTextBox.BindOneTime(nameof(TextBox.Text), viewModel, nameof(PropertiesViewModel.ShortName));
            shortNameTextBox.BindOneTime(nameof(TextBox.Enabled), viewModel, nameof(PropertiesViewModel.IsFatInfoAvailable));
            firstClusterTextBox.BindOneTime(nameof(TextBox.Text), viewModel, nameof(PropertiesViewModel.FirstCluster));
            firstClusterTextBox.BindOneTime(nameof(TextBox.Enabled), viewModel, nameof(PropertiesViewModel.IsFatInfoAvailable));

            BindHash(crc32TextBox, viewModel, nameof(PropertiesViewModel.Crc32Text));
            BindHash(md5TextBox, viewModel, nameof(PropertiesViewModel.Md5Text));
            BindHash(sha1TextBox, viewModel, nameof(PropertiesViewModel.Sha1Text));

            BindDate(createdCheckBox, createdDateTimePicker, viewModel.Created);
            BindDate(modifiedCheckBox, modifiedDateTimePicker, viewModel.Modified);
            BindDate(accessedCheckBox, accessedDateTimePicker, viewModel.Accessed);

            BindAttribute(readOnlyCheckBox, viewModel, nameof(PropertiesViewModel.ReadOnly));
            BindAttribute(hiddenCheckBox, viewModel, nameof(PropertiesViewModel.Hidden));
            BindAttribute(systemCheckBox, viewModel, nameof(PropertiesViewModel.System));
            BindAttribute(archiveCheckBox, viewModel, nameof(PropertiesViewModel.Archive));

            okButton.Command = viewModel.AcceptCommand;
            Load += (_, _) => viewModel.LoadCommand.Execute(null);
            FormClosing += (_, _) => viewModel.StopBackgroundWork();
        }

        private static void BindHash(TextBox textBox, PropertiesViewModel viewModel, string property)
        {
            textBox.BindOneWay(nameof(TextBox.Text), viewModel, property);
            textBox.BindOneTime(nameof(TextBox.Enabled), viewModel, nameof(PropertiesViewModel.AreHashesAvailable));
            textBox.Click += (_, _) => viewModel.CalculateHashesCommand.Execute(null);
        }

        private static void BindDate(CheckBox checkBox, DateTimePicker picker, EntryDateViewModel date)
        {
            checkBox.BindTwoWay(nameof(CheckBox.Checked), date, nameof(EntryDateViewModel.IsSet));
            checkBox.BindOneTime(nameof(CheckBox.Enabled), date, nameof(EntryDateViewModel.CanToggle));
            picker.BindTwoWay(nameof(DateTimePicker.Value), date, nameof(EntryDateViewModel.Value));
            picker.BindOneWay(nameof(DateTimePicker.Enabled), date, nameof(EntryDateViewModel.CanEditValue));
            // A blank custom format shows an unset date as empty.
            picker.DataBindings.Add(new Binding(nameof(DateTimePicker.CustomFormat), date, nameof(EntryDateViewModel.Format), true, DataSourceUpdateMode.Never) { NullValue = " " });
        }

        private static void BindAttribute(CheckBox checkBox, PropertiesViewModel viewModel, string property)
        {
            checkBox.BindCheckState(viewModel, property);
            checkBox.BindOneTime(nameof(CheckBox.Enabled), viewModel, nameof(PropertiesViewModel.CanEditAttributes));
        }
    }
}
