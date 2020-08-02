using System;
using System.IO;
using TotalImage.FileSystems;
using TotalImage.FileSystems.FAT;

namespace TotalImage.ImageFormats
{
    /*
     * Class for handling raw sector disk images
     */
    public class RawSector
    {
        private byte[] imageBytes;
        private Stream stream;
        /*private*/ public Fat12 fat12;
      
        public byte[] GetImageBytes()
        {
            return imageBytes;
        }

        //Creates a new image with the provided parameters
        public void CreateImage(BiosParameterBlock bpb, byte tracks, bool writeBPB)
        {
            if (bpb == null)
                throw new ArgumentNullException(nameof(bpb), "BPB cannot be null!");

            uint imageSize = (uint)(bpb.BytesPerLogicalSector * bpb.PhysicalSectorsPerTrack * bpb.NumberOfHeads * tracks);
            imageBytes = new byte[imageSize];

            //At this point we need to consider writeBPB value...
            stream = new MemoryStream(imageBytes, true); 
            fat12 = Fat12.Create(stream, bpb);
        }

        //Loads an image file
        public void LoadImage(string path)
        {
            //For larger images (HDD etc.) we probably won't read the entire file at once, but use the stream instead...
            stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            imageBytes = new byte[stream.Length];
            stream.Read(imageBytes, 0, (int)stream.Length);
            fat12 = new Fat12(stream);
            //fat12.ReadRootDir();
        }

        //Closes and unlocks the file
        public void CloseImage()
        {
            if(stream != null)
            {
                stream.Flush();
                stream.Close();
                stream.Dispose();
            }
        }

        //Sets a new volume label
        public void ChangeVolumeLabel(string label)
        {
            fat12.ChangeVolLabel(label);
        }
        
        //Returns the current volume label in the root directory
        public string GetRDVolumeLabel()
        {
            return fat12.GetRDVolLabel();
        }

        //Returns the current volume label in the BPB
        public string GetBPBVolumeLabel()
        {
            return fat12.GetBPBVolLabel();
        }

        /* Extracts the file system object from the image to the specified path
         * This is a very basic solution for now. Needs to be decided if attributes and other DateTime values should be preserved
         * in the extracted file as well. */
        public void ExtractFile(DirectoryEntry entry, string path)
        {
            if (!Convert.ToBoolean(entry.attr & 0x10))
            {
                uint cluster = ((uint)entry.fstClusHI << 16) | entry.fstClusLO;

                using (var fs = new FileStream(path + Path.DirectorySeparatorChar + entry.name, FileMode.Append, FileAccess.Write))
                {
                    do
                    {
                        byte[] clusterBytes = fat12.ReadCluster(cluster);
                        fs.Write(clusterBytes, 0, clusterBytes.Length);
                        cluster = fat12.FatGetNextCluster(cluster, false); 
                    }
                    while (cluster <= 0xFEF);

                    fs.SetLength(entry.fileSize); //Remove the trailing unused bytes from the last cluster
                }

                System.IO.File.SetLastWriteTime(path + Path.DirectorySeparatorChar + entry.name, (DateTime)Helper.FatToDateTime(entry.wrtDate, entry.wrtTime));
            }
        }
    }
}