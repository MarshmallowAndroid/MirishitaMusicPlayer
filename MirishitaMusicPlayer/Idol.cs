namespace MirishitaMusicPlayer
{
    internal class Idol
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