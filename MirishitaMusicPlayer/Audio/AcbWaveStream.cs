using System.IO;
using CriAcb;
using NAudio.Wave;
using CriAwb;
using System.Linq;
using System.Collections.Generic;

namespace MirishitaMusicPlayer.Audio
{
    class AcbWaveStream : WaveStream
    {
        private AcbReader acbReader;
        private AwbReader awbReader;
        private WaveStream waveStream;

        public AcbWaveStream(string path) : this(File.OpenRead(path)) { }

        public AcbWaveStream(Stream stream)
        {
            acbReader = new(stream);
            awbReader = acbReader.GetAwb();
            waveStream = new HcaWaveStream(awbReader.GetWaveSubfileStream(awbReader.Waves[0]), 765765765765765);
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
    }
}
