using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace MirishitaMusicPlayer.Audio
{
    class RhythmPlayer : ISampleProvider
    {
        private readonly CachedSound tapSound;
        private readonly CachedSound flickSound;

        private readonly MixingSampleProvider mixer;

        public RhythmPlayer()
        {

        }

        public RhythmPlayer(string tapPath, string flickPath)
        {
            tapSound = new(tapPath);
            flickSound = new(flickPath);

            mixer = new MixingSampleProvider(tapSound.WaveFormat)
            {
                ReadFully = true
            };
        }

        public void Tap()
        {
            mixer.AddMixerInput(new CachedSoundSampleProvider(tapSound));
        }

        public void Flick()
        {
            mixer.AddMixerInput(new CachedSoundSampleProvider(flickSound));
        }



        public void Stop()
        {
            mixer.ReadFully = false;
        }

        public WaveFormat WaveFormat => mixer.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            int read = mixer.Read(buffer, offset, count);

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] *= 0.15f;
            }

            return read;
        }
    }
}

