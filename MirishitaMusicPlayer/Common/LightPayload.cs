using MirishitaMusicPlayer.Imas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Common
{
    public class LightPayload
    {
        public ColorRGBA Color { get; set; }

        public ColorRGBA Color2 { get; set; }

        public ColorRGBA Color3 { get; set; }

        public int Target { get; set; }

        public float Duration { get; set; }
    }
}
