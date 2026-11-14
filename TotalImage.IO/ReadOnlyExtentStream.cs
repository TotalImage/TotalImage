using System;
using System.Collections.Generic;
using System.IO;

namespace TotalImage;

/// <summary>
/// A contiguous byte range in a backing stream.
/// </summary>
/// <param name="Offset">The absolute byte offset of the range in the backing stream.</param>
/// <param name="Length">The number of bytes in the range.</param>
internal readonly record struct StreamExtent(long Offset, long Length);

/// <summary>
/// A read-only, seekable, non-owning view of concatenated ranges of another stream.
/// </summary>
/// <remarks>
/// Each view maintains its own position and serializes access to the shared source while reading.
/// Disposing a view does not dispose its source stream.
/// </remarks>
internal class ReadOnlyExtentStream : Stream
{
    /// <summary>The source stream containing the indexed ranges.</summary>
    private readonly Stream _source;
    /// <summary>The non-empty source ranges, in logical stream order.</summary>
    private readonly StreamExtent[] _extents;
    /// <summary>The logical starting offset of each range.</summary>
    private readonly long[] _starts;
    /// <summary>The combined length of all ranges.</summary>
    private readonly long _length;
    /// <summary>The current position within this logical view.</summary>
    private long _position;
    /// <summary>Whether this view has been disposed.</summary>
    private bool _disposed;

    /// <summary>
    /// Creates a view of the specified source ranges in the supplied order.
    /// </summary>
    /// <param name="source">The readable, seekable stream containing the ranges.</param>
    /// <param name="extents">The source ranges to concatenate. Empty ranges are omitted.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
    /// <exception cref="ArgumentException">The source is not readable and seekable.</exception>
    /// <exception cref="InvalidDataException">A range is negative or extends beyond the source.</exception>
    /// <exception cref="OverflowException">The combined range length exceeds <see cref="long.MaxValue"/>.</exception>
    internal ReadOnlyExtentStream(Stream source, IReadOnlyList<StreamExtent> extents)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (!source.CanRead || !source.CanSeek)
        {
            throw new ArgumentException("A readable, seekable stream is required.", nameof(source));
        }
        _source = source;
        var ranges = new List<StreamExtent>();
        var starts = new List<long>();
        foreach (var extent in extents)
        {
            if (extent.Offset < 0 || extent.Length < 0 || extent.Offset > source.Length - extent.Length)
            {
                throw new InvalidDataException("An extent extends beyond the source stream.");
            }
            if (extent.Length == 0)
            {
                continue;
            }
            starts.Add(_length);
            ranges.Add(extent);
            _length = checked(_length + extent.Length);
        }
        _extents = ranges.ToArray();
        _starts = starts.ToArray();
    }

    /// <inheritdoc />
    public override bool CanRead => !_disposed && _source.CanRead;

    /// <inheritdoc />
    public override bool CanSeek => !_disposed && _source.CanSeek;

    /// <inheritdoc />
    public override bool CanWrite => false;

    /// <inheritdoc />
    public override long Length
    {
        get
        {
            ThrowIfDisposed();
            return _length;
        }
    }

    /// <inheritdoc />
    public override long Position
    {
        get
        {
            ThrowIfDisposed();
            return _position;
        }
        set
        {
            ThrowIfDisposed();
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            _position = value;
        }
    }

    /// <inheritdoc />
    /// <exception cref="EndOfStreamException">The backing stream ends before an indexed range is read.</exception>
    public override int Read(byte[] buffer, int offset, int count)
    {
        return Read(buffer.AsSpan(offset, count));
    }

    /// <inheritdoc />
    /// <exception cref="EndOfStreamException">The backing stream ends before an indexed range is read.</exception>
    public override int Read(Span<byte> buffer)
    {
        ThrowIfDisposed();
        int total = 0;
        while (!buffer.IsEmpty && _position < _length)
        {
            int index = Array.BinarySearch(_starts, _position);
            if (index < 0)
            {
                index = ~index - 1;
            }
            var extent = _extents[index];
            long within = _position - _starts[index];
            int count = (int)Math.Min(buffer.Length, extent.Length - within);
            int read;
            // Separate file views have independent positions, even when interleaved.
            lock (_source)
            {
                _source.Position = extent.Offset + within;
                read = _source.Read(buffer[..count]);
            }
            if (read == 0)
            {
                throw new EndOfStreamException("The image was truncated while reading an extent.");
            }
            _position += read;
            total += read;
            buffer = buffer[read..];
        }
        return total;
    }

    /// <inheritdoc />
    /// <exception cref="OverflowException">The requested position exceeds the signed 64-bit offset range.</exception>
    public override long Seek(long offset, SeekOrigin origin)
    {
        long start = origin switch
        {
            SeekOrigin.Begin => 0,
            SeekOrigin.Current => Position,
            SeekOrigin.End => Length,
            _ => throw new ArgumentOutOfRangeException(nameof(origin))
        };
        long target = checked(start + offset);
        if (target < 0)
        {
            throw new IOException("Cannot seek before the beginning of the stream.");
        }
        return Position = target;
    }

    /// <inheritdoc />
    public override void Flush()
    {
        ThrowIfDisposed();
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">The view is read-only.</exception>
    public override void SetLength(long value)
    {
        throw new NotSupportedException("The image is read-only.");
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">The view is read-only.</exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException("The image is read-only.");
    }

    /// <inheritdoc />
    /// <remarks>Only this view is disposed; the backing stream remains open.</remarks>
    protected override void Dispose(bool disposing)
    {
        _disposed = true;
        base.Dispose(disposing);
    }

    /// <summary>
    /// Rejects operations on a disposed view.
    /// </summary>
    /// <exception cref="ObjectDisposedException">This view has been disposed.</exception>
    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }
}
