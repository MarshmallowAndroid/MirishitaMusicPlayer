using MirishitaMusicPlayer.Audio;
using MirishitaMusicPlayer.Imas;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer
{
    public class ScenarioPlayer
    {
        private readonly Song song;

        private readonly SongMixer songMixer;
        private readonly Thread scenarioThread;

        private readonly ScenarioScrObject mainScenario;
        private readonly ScenarioScrObject orientScenario;
        private readonly List<EventScenarioData> muteScenarios;
        private readonly List<EventScenarioData> expressionScenarios;
        private readonly List<EventScenarioData> lightScenarios;

        private bool stopRequested = false;
        private bool seeked = false;

        public ScenarioPlayer(Song selectedSong)
        {
            song = selectedSong;

            songMixer = song.Scenario.Configuration.SongMixer;

            mainScenario = song.Scenario.MainScenario;
            orientScenario = song.Scenario.OrientationScenario;
            muteScenarios = song.Scenario.MuteScenarios;
            expressionScenarios = song.Scenario.ExpressionScenarios;
            lightScenarios = song.Scenario.LightScenarios;

            scenarioThread = new Thread(DoScenarioPlayback);
        }

        public int Idol { get; set; } = 0;

        public int Layer { get; set; } = 0;

        public void Start()
        {
            scenarioThread.Start();
        }

        public void Stop()
        {
            stopRequested = true;
        }

        public void UpdatePosition()
        {
            seeked = true;
        }

        private void DoScenarioPlayback()
        {
            int mainScenarioIndex = 0;
            int orientScenarioIndex = 0;
            int muteIndex = 0;
            int expressionScenarioIndex = 0;

            double secondsElapsed = 0;

            EventScenarioData currentMainScenario;
            EventScenarioData currentOrientScenario;
            EventScenarioData currentMuteScenario;
            EventScenarioData currentExpressionScenario;

            currentMuteScenario = muteScenarios[muteIndex];
            currentExpressionScenario = expressionScenarios[expressionScenarioIndex];
            currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
            currentOrientScenario = orientScenario.Scenario[orientScenarioIndex];

            while (!songMixer.HasEnded && !stopRequested)
            {
                if (seeked)
                {
                    muteIndex = 0;
                    expressionScenarioIndex = 0;
                    mainScenarioIndex = 0;
                    orientScenarioIndex = 0;

                    currentMuteScenario = muteScenarios[muteIndex];
                    currentExpressionScenario = expressionScenarios[expressionScenarioIndex];
                    currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
                    currentOrientScenario = orientScenario.Scenario[orientScenarioIndex];

                    secondsElapsed = songMixer.CurrentTime.TotalSeconds;

                    if (secondsElapsed > 0)
                    {
                        while (secondsElapsed >= currentMuteScenario.AbsTime)
                        {
                            if (muteIndex < muteScenarios.Count - 1) muteIndex++;
                            else break;
                            currentMuteScenario = muteScenarios[muteIndex];
                        }

                        while (secondsElapsed >= currentExpressionScenario.AbsTime)
                        {
                            if (expressionScenarioIndex < expressionScenarios.Count - 1) expressionScenarioIndex++;
                            else break;
                            currentExpressionScenario = expressionScenarios[expressionScenarioIndex];
                        }

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
                    }

                    seeked = false;
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

                    if (muteIndex < muteScenarios.Count - 1) muteIndex++;
                    else break;
                    currentMuteScenario = muteScenarios[muteIndex];
                }

                while (secondsElapsed >= currentExpressionScenario.AbsTime)
                {
                    if (currentExpressionScenario.Type == ScenarioType.Expression)
                    {
                        if (currentExpressionScenario.Idol == Idol && currentExpressionScenario.Layer == Layer)
                            ExpressionChanged?.Invoke(currentExpressionScenario.Param, currentExpressionScenario.EyeClose == 1);
                    }

                    if (expressionScenarioIndex < expressionScenarios.Count - 1) expressionScenarioIndex++;
                    else break;
                    currentExpressionScenario = expressionScenarios[expressionScenarioIndex];
                }

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

                    if (currentMainScenario.Type == ScenarioType.Lights)
                    {
                        if (currentMainScenario.Layer == Layer)
                        {
                            LightsChanged?.Invoke(new LightPayload
                            {
                                Color = currentMainScenario.Col,
                                Color2 = currentMainScenario.Col2,
                                Color3 = currentMainScenario.Col3,
                                Duration = currentMainScenario.AbsEndTime - currentMainScenario.AbsTime,
                                Target = currentMainScenario.Target
                            });
                        }
                    }

                    ScenarioTriggered?.Invoke((int)currentMainScenario.Type);

                    if (mainScenarioIndex < mainScenario.Scenario.Count - 1) mainScenarioIndex++;
                    else break;
                    currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
                }

                while (secondsElapsed >= currentOrientScenario.AbsTime)
                {
                    ScenarioTriggered?.Invoke((int)currentOrientScenario.Type);

                    if (orientScenarioIndex < orientScenario.Scenario.Count - 1) orientScenarioIndex++;
                    else break;
                    currentOrientScenario = orientScenario.Scenario[orientScenarioIndex];
                }

                Thread.Sleep(1);
            }

            songMixer.Dispose();

            SongStopped?.Invoke();
        }

        public delegate void ExpressionChangedEventHandler(int expressionId, bool eyeClose);
        public event ExpressionChangedEventHandler ExpressionChanged;

        public delegate void LipSyncChangedEventHandler(int lipSyncId);
        public event LipSyncChangedEventHandler LipSyncChanged;

        public delegate void LyricsChangedEventHandler(string lyrics);
        public event LyricsChangedEventHandler LyricsChanged;

        public delegate void MuteChangedEventHandler(byte[] mutes);
        public event MuteChangedEventHandler MuteChanged;

        public delegate void LightsChangedEventHandler(LightPayload lightPayload);
        public event LightsChangedEventHandler LightsChanged;

        public delegate void SongStoppedEventHandler();
        public event SongStoppedEventHandler SongStopped;

        public delegate void ScenarioTriggeredEventHandler(int type);
        public event ScenarioTriggeredEventHandler ScenarioTriggered;
    }
}
