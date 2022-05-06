using AssetStudio;
using MirishitaMusicPlayer.Imas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer
{
    internal class Song
    {
        private readonly string originalBgmFile;
        private readonly string bgmFile;
        private readonly List<string> voiceFiles = new();
        private readonly string extraFile;

        public Song(AssetList assets, string songID, AssetsManager assetsManager)
        {


            string originalBgmString = $"song3_{songID}.acb.unity3d";
            string bgmString = $"song3_{songID}_bgm.acb.unity3d";
            Regex voiceRegex = new($"song3_{songID}_([0-9]{{3}})([a-z]{{3}}).acb.unity3d");
            string extraString = $"song3_{songID}_ex.acb.unity3d";

            // Assign matching filenames to their corresponding audio type
            foreach (var asset in assets.Assets)
            {
                if (originalBgmString.Equals(asset.Name))
                    originalBgmFile = asset.Name;
                if (bgmString.Equals(asset.Name))
                    bgmFile = asset.Name;
                if (voiceRegex.IsMatch(asset.Name))
                    voiceFiles.Add(asset.Name);
                if (extraString.Equals(asset.Name))
                    extraFile = asset.Name;
            }

            if (!string.IsNullOrEmpty(originalBgmFile))
                SongFlags |= SongFlags.Normal;

            if (!string.IsNullOrEmpty(bgmFile))
            {
                if (voiceFiles.Count == 13)
                    SongFlags |= SongFlags.Normal;
            }

            if (!string.IsNullOrEmpty(originalBgmFile))
                SongFlags |= SongFlags.Normal;

            if (!string.IsNullOrEmpty(originalBgmFile))
                SongFlags |= SongFlags.Normal;

            //// Load scenarios and notes first
            //string scenarioPath = Path.Combine(Program.CachePath, $"scrobj_{songID}.unity3d");
            //assetsManager.LoadFiles(new[] { scenarioPath });

            //MainScenario = null; // Main scenario for static events, like lyrics and lipsync
            //YokoScenario = null; // Landscape scenario, for unit or solo facial expressions and idol mute
            //TateScenario = null; // Portrait scenario, for unit or solo facial expressions and idol mute
            //Notes = null; // Tap notes and song timing via EventConductor

            //foreach (var file in assetsManager.assetsFileList)
            //{
            //    foreach (var gameObject in file.Objects)
            //    {
            //        if (gameObject.type == ClassIDType.MonoBehaviour)
            //        {
            //            // Find matching MonoBehaviour name and serialize into their corresponding types

            //            MonoBehaviour monoBehaviour = (MonoBehaviour)gameObject;
            //            if (monoBehaviour.m_Name == $"{songID}_scenario_sobj")
            //                MainScenario = new(monoBehaviour);
            //            else if (monoBehaviour.m_Name == $"{songID}_scenario_yoko_sobj")
            //                YokoScenario = new(monoBehaviour);
            //            else if (monoBehaviour.m_Name == $"{songID}_scenario_tate_sobj")
            //                TateScenario = new(monoBehaviour);
            //            else if (monoBehaviour.m_Name == $"{songID}_fumen_sobj")
            //                Notes = new(monoBehaviour);
            //        }
            //    }
            //}

            //// Figure out where the expression events are: in main scenario or orientation scenario?
            //Func<EventScenarioData, bool> expressionPredicate = new(s => s.Type == ScenarioType.Expression);
            //ExpressionScenarios = OrientationScenario.Scenario.Where(expressionPredicate).ToList();
            //if (ExpressionScenarios.Count < 1) ExpressionScenarios = MainScenario.Scenario.Where(expressionPredicate).ToList();

            //// Figure out where the mute events are: in main scenario or orientation scenario?
            //Func<EventScenarioData, bool> mutePredicate = new(s => s.Type == ScenarioType.Mute);
            //MuteScenarios = OrientationScenario.Scenario.Where(mutePredicate).ToList();
            //if (MuteScenarios.Count < 1) MuteScenarios = MainScenario.Scenario.Where(mutePredicate).ToList();

            //VoiceCount = MuteScenarios[0].Mute.Length;

            //assetsManager.Clear();
        }

        public ScenarioScrObject MainScenario { get; }

        public ScenarioScrObject YokoScenario { get; }

        public ScenarioScrObject TateScenario { get; }

        public NoteScrObject Notes { get; }

        public SongFlags SongFlags { get; }

        public SongConfiguration SongConfiguration { get; }
    }

    enum SongFlags
    {
        Normal,
        OngenSentaku, // Sound source selection
        Utaiwake, // (Singing) Interchangeable singers, all 765 MILLION STARS
        GenteiUtaiwake, // (Limited Singing) Interchangeable, but only select idols
        IchibuUtaiwake, // (Partial Singing) Interchangeable, but only during a certain part of the song
        ThirteenLive,
        ThirtyNineLive
    }
    
    internal enum Orientation
    {
        Yoko,   // Landscape mode
        Tate    // Portrait mode
    }

    enum PerformanceConfiguration
    {
        Default,
        Solo,

    }

    class SongConfiguration
    {
        public SongConfiguration(Song song)
        {

        }

        public Idol[] IdolOrder { get; set; }

        public PerformanceConfiguration ValidConfigurations { get; }

        public PerformanceConfiguration PerformanceConfiguration { get; set; }
    }

    class SongAsset
    {

    }
}
