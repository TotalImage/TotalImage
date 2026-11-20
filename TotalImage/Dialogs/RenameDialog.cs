using System.Windows.Forms;
using TotalImage.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Dialogs
{
    public partial class RenameDialog : Form
    {
        public RenameDialog(RenameViewModel viewModel)
        {
            InitializeComponent();

            promptLabel.Text = $"Enter a new name for {viewModel.CurrentName}:";
            nameTextBox.BindTwoWay(nameof(TextBox.Text), viewModel, nameof(RenameViewModel.Name));
            shortNameValueLabel.BindOneTime(nameof(Label.Text), viewModel, nameof(RenameViewModel.ShortName));
            okButton.Command = viewModel.AcceptCommand;
        }
    }
}
