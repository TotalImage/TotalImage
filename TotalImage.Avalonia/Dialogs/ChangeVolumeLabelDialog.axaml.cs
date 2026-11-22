using Avalonia.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// Asks for new volume labels for a FAT volume. Binding only.
/// </summary>
public partial class ChangeVolumeLabelDialog : Window
{
    // For the XAML previewer.
    public ChangeVolumeLabelDialog()
    {
        InitializeComponent();
    }

    public ChangeVolumeLabelDialog(VolumeLabelViewModel viewModel) : this()
    {
        DataContext = viewModel;
        this.BindAccept(okButton, viewModel.AcceptCommand);
        this.BindCancel(cancelButton);
        Opened += (_, _) => rootDirectoryLabelTextBox.Focus();
    }
}
