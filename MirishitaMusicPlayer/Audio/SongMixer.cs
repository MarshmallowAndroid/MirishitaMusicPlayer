using System;
using System.Collections.Generic;
using MirishitaMusicPlayer.Imas;
using NAudio.Wave;

namespace MirishitaMusicPlayer.Audio
{
    class SongMixer : ISampleProvider, IDisposable
    {
        private readonly AcbWaveStream backgroundWaveStream;
        private readonly ISampleProvider backgroundSampleProvider;

        private readonly List<VoiceTrack> voiceSampleProviders;
        private readonly VoiceTrack backgroundEx;

        private readonly int voiceChannelCount;

        private readonly int channelDivide;

        private bool disposing;

        public SongMixer(
            List<EventScenarioData> muteScenarios,
            AcbWaveStream[] voiceAcbs,
            AcbWaveStream bgmAcb,
            AcbWaveStream bgmExAcb)
        {
            backgroundWaveStream = bgmAcb;
            backgroundSampleProvider = bgmAcb.ToSampleProvider();

            if (bgmExAcb != null)
                backgroundEx = new VoiceTrack(bgmExAcb, muteScenarios, 5, true);

            voiceSampleProviders = new();

            if (voiceAcbs != null)
            {
                int voiceIndex = 0;

                if (voiceAcbs.Length > 1)
                {
                    foreach (var voiceAcb in voiceAcbs)
                    {
                        if (voiceAcb != null)
                            voiceSampleProviders.Add(new VoiceTrack(voiceAcb, muteScenarios, voiceIndex++));
                    }
                }
                else
                {
                    if (voiceAcbs[0] != null)
                        voiceSampleProviders.Add(new VoiceTrack(voiceAcbs[0], muteScenarios, -1));
                }
            }

            // For converting mono voices to stereo

            if (voiceSampleProviders.Count > 0)
                voiceChannelCount = voiceSampleProviders[0].WaveFormat.Channels; // Get the channel count of the voices

            if (voiceChannelCount != 2)
                channelDivide = 2; // Read half the bytes (count / 2), one half for each stereo channel
            else
                channelDivide = 1; // Already stereo, do not divide
        }

        public bool HasEnded { get; private set; }

        public bool MuteVoices { get; set; }

        public bool MuteBackground { get; set; }

        public byte[] VoiceControl { get; set; }

        public TimeSpan CurrentTime => backgroundWaveStream.CurrentTime;

        public WaveFormat WaveFormat => backgroundSampleProvider.WaveFormat;

        public void Reset()
        {
            backgroundWaveStream.Position = 0;

            backgroundEx.Reset();

            foreach (var voice in voiceSampleProviders)
            {
                voice.Reset();
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

            backgroundEx.Seek(seconds);

            foreach (var voice in voiceSampleProviders)
            {
                voice.Seek(seconds);
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (disposing) return 0;

            for (int i = 0; i < count; i++)
            {
                buffer[i] = 0;
            }

            float[] bufferMain = new float[count];
            float[] bufferEx = new float[count];

            // Mix voices
            for (int i = 0; i < voiceSampleProviders.Count; i++)
            {
                voiceSampleProviders[i].Read(bufferMain, offset, count / channelDivide);

                if (MuteVoices) continue;

                // Mono to stereo conversion
                int index = offset;
                for (int j = 0; j < count / channelDivide; j++)
                {
                    for (int channel = 0; channel < channelDivide; channel++)
                    {
                        buffer[index++] += bufferMain[j];
                    }
                }
            }


            // Adjust volume of voices
            //
            // We need the number of real voices enabled, for cases where
            // the VoiceControl array is less than sample provider count
            //
            // Necessary for proper voice volume adjustment
            float multiplier = ((float)1 / (GetEnabledVoiceCount() + 1)) + 0.10f;
            for (int i = 0; i < count; i++)
            {
                buffer[i] *= multiplier;
            }

            int finalRead;

            // Read from background music
            int backgroundRead = backgroundSampleProvider.Read(bufferMain, offset, count);

            // If available, read from extra (secondary) background music
            int backgroundExRead = 0;
            if (backgroundEx != null)
            {
                backgroundExRead = backgroundEx.Read(bufferEx, offset, count);
            }

            float[] sourceBuffer;

            // If the extra background music is specified and voice control is available and has 6 voices,
            // use the extra background music instead of the original background music if the 6th voice
            // is active.
            //
            // The 6th voice or extra background music is just a pre-mixed version of all the idols plus
            // the background music.
            if (backgroundEx != null && backgroundEx.Singing)
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

            backgroundEx.Dispose();

            foreach (var voice in voiceSampleProviders)
            {
                voice.Dispose();
            }
        }

        private int GetEnabledVoiceCount()
        {
            if (voiceSampleProviders == null) return 0;

            int voicesEnabled = 0;
            for (int i = 0; i < voiceSampleProviders.Count; i++)
            {
                if (voiceSampleProviders[i].Singing) voicesEnabled++;
            }

            return Math.Min(voicesEnabled, voiceSampleProviders.Count);
        }
    }
}
