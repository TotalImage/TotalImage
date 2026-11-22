using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TotalImage.Containers;
using TotalImage.FileSystems;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Tests.Main;
using Xunit;
using Directory = TotalImage.FileSystems.Directory;
using File = System.IO.File;

namespace TotalImage.ViewModels.Tests.Services;

public class FileExtractionServiceTests : IDisposable
{
    private readonly TempFolder temp = new();
    private readonly FileExtractionService service = new();
    private readonly Container image;
    private readonly string target;

    public FileExtractionServiceTests()
    {
        var path = temp.File("browse.img");
        BrowseTestImage.Write(path, temp.Path);
        image = new ImageFileService().OpenAsync(path).GetAwaiter().GetResult();
        target = temp.File("out");
    }

    public void Dispose()
    {
        image.Dispose();
        temp.Dispose();
    }

    private Directory Root => image.PartitionTable.Partitions[0].FileSystem.RootDirectory;

    private FileSystemObject Entry(string name, Directory? folder = null)
        => (folder ?? Root).EnumerateFileSystemObjects(true).Single(entry => entry.Name == name);

    private ExtractionOptions Options(DirectoryExtractionMode mode = DirectoryExtractionMode.Preserve, bool includeHidden = true,
        bool preserveDates = false, bool preserveAttributes = false)
        => new(target, mode, includeHidden, preserveDates, preserveAttributes);

    // The extracted files, relative to the target folder.
    private string[] Extracted() => System.IO.Directory.Exists(target)
        ? System.IO.Directory.GetFiles(target, "*", SearchOption.AllDirectories).Select(path => Path.GetRelativePath(target, path)).Order().ToArray()
        : [];

    private string TargetFile(string relativePath) => Path.Combine(target, relativePath);

    private void WriteExisting(string name, string text)
    {
        System.IO.Directory.CreateDirectory(target);
        File.WriteAllText(TargetFile(name), text);
    }

    [Fact]
    public async Task FilesAreCopiedIntoTheTargetFolderWhichIsCreated()
    {
        var done = await service.ExtractAsync([Entry("ALPHA.TXT"), Entry("MID.BIN")], Options(), null);

        Assert.True(done);
        Assert.Equal(["ALPHA.TXT", "MID.BIN"], Extracted());
        Assert.Equal(10, new FileInfo(TargetFile("ALPHA.TXT")).Length);
        Assert.Equal(2000, new FileInfo(TargetFile("MID.BIN")).Length);
    }

    [Fact]
    public async Task PreserveKeepsTheFolderStructure()
    {
        await service.ExtractAsync([Entry("DOCS")], Options(DirectoryExtractionMode.Preserve), null);

        Assert.Equal([Path.Combine("DOCS", "A.TXT"), Path.Combine("DOCS", "B.TXT"), Path.Combine("DOCS", "DEEP", "X.TXT")], Extracted());
    }

    [Fact]
    public async Task MergePutsEveryFileIntoTheTargetFolder()
    {
        await service.ExtractAsync([Entry("DOCS")], Options(DirectoryExtractionMode.Merge), null);

        Assert.Equal(["A.TXT", "B.TXT", "X.TXT"], Extracted());
    }

