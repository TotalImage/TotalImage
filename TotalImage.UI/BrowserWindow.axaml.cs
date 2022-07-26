using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using TotalImage.Containers.NHD;
using TotalImage.Containers.VHD;
using TotalImage.Containers;
using TotalImage.UI.ViewModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Avalonia.Input;

namespace TotalImage.UI
{
    public partial class BrowserWindow : Window
    {
        private static List<FilePickerFileType> GetPlatformFilter()
        {
            var filters = new List<FilePickerFileType>
            {
                new("Raw sector image")
                {
                    Patterns = ["*.img", "*.ima", "*.vfd", "*.flp", "*.dsk", "*.xdf", "*.hdm"]
                },
                new("ISO image")
                {
                    Patterns = [ "*.iso" ]
                },
                new("Microsoft VHD")
                {
                    Patterns = ["*.vhd"]
                },
                new("T98-Next HD")
                {
                    Patterns = ["*.nhd"]
                },
                new("All files")
                {
                    Patterns = ["*.*"]
                }
            };

            return filters;
        }

        public BrowserWindow()
        {
            InitializeComponent();
        }

        public static async Task<BrowserWindowViewModel?> OpenFileAsync(Window parent)
        {
            FilePickerOpenOptions openOptions = new()
            {
                AllowMultiple = false,
                FileTypeFilter = GetPlatformFilter()
            };

            var result = await parent.StorageProvider.OpenFilePickerAsync(openOptions);

            if (result is not { Count: 1 })
            {
                return null;
            }

            IStorageFile file = result[0];
            string path = file.Path.LocalPath.ToString();

            bool memoryMapping = false;
            string ext = Path.GetExtension(path).ToLowerInvariant();
            Container image = ext switch
            {
                ".vhd" => new VhdContainer(path.ToString(), memoryMapping),
                ".nhd" => new NhdContainer(path, memoryMapping),
                _ => new RawContainer(path, memoryMapping),
            };

            if (image.PartitionTable.Partitions.Count == 0)
            {
                return null;
            }

            if (image.PartitionTable.Partitions.Count == 1)
            {
                return new BrowserWindowViewModel(path, image, image.PartitionTable.Partitions[0]);
            }


            SelectPartitionDialog partitionWnd = new()
            {
                DataContext = new SelectPartitionViewModel(image.PartitionTable)
            };
            await partitionWnd.ShowDialog(parent);

            if (partitionWnd.DataContext is SelectPartitionViewModel spvm && spvm.SelectedPartition != null)
            {

                return new BrowserWindowViewModel(path, image, spvm.SelectedPartition);
            }

            return null;
        }

        public async void MenuOpen_OnClick(object? sender, EventArgs e)
        {
            var result = await OpenFileAsync(this);
            if (result != null)
            {
                DataContext = result;
            }
            else
            {
                Close();
            }
        }

        private void MenuQuit_OnClick(object? sender, EventArgs e)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }

        private async void Extract_OnClick(object? sender, RoutedEventArgs e)
        {
            if (FolderItems.SelectedItems.Count == 0)
            {
                return;
            }

            var items = new List<IFileSystemObjectViewModel>();
            foreach (object? selectedItem in FolderItems.SelectedItems)
            {
                if (selectedItem is IFileSystemObjectViewModel vm)
                {
                    items.Add(vm);
                }
            }

            if (items.Count == 0)
            {
                return;
            }

            await ExtractAsync(items);
        }

        private async Task ExtractAsync(IEnumerable<IFileSystemObjectViewModel> items)
        {
            var result = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                AllowMultiple = false,
                Title = "Select extraction folder"
            });
            if (result is not { Count: 1 })
            {
                return;
            }

            var folder = result[0];
            string? folderPath = folder.TryGetLocalPath();
            if (folderPath == null)
            {
                return;
            }

            foreach (var item in items)
            {
                await item.Extract(folderPath);
            }
        }

        private async void Properties_OnClick(object? sender, RoutedEventArgs e)
        {
            if (FolderItems.SelectedItems.Count == 1)
            {
                if (FolderItems.SelectedItems[0] is FileViewModel fvm)
                {
                    FilePropertiesWindow fpw = new()
                    {
                        DataContext = fvm
                    };

                    await fpw.ShowDialog(this);
                }
                else if (FolderItems.SelectedItems[0] is DirectoryViewModel dvm)
                {
                    DirectoryPropertiesWindow dpw = new()
                    {
                        DataContext = dvm
                    };

                    await dpw.ShowDialog(this);
                }
            }
            else if (FolderItems.SelectedItems.Count > 1)
            {
                // Handle multiple items property sheet
            }
        }

        private async void FolderItems_OnDoubleTapped(object? sender, TappedEventArgs e)
        {
            if (e.Source is Control c && c.DataContext is IFileSystemObjectViewModel vm)
            {
                if (vm is DirectoryViewModel dvm)
                {
                    if (DataContext is BrowserWindowViewModel bvm)
                    {
                        bvm.SelectedDirectory = dvm;
                    }
                }
                else if (vm is FileViewModel fvm)
                {
                    // Open the properties window for files on double tap
                    FilePropertiesWindow fpw = new()
                    {
                        DataContext = fvm
                    };

                    await fpw.ShowDialog(this);
                }
            }
        }
    }
}
