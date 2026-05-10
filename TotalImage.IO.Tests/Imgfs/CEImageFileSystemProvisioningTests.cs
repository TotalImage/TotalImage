using System.Linq;
using System.Text;
using TotalImage.FileSystems.IMGFS;
using Xunit;

namespace TotalImage.IO.Tests.Imgfs;

public class CEImageFileSystemProvisioningTests
{
    [Fact]
    public void ParseProvisioningOverlayData_DecodesUtf16HexEscapesInPaths()
    {
        byte[] bytes = Encoding.UTF8.GetBytes("Directory(\"\\Window\\x0073\"):-File(\"calc.exe\",\"calc.exe\")");

        var overlay = CEImageFileSystem.ParseProvisioningOverlayData(bytes);

        Assert.Contains("Windows", overlay.Directories);
        Assert.Equal("calc.exe", overlay.FileMappings["Windows\\calc.exe"]);
    }

    [Fact]
    public void ParseProvisioningOverlayData_TrimsTrailingDirectorySeparators()
    {
        byte[] bytes = Encoding.UTF8.GetBytes("Directory(\"\\Windows\\\"):-Directory(\"Start Menu\\\"):-File(\"App.lnk\",\"app.lnk\")");

        var overlay = CEImageFileSystem.ParseProvisioningOverlayData(bytes);

        Assert.Contains("Windows", overlay.Directories);
        Assert.Contains("Windows\\Start Menu", overlay.Directories);
        Assert.Equal("app.lnk", overlay.FileMappings["Windows\\Start Menu\\App.lnk"]);
    }

    [Fact]
    public void ParseProvisioningOverlayData_SupportsRootResetWithinChainedSegments()
    {
        byte[] bytes = Encoding.UTF8.GetBytes("Directory(\"Windows\"):-Directory(\"StartUp\"):-root:-File(\"welcome.exe\",\"welcome.exe\")");

        var overlay = CEImageFileSystem.ParseProvisioningOverlayData(bytes);

        Assert.Contains("Windows", overlay.Directories);
        Assert.Contains("Windows\\StartUp", overlay.Directories);
        Assert.Equal("welcome.exe", overlay.FileMappings["welcome.exe"]);
    }

    [Fact]
    public void MergeProvisioningOverlayData_LaterOverlayWinsForDuplicateTargets()
    {
        byte[] initObj = Encoding.UTF8.GetBytes("Directory(\"\\Windows\"):-File(\"Calendar.lnk\",\"calendar_obj.lnk\")");
        byte[] initFlashFiles = Encoding.UTF8.GetBytes("Directory(\"\\Windows\\\"):-File(\"Calendar.lnk\",\"calendar_flash.lnk\")");

        var overlay = CEImageFileSystem.MergeProvisioningOverlayData(initObj, initFlashFiles);

        Assert.Equal("calendar_flash.lnk", overlay.FileMappings["Windows\\Calendar.lnk"]);
    }

    [Fact]
    public void ParseProvisioningOverlayData_DecodesUtf16LittleEndianText()
    {
        byte[] bytes = Encoding.Unicode.GetPreamble()
            .Concat(Encoding.Unicode.GetBytes("Directory(\"\\Windows\\\"):-File(\"notes.txt\",\"note\\x0073.txt\")"))
            .ToArray();

        var overlay = CEImageFileSystem.ParseProvisioningOverlayData(bytes);

        Assert.Equal("notes.txt", overlay.FileMappings["Windows\\notes.txt"]);
        Assert.Contains("Windows", overlay.Directories);
    }
}
