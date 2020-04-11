using System.IO;
using TotalImage.FileSystems.FAT;

namespace TotalImage.ImageFormats
{
    /*
     * Class for handling raw sector disk images
     */
    public class RawSector
    {
        private byte[] imageBytes;
        //private BiosParameterBlock bpb;
        private MemoryStream stream;
      
        public byte[] GetImageBytes()
        {
            return imageBytes;
        }

        //Creates a new image from the selected preset
        public void CreateImage(BiosParameterBlock bpb, byte tracks)
        {
            uint imageSize = (uint)(bpb.BytesPerLogicalSector * bpb.PhysicalSectorsPerTrack * bpb.NumberOfHeads * tracks);
            imageBytes = new byte[imageSize];
            stream = new MemoryStream(imageBytes, true);
            Fat12 fat12 = Fat12.Create(stream, bpb, tracks);
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
            imageBytes = System.IO.File.ReadAllBytes(path);
            stream = new MemoryStream(imageBytes, true);
            Fat12 fat12 = new Fat12(stream);
            /*bpb = fat12.Parse();
            fat12.ReadRootDir(bpb);*/
        }

        //Closes and unlocks the file
        public void CloseImage()
        {
            if(stream != null)
            {
                stream.Flush();
                stream.Close();
            }
        }

        //Lists the contents of the specified directory
        public void ListDirectory(FatDirEntry entry)
        {
            Fat12 fat12 = new Fat12(stream);
            //fat12.ListDir(bpb, entry);
        }

        //Lists the contents of the root directory
        public void ListRootDirectory()
        {
            Fat12 fat12 = new Fat12(stream);
            //fat12.ListRootDir(bpb);
        }

        //Changes the volume label
        public void ChangeVolumeLabel(string label)
        {
            Fat12 fat12 = new Fat12(stream);
            //fat12.ChangeVolLabel(bpb, label);
        }
    }
}