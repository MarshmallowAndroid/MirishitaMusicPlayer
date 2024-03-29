﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.FaceSource
{
    public interface IFaceSource : IDisposable
    {
        public Image Expression(int id, bool eyeClose);

        public Image Mouth(int id);
    }
}
