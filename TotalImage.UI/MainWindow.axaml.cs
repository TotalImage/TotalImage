using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using TotalImage.Containers;
using TotalImage.FileSystems;
using TotalImage.Partitions;
using TotalImage.UI.Converters;
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
        private readonly PartitionEntry? _partitionEntry;

        private FileSystems.Directory? Root
            => _partitionEntry?.FileSystem.RootDirectory;

        public IEnumerable<DirectoryViewModel> Directories
            => Root == null
                ? Array.Empty<DirectoryViewModel>()
                : new[] { new DirectoryViewModel(Root) };

        public string WindowTitle
            => string.IsNullOrEmpty(_imagePath)
                ? "TotalImage"
                : $"{_imagePath} - TotalImage";

        public long? PartitionSize => _partitionEntry?.FileSystem.TotalSize;
        public long? PartitionFree => _partitionEntry?.FileSystem.TotalFreeSpace;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(string path)
        {
            _imagePath = path;
            _container = new RawContainer(path, false);
            _partitionIndex = 0;
            _partitionEntry = _container == null || _partitionIndex == null
                ? null
                : _container.PartitionTable.Partitions[_partitionIndex.Value];
        }
    }

    public interface IFileSystemObjectViewModel
    {
        string Name { get; }
        string FullName { get; }
        ulong Length { get; }
        DateTime? LastWriteTime { get; }
        string Attributes { get; }
    }

    public abstract class FileSystemObjectViewModel<T> : IFileSystemObjectViewModel
        where T : FileSystemObject
    {
        protected readonly T _fsObject;

        public string Name
            => _fsObject.Name == "\0" || _fsObject.Name == ""
                ? "\\"
                : _fsObject.Name;

        public string FullName
            => _fsObject.FullName == "\0" || _fsObject.Name == ""
                ? "\\"
        : _fsObject.FullName;

        public ulong Length => _fsObject.Length;

        public DateTime? LastWriteTime => _fsObject.LastWriteTime;

        public string Attributes
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(_fsObject.Attributes.HasFlag(FileAttributes.Directory) ? 'D' : '-');
                sb.Append(_fsObject.Attributes.HasFlag(FileAttributes.Archive) ? 'A' : '-');
                sb.Append(_fsObject.Attributes.HasFlag(FileAttributes.System) ? 'S' : '-');
                sb.Append(_fsObject.Attributes.HasFlag(FileAttributes.Hidden) ? 'H' : '-');
                sb.Append(_fsObject.Attributes.HasFlag(FileAttributes.ReadOnly) ? 'R' : '-');
                return sb.ToString();
            }
        }

        public FileSystemObjectViewModel(T obj)
        {
            _fsObject = obj;
        }
    }

    public class DirectoryViewModel : FileSystemObjectViewModel<FileSystems.Directory>
    {

        public IEnumerable<DirectoryViewModel> Directories
            => _fsObject.EnumerateDirectories(true, true)
                .Select(e => new DirectoryViewModel(e));

        public IEnumerable<IFileSystemObjectViewModel> Objects
            => _fsObject.EnumerateFileSystemObjects(true, true)
                .Select(e => e is FileSystems.Directory dir
                    ? new DirectoryViewModel(dir)
                    : new FileViewModel((FileSystems.File)e) as IFileSystemObjectViewModel);

        public IEnumerable<FileViewModel> Files
            => _fsObject.EnumerateFiles(true, true)
                .Select(e => new FileViewModel(e));

        public string FooterLabel
            => Files.Any()
                ? $"{SizeConverter.Convert(Files.Sum(e => (long)e.Length))} in {Files.Count()} item(s)"
                : "";

        public DirectoryViewModel(FileSystems.Directory obj) : base(obj)
        {
        }
    }

    public class FileViewModel : FileSystemObjectViewModel<FileSystems.File>
    {
        public FileViewModel(FileSystems.File obj) : base(obj)
        {
        }
    }
}
