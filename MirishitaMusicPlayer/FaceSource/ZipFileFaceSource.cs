using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.FaceSource
{
    internal class ZipFileFaceSource : IFaceSource, IDisposable
    {
        private readonly ZipArchive archive;

        private readonly Dictionary<string, Image> images = new();

        public ZipFileFaceSource(string zipFilePath)
        {
            archive = ZipFile.OpenRead(zipFilePath);

            foreach (var item in archive.Entries)
            {
                if (item.Name.EndsWith(".png"))
                {
                    images.Add(item.Name, Image.FromStream(item.Open()));
                }
            }
        }

        public Image Expression(int id, bool eyeClose)
        {
            string resourceName = $"{(eyeClose ? "close" : "open")}_{id}.png";
            return images[resourceName];
        }

        public Image Mouth(int id)
        {
            string resourceName = $"mouth_{id}.png";

            if (images.ContainsKey(resourceName))
                return images[resourceName];
            else
                return null;
        }

        public void Dispose()
        {
            foreach (var item in images)
            {
                item.Value.Dispose();
            }
            archive.Dispose();
        }
    }
}
