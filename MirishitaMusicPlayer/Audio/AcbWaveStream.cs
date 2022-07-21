using CriWareFormats;
using NAudio.Wave;
using System.IO;
using System.Linq;

namespace MirishitaMusicPlayer.Audio
{
    public class AcbWaveStream : WaveStream
    {
        private readonly Stream sourceStream;
        private readonly AcbReader acbReader;
        private readonly AwbReader awbReader;
        private readonly WaveStream waveStream;

        public AcbWaveStream(string path) : this(File.OpenRead(path))
        {
        }

        public AcbWaveStream(Stream stream)
        {
            sourceStream = stream;
            acbReader = new(stream);
            awbReader = acbReader.GetAwb();
            waveStream = new HcaWaveStream(
                awbReader.GetWaveSubfileStream(
                    awbReader.Waves.Count > 2 ? awbReader.Waves[^1] : awbReader.Waves[0]),
                    765765765765765);
        }

        public AcbWaveStream(Stream stream, string waveName)
        {
            acbReader = new(stream);
            awbReader = acbReader.GetAwb();
            waveStream = new HcaWaveStream(
                awbReader.GetWaveSubfileStream(
                    awbReader.Waves.FirstOrDefault()), 765765765765765);
        }

        public override WaveFormat WaveFormat => waveStream.WaveFormat;

        public override long Length => waveStream.Length;

        public override long Position { get => waveStream.Position; set => waveStream.Position = value; }

        public override int Read(byte[] buffer, int offset, int count) =>
            waveStream.Read(buffer, offset, count);

        protected override void Dispose(bool disposing)
        {
            sourceStream.Dispose();
            base.Dispose(disposing);
        }
    }
}