using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalImage.DiskGeometries
{
    /// <summary>
    /// Class that represents a floppy disk geometry/format.
    /// </summary>
    public class FloppyGeometry
    {
        /// /// <summary>
        /// Number of sides, either one or two.
        /// </summary>
        public byte Sides { get; }
        /// <summary>
        /// Number of tracks per side.
        /// </summary>
        public byte Tracks { get; }
        /// <summary>
        /// Number of sectors per track.
        /// </summary>
        public byte SPT { get; }
        /// <summary>
        /// Number of bytes per sector. Left-shift 128 by this number to get the actual number of bytes.
        /// </summary>
        public byte BPS { get; }
        /// <summary>
        /// Media descriptor byte.
        /// </summary>
        public byte MediaDescriptor { get; }
        /// <summary>
        /// Number of sectors per cluster.
        /// </summary>
        public byte SPC { get; }
        /// <summary>
        /// Number of file allocation tables, usually two.
        /// </summary>
        public byte NoOfFATs { get; }
        /// <summary>
        /// Number of sectors per FAT.
        /// </summary>
        public byte SPF { get; }
        /// <summary>
        /// Number of root directory entries.
        /// </summary>
        public ushort RootDirectoryEntries { get; }
        /// <summary>
        /// Number of reserved sectors, usually one.
        /// </summary>
        public byte ReservedSectors { get; }

        /// <summary>
        /// Friendly names for known floppy disk geometries.
        /// </summary>
        public enum FriendlyName
        {
            /// <summary>
            /// A standard single-sided 5.25" NorthStar format with 87.5 KiB formatted capacity.
            /// </summary>
            [Display(Name = "87.5 KiB")]
            SingleDensity87k,
            /// <summary>
            /// A standard single-sided 5.25" PC-compatible format with 160 KiB formatted capacity.
            /// </summary>
            [Display(Name = "160 KiB")]
            DoubleDensity160k,
            /// <summary>
            /// A standard single-sided 5.25" PC-compatible format with 180 KiB formatted capacity.
            /// </summary>
            [Display(Name = "180 KiB")]
            DoubleDensity180k,
            /// <summary>
            /// A standard single-sided 8" format with 250.25 KiB formatted capacity.
            /// </summary>
            [Display(Name = "250.25 KiB")]
            SingleDensity,
            /// <summary>
            /// A standard double-sided 5.25" PC-compatible format with 320 KiB formatted capacity.
            /// </summary>
            [Display(Name = "320 KiB")]
            DoubleDensity320k,
            /// <summary>
            /// A standard double-sided 5.25" PC-compatible format with 360 KiB formatted capacity.
            /// </summary>
            [Display(Name = "360 KiB")]
            DoubleDensity360k,
            /// <summary>
            /// A proprietary double-sided 5.25" format with 400 KiB formatted capacity, used by Triumph-Adler Alphatronic PC-16.
            /// </summary>
            [Display(Name = "400 KiB (Alphatronic PC-16)")]
            AlphatronicPC16,
            /// <summary>
            /// A proprietary single-sided 5.25" format with 612 KiB formatted capacity, used by Victor 9000/Sirius 1.
            /// </summary>
            [Display(Name = "612 KiB (Victor 9000)")]
            Victor9000SS,
            /// <summary>
            /// A standard single-sided 8" format with 616 KiB formatted capacity.
            /// </summary>
            [Display(Name = "616 KiB")]
            HighDensity616k,
            /// <summary>
            /// A rare double-sided 5.25" or 3.5" format with 640 KiB formatted capacity.
            /// </summary>
            [Display(Name = "640 KiB")]
            QuadDensity,
            /// <summary>
            /// A proprietary double-sided 5.25" format with 720 KiB formatted capacity, used by Tandy 2000.
            /// </summary>
            [Display(Name = "720 KiB (Tandy 2000)")]
            Tandy2000,
            /// <summary>
            /// A proprietary double-sided 5.25" format with 720 KiB formatted capacity, used by Siemens PC-D.
            /// </summary>
            [Display(Name = "720 KiB (Siemens PC-D)")]
            SiemensPCD,
            /// <summary>
            /// A standard double-sided 3.5" PC-compatible format with 720 KiB formatted capacity.
            /// </summary>
            [Display(Name = "720 KiB")]
            DoubleDensity720k,
            /// <summary>
            /// A proprietary double-sided 5.25" format with 800 KiB formatted capacity, used by the Eagle 1600.
            /// </summary>
            [Display(Name = "800 KiB (Eagle 1600)")]
            Eagle1600,
            /// <summary>
            /// A proprietary double-sided 5.25" format with 800 KiB formatted capacity, used by the Acorn BBC Master 512.
            /// </summary>
            [Display(Name = "800 KiB (BBC Master 512)")]
            Acorn800k,
            /// <summary>
            /// A proprietary double-sided 5.25" format with just over 1195 KiB formatted capacity, used by Victor 9000/Sirius 1.
            /// </summary>
            [Display(Name = "1195 KiB (Victor 9000)")]
            Victor9000DS,
            /// <summary>
            /// A standard double-sided 5.25" PC-compatible format with 1200 KiB formatted capacity.
            /// </summary>
            [Display(Name = "1200 KiB")]
            HighDensity1200k,
            /// <summary>
            /// A standard double-sided 8" or 5.25" or 3.5" format with 1232 KiB formatted capacity, mostly used by NEC PC-98.
            /// </summary>
            [Display(Name = "1232 KiB")]
            HighDensity1232k,
            /// <summary>
            /// A standard double-sided 3.5" PC-compatible format with 1440 KiB formatted capacity.
            /// </summary>
            [Display(Name = "1440 KiB")]
            HighDensity1440k,
            /// <summary>
            /// A proprietary double-sided 3.5" format with 1680 KiB formatted capacity and 1024 byte clusters, developed by Microsoft.
            /// </summary>
            [Display(Name = "1680 KiB (DMF 1024)")]
            DMF1024,
            /// <summary>
            /// A proprietary double-sided 3.5" format with 1680 KiB formatted capacity and 2048 byte clusters, developed by Microsoft.
            /// </summary>
            [Display(Name = "1680 KiB (DMF 2048)")]
            DMF2048,
            /// <summary>
            /// A rare double-sided 3.5" format with 1722 KiB formatted capacity.
            /// </summary>
            [Display(Name = "1722 KiB")]
            HighDensity1722k,
            /// <summary>
            /// A standard double-sided 3.5" PC-compatible format with 2880 KiB formatted capacity.
            /// </summary>
            [Display(Name = "2880 KiB")]
            ExtendedDensity,
            /// <summary>
            /// This value signals the use of custom parameters not included in KnownGeometries.
            /// </summary>
            [Display(Name = "Custom...")]
            Custom
        }

        /// <summary>
        /// Creates a new floppy geometry with provided parameters.
        /// </summary>
        /// <param name="sides">Number of sides, either 1 or 2.</param>
        /// <param name="tracks">Number of tracks per side.</param>
        /// <param name="spt">Number of sectors per track.</param>
        /// <param name="bps">Number of bytes per sector. Left-shift 128 by this number to get the actual value.</param>
        /// <param name="mediadesc">Media descriptor byte.</param>
        /// <param name="spc">Number of sectors per cluster.</param>
        /// <param name="noofFATs">Number of file allocation tables.</param>
        /// <param name="spf">Number of sectors per FAT.</param>
        /// <param name="rootdirentries">Maximum number of root directory entries.</param>
        /// <param name="reservedsectors">Number of reserved sectors.</param>
        public FloppyGeometry(byte sides, byte tracks, byte spt, byte bps, byte mediadesc, byte spc, byte noofFATs, byte spf,
            ushort rootdirentries, byte reservedsectors)
        {
            Sides = sides;
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

        /// <summary>
        /// List of known floppy disk geometries.
        /// </summary>
        public static readonly IReadOnlyDictionary<FriendlyName, FloppyGeometry> KnownGeometries = new Dictionary<FriendlyName, FloppyGeometry>()
        {
            { FriendlyName.SingleDensity87k,  new FloppyGeometry(1, 35, 10, 1, 0xFE, 1, 2,  2,  64, 1) },
            { FriendlyName.DoubleDensity160k, new FloppyGeometry(1, 40,  8, 2, 0xFE, 1, 2,  1,  64, 1) },
            { FriendlyName.DoubleDensity180k, new FloppyGeometry(1, 40,  9, 2, 0xFC, 1, 2,  1,  64, 1) },
            { FriendlyName.SingleDensity,     new FloppyGeometry(1, 77, 26, 0, 0xFE, 4, 2,  6,  68, 1) },
            { FriendlyName.DoubleDensity320k, new FloppyGeometry(2, 40,  8, 2, 0xFF, 2, 2,  1, 112, 1) },
            { FriendlyName.DoubleDensity360k, new FloppyGeometry(2, 40,  9, 2, 0xFD, 2, 2,  2, 112, 1) },
            { FriendlyName.AlphatronicPC16,   new FloppyGeometry(2, 40,  5, 3, 0xFF, 2, 2,  1, 128, 1) },
            { FriendlyName.Victor9000SS,      new FloppyGeometry(1, 80,  0, 2, 0xF8, 4, 2,  1, 128, 1) },
            { FriendlyName.QuadDensity,       new FloppyGeometry(2, 80,  8, 2, 0xFB, 2, 2,  2, 112, 1) },
            { FriendlyName.Tandy2000,         new FloppyGeometry(2, 80,  9, 2, 0xFD, 4, 2,  2, 112, 1) },
            { FriendlyName.SiemensPCD,        new FloppyGeometry(2, 80,  9, 2, 0xF9, 4, 2,  2, 144, 1) },
            { FriendlyName.DoubleDensity720k, new FloppyGeometry(2, 80,  9, 2, 0xF9, 2, 2,  3, 112, 1) },
            { FriendlyName.Eagle1600,         new FloppyGeometry(2, 80,  5, 3, 0xFD, 1, 2,  2, 320, 1) },
            { FriendlyName.Acorn800k,         new FloppyGeometry(2, 80,  5, 3, 0xFD, 1, 1,  2, 192, 0) },
            { FriendlyName.Victor9000DS,      new FloppyGeometry(2, 80,  0, 2, 0xF8, 4, 2,  2, 128, 1) },
            { FriendlyName.HighDensity1200k,  new FloppyGeometry(2, 80, 15, 2, 0xF9, 1, 2,  7, 224, 1) },
            { FriendlyName.HighDensity616k,   new FloppyGeometry(1, 77,  8, 3, 0xFE, 1, 2,  1,  96, 1) },
            { FriendlyName.HighDensity1232k,  new FloppyGeometry(2, 77,  8, 3, 0xFE, 1, 2,  2, 192, 1) },
            { FriendlyName.HighDensity1440k,  new FloppyGeometry(2, 80, 18, 2, 0xF0, 1, 2,  9, 224, 1) },
            { FriendlyName.DMF1024,           new FloppyGeometry(2, 80, 21, 2, 0xF0, 2, 2,  5,  16, 1) },
            { FriendlyName.DMF2048,           new FloppyGeometry(2, 80, 21, 2, 0xF0, 4, 2,  3,  16, 1) },
            { FriendlyName.HighDensity1722k,  new FloppyGeometry(2, 82, 21, 2, 0xF0, 1, 2, 10, 224, 1) },
            { FriendlyName.ExtendedDensity,   new FloppyGeometry(2, 80, 36, 2, 0xF0, 2, 2,  9, 240, 1) },
        }.ToFrozenDictionary();

        /// <inheritdoc />
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();

            hash.Add(BPS);
            hash.Add(MediaDescriptor);
            hash.Add(NoOfFATs);
            hash.Add(ReservedSectors);
            hash.Add(RootDirectoryEntries);
            hash.Add(Sides);
            hash.Add(SPC);
            hash.Add(SPF);
            hash.Add(SPT);
            hash.Add(Tracks);

            return hash.ToHashCode();
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                FloppyGeometry fg = (FloppyGeometry)obj;
                return fg.BPS == BPS && fg.MediaDescriptor == MediaDescriptor && fg.NoOfFATs == NoOfFATs && fg.ReservedSectors == ReservedSectors
                    && fg.RootDirectoryEntries == RootDirectoryEntries && fg.Sides == Sides && fg.SPC == SPC && fg.SPF == SPF && fg.SPT == SPT
                    && fg.Tracks == Tracks;
            }
        }
    }
}
