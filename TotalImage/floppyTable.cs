namespace TotalImage
{
    /*
     * List of standard floppy disk parameters for the "New image" dialog
     * This could probably be improved significantly
     */
    class floppyTable
    {
		//Hole, sides, data rate, encoding, RPM, tracks, SPT, BPS, media desc, SPC, no. of FATs, SPF, root dir entries, reserved sectors
		public static byte[][] fdt = new byte[][]{         
            new byte[]{ 0,  1, 2, 1, 0,  40,  8, 2, 0xfe, 2, 2,   1,  64,  1 }, /*  160 KiB */
			new byte[]{ 0,  1, 2, 1, 0,  40,  9, 2, 0xfc, 2, 2,   1,  64,  1 }, /*  180 KiB */
			new byte[]{ 0,  2, 2, 1, 0,  40,  8, 2, 0xff, 2, 2,   1, 112,  1 }, /*  320 KiB (5.25") */
			new byte[]{ 0,  1, 2, 1, 0,  40,  8, 2, 0xff, 2, 2,   1, 112,  1 }, /*  320 KiB (3.5") - UNRELIABLE VALUES!*/
			new byte[]{ 0,  2, 2, 1, 0,  40,  9, 2, 0xfd, 2, 2,   2, 112,  1 }, /*  360 KiB (5.25") */
			new byte[]{ 0,  1, 2, 1, 0,  80,  9, 2, 0xfd, 2, 2,   2, 112,  1 }, /*  360 KiB (3.5") - UNRELIABLE VALUES!*/
			new byte[]{ 0,  1, 2, 1, 0,  80, 10, 2, 0xfa, 1, 2,   3,  96, 20 }, /*  400 KiB (DEC RX50) */
			new byte[]{ 0,  2, 2, 1, 0,  80,  8, 2, 0xfb, 2, 2,   2, 112,  1 }, /*  640 KiB */
			new byte[]{ 0,  2, 2, 1, 0,  80,  9, 2, 0xed, 4, 2,   2, 112,  1 }, /*  720 KiB (Tandy 2000) */
			new byte[]{ 0,  2, 2, 1, 0,  80,  9, 2, 0xf9, 2, 2,   3, 112,  1 }, /*  720 KiB (3.5") */
			new byte[]{ 0,  2, 2, 1, 0,  80, 10, 2, 0xf9, 2, 2,   3, 112,  1 }, /*  800 KiB - UNRELIABLE VALUES!*/
			new byte[]{ 1,  2, 0, 1, 1,  80, 15, 2, 0xf9, 1, 2,   7, 224,  1 }, /* 1200 KiB */
			new byte[]{ 1,  2, 0, 1, 1,  77,  8, 3, 0xfe, 1, 2,   2, 192,  1 }, /* 1232 KiB (NEC PC-98) */
			new byte[]{ 1,  2, 0, 1, 0,  80, 18, 2, 0xf0, 1, 2,   9, 224,  1 }, /* 1440 KiB */
			new byte[]{ 1,  2, 0, 1, 0,  80, 18, 2, 0xf0, 1, 2,   9, 224,  1 }, /* 1520 KiB (IBM XDF) - UNRELIABLE VALUES!*/
			new byte[]{ 1,  2, 0, 1, 0,  80, 21, 2, 0xf0, 2, 2,   5,  16,  1 }, /* 1680 KiB Microsoft DMF 1024 BPC */
			new byte[]{ 1,  2, 0, 1, 0,  80, 21, 2, 0xf0, 4, 2,   3,  16,  1 }, /* 1680 KiB Microsoft DMF 2048 BPC */
			new byte[]{ 1,  2, 0, 1, 0,  82, 21, 2, 0xf0, 1, 2,  10, 224,  1 }, /* 1722 KiB */
			new byte[]{ 1,  2, 0, 1, 0,  80, 23, 2, 0xf0, 1, 2,  11, 224,  1 }, /* 1840 KiB (IBM XDF) - UNRELIABLE VALUES!*/
			new byte[]{ 2,  2, 3, 1, 0,  80, 36, 2, 0xf0, 2, 2,   9, 240,  1 }, /* 2880 KiB */
			new byte[]{ 2,  2, 3, 1, 0,  80, 36, 2, 0xf0, 2, 2,   9, 240,  1 }, /* 3680 KiB (IBM XDF) - UNRELIABLE VALUES!*/
        };
    }
}