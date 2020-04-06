using System;
using System.IO;

namespace TotalImage.ImageFormats
{
    public class IMG
    {
        //Commented stuff is irrelevant to basic sector floppy disk images - uncomment when hard disks come into play
        struct FloppyPreset
        {
            //int hole;
            public uint sides;
            //int data_rate;
            //int encoding;
            //int rpm;
            public uint tracks;
            public uint sectors;
            public uint sector_len;
            public uint media_desc;
            public uint spc;
            //int num_fats;
            public uint spfat;
            public uint root_dir_entries;
        };

        private byte[] imageBytes;
        private FloppyPreset[] presets = 
        {
            new FloppyPreset {sides = 1, tracks = 40,  sectors = 8, sector_len = 512, media_desc = 0xfe, spc = 2, spfat = 1, root_dir_entries = 112}, //160k 
            new FloppyPreset {sides = 1, tracks = 40,  sectors = 9, sector_len = 512, media_desc = 0xfc, spc = 2, spfat = 1, root_dir_entries = 112}, //180k
            new FloppyPreset {sides = 2, tracks = 40,  sectors = 8, sector_len = 512, media_desc = 0xff, spc = 2, spfat = 1, root_dir_entries = 112}, //320k 5.25"
            //320k 3.5" is uncertain, don't use!
            new FloppyPreset {sides = 1, tracks = 80,  sectors = 8, sector_len = 512, media_desc = 0xff, spc = 2, spfat = 1, root_dir_entries = 112}, //320k 3.5"
            new FloppyPreset {sides = 2, tracks = 40,  sectors = 9, sector_len = 512, media_desc = 0xfd, spc = 2, spfat = 2, root_dir_entries = 112}, //360k 5.25"
            //360k 3.5" is uncertain, don't use!
            new FloppyPreset {sides = 1, tracks = 80,  sectors = 9, sector_len = 512, media_desc = 0xfd, spc = 2, spfat = 2, root_dir_entries = 112}, //360k 3.5"
            //400k is uncertain, don't use!
            new FloppyPreset {sides = 1, tracks = 80,  sectors = 10, sector_len = 512, media_desc = 0xfd, spc = 2, spfat = 2, root_dir_entries = 112}, //400k RX50
            new FloppyPreset {sides = 2, tracks = 80,  sectors = 8, sector_len = 512, media_desc = 0xfb, spc = 2, spfat = 2, root_dir_entries = 112}, //640k
            new FloppyPreset {sides = 2, tracks = 80,  sectors = 9, sector_len = 512, media_desc = 0xf9, spc = 2, spfat = 3, root_dir_entries = 112}, //720k
            //800k is uncertain, don't use!
            new FloppyPreset {sides = 2, tracks = 80,  sectors = 10, sector_len = 512, media_desc = 0xf9, spc = 2, spfat = 3, root_dir_entries = 112}, //800k
            new FloppyPreset {sides = 2, tracks = 80,  sectors = 15, sector_len = 512, media_desc = 0xf9, spc = 1, spfat = 7, root_dir_entries = 224}, //1200k
            new FloppyPreset {sides = 2, tracks = 77,  sectors = 8, sector_len = 1024, media_desc = 0xfe, spc = 1, spfat = 2, root_dir_entries = 192}, //1232k
            new FloppyPreset {sides = 2, tracks = 80,  sectors = 18, sector_len = 512, media_desc = 0xf0, spc = 1, spfat = 9, root_dir_entries = 224}, //1440k
            //1520k is uncertain, don't use!
            new FloppyPreset {sides = 2, tracks = 80,  sectors = 15, sector_len = 512, media_desc = 0xf8, spc = 1, spfat = 7, root_dir_entries = 224}, //1520k
            new FloppyPreset {sides = 2, tracks = 80,  sectors = 21, sector_len = 512, media_desc = 0xf0, spc = 4, spfat = 3, root_dir_entries = 16}, //1680k
            new FloppyPreset {sides = 2, tracks = 82,  sectors = 21, sector_len = 512, media_desc = 0xf0, spc = 1, spfat = 10, root_dir_entries = 224}, //1722k
            //1840k is uncertain, don't use!
            new FloppyPreset {sides = 2, tracks = 80,  sectors = 18, sector_len = 512, media_desc = 0xf0, spc = 1, spfat = 9, root_dir_entries = 224}, //1840k
            new FloppyPreset {sides = 2, tracks = 80,  sectors = 36, sector_len = 512, media_desc = 0xf0, spc = 2, spfat = 9, root_dir_entries = 240}, //2880k
            //3680k is uncertain, don't use!
            new FloppyPreset {sides = 2, tracks = 80,  sectors = 36, sector_len = 512, media_desc = 0xf0, spc = 2, spfat = 9, root_dir_entries = 240}, //3680k
        };

