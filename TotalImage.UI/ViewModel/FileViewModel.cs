using System.IO;
using System.Threading.Tasks;

namespace TotalImage.UI.ViewModel
{
    public class FileViewModel : FileSystemObjectViewModel<FileSystems.File>
    {
        public override async Task Extract(string destination)
        {
            string path = Path.Combine(destination, _fsObject.Name);
            await using (var destStream = new FileStream(path, FileMode.Create))
            await using (var fileStream = _fsObject.GetStream())
            {
                fileStream.Position = 0; // reset position to zero because CopyTo will only go from current position
                await fileStream.CopyToAsync(destStream);
            }

            if (_fsObject.CreationTime.HasValue)
            {
                File.SetCreationTime(path, _fsObject.CreationTime.Value);
            }

            if (_fsObject.LastAccessTime.HasValue)
            {
                File.SetLastAccessTime(path, _fsObject.LastAccessTime.Value);
            }

            if (_fsObject.LastWriteTime.HasValue)
            {
                File.SetLastWriteTime(path, _fsObject.LastWriteTime.Value);
            }
        }

        public FileViewModel(FileSystems.File obj) : base(obj)
        {
        }
    }
}
