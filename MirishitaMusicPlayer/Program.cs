using AssetStudio;
using MirishitaMusicPlayer.Audio;
using MirishitaMusicPlayer.Forms;
using MirishitaMusicPlayer.Imas;
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
            bool setup = false;

            while (!quit)
            {
                SongSelectForm songSelectForm = new();
                songSelectForm.ShowDialog();

                string songID = songSelectForm.ResultSongID ?? "";
                if (songID == "") return;

                Console.Title = songID;

                string filesPath = "Cache\\Songs";

                AssetsManager assetsManager = new();

                ScenarioLoader scenarios = new(assetsManager, filesPath, songID);
                AudioLoader audioLoader = new(assetsManager, scenarios.MuteScenarios, filesPath, songID);

                ScenarioScrObject mainScenario = scenarios.MainScenario;

                List<EventScenarioData> expressionScenarios = scenarios.ExpressionScenarios;
                List<EventScenarioData> muteScenarios = scenarios.MuteScenarios;

                Idol[] order = null;
                if (audioLoader.Singers != null)
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Available singers:");
                    for (int i = 0; i < audioLoader.Singers.Length; i++)
                    {
                        Console.WriteLine(" " + i + " " + audioLoader.Singers[i].IdolFirstName);
                    }

                    Console.WriteLine();

                    int validVoiceCount = Math.Min(scenarios.VoiceCount, 5);

                    Console.Write("Select in order (" + validVoiceCount + " max)" + ": ");
                    string[] orderInput = Console.ReadLine().Trim().Split(' ');
                    int validOrderCount = Math.Min(orderInput.Length, 5);
                    order = new Idol[validOrderCount];

                    for (int i = 0; i < validOrderCount; i++)
                    {
                        int selectionIndex = int.Parse(orderInput[i]);
                        order[i] = audioLoader.Singers[selectionIndex];
                    }
                }

                audioLoader.Setup(order, scenarios.VoiceCount > 5);

                SongMixer songMixer = audioLoader.SongMixer;

                Console.WriteLine();
                Console.Write("Press any key to play...");

                Console.ReadKey();
                Console.WriteLine('\n');
                Console.CursorVisible = false;

                int cursorTopBase = Console.CursorTop;
                int timeCursorTop = cursorTopBase;
                int voiceCursorTop = timeCursorTop + 2;
                int eyesCursorTop = voiceCursorTop + 2;
                int mouthCursorTop = eyesCursorTop + 6;
                int lyricsCursorTop = mouthCursorTop + 7;

                int mainScenarioIndex = 0;
                int expressionScenarioIndex = 0;
                int muteIndex = 0;

                WaveOutEvent outputDevice = audioLoader.OutputDevice;
                outputDevice.Play();

                while (!songMixer.HasEnded && !quit)
                {
                    bool seeked = false;
                    if (Console.KeyAvailable)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.V:
                                songMixer.MuteVoices = !songMixer.MuteVoices;
                                break;

                            case ConsoleKey.B:
                                songMixer.MuteBackground = !songMixer.MuteBackground;
                                break;

                            case ConsoleKey.Q:
                                outputDevice.Stop();
                                quit = true;
                                break;

                            case ConsoleKey.R:
                                muteIndex = 0;
                                expressionScenarioIndex = 0;
                                mainScenarioIndex = 0;
                                songMixer.Reset();
                                break;

                            case ConsoleKey.S:
                                outputDevice.Stop();
                                setup = true;
                                break;

                            case ConsoleKey.Spacebar:
                                if (outputDevice.PlaybackState == PlaybackState.Playing)
                                    outputDevice.Pause();
                                else if (outputDevice.PlaybackState == PlaybackState.Paused)
                                    outputDevice.Play();
                                break;

                            case ConsoleKey.LeftArrow:
                                songMixer.Seek(-3.0f);
                                seeked = true;
                                break;

                            case ConsoleKey.RightArrow:
                                songMixer.Seek(3.0f);
                                seeked = true;
                                break;

                            default:
                                break;
                        }
                    }

                    double secondsElapsed = songMixer.CurrentTime.TotalSeconds;

                    if (setup)
                    {
                        setup = false;
                        break;
                    }

                    if (seeked)
                    {
                        muteIndex = 0;
                        expressionScenarioIndex = 0;
                        mainScenarioIndex = 0;

                        secondsElapsed = songMixer.CurrentTime.TotalSeconds;

                        while (secondsElapsed >= muteScenarios[muteIndex].AbsTime)
                        {
                            if (muteIndex < muteScenarios.Count - 1) muteIndex++;
                            else break;
                        }
                        while (secondsElapsed >= expressionScenarios[expressionScenarioIndex].AbsTime)
                        {
                            if (expressionScenarioIndex < expressionScenarios.Count - 1) expressionScenarioIndex++;
                            else break;
                        }
                        while (secondsElapsed >= mainScenario.Scenario[mainScenarioIndex].AbsTime)
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

                    Console.CursorLeft = 0;
                    Console.CursorTop = timeCursorTop;
                    Console.WriteLine($" {secondsElapsed:f4}s elapsed    ");

                    EventScenarioData currentMuteScenario = muteScenarios[muteIndex];
                    if (secondsElapsed >= currentMuteScenario.AbsTime)
                    {
                        Console.CursorLeft = 0;
                        Console.CursorTop = voiceCursorTop;

                        for (int i = 0; i < currentMuteScenario.Mute.Length; i++)
                        {
                            if (currentMuteScenario.Mute[i] == 1)
                                Console.Write($" [{i + 1}] ");
                            else Console.Write(" [ ] ");
                        }

                        if (muteIndex < muteScenarios.Count - 1) muteIndex++;
                    }

                    EventScenarioData currentOrientScenario = expressionScenarios[expressionScenarioIndex];
                    while (secondsElapsed >= currentOrientScenario.AbsTime)
                    {
                        EyesVisualizer.Render(currentOrientScenario, eyesCursorTop);

                        if (expressionScenarioIndex < expressionScenarios.Count - 1) expressionScenarioIndex++;
                        else break;
                        currentOrientScenario = expressionScenarios[expressionScenarioIndex];
                    }

                    EventScenarioData currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
                    while (secondsElapsed >= currentMainScenario.AbsTime)
                    {
                        MouthVisualizer.Render(currentMainScenario, mouthCursorTop);

                        if (currentMainScenario.Type == ScenarioType.ShowLyrics || currentMainScenario.Type == ScenarioType.HideLyrics)
                        {
                            Console.CursorLeft = 0;
                            Console.CursorTop = lyricsCursorTop;

                            StringBuilder lyricsStringBuilder = new();
                            lyricsStringBuilder.Append(' ', Console.WindowWidth - 2);
                            lyricsStringBuilder.Append('\n', 1);
                            lyricsStringBuilder.Append(' ', Console.WindowWidth - 2);
                            Console.Write(lyricsStringBuilder.ToString());

                            lyricsStringBuilder.Clear();
                            lyricsStringBuilder.Append(' ' + currentMainScenario.Str);
                            //lyricsStringBuilder.Append(" BPM: " + (int)(currentMainScenario.Tick / currentMainScenario.AbsTime / 8));

                            Console.CursorLeft = 0;
                            Console.CursorTop = lyricsCursorTop;
                            Console.Write(lyricsStringBuilder.ToString());
                        }

                        if (mainScenarioIndex < mainScenario.Scenario.Count - 1) mainScenarioIndex++;
                        else break;
                        currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
                    }

                    Thread.Sleep(1);
                }

                Console.Clear();
            }
        }
    }
}