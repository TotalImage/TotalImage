# TotalImage
**TotalImage** is a free and open source disk image editor licensed under the MIT license. For more information, see the `LICENSE` file. It is written in C# and uses Windows Forms for the UI.

### Downloads
Currently, the project is still in early stages of development (alpha-level), and as such is not quite ready for everyday use yet. We will provide official builds for major pre-release milestones and stable releases only, in the mean time you can build it yourself or ask us to provide binaries for you (see the *Development* and *Support* sections below).

### System requirements
* [.NET 10.0](https://dotnet.microsoft.com/download/dotnet/10.0) Desktop Runtime
* Windows 10 or 11 (latest version recommended)

Please see [this page](https://github.com/TotalImage/TotalImage/blob/master/Docs/oldwin.md) if you're using Windows 7 or 8.1.

## Current status
See the [Status page](https://github.com/TotalImage/TotalImage/blob/master/Docs/status.md) for details about currently implemented features.

## Planned features
Below is a non-exhaustive list of things we plan to support. Please note that this list may change over time. We're open to adding support for more stuff in the future, but we have to start with the basics. For currently supported features, see the linked status page in the section above.

### Media types
* Floppy disks
* Hard disks
* Optical discs
* Superfloppies

### Disk image containers
* Raw sector images (.IMG, .IMA,  etc.)
* TeleDisk (.TD0)
* ImageDisk (.IMD)
* 86Box 86F (.86F)
* Virtual Hard Disk (.VHD)
* ISO (.ISO)
* And more...

### Disk geometries (formats)
* Common PC-compatible formats (160/180/320/360/720/1200/1440/1680/2880k)
* 8" formats (such as 1232k) and others with sector sizes other than 512 bytes
* IBM XDF
* DEC RX50 400k
* Acorn 640k/800k
* And more...

### File systems
* FAT12, FAT16, FAT32
* HPFS, NTFS
* High Sierra, ISO9660, UDF
* IMGFS
* And more...

### Partitioning schemes
* Various MBR variants
* GPT
* PC-98 partition table
* And more...

## Development
To work on TotalImage, we suggest you install [Microsoft Visual Studio](https://visualstudio.microsoft.com/vs/). This includes the designer for Windows Forms, which makes it easier to work with the frontend.

Alternatively, you can also build the solution with [.NET CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/), which is included in the [.NET SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

You will also need to install [git](https://git-scm.com/) in both cases and make sure the `git.exe` executable is included in the `PATH` environment variable (required for displaying git commit hash in the About dialog).

## Support
Please open an issue on this repo for any bug reports and suggestions you may have. If you wish to talk to us directly, visit the official TotalImage Discord server:

[![Visit our Discord server](https://discordapp.com/api/guilds/822572019304103937/embed.png)](https://discord.gg/htph4vsuzB)
