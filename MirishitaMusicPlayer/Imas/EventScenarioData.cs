namespace MirishitaMusicPlayer.Imas
{
    class EventScenarioData
    {
        public double AbsTime { get; set; }

        public long Tick { get; set; }

        public int Track { get; set; }

        public ScenarioType Type { get; set; }

        public int Param { get; set; }

        public int Target { get; set; }

        public string Str { get; set; }

        public int Idol { get; set; }

        public int CamNo { get; set; }

        public byte[] Mute { get; set; }

        public byte EyeClose { get; set; }

        public int SeekFrame { get; set; }

        public int Idol2 { get; set; }
    }
}

