using AssetStudio;
using MirishitaMusicPlayer.Audio;
using MirishitaMusicPlayer.Imas;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer
{
    class AudioLoader
    {
        private readonly AssetsManager assetsManager;

        private readonly List<string> voicePaths = new();
        private readonly string bgmPath = "";
        private readonly string extraPath = "";

        private readonly List<string> loadPaths = new();

        private readonly List<EventScenarioData> muteScenarios;
        
        private AcbWaveStream bgmAcb;
        private AcbWaveStream[] voiceAcbs;
        private AcbWaveStream extraAcb;

        public AudioLoader(AssetsManager assetsManager, List<EventScenarioData> muteScenarios, string filesPath, string songID)
        {
            this.assetsManager = assetsManager;
            this.muteScenarios = muteScenarios; // Needed for direct voice control for each track

            // Get all the audio files for the song
            string[] audioFiles = Directory.GetFiles(filesPath, $"song3_{songID}*");

            Regex normalRegex = new($"song3_{songID}.acb.unity3d");
            Regex bgmRegex = new($"song3_{songID}_bgm.acb.unity3d");
            Regex voiceRegex = new($"song3_{songID}_([0-9]{{3}})([a-z]{{3}}).acb.unity3d");
            Regex extraRegex = new($"song3_{songID}_ex.acb.unity3d");

            // Assign matching filenames to their corresponding audio type
            foreach (var file in audioFiles)
            {
                if (normalRegex.IsMatch(file) || bgmRegex.IsMatch(file))
                    bgmPath = file;
                if (voiceRegex.IsMatch(file))
                    voicePaths.Add(file);
                else if (extraRegex.IsMatch(file))
                    extraPath = file;
            }

            // For special songs with swappable voices
            if (voicePaths.Count > 0)
            {
                // Add singers based on found files
                Singers = new Idol[voicePaths.Count];
                for (int i = 0; i < voicePaths.Count; i++)
                {
                    string fileNameOnly = Path.GetFileNameWithoutExtension(voicePaths[i]);
                    string idolNameID = fileNameOnly.Substring($"song3_{songID}_".Length, 6);
                    Singers[i] = new Idol(idolNameID);
                }
            }
        }

        public Idol[] Singers { get; }

        public SongMixer SongMixer { get; private set; }

        public WaveOutEvent OutputDevice { get; } = new() { DesiredLatency = 75 };

        public void Setup(Idol[] order = null, bool extraEnabled = false, RhythmPlayer rhythmPlayer = null)
        {
            // Stop and dispose output device
            OutputDevice.Stop();
            if (SongMixer != null) SongMixer.Dispose();

            // Clear load paths
            loadPaths.Clear();

            // Add BGM file to load path
            loadPaths.Add(bgmPath);

            // If order is not null, add each voice to the load path
            if (order != null)
            {
                for (int i = 0; i < order.Length; i++)
                    // Identify each voice based on the idol's internal ID (like 001har for Haruka)
                    loadPaths.Add(voicePaths.Where(p => p.Contains(order[i].IdolNameID)).FirstOrDefault());
            }

            // If the extra path is not empty and extra is enabled, add the extra track to the load path
            if (!string.IsNullOrEmpty(extraPath) && extraEnabled)
                loadPaths.Add(extraPath);

            // Load the files into AssetStudio for reading
            //
            // The loaded files put into the assetsFileList array
            // in the same order they were loaded in (very important)
            assetsManager.LoadFiles(loadPaths.ToArray());

            // Get the stream of the BGM file and initialize its WaveStream
            //
            // BGM is always the first in the array, as per how we loaded them
            bgmAcb = new(GetStreamFromAsset(assetsManager.assetsFileList[0]));
            
            // Leave these null for now
            voiceAcbs = null;
            extraAcb = null;

            // If order is not nu--
            if (order != null)
            {
                // Get voice ACB streams and initialize their WaveStreams
                voiceAcbs = new AcbWaveStream[order.Length];

                for (int i = 0; i < order.Length; i++)
                    // Voices are after BGM and before extra (if any)
                    voiceAcbs[i] = new(GetStreamFromAsset(assetsManager.assetsFileList[i + 1]));
            }

            if (!string.IsNullOrEmpty(extraPath) && extraEnabled)
                // Same as for the BGM and voices
                //
                // Extra ACB is always the last
                extraAcb = new(GetStreamFromAsset(assetsManager.assetsFileList[^1]));

            // We got what we wanted (their MemoryStreams) so no need to keep them loaded
            assetsManager.Clear();

            // Initialize the song mixer
            SongMixer = new(muteScenarios, voiceAcbs, bgmAcb, extraAcb);

            // Initialize the output device, with the input depending on whether the rhythm player exists or not
            OutputDevice.Init(
                rhythmPlayer != null ?
                new MixingSampleProvider(new ISampleProvider[] { SongMixer, rhythmPlayer }) :
                SongMixer);
        }

        static Stream GetStreamFromAsset(SerializedFile file)
        {
            // CRIWARE ACB files are stored as TextAssets in the m_Script field

            TextAsset asset = (TextAsset)file.Objects.FirstOrDefault(o => o.type == ClassIDType.TextAsset);

            return new MemoryStream(asset.m_Script);
        }
    }

    class Idol
    {
        public Idol(string idolNameID)
        {
            IdolNameID = idolNameID;
        }

        public string IdolNameID { get; }

        public int IdolIDNumber => int.Parse(IdolNameID[0..3]);

        public string IdolNameFull => IdolNameIDToFull(IdolNameID);

        public string IdolFirstName => IdolNameFull.Split(' ')[0].Trim();

        public string IdolLastName => IdolNameFull.Split(' ')[1].Trim();

        private static string IdolNameIDToFull(string nameID)
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
                _ => "Unknown",
            };
        }
    }
}
