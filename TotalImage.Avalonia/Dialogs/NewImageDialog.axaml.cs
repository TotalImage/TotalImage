using Avalonia.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// Asks what kind of image to create. Binding only.
/// </summary>
public partial class NewImageDialog : Window
{
    // For the XAML previewer.
    public NewImageDialog()
    {
        InitializeComponent();
    }

    public NewImageDialog(NewImageViewModel viewModel) : this()
    {
        DataContext = viewModel;
        this.BindCancel(cancelButton);

        // OK runs the view model's create command, which may ask before answering. The dialog
        // closes only once it has answered: it does not when creating an image without a BPB was refused.
        okButton.Click += async (_, e) =>
        {
            e.Handled = true;
            if (!viewModel.CreateCommand.CanExecute(null))
                return;

            await viewModel.CreateCommand.ExecuteAsync(null);
            if (viewModel.Answer is not null)
                Close(true);
        };
    }
}
