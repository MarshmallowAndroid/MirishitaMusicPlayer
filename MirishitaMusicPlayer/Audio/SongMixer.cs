using System;
using System.Collections.Generic;
using NAudio.Wave;

namespace MirishitaMusicPlayer.Audio
{
    class SongMixer : ISampleProvider
    {
        private readonly AcbWaveStream backgroundWaveStream;
        private readonly ISampleProvider backgroundSampleProvider;
        private readonly AcbWaveStream backgroundExWaveStream;
        private readonly ISampleProvider backgroundExSampleProvider;

        private readonly IEnumerable<AcbWaveStream> voiceWaveStreams;
        private readonly List<ISampleProvider> voiceSampleProviders;
        private readonly int voiceChannelCount;

        private readonly int channelDivide;

        public SongMixer(IEnumerable<AcbWaveStream> voiceAcbs, AcbWaveStream bgmAcb, AcbWaveStream bgmExAcb)
        {
            backgroundWaveStream = bgmAcb;
            backgroundSampleProvider = bgmAcb.ToSampleProvider();

            if (bgmExAcb != null)
            {
                backgroundExWaveStream = bgmExAcb;
                backgroundExSampleProvider = bgmExAcb.ToSampleProvider();
            }

            voiceWaveStreams = voiceAcbs;
            voiceSampleProviders = new();

            if (voiceAcbs != null)
            {
                foreach (var voiceAcb in voiceAcbs)
                {
                    if (voiceAcb != null)
                        voiceSampleProviders.Add(voiceAcb.ToSampleProvider());
                }
            }

            if (voiceSampleProviders.Count > 0)
                voiceChannelCount = voiceSampleProviders[0].WaveFormat.Channels;

            if (voiceChannelCount != 2)
                channelDivide = 2;
            else
                channelDivide = 1;
        }

        public bool HasEnded { get; private set; }

        public byte[] VoiceControl { get; set; }

        public TimeSpan CurrentTime => backgroundWaveStream.CurrentTime;

        public WaveFormat WaveFormat => backgroundSampleProvider.WaveFormat;

        public void Reset()
        {
            backgroundWaveStream.Position = 0;

            if (backgroundExWaveStream != null)
                backgroundExWaveStream.Position = 0;

            foreach (var voiceWaveStream in voiceWaveStreams)
            {
                voiceWaveStream.Position = 0;
            }
        }

        public void Seek(float seconds)
        {
            backgroundWaveStream.Position += (long)(seconds *
                (backgroundWaveStream.WaveFormat.BitsPerSample / 8 *
                backgroundWaveStream.WaveFormat.SampleRate *
                backgroundWaveStream.WaveFormat.Channels));

            if (backgroundExWaveStream != null)
                backgroundExWaveStream.Position += (long)(seconds *
                (backgroundExWaveStream.WaveFormat.BitsPerSample / 8 *
                backgroundExWaveStream.WaveFormat.SampleRate *
                backgroundExWaveStream.WaveFormat.Channels));

            foreach (var voiceWaveStream in voiceWaveStreams)
            {
                voiceWaveStream.Position += (long)(seconds *
                    (voiceWaveStream.WaveFormat.BitsPerSample / 8 *
                    voiceWaveStream.WaveFormat.SampleRate *
                    voiceWaveStream.WaveFormat.Channels));
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            float[] buffer2 = new float[count];
            float[] bufferEx = new float[count];

            int voicesEnabled = 0; // Number of real voices enabled,
                                   // for cases where VoiceControl array is less than sample provider count

            // Mix voices
            for (int i = 0; i < voiceSampleProviders.Count; i++)
            {
                voiceSampleProviders[i].Read(buffer2, offset, count / channelDivide);

                // Play the corresponding voice depending on if the idol at the specified
                // index is active, or just play the entire thing if solo (one voice only)
                if ((VoiceControl != null && VoiceControl[i] == 1) || (voiceSampleProviders.Count == 1))
                {
                    int index = offset;

                    for (int j = 0; j < count / channelDivide; j++)
                    {
                        for (int channel = 0; channel < channelDivide; channel++)
                        {
                            buffer[index++] += buffer2[j];
                        }
                    }

                    voicesEnabled++;
                }
            }

            // Adjust volume of voices
            float multiplier = ((float)1 / (voicesEnabled + 1)) + 0.10f;
            for (int i = 0; i < count; i++)
            {
                buffer[i] *= multiplier;
            }

            int read = backgroundSampleProvider.Read(buffer2, offset, count);

            if (backgroundExSampleProvider != null)
            {
                backgroundExSampleProvider.Read(bufferEx, offset, count);
            }

            float[] sourceBuffer;

            if (VoiceControl != null && VoiceControl.Length == 6 && VoiceControl[5] == 1 && backgroundExSampleProvider != null)
                sourceBuffer = bufferEx;
            else
                sourceBuffer = buffer2;

            for (int i = 0; i < count; i++)
            {
                buffer[i] += sourceBuffer[i];
            }

            if (read != count) HasEnded = true;

            return read;
        }
    }
}
