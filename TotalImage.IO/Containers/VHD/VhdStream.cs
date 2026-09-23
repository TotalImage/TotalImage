using System;
using System.IO;

namespace TotalImage.Containers.VHD;

internal class VhdStream : Stream
{
    private readonly VhdContainer _vhd;
    private readonly Stream _base;
    private long _position = 0;

    public override bool CanRead => _base.CanRead;

    public override bool CanSeek => _base.CanSeek;

    public override bool CanWrite => _base.CanWrite;

    public override long Length => (long)_vhd.Footer.CurrentSize;

    public override long Position
    {
        get => _position;
        set => Seek(value, SeekOrigin.Begin);
    }

    internal VhdStream(VhdContainer vhd, Stream baseStream)
    {
        _vhd = vhd;
        _base = baseStream;
    }

    public override void Flush()
    {
        throw new System.NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        count = (int)Math.Min(count, Length - Position);

        if (count <= 0) return 0;

        if (_vhd.DynamicHeader != null && _vhd.BlockAllocationTable != null)
        {
            var totalRead = 0;
            while (totalRead < count)
            {
                long block = Position / _vhd.DynamicHeader.BlockSize;
                int chunk = (int)Math.Min(count - totalRead, _vhd.DynamicHeader.BlockSize - Position % _vhd.DynamicHeader.BlockSize);
                Seek(0, SeekOrigin.Current);
                int readBytes = _vhd.BlockAllocationTable.HasData(block)
                    ? _base.Read(buffer, offset + totalRead, chunk)
                    : ReadZeros(buffer, offset + totalRead, chunk);
                if (readBytes == 0) break;
                _position += readBytes;
                totalRead += readBytes;
            }

            return totalRead;
        }
        else
        {
            Seek(0, SeekOrigin.Current);
            int read = _base.Read(buffer, offset, count);
            _position += read;
            return read;
        }
    }

    private int ReadZeros(byte[] buffer, int offset, int count)
    {
        Array.Clear(buffer, offset, count);
        return count;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        var target = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => Position + offset,
            SeekOrigin.End => Length + offset,
            _ => throw new ArgumentException()
        };

        if (target < 0)
            throw new ArgumentException();

        if (_vhd.DynamicHeader != null && _vhd.BlockAllocationTable != null)
        {
            if (target == Length)
            {
                _position = target;
                return target;
            }
            var block = (uint)(target / _vhd.DynamicHeader.BlockSize);
            var blockOffset = target % _vhd.DynamicHeader.BlockSize;

            if (_vhd.BlockAllocationTable.HasData(block))
                _base.Seek(_vhd.BlockAllocationTable.GetBlockDataAddress(block) + blockOffset, SeekOrigin.Begin);
        }
        else
        {
            _base.Seek(target, SeekOrigin.Begin);
        }

        _position = target;
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
