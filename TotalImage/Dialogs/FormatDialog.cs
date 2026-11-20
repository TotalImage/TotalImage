using System.Windows.Forms;
using TotalImage.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Dialogs
{
    public partial class FormatDialog : Form
    {
        public FormatDialog(FormatViewModel viewModel)
        {
            InitializeComponent();

            fileSystemComboBox.BindItems(viewModel.FileSystems, viewModel, nameof(FormatViewModel.FileSystem));
            clusterSizeComboBox.BindItems(viewModel.ClusterSizes, viewModel, nameof(FormatViewModel.ClusterSize));
            volumeLabelTextBox.MaxLength = FormatViewModel.MaxVolumeLabelLength;
            volumeLabelTextBox.BindTwoWay(nameof(TextBox.Text), viewModel, nameof(FormatViewModel.VolumeLabel));
            quickFormatCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(FormatViewModel.QuickFormat));
            okButton.Command = viewModel.AcceptCommand;
        }
    }
}
