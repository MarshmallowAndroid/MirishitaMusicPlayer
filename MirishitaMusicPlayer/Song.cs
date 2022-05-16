using AssetStudio;
using MirishitaMusicPlayer.Audio;
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
    public class Song
    {
        private readonly AssetsManager _assetsManager;

        public Song(AssetList assets, string songId, AssetsManager assetsManager)
        {
            SongId = songId;

            // Required files
            string scenarioString = $"scrobj_{songId}.unity3d";
            string scenarioPlusString = $"scrobj_{songId[..^1]}+.unity3d";
            string scenarioThirtyNineString = $"scrobj_{songId[..^2]}39.unity3d";

            // Late files
            string originalBgmString = $"song3_{songId}.acb.unity3d";
            string bgmString = $"song3_{songId}_bgm.acb.unity3d";
            Regex voiceRegex = new($"song3_{songId}_([0-9]{{3}})([a-z]{{3}}).acb.unity3d");
            string allString = $"song3_{songId}_all.acb.unity3d";
            string extraString = $"song3_{songId}_ex.acb.unity3d";
            string extraSecondString = $"song3_{songId}_ex2.acb.unity3d";

            // Assign matching filenames to their corresponding audio type
            foreach (var asset in assets.Assets)
            {
                if (scenarioString.Equals(asset.Name))
                    ScenarioAsset = asset;
                else if (scenarioPlusString.Equals(asset.Name))
                    ScenarioPlusAsset = asset;
                else if (scenarioThirtyNineString.Equals(asset.Name))
                    ScenarioThirtyNineAsset = asset;
                else if (originalBgmString.Equals(asset.Name))
                    OriginalBgmAsset = asset;
                else if (bgmString.Equals(asset.Name))
                    BgmAsset = asset;
                else if (voiceRegex.IsMatch(asset.Name))
                    VoiceAssets.Add(asset);
                else if (allString.Equals(asset.Name))
                    AllAsset = asset;
                else if (extraString.Equals(asset.Name))
                    ExtraAsset = asset;
                else if (extraSecondString.Equals(asset.Name))
                    ExtraSecondAsset = asset;
            }

            Singers = new Idol[VoiceAssets.Count];
            for (int i = 0; i < VoiceAssets.Count; i++)
            {
                string fileNameOnly = Path.GetFileNameWithoutExtension(VoiceAssets[i].Name);
                string idolNameId = fileNameOnly.Substring($"song3_{songId}_".Length, 6);
                Singers[i] = new Idol(idolNameId);
            }

            Array.Sort(Singers, (s1, s2) => s1.IdolNameId.CompareTo(s2.IdolNameId));

            _assetsManager = assetsManager;
        }

        public string SongId { get; }

        public Asset ScenarioAsset { get; }

        public Asset ScenarioPlusAsset { get; }

        public Asset ScenarioThirtyNineAsset { get; }

        public Asset OriginalBgmAsset { get; }

        public Asset BgmAsset { get; }

        public List<Asset> VoiceAssets { get; } = new();

        public Asset AllAsset { get; }

        public Asset ExtraAsset { get; }

        public Asset ExtraSecondAsset { get; }

        public Idol[] Singers { get; }

        public SongScenario Scenario { get; private set; }

        public void LoadScenario(Asset scenarioAsset, string directory)
        {
            Scenario = new SongScenario(Path.Combine(directory, scenarioAsset.Name), this, _assetsManager);
        }
    }

    public enum ScenarioOrientation
    {
        Yoko,   // Landscape mode
        Tate    // Portrait mode
    }

    public class SongScenario
    {
        public SongScenario(
            string scenarioFile,
            Song song,
            AssetsManager assetsManager,
            ScenarioOrientation orientation = ScenarioOrientation.Tate)
        {
            // Load scenarios and notes first
            assetsManager.LoadFiles(new[] { scenarioFile });

            MainScenario = null; // Main scenario for static events, like lyrics and lipsync
            OrientationScenario = null; // Landscape or portrait scenario, for unit or solo facial expressions and idol mute
            //Notes = null; // Tap notes and song timing via EventConductor

            foreach (var file in assetsManager.assetsFileList)
            {
                foreach (var gameObject in file.Objects)
                {
                    if (gameObject.type == ClassIDType.MonoBehaviour)
                    {
                        // Find matching MonoBehaviour name and serialize into their corresponding types

                        MonoBehaviour monoBehaviour = (MonoBehaviour)gameObject;
                        if (monoBehaviour.m_Name.EndsWith("_scenario_sobj"))
                            MainScenario = new(monoBehaviour);
                        else if (monoBehaviour.m_Name.EndsWith("_scenario_yoko_sobj") && orientation == ScenarioOrientation.Yoko)
                            OrientationScenario = new(monoBehaviour);
                        else if (monoBehaviour.m_Name.EndsWith("_scenario_tate_sobj") && orientation == ScenarioOrientation.Tate)
                            OrientationScenario = new(monoBehaviour);
                        //else if (monoBehaviour.m_Name == $"{songID}_fumen_sobj")
                        //    Notes = new(monoBehaviour);
                    }
                }
            }

            // Figure out where the expression events are: in main scenario or orientation scenario?
            Func<EventScenarioData, bool> expressionPredicate = new(s => s.Type == ScenarioType.Expression);
            ExpressionScenarios = OrientationScenario.Scenario.Where(expressionPredicate).ToList();
            if (ExpressionScenarios.Count < 1) ExpressionScenarios = MainScenario.Scenario.Where(expressionPredicate).ToList();

            // Figure out where the mute events are: in main scenario or orientation scenario?
            Func<EventScenarioData, bool> mutePredicate = new(s => s.Type == ScenarioType.Mute);
            MuteScenarios = OrientationScenario.Scenario.Where(mutePredicate).ToList();
            if (MuteScenarios.Count < 1) MuteScenarios = MainScenario.Scenario.Where(mutePredicate).ToList();

            StageMemberCount = MuteScenarios[0].Mute.Length;

            assetsManager.Clear();

            Configuration = new(song, this, assetsManager);
        }

        public int StageMemberCount { get; }

        public ScenarioScrObject MainScenario { get; }

        public ScenarioScrObject OrientationScenario { get; }

        public List<EventScenarioData> ExpressionScenarios { get; }

        public List<EventScenarioData> MuteScenarios { get; }

        public SongScenarioConfiguration Configuration { get; }
    }

    [Flags]
    public enum SongMode
    {
        Normal = 0x1,
        Utaiwake = 0x2,
        OngenSentaku = 0x4
    }

    public class SongScenarioConfiguration
    {
        private readonly Song _song;
        private readonly SongScenario _songScenario;
        private readonly AssetsManager _assetsManager;

        public SongScenarioConfiguration(Song song, SongScenario songScenario, AssetsManager assetsManager)
        {
            if (song.OriginalBgmAsset != null)
                Modes |= SongMode.Normal;

            if (song.VoiceAssets.Count > 0)
                Modes |= SongMode.Utaiwake;

            if (song.ExtraAsset != null)
                Modes |= SongMode.OngenSentaku;

            _song = song;
            _songScenario = songScenario;
            _assetsManager = assetsManager;
        }

        public Idol[] Order { get; set; }

        public SongMode Modes { get; }

        public SongMode Mode { get; set; }

        public bool UseOriginalBgm { get; set; }

        public SongMixer SongMixer { get; private set; }

        public List<Asset> GetRequiredAssets()
        {
            var assets = new List<Asset>();

            // BGM
            if (Modes.HasFlag(Mode)
                && Mode == SongMode.OngenSentaku)
            {
                assets.Add(_song.ExtraAsset);
            }
            else
            {
                if (_song.OriginalBgmAsset != null && (UseOriginalBgm || Mode != SongMode.Utaiwake))
                {
                    assets.Add(_song.OriginalBgmAsset);
                }
                else
                {
                    assets.Add(_song.BgmAsset);
                }
            }

            // Voices
            if (Modes.HasFlag(Mode) && Mode == SongMode.Utaiwake)
            {
                foreach (var idol in Order)
                {
                    var matchingAsset = _song.VoiceAssets.FirstOrDefault(va => va.Name.Contains(idol.IdolAudioNameId));
                    if (matchingAsset != null) assets.Add(matchingAsset);
                }
            }

            // Extra
            if (_song.ExtraAsset != null && Mode != SongMode.OngenSentaku)
                assets.Add(_song.ExtraAsset);

            return assets;
        }

        public void Load(List<Asset> requiredAssets, string directory)
        {
            List<string> loadPaths = new();

            foreach (var asset in requiredAssets)
            {
                loadPaths.Add(Path.Combine(directory, asset.Name));
            }

            // Load the files into AssetStudio for reading
            //
            // The loaded files put into the assetsFileList array
            // in the same order they were loaded in (very important)
            _assetsManager.LoadFiles(loadPaths.ToArray());

            // Get the stream of the BGM file and initialize its WaveStream
            //
            // BGM is always the first in the array, as per how we loaded them
            AcbWaveStream bgmAcb = new(GetStreamFromAsset(_assetsManager.assetsFileList[0]));

            // Leave these null for now
            AcbWaveStream[] voiceAcbs = null;
            AcbWaveStream extraAcb = null;

            // If order is not null
            if (Order != null)
            {
                // Get voice ACB streams and initialize their WaveStreams
                voiceAcbs = new AcbWaveStream[Order.Length];

                for (int i = 0; i < Order.Length; i++)
                {
                    // Voices are after BGM and before extra (if any)
                    voiceAcbs[i] = new(GetStreamFromAsset(_assetsManager.assetsFileList[i + 1]));
                }
            }

            if (_song.ExtraAsset != null && _songScenario.StageMemberCount > 13)
            {
                // Same as for the BGM and voices
                //
                // Extra ACB is always the last
                extraAcb = new(GetStreamFromAsset(_assetsManager.assetsFileList[^1]));
            }

            // We got what we wanted (their MemoryStreams) so no need to keep them loaded
            _assetsManager.Clear();

            // Initialize the song mixer
            SongMixer = new(_songScenario.MuteScenarios, voiceAcbs, bgmAcb, extraAcb);
        }

        private static Stream GetStreamFromAsset(SerializedFile file)
        {
            // CRIWARE ACB files are stored as TextAssets in the m_Script field

            TextAsset asset = (TextAsset)file.Objects.FirstOrDefault(o => o.type == ClassIDType.TextAsset);

            return new MemoryStream(asset.m_Script);
        }
    }
}
