using AssetStudio;
using MirishitaMusicPlayer.Audio;
using MirishitaMusicPlayer.Forms;
using MirishitaMusicPlayer.Imas;
using MirishitaMusicPlayer.Net.TDAssets;
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

        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Console.OutputEncoding = Encoding.UTF8;

            bool quit = false;

            AssetsManager assetsManager = new();
            SongSelectForm songSelectForm = new(assetsManager);

            while (!quit)
            {
                songSelectForm.ShowDialog();

                Song song = songSelectForm.Song;
                if (song == null) return;

                Console.Title = song.SongId;

                IdolOrderForm idolOrderForm = new(song);

                if (idolOrderForm.ProcessSong())
                {
                    OutputDevice.Play();
                }
                else continue;

                idolOrderForm.Dispose();
                //SongMixer songMixer = idolOrderForm.SongMixer;
                //WaveOutEvent outputDevice = idolOrderForm.OutputDevice;

                //if (songMixer == null) continue;

                ScenarioPlayer scenarioPlayer = new(song);

                //PlayerForm playerForm = new(idolOrderForm.Order, scenarios.VoiceCount, songMixer, outputDevice);

                //scenarioPlayback.ExpressionChanged += (ex, ey) => playerForm.UpdateExpression(ex, ey);
                //scenarioPlayback.LipSyncChanged += (l) => playerForm.UpdateLipSync(l);
                //scenarioPlayback.LyricsChanged += (l) => playerForm.UpdateLyrics(l);
                //scenarioPlayback.MuteChanged += (m) => playerForm.UpdateMute(m);
                //scenarioPlayback.SongStopped += () => playerForm.Stop(true);

                //scenarioPlayback.Start();

                //playerForm.ShowDialog();
            }
        }
    }
}