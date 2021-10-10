using System;
using System.Collections.Generic;
using NAudio.Wave;

namespace MirishitaMusicPlayer.Audio
{
    class SongMixer : ISampleProvider, IDisposable
    {
        private readonly AcbWaveStream backgroundWaveStream;
        private readonly ISampleProvider backgroundSampleProvider;
        private readonly AcbWaveStream backgroundExWaveStream;
        private readonly ISampleProvider backgroundExSampleProvider;

        private readonly IEnumerable<AcbWaveStream> voiceWaveStreams;
        private readonly List<ISampleProvider> voiceSampleProviders;
        private readonly int voiceChannelCount;

        private readonly int channelDivide;

        private bool disposing;

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

        public bool MuteBackground { get; set; }

        public byte[] VoiceControl { get; set; }

        public TimeSpan CurrentTime => backgroundWaveStream.CurrentTime;

        public WaveFormat WaveFormat => backgroundSampleProvider.WaveFormat;

        public void Reset()
        {
            backgroundWaveStream.Position = 0;

            if (backgroundExWaveStream != null)
                backgroundExWaveStream.Position = 0;

            if (voiceWaveStreams != null)
                foreach (var voiceWaveStream in voiceWaveStreams)
                {
                    voiceWaveStream.Position = 0;
                }
        }

        public void Seek(float seconds)
        {
            // TODO: Simplify

            if (backgroundWaveStream.CurrentTime.TotalSeconds + seconds < 0)
            {
                Reset();
                return;
            }

            backgroundWaveStream.Position += (long)(seconds *
                (backgroundWaveStream.WaveFormat.BitsPerSample / 8 *
                backgroundWaveStream.WaveFormat.SampleRate *
                backgroundWaveStream.WaveFormat.Channels));

            if (backgroundExWaveStream != null)
                backgroundExWaveStream.Position += (long)(seconds *
                (backgroundExWaveStream.WaveFormat.BitsPerSample / 8 *
                backgroundExWaveStream.WaveFormat.SampleRate *
                backgroundExWaveStream.WaveFormat.Channels));

            if (voiceWaveStreams != null)
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
            if (disposing) return 0;

            float[] bufferMain = new float[count];
            float[] bufferEx = new float[count];

            int voicesEnabled = 0; // Number of real voices enabled,
                                   // for cases where VoiceControl array is less than sample provider count

            // Mix voices
            for (int i = 0; i < voiceSampleProviders.Count; i++)
            {
                voiceSampleProviders[i].Read(bufferMain, offset, count / channelDivide);

                // Play the corresponding voice depending on if the idol at the specified
                // index is active, or just play the entire thing if solo (one voice only)
                if ((VoiceControl != null && VoiceControl[i] == 1) || (voiceSampleProviders.Count == 1))
                {
                    int index = offset;

                    for (int j = 0; j < count / channelDivide; j++)
                    {
                        for (int channel = 0; channel < channelDivide; channel++)
                        {
                            buffer[index++] += bufferMain[j];
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

            int finalRead;

            // Read from background music
            int backgroundRead = backgroundSampleProvider.Read(bufferMain, offset, count);

            // If available, read from extra (secondary) background music
            int backgroundExRead = 0;
            if (backgroundExSampleProvider != null)
            {
                backgroundExRead = backgroundExSampleProvider.Read(bufferEx, offset, count);
            }

            float[] sourceBuffer;

            // If the extra background music is specified and voice control is available and has 6 voices,
            // use the extra background music instead of the original background music if the 6th voice
            // is active.
            //
            // The 6th voice or extra background music is just a pre-mixed version of all the idols plus
            // the background music.
            if (VoiceControl != null && VoiceControl.Length == 6 && VoiceControl[5] == 1 && backgroundExSampleProvider != null)
            {
                sourceBuffer = bufferEx;
                finalRead = backgroundExRead;
            }
            else
            {
                sourceBuffer = bufferMain;
                finalRead = backgroundRead;
            }

            // Plop the data into the output buffer
            for (int i = 0; i < count; i++)
            {
                if (MuteBackground) break;
                buffer[i] += sourceBuffer[i];
            }

            // Set our end flag to true
            if (finalRead != count) HasEnded = true;

            // Return bytes read
            return finalRead;
        }

        public void Dispose()
        {
            disposing = true;

            backgroundWaveStream.Dispose();
            foreach (var waveStream in voiceWaveStreams)
                waveStream.Dispose();
            if (backgroundExWaveStream != null)
                backgroundExWaveStream.Dispose();
        }
    }
}
