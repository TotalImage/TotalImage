# Status

This is the list of all currently supported media types, image containers, partitioning schemes, file systems and disk formats (geometries).

* Read = existing data can be read
* Write = existing data can be modified
* Create = new data can be created

## Media types
| Name | Read | Write | Create |
| --- | --- | --- | --- |
| Floppy disk | ✔ Yes | ❌ No | ✔ Yes |

Read and Create functionality is limited to supported image containers and disk formats (see below).

## Image containers
| Name | Common file extensions | Read | Write | Create |
| --- | --- | --- | --- | --- |
| Raw sector image | IMG, IMA, FLP, VFD, DSK | ✔ Yes | ❌ No | ✔ Yes |

Read and Create functionality is limited to supported disk formats (see below).

## Disk formats
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

These disk formats are only supported when formatted with FAT12 and contained in a supported container (see above).

Acorn 800k Create functionality currently incorrectly writes a broken bootsector even though it shouldn't.

## Partitioning schemes
Since floppy disks are not partitioned (the entire capacity is allocated to a single partition and there is no partition table), no partitioning scheme is currently implemented.

## File systems and extensions
| Name | Read | Write | Create |
| --- | --- | --- | --- |
| FAT12 | ⚠ Partial | ❌ No | ✔ Yes |

Read functionality is currently limited to listing directories and displaying attributes.
