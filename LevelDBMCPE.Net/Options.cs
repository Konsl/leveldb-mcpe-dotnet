using System;

namespace LevelDBMCPE
{
    public class Options : NativeObject
    {
        protected Options(IntPtr handle) : base(handle) { }

        public static Options Create()
        {
            return new Options(Library.LevelDBOptionsCreate());
        }

        protected override void InternalClose()
        {
            EnsureNotDisposed();
            Library.LevelDBOptionsDestroy(NativeHandle);
        }

        public Comparator Comparator
        {
            set
            {
                EnsureNotDisposed();
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                value.EnsureNotDisposed();
                Library.LevelDBOptionsSetComparator(NativeHandle, value);
            }
        }

        public FilterPolicy FilterPolicy
        {
            set
            {
                EnsureNotDisposed();
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                value.EnsureNotDisposed();
                Library.LevelDBOptionsSetFilterPolicy(NativeHandle, value);
            }
        }

        public bool CreateIfMissing
        {
            set
            {
                EnsureNotDisposed();
                Library.LevelDBOptionsSetCreateIfMissing(NativeHandle, value);
            }
        }

        public bool ErrorIfExists
        {
            set
            {
                EnsureNotDisposed();
                Library.LevelDBOptionsSetErrorIfExists(NativeHandle, value);
            }
        }

        public bool ParanoidChecks
        {
            set
            {
                EnsureNotDisposed();
                Library.LevelDBOptionsSetParanoidChecks(NativeHandle, value);
            }
        }

        public Env Env
        {
            set
            {
                EnsureNotDisposed();
                if(value is null)
                    throw new ArgumentNullException(nameof(value));
                value.EnsureNotDisposed();
                Library.LevelDBOptionsSetEnv(NativeHandle, value);
            }
        }

        public Logger InfoLog
        {
            set
            {
                EnsureNotDisposed();
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                value.EnsureNotDisposed();
                Library.LevelDBOptionsSetInfoLog(NativeHandle, value);
            }
        }

        public ulong WriteBufferSize
        {
            set
            {
                EnsureNotDisposed();
                Library.LevelDBOptionsSetWriteBufferSize(NativeHandle, value);
            }
        }

        public int MaxOpenFiles
        {
            set
            {
                EnsureNotDisposed();
                Library.LevelDBOptionsSetMaxOpenFiles(NativeHandle, value);
            }
        }

        public Cache Cache
        {
            set
            {
                EnsureNotDisposed();
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                value.EnsureNotDisposed();
                Library.LevelDBOptionsSetCache(NativeHandle, value);
            }
        }

        public ulong BlockSize
        {
            set
            {
                EnsureNotDisposed();
                Library.LevelDBOptionsSetBlockSize(NativeHandle, value);
            }
        }

        public int BlockRestartInterval
        {
            set
            {
                EnsureNotDisposed();
                Library.LevelDBOptionsSetBlockRestartInterval(NativeHandle, value);
            }
        }

        public Compression Compression
        {
            set
            {
                EnsureNotDisposed();
                Library.LevelDBOptionsSetCompression(NativeHandle, (Library.LevelDBCompression)value);
            }
        }

        public void SetCompression(Compression value, int index = 0)
        {
            EnsureNotDisposed();
            Library.LevelDBOptionsSetCompressionByIndex(NativeHandle, (Library.LevelDBCompression)value, index);
        }
    }
}
