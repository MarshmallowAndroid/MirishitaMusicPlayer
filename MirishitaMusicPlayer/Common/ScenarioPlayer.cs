using MirishitaMusicPlayer.Audio;
using MirishitaMusicPlayer.Imas;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Common
{
    public class ScenarioPlayer
    {
        private readonly Song song;

        private readonly SongMixer songMixer;

        private Thread scenarioThread;
        private Thread lightsThread;

        private readonly List<ScenarioEvent> scenarioEvents;
        private readonly ScenarioEvent lightScenarioEvent;

        private bool stopRequested = false;
        private bool scenarioSeeked = false;
        private bool lightsSeeked = false;

        public ScenarioPlayer(Song selectedSong)
        {
            song = selectedSong;
            songMixer = song.Scenario.Configuration.SongMixer;
            scenarioEvents = new()
            {
                new(song.Scenario.MainScenario.Scenario, s =>
                {
                    if (s.Type == ScenarioType.ShowLyrics || s.Type == ScenarioType.HideLyrics)
                    {
                        if (s.Layer == Layer) LyricsChanged?.Invoke(s.Str);
                    }

                    ScenarioTriggered?.Invoke(s);
                }),
                new(song.Scenario.OrientationScenario.Scenario, s =>
                {
                    if (s.Type == ScenarioType.ShowLyrics || s.Type == ScenarioType.HideLyrics)
                    {
                        if (s.Layer == Layer) LyricsChanged?.Invoke(s.Str);
                    }

                    ScenarioTriggered?.Invoke(s);
                }),
                new(song.Scenario.MuteScenarios, s =>
                {
                    if (s.Layer == 0) MuteChanged?.Invoke(s.Mute);
                }),
                new(song.Scenario.LipSyncScenarios, s =>
                {
                    if (s.Idol == Idol && s.Layer == Layer) LipSyncChanged?.Invoke(s.Param);
                }),
                new(song.Scenario.ExpressionScenarios, s =>
                {
                    if ((s.Idol == Idol || s.Idol == Idol + 100) && s.Layer == Layer) ExpressionChanged?.Invoke(s.Param, s.EyeClose == 1);
                })
            };
            lightScenarioEvent = new(song.Scenario.LightScenarios, s =>
            {
                if (s.Layer == Layer)
                {
                    bool on = s.On > 0;

                    LightsChanged?.Invoke(new LightPayload
                    {
                        Color = on ? s.Col : new ColorRGBA(0, 0, 0, 0),
                        Color2 = on ? s.Col2 : new ColorRGBA(0, 0, 0, 0),
                        Color3 = on ? s.Col3 : new ColorRGBA(0, 0, 0, 0),
                        Duration = (float)(s.AbsEndTime - s.AbsTime) * 1000f,
                        Target = s.Target
                    });
                }
            });
        }

        public int Idol { get; set; } = 0;

        public int Layer { get; set; } = 0;

        public void Start()
        {
            if (scenarioThread is not null) return;

            lightsThread = new Thread(DoLightsPlayback);
            lightsThread.IsBackground = true;
            lightsThread.Start();

            scenarioThread = new Thread(DoScenarioPlayback);
            scenarioThread.IsBackground = true;
            scenarioThread.Start();
        }

        public void Stop()
        {
            stopRequested = true;
        }

        public void UpdatePosition()
        {
            scenarioSeeked = true;
            lightsSeeked = true;
        }

        public void Reset()
        {
            foreach (var scenarioEvent in scenarioEvents)
            {
                scenarioEvent.Reset();
                scenarioEvent.Update(0);
            }

            lightScenarioEvent.Reset();
            lightScenarioEvent.Update(0);
        }

        private void DoScenarioPlayback()
        {
            double secondsElapsed = 0;

            while (!songMixer.HasEnded && !stopRequested)
            {
                if (scenarioSeeked)
                {
                    secondsElapsed = songMixer.CurrentTime.TotalSeconds;
                    foreach (var scenarioEvent in scenarioEvents)
                    {
                        scenarioEvent.Seek(secondsElapsed);
                    }

                    scenarioSeeked = false;
                }

                if (secondsElapsed == songMixer.CurrentTime.TotalSeconds)
                {
                    Thread.Sleep(1);
                    continue;
                }

                secondsElapsed = songMixer.CurrentTime.TotalSeconds;
                foreach (var scenarioEvent in scenarioEvents)
                {
                    scenarioEvent.Update(secondsElapsed);
                }

                Thread.Sleep(1);
            }

            scenarioThread = null;

            SongStopped?.Invoke();
        }

        private void DoLightsPlayback()
        {
            double secondsElapsed = 0;

            while (!songMixer.HasEnded && !stopRequested)
            {
                if (lightsSeeked)
                {
                    secondsElapsed = songMixer.CurrentTime.TotalSeconds;
                    lightScenarioEvent.Seek(secondsElapsed);
                    lightsSeeked = false;
                }

                if (secondsElapsed == songMixer.CurrentTime.TotalSeconds)
                {
                    Thread.Sleep(1);
                    continue;
                }

                secondsElapsed = songMixer.CurrentTime.TotalSeconds;
                lightScenarioEvent.Update(secondsElapsed);

                Thread.Sleep(1);
            }

            lightsThread = null;
        }

        public delegate void MuteChangedEventHandler(byte[] mutes);
        public event MuteChangedEventHandler MuteChanged;

        public delegate void ExpressionChangedEventHandler(int expressionId, bool eyeClose);
        public event ExpressionChangedEventHandler ExpressionChanged;

        public delegate void LipSyncChangedEventHandler(int lipSyncId);
        public event LipSyncChangedEventHandler LipSyncChanged;

        public delegate void LyricsChangedEventHandler(string lyrics);
        public event LyricsChangedEventHandler LyricsChanged;

        public delegate void LightsChangedEventHandler(LightPayload lightPayload);
        public event LightsChangedEventHandler LightsChanged;

        public delegate void ScenarioTriggeredEventHandler(EventScenarioData scenarioData);
        public event ScenarioTriggeredEventHandler ScenarioTriggered;

        public delegate void SongStoppedEventHandler();
        public event SongStoppedEventHandler SongStopped;
    }
}
