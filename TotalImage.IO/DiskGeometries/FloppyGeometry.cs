using System.Collections.Generic;
using System.Collections.Immutable;

namespace TotalImage.DiskGeometries
{
    public class FloppyGeometry
    {
        /// <summary>
        /// What this?
        /// </summary>
        public byte Hole { get; }
        /// <summary>
        /// Number of sides, either one or two.
        /// </summary>
        public byte Sides { get; }
        /// <summary>
        /// The data rate.
        /// </summary>
        public ushort DataRate { get; }
        /// <summary>
        /// Encoding type, either FM or MFM.
        /// </summary>
        public ushort Encoding { get; }
        /// <summary>
        /// Revolutions per minute.
        /// </summary>
        public ushort RPM { get; }
        /// <summary>
        /// Number of tracks per side.
        /// </summary>
        public ushort Tracks { get; }
        /// <summary>
        /// Number of sectors per track.
        /// </summary>
        public ushort SPT { get; }
        /// <summary>
        /// Number of bytes per sector.
        /// </summary>
        public ushort BPS { get; }
        /// <summary>
        /// Media descriptor byte.
        /// </summary>
        public ushort MediaDescriptor { get; }
        /// <summary>
        /// Number of sectors per cluster.
        /// </summary>
        public ushort SPC { get; }
        /// <summary>
        /// Number of file allocation tables, usually two.
        /// </summary>
        public ushort NoOfFATs { get; }
        /// <summary>
        /// Number of sectors per FAT.
        /// </summary>
        public ushort SPF { get; }
        /// <summary>
        /// Number of root directory entries.
        /// </summary>
        public ushort RootDirectoryEntries { get; }
        /// <summary>
        /// Number of reserved sectors, usually one.
        /// </summary>
        public ushort ReservedSectors { get; }

        public enum FriendlyName
        {
            /// <summary>
            /// A standard single-sided 5.25" PC-compatible format with 160 KiB formatted capacity.
            /// </summary>
            DoubleDensity160k,
            /// <summary>
            /// A standard single-sided 5.25" PC-compatible format with 180 KiB formatted capacity.
            /// </summary>
            DoubleDensity180k,
            /// <summary>
            /// A standard single-sided 8" format with 250 KiB formatted capacity.
            /// </summary>
            SingleDensity,
            /// <summary>
            /// A standard double-sided 5.25" PC-compatible format with 320 KiB formatted capacity.
            /// </summary>
            DoubleDensity320k,
            /// <summary>
            /// A standard double-sided 5.25" PC-compatible format with 360 KiB formatted capacity.
            /// </summary>
            DoubleDensity360k,
            /// <summary>
            /// A proprietary double-sided 5.25" format with 400 KiB formatted capacity, used by Triumph-Adler Alphatronic PC-16.
            /// </summary>
            AlphatronicPC16,
            /// <summary>
            /// A rare double-sided 5.25" or 3.5" format with 640 KiB formatted capacity.
            /// </summary>
            QuadDensity,
            /// <summary>
            /// A proprietary double-sided 5.25" format with 720 KiB formatted capacity, used by Tandy 2000.
            /// </summary>
            Tandy2000,
            /// <summary>
            /// A proprietary double-sided 5.25" format with 720 KiB formatted capacity, used by Siemens PC-D.
            /// </summary>
            SiemensPCD,
            /// <summary>
            /// A standard double-sided 3.5" PC-compatible format with 720 KiB formatted capacity.
            /// </summary>
            DoubleDensity720k,
            /// <summary>
            /// A proprietary double-sided 5.25" format with 800 KiB formatted capacity, used by the Eagle 1600.
            /// </summary>
            Eagle1600,
            /// <summary>
            /// A standard double-sided 5.25" PC-compatible format with 1200 KiB formatted capacity.
            /// </summary>
            HighDensity1200k,
            /// <summary>
            /// A standard double-sided 8" or 5.25" or 3.5" format with 1232 KiB formatted capacity, mostly used by NEC PC-98.
            /// </summary>
            HighDensity1232k,
            /// <summary>
            /// A standard double-sided 3.5" PC-compatible format with 1440 KiB formatted capacity.
            /// </summary>
            HighDensity1440k,
            /// <summary>
            /// A proprietary double-sided 3.5" format with 1680 KiB formatted capacity and 1024 byte clusters, developed by Microsoft.
            /// </summary>
            DMF1024,
            /// <summary>
            /// A proprietary double-sided 3.5" format with 1680 KiB formatted capacity and 2048 byte clusters, developed by Microsoft.
            /// </summary>
            DMF2048,
            /// <summary>
            /// A rare double-sided 3.5" format with 1722 KiB formatted capacity.
            /// </summary>
            HighDensity1722k,
            /// <summary>
            /// A standard double-sided 3.5" PC-compatible format with 2880 KiB formatted capacity.
            /// </summary>
            ExtendedDensity
        }

