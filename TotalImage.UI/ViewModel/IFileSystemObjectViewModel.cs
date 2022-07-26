using System;
using System.Threading.Tasks;

namespace TotalImage.UI.ViewModel
{
    public interface IFileSystemObjectViewModel
    {
        string Name { get; }
        string FullName { get; }
        ulong Length { get; }
        DateTime? LastWriteTime { get; }
        string Attributes { get; }
        bool IsHidden { get; }

        Task Extract(string destination);
    }
}
