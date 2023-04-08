using System;
using System.IO;

namespace TotalImage.Containers.VHD;

internal class VhdStream : Stream
{
    private readonly VhdContainer _vhd;
    private readonly Stream _base;

    private long _position = 0;

    internal VhdStream(VhdContainer vhd, Stream baseStream)
    {
        _vhd = vhd;
        _base = baseStream;
    }

    public override bool CanRead => _base.CanRead;

    public override bool CanSeek => _base.CanSeek;

    public override bool CanWrite => _base.CanWrite;

    public override long Length => (long)_vhd.Footer.CurrentSize;

    public override long Position
    {
        get => _position;
        set => Seek(value, SeekOrigin.Begin);
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
            var firstBlock = Position / _vhd.DynamicHeader.BlockSize;
            var lastBlock = (Position + count - 1) / _vhd.DynamicHeader.BlockSize;
            var blockRemaining = _vhd.DynamicHeader.BlockSize - (Position % _vhd.DynamicHeader.BlockSize);

            Seek(0, SeekOrigin.Current);

            var totalRead = _vhd.BlockAllocationTable.HasData(firstBlock) ?
                _base.Read(buffer, offset, (int)Math.Min(count, blockRemaining)) :
                ReadZeros(buffer, offset, (int)Math.Min(count, blockRemaining));

            _position += totalRead;

            for (var i = firstBlock; i < lastBlock; i++)
            {
                Seek(0, SeekOrigin.Current);

                var readBytes = _vhd.BlockAllocationTable.HasData(i) ?
                    _base.Read(buffer, offset + totalRead, Math.Min(count - totalRead, (int)_vhd.DynamicHeader.BlockSize)) :
                    ReadZeros(buffer, offset + totalRead, Math.Min(count - totalRead, (int)_vhd.DynamicHeader.BlockSize));

                _position += (uint)readBytes;
                totalRead += readBytes;
            }

            return totalRead;
        }
        else
        {
            Seek(0, SeekOrigin.Current);
            return _base.Read(buffer, offset, count);
        }
    }

    private int ReadZeros(byte[] buffer, int offset, int count)
    {
        var result = 0;
        for(var i = offset; i < count; i++)
        {
            buffer[0] = 0;
            result++;
        }
        return result;
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
            var block = (uint)target / _vhd.DynamicHeader.BlockSize;
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
