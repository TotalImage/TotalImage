using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.Dialogs;
using TotalImage.FileSystems;
using TotalImage.Properties;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Services
{
    public sealed class WinFormsEntryDialogs(DialogOwner owner, ISettingsService settings, IFilePickerService filePicker, IMessageService messages, WinFormsFileTypeInfo fileTypeInfo) : IEntryDialogs
    {
        public Task<string?> AskRenameAsync(string currentName)
        {
            var viewModel = new RenameViewModel(currentName);
            using var dialog = new RenameDialog(viewModel);
            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? viewModel.Answer : null);
        }

        public Task<string?> AskNewFolderNameAsync()
        {
            var viewModel = new NewFolderViewModel();
            using var dialog = new NewFolderDialog(viewModel);
            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? viewModel.Answer : null);
        }

        public Task<VolumeLabelChange?> AskVolumeLabelAsync(string rootDirectoryLabel, string bpbLabel)
        {
            var viewModel = new VolumeLabelViewModel(rootDirectoryLabel, bpbLabel);
            using var dialog = new ChangeVolumeLabelDialog(viewModel);
            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? viewModel.Answer : null);
        }

        public Task<ExtractOptions?> AskExtractOptionsAsync(int itemCount)
        {
            var viewModel = new ExtractViewModel(itemCount, settings, filePicker, messages);
            using var dialog = new ExtractDialog(viewModel);
            return Task.FromResult(ShowWithNestedDialogs(dialog) == DialogResult.OK ? viewModel.Answer : null);
        }

        public Task<EntryPropertiesChange?> AskPropertiesAsync(IReadOnlyList<FileSystemObject> entries)
        {
            var viewModel = new PropertiesViewModel(entries, settings, fileTypeInfo);
            var (icon, stretch) = viewModel.IsMultiple
                ? (Resources.page_white_stack, true)
                : fileTypeInfo.GetLargeIcon(entries[0].Name, entries[0].Attributes);
            using var dialog = new PropertiesDialog(viewModel, icon, stretch);
            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? viewModel.Answer : null);
        }

        public Task ShowExtractionProgressAsync(Func<IProgress<ExtractionProgress>, CancellationToken, Task> extraction)
        {
            using var cancellation = new CancellationTokenSource();

            // The window stays open until the extraction has stopped, so the image is never read
            // while the main window can be used.
            var cancelButton = TaskDialogButton.Cancel;
            cancelButton.AllowCloseDialog = false;
            cancelButton.Click += (_, _) => cancellation.Cancel();

            var page = new TaskDialogPage()
            {
                Caption = "Extracting",
                Heading = "Extracting files, please wait...",
                Text = "This might take a while, so sit back and relax while we do all the work.",
                Buttons = { cancelButton },
                ProgressBar = new TaskDialogProgressBar()
                {
                    State = TaskDialogProgressBarState.Marquee
                },
                Footnote = new TaskDialogFootnote()
                {
                    Text = "Enumerating files..."
                }
            };

            var progress = new Progress<ExtractionProgress>(report =>
            {
                page.ProgressBar.Maximum = report.Total;
                page.ProgressBar.State = TaskDialogProgressBarState.Normal;
                page.ProgressBar.Value = report.Completed;
                if (report.FileName is not null)
                    page.Footnote.Text = $"Extracting {report.FileName}";
            });

            Exception? failure = null;
            page.Created += async (_, _) =>
            {
                // Questions asked during the extraction (like overwriting a file) belong to this window.
                var parent = owner.Window;
                owner.Window = page.BoundDialog;
                try
                {
                    await extraction(progress, cancellation.Token);
                }
                catch (Exception ex)
                {
                    failure = ex;
                }
                finally
                {
                    owner.Window = parent;
                    page.BoundDialog?.Close();
                }
            };

            if (owner.Window is null)
                TaskDialog.ShowDialog(page);
            else
                TaskDialog.ShowDialog(owner.Window, page);

            return failure is null ? Task.CompletedTask : Task.FromException(failure);
        }

        // Shows a dialog whose view model asks further questions, so that the pickers and
        // messages it opens are owned by the dialog rather than the main window.
        private DialogResult ShowWithNestedDialogs(Form dialog)
        {
            var parent = owner.Window;
            owner.Window = dialog;
            try
            {
                return dialog.ShowDialog(parent);
            }
            finally
            {
                owner.Window = parent;
            }
        }
    }
}