        //Returns byte array of the image
        public byte[] GetImageBytes()
        {
            return imageBytes;
        }

        //Creates a new image from the selected preset - PORTED FROM 86BOX
        public void CreateImage(int index)
        {
            uint total_size = 0;
            uint total_sectors = 0;
            uint root_dir_bytes = 0;
            uint fat_size = 0;
            uint fat1_offs = 0;
            uint fat2_offs = 0;
            uint zero_bytes = 0;

            total_sectors = presets[index].sides * presets[index].tracks * presets[index].sectors;
            total_size = total_sectors * presets[index].sector_len;
            root_dir_bytes = presets[index].root_dir_entries << 5;
            fat_size = presets[index].spfat * presets[index].sector_len;
            fat1_offs = presets[index].sector_len;
            fat2_offs = fat1_offs + fat_size;
            zero_bytes = fat2_offs + fat_size + root_dir_bytes;

            imageBytes = new byte[total_size];
            imageBytes.Initialize(); //Fill with zeroes

            //Fill the data area with 0xF6
            for(uint i = zero_bytes; i < total_size; i++)
            {
                imageBytes[i] = 0xF6;
            }

            imageBytes[0x00] = 0xEB;         /* Jump to make MS-DOS happy. */
            imageBytes[0x01] = 0x58;
            imageBytes[0x02] = 0x90;
            imageBytes[0x03] = 0x38;         /* '86BOX5.0' OEM ID. */
            imageBytes[0x04] = 0x36;
            imageBytes[0x05] = 0x42;
            imageBytes[0x06] = 0x4F;
            imageBytes[0x07] = 0x58;
            imageBytes[0x08] = 0x35;
            imageBytes[0x09] = 0x2E;
            imageBytes[0x0A] = 0x30;

            Buffer.BlockCopy(BitConverter.GetBytes(presets[index].sector_len), 0, imageBytes, 0x0B, 2);
            imageBytes.SetValue((byte)presets[index].spc, 0x0D);
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)1), 0, imageBytes, 0x0E, 2);
            imageBytes.SetValue((byte)2, 0x10);
            Buffer.BlockCopy(BitConverter.GetBytes(presets[index].root_dir_entries), 0, imageBytes, 0x11, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(total_sectors), 0, imageBytes, 0x13, 2);
            imageBytes.SetValue((byte)presets[index].media_desc, 0x15);
            Buffer.BlockCopy(BitConverter.GetBytes(presets[index].spfat), 0, imageBytes, 0x16, 2);
            imageBytes.SetValue((byte)presets[index].sectors, 0x18);
            imageBytes.SetValue((byte)presets[index].sides, 0x1A);

            Random rand = new Random();
            imageBytes[0x26] = 0x29;         /* ')' followed by randomly-generated volume serial number. */
            imageBytes[0x27] = (byte)rand.Next();
            imageBytes[0x28] = (byte)rand.Next();
            imageBytes[0x29] = (byte)rand.Next();
            imageBytes[0x2A] = (byte)rand.Next();

            for(int i = 0x2B; i <= 0x36; i++)
            {
                imageBytes[i] = 0x20;
            }

            imageBytes[0x36] = (byte)'F';
            imageBytes[0x37] = (byte)'A';
            imageBytes[0x38] = (byte)'T';
            imageBytes[0x39] = (byte)'1';
            imageBytes[0x3A] = (byte)'2';

            for (int i = 0x3B; i < 0x3E; i++)
            {
                imageBytes[i] = 0x20;
            }

            imageBytes[0x1FE] = 0x55;
            imageBytes[0x1FF] = 0xAA;

            imageBytes[fat1_offs + 0x00] = imageBytes[fat2_offs + 0x00] = imageBytes[0x15];
            imageBytes[fat1_offs + 0x01] = imageBytes[fat2_offs + 0x01] = 0xFF;
            imageBytes[fat1_offs + 0x02] = imageBytes[fat2_offs + 0x02] = 0xFF;
        }

        //This method will take all the custom parameters and create a new image based on them
        public void CreateCustomImage()
        {
            /* Do custom parameter stuff here */
        }

        public void LoadImage(string path)
        {
            imageBytes = File.ReadAllBytes(path);
        }
    }
}