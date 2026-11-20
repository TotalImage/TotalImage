using System.Windows.Forms;
using TotalImage.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Dialogs
{
    public partial class BootSectorDialog : Form
    {
        public BootSectorDialog(BootSectorViewModel viewModel)
        {
            InitializeComponent();

            // Two-way bindings on grouped radio buttons write the stale value of the other button
            // back to the view model, so the choice goes back through CheckedChanged instead.
            partitionRadioButton.BindOneWay(nameof(RadioButton.Checked), viewModel, nameof(BootSectorViewModel.IsPartitionSelected));
            diskRadioButton.BindOneWay(nameof(RadioButton.Checked), viewModel, nameof(BootSectorViewModel.IsDiskSelected));
            partitionRadioButton.CheckedChanged += (_, _) => viewModel.IsPartitionSelected = partitionRadioButton.Checked;
            diskRadioButton.CheckedChanged += (_, _) => viewModel.IsDiskSelected = diskRadioButton.Checked;
            diskRadioButton.BindOneTime(nameof(RadioButton.Enabled), viewModel, nameof(BootSectorViewModel.CanShowDisk));
            bootSectorTextBox.BindOneWay(nameof(RichTextBox.Text), viewModel, nameof(BootSectorViewModel.HexText));
            exportButton.Command = viewModel.ExportCommand;
        }
    }
}
