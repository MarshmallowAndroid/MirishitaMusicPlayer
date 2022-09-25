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

        private readonly ScenarioScrObject mainScenario;
        private readonly ScenarioScrObject orientScenario;
        private readonly List<EventScenarioData> muteScenarios;
        private readonly List<EventScenarioData> expressionScenarios;
        private readonly List<EventScenarioData> lightScenarios;

        private bool stopRequested = false;
        private bool scenarioSeeked = false;
        private bool lightsSeeked = false;

        public ScenarioPlayer(Song selectedSong)
        {
            song = selectedSong;

            songMixer = song.Scenario.Configuration.SongMixer;

            mainScenario = song.Scenario.MainScenario;
            orientScenario = song.Scenario.OrientationScenario;
            muteScenarios = song.Scenario.MuteScenarios;
            expressionScenarios = song.Scenario.ExpressionScenarios;
            lightScenarios = song.Scenario.LightScenarios;
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

        private void DoScenarioPlayback()
        {
            int muteScenarioIndex = 0;
            int expressionScenarioIndex = 0;
            //int lightScenarioIndex = 0;
            int mainScenarioIndex = 0;
            int orientScenarioIndex = 0;

            double secondsElapsed = 0;

            EventScenarioData currentMuteScenario;
            EventScenarioData currentExpressionScenario;
            //EventScenarioData currentLightScenario;
            EventScenarioData currentMainScenario;
            EventScenarioData currentOrientScenario;

            currentMuteScenario = muteScenarios[muteScenarioIndex];
            currentExpressionScenario = expressionScenarios[expressionScenarioIndex];
            //currentLightScenario = lightScenarios[lightScenarioIndex];
            currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
            currentOrientScenario = orientScenario.Scenario[orientScenarioIndex];

            while (!songMixer.HasEnded && !stopRequested)
            {
                if (scenarioSeeked)
                {
                    muteScenarioIndex = 0;
                    expressionScenarioIndex = 0;
                    //lightScenarioIndex = 0;
                    mainScenarioIndex = 0;
                    orientScenarioIndex = 0;

                    currentMuteScenario = muteScenarios[muteScenarioIndex];
                    currentExpressionScenario = expressionScenarios[expressionScenarioIndex];
                    //currentLightScenario = lightScenarios[lightScenarioIndex];
                    currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
                    currentOrientScenario = orientScenario.Scenario[orientScenarioIndex];

                    secondsElapsed = songMixer.CurrentTime.TotalSeconds;

                    if (secondsElapsed > 0)
                    {
                        while (secondsElapsed >= currentMuteScenario.AbsTime)
                        {
                            if (muteScenarioIndex < muteScenarios.Count - 1) muteScenarioIndex++;
                            else break;
                            currentMuteScenario = muteScenarios[muteScenarioIndex];
                        }

                        while (secondsElapsed >= currentExpressionScenario.AbsTime)
                        {
                            if (expressionScenarioIndex < expressionScenarios.Count - 1) expressionScenarioIndex++;
                            else break;
                            currentExpressionScenario = expressionScenarios[expressionScenarioIndex];
                        }

                        //while (secondsElapsed >= currentLightScenario.AbsTime)
                        //{
                        //    if (lightScenarioIndex < lightScenarios.Count - 1) lightScenarioIndex++;
                        //    else break;
                        //    currentLightScenario = lightScenarios[lightScenarioIndex];
                        //}

                        while (secondsElapsed >= currentMainScenario.AbsTime)
                        {
                            if (mainScenarioIndex < mainScenario.Scenario.Count - 1) mainScenarioIndex++;
                            else break;
                            currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
                        }

                        while (secondsElapsed >= currentOrientScenario.AbsTime)
                        {
                            if (orientScenarioIndex < orientScenario.Scenario.Count - 1) orientScenarioIndex++;
                            else break;
                            currentOrientScenario = orientScenario.Scenario[orientScenarioIndex];
                        }

                        //muteIndex -= 2;
                        //expressionScenarioIndex -= 2;
                        //mainScenarioIndex -= 2;
                        //orientScenarioIndex -= 2;

                        //currentMuteScenario = muteScenarios[muteIndex];
                        //currentExpressionScenario = expressionScenarios[expressionScenarioIndex];
                        //currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
                        //currentOrientScenario = orientScenario.Scenario[orientScenarioIndex];

                        MuteChanged?.Invoke(currentMuteScenario.Mute);
                        ExpressionChanged?.Invoke(currentExpressionScenario.Param, currentExpressionScenario.EyeClose == 1);
                        LipSyncChanged?.Invoke(currentMainScenario.Param);
                        LyricsChanged?.Invoke(currentMainScenario.Str);
                    }

                    scenarioSeeked = false;
                }

                if (secondsElapsed == songMixer.CurrentTime.TotalSeconds)
                {
                    Thread.Sleep(1);
                    continue;
                }

                secondsElapsed = songMixer.CurrentTime.TotalSeconds;

                while (secondsElapsed >= currentMuteScenario.AbsTime)
                {
                    if (currentMuteScenario.Layer == 0)
                        MuteChanged?.Invoke(currentMuteScenario.Mute);

                    if (muteScenarioIndex < muteScenarios.Count - 1) muteScenarioIndex++;
                    else break;
                    currentMuteScenario = muteScenarios[muteScenarioIndex];
                }

                while (secondsElapsed >= currentExpressionScenario.AbsTime)
                {
                    if ((currentExpressionScenario.Idol == Idol ||
                        currentExpressionScenario.Idol == Idol + 100)
                        && currentExpressionScenario.Layer == Layer)
                        ExpressionChanged?.Invoke(currentExpressionScenario.Param, currentExpressionScenario.EyeClose == 1);

                    if (expressionScenarioIndex < expressionScenarios.Count - 1) expressionScenarioIndex++;
                    else break;
                    currentExpressionScenario = expressionScenarios[expressionScenarioIndex];
                }

                //while (secondsElapsed >= currentLightScenario.AbsTime)
                //{
                //    if (currentLightScenario.Layer == Layer)
                //    {
                //        bool on = currentLightScenario.On > 0;

                //        LightsChanged?.Invoke(new LightPayload
                //        {
                //            Color = on ? currentLightScenario.Col : new ColorRGBA(0, 0, 0, 0),
                //            Color2 = on ? currentLightScenario.Col2 : new ColorRGBA(0, 0, 0, 0),
                //            Color3 = on ? currentLightScenario.Col3 : new ColorRGBA(0, 0, 0, 0),
                //            Duration = (float)(currentLightScenario.AbsEndTime - currentLightScenario.AbsTime) * 1000f,
                //            Target = currentLightScenario.Target
                //        });
                //    }

                //    if (lightScenarioIndex < lightScenarios.Count - 1) lightScenarioIndex++;
                //    else break;
                //    currentLightScenario = lightScenarios[lightScenarioIndex];
                //}

                while (secondsElapsed >= currentMainScenario.AbsTime)
                {
                    if (currentMainScenario.Type == ScenarioType.LipSync)
                    {
                        if (currentMainScenario.Idol == Idol && currentMainScenario.Layer == Layer)
                            LipSyncChanged?.Invoke(currentMainScenario.Param);
                    }

                    if (currentMainScenario.Type == ScenarioType.ShowLyrics || currentMainScenario.Type == ScenarioType.HideLyrics)
                    {
                        if (currentMainScenario.Layer == Layer)
                            LyricsChanged?.Invoke(currentMainScenario.Str);
                    }

                    ScenarioTriggered?.Invoke(currentMainScenario);

                    if (mainScenarioIndex < mainScenario.Scenario.Count - 1) mainScenarioIndex++;
                    else break;
                    currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
                }

                while (secondsElapsed >= currentOrientScenario.AbsTime)
                {
                    ScenarioTriggered?.Invoke(currentOrientScenario);

                    if (orientScenarioIndex < orientScenario.Scenario.Count - 1) orientScenarioIndex++;
                    else break;
                    currentOrientScenario = orientScenario.Scenario[orientScenarioIndex];
                }

                Thread.Sleep(1);
            }

            scenarioThread = null;

            SongStopped?.Invoke();
        }

        private void DoLightsPlayback()
        {
            int lightScenarioIndex = 0;
            double secondsElapsed = 0;

            EventScenarioData currentLightScenario;
            currentLightScenario = lightScenarios[lightScenarioIndex];

            while (!songMixer.HasEnded && !stopRequested)
            {
                if (lightsSeeked)
                {
                    lightScenarioIndex = 0;
                    currentLightScenario = lightScenarios[lightScenarioIndex];

                    secondsElapsed = songMixer.CurrentTime.TotalSeconds;

                    if (secondsElapsed > 0)
                    {
                        while (secondsElapsed >= currentLightScenario.AbsTime)
                        {
                            if (lightScenarioIndex < lightScenarios.Count - 1) lightScenarioIndex++;
                            else break;
                            currentLightScenario = lightScenarios[lightScenarioIndex];
                        }

                        bool on = currentLightScenario.On > 0;

                        LightsChanged?.Invoke(new LightPayload
                        {
                            Color = on ? currentLightScenario.Col : new ColorRGBA(0, 0, 0, 0),
                            Color2 = on ? currentLightScenario.Col2 : new ColorRGBA(0, 0, 0, 0),
                            Color3 = on ? currentLightScenario.Col3 : new ColorRGBA(0, 0, 0, 0),
                            Duration = (float)(currentLightScenario.AbsEndTime - currentLightScenario.AbsTime) * 1000f,
                            Target = currentLightScenario.Target
                        });
                    }

                    lightsSeeked = false;
                }

                if (secondsElapsed == songMixer.CurrentTime.TotalSeconds)
                {
                    Thread.Sleep(1);
                    continue;
                }

                secondsElapsed = songMixer.CurrentTime.TotalSeconds;

                while (secondsElapsed >= currentLightScenario.AbsTime)
                {
                    if (currentLightScenario.Layer == Layer)
                    {
                        bool on = currentLightScenario.On > 0;

                        LightsChanged?.Invoke(new LightPayload
                        {
                            Color = on ? currentLightScenario.Col : new ColorRGBA(0, 0, 0, 0),
                            Color2 = on ? currentLightScenario.Col2 : new ColorRGBA(0, 0, 0, 0),
                            Color3 = on ? currentLightScenario.Col3 : new ColorRGBA(0, 0, 0, 0),
                            Duration = (float)(currentLightScenario.AbsEndTime - currentLightScenario.AbsTime) * 1000f,
                            Target = currentLightScenario.Target
                        });
                    }

                    if (lightScenarioIndex < lightScenarios.Count - 1) lightScenarioIndex++;
                    else break;
                    currentLightScenario = lightScenarios[lightScenarioIndex];
                }

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
