using AssetStudio;
using MirishitaMusicPlayer.AssetStudio;
using MirishitaMusicPlayer.Audio;
using MirishitaMusicPlayer.Common;
using MirishitaMusicPlayer.Forms;
using MirishitaMusicPlayer.Imas;
using MirishitaMusicPlayer.Net.TDAssets;
using MirishitaMusicPlayer.Rgb;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MirishitaMusicPlayer
{
    internal class Program
    {
        public static readonly string CachePath = "Cache";
        public static readonly string JacketsPath = Path.Combine(CachePath, "Songs");
        public static readonly string SongsPath = Path.Combine(CachePath, "Songs");
        public static readonly WaveOutEvent OutputDevice = new() { DesiredLatency = 100 };
        public static readonly RgbManager RgbManager = new();

        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Console.OutputEncoding = Encoding.UTF8;

            bool quit = false;

            AssetsManager assetsManager = AssetStudioGlobal.AssetsManager;
            SongSelectForm songSelectForm = new(assetsManager);

            while (!quit)
            {
                Song song = songSelectForm.ProcessSong(args.Length > 0 ? args[0] : "");
                if (args.Length > 0) args[0] = "";

                if (song == null) return;

                Console.Title = song.SongId;

                IdolOrderForm idolOrderForm = new(song);

                bool songProcessedSuccessfully = idolOrderForm.ProcessSong();
                idolOrderForm.Dispose();

                if (songProcessedSuccessfully)
                    OutputDevice.Play();
                else
                    continue;

                idolOrderForm.Dispose();

                PlayerForm playerForm = new(song);
                playerForm.ShowDialog();
            }
        }
    }
}