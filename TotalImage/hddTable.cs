namespace TotalImage
{
    /*
     * List of standard HDD CHS values for the Type list in "New image" dialog
     * This could probably be improved significantly...
     */
    class hddTable
    {
        public static int[][] hdt = new int[][]{
            new int[]{  306,  4, 17 },		/* 0 - 7 */
            new int[]{  615,  2, 17 },
            new int[]{  306,  4, 26 },
            new int[]{ 1024,  2, 17 },
            new int[]{  697,  3, 17 },
            new int[]{  306,  8, 17 },
            new int[]{  614,  4, 17 },
            new int[]{  615,  4, 17 },

            new int[]{  670,  4, 17 },		/* 8 - 15 */
            new int[]{  697,  4, 17 },
            new int[]{  987,  3, 17 },
            new int[]{  820,  4, 17 },
            new int[]{  670,  5, 17 },
            new int[]{  697,  5, 17 },
            new int[]{  733,  5, 17 },
            new int[]{  615,  6, 17 },

    new int[]{  462,  8, 17 },		/* 016-023 */
    new int[]{  306,  8, 26 },
    new int[]{  615,  4, 26 },
    new int[]{ 1024,  4, 17 },
    new int[]{  855,  5, 17 },
    new int[]{  925,  5, 17 },
    new int[]{  932,  5, 17 },
    new int[]{ 1024,  2, 40 },

    new int[]{  809,  6, 17 },		/* 024-031 */
    new int[]{  976,  5, 17 },
    new int[]{  977,  5, 17 },
    new int[]{  698,  7, 17 },
    new int[]{  699,  7, 17 },
    new int[]{  981,  5, 17 },
    new int[]{  615,  8, 17 },
    new int[]{  989,  5, 17 },

    new int[]{  820,  4, 26 },		/* 032-039 */
    new int[]{ 1024,  5, 17 },
    new int[]{  733,  7, 17 },
    new int[]{  754,  7, 17 },
    new int[]{  733,  5, 26 },
    new int[]{  940,  6, 17 },
    new int[]{  615,  6, 26 },
    new int[]{  462,  8, 26 },

    new int[]{  830,  7, 17 },		/* 040-047 */
    new int[]{  855,  7, 17 },
    new int[]{  751,  8, 17 },
    new int[]{ 1024,  4, 26 },
    new int[]{  918,  7, 17 },
    new int[]{  925,  7, 17 },
    new int[]{  855,  5, 26 },
    new int[]{  977,  7, 17 },

    new int[]{  987,  7, 17 },		/* 048-055 */
    new int[]{ 1024,  7, 17 },
    new int[]{  823,  4, 38 },
    new int[]{  925,  8, 17 },
    new int[]{  809,  6, 26 },
    new int[]{  976,  5, 26 },
    new int[]{  977,  5, 26 },
    new int[]{  698,  7, 26 },

    new int[]{  699,  7, 26 },		/* 056-063 */
    new int[]{  940,  8, 17 },
    new int[]{  615,  8, 26 },
    new int[]{ 1024,  5, 26 },
    new int[]{  733,  7, 26 },
    new int[]{ 1024,  8, 17 },
    new int[]{  823, 10, 17 },
    new int[]{  754, 11, 17 },

    new int[]{  830, 10, 17 },		/* 064-071 */
    new int[]{  925,  9, 17 },
    new int[]{ 1224,  7, 17 },
    new int[]{  940,  6, 26 },
    new int[]{  855,  7, 26 },
    new int[]{  751,  8, 26 },
    new int[]{ 1024,  9, 17 },
    new int[]{  965, 10, 17 },

    new int[]{  969,  5, 34 },		/* 072-079 */
    new int[]{  980, 10, 17 },
    new int[]{  960,  5, 35 },
    new int[]{  918, 11, 17 },
    new int[]{ 1024, 10, 17 },
    new int[]{  977,  7, 26 },
    new int[]{ 1024,  7, 26 },
    new int[]{ 1024, 11, 17 },

    new int[]{  940,  8, 26 },		/* 080-087 */
    new int[]{  776,  8, 33 },
    new int[]{  755, 16, 17 },
    new int[]{ 1024, 12, 17 },
    new int[]{ 1024,  8, 26 },
    new int[]{  823, 10, 26 },
    new int[]{  830, 10, 26 },
    new int[]{  925,  9, 26 },

    new int[]{  960,  9, 26 },		/* 088-095 */
    new int[]{ 1024, 13, 17 },
    new int[]{ 1224, 11, 17 },
    new int[]{  900, 15, 17 },
    new int[]{  969,  7, 34 },
    new int[]{  917, 15, 17 },
    new int[]{  918, 15, 17 },
    new int[]{ 1524,  4, 39 },

    new int[]{ 1024,  9, 26 },		/* 096-103 */
    new int[]{ 1024, 14, 17 },
    new int[]{  965, 10, 26 },
    new int[]{  980, 10, 26 },
    new int[]{ 1020, 15, 17 },
    new int[]{ 1023, 15, 17 },
    new int[]{ 1024, 15, 17 },
    new int[]{ 1024, 16, 17 },

    new int[]{ 1224, 15, 17 },		/* 104-111 */
    new int[]{  755, 16, 26 },
    new int[]{  903,  8, 46 },
    new int[]{  984, 10, 34 },
    new int[]{  900, 15, 26 },
    new int[]{  917, 15, 26 },
    new int[]{ 1023, 15, 26 },
    new int[]{  684, 16, 38 },

    new int[]{ 1930,  4, 62 },		/* 112-119 */
    new int[]{  967, 16, 31 },
    new int[]{ 1013, 10, 63 },
    new int[]{ 1218, 15, 36 },
    new int[]{  654, 16, 63 },
    new int[]{  659, 16, 63 },
    new int[]{  702, 16, 63 },
    new int[]{ 1002, 13, 63 },

    new int[]{  854, 16, 63 },		/* 119-127 */
    new int[]{  987, 16, 63 },
    new int[]{  995, 16, 63 },
    new int[]{ 1024, 16, 63 },
    new int[]{ 1036, 16, 63 },
    new int[]{ 1120, 16, 59 },
    new int[]{ 1054, 16, 63 }
        };
    }
}
