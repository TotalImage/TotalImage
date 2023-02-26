using System.IO;
using System.IO.Compression;

namespace TotalImage.Containers
{
    /// <summary>
    /// Class for handling a compressed raw container (IMZ), supported by WinImage
    /// </summary>
    public class ImzContainer : Container
    {
        /// <inheritdoc />
        public override Stream Content { get; }

        /// <inheritdoc />
        public override string DisplayName => "WinImage compressed image";

        /// <inheritdoc />
        public ImzContainer(string path, bool memoryMapping) : base(path, memoryMapping)
        {
            var zip = new ZipArchive(containerStream, ZipArchiveMode.Read);
            Content = new MemoryStream();
            zip.Entries[0].Open().CopyTo(Content);
        }
    }
}
