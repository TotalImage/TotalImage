using System;
using System.IO;
using System.Linq;
using TotalImage.Containers.TAP;
using Xunit;

namespace TotalImage.IO.Tests.Mtf;

/// <summary>
/// Exercises SIMH tape-record framing and logical stream behavior using generated payloads.
/// </summary>
public class TapStreamTests
{
    /// <summary>Verifies omission of framing, tape marks, and odd-byte padding, plus non-owning disposal.</summary>
    [Fact]
    public void RecordFramingMarksAndOddPadding_AreRemoved()
    {
        byte[] payload = Enumerable.Range(0, 1000).Select(i => (byte)i).ToArray();
        using var source = new MemoryStream(MtfFileSystemTests.WrapTape(payload, 17));
        using var tape = new TapStream(source);
        Assert.Equal(payload.LongLength, tape.Length);
        using var output = new MemoryStream();
        tape.CopyTo(output);
        Assert.Equal(payload, output.ToArray());
        tape.Position = 16;
        Assert.Equal(payload[16], tape.ReadByte());
        Assert.Equal(payload[17], tape.ReadByte());
        tape.Seek(10, SeekOrigin.End);
        Assert.Equal(-1, tape.ReadByte());
        tape.Dispose();
        Assert.True(source.CanRead);
        Assert.False(tape.CanRead);
        Assert.Throws<ObjectDisposedException>(() => tape.ReadByte());
    }

    /// <summary>Verifies that malformed or unsupported record markers produce the expected error type.</summary>
    /// <param name="hex">The hexadecimal representation of the invalid tape image.</param>
    /// <param name="exceptionType">The expected framing or unsupported-feature exception type.</param>
    [Theory]
    [InlineData("01000000", typeof(InvalidDataException))] // Missing payload/trailer.
    [InlineData("01000000410002000000", typeof(InvalidDataException))] // Mismatched trailer.
    [InlineData("01000080410001000080", typeof(InvalidDataException))] // Error record.
    [InlineData("feffffff", typeof(NotSupportedException))] // Erasure gap.
    [InlineData("ffffffff00", typeof(InvalidDataException))] // Data beyond EOM.
    [InlineData("0000", typeof(InvalidDataException))] // Partial marker.
    [InlineData("00000000ffffffff", typeof(InvalidDataException))] // No records.
    public void InvalidTapeFraming_IsRejected(string hex, Type exceptionType)
    {
        using var source = new MemoryStream(Convert.FromHexString(hex));
        Assert.Throws(exceptionType, () => new TapStream(source));
    }

    /// <summary>Verifies that a physical end of file can terminate a tape without an explicit EOM marker.</summary>
    [Fact]
    public void PhysicalEndOfFileWithoutEom_IsAccepted()
    {
        byte[] tape = MtfFileSystemTests.WrapTape("abc"u8.ToArray());
        using var source = new MemoryStream(tape[..^4]);
        using var content = new TapStream(source);
        Assert.Equal(3, content.Length);
    }
}
