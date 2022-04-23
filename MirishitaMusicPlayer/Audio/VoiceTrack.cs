using MirishitaMusicPlayer.Imas;
using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace MirishitaMusicPlayer.Audio
{
    internal class VoiceTrack : ISampleProvider, IDisposable
    {
        private readonly WaveStream waveStream;
        private readonly ISampleProvider sampleProvider;
        private readonly List<(long Sample, bool Singing)> triggers = new();
        private readonly bool alwaysSing = false;

        private int nextTriggerIndex = 0;
        private long currentSample = 0;

        public VoiceTrack(AcbWaveStream voiceAcb, List<EventScenarioData> muteScenarios, int voiceIndex, bool forceSinging = false)
        {
            waveStream = voiceAcb;
            sampleProvider = waveStream.ToSampleProvider();
            alwaysSing = forceSinging;

            if (voiceIndex >= 0)
            {
                foreach (var item in muteScenarios)
                {
                    triggers.Add((AbsTimeToSamples(item.AbsTime), item.Mute[voiceIndex] == 1));
                }
            }
            else triggers.Add((0, true));
        }

        public WaveFormat WaveFormat => waveStream.WaveFormat;

        public bool Singing { get; private set; } = false;

        public void Seek(float seconds)
        {
            waveStream.Position += (long)(seconds *
                (waveStream.WaveFormat.BitsPerSample / 8 *
                waveStream.WaveFormat.SampleRate *
                waveStream.WaveFormat.Channels));
            currentSample = waveStream.Position / WaveFormat.Channels / (WaveFormat.BitsPerSample / 8);
            nextTriggerIndex = 0;
        }

        public void Reset()
        {
            waveStream.Position = 0;
            currentSample = 0;
            nextTriggerIndex = 0;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = sampleProvider.Read(buffer, offset, count);
            for (int i = 0; i < samplesRead / WaveFormat.Channels; i++)
            {
                while (currentSample >= triggers[nextTriggerIndex].Sample)
                {
                    Singing = triggers[nextTriggerIndex].Singing;

                    if (nextTriggerIndex < triggers.Count - 1) nextTriggerIndex++;
                    else break;
                }

                for (int j = 0; j < WaveFormat.Channels; j++)
                {
                    buffer[i * WaveFormat.Channels + j] *= alwaysSing ? 1 : Singing ? 1 : 0;
                }

                currentSample++;
            }

            return samplesRead;
        }

        private long AbsTimeToSamples(double absTime)
        {
            return (long)(absTime * WaveFormat.SampleRate);
        }

        public void Dispose()
        {
            waveStream.Dispose();
        }
    }
}
