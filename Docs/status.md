# Status

This is the list of all currently supported media types, image containers, partitioning schemes, file systems and disk formats (geometries).

* Read = existing data can be read
* Write = existing data can be modified
* Create = new data can be created

## Media types
| Name | Read | Write | Create |
| --- | --- | --- | --- |
| Floppy disks | ✔ Yes | ❌ No | ✔ Yes |
| Hard disks | ✔ Yes | ❌ No | ❌ No |

Read and Create functionality is limited to supported features below.

## Image containers
| Name | Common file extensions | Read | Write | Create |
| --- | --- | --- | --- | --- |
| Raw sector image | IMG, IMA, FLP, VFD, DSK | ✔ Yes | ❌ No | ✔ Yes |
| Microsoft VHD | VHD | ⚠ Partial | ❌ No | ❌ No |

Read and Create functionality is limited to supported features below.

Only fixed-size VHDs are currently supported.

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
| Acorn 800k | ✔ Yes | ❌ No | ⚠ Partial |
| Apricot 315k/720k | ✔ Yes | ❌ No | ❌ No |
| Hard disks with 512-byte sectors | ✔ Yes | ❌ No | ❌ No |

These disk formats are only supported when formatted with FAT12 or FAT16 and contained in a supported container (see above).

Acorn 800k Create functionality currently incorrectly writes a broken bootsector.

## Partitioning schemes
| Name | Read | Write | Create |
| --- | --- | --- | --- |
| Master Boot Record | ✔ Yes | ❌ No | ❌ No |
| GUID Partition Table | ✔ Yes | ❌ No | ❌ No |

Only FAT12 and FAT16 primary partitions are currently supported.

## File systems and extensions
| Name | Read | Write | Create |
| --- | --- | --- | --- |
| FAT12 | ⚠ Partial | ❌ No | ✔ Yes |
| FAT16 | ⚠ Partial | ❌ No | ❌ No |

Read functionality is currently limited to listing directories, displaying attributes and rudimentary file extraction.

Long file names (LFN) are supported on all supported FAT variants.
