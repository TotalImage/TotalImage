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

            OpenFolderDialog ofd = new OpenFolderDialog();
            string? folder = await ofd.ShowAsync(this);
            if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder))
            {
                return;
            }

            foreach (object? selectedItem in FolderItems.SelectedItems)
            {
                if (selectedItem is not IFileSystemObjectViewModel fsovm)
                {
                    return;
                }

                await fsovm.Extract(folder);
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
                    // Handle directory property sheet
                }
            }
            else if (FolderItems.SelectedItems.Count > 1)
            {
                // Handle multiple items property sheet
            }
        }
    }
}
