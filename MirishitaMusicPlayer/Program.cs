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
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Console.OutputEncoding = Encoding.UTF8;

            bool quit = false;

            WaveOutEvent waveOutEvent = new() { DesiredLatency = 100 };
            AssetsManager assetsManager = new();
            SongSelectForm songSelectForm = new(waveOutEvent);

            while (!quit)
            {
                songSelectForm.ShowDialog();

                string songID = songSelectForm.ResultSongID ?? "";
                if (songID == "") return;

                Console.Title = songID;

                string filesPath = "Cache\\Songs";

                ScenarioLoader scenarios = new(assetsManager, filesPath, songID);
                ScenarioScrObject mainScenario = scenarios.MainScenario;
                List<EventScenarioData> expressionScenarios = scenarios.ExpressionScenarios;
                List<EventScenarioData> muteScenarios = scenarios.MuteScenarios;

                IdolOrderForm idolOrderForm = new(
                    songSelectForm.AssetList,
                    songID,
                    scenarios.VoiceCount,
                    assetsManager,
                    scenarios.MuteScenarios);
                idolOrderForm.ProcessSong();

                SongMixer songMixer = idolOrderForm.SongMixer;
                WaveOutEvent outputDevice = idolOrderForm.OutputDevice;

                if (songMixer == null) continue;

                idolOrderForm.Dispose();

                ScenarioPlayer scenarioPlayback = new(
                    outputDevice,
                    songMixer,
                    mainScenario,
                    expressionScenarios,
                    muteScenarios);

                PlayerForm playerForm = new(idolOrderForm.Order, songMixer, outputDevice);

                scenarioPlayback.ExpressionChanged += (ex, ey) => playerForm.UpdateExpression(ex, ey);
                scenarioPlayback.LipSyncChanged += (l) => playerForm.UpdateLipSync(l);
                scenarioPlayback.LyricsChanged += (l) => playerForm.UpdateLyrics(l);
                scenarioPlayback.MuteChanged += (m) => playerForm.UpdateMute(m);
                scenarioPlayback.SongStopped += () => playerForm.Stop(true);

                scenarioPlayback.Start();
                outputDevice.Play();

                playerForm.ShowDialog();
            }
        }
    }
}