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
    internal class ScenarioPlayer
    {
        private readonly Song song;

        private readonly SongMixer songMixer;
        private readonly Thread scenarioThread;

        private readonly ScenarioScrObject mainScenario;
        private readonly List<EventScenarioData> expressionScenarios;
        private readonly List<EventScenarioData> muteScenarios;

        bool shouldStop = false;

        public ScenarioPlayer(Song selectedSong)
        {
            song = selectedSong;
            
            songMixer = song.Scenario.Configuration.SongMixer;

            mainScenario = song.Scenario.MainScenario;
            expressionScenarios = song.Scenario.ExpressionScenarios;
            muteScenarios = song.Scenario.MuteScenarios;

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
            shouldStop = true;
        }

        private void DoScenarioPlayback()
        {
            int mainScenarioIndex = 0;
            int expressionScenarioIndex = 0;
            int muteIndex = 0;

            double secondsElapsed = 0;

            while (!songMixer.HasEnded)
            {
                if (shouldStop) break;

                if (secondsElapsed > songMixer.CurrentTime.TotalSeconds)
                {
                    muteIndex = 0;
                    expressionScenarioIndex = 0;
                    mainScenarioIndex = 0;

                    secondsElapsed = songMixer.CurrentTime.TotalSeconds;

                    while (secondsElapsed > muteScenarios[muteIndex].AbsTime)
                    {
                        if (muteIndex < muteScenarios.Count - 1) muteIndex++;
                        else break;
                    }
                    while (secondsElapsed > expressionScenarios[expressionScenarioIndex].AbsTime)
                    {
                        if (expressionScenarioIndex < expressionScenarios.Count - 1) expressionScenarioIndex++;
                        else break;
                    }
                    while (secondsElapsed > mainScenario.Scenario[mainScenarioIndex].AbsTime)
                    {
                        if (mainScenarioIndex < mainScenario.Scenario.Count - 1) mainScenarioIndex++;
                        else break;
                    }

                    if (muteIndex > 0)
                        muteIndex--;
                    if (expressionScenarioIndex > 0)
                        expressionScenarioIndex--;
                    if (mainScenarioIndex > 0)
                        mainScenarioIndex--;
                }

                secondsElapsed = songMixer.CurrentTime.TotalSeconds;

                EventScenarioData currentMuteScenario = muteScenarios[muteIndex];
                EventScenarioData targetMuteScenario = null;
                while (secondsElapsed >= currentMuteScenario.AbsTime)
                {
                    if (currentMuteScenario.Layer == 0)
                        targetMuteScenario = currentMuteScenario;

                    if (muteIndex < muteScenarios.Count - 1) muteIndex++;
                    else break;
                    currentMuteScenario = muteScenarios[muteIndex];
                }
                if (targetMuteScenario != null)
                    MuteChanged?.Invoke(targetMuteScenario.Mute);

                EventScenarioData currentExpressionScenario = expressionScenarios[expressionScenarioIndex];
                EventScenarioData targetExpressionScenario = null;
                while (secondsElapsed >= currentExpressionScenario.AbsTime)
                {
                    if (currentExpressionScenario.Type == ScenarioType.Expression)
                    {
                        if (currentExpressionScenario.Idol == Idol && currentExpressionScenario.Layer == Layer)
                            targetExpressionScenario = currentExpressionScenario;
                    }

                    if (expressionScenarioIndex < expressionScenarios.Count - 1) expressionScenarioIndex++;
                    else break;
                    currentExpressionScenario = expressionScenarios[expressionScenarioIndex];
                }
                if (targetExpressionScenario != null)
                    ExpressionChanged?.Invoke(targetExpressionScenario.Param, targetExpressionScenario.EyeClose == 1);

                EventScenarioData currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
                EventScenarioData targetLipSyncScenario = null;
                EventScenarioData targetLyricsScenario = null;
                while (secondsElapsed >= currentMainScenario.AbsTime)
                {
                    if (currentMainScenario.Type == ScenarioType.LipSync)
                    {
                        if (currentMainScenario.Idol == Idol && currentMainScenario.Layer == Layer)
                            targetLipSyncScenario = currentMainScenario;
                    }

                    if (currentMainScenario.Type == ScenarioType.ShowLyrics || currentMainScenario.Type == ScenarioType.HideLyrics)
                    {
                        if (currentMainScenario.Layer == Layer)
                            targetLyricsScenario = currentMainScenario;
                    }

                    if (mainScenarioIndex < mainScenario.Scenario.Count - 1) mainScenarioIndex++;
                    else break;
                    currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
                }
                if (targetLipSyncScenario != null)
                    LipSyncChanged?.Invoke(targetLipSyncScenario.Param);
                if (targetLyricsScenario != null)
                    LyricsChanged?.Invoke(targetLyricsScenario.Str);

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

        public delegate void SongStoppedEventHandler();
        public event SongStoppedEventHandler SongStopped;
    }
}
