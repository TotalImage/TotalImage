using System;
using System.Collections.Generic;
using static System.IO.Path;

namespace TotalImage.FileSystems
{
    public abstract class Directory : FileSystemObject
    {
        public abstract Directory Parent { get; }
        public abstract Directory Root { get; }

        public abstract Directory[] GetDirectories();
        public abstract File[] GetFiles();
        public abstract FileSystemObject[] GetFileSystemObjects();

        public override string FullName
        {
            get
            {
                var path = new List<string>();
                var currentDir = this;

                while((currentDir = currentDir.Parent) != null)
                    path.Add(currentDir.Name);

                path.Reverse();

                return DirectorySeparatorChar + string.Join(DirectorySeparatorChar.ToString(), path);
            }
        }

        public abstract Directory CreateSubdirectory(string path);
    }
}