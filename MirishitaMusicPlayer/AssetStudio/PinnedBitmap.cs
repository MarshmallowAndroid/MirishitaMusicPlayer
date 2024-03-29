﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MirishitaMusicPlayer.AssetStudio
{
    class PinnedBitmap : IDisposable
    {
        private readonly GCHandle bitmapHandle;
        private readonly Bitmap bitmap;

        public PinnedBitmap(byte[] imageBytes, int width, int height)
        {
            bitmapHandle = GCHandle.Alloc(imageBytes, GCHandleType.Pinned);
            bitmap = new Bitmap(
                width,
                height,
                width * 4,
                PixelFormat.Format32bppArgb,
                bitmapHandle.AddrOfPinnedObject());
        }

        public Bitmap Bitmap => bitmap;

        public void Dispose()
        {
            bitmap.Dispose();
            bitmapHandle.Free();
        }
    }
}
