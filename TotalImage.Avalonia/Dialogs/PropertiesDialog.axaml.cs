using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// Shows and edits the properties of one or more entries. Binding only.
/// </summary>
public partial class PropertiesDialog : Window
{
    // For the XAML previewer.
    public PropertiesDialog()
    {
        InitializeComponent();
    }

    /// <param name="icon">The large icon of the entry (or entries).</param>
    public PropertiesDialog(PropertiesViewModel viewModel, IImage icon) : this()
    {
        DataContext = viewModel;
        iconImage.Source = icon;
        this.BindAccept(okButton, viewModel.AcceptCommand);
        this.BindCancel(cancelButton);

        // Clicking a hash field calculates the hashes. The text box handles the press itself, so
        // listen before it does.
        foreach (var hashTextBox in new[] { crc32TextBox, md5TextBox, sha1TextBox })
        {
            hashTextBox.AddHandler(PointerPressedEvent, (_, _) =>
            {
                if (viewModel.CalculateHashesCommand.CanExecute(null))
                    viewModel.CalculateHashesCommand.Execute(null);
            }, RoutingStrategies.Tunnel);
        }

        Opened += (_, _) => viewModel.LoadCommand.Execute(null);
        Closing += (_, _) => viewModel.StopBackgroundWork();
    }
}
