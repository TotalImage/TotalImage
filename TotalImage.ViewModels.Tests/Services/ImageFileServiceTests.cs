using System;
using System.IO;
using System.Threading.Tasks;
using TotalImage.Changes;
using TotalImage.Containers;
using TotalImage.FileSystems.FAT;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Tests.Dialogs;
using Xunit;

namespace TotalImage.ViewModels.Tests.Services;

public class ImageFileServiceTests : IDisposable
{
    private readonly TempFolder temp = new();
    private readonly ImageFileService service = new();

    public void Dispose() => temp.Dispose();

    private string WriteFloppy(string name = "disk.img")
    {
        var path = temp.File(name);
        File.WriteAllBytes(path, PartitionTestImages.CreateFatVolume());
        return path;
    }

    [Fact]
    public async Task OpensARawImageAndDetectsItsFileSystem()
    {
        var path = WriteFloppy();

        using var image = await service.OpenAsync(path);

        Assert.IsType<RawContainer>(image);
        Assert.IsAssignableFrom<FatFileSystem>(image.PartitionTable.Partitions[0].FileSystem);
    }

    [Fact]
    public async Task AnEmptyFileIsRejected()
    {
        var path = temp.File("empty.img");
        File.WriteAllBytes(path, []);

        var ex = await Assert.ThrowsAsync<EmptyImageFileException>(() => service.OpenAsync(path));
        Assert.Equal(path, ex.Path);
    }

    [Fact]
    public async Task AMissingFileIsReported()
    {
        await Assert.ThrowsAsync<FileNotFoundException>(() => service.OpenAsync(temp.File("missing.img")));
    }

    [Fact]
    public async Task AnUnknownExtensionIsRejected()
    {
        var path = temp.File("disk.xyz");
        File.WriteAllBytes(path, PartitionTestImages.CreateFatVolume());

        await Assert.ThrowsAsync<InvalidDataException>(() => service.OpenAsync(path));
    }

    [Fact]
    public async Task SavingAnUnchangedImageCopiesItAndReleasesTheContainer()
    {
        var source = WriteFloppy();
        var target = temp.File("copy.img");
        var image = await service.OpenAsync(source);

        await service.SaveAsync(image, target);

        Assert.Equal(File.ReadAllBytes(source), File.ReadAllBytes(target));
        Assert.Throws<ObjectDisposedException>(() => image.Content.Position = 0);
    }

    [Fact]
    public async Task SavingAChangedImageWritesThePendingChanges()
    {
        var path = WriteFloppy();
        var image = await service.OpenAsync(path);
        image.PendingChanges.Add(new CreateDirectoryChange(["NEWDIR"]));

        await service.SaveAsync(image, path);

        using var reopened = await service.OpenAsync(path);
        Assert.Contains(reopened.PartitionTable.Partitions[0].FileSystem.RootDirectory.EnumerateDirectories(true), d => d.Name == "NEWDIR");
    }
}
