using AssetStudio;
using MirishitaMusicPlayer.AssetStudio;
using MirishitaMusicPlayer.Audio;
using MirishitaMusicPlayer.Common;
using MirishitaMusicPlayer.Forms;
using MirishitaMusicPlayer.Imas;
using MirishitaMusicPlayer.Net.TDAssets;
using MirishitaMusicPlayer.RgbPluginBase;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MirishitaMusicPlayer
{
    internal class Program
    {
        private static AssemblyLoadContext rgbPluginContext;

        public static readonly string CachePath = "Cache";
        public static readonly string JacketsPath = Path.Combine(CachePath, "Songs");
        public static readonly string SongsPath = Path.Combine(CachePath, "Songs");
        public static readonly WaveOutEvent OutputDevice = new() { DesiredLatency = 100 };

        private static Stream pluginFileStream;

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

        public static IRgbManager RgbManager { get; set; }

        public static void UnloadPlugin()
        {
            RgbManager = null;

            if (rgbPluginContext?.Assemblies.Count() > 0)
                rgbPluginContext.Unload();

            pluginFileStream?.Dispose();
        }

        public static IRgbManager CreateRgbManager()
        {
            string[] pluginPaths = Directory.GetFiles(Directory.GetCurrentDirectory(), "*Plugin.*");

            if (pluginPaths.Length < 1) return null;

            pluginFileStream = File.OpenRead(pluginPaths[0]);
            rgbPluginContext = new AssemblyLoadContext(null, true);

            Assembly assembly = rgbPluginContext.LoadFromStream(pluginFileStream);
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IRgbManager).IsAssignableFrom(type))
                {
                    return (IRgbManager)Activator.CreateInstance(type);
                }
            }

            return null;
        }
    }
}