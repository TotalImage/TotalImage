using Avalonia.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// The partition manager. Only the list and the selection-dependent buttons exist so far.
/// Binding only.
/// </summary>
public partial class ManagePartitionsDialog : Window
{
    // For the XAML previewer.
    public ManagePartitionsDialog()
    {
        InitializeComponent();
    }

    public ManagePartitionsDialog(ManagePartitionsViewModel viewModel) : this()
    {
        DataContext = viewModel;
        // Nothing to apply yet: both buttons just close the window.
        this.BindClose(okButton);
        this.BindClose(cancelButton);
    }
}
