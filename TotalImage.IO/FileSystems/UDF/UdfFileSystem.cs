using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalImage.FileSystems.UDF
{
    public class UdfFileSystem : FileSystem
    {
        public UdfFileSystem(Stream containerStream) : base(containerStream)
        {

        }

        public override string DisplayName => throw new NotImplementedException();

        public override string VolumeLabel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override Directory RootDirectory => throw new NotImplementedException();

        public override long TotalFreeSpace => throw new NotImplementedException();

        public override long TotalSize => throw new NotImplementedException();

        public override uint AllocationUnitSize => throw new NotImplementedException();
    }
}
