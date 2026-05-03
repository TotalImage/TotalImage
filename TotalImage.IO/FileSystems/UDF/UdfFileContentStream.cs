using System;
using System.IO;

namespace TotalImage.FileSystems.UDF
{
    internal sealed class UdfFileContentStream : Stream
    {
        private readonly UdfFileSystem _fileSystem;
        private readonly UdfFileEntryInfo _entry;
        private readonly long _length;
        private long _position;

        internal UdfFileContentStream(UdfFileSystem fileSystem, UdfFileEntryInfo entry)
        {
            _fileSystem = fileSystem;
            _entry = entry;
            _length = checked((long)Math.Min(entry.InformationLength, (ulong)long.MaxValue));
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => _length;

        public override long Position
        {
            get => _position;
            set
            {
                if (value < 0 || value > _length)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _position = value;
            }
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ArgumentNullException.ThrowIfNull(buffer);
            ArgumentOutOfRangeException.ThrowIfNegative(offset);
            ArgumentOutOfRangeException.ThrowIfNegative(count);

            if (buffer.Length - offset < count)
            {
                throw new ArgumentException();
            }

            int read = Read(buffer.AsSpan(offset, count));
            return read;
        }

        public override int Read(Span<byte> buffer)
        {
            int read = _fileSystem.ReadContent(_entry, _position, buffer);
            _position += read;
            return read;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long target = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => _position + offset,
                SeekOrigin.End => _length + offset,
                _ => throw new ArgumentOutOfRangeException(nameof(origin))
            };

            Position = target;
            return _position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}
