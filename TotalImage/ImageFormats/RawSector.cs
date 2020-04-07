using System;
using System.IO;
using TotalImage.FileSystems;

namespace TotalImage.ImageFormats
{
    public class RawSector
    {
        private byte[] imageBytes;
      
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

        //Create a new image based on custom parameters
        public void CreateCustomImage()
        {
            /* Do custom parameter stuff here */
        }

        //Load an image file
        public void LoadImage(string path)
        {
            imageBytes = File.ReadAllBytes(path);
            Fat12 fat12 = new Fat12();
            fat12.ReadRootDir(imageBytes);
        }
    }
}