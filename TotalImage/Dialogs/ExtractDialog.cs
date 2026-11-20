using System.Windows.Forms;
using TotalImage.Controls;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Extraction;

namespace TotalImage.Dialogs
{
    public partial class ExtractDialog : Form
    {
        public ExtractDialog(ExtractViewModel viewModel)
        {
            InitializeComponent();

            promptLabel.BindOneTime(nameof(Label.Text), viewModel, nameof(ExtractViewModel.Prompt));
            targetPathTextBox.BindTwoWay(nameof(TextBox.Text), viewModel, nameof(ExtractViewModel.TargetPath));
            skipDirectoriesRadioButton.BindRadioButton(viewModel, nameof(ExtractViewModel.DirectoryExtractionMode), DirectoryExtractionMode.Skip);
            mergeDirectoriesRadioButton.BindRadioButton(viewModel, nameof(ExtractViewModel.DirectoryExtractionMode), DirectoryExtractionMode.Merge);
            preserveDirectoriesRadioButton.BindRadioButton(viewModel, nameof(ExtractViewModel.DirectoryExtractionMode), DirectoryExtractionMode.Preserve);
            openFolderCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(ExtractViewModel.OpenFolder));
            okButton.BindOneWay(nameof(Button.Enabled), viewModel, nameof(ExtractViewModel.CanExtract));
            browseButton.Command = viewModel.BrowseCommand;

            // The view model may ask to create the target folder first, and the dialog stays open
            // when the user declines.
            okButton.Click += async (_, _) =>
            {
                if (await viewModel.TryAcceptAsync())
                    DialogResult = DialogResult.OK;
            };
        }
    }
}
