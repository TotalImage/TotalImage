using System.IO;

namespace TotalImage.FileSystems.ExFAT;

public interface IStreamable
{
    Stream GetStream(ExFatFileSystem fileSystem);
}
