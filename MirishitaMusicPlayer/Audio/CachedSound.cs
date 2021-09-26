using System.Collections.Generic;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace MirishitaMusicPlayer.Audio
{
    class CachedSound
    {
        public CachedSound(string soundFileName)
        {
            WaveFileReader reader = new(soundFileName);
            ResamplerDmoStream resampler = new ResamplerDmoStream(reader, new WaveFormat(44100, 2));

            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
            ISampleProvider sampleProvider = new SampleChannel(resampler);

            var allData = new List<float>((int)(reader.Length / 4));
            var buffer = new float[resampler.WaveFormat.SampleRate * resampler.WaveFormat.Channels];

            while (sampleProvider.Read(buffer, 0, buffer.Length) > 0)
            {
                allData.AddRange(buffer);
            }

            AudioData = allData.ToArray();
        }

        public WaveFormat WaveFormat { get; }

        public float[] AudioData { get; }
    }
}

