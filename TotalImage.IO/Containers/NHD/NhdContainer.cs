using System.IO;

namespace TotalImage.Containers.NHD
{
    /// <summary>
    /// Represents a T98-Next HD (NHD) container file
    /// </summary>
    public class NhdContainer : Container, IContainerComment
    {
        private readonly Stream _contentStream;
        private readonly NhdHeader _header;

        /// <inheritdoc />
        public NhdContainer(string path, bool memoryMapping) : base(path, memoryMapping)
        {
            _contentStream = new PartialStream(containerStream, 512, containerStream.Length);

            byte[] header = new byte[512];
            containerStream.Seek(0, SeekOrigin.Begin);
            containerStream.Read(header, 0, 512);
            _header = new NhdHeader(header);
        }

        /// <inheritdoc />
        public override Stream Content => _contentStream;

        /// <inheritdoc />
        public override string DisplayName => "T98-Next HD";

        /// <inheritdoc />
        public string? Comment => Header.Comment;

        /// <summary>
        /// The header structure of this NHD image
        /// </summary>
        public NhdHeader Header => _header;
    }
}
