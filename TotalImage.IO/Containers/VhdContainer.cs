using System;
using System.IO;

namespace TotalImage.Containers
{
    /// <summary>
    /// Represents a Microsoft Virtual Hard Disk (VHD) container file
    /// </summary>
    public class VhdContainer : Container
    {
        private readonly Stream _contentStream;

        /// <inheritdoc />
        public VhdContainer(string path) : base(path)
        {
            _contentStream = new PartialStream(containerStream, 0, containerStream.Length - 512);
        }

        /// <inheritdoc />
        public VhdContainer(Stream stream) : base(stream)
        {
            _contentStream = new PartialStream(containerStream, 0, containerStream.Length - 512);
        }

        /// <inheritdoc />
        public override Stream Content => _contentStream;

        /// <inheritdoc />
        public override string DisplayName => "Microsoft VHD";
    }
}
