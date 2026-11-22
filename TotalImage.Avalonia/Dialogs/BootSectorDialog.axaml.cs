using Avalonia.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// Shows the boot sector of the disk or the selected partition and exports it. Binding only.
/// </summary>
public partial class BootSectorDialog : Window
{
    // For the XAML previewer.
    public BootSectorDialog()
    {
        InitializeComponent();
    }

    public BootSectorDialog(BootSectorViewModel viewModel) : this()
    {
        DataContext = viewModel;

        // Nothing can be changed yet, so OK and Cancel both just close the window.
        this.BindClose(okButton);
        this.BindCancel(cancelButton);
    }
}
