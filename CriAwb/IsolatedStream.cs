using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CriAwb
{
    public class IsolatedStream : Stream
    {
        Stream sourceStream;
        long realPosition;
        long internalPosition;

        public IsolatedStream(Stream sourceStream, long offset, long length)
        {
            this.sourceStream = sourceStream;
            realPosition = offset;
            internalPosition = 0;

            Length = length;
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length { get; }

        public override long Position
        {
            get => internalPosition;
            set
            {
                long checkValue = value + realPosition;
                if (value < 0 || value >= Length) throw new ArgumentOutOfRangeException();
                internalPosition = value;
                sourceStream.Position = checkValue;
            }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            long restore = sourceStream.Position;
            sourceStream.Position = realPosition + internalPosition;
            int read = sourceStream.Read(buffer, offset, count);
            internalPosition += read;
            sourceStream.Position = restore;
            return read;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    internalPosition = 0;
                    internalPosition += offset;
                    break;
                case SeekOrigin.Current:
                    if (offset >= Length) throw new ArgumentOutOfRangeException();
                    internalPosition += offset;
                    break;
                case SeekOrigin.End:
                    internalPosition = Length;
                    if (realPosition - offset < realPosition) throw new ArgumentOutOfRangeException();
                    internalPosition -= offset;
                    break;
                default:
                    break;
            }

            return internalPosition;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}
