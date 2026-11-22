using Avalonia.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// The advanced options of a new floppy image. Binding only: the new image view model reads the
/// values back however the dialog was closed.
/// </summary>
public partial class NewImageAdvancedDialog : Window
{
    // For the XAML previewer.
    public NewImageAdvancedDialog()
    {
        InitializeComponent();
    }

    public NewImageAdvancedDialog(NewImageAdvancedViewModel viewModel) : this()
    {
        DataContext = viewModel;
        this.BindClose(okButton);
    }
}
