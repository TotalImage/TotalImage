using System.Buffers.Binary;
using System.IO;

namespace TotalImage.Containers
{
    /// <summary>
    /// Class for handling DiskDupe disk images (DDI).
    /// </summary>
    /// <remarks>Currently, we ignore the DiskDupe header at the start of the image because its contents are very poorly understood.</remarks>
    public class DiskDupeContainer : Container
    {
        private readonly Stream _contentStream;

        /// <inheritdoc />
        public override Stream Content => _contentStream;

        /// <inheritdoc />
        public override string DisplayName => "DiskDupe disk image";

        /// <inheritdoc />
        public DiskDupeContainer(string path, bool memoryMapping) : base(path, memoryMapping)
        {
            //NOTE 1: there are disk images that have the entire header before the data area zeroed out. Some alternate detection method is needed for such images...
            //NOTE 2: DiskDupe images are often truncated at the end, which we don't handle properly yet...

            containerStream.Seek(0, SeekOrigin.Begin);
            byte[] magicBytes = new byte[11]; //Magic bytes
            containerStream.Read(magicBytes, 0, 11);
            short dataOffset = 0x1200; // If magicBytes[10] == 0x01 or 0x03, data usually starts at 0x1200

            //Otherwise, it usually starts at 0x1E00 or 0x2400
            if (magicBytes[10] == 0x02)
                dataOffset = 0x1E00;
            else if (magicBytes[10] == 0x04)
                dataOffset = 0x2400;
            else //It's none of the above, so just search for the first occurrence of the jump instruction (0xEB) in the first 0x3000 bytes of the file
            {
                for(int i = 0; i < 0x3000; i += 0x10)
                {
                    containerStream.Seek(i, SeekOrigin.Begin);
                    byte[] jmpBytes = new byte[4];

                    containerStream.Read(jmpBytes, 0, 4);

                    if (jmpBytes[0] == 0xEB || jmpBytes[0] == 0x69 || jmpBytes[0] == 0xE9)
                    {
                        dataOffset = (short)i;
                        break;
                    }
                }
            }

            _contentStream = new PartialStream(containerStream, dataOffset, containerStream.Length - dataOffset);
        }
    }
}
