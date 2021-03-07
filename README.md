# TotalImage
**TotalImage** is a free and open source disk image editor licensed under the MIT license. For more information, see the `LICENSE` file. It is written in C# and uses Windows Forms for the UI.

## Features
The goal of TotalImage is to be a better alternative to some of the more widely used disk image tools of today. Our goal is to create a tool that:
* is completely free and open-source
* is updated and improved regularly, as much as personal circumstances of the developers permit
* supports a wide range of disk image containers, physical disk formats, file systems and partitioning schemes
* handles and reports exceptions properly
* offers other related advanced features that might come in handy

Because TotalImage is still under development, not all of the above can be achieved immediately. Some of the things we wish to support eventually are:

### Media types
* Floppy disks
* Hard disks
* Optical disks

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

Please note that this list is not exhaustive nor definitive. We're open to adding support for more stuff in the future, but we have to start with the basics.

## System requirements
* Windows 7 Service Pack 1 or later
* [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48) or [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0) runtime

Support for other operating systems will be considered within the limitations of .NET.

## Development
To develop TotalImage, we recommend you install Microsoft Visual Studio 2019 (free Community Edition can be downloaded [here](https://visualstudio.microsoft.com/vs/)) and at least one of the target .NET runtimes (see above).

## Support
Please open an issue on this repo for any bug reports and suggestions you may have. If you wish to talk to us directly, visit the official TotalImage Discord channel (currently part of the 86Box server):

[![Visit our Discord server](https://discordapp.com/api/guilds/262614059009048590/embed.png)](https://discord.gg/WCDkTFwPQD)
