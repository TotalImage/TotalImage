using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TotalImage.Containers;
using TotalImage.FileSystems;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Settings;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// The services a dialog's view model uses for the questions it asks itself (pickers and
/// messages). Each UI creates them for the dialog's own window.
/// </summary>
public sealed record DialogServices(IMessageService Messages, IFilePickerService FilePicker);

/// <summary>
/// Builds the view models of the image dialogs from the image, so that each UI only shows a
/// window bound to a view model and never sees the image itself.
/// </summary>
public abstract class ImageDialogsBase(ISettingsService settings, IImageHashService hashService) : IImageDialogs
{
    /// <inheritdoc/>
    public abstract Task<FormatOptions?> AskFormatOptionsAsync();

    /// <inheritdoc/>
    public abstract Task ShowManagePartitionsAsync();

    /// <inheritdoc/>
    public abstract Task<NewImageOptions?> AskNewImageAsync();

    /// <inheritdoc/>
    public Task<PartitionSelection?> AskSelectPartitionAsync(Container image)
        => ShowSelectPartitionAsync(services => new SelectPartitionViewModel(image, settings, services.Messages));

    /// <inheritdoc/>
    public Task ShowImageInfoAsync(Container image, string? filePath, int partitionIndex)
        => ShowImageInfoAsync(services => new ImageInfoViewModel(image, filePath, partitionIndex, settings, hashService,
            services.FilePicker, services.Messages, Assembly.GetEntryAssembly()));

    /// <inheritdoc/>
    public Task ShowBootSectorAsync(Container image, int partitionIndex)
        => ShowBootSectorAsync(services => new BootSectorViewModel(image, partitionIndex, services.FilePicker, services.Messages));

    /// <summary>
    /// Shows the partition selection dialog.
    /// </summary>
    /// <param name="createViewModel">Creates the dialog's view model, given the services owned by the dialog.</param>
    protected abstract Task<PartitionSelection?> ShowSelectPartitionAsync(Func<DialogServices, SelectPartitionViewModel> createViewModel);

    /// <summary>
    /// Shows the image information dialog.
    /// </summary>
    /// <param name="createViewModel">Creates the dialog's view model, given the services owned by the dialog.</param>
    protected abstract Task ShowImageInfoAsync(Func<DialogServices, ImageInfoViewModel> createViewModel);

    /// <summary>
    /// Shows the boot sector dialog.
    /// </summary>
    /// <param name="createViewModel">Creates the dialog's view model, given the services owned by the dialog.</param>
    protected abstract Task ShowBootSectorAsync(Func<DialogServices, BootSectorViewModel> createViewModel);
}

/// <summary>
/// Builds the view models of the entry dialogs from the entries, so that each UI only shows a
/// window bound to a view model and never sees the entries themselves.
/// </summary>
public abstract class EntryDialogsBase(ISettingsService settings, IFileTypeInfo fileTypeInfo) : IEntryDialogs
{
    /// <summary>
    /// The application settings, which some dialogs start from.
    /// </summary>
    protected ISettingsService Settings { get; } = settings;

    /// <inheritdoc/>
    public abstract Task<string?> AskRenameAsync(string currentName);

    /// <inheritdoc/>
    public abstract Task<string?> AskNewFolderNameAsync();

    /// <inheritdoc/>
    public abstract Task<VolumeLabelChange?> AskVolumeLabelAsync(string rootDirectoryLabel, string bpbLabel);

    /// <inheritdoc/>
    public abstract Task<ExtractOptions?> AskExtractOptionsAsync(int itemCount);

    /// <inheritdoc/>
    public abstract Task ShowExtractionProgressAsync(Func<IProgress<ExtractionProgress>, CancellationToken, Task> extraction);

    /// <inheritdoc/>
    public Task<EntryPropertiesChange?> AskPropertiesAsync(IReadOnlyList<FileSystemObject> entries)
        => ShowPropertiesAsync(new PropertiesViewModel(entries, Settings, fileTypeInfo), entries[0].Name, entries[0].Attributes);

    /// <summary>
    /// Shows the properties dialog.
    /// </summary>
    /// <param name="viewModel">The dialog's view model.</param>
    /// <param name="firstName">The name of the first entry, for its icon.</param>
    /// <param name="firstAttributes">The attributes of the first entry, for its icon.</param>
    protected abstract Task<EntryPropertiesChange?> ShowPropertiesAsync(PropertiesViewModel viewModel, string firstName, FileAttributes firstAttributes);
}
