using System.ComponentModel;
using System.Windows.Forms;
using TotalImage.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Dialogs
{
    public partial class NewImageDialog : Form
    {
        private readonly NewImageViewModel viewModel;

        public NewImageDialog(NewImageViewModel viewModel)
        {
            this.viewModel = viewModel;
            InitializeComponent();

            floppyDiskRadioButton.BindTwoWay(nameof(RadioButton.Checked), viewModel, nameof(NewImageViewModel.IsFloppyDisk));
            hardDiskRadioButton.BindTwoWay(nameof(RadioButton.Checked), viewModel, nameof(NewImageViewModel.IsHardDisk));
            containerFormatComboBox.BindItems(viewModel.ContainerFormats, viewModel, nameof(NewImageViewModel.ContainerFormat));
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            floppyPanel.BindOneWay(nameof(Panel.Visible), viewModel, nameof(NewImageViewModel.IsFloppyDisk));
            hardDiskPanel.BindOneWay(nameof(Panel.Visible), viewModel, nameof(NewImageViewModel.IsHardDisk));

            geometryComboBox.BindItems(NewImageViewModel.FloppyGeometries, viewModel, nameof(NewImageViewModel.SelectedGeometry));
            advancedButton.Command = viewModel.ShowAdvancedOptionsCommand;
            volumeLabelTextBox.MaxLength = NewImageViewModel.MaxVolumeLabelLength;
            volumeLabelTextBox.BindTwoWay(nameof(TextBox.Text), viewModel, nameof(NewImageViewModel.VolumeLabel));
            writeBpbCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(NewImageViewModel.WriteBpb));
            writeBpbCheckBox.BindOneWay(nameof(CheckBox.Enabled), viewModel, nameof(NewImageViewModel.CanChangeWriteBpb));
            bpbVersionComboBox.BindItems(NewImageViewModel.BpbVersions, viewModel, nameof(NewImageViewModel.BpbVersion));
            bpbVersionComboBox.BindOneWay(nameof(ComboBox.Enabled), viewModel, nameof(NewImageViewModel.CanChooseBpbVersion));

            diskSizeNumericUpDown.BindTwoWay(nameof(NumericUpDown.Value), viewModel, nameof(NewImageViewModel.DiskSize));
            writePartitionTableCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(NewImageViewModel.WritePartitionTable));
            partitionTableComboBox.BindItems(NewImageViewModel.PartitionTables, viewModel, nameof(NewImageViewModel.PartitionTable));
            partitionTableComboBox.BindOneWay(nameof(ComboBox.Enabled), viewModel, nameof(NewImageViewModel.CanChoosePartitionTable));

            okButton.Command = viewModel.CreateCommand;
        }

        // The container formats on offer depend on the media type.
        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewImageViewModel.ContainerFormats))
            {
                containerFormatComboBox.DataSource = viewModel.ContainerFormats;
                containerFormatComboBox.SelectedItem = viewModel.ContainerFormat;
            }
        }

        // OK closes the dialog only once the view model has answered: it does not answer when
        // creating an image without a BPB was refused.
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK && viewModel.Answer is null)
                e.Cancel = true;

            base.OnFormClosing(e);
        }
    }
}
