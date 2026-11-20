using System.Windows.Forms;
using TotalImage.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Dialogs
{
    public partial class NewFolderDialog : Form
    {
        public NewFolderDialog(NewFolderViewModel viewModel)
        {
            InitializeComponent();

            nameTextBox.BindTwoWay(nameof(TextBox.Text), viewModel, nameof(NewFolderViewModel.Name));
            shortNameValueLabel.BindOneTime(nameof(Label.Text), viewModel, nameof(NewFolderViewModel.ShortName));
            okButton.Command = viewModel.AcceptCommand;
        }
    }
}
