using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Collections.Generic;

namespace MirishitaMusicPlayer.Audio
{
    internal class CachedSound
    {
        public CachedSound(string soundFileName)
        {
            WaveFileReader reader = new(soundFileName);
            MediaFoundationResampler resampler = new MediaFoundationResampler(reader, new WaveFormat(44100, 2));

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

        public CachedSound(HcaWaveStream hcaWaveStream)
        {
        }

        public WaveFormat WaveFormat { get; }

        public float[] AudioData { get; }
    }
}