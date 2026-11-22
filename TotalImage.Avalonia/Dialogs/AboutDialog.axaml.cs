using Avalonia.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// Shows information about the application. Binding only.
/// </summary>
public partial class AboutDialog : Window
{
    // For the XAML previewer.
    public AboutDialog()
    {
        InitializeComponent();
    }

    public AboutDialog(AboutViewModel viewModel) : this()
    {
        DataContext = viewModel;
        projectLinkText.Text = AboutViewModel.ProjectUrl;
        this.BindClose(okButton);
    }
}
