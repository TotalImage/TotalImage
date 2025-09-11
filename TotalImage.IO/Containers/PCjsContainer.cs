using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TotalImage.Containers;

/// <summary>
/// Represents a PCjs disk image.
/// </summary>
public class PCjsContainer : Container
{
    /// <inheritdoc />
    public override Stream Content { get; }

    /// <inheritdoc />
    public override string DisplayName => "PCjs Disk Image";

    /// <inheritdoc />
    public override bool IsReadOnly => false;

    /// <inheritdoc />
    public PCjsContainer(string path, bool memoryMapping) : base(path, memoryMapping)
    {
        var root = JsonDocument.Parse(containerStream).RootElement;

        if (root.ValueKind == JsonValueKind.Array)
        {
            throw new NotSupportedException("PCjs v1 disk images are not supported.");
        }

        try
        {
            var imageInfo = root.GetProperty("imageInfo");
            var diskSize = imageInfo.GetProperty("diskSize").GetInt32();
            var heads = imageInfo.GetProperty("heads").GetInt32();
            var trackDefault = imageInfo.GetProperty("trackDefault").GetInt32();
            var sectorDefault = imageInfo.GetProperty("sectorDefault").GetInt32();

            var image = new byte[diskSize];

            foreach (var cylinder in root.GetProperty("diskData").EnumerateArray())
            {
                foreach (var head in cylinder.EnumerateArray())
                {
                    foreach (var sector in head.EnumerateArray())
                    {
                        var c = sector.GetProperty("c").GetInt32();
                        var h = sector.GetProperty("h").GetInt32();
                        var s = sector.GetProperty("s").GetInt32();
                        var l = sector.GetProperty("l").GetInt32();

                        var offset = ((c * heads + h) * trackDefault + s - 1) * sectorDefault;

                        var d = sector.GetProperty("d").Deserialize<int[]>();

                        var bytes = new byte[l];

                        for (var i = 0; i < l / 4; i++)
                        {
                            BinaryPrimitives.WriteInt32LittleEndian(image.AsSpan()[(offset + i * 4)..], d[i < d.Length ? i : d.Length - 1]);
                        }
                    }
                }
            }

            Content = new MemoryStream(image);
        }
        catch (JsonException e)
        {
            throw new InvalidDataException("The JSON document is not a valid PCjs disk image.", e);
        }
        catch (KeyNotFoundException e)
        {
            throw new InvalidDataException("The JSON document is not a valid PCjs disk image.", e);
        }
        catch (FormatException e)
        {
            throw new InvalidDataException("The JSON document is not a valid PCjs disk image.", e);
        }
        catch (InvalidOperationException e)
        {
            throw new InvalidDataException("The JSON document is not a valid PCjs disk image.", e);
        }
    }
}
