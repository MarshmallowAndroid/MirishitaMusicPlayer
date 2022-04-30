using NAudio.Wave;
using System;

namespace MirishitaMusicPlayer.Audio
{
    internal class CachedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSound cachedSound;
        private long position;

        public CachedSoundSampleProvider(CachedSound source)
        {
            cachedSound = source;
        }

        public WaveFormat WaveFormat => cachedSound.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            long availableSamples = cachedSound.AudioData.Length - position;
            int toCopy = (int)Math.Min(availableSamples, count);
            Array.Copy(cachedSound.AudioData, position, buffer, offset, toCopy);
            position += toCopy;
            return toCopy;
        }
    }
}