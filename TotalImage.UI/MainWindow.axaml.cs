using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;

namespace TotalImage.UI
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OpenButton_OnClick(object? sender, RoutedEventArgs e)
        {
            var result = await BrowserWindow.OpenFileAsync(this);
            if (result != null)
            {
                BrowserWindow wnd = new BrowserWindow()
                {
                    DataContext = result
                };
                wnd.Show();
            }

            Close();
        }

        private void QuitButton_OnClick(object? sender, RoutedEventArgs e)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
    }
}
