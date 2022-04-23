namespace MirishitaMusicPlayer.Imas
{
    internal class EventNoteData
    {
        public double AbsTime { get; set; }

        public long Tick { get; set; }

        public int Track { get; set; }

        public int Type { get; set; }

        public float StartPosX { get; set; }

        public float EndPosX { get; set; }
    }
}