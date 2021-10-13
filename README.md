# TotalImage
**TotalImage** is a free and open source disk image editor licensed under the MIT license. For more information, see the `LICENSE` file. It is written in C# and uses Windows Forms for the UI.

## Current status
See the [Status page](https://github.com/TotalImage/TotalImage/blob/master/Docs/status.md) for details about currently implemented features.

### Downloads
Currently the project is still in early stages of development (alpha-level), and as such is not quite ready for everyday use yet. We will provide official builds once we reach that stage, in the mean time you can build it yourself or ask us to provide binaries for you (see the *Development* and *Support* sections below).

### System requirements
* [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0) Desktop Runtime
* Windows 8.1/10/11

Support for other operating systems will be considered within the limitations of .NET.

## Planned features
Below is a list of things we plan to support. Please note that this list is not exhaustive nor definitive, and may change over time. We're open to adding support for more stuff in the future, but we have to start with the basics.

### Media types
* Floppy disks
* Hard disks
* Optical discs
* Superfloppies

### Disk image containers
* Raw sector images (.IMG, .IMA etc.)
* TeleDisk (.TD0)
* ImageDisk (.IMD)
* 86Box 86F (.86F)
* Virtual Hard Disk (.VHD)
* ISO (.ISO)
* And more

### Physical disk formats
* Common PC-compatible formats (160/180/320/360/720/1200/1440/1680/2880k)
* 8" formats (such as 1232k) and others with sector sizes larger/smaller than 512 bytes
* IBM XDF, DEC RX50 400k, Acorn 640k/800k, and more

### File systems
* FAT12, FAT16, FAT32
* HPFS, NTFS
* High Sierra, ISO9660, UDF
* And more

### Partitioning schemes
* Various MBR variants
* GPT

## Development
To develop TotalImage, we recommend you install [Microsoft Visual Studio 2019](https://visualstudio.microsoft.com/vs/). This will give you the designer for Windows Forms, which makes it easier to work with the frontend.

Alternatively, you can also just build the solution with [.NET CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/), which is included in the [.NET SDK](https://dotnet.microsoft.com/download/dotnet/5.0).

## Support
Please open an issue on this repo for any bug reports and suggestions you may have. If you wish to talk to us directly, visit the official TotalImage Discord server:

[![Visit our Discord server](https://discordapp.com/api/guilds/822572019304103937/embed.png)](https://discord.gg/htph4vsuzB)
