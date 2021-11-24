using System;
using System.IO;

namespace TotalImage.FileSystems.FAT
{
    public class FatDataStream : Stream
    {
        private readonly FatFileSystem _fat;
        private readonly Stream _base;

        private readonly uint[] _clusters;
        private readonly uint _length;

        private uint _position = 0;

        public FatDataStream(FatFileSystem fat, uint firstCluster)
        {
            _fat = fat;
            _base = fat.GetStream();
            _clusters = fat.GetClusterChain(firstCluster);
            _length = (uint)_clusters.Length * fat.BytesPerCluster;
        }

        public FatDataStream(FatFileSystem fat, DirectoryEntry entry, bool ignoreSize) : this(fat, (uint)(entry.fstClusHI << 16) | entry.fstClusLO)
        {
            if (!ignoreSize)
            {
                _length = entry.fileSize;
            }
        }

        public FatDataStream(FatFileSystem fat, DirectoryEntry entry) : this(fat, entry, false)
        {

        }

        /// <inheritdoc />
        public override bool CanRead => _base.CanRead;

        /// <inheritdoc />
        public override bool CanSeek => _base.CanSeek;

        /// <inheritdoc />
        public override bool CanWrite => _base.CanWrite;

        /// <inheritdoc />
        public override long Length => _length;

        /// <inheritdoc />
        public override long Position { get => _position; set => Seek(value, SeekOrigin.Begin); }

        /// <inheritdoc />
        public override void Flush() => _base.Flush();

        public override int Read(byte[] buffer, int offset, int count)
        {
            count = Math.Min(count, (int)(_length - _position));

            if (count <= 0) return 0;

            var firstCluster = _position / _fat.BytesPerCluster;
            var lastCluster = (_position + count - 1) / _fat.BytesPerCluster;
            var bytesLeftFromCluster = _fat.BytesPerCluster - (_position % _fat.BytesPerCluster);

            Seek(0, SeekOrigin.Current);

            var totalRead = _base.Read(buffer, offset, Math.Min(count, (int)bytesLeftFromCluster));
            _position += (uint)totalRead;

            for (var i = firstCluster; i < lastCluster; i++)
            {
                Seek(0, SeekOrigin.Current);

                var readBytes = _base.Read(buffer, offset + totalRead, Math.Min(count - totalRead, (int)_fat.BytesPerCluster));

                _position += (uint)readBytes;
                totalRead += readBytes;
            }

            return totalRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            var target = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => _position + offset,
                SeekOrigin.End => _length + offset,
                _ => throw new ArgumentException()
            };

            if (target < 0)
                throw new ArgumentException();

            var cluster = _clusters[target / _fat.BytesPerCluster];

            _base.Seek(_fat.DataAreaFirstSector * _fat.BiosParameterBlock.BytesPerLogicalSector, SeekOrigin.Begin);
            _base.Seek((cluster - 2) * _fat.BytesPerCluster, SeekOrigin.Current);
            _base.Seek(target % _fat.BytesPerCluster, SeekOrigin.Current);

            _position = (uint)target;
            return target;
        }

        public override void SetLength(long value)
        {
            throw new System.NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new System.NotImplementedException();
        }
    }
}