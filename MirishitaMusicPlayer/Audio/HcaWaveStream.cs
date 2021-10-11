using System.IO;
using NAudio.Wave;
using ClHcaSharp;

namespace MirishitaMusicPlayer.Audio
{
    public class HcaWaveStream : WaveStream
    {
        private readonly Stream hcaFileStream;
        private readonly BinaryReader hcaFileReader;
        private readonly HcaDecoder decoder;
        private readonly HcaInfo info;
        private readonly long dataStart;
        private readonly object positionLock = new();

        private readonly short[][] sampleBuffer;

        private int currentBlock;
        private long samplePosition;

        public HcaWaveStream(Stream hcaFile, ulong key)
        {
            hcaFileStream = hcaFile;
            hcaFileReader = new(hcaFile);
            decoder = new(hcaFile, key);
            info = decoder.GetInfo();
            dataStart = hcaFile.Position;

            sampleBuffer = new short[info.ChannelCount][];
            for (int i = 0; i < info.ChannelCount; i++)
            {
                sampleBuffer[i] = new short[info.SamplesPerBlock];
            }

            Loop = info.LoopEnabled;
            WaveFormat = new WaveFormat(info.SamplingRate, info.ChannelCount);
        }

        public HcaInfo Info => info;

        public bool Loop { get; set; }

        public override WaveFormat WaveFormat { get; }

        public override long Length => info.SampleCount * info.ChannelCount * sizeof(short);

        public override long Position
        {
            get
            {
                lock (positionLock)
                {
                    return (samplePosition - info.EncoderDelay) * info.ChannelCount * sizeof(short);
                }
            }
            set
            {
                lock (positionLock)
                {
                    samplePosition = (value + info.EncoderDelay) / info.ChannelCount / sizeof(short);

                    currentBlock = (int)(samplePosition / info.SamplesPerBlock);
                    hcaFileStream.Position = dataStart + currentBlock * info.BlockSize;
                    FillBuffer();
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (positionLock)
            {
                int read = 0;

                for (int i = 0; i < count / info.ChannelCount / sizeof(short); i++)
                {
                    if (Position >= Length - info.EncoderPadding) break;
                    if (samplePosition % info.SamplesPerBlock == 0) FillBuffer();

                    for (int j = 0; j < info.ChannelCount; j++)
                    {
                        int bufferOffset = (i * info.ChannelCount + j) * sizeof(short);
                        buffer[offset + bufferOffset] = (byte)sampleBuffer[j][samplePosition % info.SamplesPerBlock];
                        buffer[offset + bufferOffset + 1] = (byte)(sampleBuffer[j][samplePosition % info.SamplesPerBlock] >> 8);

                        read += sizeof(short);
                    }

                    samplePosition++;

                    if (samplePosition >= info.LoopEndSample + info.EncoderDelay && Loop)
                    {
                        hcaFileStream.Position = dataStart + info.LoopStartBlock * info.BlockSize;
                        FillBuffer();

                        samplePosition = info.LoopStartSample + info.EncoderDelay;
                    }
                }

                return read;
            }
        }

        private void FillBuffer()
        {
            byte[] blockBytes = hcaFileReader.ReadBytes(info.BlockSize);
            if (blockBytes.Length > 0)
            {
                decoder.DecodeBlock(blockBytes);
                decoder.ReadSamples16(sampleBuffer);
            }
            else
            {
                for (int i = 0; i < sampleBuffer.Length; i++)
                {
                    for (int j = 0; j < sampleBuffer[i].Length; j++)
                    {
                        sampleBuffer[i][j] = 0;
                    }
                }
            }
        }
    }
}
