using System;
using System.IO;
using System.Runtime.InteropServices;

namespace LevelDBMCPE
{
    public class UnsafeMemoryStream : Stream
    {
        public IntPtr BaseAddress { get; private set; }

        public UnsafeMemoryStream(IntPtr address)
        {
            BaseAddress = address;
        }

        public override bool CanRead => BaseAddress != IntPtr.Zero;

        public override bool CanSeek => BaseAddress != IntPtr.Zero;

        public override bool CanWrite => BaseAddress != IntPtr.Zero;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => _position;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _position = value;
            }
        }
        private long _position = 0;

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Marshal.Copy(new IntPtr(BaseAddress.ToInt64() + Position), buffer, offset, count);
            Position += count;
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.Current:
                    Position += offset;
                    break;
                case SeekOrigin.End:
                    throw new NotSupportedException();
            }
            return Position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Marshal.Copy(buffer, offset, new IntPtr(BaseAddress.ToInt64() + Position), count);
            Position += count;
        }
    }
}
