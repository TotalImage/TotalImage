using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.IMGFS
{
    /// <summary>
    /// Detects Windows CE IMGFS images.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The reference <c>eimgfs</c> implementation supports two common layouts:
    /// </para>
    /// <list type="bullet">
    /// <item><description>An IMGFS image that starts directly at stream offset 0.</description></item>
    /// <item><description>An IMGFS image wrapped in a flash image that starts with an MBR and exposes IMGFS through partition type <c>0x25</c>, optionally accompanied by an <c>MSFLSH50</c> header one sector after the MBR.</description></item>
    /// </list>
    /// <para>
    /// When IMGFS is embedded, the factory returns a file system over a bounded substream so the parser can continue to treat offset 0 as the IMGFS header.
    /// </para>
    /// </remarks>
    public class ImgfsFactory : IFileSystemFactory
    {
        private static readonly byte[] Signature = [0xf8, 0xac, 0x2c, 0x9d, 0xe3, 0xd4, 0x2b, 0x4d, 0xbd, 0x30, 0x91, 0x6e, 0xd8, 0x4f, 0x31, 0xdc];
        private static readonly byte[] MsFlashSignature = Encoding.ASCII.GetBytes("MSFLSH50");

        /// <inheritdoc />
        public FileSystem? TryLoadFileSystem(Stream stream)
        {
            if (!stream.CanSeek || !stream.CanRead || stream.Length < Signature.Length)
            {
                return null;
            }

            long originalPosition = stream.Position;
            try
            {
                if (HasImgfsSignature(stream, 0))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    return new CEImageFileSystem(stream, CEImageModulePeReconstructor.DefaultMachineTypeArm);
                }

                if (!TryFindWrappedImgfsOffset(stream, out long imgfsOffset, out long imgfsLength))
                {
                    return null;
                }

                return new CEImageFileSystem(new PartialStream(stream, imgfsOffset, imgfsLength), CEImageModulePeReconstructor.DefaultMachineTypeArm);
            }
            catch (InvalidDataException)
            {
                return null;
            }
            finally
            {
                stream.Seek(originalPosition, SeekOrigin.Begin);
            }
        }

        private static bool TryFindWrappedImgfsOffset(Stream stream, out long imgfsOffset, out long imgfsLength)
        {
            imgfsOffset = 0;
            imgfsLength = 0;

            if (stream.Length < 512 || !HasMbrSignature(stream))
            {
                return false;
            }

            uint sectorSize = DetectMsFlashSectorSize(stream);
            uint? partitionOffset = TryFindPartitionOfType(stream, sectorSize, 0x25);
            if (!partitionOffset.HasValue)
            {
                return false;
            }

            if (!HasImgfsSignature(stream, partitionOffset.Value))
            {
                return false;
            }

            imgfsOffset = partitionOffset.Value;
            imgfsLength = stream.Length - imgfsOffset;
            return true;
        }

        private static bool HasImgfsSignature(Stream stream, long offset)
        {
            if (offset < 0 || offset + Signature.Length > stream.Length)
            {
                return false;
            }

            Span<byte> buffer = stackalloc byte[16];
            stream.Seek(offset, SeekOrigin.Begin);
            stream.ReadExactly(buffer);
            return buffer.SequenceEqual(Signature);
        }

        private static bool HasMbrSignature(Stream stream)
        {
            Span<byte> signature = stackalloc byte[2];
            stream.Seek(0x1FE, SeekOrigin.Begin);
            stream.ReadExactly(signature);
            return BinaryPrimitives.ReadUInt16LittleEndian(signature) == 0xAA55;
        }

        private static uint DetectMsFlashSectorSize(Stream stream)
        {
            Span<byte> buffer = stackalloc byte[8];
            for (uint candidate = 512; candidate <= 65536 && candidate + buffer.Length <= stream.Length; candidate *= 2)
            {
                stream.Seek(candidate, SeekOrigin.Begin);
                stream.ReadExactly(buffer);
                if (buffer.SequenceEqual(MsFlashSignature))
                {
                    return candidate;
                }
            }

            return 512;
        }

        private static uint? TryFindPartitionOfType(Stream stream, uint sectorSize, byte partitionType)
        {
            Span<byte> partitionTable = stackalloc byte[64];
            stream.Seek(0x1BE, SeekOrigin.Begin);
            stream.ReadExactly(partitionTable);

            for (int index = 0; index < 4; index++)
            {
                ReadOnlySpan<byte> entry = partitionTable.Slice(index * 16, 16);
                if (entry[4] != partitionType)
                {
                    continue;
                }

                uint lbaStart = BinaryPrimitives.ReadUInt32LittleEndian(entry[8..12]);
                return lbaStart * sectorSize;
            }

            return null;
        }
    }
}
