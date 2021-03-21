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
        private readonly long _offsetEnd;

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
            _offsetEnd = offset + length;
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
        public override long Position
        {
            get => _base.Position + _offsetStart;
            set
            {
                if (value >= _length)
                {
                    throw new IOException();
                }

                _base.Position = value + _offsetStart;
            }
        }

        /// <inheritdoc />
        public override void Flush() => _base.Flush();

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_base.Position >= _offsetEnd)
            {
                return 0; // already at end of partial stream
            }

            long targetEnd = _base.Position + count;
            if (targetEnd < _offsetEnd)
            {
                // falls entirely within partial stream
                return _base.Read(buffer, offset, count);
            }

            int revisedCount = (int)(_offsetEnd - _base.Position);
            int actualRead = _base.Read(buffer, offset, revisedCount);

            return Math.Min(revisedCount, actualRead);
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            long target;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    target = offset + _offsetStart;
                    break;
                case SeekOrigin.End:
                    target = offset + _offsetEnd;
                    break;
                case SeekOrigin.Current:
                    target = offset + _base.Position;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (target < _offsetStart || target >= _offsetEnd)
            {
                throw new ArgumentOutOfRangeException();
            }

            long result = _base.Seek(target, SeekOrigin.Begin);
            return result - _offsetStart;
        }

        /// <inheritdoc />
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            if ((_base.Position + count) >= _offsetEnd)
            {
                throw new ArgumentOutOfRangeException();
            }

            _base.Write(buffer, offset, count);
        }
    }
}
