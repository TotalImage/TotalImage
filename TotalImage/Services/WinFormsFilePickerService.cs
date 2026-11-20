using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.ViewModels.Services;

namespace TotalImage.Services
{
    public sealed class WinFormsFilePickerService(DialogOwner owner) : IFilePickerService
    {
        private static string StartFolder => Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

        public Task<string?> PickImageToOpenAsync()
        {
            using OpenFileDialog dialog = new()
            {
                AutoUpgradeEnabled = true,
                InitialDirectory = StartFolder,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                Filter = ToFilter(ImageFileTypes.Open)
            };
            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? dialog.FileName : null);
        }

        public Task<string?> PickImageSaveLocationAsync(string? suggestedFileName)
            => PickSaveFileAsync(ImageFileTypes.Save, ImageFileTypes.DefaultSaveExtension, suggestedFileName);

        public Task<string?> PickSaveFileAsync(IReadOnlyList<FileTypeFilter> filters, string defaultExtension, string? suggestedFileName = null)
        {
            using SaveFileDialog dialog = new()
            {
                AutoUpgradeEnabled = true,
                InitialDirectory = StartFolder,
                OverwritePrompt = true,
                DefaultExt = defaultExtension,
                Filter = ToFilter(filters)
            };
            if (suggestedFileName is not null)
                dialog.FileName = suggestedFileName;

            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? dialog.FileName : null);
        }

        public Task<string?> PickFolderAsync(string? initialFolder = null)
        {
            using FolderBrowserDialog dialog = new() { ShowNewFolderButton = true };
            if (!string.IsNullOrEmpty(initialFolder))
                dialog.InitialDirectory = initialFolder;

            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? dialog.SelectedPath : null);
        }

        public Task<IReadOnlyList<string>> PickFilesToAddAsync()
        {
            using OpenFileDialog dialog = new()
            {
                AutoUpgradeEnabled = true,
                InitialDirectory = StartFolder,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = true,
                Filter = ToFilter([ImageFileTypes.AllFiles])
            };
            IReadOnlyList<string> result = dialog.ShowDialog(owner.Window) == DialogResult.OK ? dialog.FileNames : [];
            return Task.FromResult(result);
        }

        // "Name (*.a,*.b)|*.a;*.b|..." as the WinForms file dialogs expect.
        private static string ToFilter(IEnumerable<FileTypeFilter> filters)
            => string.Join("|", filters.Select(f => $"{f.Name} ({string.Join(", ", f.Patterns)})|{string.Join(";", f.Patterns)}"));
    }
}
