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

            Console.CursorVisible = true;
            Console.Write("Enter song ID: ");
            string songID = Console.ReadLine();
            Console.WriteLine();

            if (Directory.GetFiles(filesPath, $"*_{songID}*").Length < 1)
            {
                Console.Write("Unable to find song with the specified ID.");
                Console.ReadKey(true);
                return;
            }

            AssetsManager assetsManager = new();

            // Load scenarios and notes first
            string scenarioPath = Path.Combine(filesPath, $"scrobj_{songID}.unity3d");
            assetsManager.LoadFiles(new[] { scenarioPath });

            ScenarioScrObject mainScenario = null;
            ScenarioScrObject yokoScenario = null; // Landscape mode
            ScenarioScrObject tateScenario = null; // Portrait mode
            NoteScrObject notes = null; // Tap notes and song timing via EventConductor

            foreach (var file in assetsManager.assetsFileList)
            {
                foreach (var gameObject in file.Objects)
                {
                    if (gameObject.type == ClassIDType.MonoBehaviour)
                    {
                        MonoBehaviour monoBehaviour = (MonoBehaviour)gameObject;
                        if (monoBehaviour.m_Name == $"{songID}_scenario_sobj")
                            mainScenario = new(monoBehaviour);
                        else if (monoBehaviour.m_Name == $"{songID}_scenario_yoko_sobj")
                            yokoScenario = new(monoBehaviour);
                        else if (monoBehaviour.m_Name == $"{songID}_scenario_tate_sobj")
                            tateScenario = new(monoBehaviour);
                        else if (monoBehaviour.m_Name == $"{songID}_fumen_sobj")
                            notes = new(monoBehaviour);
                    }
                }
            }

            //List<object> exist = new();
            //foreach (var scenario in tateScenario.Scenario.Where(s => s.Type == ScenarioType.Expression))
            //{
            //    object target = scenario.Param;
            //    if (!exist.Contains(target))
            //    {
            //        exist.Add(target);
            //        Console.WriteLine(target);
            //    }
            //}
            //Console.WriteLine();

            assetsManager.Clear();

            ScenarioScrObject orientScenario = tateScenario;

            List<EventScenarioData> muteScenarios = mainScenario.Scenario.Where(s => s.Type == ScenarioType.Mute).ToList();
            if (muteScenarios.Count < 1) muteScenarios = orientScenario.Scenario.Where(s => s.Type == ScenarioType.Mute).ToList();

            int voiceCount = 0;
            // Get the first mute event so we know how many voices are going to be singing
            EventScenarioData firstMuteEvent = muteScenarios[0];
            if (firstMuteEvent != null)
                voiceCount = firstMuteEvent.Mute.Length;

            // Get all the audio files for the song
            string[] audioFiles = Directory.GetFiles(filesPath, $"song3_{songID}*");

            Regex normalRegex = new($"song3_{songID}.acb.unity3d");
            Regex bgmRegex = new($"song3_{songID}_bgm.acb.unity3d");
            Regex voiceRegex = new($"song3_{songID}_([0-9]{{3}})([a-z]{{3}}).acb.unity3d");
            Regex extraVoiceRegex = new($"song3_{songID}_ex.acb.unity3d");

            List<string> voicePaths = new();
            string bgmPath = "";
            string extraVoicePath = "";

            foreach (var file in audioFiles)
            {
                if (normalRegex.IsMatch(file) || bgmRegex.IsMatch(file))
                    bgmPath = file;
                if (voiceRegex.IsMatch(file))
                    voicePaths.Add(file);
                else if (extraVoiceRegex.IsMatch(file))
                    extraVoicePath = file;
            }

            List<string> loadPaths = new();
            loadPaths.Add(bgmPath);

            Console.WriteLine("Individual voices: " + (voicePaths.Count > 0));
            Console.WriteLine("Extra voices: " + !string.IsNullOrEmpty(extraVoicePath));

            int selectedVoices = 0;
            if (voicePaths.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Available singers:");
                for (int i = 0; i < voicePaths.Count; i++)
                {
                    string fileNameOnly = Path.GetFileNameWithoutExtension(voicePaths[i]);
                    string idolNameID = fileNameOnly.Substring($"song3_{songID}_".Length, 6);
                    Console.WriteLine(" " + i + " " + IdolNameIDToFull(idolNameID));
                }
                Console.WriteLine();

                Console.Write("Select in order (" + voiceCount + " max)" + ": ");
                string[] selection = Console.ReadLine().Trim().Split(' ');
                string[] selected = new string[selection.Length];
                selectedVoices = selection.Length;

                for (int i = 0; i < selection.Length; i++)
                {
                    int selectionIndex = int.Parse(selection[i]);
                    selected[i] = voicePaths[selectionIndex];
                }

                loadPaths.AddRange(selected);
            }

            if (!string.IsNullOrEmpty(extraVoicePath))
                loadPaths.Add(extraVoicePath);

            assetsManager.LoadFiles(loadPaths.ToArray());

            AcbWaveStream bgmAcb = new(GetStreamFromAsset(assetsManager.assetsFileList[0]));
            AcbWaveStream[] voiceAcbs = new AcbWaveStream[selectedVoices];
            AcbWaveStream bgmExAcb = null;

            if (!string.IsNullOrEmpty(extraVoicePath))
            {
                Console.Write("Song has extra voices. Include? (Y/N) ");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Y:
                        bgmExAcb = new(GetStreamFromAsset(assetsManager.assetsFileList[^1]));
                        break;
                    default:
                        break;
                }
            }

            for (int i = 0; i < selectedVoices; i++)
            {
                voiceAcbs[i] = new(GetStreamFromAsset(assetsManager.assetsFileList[i + 1]));
            }

            assetsManager.Clear();

            //return;
            //string tapPath = audioPath + "rhy_se_05_tap.wav.bytes";
            //string flickPath = audioPath + "rhy_se_05_flick.wav.bytes";

            SongMixer voiceMixer = new(voiceAcbs, bgmAcb, bgmExAcb);
            //RhythmPlayer rhythm = new(tapPath, flickPath);

            MixingSampleProvider mixer = new(voiceMixer.WaveFormat);
            mixer.AddMixerInput(voiceMixer);
            //mixer.AddMixerInput(rhythm);

            WasapiOut wasapiOut = new();
            wasapiOut.Init(voiceMixer);

            EventConductorData ct = notes.Ct[0];
            float ticksPerSecond = (float)(ct.Tempo * (ct.TSigNumerator + ct.TSigDenominator));

            //Console.WriteLine("Song ID: " + songID);
            Console.WriteLine();
            Console.WriteLine("Tempo: " + ct.Tempo);
            Console.WriteLine($"Time signature: {ct.TSigNumerator}/{ct.TSigDenominator}");
            Console.WriteLine("Calculated rate: " + ticksPerSecond + " ticks per second");
            Console.WriteLine();

            //Console.WriteLine("Voice control scenarios: ");
            //for (int i = 0; i < voiceControlScenarios.Length; i++)
            //{
            //    for (int j = 0; j < voiceControlScenarios[i].Mute.Length; j++)
            //    {
            //        if (voiceControlScenarios[i].Mute[j])
            //            Console.Write($"  {j + 1}  ");
            //        else Console.Write("     ");
            //    }

            //    int tick = voiceControlScenarios[i].Tick;
            //    Console.WriteLine(tick + " ticks, or " + (tick / ticksPerSecond) + " seconds");
            //}
            //Console.WriteLine();
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

            wasapiOut.Play();
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

            int mainScenarioIndex = 0;
            int orientScenarioIndex = 0;
            int muteIndex = 0;
            int eventIndex = 0;

            if (voiceCount < 1)
            {
                Console.CursorLeft = 0;
                Console.CursorTop = voiceCursorTop;
                Console.Write(" [ VOICE CONTROL UNAVAILABLE ]");
            }

            while (!voiceMixer.HasEnded)
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.R:
                            mainScenarioIndex = 0;
                            orientScenarioIndex = 0;
                            muteIndex = 0;
                            voiceMixer.Reset();
                            break;
                        case ConsoleKey.Spacebar:
                            paused = !paused;
                            break;
                        case ConsoleKey.RightArrow:
                            voiceMixer.Seek(3.0f);
                            break;
                        default:
                            break;
                    }
                }


                if (paused)
                {
                    wasapiOut.Pause();
                    continue;
                }
                else
                    wasapiOut.Play();


                double secondsElapsed = voiceMixer.CurrentTime.TotalSeconds;

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
                    if (currentOrientScenario.Type == ScenarioType.Expression)
                    {
                        if (currentOrientScenario.Idol == 0)
                        {
                            //Console.WriteLine(current.Idol + ": Expression = " + current.Param + ", Blink = " + current.EyeClose);
                            //if (eyesCursorTop < 0) eyesCursorTop = Console.CursorTop;
                            Console.CursorLeft = 0;
                            Console.CursorTop = eyesCursorTop;

                            //Console.WriteLine(currentOrientScenario.Idol);

                            //Console.WriteLine("Parameter: " + currentYoko.Param + "    ");
                            //Console.WriteLine("Type: " + current.Type + ", Parameter: " + current.Param + "        ");
                            //Console.WriteLine();
                            StringBuilder eyesStringBuilder = new();

                            switch (currentOrientScenario.Param)
                            {
                                case 0:
                                case 1:
                                case 2:
                                case 3:
                                    eyesStringBuilder.Append(
                                        @"   ___        ___  " + "\n" +
                                        @" --              --" + "\n" +
                                        @"  ---          --- " + "\n" +
                                        @" | O |        | O |" + "    Expression ID: " + currentOrientScenario.Param + "        \n" +
                                        @" -----        -----" + "    Eye close: " + currentOrientScenario.EyeClose + "\n" +
                                        @"                   " + "\n");
                                    break;
                                case 4:
                                    eyesStringBuilder.Append(
                                        @"    __        __   " + "\n" +
                                        @" ---            ---" + "\n" +
                                        @"  ---          --- " + "\n" +
                                        @" | O |        | O |" + "    Expression ID: " + currentOrientScenario.Param + "        \n" +
                                        @" -----        -----" + "    Eye close: " + currentOrientScenario.EyeClose + "\n" +
                                        @"                   " + "\n");
                                    break;
                                case 5:
                                    eyesStringBuilder.Append(
                                        @"   ___        ___  " + "\n" +
                                        @" --              --" + "\n" +
                                        @"  ___          ___ " + "\n" +
                                        @" /   \        /   \" + "    Expression ID: " + currentOrientScenario.Param + "        \n" +
                                        @"                   " + "    Eye close: " + currentOrientScenario.EyeClose + "\n" +
                                        @"                   " + "\n");
                                    break;
                                case 8:
                                case 9:
                                    eyesStringBuilder.Append(
                                        @"                   " + "\n" +
                                        @" ----_        _----" + "\n" +
                                        @"  ---          --- " + "\n" +
                                        @" | O |        | O |" + "    Expression ID: " + currentOrientScenario.Param + "        \n" +
                                        @" -----        -----" + "    Eye close: " + currentOrientScenario.EyeClose + "\n" +
                                        @"                   " + "\n");
                                    break;
                                case 10:
                                case 16:
                                    eyesStringBuilder.Append(
                                        @"                   " + "\n" +
                                        @" ---__        __---" + "\n" +
                                        @"  ---          --- " + "\n" +
                                        @" | O |        | O |" + "    Expression ID: " + currentOrientScenario.Param + "        \n" +
                                        @" -----        -----" + "    Eye close: " + currentOrientScenario.EyeClose + "\n" +
                                        @"                   " + "\n");
                                    break;
                                case 17:
                                    eyesStringBuilder.Append(
                                        @"                   " + "\n" +
                                        @" ---__        __---" + "\n" +
                                        @"  ___          ___ " + "\n" +
                                        @" | O |        | O |" + "    Expression ID: " + currentOrientScenario.Param + "        \n" +
                                        @" -----        -----" + "    Eye close: " + currentOrientScenario.EyeClose + "\n" +
                                        @"                   " + "\n");
                                    break;
                                case 21:
                                    eyesStringBuilder.Append(
                                        @"                   " + "\n" +
                                        @" --__-        -__--" + "\n" +
                                        @"  ---          --- " + "\n" +
                                        @" | O |        | O |" + "    Expression ID: " + currentOrientScenario.Param + "        \n" +
                                        @" -----        -----" + "    Eye close: " + currentOrientScenario.EyeClose + "\n" +
                                        @"                   " + "\n");
                                    break;
                                case 23:
                                    eyesStringBuilder.Append(
                                        @"                   " + "\n" +
                                        @" ____-        -____" + "\n" +
                                        @"  ---          --- " + "\n" +
                                        @" | O |        | O |" + "    Expression ID: " + currentOrientScenario.Param + "        \n" +
                                        @" -----        -----" + "    Eye close: " + currentOrientScenario.EyeClose + "\n" +
                                        @"                   " + "\n");
                                    break;
                                case 26:
                                    eyesStringBuilder.Append(
                                        @"   ___        ___  " + "\n" +
                                        @" --              --" + "\n" +
                                        @"  ___          --- " + "\n" +
                                        @" /   \        | O |" + "    Expression ID: " + currentOrientScenario.Param + "        \n" +
                                        @"              -----" + "    Eye close: " + currentOrientScenario.EyeClose + "\n" +
                                        @"                   " + "\n");
                                    break;
                                case 27:
                                    eyesStringBuilder.Append(
                                        @"   ___        ___  " + "\n" +
                                        @" --              --" + "\n" +
                                        @"  ---          ___ " + "\n" +
                                        @" | O |        /   \" + "    Expression ID: " + currentOrientScenario.Param + "        \n" +
                                        @" -----             " + "    Eye close: " + currentOrientScenario.EyeClose + "\n" +
                                        @"                   " + "\n");
                                    break;
                                default:
                                    eyesStringBuilder.Append(
                                        @"                   " + "\n" +
                                        @" -----        -----" + "\n" +
                                        @"  ---          --- " + "\n" +
                                        @" | O |        | O |" + "    Unknown expr.: " + currentOrientScenario.Param + "        \n" +
                                        @"  ---          --- " + "    Eye close: " + currentOrientScenario.EyeClose + "\n" +
                                        @"                   " + "\n");
                                    break;
                            }

                            Console.Write(eyesStringBuilder.ToString());

                            if (currentOrientScenario.EyeClose == 1)
                            {
                                eyesStringBuilder.Clear();
                                eyesStringBuilder.Append(
                                    //@"                   " + "\n" +
                                    //@" -----        -----" + "\n" +
                                    @"                   " + "\n" +
                                    @" \___/        \___/" + "    Expression ID: " + currentOrientScenario.Param + "        \n" +
                                    @"                   " + "    Eye close: " + currentOrientScenario.EyeClose + " \n" +
                                    @"                   " + "\n");

                                Console.CursorLeft = 0;
                                Console.CursorTop = eyesCursorTop;
                                Console.CursorTop += 2;
                                Console.Write(eyesStringBuilder.ToString());
                            }
                        }
                    }

                    if (orientScenarioIndex < orientScenario.Scenario.Count - 1) orientScenarioIndex++;
                    else break;
                    currentOrientScenario = orientScenario.Scenario[orientScenarioIndex];
                }

                EventScenarioData currentMainScenario = mainScenario.Scenario[mainScenarioIndex];
                while (secondsElapsed >= (currentMainScenario.Tick / ticksPerSecond))
                {
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

                    else if (currentMainScenario.Type == ScenarioType.LipSync)
                    {
                        //if (mouthCursorTop < 0) mouthCursorTop = Console.CursorTop;
                        Console.CursorLeft = 0;
                        Console.CursorTop = mouthCursorTop;

                        //Console.WriteLine(" [" + current.Param + "] ");
                        //Console.WriteLine("Type: " + current.Type + ", Parameter: " + current.Param + "        ");
                        //Console.WriteLine();
                        StringBuilder mouthStringBuilder = new();

                        switch (currentMainScenario.Param)
                        {
                            case 0:
                                mouthStringBuilder.Append(
                                    @"   ______________  " + "\n" +
                                    @"   |            |  " + "\n" +
                                    @"   |            |  " + "    Sound: A, ID: " + currentMainScenario.Param + "        \n" +
                                    @"   |            |  " + "\n" +
                                    @"   \____________/  " + "\n" +
                                    @"                   " + "\n");
                                break;
                            case 1:
                                mouthStringBuilder.Append(
                                    @" __________________" + "\n" +
                                    @" |\______________/|" + "\n" +
                                    @" \  ____________  /" + "    Sound: I, ID: " + currentMainScenario.Param + "        \n" +
                                    @"  \/____________\/ " + "\n" +
                                    @"                   " + "\n" +
                                    @"                   " + "\n");
                                break;
                            case 2:
                                mouthStringBuilder.Append(
                                    @"       ______      " + "\n" +
                                    @"       |    |      " + "\n" +
                                    @"       |____|      " + "    Sound: U, ID: " + currentMainScenario.Param + "        \n" +
                                    @"                   " + "\n" +
                                    @"                   " + "\n" +
                                    @"                   " + "\n");
                                break;
                            case 3:
                                mouthStringBuilder.Append(
                                    @" __________________" + "\n" +
                                    @" |\______________/|" + "\n" +
                                    @" |                |" + "    Sound: E, ID: " + currentMainScenario.Param + "        \n" +
                                    @" \________________/" + "\n" +
                                    @"                   " + "\n" +
                                    @"                   " + "\n");
                                break;
                            case 4:
                                mouthStringBuilder.Append(
                                    @"     __________    " + "\n" +
                                    @"     |        |    " + "\n" +
                                    @"     |        |    " + "    Sound: O, ID: " + currentMainScenario.Param + "        \n" +
                                    @"     |        |    " + "\n" +
                                    @"     |________|    " + "\n" +
                                    @"                   " + "\n");
                                break;
                            case 54:
                                mouthStringBuilder.Append(
                                    @"                   " + "\n" +
                                    @"     \________/    " + "\n" +
                                    @"                   " + "    Sound: -, ID: " + currentMainScenario.Param + "        \n" +
                                    @"                   " + "\n" +
                                    @"                   " + "\n" +
                                    @"                   " + "\n");
                                break;
                            default:
                                mouthStringBuilder.Append(
                                   @"                    " + "\n" +
                                   @"   /\/\/\/\/\/\/\   " + "\n" +
                                   @"                    " + "    Unknown mouth shape: " + currentMainScenario.Param + "\n" +
                                   @"                    " + "\n" +
                                   @"                    " + "\n" +
                                   @"                    " + "\n");
                                break;
                        }

                        Console.Write(mouthStringBuilder.ToString());
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

            //rhythm.Stop();
        }

        static string IdolNameIDToFull(string nameID)
        {
            return nameID switch
            {
                "001har" => "Haruka Amami",
                "002chi" => "Chihaya Kisaragi",
                "003mik" => "Miki Hoshii",
                "004yuk" => "Yukiho Hagiwara",
                "005yay" => "Yayoi Takatsuki",
                "006mak" => "Makoto Kikuchi",
                "007ior" => "Iori Minase",
                "008tak" => "Takane Shijou",
                "009rit" => "Ritsuko Akizuki",
                "010azu" => "Azusa Miura",
                "011maw" => "Ami Futami",
                "012mam" => "Mami Futami",
                "013hib" => "Hibiki Ganaha",
                "014mir" => "Mirai Kasuga",
                "015siz" => "Shizuka Mogami",
                "016tsu" => "Tsubasa Ibuki",
                "017kth" => "Kotoha Tanaka",
                "018ele" => "Elena Shimabara",
                "019min" => "Minako Satake",
                "020meg" => "Megumi Tokoro",
                "021mat" => "Matsuri Tokugawa",
                "022ser" => "Serika Hakozaki",
                "023aka" => "Akane Nonohara",
                "024ann" => "Anna Mochizuki",
                "025roc" => "Roco Handa",
                "026yur" => "Yuriko Nanao",
                "027say" => "Sayoko Takayama",
                "028ari" => "Arisa Matsuda",
                "029umi" => "Umi Kousaka",
                "030iku" => "Iku Nakatani",
                "031tom" => "Tomoka Tenkubashi",
                "032emi" => "Emily Stewart",
                "033sih" => "Shiho Kitazawa",
                "034ayu" => "Ayumu Maihama",
                "035hin" => "Hinata Kinoshita",
                "036kan" => "Kana Yabuki",
                "037nao" => "Nao Yokoyama",
                "038chz" => "Chizuru Nikaido",
                "039kon" => "Konomi Baba",
                "040tam" => "Tamaki Ogami",
                "041fuk" => "Fuka Toyokawa",
                "042miy" => "Miya Miyao",
                "043nor" => "Noriko Fukuda",
                "044miz" => "Mizuki Makabe",
                "045kar" => "Karen Shinomiya",
                "046rio" => "Rio Momose",
                "047sub" => "Subaru Nagayoshi",
                "048rei" => "Reika Kitakami",
                "049mom" => "Momoko Suou",
                "050jul" => "Julia",
                "051tmg" => "Tsumugi Shiraishi",
                "052kao" => "Kaori Sakuramori",
                _ => "Unknown idol.",
            };
        }

        static Stream GetStreamFromAsset(SerializedFile file)
        {
            TextAsset asset = (TextAsset)file.Objects.FirstOrDefault(o => o.type == ClassIDType.TextAsset);

            return new MemoryStream(asset.m_Script);
        }

        static bool Matches(int match, int[] matchList)
        {
            for (int i = 0; i < matchList.Length; i++)
            {
                if (match == matchList[i]) return true;
            }

            return false;
        }
    }
}