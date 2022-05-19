using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Imas
{
    public class Color
    {
        public float A { get; set; }

        public float R { get; set; }

        public float G { get; set; }

        public float B { get; set; }

        public System.Drawing.Color ToColor()
        {
            int a = (int)(255 * A);
            int r = (int)(255 * R);
            int g = (int)(255 * G);
            int b = (int)(255 * B);

            return System.Drawing.Color.FromArgb(a, r, g, b);
        }
    }
}
