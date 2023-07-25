using AssetStudio;
using MirishitaMusicPlayer.Audio;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MirishitaMusicPlayer.Common
{
    public class SongScenarioConfiguration
    {
        private readonly Song _song;
        private readonly SongScenario _songScenario;
        private readonly AssetsManager _assetsManager;

        public SongScenarioConfiguration(Song song, SongScenario songScenario, AssetsManager assetsManager)
        {
            if (song.OriginalBgmAsset is not null)
                Modes |= SongMode.Normal;

            if (song.VoiceAssets.Count > 0)
                Modes |= SongMode.Utaiwake;

            if (song.VoiceAssets.Count == 0 && song.ExtraAsset is not null)
                Modes |= SongMode.OngenSentaku;

            if (song.InstrumentalAsset is not null)
                Modes |= SongMode.Instrumental;

            _song = song;
            _songScenario = songScenario;
            _assetsManager = assetsManager;
        }

        public Idol[] Order { get; set; }

        public SongMode Modes { get; }

        public SongMode Mode { get; set; }

        public SongMixer SongMixer { get; private set; }

        public List<Asset> GetRequiredAssets()
        {
            var assets = new List<Asset>();

            // BGM
            if (Mode == SongMode.OngenSentaku)
                assets.Add(_song.ExtraAsset);
            else
            {
                if (Mode == SongMode.Normal)
                    assets.Add(_song.OriginalBgmAsset);
                else if (Mode == SongMode.Instrumental)
                    assets.Add(_song.InstrumentalAsset);
                else
                    assets.Add(_song.BgmAsset);
            }

            // Voices
            if (Mode == SongMode.Utaiwake)
            {
                foreach (var idol in Order)
                {
                    var matchingAsset = _song.VoiceAssets.FirstOrDefault(va => va.Name.Contains(idol.IdolAudioNameId));
                    if (matchingAsset is not null) assets.Add(matchingAsset);
                }
            }

            // Extra
            if (_song.ExtraAsset is not null && Mode == SongMode.Utaiwake)
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

            if (Mode != SongMode.Instrumental)
            {
                // If order is not null
                if (Mode == SongMode.Utaiwake)
                {
                    // Get voice ACB streams and initialize their WaveStreams
                    voiceAcbs = new AcbWaveStream[Order.Length];

                    for (int i = 0; i < Order.Length; i++)
                    {
                        // Voices are after BGM and before extra (if any)
                        voiceAcbs[i] = new(GetStreamFromAsset(_assetsManager.assetsFileList[i + 1]));
                    }
                }

                if (_song.ExtraAsset is not null && _songScenario.StageMemberCount == 6 || _songScenario.StageMemberCount > 14)
                {
                    // Same as for the BGM and voices
                    //
                    // Extra ACB is always the last
                    extraAcb = new(GetStreamFromAsset(_assetsManager.assetsFileList[^1]));
                }
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
