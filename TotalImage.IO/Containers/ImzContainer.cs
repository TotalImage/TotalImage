using System.IO;
using System.IO.Compression;

namespace TotalImage.Containers
{
    /// <summary>
    /// Class for handling a compressed raw container (IMZ), supported by WinImage
    /// </summary>
    public class ImzContainer : RawContainer
    {
        /// <inheritdoc />
        public override Stream Content => containerStream;

        /// <inheritdoc />
        public override string DisplayName => "WinImage compressed image";

        /// <inheritdoc />
        public ImzContainer(string path, bool memoryMapping) : base(path, memoryMapping)
        {
            using (var zip = new ZipArchive(containerStream, ZipArchiveMode.Read))
            {
                foreach (var entry in zip.Entries)
                {
                    using (var stream = entry.Open())
                    {
                        //stream here is the unzipped image
                    }
                }
            }
        }
    }
}
