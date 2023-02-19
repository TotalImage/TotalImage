using System;
using System.Collections.Generic;
using System.ComponentModel;
using TotalImage.Partitions;

namespace TotalImage.UI.ViewModel
{
    public class BrowserWindowViewModel : INotifyPropertyChanged
    {
        private readonly string _imagePath = "";
        private readonly Containers.Container? _container;
        private readonly PartitionEntry? _partitionEntry;
        private DirectoryViewModel? _selectedDirectory;

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

        public DirectoryViewModel? SelectedDirectory
        {
            get => _selectedDirectory;
            set
            {
                _selectedDirectory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDirectory)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public BrowserWindowViewModel()
        {
        }

        public BrowserWindowViewModel(string path, Containers.Container? container, PartitionEntry? partition)
        {
            _imagePath = path;
            _container = container;
            _partitionEntry = partition;
            _selectedDirectory = _partitionEntry != null
                ? new DirectoryViewModel(_partitionEntry.FileSystem.RootDirectory)
                : null;
        }
    }
}
