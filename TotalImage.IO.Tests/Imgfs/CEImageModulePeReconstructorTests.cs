using System;
using System.Buffers.Binary;
using TotalImage.FileSystems.IMGFS;
using Xunit;

namespace TotalImage.IO.Tests.Imgfs;

public class CEImageModulePeReconstructorTests
{
    [Fact]
    public void Reconstruct_BuildsPortableExecutableFromSingleSectionModule()
    {
        byte[] romHeader = new byte[0x70 + 0x18];

        BinaryPrimitives.WriteUInt16LittleEndian(romHeader.AsSpan(0x00, 2), 1);
        BinaryPrimitives.WriteUInt16LittleEndian(romHeader.AsSpan(0x02, 2), 0x2102);
        BinaryPrimitives.WriteUInt32LittleEndian(romHeader.AsSpan(0x04, 4), 0x1000);
        BinaryPrimitives.WriteUInt32LittleEndian(romHeader.AsSpan(0x08, 4), 0x10000000);
        BinaryPrimitives.WriteUInt16LittleEndian(romHeader.AsSpan(0x0C, 2), 4);
        BinaryPrimitives.WriteUInt16LittleEndian(romHeader.AsSpan(0x0E, 2), 0);
        BinaryPrimitives.WriteUInt32LittleEndian(romHeader.AsSpan(0x10, 4), 0x2000);
        BinaryPrimitives.WriteUInt32LittleEndian(romHeader.AsSpan(0x14, 4), 0x2000);
        BinaryPrimitives.WriteUInt32LittleEndian(romHeader.AsSpan(0x20, 4), 0x12345678);
        BinaryPrimitives.WriteUInt16LittleEndian(romHeader.AsSpan(0x6C, 2), 2);

        BinaryPrimitives.WriteUInt32LittleEndian(romHeader.AsSpan(0x70 + 0x00, 4), 0x100);
        BinaryPrimitives.WriteUInt32LittleEndian(romHeader.AsSpan(0x70 + 0x04, 4), 0x1000);
        BinaryPrimitives.WriteUInt32LittleEndian(romHeader.AsSpan(0x70 + 0x08, 4), 0x100);
        BinaryPrimitives.WriteUInt32LittleEndian(romHeader.AsSpan(0x70 + 0x0C, 4), 0x1000);
        BinaryPrimitives.WriteUInt32LittleEndian(romHeader.AsSpan(0x70 + 0x10, 4), 0x10001000);
        BinaryPrimitives.WriteUInt32LittleEndian(romHeader.AsSpan(0x70 + 0x14, 4), 0x60000020);

        byte[] sectionData =
        [
            0x2D, 0xE9, 0xF0, 0x41, 0x04, 0x00, 0x9F, 0xE5,
            0x04, 0x10, 0x9F, 0xE5, 0x04, 0x20, 0x9F, 0xE5
        ];

        byte[] image = CEImageModulePeReconstructor.Reconstruct(
            romHeader,
            [new CEImageModulePeReconstructor.ModuleSectionData(".text", sectionData)]);

        Assert.Equal((byte)'M', image[0]);
        Assert.Equal((byte)'Z', image[1]);

        int peOffset = BinaryPrimitives.ReadInt32LittleEndian(image.AsSpan(0x3C, 4));
        Assert.Equal(0x80, peOffset);
        Assert.Equal(0x00004550u, BinaryPrimitives.ReadUInt32LittleEndian(image.AsSpan(peOffset, 4)));
        Assert.Equal(CEImageModulePeReconstructor.DefaultMachineTypeArm, BinaryPrimitives.ReadUInt16LittleEndian(image.AsSpan(peOffset + 4, 2)));
        Assert.Equal((ushort)1, BinaryPrimitives.ReadUInt16LittleEndian(image.AsSpan(peOffset + 6, 2)));
        Assert.Equal(0x1000u, BinaryPrimitives.ReadUInt32LittleEndian(image.AsSpan(peOffset + 0x28, 4)));

        int sectionHeaderOffset = peOffset + 0x18 + 0xE0;
        Assert.Equal((byte)'.', image[sectionHeaderOffset + 0]);
        Assert.Equal((byte)'t', image[sectionHeaderOffset + 1]);
        Assert.Equal((byte)'e', image[sectionHeaderOffset + 2]);
        Assert.Equal((byte)'x', image[sectionHeaderOffset + 3]);
        Assert.Equal((byte)'t', image[sectionHeaderOffset + 4]);

        uint rawPointer = BinaryPrimitives.ReadUInt32LittleEndian(image.AsSpan(sectionHeaderOffset + 0x14, 4));
        Assert.Equal(sectionData[0], image[rawPointer]);
        Assert.Equal(sectionData[1], image[rawPointer + 1]);
    }
}