        public FloppyGeometry(byte hole, byte sides, ushort datarate, ushort encoding, ushort rpm, ushort tracks, ushort spt, ushort bps,
            ushort mediadesc, ushort spc, ushort noofFATs, ushort spf, ushort rootdirentries, ushort reservedsectors)
        {
            Hole = hole;
            Sides = sides;
            DataRate = datarate;
            Encoding = encoding;
            RPM = rpm;
            Tracks = tracks;
            SPT = spt;
            BPS = bps;
            MediaDescriptor = mediadesc;
            SPC = spc;
            NoOfFATs = noofFATs;
            SPF = spf;
            RootDirectoryEntries = rootdirentries;
            ReservedSectors = reservedsectors;
        }

        //Key is the friendly name for a geometry/format, value is the FloppyGeometry object containing actual parameters
        public static readonly IReadOnlyDictionary<FriendlyName, FloppyGeometry> KnownGeometries = ImmutableDictionary.CreateRange(new Dictionary<FriendlyName, FloppyGeometry>()
        {
            { FriendlyName.DoubleDensity160k, new FloppyGeometry(0, 1, 2, 1, 0, 40,  8, 2, 0xFE, 2, 2,  1,  64, 1) },
            { FriendlyName.DoubleDensity180k, new FloppyGeometry(0, 1, 2, 1, 0, 40,  9, 2, 0xFC, 2, 2,  1,  64, 1) },
            { FriendlyName.SingleDensity, new FloppyGeometry(0, 1, 2, 1, 0, 77, 26, 0, 0xFE, 4, 2,  6,  68, 1) },
            { FriendlyName.DoubleDensity320k, new FloppyGeometry(0, 2, 2, 1, 0, 40,  8, 2, 0xFF, 2, 2,  1, 112, 1) },
            { FriendlyName.DoubleDensity360k, new FloppyGeometry(0, 2, 2, 1, 0, 40,  9, 2, 0xFD, 2, 2,  2, 112, 1) },
            { FriendlyName.AlphatronicPC16, new FloppyGeometry(0, 2, 2, 1, 0, 40,  5, 3, 0xFF, 2, 2,  1, 128, 1) },
            { FriendlyName.QuadDensity, new FloppyGeometry(0, 2, 2, 1, 0, 80,  8, 2, 0xFB, 2, 2,  2, 112, 1) },
            { FriendlyName.Tandy2000, new FloppyGeometry(0, 2, 2, 1, 0, 80,  9, 2, 0xFD, 4, 2,  2, 112, 1) },
            { FriendlyName.SiemensPCD, new FloppyGeometry(0, 2, 2, 1, 0, 80,  9, 2, 0xF9, 4, 2,  2, 144, 1) },
            { FriendlyName.DoubleDensity720k, new FloppyGeometry(0, 2, 2, 1, 0, 80,  9, 2, 0xF9, 2, 2,  3, 112, 1) },
            { FriendlyName.Eagle1600, new FloppyGeometry(0, 2, 2, 1, 0, 80,  5, 3, 0xFD, 1, 2,  2, 320, 1) },
            { FriendlyName.HighDensity1200k, new FloppyGeometry(1, 2, 0, 1, 1, 80, 15, 2, 0xF9, 1, 2,  7, 224, 1) },
            { FriendlyName.HighDensity1232k, new FloppyGeometry(1, 2, 0, 1, 1, 77,  8, 3, 0xFE, 1, 2,  2, 192, 1) },
            { FriendlyName.HighDensity1440k, new FloppyGeometry(1, 2, 0, 1, 0, 80, 18, 2, 0xF0, 1, 2,  9, 224, 1) },
            { FriendlyName.DMF1024, new FloppyGeometry(1, 2, 0, 1, 0, 80, 21, 2, 0xF0, 2, 2,  5,  16, 1) },
            { FriendlyName.DMF2048, new FloppyGeometry(1, 2, 0, 1, 0, 80, 21, 2, 0xF0, 4, 2,  3,  16, 1) },
            { FriendlyName.HighDensity1722k, new FloppyGeometry(1, 2, 0, 1, 0, 82, 21, 2, 0xF0, 1, 2, 10, 224, 1) },
            { FriendlyName.ExtendedDensity, new FloppyGeometry(2, 2, 3, 1, 0, 80, 36, 2, 0xF0, 2, 2,  9, 240, 1) },
        });
    }
}