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

namespace MirishitaMusicPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            if (args.Length > 0) Directory.SetCurrentDirectory(args[0]);

            string filesPath = Directory.GetCurrentDirectory();
            string songID;

            do
            {
                Console.CursorVisible = true;
                Console.Write("Enter song ID: ");
                songID = Console.ReadLine();

                if (Directory.GetFiles(filesPath, $"*_{songID}*").Length > 0)
                    break;

                Console.Write("Unable to find song with the specified ID.");
                Console.ReadKey(true);
            } while (true);

            AssetsManager assetsManager = new();

            ScenarioLoader scenarios = new(assetsManager, filesPath, songID);
            AudioLoader audioLoader = new(assetsManager, scenarios.MuteScenarios, filesPath, songID);

            ScenarioScrObject mainScenario = scenarios.MainScenario;
            ScenarioScrObject orientScenario = scenarios.OrientationScenario;

            List<EventScenarioData> muteScenarios = mainScenario.Scenario.Where(s => s.Type == ScenarioType.Mute).ToList();
            if (muteScenarios.Count < 1) muteScenarios = orientScenario.Scenario.Where(s => s.Type == ScenarioType.Mute).ToList();

            Idol[] order = null;
            if (audioLoader.Singers != null)
            {
                Console.WriteLine();
                Console.WriteLine("Available singers:");
                for (int i = 0; i < audioLoader.Singers.Length; i++)
                {
                    Console.WriteLine(" " + i + " " + audioLoader.Singers[i].IdolFirstName);
                }

                Console.WriteLine();

                int selectedVoices = 0;
                Console.Write("Select in order (" + audioLoader.Singers.Length + " max)" + ": ");
                string[] orderInput = Console.ReadLine().Trim().Split(' ');
                order = new Idol[orderInput.Length];
                selectedVoices = orderInput.Length;

                for (int i = 0; i < orderInput.Length; i++)
                {
                    int selectionIndex = int.Parse(orderInput[i]);
                    order[i] = audioLoader.Singers[selectionIndex];
                }
            }

            audioLoader.Setup(order);

            SongMixer voiceMixer = audioLoader.SongMixer;
            WasapiOut outputDevice = audioLoader.OutputDevice;

            float ticksPerSecond = scenarios.TicksPerSecond;

            Console.WriteLine();
            Console.Write("Press any key to play...");

            Console.ReadKey();
            Console.WriteLine('\n');
            Console.CursorVisible = false;

            int[] tracks = new int[]
            {
                //1, 2,                   // 2Mix
                //3, 4,                   // 2Mix+
                //9, 10, 11, 12,          // 4Mix
                25, 26, 27, 28, 29, 30, // 6Mix
                //31, 32, 33, 34, 35, 36, // Million Mix
            };

            int[] types = new int[]
            {
                41,
                53,
                54,
                55,
            };

            outputDevice.Play();
            //rhythm.Tap();

            bool paused = false;

            int cursorTopBase = Console.CursorTop;
            int timeCursorTop = cursorTopBase;
            int voiceCursorTop = timeCursorTop + 2;
            int eyesCursorTop = voiceCursorTop + 2;
            int mouthCursorTop = eyesCursorTop + 6;
            int lyricsCursorTop = mouthCursorTop + 7;
            int rhythmCursorTop = -1;

            bool queueErase = false;

            int muteIndex = 0;
            int orientScenarioIndex = 0;
            int mainScenarioIndex = 0;
            int eventIndex = 0;

            //if (voiceCount < 1)
            //{
            //    Console.CursorLeft = 0;
            //    Console.CursorTop = voiceCursorTop;
            //    Console.Write(" [ VOICE CONTROL UNAVAILABLE ]");
            //}

            double secondsElapsed = 0;

            while (!voiceMixer.HasEnded)
            {
                bool seeked = false;
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.B:
                            voiceMixer.MuteBackground = !voiceMixer.MuteBackground;
                            break;
                        case ConsoleKey.R:
                            muteIndex = 0;
                            orientScenarioIndex = 0;
                            mainScenarioIndex = 0;
                            voiceMixer.Reset();
                            break;
                        case ConsoleKey.Spacebar:
                            paused = !paused;
                            break;
                        case ConsoleKey.LeftArrow:
                            voiceMixer.Seek(-3.0f);
                            seeked = true;
                            break;
                        case ConsoleKey.RightArrow:
                            voiceMixer.Seek(3.0f);
                            seeked = true;
                            break;
                        default:
                            break;
                    }

                    if (paused)
                        outputDevice.Pause();
                    else
                        outputDevice.Play();
                }

                secondsElapsed = voiceMixer.CurrentTime.TotalSeconds;

                if (seeked)
                {
                    muteIndex = 0;
                    orientScenarioIndex = 0;
                    mainScenarioIndex = 0;

                    secondsElapsed = voiceMixer.CurrentTime.TotalSeconds;

                    while (secondsElapsed >= (muteScenarios[muteIndex].Tick / ticksPerSecond))
                    {
                        if (muteIndex < muteScenarios.Count - 1) muteIndex++;
                        else break;
                    }
                    while (secondsElapsed >= (orientScenario.Scenario[orientScenarioIndex].Tick / ticksPerSecond))
                    {
                        if (orientScenarioIndex < orientScenario.Scenario.Count - 1) orientScenarioIndex++;
                        else break;
                    }
                    while (secondsElapsed >= (mainScenario.Scenario[mainScenarioIndex].Tick / ticksPerSecond))
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
                Console.WriteLine($" {secondsElapsed:f4}s elapsed");

                EventScenarioData currentMuteScenario = muteScenarios[muteIndex];
                if (secondsElapsed >= (currentMuteScenario.Tick / ticksPerSecond))
                {
                    voiceMixer.VoiceControl = currentMuteScenario.Mute;

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

                EventScenarioData currentOrientScenario = orientScenario.Scenario[orientScenarioIndex];
                while (secondsElapsed >= (currentOrientScenario.Tick / ticksPerSecond))
                {
                    EyesVisualizer.Render(currentOrientScenario, eyesCursorTop);

                    if (orientScenarioIndex < orientScenario.Scenario.Count - 1) orientScenarioIndex++;
                    else break;
                    currentOrientScenario = orientScenario.Scenario[orientScenarioIndex];
                }

                EventScenarioData currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
                while (secondsElapsed >= (currentMainScenario.Tick / ticksPerSecond))
                {
                    MouthVisualizer.Render(currentMainScenario, mouthCursorTop);

                    if (currentMainScenario.Type == ScenarioType.ShowLyrics || currentMainScenario.Type == ScenarioType.HideLyrics)
                    {
                        Console.CursorLeft = 0;
                        Console.CursorTop = lyricsCursorTop;

                        StringBuilder lyricsStringBuilder = new();
                        lyricsStringBuilder.Append(' ', Console.WindowWidth - 2);
                        Console.Write(lyricsStringBuilder.ToString());

                        lyricsStringBuilder.Clear();
                        lyricsStringBuilder.Append(' ' + currentMainScenario.Str);

                        Console.CursorLeft = 0;
                        Console.CursorTop = lyricsCursorTop;
                        Console.Write(lyricsStringBuilder.ToString());
                    }

                    if (mainScenarioIndex < mainScenario.Scenario.Count - 1) mainScenarioIndex++;
                    else break;
                    currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
                }

                //Event currentEvent = fumen.Events[eventIndex];
                //while (secondsElapsed >= (currentEvent.Tick / ticksPerSecond))
                //{
                //    if (MatchesTrack(currentEvent.Track, tracks))
                //    {
                //        if (currentEvent.EndPosX >= 0 && currentEvent.Type >= 0)
                //        {
                //            if (rhythmCursorTop < 0) rhythmCursorTop = Console.CursorTop;
                //            Console.CursorTop = rhythmCursorTop;

                //            if (queueErase)
                //            {
                //                Console.CursorTop = rhythmCursorTop;
                //                Console.CursorLeft = 0;
                //                StringBuilder erase = new();

                //                for (int i = 0; i < 6; i++)
                //                {
                //                    erase.Append(" ( ) ");
                //                }
                //                Console.Write(erase.ToString());
                //                Console.CursorLeft = 0;

                //                queueErase = false;
                //            }

                //            Console.CursorLeft = (int)currentEvent.EndPosX * 5;
                //            //Console.Write(currentEvent.startPosx);
                //            switch (currentEvent.Type)
                //            {
                //                case 0:
                //                    Console.Write(" (o)  ");
                //                    rhythm.Tap();
                //                    break;
                //                case 1:
                //                    Console.Write(" (@) ");
                //                    rhythm.Tap();
                //                    break;
                //                case 2:
                //                    Console.Write(" (◄) ");
                //                    rhythm.Flick();
                //                    break;
                //                case 3:
                //                    Console.Write(" (▲) ");
                //                    rhythm.Flick();
                //                    break;
                //                case 4:
                //                    Console.Write(" (►) ");
                //                    rhythm.Flick();
                //                    break;
                //                case 5:
                //                    Console.Write(" (|) ");
                //                    rhythm.Tap();
                //                    break;
                //                case 6:
                //                    Console.Write(" (%) ");
                //                    rhythm.Tap();
                //                    break;
                //                case 7:
                //                    Console.Write(" (█) ");
                //                    rhythm.Tap();
                //                    break;
                //                case 8:
                //                    Console.Write(" (A)  (P)  (P)  (E)  (A)  (L) ");
                //                    break;
                //                default:
                //                    Console.Write(" " + currentEvent.Type + " ");
                //                    break;
                //            }
                //        }
                //    }

                //    Event nextEvent = fumen.Events[eventIndex + 1];
                //    if (nextEvent.AbsoluteTime != currentEvent.AbsoluteTime)
                //    {
                //        //Console.WriteLine();
                //        queueErase = true;
                //    }

                //    if (eventIndex < fumen.Events.Length - 1) eventIndex++;
                //    currentEvent = fumen.Events[eventIndex];
                //}

                Thread.Sleep(1);
            }

            Console.Clear();

            //rhythm.Stop();
        }
    }
}