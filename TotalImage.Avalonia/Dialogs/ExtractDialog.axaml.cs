using Avalonia.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// Asks where and how to extract the selected entries. Binding only.
/// </summary>
public partial class ExtractDialog : Window
{
    // For the XAML previewer.
    public ExtractDialog()
    {
        InitializeComponent();
    }

    public ExtractDialog(ExtractViewModel viewModel) : this()
    {
        DataContext = viewModel;
        this.BindCancel(cancelButton);

        // The view model may ask to create the target folder first, and the dialog stays open
        // when the user declines.
        okButton.Click += async (_, e) =>
        {
            e.Handled = true;
            if (await viewModel.TryAcceptAsync())
                Close(true);
        };
        Opened += (_, _) => targetPathTextBox.Focus();
    }
}
