# Status

**PLEASE NOTE: the Alpha 1 preview release only contains read functionality. Files can't be modified or created. Write functionality will be added in a future release.**

This is the list of all currently supported media types, image containers, partitioning schemes, file systems and disk formats (geometries).

* Read = existing data can be read
* Write = existing data can be modified
* Create = new data can be created

## Media types
| Name | Read | Write | Create |
| --- | --- | --- | --- |
| Floppy disks | ✔ Yes | ❌ No | ❌ No |
| CD-ROM images |  ✔ Yes | ❌ No | ❌ No |
| Hard disks | ✔ Yes | ❌ No | ❌ No |

### Notes
* Read functionality is limited to supported features below.

## Image containers
| Name | Common file extensions | Read | Write | Create |
| --- | --- | --- | --- | --- |
| Raw sector image | IMG, ISO, IMA, FLP, VFD, DSK | ✔ Yes | ❌ No | ❌ No |
| Microsoft VHD | VHD | ⚠ Partial | ❌ No | ❌ No |
| T98-Next HD | NHD | ✔ Yes | ❌ No | ❌ No |
| WinImage compressed image | IMZ | ✔ Yes | ❌ No | ❌ No |
| Anex86 disk image | FDI, HDI | ✔ Yes | ❌ No | ❌ No |

### Notes
* Read functionality is limited to supported features below.
* Only fixed-size and dynamic VHD types are currently supported.

## Disk geometries
| Name | Read | Write | Create |
| --- | --- | --- | --- |
| Common PC-compatible formats with a BPB | ✔ Yes | ❌ No | ❌ No|
| Common PC-compatible formats without a BPB | ✔ Yes | ❌ No | ❌ No |
| Microsoft DMF (1024 and 2048-byte clusters) | ✔ Yes | ❌ No | ❌ No |
| 8" 250k/1232k | ✔ Yes | ❌ No | ❌ No |
| Siemens PC-D 720k | ✔ Yes | ❌ No | ❌ No |
| Alphatronic PC-16 400k | ✔ Yes | ❌ No | ❌ No |
| Eagle 1600 800k | ✔ Yes | ❌ No | ❌ No |
| Tandy 2000 720k | ✔ Yes | ❌ No | ❌ No |
| Acorn 800k | ✔ Yes | ❌ No | ❌ No |
| Apricot 315k/720k | ✔ Yes | ❌ No | ❌ No |
| Hard disks with 512-byte sectors | ✔ Yes | ❌ No | ❌ No |
| Optical discs of any volume size | ✔ Yes | ❌ No | ❌ No |

### Notes
* These disk geometries are only supported when formatted with a supported file system (depends on media type) and contained in a supported container.

## Partitioning schemes
| Name | Read | Write | Create |
| --- | --- | --- | --- |
| Master Boot Record | ✔ Yes | ❌ No | ❌ No |
| GUID Partition Table | ✔ Yes | ❌ No | ❌ No |

### Notes
* Only FAT12, FAT16 and FAT32 primary partitions are currently supported (see below).
* Other common partition types are only identified in the partition selection dialog.

## File systems and extensions
| Name | Read | Write | Create |
| --- | --- | --- | --- |
| FAT12 | ✔ Yes | ❌ No | ❌ No |
| FAT16 | ✔ Yes | ❌ No | ❌ No |
| FAT32 | ✔ Yes | ❌ No | ❌ No |
| FAT Long File Names | ✔ Yes | ❌ No | ❌ No |
| ISO-9660 | ✔ Yes | ❌ No | ❌ No |
| High Sierra Format | ✔ Yes | ❌ No | ❌ No |
| Joliet | ✔ Yes | ❌ No | ❌ No |

### Notes
* Joliet and FAT LFN are always used (displayed) when present.
