using Avalonia.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// Asks for a new name for an entry. Binding only.
/// </summary>
public partial class RenameDialog : Window
{
    // For the XAML previewer.
    public RenameDialog()
    {
        InitializeComponent();
    }

    public RenameDialog(RenameViewModel viewModel) : this()
    {
        DataContext = viewModel;
        this.BindAccept(okButton, viewModel.AcceptCommand);
        this.BindCancel(cancelButton);
        Opened += (_, _) =>
        {
            nameTextBox.Focus();
            nameTextBox.SelectAll();
        };
    }
}
