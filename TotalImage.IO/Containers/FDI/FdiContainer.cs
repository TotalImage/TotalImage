using System.IO;

namespace TotalImage.Containers.FDI
{
    /// <summary>
    /// Class for handling FDI disk images, used and supported by various PC-98 emulators.
    /// </summary>
    public class FdiContainer : Container
    {
        private readonly Stream _contentStream;
        private readonly FdiHeader _header;

        /// <inheritdoc />
        public override Stream Content => _contentStream;

        /// <summary>
        /// The header structure of this FDI image
        /// </summary>
        public FdiHeader Header => _header;

        /// <inheritdoc />
        public override string DisplayName => "Anex86 floppy disk image";

        /// <inheritdoc />
        public FdiContainer(string path, bool memoryMapping) : base(path, memoryMapping)
        {
            _contentStream = new PartialStream(containerStream, 4096, containerStream.Length - 4096);

            byte[] header = new byte[4096];
            containerStream.Seek(0, SeekOrigin.Begin);
            containerStream.Read(header, 0, 4096);
            _header = new FdiHeader(header);
        }
    }
}