    [Fact]
    public async Task SkipLeavesFoldersOut()
    {
        await service.ExtractAsync([Entry("DOCS"), Entry("ZETA.ARC")], Options(DirectoryExtractionMode.Skip), null);

        Assert.Equal(["ZETA.ARC"], Extracted());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task HiddenEntriesInsideFoldersFollowTheOption(bool includeHidden)
    {
        await service.ExtractAsync([Root], Options(DirectoryExtractionMode.Merge, includeHidden: includeHidden), null);

        Assert.Equal(includeHidden, Extracted().Contains("HIDDEN.SYS"));
        Assert.Contains("ALPHA.TXT", Extracted());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DatesAreKeptWhenAskedTo(bool preserveDates)
    {
        await service.ExtractAsync([Entry("ALPHA.TXT")], Options(preserveDates: preserveDates), null);

        var written = File.GetLastWriteTime(TargetFile("ALPHA.TXT"));
        Assert.Equal(preserveDates, written.Year == 2001);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task AttributesAreKeptWhenAskedTo(bool preserveAttributes)
    {
        Assert.SkipUnless(OperatingSystem.IsWindows(), "File attributes are Windows-only.");
        await service.ExtractAsync([Entry("ALPHA.TXT")], Options(preserveAttributes: preserveAttributes), null);

        var readOnly = File.GetAttributes(TargetFile("ALPHA.TXT")).HasFlag(FileAttributes.ReadOnly);
        if (readOnly)
            File.SetAttributes(TargetFile("ALPHA.TXT"), FileAttributes.Normal);
        Assert.Equal(preserveAttributes, readOnly);
    }

    [Fact]
    public async Task WithoutAQuestionExistingFilesAreOverwritten()
    {
        WriteExisting("ALPHA.TXT", "old");

        var done = await service.ExtractAsync([Entry("ALPHA.TXT")], Options(), null);

        Assert.True(done);
        Assert.Equal(10, new FileInfo(TargetFile("ALPHA.TXT")).Length);
    }

    [Fact]
    public async Task OnlyExistingFilesAreAskedAbout()
    {
        WriteExisting("MID.BIN", "old");
        var asked = new List<string>();

        await service.ExtractAsync([Entry("ALPHA.TXT"), Entry("MID.BIN")], Options(), name =>
        {
            asked.Add(name);
            return Task.FromResult(new OverwriteAnswer(OverwriteChoice.Overwrite));
        });

        Assert.Equal(["MID.BIN"], asked);
        Assert.Equal(2000, new FileInfo(TargetFile("MID.BIN")).Length);
    }

    [Fact]
    public async Task SkippingKeepsTheExistingFileAndGoesOn()
    {
        WriteExisting("ALPHA.TXT", "old");

        var done = await service.ExtractAsync([Entry("ALPHA.TXT"), Entry("MID.BIN")], Options(),
            _ => Task.FromResult(new OverwriteAnswer(OverwriteChoice.Skip)));

        Assert.True(done);
        Assert.Equal("old", File.ReadAllText(TargetFile("ALPHA.TXT")));
        Assert.True(File.Exists(TargetFile("MID.BIN")));
    }

    [Fact]
    public async Task CancellingTheQuestionStopsTheExtraction()
    {
        WriteExisting("ALPHA.TXT", "old");

        var done = await service.ExtractAsync([Entry("ALPHA.TXT"), Entry("MID.BIN")], Options(),
            _ => Task.FromResult(new OverwriteAnswer(OverwriteChoice.Cancel, ApplyToAll: true)));

        Assert.False(done);
        Assert.Equal("old", File.ReadAllText(TargetFile("ALPHA.TXT")));
        Assert.False(File.Exists(TargetFile("MID.BIN")));
    }

    [Theory]
    [InlineData(OverwriteChoice.Overwrite)]
    [InlineData(OverwriteChoice.Skip)]
    public async Task AnAnswerForAllIsNotAskedAgain(OverwriteChoice choice)
    {
        string[] names = ["ALPHA.TXT", "MID.BIN", "ZETA.ARC"];
        foreach (var name in names)
            WriteExisting(name, "old");
        var asked = 0;

        await service.ExtractAsync(names.Select(name => Entry(name)), Options(), _ =>
        {
            asked++;
            return Task.FromResult(new OverwriteAnswer(choice, ApplyToAll: true));
        });

        Assert.Equal(1, asked);
        Assert.All(names, name => Assert.Equal(choice == OverwriteChoice.Skip, File.ReadAllText(TargetFile(name)) == "old"));
    }

    [Fact]
    public async Task AnAnswerForOneFileIsAskedAgainForTheNext()
    {
        string[] names = ["ALPHA.TXT", "MID.BIN"];
        foreach (var name in names)
            WriteExisting(name, "old");
        var asked = new List<string>();

        await service.ExtractAsync(names.Select(name => Entry(name)), Options(), name =>
        {
            asked.Add(name);
            return Task.FromResult(new OverwriteAnswer(asked.Count == 1 ? OverwriteChoice.Skip : OverwriteChoice.Overwrite));
        });

        Assert.Equal(names, asked);
        Assert.Equal("old", File.ReadAllText(TargetFile("ALPHA.TXT")));
        Assert.Equal(2000, new FileInfo(TargetFile("MID.BIN")).Length);
    }

    [Fact]
    public async Task ProgressCountsTheFilesAndNamesEachOne()
    {
        var reports = new List<ExtractionProgress>();

        await service.ExtractAsync([Entry("DOCS"), Entry("ALPHA.TXT")], Options(), null, new SyncProgress(reports.Add));

        Assert.Equal(
        [
            new(0, 4, null),
            new(0, 4, "ALPHA.TXT"),
            new(1, 4, "A.TXT"),
            new(2, 4, "B.TXT"),
            new(3, 4, "X.TXT")
        ], reports);
    }

    [Fact]
    public async Task CancellingBeforeTheStartExtractsNothing()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        var done = await service.ExtractAsync([Entry("ALPHA.TXT")], Options(), null, null, cancellation.Token);

        Assert.False(done);
        Assert.Empty(Extracted());
    }

    [Fact]
    public async Task CancellingStopsBeforeTheNextFile()
    {
        using var cancellation = new CancellationTokenSource();
        var progress = new SyncProgress(report =>
        {
            if (report.FileName == "MID.BIN")
                cancellation.Cancel();
        });

        var done = await service.ExtractAsync([Entry("ALPHA.TXT"), Entry("MID.BIN"), Entry("ZETA.ARC")], Options(), null, progress, cancellation.Token);

        Assert.False(done);
        Assert.Equal(["ALPHA.TXT"], Extracted());
    }

    [Fact]
    public async Task AFileThatCannotBeWrittenFails()
    {
        Assert.SkipUnless(OperatingSystem.IsWindows(), "Read-only files can be replaced elsewhere.");
        WriteExisting("ALPHA.TXT", "old");
        File.SetAttributes(TargetFile("ALPHA.TXT"), FileAttributes.ReadOnly);
        try
        {
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.ExtractAsync([Entry("ALPHA.TXT")], Options(), null));
        }
        finally
        {
            File.SetAttributes(TargetFile("ALPHA.TXT"), FileAttributes.Normal);
        }
    }

    /// <summary>
    /// Reports progress right away, on the reporting thread.
    /// </summary>
    internal sealed class SyncProgress(Action<ExtractionProgress> report) : IProgress<ExtractionProgress>
    {
        public void Report(ExtractionProgress value) => report(value);
    }
}
