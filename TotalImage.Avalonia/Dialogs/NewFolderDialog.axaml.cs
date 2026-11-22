using Avalonia.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// Asks for the name of a new folder. Binding only.
/// </summary>
public partial class NewFolderDialog : Window
{
    // For the XAML previewer.
    public NewFolderDialog()
    {
        InitializeComponent();
    }

    public NewFolderDialog(NewFolderViewModel viewModel) : this()
    {
        DataContext = viewModel;
        this.BindAccept(okButton, viewModel.AcceptCommand);
        this.BindCancel(cancelButton);
        Opened += (_, _) => nameTextBox.Focus();
    }
}
