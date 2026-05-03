using System;
using System.IO;

namespace TotalImage.FileSystems.ExFAT
{
    /// <summary>
    /// Provides stream access to data stored in an exFAT cluster chain.
    /// </summary>
    public class ExFatDataStream : Stream
    {
        private readonly ExFatFileSystem _fileSystem;
        private readonly Stream _base;

        private readonly uint[] _clusters;
        private readonly long _length;

        private uint _position = 0;

        /// <summary>
        /// Creates a stream over an exFAT cluster chain.
        /// </summary>
        /// <param name="fileSystem">The file system that owns the cluster chain.</param>
        /// <param name="firstCluster">The first cluster in the chain.</param>
        public ExFatDataStream(ExFatFileSystem fileSystem, uint firstCluster)
        {
            _fileSystem = fileSystem;
            _base = fileSystem.GetStream();
            _clusters = fileSystem.ActiveFat.GetClusterChain(firstCluster);
            _length = _clusters.Length * fileSystem.BytesPerCluster;
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

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            count = Math.Min(count, (int)(_length - _position));

            if (count <= 0) return 0;

            var firstCluster = _position / _fileSystem.BytesPerCluster;
            var lastCluster = (_position + count - 1) / _fileSystem.BytesPerCluster;
            var bytesLeftFromCluster = _fileSystem.BytesPerCluster - (_position % _fileSystem.BytesPerCluster);

            Seek(0, SeekOrigin.Current);

            var totalRead = _base.Read(buffer, offset, Math.Min(count, (int)bytesLeftFromCluster));
            _position += (uint)totalRead;

            for (var i = firstCluster; i < lastCluster; i++)
            {
                Seek(0, SeekOrigin.Current);

                var readBytes = _base.Read(buffer, offset + totalRead, Math.Min(count - totalRead, (int)_fileSystem.BytesPerCluster));

                _position += (uint)readBytes;
                totalRead += readBytes;
            }

            return totalRead;
        }

        /// <inheritdoc />
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

            var cluster = _clusters[target / _fileSystem.BytesPerCluster];

            _base.Seek(_fileSystem.BootSector.ClusterHeapOffset * _fileSystem.BytesPerSector, SeekOrigin.Begin);
            _base.Seek((cluster - 2) * _fileSystem.BytesPerCluster, SeekOrigin.Current);
            _base.Seek(target % _fileSystem.BytesPerCluster, SeekOrigin.Current);

            _position = (uint)target;
            return target;
        }

        /// <inheritdoc />
        public override void SetLength(long value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new System.NotImplementedException();
        }
    }
}
