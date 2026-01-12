using System.Security.Cryptography;

/// <summary>
/// Implementation of a CRC-32 hash algorithm.
/// </summary>
public sealed class CRC32 : HashAlgorithm
{
    private const uint Polynomial = 0xEDB88320u;
    private static readonly uint[] Table = InitializeTable();
    private uint _crc;

    public CRC32()
    {
        HashSizeValue = 32;
        Initialize();
    }

    /// <summary>
    /// Creates a new instance of the CRC-32 hash algorithm to mirror other HashAlgorithm implementations.
    /// </summary>
    public static CRC32 Create()
        => new CRC32();

    /// <inheritdoc/>
    public override void Initialize()
    {
        _crc = 0xFFFFFFFFu;
    }

    /// <inheritdoc/>
    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        uint crc = _crc;

        for (int i = ibStart; i < ibStart + cbSize; i++)
        {
            crc = Table[(crc ^ array[i]) & 0xFF] ^ (crc >> 8);
        }

        _crc = crc;
    }

    /// <inheritdoc/>
    protected override byte[] HashFinal()
    {
        uint finalCrc = _crc ^ 0xFFFFFFFFu;

        // Returns big-endian
        return
        [
            (byte)(finalCrc >> 24),
            (byte)(finalCrc >> 16),
            (byte)(finalCrc >> 8),
            (byte)(finalCrc)
        ];
    }

    private static uint[] InitializeTable()
    {
        var table = new uint[256];

        for (uint i = 0; i < table.Length; i++)
        {
            uint entry = i;

            for (int j = 0; j < 8; j++)
            {
                if ((entry & 1) == 1)
                    entry = (entry >> 1) ^ Polynomial;
                else
                    entry >>= 1;
            }

            table[i] = entry;
        }

        return table;
    }
}
