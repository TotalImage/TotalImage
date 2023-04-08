using System.IO;
using System.IO.Compression;
using System.Linq;

namespace TotalImage.Containers
{
    /// <summary>
    /// Class for handling a compressed raw container (IMZ), supported by WinImage
    /// </summary>
    public class ImzContainer : Container, IContainerComment
    {
        /// <inheritdoc />
        public override Stream Content { get; }

        /// <inheritdoc />
        public string? Comment { get; }

        /// <inheritdoc />
        public override string DisplayName => "WinImage compressed image";

        /// <inheritdoc />
        public ImzContainer(string path, bool memoryMapping) : base(path, memoryMapping)
        {
            using var zip = new ZipArchive(containerStream, ZipArchiveMode.Read, true);
            var image = zip.Entries.Single();

            Content = new MemoryStream((int)image.Length);

            using var imageStream = image.Open();
            imageStream.CopyTo(Content);

            Comment = zip.Comment;
        }
    }
}
