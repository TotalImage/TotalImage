using System.Collections.Generic;
using System.Runtime.InteropServices;
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

namespace TotalImage.UI
{
    public partial class BrowserWindow : Window
    {
        private static List<FileDialogFilter> GetPlatformFilter()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return new List<FileDialogFilter>
                {
                    new FileDialogFilter
                    {
                        Name = "Supported files",
                        Extensions = new List<string> { "*.img", "*.ima", "*.vfd", "*.flp", "*.dsk", "*.xdf", "*.hdm", "*.iso", "*.vhd", "*.nhd" }
                    }
                };
            }

            return new List<FileDialogFilter>
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
            };
        }

        public BrowserWindow()
        {
            InitializeComponent();
        }

        public static async Task<BrowserWindowViewModel?> OpenFileAsync(Window parent)
        {
            OpenFileDialog ofd = new()
            {
                AllowMultiple = false,
                Filters = GetPlatformFilter()
            };

            string[]? result = await ofd.ShowAsync(parent);
            if (result is not { Length: 1 })
            {
                return null;
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

        public async void MenuOpen_OnClick(object? sender, RoutedEventArgs e)
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

        private void MenuQuit_OnClick(object? sender, RoutedEventArgs e)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
    }
}
