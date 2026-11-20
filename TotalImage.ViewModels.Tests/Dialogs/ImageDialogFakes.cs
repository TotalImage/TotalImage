using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TotalImage.Containers;
using TotalImage.DiskGeometries;
using TotalImage.FileSystems.BPB;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.ViewModels.Tests.Dialogs;

/// <summary>
/// Images and fake services for the image information and boot sector view model tests.
/// </summary>
internal static class ImageDialogFakes
{
    /// <summary>
    /// A blank 1.44 MB FAT12 floppy with a DOS 4.0 extended BPB.
    /// </summary>
    public static RawContainer CreateFloppy()
    {
        var geometry = FloppyGeometry.KnownGeometries[FloppyGeometry.FriendlyName.HighDensity1440k];
        var bpb = ExtendedBiosParameterBlock.FromGeometry(geometry, BiosParameterBlockVersion.Dos40, "MSDOS5.0", "1234ABCD", "FAT12", "TESTDISK");
        return RawContainer.CreateImage(bpb, geometry.Tracks, true);
    }

    public sealed class FakeSettings(SizeUnit sizeUnit = SizeUnit.Bytes) : ISettingsService
    {
        public SettingsModel Current { get; } = new() { SizeUnit = sizeUnit };

        public event EventHandler? ExternallyChanged { add { } remove { } }

        public void Save() { }

        public void Reload() { }

        public void ResetToDefaults() { }

        public void AddRecentImage(string path) { }

        public void RemoveRecentImage(string path) { }

        public void ClearRecentImages() { }

        public void RemoveMissingRecentImages() { }
    }

    /// <summary>
    /// Records the questions and messages, answering confirmations with <see cref="ConfirmAnswer"/>.
    /// </summary>
    public sealed class Messages : MessageServiceStub
    {
        public bool ConfirmAnswer { get; set; } = true;

        public int ExportWithoutHashesQuestions { get; private set; }

        public List<string> ExportSucceeded { get; } = [];

        public List<(string Operation, Exception Exception)> Errors { get; } = [];

        public override Task<bool> ConfirmFormatAsync() => Task.FromResult(ConfirmAnswer);

        public override Task<bool> ConfirmExportWithoutHashesAsync()
        {
            ExportWithoutHashesQuestions++;
            return Task.FromResult(ConfirmAnswer);
        }

        public override Task ShowExportSucceededAsync(string path)
        {
            ExportSucceeded.Add(path);
            return Task.CompletedTask;
        }

        public override Task ShowErrorAsync(string operation, Exception exception)
        {
            Errors.Add((operation, exception));
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Answers save pickers with <see cref="SavePath"/> and records what they offered.
    /// </summary>
    public sealed class FilePicker : IFilePickerService
    {
        public string? SavePath { get; set; }

        public int SaveRequests { get; private set; }

        public IReadOnlyList<FileTypeFilter>? LastFilters { get; private set; }

        public string? LastDefaultExtension { get; private set; }

        public Task<string?> PickSaveFileAsync(IReadOnlyList<FileTypeFilter> filters, string defaultExtension, string? suggestedFileName = null)
        {
            SaveRequests++;
            LastFilters = filters;
            LastDefaultExtension = defaultExtension;
            return Task.FromResult(SavePath);
        }

        public Task<string?> PickImageToOpenAsync() => throw new NotSupportedException();

        public Task<string?> PickImageSaveLocationAsync(string? suggestedFileName) => throw new NotSupportedException();

        public Task<string?> PickFolderAsync(string? initialFolder = null) => throw new NotSupportedException();

        public Task<IReadOnlyList<string>> PickFilesToAddAsync() => throw new NotSupportedException();
    }

    /// <summary>
    /// Returns a hash only when the test completes it, so the pending state can be observed.
    /// </summary>
    public sealed class ManualHashService : IImageHashService
    {
        private readonly Queue<TaskCompletionSource<string>> pending = new();

        public List<ImageHashKind> Requests { get; } = [];

        public Task<string> ComputeAsync(Stream stream, ImageHashKind kind, CancellationToken cancellationToken)
        {
            Requests.Add(kind);
            var source = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            cancellationToken.Register(() => source.TrySetCanceled(cancellationToken));
            pending.Enqueue(source);
            return source.Task;
        }

        public void Complete(string value) => pending.Dequeue().SetResult(value);
    }

    /// <summary>
    /// A clock stopped at a fixed UTC time.
    /// </summary>
    public sealed class FixedTime(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now;

        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;
    }
}
