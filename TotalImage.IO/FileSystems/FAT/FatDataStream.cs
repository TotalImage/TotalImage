using System;
using System.IO;

namespace TotalImage.FileSystems.FAT
{
    public class FatDataStream : Stream
    {
        private readonly Fat12 _fat12;
        private readonly Stream _base;

        private readonly uint _firstCluster;
        private readonly uint _length;

        private uint _position = 0;

        public FatDataStream(Fat12 fat12, DirectoryEntry entry)
        {
            _fat12 = fat12;
            _base = fat12.GetStream();
            _firstCluster = (uint)(entry.fstClusHI << 16) | entry.fstClusLO;
            _length = entry.fileSize;
        }

        public FatDataStream(Fat12 fat12, uint firstCluster)
        {
            _fat12 = fat12;
            _base = fat12.GetStream();
            _firstCluster = firstCluster;
            _length = _fat12.BytesPerCluster;

            var cluster = firstCluster;
            while((cluster = _fat12.GetNextCluster(cluster)) <= 0xFEF)
                _length += _fat12.BytesPerCluster;
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

            var firstCluster = _position / _fat12.BytesPerCluster;
            var lastCluster = (_position + count) / _fat12.BytesPerCluster;
            var bytesLeftFromCluster = _fat12.BytesPerCluster - (_position % _fat12.BytesPerCluster);

            Seek(0, SeekOrigin.Current);

            var totalRead = _base.Read(buffer, offset, Math.Min(count, (int)bytesLeftFromCluster));
            _position += (uint)totalRead;

            for (var i = firstCluster; i < lastCluster; i++)
            {
                Seek(0, SeekOrigin.Current);

                var readBytes = _base.Read(buffer, offset + totalRead, Math.Min(count - totalRead, (int)_fat12.BytesPerCluster));

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

            if (target < 0 || target > _length)
                throw new ArgumentOutOfRangeException();

            var cluster = _firstCluster;

            for(int i = 0; i < target / _fat12.BytesPerCluster; i++)
            {
                cluster = _fat12.GetNextCluster(cluster);
            }

            _base.Seek(_fat12.DataAreaFirstSector * _fat12.BiosParameterBlock.BytesPerLogicalSector, SeekOrigin.Begin);
            _base.Seek((cluster - 2) * _fat12.BytesPerCluster, SeekOrigin.Current);
            _base.Seek(target % _fat12.BytesPerCluster, SeekOrigin.Current);

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