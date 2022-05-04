using System.Collections.Generic;

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

        public string Str { get; set; }

        public string Info { get; set; }

        public int On { get; set; }

        public int On2 { get; set; }

        public object Col { get; set; }

        public object Col2 { get; set; }

        public object Col3 { get; set; }

        public object[] Cols { get; set; }

        public int Trig { get; set; }

        public float Speed { get; set; }

        public int Idol { get; set; }

        public int CamNo { get; set; }

        public byte[] Mute { get; set; }

        public byte AddF { get; set; }

        public float EyeX { get; set; }

        public float EyeY { get; set; }

        public object[] Formation { get; set; }

        public byte Appeal { get; set; }

        public int Layer { get; set; }

        public int CheekLv { get; set; }

        public byte EyeClose { get; set; }

        public byte Talking { get; set; }

        public byte Delay { get; set; }

        public int SeekFrame { get; set; }

        public int Idol2 { get; set; }

        public int Param2 { get; set; }

        public byte Bool1 { get; set; }
    }
}