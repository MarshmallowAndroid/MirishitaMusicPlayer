namespace MirishitaMusicPlayer.Imas
{
    internal class EventConductorData
    {
        public double AbsTime { get; set; }

        public long Tick { get; set; }

        public double Tempo { get; set; }

        public int TSigNumerator { get; set; }

        public int TSigDenominator { get; set; }

        public double TicksPerSecond
        {
            get
            {
                return Tempo * (TSigNumerator + TSigDenominator);
            }
        }
    }
}