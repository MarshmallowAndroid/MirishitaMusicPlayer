using System;
using System.IO;
using Newtonsoft.Json;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Text;
using System.Linq;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using AssetStudio;
using MirishitaMusicPlayer.Imas;
using MirishitaMusicPlayer.Audio;
using System.Diagnostics;

namespace MirishitaMusicPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            string filesPath;

            if (args.Length > 0) filesPath = args[0];
            else filesPath = Directory.GetCurrentDirectory();

            string songID;

            do
            {
                Console.CursorVisible = true;
                Console.Write("Enter song ID: ");
                songID = Console.ReadLine();

                if (Directory.GetFiles(filesPath, $"*_{songID}*").Length > 0)
                    break;

                Console.WriteLine("Unable to find song with the specified ID.");
                Console.ReadKey(true);
            } while (true);

            AssetsManager assetsManager = new();

            ScenarioLoader scenarios = new(assetsManager, filesPath, songID);
            AudioLoader audioLoader = new(assetsManager, scenarios.MuteScenarios, filesPath, songID);

            RhythmPlayer rhythm = new(
                @"D:\mirishita\EXPORT\TextAsset\rhy_se_05_tap.wav.bytes",
                @"D:\mirishita\EXPORT\TextAsset\rhy_se_05_flick.wav.bytes");

            ScenarioScrObject mainScenario = scenarios.MainScenario;
            ScenarioScrObject orientScenario = scenarios.OrientationScenario;

            NoteScrObject notes = scenarios.Notes;
            List<EventConductorData> conductors = notes.Ct.ToList();

            List<EventScenarioData> muteScenarios = scenarios.MuteScenarios;

            WaveOutEvent outputDevice = audioLoader.OutputDevice;

            //float ticksPerSecond = scenarios.TicksPerSecond;

            int[] tracks = new int[]
            {
                //1, 2,                   // 2Mix
                //3, 4,                   // 2Mix+
                //9, 10, 11, 12,          // 4Mix
                //25, 26, 27, 28, 29, 30, // 6Mix
                31, 32, 33, 34, 35, 36, // Million Mix
            };

            int[] types = new int[]
            {
                41,
                53,
                54,
                55,
            };

            //outputDevice.Play();
            //rhythm.Tap();

            bool quit = false;
            bool setup = false;

            int eventIndex = 0;

            double secondsElapsed = 0;

            while (!quit)
            {
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

                    audioLoader.Setup(order, scenarios.VoiceCount > 5, rhythm);
                }


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
                int rhythmCursorTop = lyricsCursorTop + 5;

                bool queueErase = false;

                double ticksPerSecond = conductors[0].TicksPerSecond;

                int conductorIndex = 0;
                int muteIndex = 0;
                int orientScenarioIndex = 0;
                int mainScenarioIndex = 0;

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
                                orientScenarioIndex = 0;
                                mainScenarioIndex = 0;
                                songMixer.Reset();
                                break;
                            case ConsoleKey.S:
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

                    secondsElapsed = songMixer.CurrentTime.TotalSeconds + 0.03;

                    if (setup)
                    {
                        setup = false;
                        break;
                    }

                    if (seeked)
                    {
                        muteIndex = 0;
                        orientScenarioIndex = 0;
                        mainScenarioIndex = 0;

                        secondsElapsed = songMixer.CurrentTime.TotalSeconds;

                        while (secondsElapsed >= muteScenarios[muteIndex].AbsTime)
                        {
                            if (muteIndex < muteScenarios.Count - 1) muteIndex++;
                            else break;
                        }
                        while (secondsElapsed >= orientScenario.Scenario[orientScenarioIndex].AbsTime)
                        {
                            if (orientScenarioIndex < orientScenario.Scenario.Count - 1) orientScenarioIndex++;
                            else break;
                        }
                        while (secondsElapsed >= mainScenario.Scenario[mainScenarioIndex].AbsTime)
                        {
                            if (mainScenarioIndex < mainScenario.Scenario.Count - 1) mainScenarioIndex++;
                            else break;
                        }

                        muteIndex--;
                        orientScenarioIndex--;
                        mainScenarioIndex--;

                        seeked = false;
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

                        //Console.Write(" Accuracy: " + (secondsElapsed - currentMuteScenario.AbsTime) + "s");
                        //Console.Write("                                     ");

                        if (muteIndex < muteScenarios.Count - 1) muteIndex++;
                    }

                    EventConductorData currentConductor = conductors[conductorIndex];
                    if (secondsElapsed >= currentConductor.AbsTime)
                    {
                        ticksPerSecond = currentConductor.TicksPerSecond;

                        if (conductorIndex < conductors.Count - 1) conductorIndex++;
                        currentConductor = conductors[conductorIndex];
                    }

                    EventScenarioData currentOrientScenario = orientScenario.Scenario[orientScenarioIndex];
                    //while (secondsElapsed >= currentOrientScenario.Tick / ticksPerSecond)
                    while (secondsElapsed >= currentOrientScenario.AbsTime)
                    {
                        EyesVisualizer.Render(currentOrientScenario, eyesCursorTop);

                        if (orientScenarioIndex < orientScenario.Scenario.Count - 1) orientScenarioIndex++;
                        else break;
                        currentOrientScenario = orientScenario.Scenario[orientScenarioIndex];
                    }

                    EventScenarioData currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
                    //while (secondsElapsed >= currentMainScenario.Tick / ticksPerSecond)
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

                    //continue;

                    #region "Rhythm game" code
                    //EventNoteData currentEvent = notes.Evts[eventIndex];
                    //while (secondsElapsed >= currentEvent.AbsTime)
                    ////while (secondsElapsed >= (currentEvent.Tick / ticksPerSecond))
                    //{
                    //    if (MatchesTrack(currentEvent.Track, tracks))
                    //    {
                    //        if (currentEvent.EndPosX >= 0 && currentEvent.Type >= 0)
                    //        {
                    //            //if (rhythmCursorTop < 0) rhythmCursorTop = Console.CursorTop;
                    //            //Console.CursorTop = rhythmCursorTop;

                    //            if (queueErase)
                    //            {
                    //                Console.CursorTop = rhythmCursorTop;
                    //                Console.CursorLeft = 0;
                    //                StringBuilder erase = new();

                    //                for (int i = 0; i < 6; i++)
                    //                {
                    //                    erase.Append(" [ ] ");
                    //                }
                    //                Console.Write(erase.ToString());
                    //                Console.CursorLeft = 0;

                    //                queueErase = false;
                    //            }

                    //            Console.CursorLeft = (int)currentEvent.EndPosX * 5;
                    //            //Console.Write(currentEvent.EndPosX);
                    //            switch (currentEvent.Type)
                    //            {
                    //                case 0:
                    //                    Console.Write(" [o] ");
                    //                    rhythm.Tap();
                    //                    break;
                    //                case 1:
                    //                    Console.Write(" [@] ");
                    //                    rhythm.Tap();
                    //                    break;
                    //                case 2:
                    //                    Console.Write(" [◄] ");
                    //                    rhythm.Flick();
                    //                    break;
                    //                case 3:
                    //                    Console.Write(" [▲] ");
                    //                    rhythm.Flick();
                    //                    break;
                    //                case 4:
                    //                    Console.Write(" [►] ");
                    //                    rhythm.Flick();
                    //                    break;
                    //                case 5:
                    //                    Console.Write(" [|] ");
                    //                    rhythm.Tap();
                    //                    break;
                    //                case 6:
                    //                    Console.Write(" [%] ");
                    //                    rhythm.Tap();
                    //                    break;
                    //                case 7:
                    //                    Console.Write(" [█] ");
                    //                    rhythm.Tap();
                    //                    break;
                    //                case 8:
                    //                    Console.Write(" [A]  [P]  [P]  [E]  [A]  [L] ");
                    //                    break;
                    //                default:
                    //                    Console.Write(" " + currentEvent.Type + " ");
                    //                    break;
                    //            }
                    //        }
                    //    }

                    //    EventNoteData nextEvent = notes.Evts[eventIndex + 1];
                    //    if (nextEvent.AbsTime != currentEvent.AbsTime)
                    //    {
                    //        Console.WriteLine();
                    //        queueErase = true;
                    //    }

                    //    if (eventIndex < notes.Evts.Count - 1) eventIndex++;
                    //    currentEvent = notes.Evts[eventIndex];
                    //}
                    #endregion

                    Thread.Sleep(1);
                }

                Console.Clear();
            }

            //rhythm.Stop();
        }

        static bool MatchesTrack(int track, int[] tracks)
        {
            for (int i = 0; i < tracks.Length; i++)
            {
                if (track == tracks[i]) return true;
            }

            return false;
        }
    }
}