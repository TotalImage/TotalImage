using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using TotalImage.Containers;
using TotalImage.FileSystems;
using Container = TotalImage.Containers.Container;

namespace TotalImage.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private async void MenuOpen_OnClick(object? sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string[]? result = await ofd.ShowAsync(this);
            if (result is not { Length: 1 })
            {
                return;
            }

            DataContext = new MainWindowViewModel(result[0]);
        }

        private void MenuQuit_OnClick(object? sender, RoutedEventArgs e)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly string _imagePath = "";
        private readonly Container? _container;
        private readonly int? _partitionIndex;

        private Directory? Root
            => _container == null || _partitionIndex == null
                ? null
                : _container.PartitionTable.Partitions[_partitionIndex.Value].FileSystem.RootDirectory;

        public IEnumerable<DirectoryViewModel> Directories
            => Root == null
                ? Array.Empty<DirectoryViewModel>()
                : new [] { new DirectoryViewModel(Root) };

        public string WindowTitle
            => string.IsNullOrEmpty(_imagePath)
                ? "TotalImage"
                : $"{_imagePath} - TotalImage";

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(string path)
        {
            _imagePath = path;
            _container = new RawContainer(path, false);
            _partitionIndex = 0;
        }
    }

    public class DirectoryViewModel
    {
        private readonly Directory _directory;

        public string Name => _directory.Name;

        public IEnumerable<DirectoryViewModel> Directories
            => _directory.EnumerateDirectories(true, true)
                .Select(e => new DirectoryViewModel(e));

        public IEnumerable<FileSystemObject> Objects
            => _directory.EnumerateFileSystemObjects(true, true);

        public DirectoryViewModel(Directory dir)
        {
            _directory = dir;
        }
    }
}
