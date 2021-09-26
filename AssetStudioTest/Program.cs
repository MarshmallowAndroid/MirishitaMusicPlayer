using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using AssetStudio;

namespace AssetStudioTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string downloadPath = @"D:\mirishita\DOWNLOAD";

            string songID = "sjewel";
            string scenarioPath = Path.Combine(downloadPath, $"scrobj_{songID}.unity3d");

            AssetsManager assetsManager = new();

            // Load scenarios and notes first
            assetsManager.LoadFiles(new[] { scenarioPath });

            ScenarioScrObject mainScenario;
            ScenarioScrObject yokoScenario;
            ScenarioScrObject tateScenario;
            NoteScrObject fumen;

            foreach (var file in assetsManager.assetsFileList)
            {
                foreach (var gameObject in file.Objects)
                {
                    if (gameObject.type == ClassIDType.MonoBehaviour)
                    {
                        MonoBehaviour monoBehaviour = (MonoBehaviour)gameObject;
                        if (monoBehaviour.m_Name == $"{songID}_scenario_sobj")
                        {
                            File.WriteAllText("scenario_dump.txt", monoBehaviour.Dump());
                            mainScenario = new(monoBehaviour);
                        }
                        else if (monoBehaviour.m_Name == $"{songID}_scenario_yoko_sobj")
                            yokoScenario = new(monoBehaviour);
                        else if (monoBehaviour.m_Name == $"{songID}_scenario_tate_sobj")
                            tateScenario = new(monoBehaviour);
                        else if (monoBehaviour.m_Name == $"{songID}_fumen_sobj")
                            fumen = new(monoBehaviour);
                    }
                }
            }

            string[] audioFiles = Directory.GetFiles(downloadPath, $"song3_{songID}*");

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

            Console.WriteLine("Individual voices: " + (voicePaths.Count > 0));
            Console.WriteLine("BGM: " + !string.IsNullOrEmpty(bgmPath));
            Console.WriteLine("Extra voices: " + !string.IsNullOrEmpty(extraVoicePath));
            Console.WriteLine();

            List<string> loadPaths = new();

            Console.WriteLine("Available singers:");
            foreach (var voice in voicePaths)
            {
                string fileNameOnly = Path.GetFileNameWithoutExtension(voice);
                string idolNameID = fileNameOnly.Substring($"song3_{songID}_".Length, 6);
                Console.WriteLine("    " + IdolNameIDToFull(idolNameID));
            }

            loadPaths.Add(bgmPath);
        }

        static string IdolNameIDToFull(string nameID)
        {
            switch (nameID)
            {
                case "001har":
                    return "Haruka Amami";
                case "002chi":
                    return "Chihaya Kisaragi";
                case "003mik":
                    return "Miki Hoshii";
                case "004yuk":
                    return "Yukiho Hagiwara";
                case "005yay":
                    return "Yayoi Takatsuki";
                case "006mak":
                    return "Makoto Kikuchi";
                case "007ior":
                    return "Iori Minase";
                case "008tak":
                    return "Takane Shijou";
                case "009rit":
                    return "Ritsuko Akizuki";
                case "010azu":
                    return "Azusa Miura";
                case "011maw":
                    return "Ami Futami";
                case "012mam":
                    return "Mami Futami";
                case "013hib":
                    return "Hibiki Ganaha";
                case "014mir":
                    return "Mirai Kasuga";
                case "015siz":
                    return "Shizuka Mogami";
                case "016tsu":
                    return "Tsubasa Ibuki";
                case "017kth":
                    return "Kotoha Tanaka";
                case "018ele":
                    return "Elena Shimabara";
                case "019min":
                    return "Minako Satake";
                case "020meg":
                    return "Megumi Tokoro";
                case "021mat":
                    return "Matsuri Tokugawa";
                case "022ser":
                    return "Serika Hakozaki";
                case "023aka":
                    return "Akane Nonohara";
                case "024ann":
                    return "Anna Mochizuki";
                case "025roc":
                    return "Roco Handa";
                case "026yur":
                    return "Yuriko Nanao";
                case "027say":
                    return "Sayoko Takayama";
                case "028ari":
                    return "Arisa Matsuda";
                case "029umi":
                    return "Umi Kousaka";
                case "030iku":
                    return "Iku Nakatani";
                case "031tom":
                    return "Tomoka Tenkubashi";
                case "032emi":
                    return "Emily Stewart";
                case "033sih":
                    return "Shiho Kitazawa";
                case "034ayu":
                    return "Ayumu Maihama";
                case "035hin":
                    return "Hinata Kinoshita";
                case "036kan":
                    return "Kana Yabuki";
                case "037nao":
                    return "Nao Yokoyama";
                case "038chz":
                    return "Chizuru Nikaido";
                case "039kon":
                    return "Konomi Baba";
                case "040tam":
                    return "Tamaki Ogami";
                case "041fuk":
                    return "Fuka Toyokawa";
                case "042miy":
                    return "Miya Miyao";
                case "043nor":
                    return "Noriko Fukuda";
                case "044miz":
                    return "Mizuki Makabe";
                case "045kar":
                    return "Karen Shinomiya";
                case "046rio":
                    return "Rio Momose";
                case "047sub":
                    return "Subaru Nagayoshi";
                case "048rei":
                    return "Reika Kitakami";
                case "049mom":
                    return "Momoko Suou";
                case "050jul":
                    return "Julia";
                case "051tmg":
                    return "Tsumugi Shiraishi";
                case "052kao":
                    return "Kaori Sakuramori";
                default:
                    return "Unknown idol.";
            }
        }
    }
}
