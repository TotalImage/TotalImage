using System.Windows.Forms;
using TotalImage.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Dialogs
{
    public partial class ChangeVolumeLabelDialog : Form
    {
        public ChangeVolumeLabelDialog(VolumeLabelViewModel viewModel)
        {
            InitializeComponent();

            rootDirectoryLabelTextBox.MaxLength = bpbLabelTextBox.MaxLength = VolumeLabelViewModel.MaxLength;
            rootDirectoryLabelTextBox.BindTwoWay(nameof(TextBox.Text), viewModel, nameof(VolumeLabelViewModel.RootDirectoryLabel));
            writeBpbLabelCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(VolumeLabelViewModel.WriteBpbLabel));
            syncLabelsCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(VolumeLabelViewModel.SyncLabels));
            syncLabelsCheckBox.BindOneWay(nameof(CheckBox.Enabled), viewModel, nameof(VolumeLabelViewModel.CanEditBpbLabel));
            bpbLabelTextBox.BindTwoWay(nameof(TextBox.Text), viewModel, nameof(VolumeLabelViewModel.BpbLabel));
            bpbLabelTextBox.BindOneWay(nameof(TextBox.Enabled), viewModel, nameof(VolumeLabelViewModel.CanEditBpbLabel));
            bpbLabelTextBox.BindOneWay(nameof(TextBox.ReadOnly), viewModel, nameof(VolumeLabelViewModel.IsBpbLabelReadOnly));
            okButton.Command = viewModel.AcceptCommand;
        }
    }
}
