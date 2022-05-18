using MirishitaMusicPlayer.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Forms.Classes
{
    internal class EmbeddedResourceFaceSource : IFaceSource
    {
        public EmbeddedResourceFaceSource()
        {

        }
        public Image Expression(int id, bool eyeClose)
        {
            string resourceName = $"{(eyeClose ? "close" : "open")}_{id}";
            return Resources.ResourceManager.GetObject(resourceName) as Image;
        }

        public Image Mouth(int id)
        {
            string resourceName = $"mouth_{id}";
            return Resources.ResourceManager.GetObject(resourceName) as Image;
        }


        public void Dispose()
        {
        }

    }
}
