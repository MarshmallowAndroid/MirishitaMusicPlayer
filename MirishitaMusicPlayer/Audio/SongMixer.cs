using MirishitaMusicPlayer.Imas;
using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace MirishitaMusicPlayer.Audio
{
    public class SongMixer : ISampleProvider, IDisposable
    {
        private readonly AcbWaveStream backgroundWaveStream;
        private readonly AcbWaveStream backgroundExWaveStream;
        private readonly ISampleProvider backgroundSampleProvider;
        private readonly ISampleProvider backgroundExSampleProvider;

        private readonly List<VoiceTrack> voiceSampleProviders;

        private readonly List<(long Sample, int ActiveSingers)> volumeTriggers = new();
        private readonly List<(long Sample, bool Active)> exTriggers = new();

        private readonly int voiceChannelCount;
        private readonly int channelDivide;

        private long currentVolumeSample = 0;
        private long currentExSample = 0;
        private int nextVolumeTriggerIndex = 0;
        private int nextExTriggerIndex = 0;

        private float multiplier = 1.0f;
        private bool exActive = false;

        private bool disposing;

        public SongMixer(
            List<EventScenarioData> muteScenarios,
            AcbWaveStream[] voiceAcbs,
            AcbWaveStream bgmAcb,
            AcbWaveStream bgmExAcb)
        {
            backgroundWaveStream = bgmAcb;
            backgroundSampleProvider = bgmAcb.ToSampleProvider();

            // Add the extra BGM, if any
            if (bgmExAcb != null)
            {
                backgroundExWaveStream = bgmExAcb;
                backgroundExSampleProvider = backgroundExWaveStream.ToSampleProvider();
            }

            // Add our voice SampleProviders
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
                else if (voiceAcbs.Length == 1)
                {
                    if (voiceAcbs[0] != null)
                        voiceSampleProviders.Add(new VoiceTrack(voiceAcbs[0], muteScenarios, -1));
                }
            }

            // Convert our mute scenarios into triggers
            foreach (var muteScenario in muteScenarios)
            {
                volumeTriggers.Add((AbsTimeToSamples(muteScenario.AbsTime), GetActiveSingers(muteScenario.Mute)));

                if (backgroundExSampleProvider != null)
                {
                    exTriggers.Add((AbsTimeToSamples(muteScenario.AbsTime), muteScenario.Mute[^1] == 1));
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

        public int VoiceCount => voiceSampleProviders.Count;

        public TimeSpan CurrentTime => backgroundWaveStream.CurrentTime;

        public TimeSpan TotalTime => backgroundWaveStream.TotalTime;

        public WaveFormat WaveFormat => backgroundSampleProvider.WaveFormat;

        public long Position
        {
            get
            {
                return backgroundWaveStream.Position;
            }
            set
            {
                backgroundWaveStream.Position = value;

                if (backgroundExWaveStream != null)
                    backgroundExWaveStream.Position = backgroundWaveStream.Position;

                foreach (var voice in voiceSampleProviders)
                {
                    voice.Position = backgroundWaveStream.Position / 2;
                }

                currentVolumeSample = backgroundWaveStream.Position /
                    backgroundWaveStream.WaveFormat.Channels /
                    (backgroundWaveStream.WaveFormat.BitsPerSample / 8);
                currentExSample = backgroundWaveStream.Position /
                    backgroundWaveStream.WaveFormat.Channels /
                    (backgroundWaveStream.WaveFormat.BitsPerSample / 8);
                nextVolumeTriggerIndex = 0;
                nextExTriggerIndex = 0;
            }
        }

        public long Length => backgroundWaveStream.Length;

        public int Read(float[] buffer, int offset, int count)
        {
            if (disposing) return 0;

            // Clear buffer
            for (int i = 0; i < count; i++)
            {
                buffer[i] = 0;
            }

            float[] voiceBuffer = new float[count / channelDivide];
            float[] bufferMain = new float[count];

            long sampleTemp = currentVolumeSample;

            // Read and mix voices
            for (int i = 0; i < voiceSampleProviders.Count; i++)
            {
                voiceSampleProviders[i].Read(voiceBuffer, offset, count / channelDivide);

                if (MuteVoices) continue;

                currentVolumeSample = sampleTemp;

                // Mono to stereo conversion
                int index = offset;
                for (int j = 0; j < count / channelDivide; j++)
                {
                    while (currentVolumeSample >= volumeTriggers[nextVolumeTriggerIndex].Sample)
                    {
                        if (volumeTriggers[nextVolumeTriggerIndex].ActiveSingers == 1)
                            multiplier = (1.0f / 2.0f) + 0.15f;
                        else
                            multiplier = 1.0f / (volumeTriggers[nextVolumeTriggerIndex].ActiveSingers + 1) + 0.15f;

                        if (nextVolumeTriggerIndex < volumeTriggers.Count - 1) nextVolumeTriggerIndex++;
                        else break;
                    }

                    for (int channel = 0; channel < channelDivide; channel++)
                    {
                        buffer[index++] += voiceBuffer[j] * multiplier;
                    }

                    currentVolumeSample++;
                }
            }

            // Read from background music
            int backgroundRead = backgroundSampleProvider.Read(bufferMain, offset, count);

            // If available, read from extra (secondary) background music
            float[] bufferEx = new float[count];
            int backgroundExRead = 0;
            if (backgroundExSampleProvider != null)
            {
                backgroundExRead = backgroundExSampleProvider.Read(bufferEx, offset, count);
            }

            // If the extra background music is specified and voice control is available and has 6 voices,
            // use the extra background music instead of the original background music if the 6th voice
            // is active.
            //
            // The 6th voice or extra background music is just a pre-mixed version of all the idols plus
            // the background music.
            for (int i = offset; i < count / backgroundWaveStream.WaveFormat.Channels; i++)
            {
                if (MuteBackground) break;

                if (backgroundExWaveStream != null)
                {
                    while (currentExSample >= exTriggers[nextExTriggerIndex].Sample)
                    {
                        exActive = exTriggers[nextExTriggerIndex].Active;

                        if (nextExTriggerIndex < exTriggers.Count - 1) nextExTriggerIndex++;
                        else break;
                    }
                }

                for (int j = 0; j < WaveFormat.Channels; j++)
                {
                    int index = i * WaveFormat.Channels + j;
                    buffer[index] += exActive ? bufferEx[index] : bufferMain[index];
                }

                currentExSample++;
            }

            int finalRead = exActive ? backgroundExRead : backgroundRead;

            // Set our end flag to true
            if (finalRead != count) HasEnded = true;

            // Return bytes read
            return finalRead;
        }

        public void Dispose()
        {
            disposing = true;

            backgroundWaveStream.Dispose();

            if (backgroundExWaveStream != null)
                backgroundExWaveStream.Dispose();

            foreach (var voice in voiceSampleProviders)
            {
                if (voice != null)
                    voice.Dispose();
            }

            HasEnded = true;
        }

        private long AbsTimeToSamples(double absTime)
        {
            return (long)(absTime * WaveFormat.SampleRate);
        }

        private int GetActiveSingers(byte[] muteArray)
        {
            int activeSingers = 0;
            for (int i = 0; i < muteArray.Length; i++)
            {
                if (muteArray[i] > 0) activeSingers++;
            }
            return Math.Min(activeSingers, voiceSampleProviders.Count);
        }
    }
}