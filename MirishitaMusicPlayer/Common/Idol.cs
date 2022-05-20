using System.Collections.Generic;

namespace MirishitaMusicPlayer.Common
{
    public class Idol
    {
        private static readonly Dictionary<int, string[]> idols = new()
        {
            { 001, new[] { "001har", "001har", "Haruka Amami" } },
            { 002, new[] { "002chi", "002chi", "Chihaya Kisaragi" } },
            { 003, new[] { "003mik", "003mik", "Miki Hoshii" } },
            { 004, new[] { "004yuk", "004yuk", "Yukiho Hagiwara" } },
            { 005, new[] { "005yay", "005yay", "Yayoi Takatsuki" } },
            { 006, new[] { "006mak", "006mak", "Makoto Kikuchi" } },
            { 007, new[] { "007ior", "007ior", "Iori Minase" } },
            { 008, new[] { "008tak", "008tak", "Takane Shijou" } },
            { 009, new[] { "009rit", "009rit", "Ritsuko Akizuki" } },
            { 010, new[] { "010azu", "010azu", "Azusa Miura" } },
            { 011, new[] { "011ami", "011maw", "Ami Futami" } },
            { 012, new[] { "012mam", "012mam", "Mami Futami" } },
            { 013, new[] { "013hib", "013hib", "Hibiki Ganaha" } },
            { 014, new[] { "014mir", "014mir", "Mirai Kasuga" } },
            { 015, new[] { "015siz", "015siz", "Shizuka Mogami" } },
            { 016, new[] { "016tsu", "016tsu", "Tsubasa Ibuki" } },
            { 017, new[] { "017kth", "017kth", "Kotoha Tanaka" } },
            { 018, new[] { "018ele", "018ele", "Elena Shimabara" } },
            { 019, new[] { "019min", "019min", "Minako Satake" } },
            { 020, new[] { "020meg", "020meg", "Megumi Tokoro" } },
            { 021, new[] { "021mat", "021mat", "Matsuri Tokugawa" } },
            { 022, new[] { "022ser", "022ser", "Serika Hakozaki" } },
            { 023, new[] { "023aka", "023aka", "Akane Nonohara" } },
            { 024, new[] { "024ann", "024ann", "Anna Mochizuki" } },
            { 025, new[] { "025roc", "025roc", "Roco Handa" } },
            { 026, new[] { "026yur", "026yur", "Yuriko Nanao" } },
            { 027, new[] { "027say", "027say", "Sayoko Takayama" } },
            { 028, new[] { "028ari", "028ari", "Arisa Matsuda" } },
            { 029, new[] { "029umi", "029umi", "Umi Kousaka" } },
            { 030, new[] { "030iku", "030iku", "Iku Nakatani" } },
            { 031, new[] { "031tom", "031tom", "Tomoka Tenkubashi" } },
            { 032, new[] { "032emi", "032emi", "Emily Stewart" } },
            { 033, new[] { "033sih", "033sih", "Shiho Kitazawa" } },
            { 034, new[] { "034ayu", "034ayu", "Ayumu Maihama" } },
            { 035, new[] { "035hin", "035hin", "Hinata Kinoshita" } },
            { 036, new[] { "036kan", "036kan", "Kana Yabuki" } },
            { 037, new[] { "037nao", "037nao", "Nao Yokoyama" } },
            { 038, new[] { "038chz", "038chz", "Chizuru Nikaido" } },
            { 039, new[] { "039kon", "039kon", "Konomi Baba" } },
            { 040, new[] { "040tam", "040tam", "Tamaki Ogami" } },
            { 041, new[] { "041fuk", "041fuk", "Fuka Toyokawa" } },
            { 042, new[] { "042miy", "042miy", "Miya Miyao" } },
            { 043, new[] { "043nor", "043nor", "Noriko Fukuda" } },
            { 044, new[] { "044miz", "044miz", "Mizuki Makabe" } },
            { 045, new[] { "045kar", "045kar", "Karen Shinomiya" } },
            { 046, new[] { "046rio", "046rio", "Rio Momose" } },
            { 047, new[] { "047sub", "047sub", "Subaru Nagayoshi" } },
            { 048, new[] { "048rei", "048rei", "Reika Kitakami" } },
            { 049, new[] { "049mom", "049mom", "Momoko Suou" } },
            { 050, new[] { "050jul", "050jul", "Julia" } },
            { 051, new[] { "051tmg", "051tmg", "Tsumugi Shiraishi" } },
            { 052, new[] { "052kao", "052kao", "Kaori Sakuramori" } },
            { 201, new[] { "201xxx", "201xxx", "Shika" } },
            { 202, new[] { "202xxx", "202xxx", "Leon" } },
            { 204, new[] { "204xxx", "204xxx", "Frederica Miyamoto" } },
            { 205, new[] { "205xxx", "205xxx", "Shiki Ichinose" } },
        };

        public Idol(int idolIDNumber)
        {
            IdolIdNumber = idolIDNumber;
        }

        public Idol(string idolNameID)
        {
            IdolIdNumber = int.Parse(idolNameID[0..3]);
        }

        public string IdolNameId => idols[IdolIdNumber][0];

        public string IdolAudioNameId => idols[IdolIdNumber][1];

        public int IdolIdNumber { get; }

        public string IdolNameFull => idols[IdolIdNumber][2];

        public string IdolFirstName => IdolNameFull.Split(' ')[0].Trim();

        public string IdolLastName => IdolNameFull.Split(' ')[1].Trim();
    }
}