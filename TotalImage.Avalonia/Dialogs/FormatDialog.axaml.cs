using Avalonia.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// Asks how to format the current partition. Binding only.
/// </summary>
public partial class FormatDialog : Window
{
    // For the XAML previewer.
    public FormatDialog()
    {
        InitializeComponent();
    }

    public FormatDialog(FormatViewModel viewModel) : this()
    {
        DataContext = viewModel;
        this.BindAccept(okButton, viewModel.AcceptCommand);
        this.BindCancel(cancelButton);
    }
}
