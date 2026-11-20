using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Sorting;
using Xunit;

namespace TotalImage.ViewModels.Tests;

public class SizeUnitTests
{
    [Theory]
    [InlineData(SizeUnit.Bytes, 1536UL, "1,536 B")]
    [InlineData(SizeUnit.Decimal, 1536UL, "1.54 KB")]
    [InlineData(SizeUnit.Binary, 1536UL, "1.5 KiB")]
    [InlineData(SizeUnit.Binary, 1000UL, "1,000 B")]
    [InlineData(SizeUnit.Binary, 1047552UL, "1 MiB")]
    public void FormatSize(SizeUnit unit, ulong size, string expected)
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        try
        {
            Assert.Equal(expected, unit.FormatSize(size));
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }

    [Fact]
    public void FormatSize_IncludeBytes()
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        try
        {
            Assert.Equal("2 KiB (2,048 B)", SizeUnit.Binary.FormatSize(2048, true));
            Assert.Equal("2 KiB", SizeUnit.Binary.FormatSize(2048, false));
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }
}

public class InvertedComparerTests
{
    [Fact]
    public void InvertsTheResult()
    {
        var comparer = new InvertedComparer<int>(Comparer<int>.Default);
        Assert.True(comparer.Compare(1, 2) > 0);
        Assert.True(comparer.Compare(2, 1) < 0);
        Assert.Equal(0, comparer.Compare(3, 3));
    }
}

public class TempDirectoryServiceTests
{
    [Fact]
    public void UniqueSubdirectories_AreUnderRootAndNotCreated()
    {
        using var folder = new TempFolder();
        var service = new TempDirectoryService(folder.Path);

        var a = service.GetUniqueSubdirectoryPath();
        var b = service.GetUniqueSubdirectoryPath();

        Assert.NotEqual(a, b);
        Assert.Equal(folder.Path, Path.GetDirectoryName(a));
        Assert.StartsWith("~", Path.GetFileName(a));
        Assert.False(Directory.Exists(a));
    }

    [Fact]
    public void Clear_EmptiesTheRoot()
    {
        using var folder = new TempFolder();
        var root = Path.Combine(folder.Path, "root");
        var service = new TempDirectoryService(root);
        service.EnsureRootExists();
        var sub = service.GetUniqueSubdirectoryPath();
        Directory.CreateDirectory(sub);
        File.WriteAllText(Path.Combine(sub, "x.txt"), "x");

        service.Clear();

        Assert.True(Directory.Exists(root));
        Assert.Empty(Directory.EnumerateFileSystemEntries(root));
    }
}
