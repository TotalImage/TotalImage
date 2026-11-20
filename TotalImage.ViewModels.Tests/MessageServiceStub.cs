using System;
using System.Threading.Tasks;
using TotalImage.ViewModels.Services;

namespace TotalImage.ViewModels.Tests;

/// <summary>
/// Base for test fakes of <see cref="IMessageService"/>: every member throws unless a test fake
/// overrides it. When a method is added to the interface, add it here only.
/// </summary>
internal abstract class MessageServiceStub : IMessageService
{
    private static Exception Unexpected(string name) => new NotSupportedException($"Unexpected call to {name}.");

    public virtual Task<bool> ConfirmFormatAsync() => throw Unexpected(nameof(ConfirmFormatAsync));
    public virtual Task ShowRecentImagesClearedAsync() => throw Unexpected(nameof(ShowRecentImagesClearedAsync));
    public virtual Task<bool> ConfirmResetSettingsAsync() => throw Unexpected(nameof(ConfirmResetSettingsAsync));
    public virtual Task ShowSettingsResetAsync() => throw Unexpected(nameof(ShowSettingsResetAsync));
    public virtual Task<bool> ConfirmClearTempFolderWithOtherInstancesAsync() => throw Unexpected(nameof(ConfirmClearTempFolderWithOtherInstancesAsync));
    public virtual Task ShowTempFolderClearedAsync() => throw Unexpected(nameof(ShowTempFolderClearedAsync));
    public virtual Task ShowTempFolderClearFailedAsync() => throw Unexpected(nameof(ShowTempFolderClearFailedAsync));
    public virtual Task ShowPartitionNotSupportedAsync() => throw Unexpected(nameof(ShowPartitionNotSupportedAsync));
}
