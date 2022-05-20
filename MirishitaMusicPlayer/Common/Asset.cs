using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Common
{
    public class Asset
    {
        public Asset(string name, string remoteName, uint size)
        {
            Name = name;
            RemoteName = remoteName;
            Size = size;
        }

        public string Name { get; }

        public string RemoteName { get; }

        public uint Size { get; }
    }
}
