namespace MirishitaMusicPlayer.Imas
{
    public class EventScenarioData
    {
        public double AbsTime { get; set; }

        public byte Selected { get; set; }

        public long Tick { get; set; }

        public int Measure { get; set; }

        public int Beat { get; set; }

        public int Track { get; set; }

        public ScenarioType Type { get; set; }

        public int Param { get; set; }

        public int Target { get; set; }

        public long Duration { get; set; }

        public double AbsEndTime { get; set; }

        public string Str { get; set; }

        public string Info { get; set; }

        public int On { get; set; }

        public int On2 { get; set; }

        public ColorRGBA Col { get; set; }

        public ColorRGBA Col2 { get; set; }

        public ColorRGBA Col3 { get; set; }

        public ColorRGBA[] Cols { get; set; }

        public int Trig { get; set; }

        public float Speed { get; set; }

        public int Idol { get; set; }

        public int CamNo { get; set; }

        public byte[] Mute { get; set; }

        public byte AddF { get; set; }

        public float EyeX { get; set; }

        public float EyeY { get; set; }

        public Vector4f[] Formation { get; set; }

        public byte Appeal { get; set; }

        public int Layer { get; set; }

        public int CheekLv { get; set; }

        public byte EyeClose { get; set; }

        public byte Talking { get; set; }

        public byte Delay { get; set; }

        public int[] ClRatio { get; set; }

        public int[] ClCols { get; set; }

        public int SeekFrame { get; set; }

        public int Idol2 { get; set; }

        public int Param2 { get; set; }

        public byte Bool1 { get; set; }
    }
}