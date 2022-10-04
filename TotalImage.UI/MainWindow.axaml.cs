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
using TotalImage.Containers.NHD;
using TotalImage.Containers.VHD;
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
            OpenFileDialog ofd = new()
            {
                AllowMultiple = false,
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter
                    {
                        Name = "Raw sector image (*.img, *.ima, *.vfd, *.flp, *.dsk, *.xdf, *.hdm)",
                        Extensions = new List<string> { "*.img", "*.ima", "*.vfd", "*.flp", "*.dsk", "*.xdf", "*.hdm" }
                    },
                    new FileDialogFilter
                    {
                        Name = "ISO image (*.iso)",
                        Extensions = new List<string> { "*.iso" }
                    },
                    new FileDialogFilter
                    {
                        Name = "Microsoft VHD (*.vhd)",
                        Extensions = new List<string> { "*.vhd" }
                    },
                    new FileDialogFilter
                    {
                        Name = "T98-Next HD (*.nhd)",
                        Extensions = new List<string> { "*.nhd" }
                    },
                    new FileDialogFilter
                    {
                        Name = "All files (*.*)",
                        Extensions = new List<string> { "*.*" }
                    }
                }
            };

            string[]? result = await ofd.ShowAsync(this);
            if (result is not { Length: 1 })
            {
                return;
            }

            string path = result[0];

            bool memoryMapping = false;
            string ext = Path.GetExtension(path).ToLowerInvariant();
            Container image = ext switch
            {
                ".vhd" => new VhdContainer(path, memoryMapping),
                ".nhd" => new NhdContainer(path, memoryMapping),
                _ => new RawContainer(path, memoryMapping),
            };

            if (image.PartitionTable.Partitions.Count == 0)
            {
                return;
            }

            if (image.PartitionTable.Partitions.Count == 1)
            {
                DataContext = new MainWindowViewModel(path, image, image.PartitionTable.Partitions[0]);
                return;
            }


            SelectPartitionDialog partitionWnd = new()
            {
                DataContext = new SelectPartitionViewModel(image.PartitionTable)
            };
            await partitionWnd.ShowDialog(this);

            if (partitionWnd.DataContext is SelectPartitionViewModel spvm && spvm.SelectedPartition != null)
            {

                DataContext = new MainWindowViewModel(path, image, spvm.SelectedPartition);
            }
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
        private readonly PartitionEntry? _partitionEntry;
        private IFileSystemObjectViewModel? _selectedItem;

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

        public PartitionEntry? SelectedPartition
        {
            get => _partitionEntry;
        }

        public IFileSystemObjectViewModel? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(string path, Container? container, PartitionEntry? partition)
        {
            _imagePath = path;
            _container = container;
            _partitionEntry = partition;
            _selectedItem = _partitionEntry != null
                ? new DirectoryViewModel(_partitionEntry.FileSystem.RootDirectory)
                : null;
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
