using System;
using System.Buffers.Binary;
using System.IO;

namespace TotalImage.Containers.Anex86
{
    /// <summary>
    /// Class for handling FDI/HDI disk images, originally used by the Anex86 PC-98 emulator and supported by various other programs as well.
    /// </summary>
    public class Anex86Container : Container
    {
        private readonly Stream _contentStream;
        private readonly Anex86Header _header;

        /// <inheritdoc />
        public override Stream Content => _contentStream;

        /// <summary>
        /// The header structure of this FDI/HDI image
        /// </summary>
        public Anex86Header Header => _header;

        /// <inheritdoc />
        public override string DisplayName => "Anex86 disk image";

        /// <inheritdoc />
        public Anex86Container(string path, bool memoryMapping) : base(path, memoryMapping)
        {
            containerStream.Seek(8, SeekOrigin.Begin);
            byte[] bytes = new byte[4];
            containerStream.Read(bytes, 0, 4);
            var headerSize = BinaryPrimitives.ReadInt32LittleEndian(bytes[0..4]);
            _contentStream = new PartialStream(containerStream, headerSize, containerStream.Length - headerSize);

            byte[] header = new byte[headerSize];
            containerStream.Seek(0, SeekOrigin.Begin);
            containerStream.Read(header, 0, headerSize);
            _header = new Anex86Header(header);
        }
    }
}
