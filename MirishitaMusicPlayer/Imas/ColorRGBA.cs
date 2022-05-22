using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Imas
{
    public class ColorRGBA
    {
        public float A { get; set; }

        public float R { get; set; }

        public float G { get; set; }

        public float B { get; set; }

        public Color ToColor()
        {
            int a = 255;
            int r = (int)Math.Clamp(255 * R, 0, 255);
            int g = (int)Math.Clamp(255 * G, 0, 255);
            int b = (int)Math.Clamp(255 * B, 0, 255);

            return System.Drawing.Color.FromArgb(a, r, g, b);
        }
    }
}
