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
        private readonly string extraVoicePath = "";

        private readonly List<string> loadPaths = new();

        private AcbWaveStream bgmAcb;
        private AcbWaveStream[] voiceAcbs;
        private AcbWaveStream bgmExAcb;

        public AudioLoader(AssetsManager assetsManager, List<EventScenarioData> muteScenarios, string filesPath, string songID)
        {
            this.assetsManager = assetsManager;

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

            foreach (var file in audioFiles)
            {
                if (normalRegex.IsMatch(file) || bgmRegex.IsMatch(file))
                    bgmPath = file;
                if (voiceRegex.IsMatch(file))
                    voicePaths.Add(file);
                else if (extraVoiceRegex.IsMatch(file))
                    extraVoicePath = file;
            }

            if (voicePaths.Count > 0)
            {
                Singers = new Idol[voicePaths.Count];
                for (int i = 0; i < voicePaths.Count; i++)
                {
                    string fileNameOnly = Path.GetFileNameWithoutExtension(voicePaths[i]);
                    string idolNameID = fileNameOnly.Substring($"song3_{songID}_".Length, 6);
                    Singers[i] = new Idol(idolNameID);
                }
            }

            //string tapPath = audioPath + "rhy_se_05_tap.wav.bytes";
            //string flickPath = audioPath + "rhy_se_05_flick.wav.bytes";

            //RhythmPlayer rhythm = new(tapPath, flickPath);

            //MixingSampleProvider mixer = new(SongMixer.WaveFormat);
            //mixer.AddMixerInput(SongMixer);
            //mixer.AddMixerInput(rhythm);

            //EventConductorData ct = scenarios.Notes.Ct[0];
            //float ticksPerSecond = (float)(ct.Tempo * (ct.TSigNumerator + ct.TSigDenominator));

            ////Console.WriteLine("Song ID: " + songID);
            //Console.WriteLine();
            //Console.WriteLine("Tempo: " + ct.Tempo);
            //Console.WriteLine($"Time signature: {ct.TSigNumerator}/{ct.TSigDenominator}");
            //Console.WriteLine("Calculated rate: " + ticksPerSecond + " ticks per second");
            //Console.WriteLine();
        }

        public Idol[] Singers { get; }

        public SongMixer SongMixer { get; private set; }

        public WaveOutEvent OutputDevice { get; } = new() { DesiredLatency = 75 };

        public void Setup(Idol[] order = null, bool extraVoices = false, RhythmPlayer rhythmPlayer = null)
        {
            OutputDevice.Stop();
            if (SongMixer != null) SongMixer.Dispose();

            loadPaths.Clear();
            loadPaths.Add(bgmPath);

            if (order != null)
            {
                for (int i = 0; i < order.Length; i++)
                    loadPaths.Add(voicePaths.Where(p => p.Contains(order[i].IdolNameID)).FirstOrDefault());
            }

            if (!string.IsNullOrEmpty(extraVoicePath) && extraVoices)
                loadPaths.Add(extraVoicePath);

            assetsManager.LoadFiles(loadPaths.ToArray());

            bgmAcb = new(GetStreamFromAsset(assetsManager.assetsFileList[0]));
            voiceAcbs = null;
            bgmExAcb = null;

            if (order != null)
            {
                voiceAcbs = new AcbWaveStream[order.Length];

                for (int i = 0; i < order.Length; i++)
                    voiceAcbs[i] = new(GetStreamFromAsset(assetsManager.assetsFileList[i + 1]));
            }

            if (!string.IsNullOrEmpty(extraVoicePath) && extraVoices)
                bgmExAcb = new(GetStreamFromAsset(assetsManager.assetsFileList[^1]));

            assetsManager.Clear();

            SongMixer = new(voiceAcbs, bgmAcb, bgmExAcb);

            OutputDevice.Init(new MixingSampleProvider(new ISampleProvider[] { SongMixer, rhythmPlayer }));
        }

        static Stream GetStreamFromAsset(SerializedFile file)
        {
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
