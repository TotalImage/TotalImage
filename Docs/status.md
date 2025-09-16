# Status

This is the list of all currently supported media types, image containers, partitioning schemes, file systems and disk formats (geometries).

* Read = existing data can be read
* Write = existing data can be modified
* Create = new images/partition tables/file systems can be created

## Media types
| Name | Read | Write | Create |
| --- | --- | --- | --- |
| Floppy disks | ✔ Yes | ❌ No | ✔ Yes |
| CD-ROM images |  ✔ Yes | ❌ No | ❌ No |
| Hard disks | ✔ Yes | ❌ No | ❌ No |

Read and Create functionality is limited to supported features below.

## Image containers
| Name | Common file extensions | Read | Write | Create |
| --- | --- | --- | --- | --- |
| Raw sector image | IMG, IMA, FLP, VFD, DSK, ISO, etc.<sup>1</sup> | ✔ Yes | ❌ No | ✔ Yes<sup>2</sup> |
| Microsoft VHD | VHD | ⚠ Partial<sup>3</sup> | ❌ No | ❌ No |
| T98-Next HD | NHD | ✔ Yes | ❌ No | ❌ No |
| WinImage compressed image | IMZ | ✔ Yes | ❌ No | ❌ No |
| Anex86 disk image | FDI, HDI | ✔ Yes | ❌ No | ❌ No |
| PCjs.org JSON | JSON | ✔ Yes | ❌ No | ❌ No |

Read and Create functionality is limited to supported features below.

<sup>1</sup> File extensions for raw sector images can be arbitrary and may also overlap with common extensions used for other non-raw sector containers (e.g. IMG or DSK).

<sup>2</sup> ISO-9660 images are currently treated as a raw sector container, but creating new images is not supported.

<sup>3</sup> Differencing VHDs are not supported yet.

## Disk geometries
| Name | Read | Write | Create |
| --- | --- | --- | --- |
| Common PC-compatible formats with a BPB | ✔ Yes | ❌ No | ✔ Yes |
| Common PC-compatible formats without a BPB | ✔ Yes | ❌ No | ✔ Yes |
| Microsoft DMF (1024 and 2048-byte clusters) | ✔ Yes | ❌ No | ✔ Yes |
| 8" 250k/1232k | ✔ Yes | ❌ No | ✔ Yes |
| Siemens PC-D 720k | ✔ Yes | ❌ No | ✔ Yes |
| Alphatronic PC-16 400k | ✔ Yes | ❌ No | ✔ Yes |
| Eagle 1600 800k | ✔ Yes | ❌ No | ✔ Yes |
| Tandy 2000 720k | ✔ Yes | ❌ No | ✔ Yes |
| Acorn 800k | ✔ Yes | ❌ No | ⚠ Partial<sup>1</sup> |
| Apricot 315k/720k | ✔ Yes<sup>2</sup> | ❌ No | ❌ No |
| Victor 9000 612k/1196k | ✔ Yes | ❌ No | ❌ No |
| Hard disks with 512-byte sectors | ✔ Yes | ❌ No | ❌ No |

These disk formats are only supported when formatted with a supported file system (depends on media type) and contained in a supported container.

<sup>1</sup> Acorn 800k Create functionality currently incorrectly writes a broken bootsector.

<sup>2</sup> Apricot disks are only supported when contained in a raw sector image; ApriDisk (.DSK) container is not supported yet.

## Partitioning schemes
| Name | Read | Write | Create |
| --- | --- | --- | --- |
| Master Boot Record | ⚠ Partial<sup>1</sup> | ❌ No | ❌ No |
| GUID Partition Table | ✔ Yes | ❌ No | ❌ No |

Only partitions formatted with supported file systems are currently supported (see below).

<sup>1</sup> Only primary partitions are currently supported.

## File systems and extensions
| Name | Read | Write | Create |
| --- | --- | --- | --- |
| FAT12 | ✔ Yes | ❌ No | ✔ Yes |
| FAT16 | ✔ Yes | ❌ No | ❌ No |
| FAT32 | ✔ Yes | ❌ No | ❌ No |
| VFAT extensions | ✔ Yes | ❌ No | ❌ No |
| exFAT | ✔ Yes | ❌ No | ❌ No |
| ISO-9660 | ✔ Yes | ❌ No | ❌ No |
| High Sierra Format | ✔ Yes | ❌ No | ❌ No |
| Joliet extensions for ISO-9660 | ✔ Yes | ❌ No | ❌ No |
