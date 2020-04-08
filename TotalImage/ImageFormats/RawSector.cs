using System;
using System.IO;
using System.Text;
using TotalImage.FileSystems;

namespace TotalImage.ImageFormats
{
    public class RawSector
    {
        private byte[] imageBytes;
        private FileStream fs;
      
        //Returns byte array of the image
        public byte[] GetImageBytes()
        {
            return imageBytes;
        }

        //Creates a new image from the selected preset - PORTED FROM 86BOX
        public void CreateImage(BiosParameterBlock bpb, string oemID, byte tracks)
        {
            uint imageSize = (ushort)(bpb.BytesPerLogicalSector * bpb.PhysicalSectorsPerTrack * bpb.NumberOfHeads * tracks);
            imageBytes = new byte[imageSize];
            Fat12 fat12 = new Fat12();
            fat12.Format(imageBytes, bpb, oemID, tracks);
        }

        //Creates a new image based on custom parameters
        public void CreateCustomImage()
        {
            /* Do custom parameter stuff here */
        }

        //Loads an image file
        public void LoadImage(string path)
        {
            //For larger images (HDD etc.) we probably won't read the entire file at once, but use the stream instead...
            imageBytes = File.ReadAllBytes(path);
            fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            Fat12 fat12 = new Fat12();
            fat12.ReadRootDir(imageBytes);
        }

        //Closes and unlocks the file
        public void CloseImage()
        {
            fs.Flush();
            fs.Close();
        }
    }
}