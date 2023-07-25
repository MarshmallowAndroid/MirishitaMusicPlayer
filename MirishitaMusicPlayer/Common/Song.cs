using AssetStudio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Common
{
    public class Song
    {
        private readonly AssetsManager _assetsManager;

        public Song(AssetList assets, string songId, AssetsManager assetsManager)
        {
            SongID = songId;

            // Required files
            string scenarioString = $"scrobj_{songId}.unity3d";
            string scenarioPlusString = $"scrobj_{songId[..^1]}+.unity3d";
            string scenarioThirtyNineString = $"scrobj_{songId[..^2]}39.unity3d";

            // Late files
            string originalBgmString = $"song3_{songId}.acb.unity3d";
            string bgmString = $"song3_{songId}_bgm.acb.unity3d";
            string instrumentalString = $"bgm_inst_{songId}.acb.unity3d";
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
                else if (instrumentalString.Equals(asset.Name))
                    InstrumentalAsset = asset;
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

        public string SongID { get; }

        public Asset ScenarioAsset { get; }

        public Asset ScenarioPlusAsset { get; }

        public Asset ScenarioThirtyNineAsset { get; }

        public Asset OriginalBgmAsset { get; }

        public Asset BgmAsset { get; }

        public Asset InstrumentalAsset { get; }

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
}
