using System;
using System.IO;

namespace TotalImage
{
    /// <summary>
    /// A stream that represents a part of an underlying stream
    /// </summary>
    internal class PartialStream : Stream
    {
        private readonly Stream _base;
        private readonly long _length;
        private readonly long _offsetStart;
        private long _position;

        /// <inheritdoc />
        public override bool CanRead => _base.CanRead;

        /// <inheritdoc />
        public override bool CanSeek => _base.CanSeek;

        /// <inheritdoc />
        public override bool CanWrite => _base.CanWrite;

        /// <inheritdoc />
        public override long Length => _length;

        /// <summary>
        /// The position within the partial stream. It is tracked independently of the
        /// underlying stream, which may be shared with other readers.
        /// </summary>
        public override long Position
        {
            get => _position;
            set
            {
                if (value < 0 || value > _length)
                {
                    throw new IOException();
                }

                _position = value;
            }
        }

        /// <summary>
        /// Creates a partial stream from
        /// </summary>
        /// <param name="stream">The underlying stream</param>
        /// <param name="offset">The start of the partial stream</param>
        /// <param name="length">The length of the partial stream</param>
        public PartialStream(Stream stream, long offset, long length)
        {
            _base = stream;
            _length = length;
            _offsetStart = offset;
        }

        /// <inheritdoc />
        public override void Flush() => _base.Flush();

        private void SyncBasePosition()
        {
            long target = _offsetStart + _position;
            if (_base.Position != target)
            {
                _base.Position = target;
            }
        }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_position >= _length || count <= 0)
            {
                return 0; // already at end of partial stream
            }

            SyncBasePosition();
            int revisedCount = (int)Math.Min(count, _length - _position);
            int actualRead = _base.Read(buffer, offset, revisedCount);
            _position += actualRead;
            return actualRead;
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            long target = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.End => offset + _length,
                SeekOrigin.Current => offset + _position,
                _ => throw new NotImplementedException(),
            };

            if (target < 0)
            {
                throw new ArgumentException();
            }

            _position = target;
            return _position;
        }

        /// <inheritdoc />
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (count > _length - _position)
            {
                throw new ArgumentOutOfRangeException();
            }

            SyncBasePosition();
            _base.Write(buffer, offset, count);
            _position += count;
        }
    }
}
